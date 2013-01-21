using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xna2dEditor
{
    public class XnaCamera2d
    {
        protected float zoom; // Camera Zoom
        private Matrix transform; // Matrix Transform
        private Vector2 position; // Camera Position
        private Vector2 origin; // Camera Position
        protected float rotation; // Camera Rotation

        protected Viewport viewPort;

        public XnaCamera2d(Viewport viewPort)
        {
            origin = new Vector2(viewPort.Width / 2.0f, viewPort.Height / 2.0f);
            zoom = 1.0f;
            rotation = 0.0f;
            position = Vector2.Zero;
        }

        // Sets and gets zoom
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            position += amount;
        }

        // Get set position
        public Vector2 Pos
        {
            get { return position; }
            set { position = value; }
        }

        //public Matrix get_transformation(GraphicsDevice graphicsDevice)
        //{
        //    transform =       // Thanks to o KB o for this solution
        //      Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
        //                                 Matrix.CreateRotationZ(Rotation) *
        //                                 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
        //                                 Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
        //    return transform;
        //}

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            transform = Matrix.CreateTranslation(new Vector3(-graphicsDevice.Viewport.Width * 0.5f - position.X, -graphicsDevice.Viewport.Height * 0.5f - position.Y, 0)) * Matrix.CreateScale(
                new Vector3((zoom * zoom * zoom),
                (zoom * zoom * zoom), 0))
            * Matrix.CreateRotationZ(rotation)
               * Matrix.CreateTranslation(new Vector3(
                graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));

            return transform;
        }

        

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-position * parallax, 0.0f)) *
                // The next line has a catch. See note below.
                   Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(new Vector3(origin, 0.0f));
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, GetViewMatrix(Vector2.One));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix(Vector2.One)));
        }
    }
}
