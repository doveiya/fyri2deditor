using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fyri.Games
{
    public class RotatingCubeGame : Fyri.Xna.Presentation.Game
    {

        #region Fields

        Fyri.Xna.Presentation.GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect effect;
        VertexPositionColor[] vertices;
        Vector3 position = Vector3.Zero;
        Vector3 size = Vector3.One;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        #endregion


        #region Constructors

        public RotatingCubeGame()
        {
            //if (!(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)))
            //{
                graphics = new Fyri.Xna.Presentation.GraphicsDeviceManager(this);
                ContentManager.RootDirectory = "Content";
            //}
        }

        #endregion


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
             base.Initialize();
           // TODO: Add your initialization logic here
            this.graphics.IsFullScreen = false;
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.ApplyChanges();

            if(this.Canvas != null)
                this.Canvas.Window.Title = "";

            this.InitializeVertices();
            this.InitializeIndices();

        }

        private void InitializeVertices()
        {
            vertices = new VertexPositionColor[8];
            vertices[0].Position = new Vector3(-10f, -10f, 10f);
            vertices[0].Color = Color.Yellow;
            vertices[1].Position = new Vector3(-10f, 10f, 10f);
            vertices[1].Color = Color.Green;
            vertices[2].Position = new Vector3(10f, 10f, 10f);
            vertices[2].Color = Color.Blue;
            vertices[3].Position = new Vector3(10f, -10f, 10f);
            vertices[3].Color = Color.Black;
            vertices[4].Position = new Vector3(10f, 10f, -10f);
            vertices[4].Color = Color.Red;
            vertices[5].Position = new Vector3(10f, -10f, -10f);
            vertices[5].Color = Color.Violet;
            vertices[6].Position = new Vector3(-10f, -10f, -10f);
            vertices[6].Color = Color.Orange;
            vertices[7].Position = new Vector3(-10f, 10f, -10f);
            vertices[7].Color = Color.Gray;
            vertexBuffer = new VertexBuffer(this.graphics.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            //this.vertexBuffer = new VertexBuffer(this.graphics.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            this.vertexBuffer.SetData(vertices);
        }

        private void InitializeIndices()
        {
            short[] indices = new short[36]{    
                0,1,2, //face devant
                0,2,3,
                3,2,4, //face droite                
                3,4,5,
                5,4,7, //face arrière                
                5,7,6,
                6,7,1, //face gauche
                6,1,0,
                6,0,3, //face bas                
                6,3,5,
                1,7,4, //face haut                
                1,4,2};
            this.indexBuffer = new IndexBuffer(this.graphics.GraphicsDevice, typeof(short), 36, BufferUsage.WriteOnly);
            this.indexBuffer.SetData(indices);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.effect = new BasicEffect(graphics.GraphicsDevice);
            this.effect.View = (Matrix.CreateLookAt(new Vector3(20, 30, -50), Vector3.Zero, Vector3.Up));
            this.effect.Projection = (Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f));
           // this.effect.EnableDefaultLighting();
           // this.effect.LightingEnabled = true;
            this.effect.VertexColorEnabled = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState()[Keys.Up] == KeyState.Down)
                position += Vector3.Up;
            if (Keyboard.GetState()[Keys.Down] == KeyState.Down)
                position += Vector3.Down;
            if (Keyboard.GetState()[Keys.Left] == KeyState.Down)
                position += Vector3.Left;
            if (Keyboard.GetState()[Keys.Right] == KeyState.Down)
                position += Vector3.Right;
            if (Keyboard.GetState()[Keys.PageUp] == KeyState.Down)
                size += new Vector3(0.1f, 0.1f, 0.1f);
            if (Keyboard.GetState()[Keys.PageDown] == KeyState.Down)
                size -= new Vector3(0.1f, 0.1f, 0.1f);


            // Allows the default game to exit on Xbox 360 and Windows
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float fAngle = (float)gameTime.TotalGameTime.TotalSeconds;

            //la transformation en elle même
            Matrix world = Matrix.CreateRotationY(fAngle) * Matrix.CreateRotationX(fAngle)
                                * Matrix.CreateScale(size)
                                * Matrix.CreateTranslation(position);

            this.effect.World = (world);

            base.Update(gameTime);
        }

        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            short[] indices = new short[36]{    
                0,1,2, //face devant
                0,2,3,
                3,2,4, //face droite                
                3,4,5,
                5,4,7, //face arrière                
                5,7,6,
                6,7,1, //face gauche
                6,1,0,
                6,0,3, //face bas                
                6,3,5,
                1,7,4, //face haut                
                1,4,2};
            this.indexBuffer = new IndexBuffer(this.graphics.GraphicsDevice, typeof(short), 36, BufferUsage.WriteOnly);
            this.indexBuffer.SetData(indices);
            //var vb = new VertexBuffer(this.graphics.GraphicsDevice, typeof (VertexPositionColor), 8, BufferUsage.None);


            vertices = new VertexPositionColor[8];
            vertices[0].Position = new Vector3(-10f, -10f, 10f);
            vertices[0].Color = Color.Yellow;
            vertices[1].Position = new Vector3(-10f, 10f, 10f);
            vertices[1].Color = Color.Green;
            vertices[2].Position = new Vector3(10f, 10f, 10f);
            vertices[2].Color = Color.Blue;
            vertices[3].Position = new Vector3(10f, -10f, 10f);
            vertices[3].Color = Color.Black;
            vertices[4].Position = new Vector3(10f, 10f, -10f);
            vertices[4].Color = Color.Red;
            vertices[5].Position = new Vector3(10f, -10f, -10f);
            vertices[5].Color = Color.Violet;
            vertices[6].Position = new Vector3(-10f, -10f, -10f);
            vertices[6].Color = Color.Orange;
            vertices[7].Position = new Vector3(-10f, 10f, -10f);
            vertices[7].Color = Color.Gray;
            vertexBuffer = new VertexBuffer(this.graphics.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            //this.vertexBuffer = new VertexBuffer(this.graphics.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            this.vertexBuffer.SetData(vertices);



            this.graphics.GraphicsDevice.SetVertexBuffer(this.vertexBuffer);
            this.graphics.GraphicsDevice.Indices = this.indexBuffer;
            //this.graphics.GraphicsDevice.Vertices[0].SetSource(this.vertexBuffer, 0, VertexPositionColor.SizeInBytes);
            //var ib = new IndexBuffer(this.graphics.GraphicsDevice, typeof (short), 36, BufferUsage.None);
            //this.graphics.GraphicsDevice.Indices = ib;
            //this.graphics.GraphicsDevice.VertexDeclaration = new VertexDeclaration(this.graphics.GraphicsDevice, VertexPositionColor.VertexElements);
            

            // TODO: Add your drawing code here    
            //effect.CurrentTechnique.Passes[0].Apply();
            //GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
                this.graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 12);
                //pass.End();
            }

            //effect.End();

            base.Draw(gameTime);
        }


    }

}
