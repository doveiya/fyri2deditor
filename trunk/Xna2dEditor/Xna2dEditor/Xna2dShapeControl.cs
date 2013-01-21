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
#endregion

namespace Fyri2dEditor
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, and displays
    /// a spinning 3D model. The main form class is responsible for loading
    /// the model: this control just displays it.
    /// </summary>
    class Xna2dShapeControl : GraphicsDeviceControl
    {
        // Timer controls the rotation speed.
        Timer updateTimer;
        Stopwatch timer;
        SpriteBatch spriteBatch;
        GameTime gameTimer;
        GameTime previousGameTimer;

        List<XnaLine2d> blueRoundLines = new List<XnaLine2d>();
        List<XnaLine2d> greenRoundLines = new List<XnaLine2d>();

        Matrix viewMatrix;
        Matrix projMatrix;
        float cameraX = 0;
        float cameraY = 0;
        float cameraZoom = 300;
        XnaCamera2d camera;
        RenderTarget2D screen;

        XnaDisc2d dude = new XnaDisc2d(0, 0, 8, Color.Red);
        bool aButtonDown = false;
        int roundLineTechniqueIndex = 0;
        string[] roundLineTechniqueNames;

        #region Xna Properties

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

        #endregion

        FyriTexture2d cursorTexture;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
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
            base.OnResize(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.GraphicsDevice);

            screen = new RenderTarget2D(
                this.GraphicsDevice,
                this.GraphicsDevice.PresentationParameters.BackBufferWidth,
                this.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                this.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            camera = new XnaCamera2d(this.GraphicsDevice.Viewport);
            camera.Pos = new Vector2(0.0f, 0.0f);

            Create2DViewMatrix();
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
            //float projScaleX;
            //float projScaleY;
            //float width = this.GraphicsDevice.Viewport.Width;
            //float height = this.GraphicsDevice.Viewport.Height;
            //if (width > height)
            //{
            //    // Wide window
            //    projScaleX = height / width;
            //    projScaleY = 1.0f;
            //}
            //else
            //{
            //    // Tall window
            //    projScaleX = 1.0f;
            //    projScaleY = width / height;
            //}
            //projMatrix = Matrix.CreateScale(projScaleX, projScaleY, 0.0f);
            //projMatrix.M43 = 0.5f;

            projMatrix = Matrix.CreateOrthographic(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, -1, 1);
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

        Vector2 mousePos = Vector2.Zero;
        public Rectangle myMouseRect = new Rectangle();
        public Rectangle myOtherMouseRect = new Rectangle();

        protected void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            
            MouseState mouseState = Mouse.GetState();

            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            //if (gamePadState.Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
            //    keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            //{
            //    this.Exit();
            //}

            //Vector2 mousePos = GraphicsDevice.Viewport.Unproject(new Vector3(mouseState.X, mouseState.Y, 0.0f)

            //System.Drawing.Point p = this.PointToClient(Control.MousePosition);

            //mousePos = new Vector2(p.X, p.Y);

            //float aspectRatio = Width / Height;

            //Vector2 mouseCam = get_mouse_vpos();
            //Vector2 screenToWorld = camera.WorldToScreen(new Vector2(p.X, p.Y));
            //Vector3 mouseP = new Vector3(p.X, p.Y, 0.0f);

            //Matrix world = camera.get_transformation(this.GraphicsDevice);
            //Matrix world = Matrix.Identity;

            //Vector3 unProjected = this.GraphicsDevice.Viewport.Unproject(mouseP, projMatrix, viewMatrix, world);

            //myMouseRect = new Rectangle((int)(p.X), (int)(p.Y), 20, 25);
            //myOtherMouseRect = new Rectangle((int)screenToWorld.X, (int)screenToWorld.Y, 20, 25);

            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                camera.Move(new Vector2(0.0f, 1.0f));
            }

            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                camera.Move(new Vector2(0.0f, -1.0f));
            }

            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                camera.Move(new Vector2(-1.0f, 0.0f));
            }

            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                camera.Move(new Vector2(1.0f, 0.0f));
            }
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

            //DrawSceneToTexture(screen);

            // Clear to the default control background color.
            //Color backColor = new Color(BackColor.R, BackColor.G, BackColor.B);
            Color backColor = Color.CornflowerBlue;

            GraphicsDevice.Clear(backColor);


            if (SpriteFont != null && LineBatch != null && Effect != null)
            {
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
                drawingContext.DrawText(SpriteFont, "Mouse X: " + mousePos.X + " Y: " + mousePos.Y, new Vector2(0, 70), Color.Black);
                //drawingContext.DrawText(SpriteFont, "Viewport Width: " + this.GraphicsDevice.Viewport.Width + " Height: " + this.GraphicsDevice.Viewport.Height, new Vector2(0, 90), Color.Black);
                //drawingContext.DrawFilledRectangle(myMouseRect, Color.HotPink);
                ////drawingContext.DrawFilledRectangle(myOtherMouseRect, Color.Magenta);
                drawingContext.End();

                spriteBatch.Begin();
	 
	            // draw cursor
                spriteBatch.Draw(cursorTexture.Texture, mousePos, null, Color.White, 0f,
	                    Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
	            spriteBatch.End();
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
