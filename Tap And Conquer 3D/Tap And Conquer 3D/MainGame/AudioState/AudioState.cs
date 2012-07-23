using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace VertexUP
{
    public class AudioState
    {

        public SoundEffect music;
        public SoundEffect rayPick;
        public SoundEffect confirm;
        public SoundEffect gameOver;
        public SoundEffectInstance instanceGameMusic;

        public bool canPlay;

        public AudioState() { }

        public AudioState(ContentManager Content, bool gameHasControl)
        {

            music = Content.Load<SoundEffect>("Sounds/loop_music");
            rayPick = Content.Load<SoundEffect>("Sounds/ray");
            confirm = Content.Load<SoundEffect>("Sounds/confirm");
            gameOver = Content.Load<SoundEffect>("Sounds/gameover");
            
            instanceGameMusic = music.CreateInstance();

            this.canPlay = gameHasControl;
            
            playGameMusic();
            
        }

        public void playGameMusic()
        {
            if (canPlay)
            {
                instanceGameMusic.IsLooped = true;
                instanceGameMusic.Play();
            }
            
        }

        public void stopGameMusic()
        {
            if (canPlay)
            {
                instanceGameMusic.Stop();
            }
        }

        public void playRayPick()
        {
            if (canPlay)
            {
                rayPick.Play();
            }

        }

        public void playConfirm()
        {
            if (canPlay)
            {
                confirm.Play();
            }
        }

        public void playGameOver()
        {
            if (canPlay)
            {
                gameOver.Play();
            }
        }

    }
}
