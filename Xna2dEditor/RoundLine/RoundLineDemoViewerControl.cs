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
#endregion

namespace Fyri2dEditor
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, and displays
    /// a spinning 3D model. The main form class is responsible for loading
    /// the model: this control just displays it.
    /// </summary>
    class RoundLineDemoViewerControl : GraphicsDeviceControl
    {
        // Timer controls the rotation speed.
        Stopwatch timer;
        SpriteBatch spriteBatch;
        GameTime gameTimer;
        GameTime previousGameTimer;

        List<RoundLine> blueRoundLines = new List<RoundLine>();
        List<RoundLine> greenRoundLines = new List<RoundLine>();
        Matrix viewMatrix;
        Matrix projMatrix;
        float cameraX = 0;
        float cameraY = 0;
        float cameraZoom = 300;
        Disc dude = new Disc(0, 0);
        bool aButtonDown = false;
        int roundLineTechniqueIndex = 0;
        string[] roundLineTechniqueNames;

        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        public RoundLineManager RoundLineManager
        {
            get { return roundLineManager; }

            set
            {
                roundLineManager = value;
                if(roundLineManager != null)
                    roundLineTechniqueNames = roundLineManager.TechniqueNames;
            }
        }

        RoundLineManager roundLineManager;

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

            spriteBatch = new SpriteBatch(this.GraphicsDevice);

            LoadContent();
        }

        public void LoadContent()
        {
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

            Create2DProjectionMatrix();
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
        }

        protected void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            //if (gamePadState.Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
            //    keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            //{
            //    this.Exit();
            //}

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

            float dx = leftX * 0.01f * cameraZoom;
            float dy = leftY * 0.01f * cameraZoom;

            bool zoomIn = gamePadState.Buttons.RightShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
                keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z);
            bool zoomOut = gamePadState.Buttons.LeftShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
                keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X);

            cameraX += dx;
            cameraY += dy;
            if (zoomIn)
                cameraZoom /= 0.995f;
            if (zoomOut)
                cameraZoom *= 0.995f;

            viewMatrix = Matrix.CreateTranslation(-cameraX, -cameraY, 0) * Matrix.CreateScale(1.0f / cameraZoom, 1.0f / cameraZoom, 1.0f);

            if (roundLineManager != null)
            {
                if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageUp))
                    roundLineManager.BlurThreshold *= 1.001f;
                if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageDown))
                    roundLineManager.BlurThreshold /= 1.001f;

                if (roundLineManager.BlurThreshold > 1)
                    roundLineManager.BlurThreshold = 1;
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

            // Clear to the default control background color.
            //Color backColor = new Color(BackColor.R, BackColor.G, BackColor.B);
            Color backColor = Color.CornflowerBlue;

            GraphicsDevice.Clear(backColor);

            if (SpriteFont != null && RoundLineManager != null && Effect != null)
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

                dude.Pos = new Vector2(cameraX, cameraY);
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
        }       
    }
}
