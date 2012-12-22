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
using System.Linq;
#endregion

namespace Fyri2dEditor
{
    class Xna2dDrawingDemo4VC : GraphicsDeviceControl
    {
        
        public event ContextEventHandler ContextChanged;
        protected virtual void OnContextChanged(ContextEventArgs e)
        {
            ContextChanged(this, e);
        }

        // Timer controls the rotation speed.
        Stopwatch timer;
        SpriteBatch spriteBatch;
        GameTime gameTimer;
        GameTime previousGameTimer;

        string[] roundLineTechniqueNames;

        DrawingTexture drawingTexture;
        DrawingComponent drawingComponent;

        public GameComponentCollection Components
        {
            get
            {
                return components;
            }
            set
            {
                components = value;
            }
        }

        GameComponentCollection components;

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
                XnaDrawingContext oldValue = drawingContext;
                drawingContext = value;
                if(oldValue != drawingContext)
                    OnContextChanged(new ContextEventArgs(oldValue, drawingContext));
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
        }

        public void LoadContent()
        {
            Components.Clear();

            Components.Add(drawingComponent);

            Color[] backgroundColor = Enumerable.Repeat<Color>(Color.White, drawingComponent.DrawingTexture.Width * drawingComponent.DrawingTexture.Height).ToArray();
            drawingComponent.DrawingTexture.Background.SetData<Color>(backgroundColor);
        }

        private bool mouseDown = false;
        private Vector2 lastMousePosition;

        protected void Update()
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            // TODO: Add your update logic here
            MouseState mouseState = Mouse.GetState();
            if (mouseDown)
            {
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                if (mousePosition != lastMousePosition)
                {
                    var preMousePosition = lastMousePosition;
                    drawingComponent.DrawingContext.DrawLine(preMousePosition, mousePosition, Color.Black);
                    lastMousePosition = mousePosition;
                }
                if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    mouseDown = false;
                }
            }
            else
            {
                if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    mouseDown = true;
                    lastMousePosition = new Vector2(mouseState.X, mouseState.Y);
                }
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
            Color backColor = Color.White;

            GraphicsDevice.Clear(backColor);

            if (DrawingContext != null && Texture != null && SpriteFont != null)
            {
            }
        }       
    }
}
