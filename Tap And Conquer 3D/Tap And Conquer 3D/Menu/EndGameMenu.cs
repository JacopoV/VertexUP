using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VertexUP.Resources;

namespace VertexUP.Menu
{
    class EndGameMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {

        SpriteBatch spriteBatch;
        Texture2D menuBackground;
        Rectangle main_menu_rect;
        Vector2 main_menu_pos;
        SpriteFont font36, font14;

        public bool resultGame;

        public event Action menu;
        public event Action restart;

        public Vector2 vectorAdd3 = Vector2.One * 4.0f;

        public EndGameMenu(Game game, bool win)
        : base(game)
        {
            TouchPanel.EnabledGestures = GestureType.Tap;

            resultGame = win;

            spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            loadContent();

            
            
        }

        public void loadContent()
        {
            font36 = Game.Content.Load<SpriteFont>("Fonts/Regular36");
            font14 = Game.Content.Load<SpriteFont>("Fonts/Regular14");
            menuBackground = Game.Content.Load<Texture2D>("Textures/loose");

            var midd_w = Game.GraphicsDevice.Viewport.Width / 2;

            main_menu_rect = new Rectangle(midd_w - (int)(font36.MeasureString(Strings.NewGame).X / 2.0f),
                200, (int)font36.MeasureString(Strings.NewGame).X, (int)font36.MeasureString(Strings.NewGame).Y);
            main_menu_pos = new Vector2(main_menu_rect.X, main_menu_rect.Y);

            AudioState aux = (AudioState)Game.Content.ServiceProvider.GetService(typeof(AudioState));
           // aux.playIntroSound();
        }

        public override void Update(GameTime gameTime)
        {
#if WINDOWS_PHONE
            while (TouchPanel.IsGestureAvailable)
            {
                var gs = TouchPanel.ReadGesture();
                if (gs.GestureType == GestureType.Tap)
                {
                    var pos = new Point((int)gs.Position.X, (int)gs.Position.Y);

                    if (main_menu_rect.Contains(pos))
                    {
                        if (main_menu_rect != null) menu();
                    }

                }
            }
#endif
#if WINDOWS
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                var pos = new Point((int)Mouse.GetState().X, (int)Mouse.GetState().Y);
                if (main_menu_rect.Contains(pos))
                {
                    if (main_menu_rect != null) restart();
                }
                else if (main_menu_rect.Contains(pos))
                {
                    if (main_menu_rect != null) menu();
                }
            }
#endif
        }

        public override void Draw(GameTime gametime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(menuBackground, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);
            
            spriteBatch.DrawString(font36, Strings.MainMenu, main_menu_pos + vectorAdd3, Color.Black * 0.5f);
            spriteBatch.DrawString(font36, Strings.MainMenu, main_menu_pos, Color.White);
            
            
            spriteBatch.End();
        }

    }
}