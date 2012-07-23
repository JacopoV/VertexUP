using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml.Serialization;
using VertexUP.Resources;


namespace VertexUP.Menu
{
    public class NewGameMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Rectangle easy_rect, medium_rect, hard_rect, back_rect;
        Vector2 easy_pos, medium_pos, hard_pos;
        SpriteFont font36;
        Texture2D back_btn;

        public NewGameMenu(Game game)
            : base(game)
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public override void Initialize()
        {
            spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            font36 = Game.Content.Load<SpriteFont>("Fonts/Regular36");
            back_btn = Game.Content.Load<Texture2D>("Textures/back");

            var midd_w = GraphicsDevice.Viewport.Width / 2;

            easy_rect = new Rectangle(600,
                65, (int)font36.MeasureString(Strings.Easy).X, (int)font36.MeasureString(Strings.Easy).Y);
            easy_pos = new Vector2(easy_rect.X, easy_rect.Y);

            medium_rect = new Rectangle(555,
                135, (int)font36.MeasureString(Strings.Medium).X, (int)font36.MeasureString(Strings.Medium).Y);
            medium_pos = new Vector2(medium_rect.X, medium_rect.Y);

            hard_rect = new Rectangle(590,
                205, (int)font36.MeasureString(Strings.Hard).X, (int)font36.MeasureString(Strings.Hard).Y);
            hard_pos = new Vector2(hard_rect.X, hard_rect.Y);

            back_rect = new Rectangle(0, 0, back_btn.Width, back_btn.Height);

            base.LoadContent();
        }

#if WINDOWS_PHONE
        public override void Update(GameTime gameTime)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                var gs = TouchPanel.ReadGesture();
                if (gs.GestureType == GestureType.Tap)
                {
                    var pos = new Point((int)gs.Position.X, (int)gs.Position.Y);
                    if (easy_rect.Contains(pos))
                    {
                        if (Easy != null) Easy();
                    }
                    else if (medium_rect.Contains(pos))
                    {
                        if (Medium != null) Medium();
                    }
                    else if (hard_rect.Contains(pos))
                    {
                        if (Hard != null) Hard();
                    }
                    else if (back_rect.Contains(pos))
                    {
                        if (Back != null) Back();
                    }
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                if (Back != null)
                    Back();

            base.Update(gameTime);
        }
#endif

#if WINDOWS
        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                var pos = new Point((int)Mouse.GetState().X, (int)Mouse.GetState().Y);
                if (easy_rect.Contains(pos))
                {
                    if (Easy != null) Easy();
                }
                else if (medium_rect.Contains(pos))
                {
                    if (Medium != null) Medium();
                }
                else if (hard_rect.Contains(pos))
                {
                    if (Hard != null) Hard();
                }
                else if (back_rect.Contains(pos))
                {
                    if (Back != null) Back();
                }
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                if (Back != null)
                    Back();

            base.Update(gameTime);
        }

#endif

        public event Action Easy;
        public event Action Medium;
        public event Action Hard;
        public event Action Back;
        public Vector2 vectorAdd3 = Vector2.One * 4.0f;

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/back"), Vector2.Zero, Color.White);

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/bkg2"), Vector2.Zero, Color.White);

            spriteBatch.DrawString(font36, Strings.Easy, easy_pos + vectorAdd3, Color.Black * 0.5f);
            spriteBatch.DrawString(font36, Strings.Medium, medium_pos + vectorAdd3, Color.Black * 0.5f);
            spriteBatch.DrawString(font36, Strings.Hard, hard_pos + vectorAdd3, Color.Black * 0.5f);

            spriteBatch.DrawString(font36, Strings.Easy, easy_pos, Color.White);
            spriteBatch.DrawString(font36, Strings.Medium, medium_pos, Color.White);
            spriteBatch.DrawString(font36, Strings.Hard, hard_pos, Color.White);

            spriteBatch.Draw(back_btn, Vector2.Zero, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
