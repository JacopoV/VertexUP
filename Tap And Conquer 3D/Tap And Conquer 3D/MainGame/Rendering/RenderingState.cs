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
using VertexUP;


namespace VertexUP
{
    public class RenderingState : RenderingPolygon
    {
        public Texture2D glowingLine;
        public Texture2D radarScanLine;
        public Texture2D radarScanCircle;
        public SpriteFont font;

        public Texture2D okButton;



        public RenderingState(ContentManager Content)
            : base(Content)
        {
            glowingLine = Content.Load<Texture2D>("Textures/glowingLine");
            radarScanCircle = Content.Load<Texture2D>("Textures/radarScanCircle");
            radarScanLine = Content.Load<Texture2D>("Textures/radarScanLine");
            font = Content.Load<SpriteFont>("Fonts/NEON");

            okButton = Content.Load<Texture2D>("Textures/ok_80");
        }



        public void DrawCentered(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            var textSize = font.MeasureString(text);
            spriteBatch.DrawString(font, text, position - textSize * 0.5f + Vector2.One * 2.0f, Color.Black);
            spriteBatch.DrawString(font, text, position - textSize * 0.5f, color);
        }

        public void Draw(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, position + Vector2.One * 2.0f, Color.Black);
            spriteBatch.DrawString(font, text, position, color);
        }

        public void DrawRightAligned(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            var textSize = font.MeasureString(text);
            textSize.Y = 0.0f;
            spriteBatch.DrawString(font, text, position - textSize + Vector2.One * 2.0f, Color.Black);
            spriteBatch.DrawString(font, text, position - textSize, color);
        }

        public void DrawTooltipOpening(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float tooltipTimer)
        {

            
            tooltipTimer = MathHelper.SmoothStep(0.0f, 1.0f, tooltipTimer);
            position -= 480 * Vector2.UnitY * (1.0f - tooltipTimer);
            var textSize = font.MeasureString(text);
            spriteBatch.DrawString(font, text, position + Vector2.One * 2.0f, Color.Black,
              0.0f, textSize * 0.5f, tooltipTimer, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, position, color,
              0.0f, textSize * 0.5f, tooltipTimer, SpriteEffects.None, 0);
            
        }

        public void DrawTooltipClosing(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float tooltipTimer)
        {
            
            tooltipTimer = MathHelper.SmoothStep(0.0f, 1.0f, tooltipTimer);
            position -= 480 * Vector2.UnitY * (1.0f - tooltipTimer);
            var textSize = font.MeasureString(text);
            spriteBatch.DrawString(font, text, position + Vector2.One * 2.0f, Color.Black,
              0.0f, textSize * 0.5f, tooltipTimer, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, position, color,
              0.0f, textSize * 0.5f, tooltipTimer, SpriteEffects.None, 0);
            
        }



        public void Draw()
        {

            base.Draw();

            GameState gs = (GameState)content.ServiceProvider.GetService(typeof(GameState));

            if (gs.quest != null)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, gs.quest.questString.ToString(), new Vector2(20, 5), Color.GreenYellow);
                spriteBatch.DrawString(font, "Time Left : " + ((int)(gs.quest.timeToComplete - gs.quest.timeActive)).ToString(), new Vector2(320, 5), Color.GreenYellow);
                spriteBatch.DrawString(font, "Score : " + gs.player.score.ToString(), new Vector2(570, 5), Color.GreenYellow);
                spriteBatch.Draw(okButton, new Rectangle(700, 415, 80, 60), Color.GreenYellow);
                spriteBatch.End();
            }
       
        }


    }


}