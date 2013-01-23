#region File Description
//-----------------------------------------------------------------------------
// Xna2dEditorControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Fyri2dEditor.Xna2dDrawingLibrary;
using Xna2dEditor;
using Draw;
#endregion

namespace Fyri2dEditor
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, and displays
    /// a spinning 3D model. The main form class is responsible for loading
    /// the model: this control just displays it.
    /// </summary>
    class Xna2dShapeControl : XnaToolUser
    {
        

        // Timer controls the rotation speed.
        Timer updateTimer;
        Stopwatch timer;
        SpriteBatch spriteBatch;
        GameTime gameTimer;
        GameTime previousGameTimer;

        List<RoundLine> blueRoundLines = new List<RoundLine>();
        List<RoundLine> greenRoundLines = new List<RoundLine>();

        Matrix viewMatrix;
        Matrix projMatrix;
        Matrix viewProjMatrix;
        //float cameraX = 0;
        //float cameraY = 0;
        //float cameraZoom = 300;
        XnaCamera2d camera;
        RenderTarget2D screen;

        float time;
        string curTechniqueName;

        Disc dude = new Disc(0, 0);
        bool aButtonDown = false;
        int roundLineTechniqueIndex = 0;
        string[] roundLineTechniqueNames;
        

        Vector2 mousePos = Vector2.Zero;

        List<RoundLine> majorLines = new List<RoundLine>();
        List<RoundLine> minorLines = new List<RoundLine>();

        public float Xorigin, Yorigin;
        public float ScaleX, ScaleY;
        private int _width, _height;
        public float Xdivs = 2, Ydivs = 2, MajorIntervals = 50;        

        #region Xna Properties

        private XnaDrawingBatch drawingBatch;
        public XnaDrawingBatch DrawingBatch
        {
            get { return drawingBatch; }
            set { drawingBatch = value; }
        }

        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public RoundLineManager RoundLineManager
        {
            get { return roundLineManager; }

            set
            {
                roundLineManager = value;
                if (roundLineManager != null)
                    roundLineTechniqueNames = roundLineManager.TechniqueNames;
            }
        }
        RoundLineManager roundLineManager;

        public XnaDrawingContext DrawingContext
        {
            get { return drawingContext; }

            set
            {
                XnaDrawingContext oldValue = drawingContext;
                drawingContext = value;
                //if (oldValue != drawingContext)
                //    OnContextChanged(new ContextEventArgs(oldValue, drawingContext));
                //if (lineBatch != null)
                //roundLineTechniqueNames = lineBatch.TechniqueNames;
            }
        }

        private XnaDrawingContext drawingContext;

        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public XnaLine2dBatch LineBatch
        {
            get { return lineBatch; }

            set
            {
                lineBatch = value;
                if (lineBatch != null)
                    roundLineTechniqueNames = lineBatch.TechniqueNames;
            }
        }

        XnaLine2dBatch lineBatch;

        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public SpriteFont SpriteFont
        {
            get { return font; }

            set
            {
                font = value;
            }
        }

        SpriteFont font;

        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public Effect Effect
        {
            get { return effect; }

            set
            {
                effect = value;
            }
        }

        Effect effect;

        private XnaTexture2dManager textureManager;
        public XnaTexture2dManager TextureManager
        {
            get { return textureManager; }
            set { textureManager = value; }
        }

        private FyriProject project;
        public FyriProject Project
        {
            get { return project; }
            set { project = value; }
        }

        #endregion

        FyriTexture2d cursorTexture;
        Texture2D blank;
        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Start the animation timer.
            timer = Stopwatch.StartNew();

            gameTimer = new GameTime();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            Mouse.WindowHandle = this.Handle;


            PresentationParameters pp = GraphicsDevice.PresentationParameters.Clone();
            pp.IsFullScreen = false;
            pp.DeviceWindowHandle = this.Handle;
            pp.BackBufferWidth = this.Width;
            pp.BackBufferHeight = this.Height;
            pp.PresentationInterval = PresentInterval.Immediate;
            pp.DisplayOrientation = DisplayOrientation.Default;

            GraphicsDevice.Reset(pp);

            LoadContent();
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if(this.camera != null)
                this.camera.Pos = new Vector2(this.Width * 0.5f, this.Height * 0.5f);
            base.OnResize(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            drawingBatch = new XnaDrawingBatch(this.GraphicsDevice);

            screen = new RenderTarget2D(
                this.GraphicsDevice,
                this.GraphicsDevice.PresentationParameters.BackBufferWidth,
                this.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                this.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            XnaDrawing.blank = blank;
            XnaDrawing.spriteBatch = spriteBatch;
            XnaDrawing.drawingBatch = drawingBatch;
            XnaDrawing.defaultFont = font;

            camera = new XnaCamera2d(this.GraphicsDevice.Viewport);
            camera.Pos = new Vector2(400.0f, 300.0f);
            camera.Zoom = 300;

            float rho = 100;
            bool blue = false;
            for (float y = -2000; y <= 2000; y += 200)
            {
                for (float x = -1000; x <= 1000; x += 200)
                {
                    blue = !blue;
                    for (float deg = 0; deg < 360; deg += 10)
                    {
                        float theta = MathHelper.ToRadians(deg);
                        float x1 = rho * (float)Math.Cos(theta);
                        float y1 = rho * (float)Math.Sin(theta);

                        if (blue)
                            blueRoundLines.Add(new RoundLine(x, y, x + x1, y + y1));
                        else
                            greenRoundLines.Add(new RoundLine(x, y, x + x1, y + y1));
                    }
                }
            }

            //Create2DViewMatrix();
            Create2DProjectionMatrix();

            if (textureManager != null)
            {
                cursorTexture = textureManager.LoadTexture2d(@"C:\Users\dovieya\Desktop\XNA 2D Editor\fyri2deditor\Xna2dEditor\Resources\Cursor.png");
            }
        }

        public void Create2DViewMatrix()
        {
            Vector3 position = Vector3.Zero;
            Vector3 target = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);

            viewMatrix = Matrix.CreateLookAt(position, target, up);
        }

        /// <summary>
        /// Create a simple 2D projection matrix
        /// </summary>
        public void Create2DProjectionMatrix()
        {
            // Projection matrix ignores Z and just squishes X or Y to balance the upcoming viewport stretch
            float projScaleX;
            float projScaleY;
            float width = this.GraphicsDevice.Viewport.Width;
            float height = this.GraphicsDevice.Viewport.Height;
            if (width > height)
            {
                // Wide window
                projScaleX = height / width;
                projScaleY = 1.0f;
            }
            else
            {
                // Tall window
                projScaleX = 1.0f;
                projScaleY = width / height;
            }
            projMatrix = Matrix.CreateScale(projScaleX, projScaleY, 0.0f);
            projMatrix.M43 = 0.5f;

            //projMatrix = Matrix.CreateOrthographic(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, -1, 1);
        }

        public Vector2 get_mouse_vpos()
        {
            System.Drawing.Point p = this.PointToClient(Control.MousePosition); 
            float MouseWorldX = (p.X - GraphicsDevice.Viewport.Width * 0.5f + (GraphicsDevice.Viewport.Width * 0.5f + camera.Pos.X) * (float)Math.Pow(camera.Zoom, 3)) /
                  (float)Math.Pow(camera.Zoom, 3);

            float MouseWorldY = ((p.Y - GraphicsDevice.Viewport.Height * 0.5f + (GraphicsDevice.Viewport.Height * 0.5f + camera.Pos.Y) * (float)Math.Pow(camera.Zoom, 3))) /
                    (float)Math.Pow(camera.Zoom, 3);

            return new Vector2(MouseWorldX, MouseWorldY);
        }        

        protected void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            
            MouseState mouseState = Mouse.GetState();

            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            MyMouseX = mouseState.X;
            MyMouseY = mouseState.Y;

            if (gamePadState.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
                keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                if (!aButtonDown)
                {
                    aButtonDown = true;
                    roundLineTechniqueIndex++;
                    if (roundLineTechniqueIndex >= roundLineTechniqueNames.Length)
                        roundLineTechniqueIndex = 0;
                }
            }
            else
            {
                aButtonDown = false;
            }

            float leftX = gamePadState.ThumbSticks.Left.X;
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                leftX -= 1.0f;
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                leftX += 1.0f;

            float leftY = gamePadState.ThumbSticks.Left.Y;
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                leftY += 1.0f;
            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                leftY -= 1.0f;

            float dx = leftX * 0.01f * camera.Zoom;
            float dy = leftY * 0.01f * camera.Zoom;

            bool zoomIn = gamePadState.Buttons.RightShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
                keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z);
            bool zoomOut = gamePadState.Buttons.LeftShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
                keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X);

            //cameraX += dx;
            //cameraY += dy;
            camera.Move(new Vector2(dx, dy));

            if (zoomIn)
                camera.Zoom /= 0.995f;
            if (zoomOut)
                camera.Zoom *= 0.995f;

            viewMatrix = Matrix.CreateTranslation(-camera.Pos.X, -camera.Pos.Y, 0) * Matrix.CreateScale(1.0f / camera.Zoom, 1.0f / camera.Zoom, 1.0f);

            if (roundLineManager != null)
            {
                if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageUp))
                    roundLineManager.BlurThreshold *= 1.001f;
                if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageDown))
                    roundLineManager.BlurThreshold /= 1.001f;

                if (roundLineManager.BlurThreshold > 1)
                    roundLineManager.BlurThreshold = 1;
            }

            //if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            //{
            //    camera.Move(new Vector2(0.0f, 1.0f));
            //}

            //if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            //{
            //    camera.Move(new Vector2(0.0f, -1.0f));
            //}

            //if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            //{
            //    camera.Move(new Vector2(-1.0f, 0.0f));
            //}

            //if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            //{
            //    camera.Move(new Vector2(1.0f, 0.0f));
            //}

            //Update view
            //viewMatrix = Matrix.CreateTranslation(-camera.Pos.X, -camera.Pos.Y, 0) * Matrix.CreateScale(1.0f / camera.Zoom, 1.0f / camera.Zoom, 1.0f);
        }

        public void UpdateTimer()
        {
            TimeSpan elapsedTime = new TimeSpan();

            if (previousGameTimer != null)
                elapsedTime = gameTimer.TotalGameTime - previousGameTimer.TotalGameTime;

            gameTimer = new GameTime(timer.Elapsed, elapsedTime);

            previousGameTimer = gameTimer;
        }
        
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            UpdateTimer();
            Update();

            viewProjMatrix = viewMatrix * projMatrix;
            time = (float)gameTimer.TotalGameTime.TotalSeconds;
            curTechniqueName = roundLineTechniqueNames[roundLineTechniqueIndex];
            
            //DrawSceneToTexture(screen);

            // Clear to the default control background color.
            //Color backColor = new Color(BackColor.R, BackColor.G, BackColor.B);
            Color backColor = Color.White;

            GraphicsDevice.Clear(backColor);


            if (SpriteFont != null && RoundLineManager != null && Effect != null)
            {
                //Draw Grid
                if (DrawGrid)
                    DrawGridsAndScale();

                drawingContext.Begin();
                ////drawingContext.Begin(
                ////    SpriteSortMode.Immediate, 
                ////    BlendState.AlphaBlend, 
                ////    SamplerState.LinearClamp, 
                ////    DepthStencilState.None, 
                ////    RasterizerState.CullNone, 
                ////    null, 
                ////    camera.get_transformation(GraphicsDevice));

                //drawingContext.DrawText(SpriteFont, "Camera X: " + camera.Pos.X + " Y: " + camera.Pos.Y, new Vector2(0, 0), Color.Black);
                drawingContext.DrawText(SpriteFont, "Mouse X: " + mousePos.X + " Y: " + mousePos.Y, new Vector2(0, 0), Color.Black);
                drawingContext.DrawText(SpriteFont, "Rect X: " + NetRectangle.X + " Y: " + NetRectangle.Y, new Vector2(0, 20), Color.Black);
                drawingContext.DrawText(SpriteFont, "Rect W: " + NetRectangle.Width + " H: " + NetRectangle.Height, new Vector2(0, 40), Color.Black);
                //drawingContext.DrawText(SpriteFont, "Viewport Width: " + this.GraphicsDevice.Viewport.Width + " Height: " + this.GraphicsDevice.Viewport.Height, new Vector2(0, 90), Color.Black);
                //drawingContext.DrawFilledRectangle(myMouseRect, Color.HotPink);
                ////drawingContext.DrawFilledRectangle(myOtherMouseRect, Color.Magenta);
                drawingContext.End();

                spriteBatch.Begin();
                drawingBatch.Begin();

                DrawNetSelection();

                if(_graphicsList != null)
                    _graphicsList.Draw(spriteBatch);

                //DrawRoundLine();

                //Always draw mouseTexture last
	 
	            // draw cursor
                spriteBatch.Draw(cursorTexture.Texture, mousePos, null, Color.White, 0f,
	                    Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
	            spriteBatch.End();
                drawingBatch.End();
            }

            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            //    SamplerState.LinearClamp, DepthStencilState.Default,
            //    RasterizerState.CullNone);

            //spriteBatch.Draw(screen, new Rectangle(0, 0, this.Width, this.Height), Color.White);

            //spriteBatch.End();
        }

        /// <summary>
        /// Draws the entire scene in the given render target.
        /// </summary>
        /// <returns>A texture2D with the scene drawn in it.</returns>
        protected void DrawSceneToTexture(RenderTarget2D renderTarget)
        {
            // Set the render target
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);
        }        

        private void DrawGridsAndScale()
        {
            

            //var majorlinesPen = new Pen(System.Drawing.Color.Wheat, 1);
            //var minorlinesPen = new Pen(System.Drawing.Color.LightGray, 1);
            majorLines.Clear();
            minorLines.Clear();

            Xorigin = Yorigin = 0;
            ScaleX = _width;
            ScaleY = _height;

            _width = (int)(SizePicture.X);
            _height = (int)(SizePicture.Y);

            var xMajorLines = (int)(_width / MajorIntervals / ScaleDraw.X);
            var yMajorLines = (int)(_height / MajorIntervals / ScaleDraw.Y);

            try
            {
                //draw X Axis major lines
                for (int i = 0; i <= xMajorLines; i++)
                {
                    float x = i * (_width / xMajorLines);
                    minorLines.Add(new RoundLine(x, 0.0f, x, _height));

                    //draw X Axis minor lines
                    for (int i1 = 1; i1 <= Xdivs; i1++)
                    {
                        float x1 = i1 * MajorIntervals / (Xdivs) * ScaleDraw.X;
                        majorLines.Add(new RoundLine(x + x1, 0, x + x1, _height));
                    }
                }

                //draw Y Axis major lines
                for (int i = 0; i <= yMajorLines; i++)
                {
                    //y = i * (Height / (yMajorLines));
                    float y = i * MajorIntervals * ScaleDraw.Y;
                    minorLines.Add(new RoundLine(0, y, _width, y));

                    //draw Y Axis minor lines
                    for (int i1 = 1; i1 <= Ydivs; i1++)
                    {
                        float y1 = i1 * MajorIntervals / (Ydivs) * ScaleDraw.Y;
                        majorLines.Add(new RoundLine(0, y + y1, _width, y + y1));
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            roundLineManager.NumLinesDrawn = 0;

            float lineRadius = 1;
            roundLineManager.BlurThreshold = roundLineManager.ComputeBlurThreshold(lineRadius, viewProjMatrix,
                GraphicsDevice.PresentationParameters.BackBufferWidth);

                      

            if (RoundLineManager != null)
            {
                roundLineManager.Draw(minorLines, lineRadius, Color.LightGray, viewProjMatrix, time, curTechniqueName);
                roundLineManager.Draw(majorLines, lineRadius, Color.Wheat, viewProjMatrix, time, curTechniqueName);

                dude.Pos = new Vector2(camera.Pos.X, camera.Pos.Y);
                roundLineManager.Draw(dude, 8, Color.Red, viewProjMatrix, time, "Tubular");
            }
        }

        void DrawRoundLine()
        {
            Matrix viewProjMatrix = viewMatrix * projMatrix;

            roundLineManager.NumLinesDrawn = 0;

            float lineRadius = 4;
            roundLineManager.BlurThreshold = roundLineManager.ComputeBlurThreshold(lineRadius, viewProjMatrix,
                GraphicsDevice.PresentationParameters.BackBufferWidth);

            float time = (float)gameTimer.TotalGameTime.TotalSeconds;
            string curTechniqueName = roundLineTechniqueNames[roundLineTechniqueIndex];

            roundLineManager.Draw(blueRoundLines, lineRadius, Color.Blue, viewProjMatrix, time, curTechniqueName);
            roundLineManager.Draw(greenRoundLines, lineRadius, Color.Green, viewProjMatrix, time, curTechniqueName);

            dude.Pos = new Vector2(camera.Pos.X, camera.Pos.Y);
            roundLineManager.Draw(dude, 8, Color.Red, viewProjMatrix, time, "Tubular");

            Vector2 textPos = new Vector2(50, 50);
            spriteBatch.Begin();
            if (gameTimer.IsRunningSlowly)
                spriteBatch.DrawString(SpriteFont, "IsRunningSlowly", textPos, Color.Red);
            else
                spriteBatch.DrawString(SpriteFont, "IsRunningNormally", textPos, Color.White);

            textPos += new Vector2(0, 30);
            spriteBatch.DrawString(SpriteFont, string.Format("{0} Lines", roundLineManager.NumLinesDrawn), textPos, Color.White);

            textPos += new Vector2(0, 30);
            spriteBatch.DrawString(SpriteFont, string.Format("Technique (Press A): {0}", curTechniqueName), textPos, Color.White);

            spriteBatch.End();
        }

        /// <summary>
        ///  Draw group selection rectangle
        /// </summary>
        /// <param name="g"></param>
        public void DrawNetSelection()
        {
            if (!DrawNetRectangle)
                return;

            XnaDrawing.DrawRectangle(NetRectangle, Color.Black);

            //var r = new System.Drawing.Rectangle(Convert.ToInt32(NetRectangle.X), Convert.ToInt32(NetRectangle.Y),
            //    Convert.ToInt32(NetRectangle.Width), Convert.ToInt32(NetRectangle.Height));
            
            //ControlPaint.DrawFocusRectangle(g, r, System.Drawing.Color.Black, System.Drawing.Color.Transparent);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor.Hide();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor.Show();
            base.OnMouseLeave(e);
        }

        
    }
}
