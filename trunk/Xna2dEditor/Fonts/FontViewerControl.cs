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
#endregion

namespace Fyri2dEditor
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, and displays
    /// a spinning 3D model. The main form class is responsible for loading
    /// the model: this control just displays it.
    /// </summary>
    class FontViewerControl : GraphicsDeviceControl
    {
        // Timer controls the rotation speed.
        Stopwatch timer;
        SpriteBatch spriteBatch;

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
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Start the animation timer.
            timer = Stopwatch.StartNew();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }
        
        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            // Clear to the default control background color.
            //Color backColor = new Color(BackColor.R, BackColor.G, BackColor.B);
            Color backColor = Color.CornflowerBlue;

            GraphicsDevice.Clear(backColor);

            if (SpriteFont != null)
            {

                // tell our graphics card that we are ready to draw
                // The first two parameters are default, the third one
                // tells our graphics card to return to the previous state
                // after drawing, so our 3d models will look nice.
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null);

                string testSentence = "The quick brown fox jumps over the lazy dog";
                Vector2 position = new Vector2((float)(this.Bounds.Width / 2) - (testSentence.Length / 2), (float)(this.Bounds.Height / 2));

                spriteBatch.DrawString(SpriteFont, testSentence, position, Color.Black);

                spriteBatch.End();
            }
        }       
    }
}
