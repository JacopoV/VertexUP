using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace VertexUP
{
    public class RenderingBackground
    {
        public ContentManager content;
        public SpriteBatch spriteBatch;
        public SpriteFont spriteFont;
        public Texture2D background;

        public Vector3 camera;
        public float fieldOfView;
        public float aspectRatio;
        public Matrix world;
        public Matrix view;
        public Matrix projection;


        public RenderingBackground(ContentManager Content)
        {
            this.content = Content;
            spriteBatch = Content.ServiceProvider.GetService(typeof(SpriteBatch)) as SpriteBatch;
            
            background = Content.Load<Texture2D>("Textures/bkg3");

            /*
             * camere definition + aspect ratio
             */ 
            camera = new Vector3(0, 2000.0f, 0);
            fieldOfView = 45;
            this.aspectRatio = (float)(4.0f/3.0f);
                
            /*
             * matrix definition 
             */ 

            world = Matrix.Identity;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfView), aspectRatio, 1.0f, 10000.0f);
            view = Matrix.CreateLookAt(camera, new Vector3(0,0,0), Vector3.Forward);
        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Texture, spriteBatch.GraphicsDevice.BlendState, SamplerState.LinearClamp, spriteBatch.GraphicsDevice.DepthStencilState, spriteBatch.GraphicsDevice.RasterizerState);
            
            spriteBatch.Draw(background, new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), new Rectangle(0, 0, background.Width, background.Height), Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);

            spriteBatch.End();
            
        }

    }
}
