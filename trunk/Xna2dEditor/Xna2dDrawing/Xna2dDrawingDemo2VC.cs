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
#endregion

namespace Fyri2dEditor
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, and displays
    /// a spinning 3D model. The main form class is responsible for loading
    /// the model: this control just displays it.
    /// </summary>
    class Xna2dDrawingDemo2VC : GraphicsDeviceControl
    {
        // Timer controls the rotation speed.
        Stopwatch timer;
        SpriteBatch spriteBatch;
        GameTime gameTimer;
        GameTime previousGameTimer;

        string[] roundLineTechniqueNames;

        public Texture2D Texture
        {
            get { return texture; }

            set
            {
                texture = value;
            }
        }

        private Texture2D texture;

        public XnaDrawingContext DrawingContext
        {
            get { return drawingContext; }

            set
            {
                drawingContext = value;
                //if (lineBatch != null)
                //roundLineTechniqueNames = lineBatch.TechniqueNames;
            }
        }

        private XnaDrawingContext drawingContext;

        public XnaDrawingBatch DrawingBatch
        {
            get { return drawingBatch; }

            set
            {
                drawingBatch = value;
                //if (lineBatch != null)
                    //roundLineTechniqueNames = lineBatch.TechniqueNames;
            }
        }

        private XnaDrawingBatch drawingBatch;

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
        }

        protected void Update()
        {
            
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
            Color backColor = Color.White;

            GraphicsDevice.Clear(backColor);

            if (DrawingBatch != null && Texture != null && SpriteFont != null)
            {
                drawingContext.Begin();
                drawingContext.DrawLine(10, 20, 100, 20, Color.Red);
                drawingContext.DrawRectangle(120, 10, 100, 20, Color.Blue);
                drawingContext.DrawTriangle(240, 10, 240, 60, 200, 60, Color.Black);
                drawingContext.DrawEllipse(310, 10, 50, 50, Color.Green);
                drawingContext.DrawTexture(texture, new Vector2(10, 300), Color.White);
                drawingContext.DrawPolyline(new Vector2[] { new Vector2(410, 10), new Vector2(440, 10), new Vector2(420, 20), new Vector2(440, 40), new Vector2(410, 60) }, Color.Aqua);
                drawingContext.DrawFilledRectangle(120, 110, 50, 50, Color.Blue);
                drawingContext.DrawFilledTriangle(240, 110, 240, 160, 200, 160, Color.Brown);
                drawingContext.DrawFilledEllipse(310, 110, 80, 40, Color.Green);
                drawingContext.DrawText(SpriteFont, "Hello World!", new Vector2(120, 300), Color.Black);
                drawingContext.End();
                
            }
        }       
    }
}
