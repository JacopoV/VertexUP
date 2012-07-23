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
using VertexUP.GameObjects;

namespace VertexUP
{
    using System.Threading;
    #if WINDOWS_PHONE
    using Microsoft.Devices;
    #endif

    public enum WinLoseState
    {
        Win,
        Lose,
        None
    }

    public class LogicState : LogicQuest
    {

        public double totalGameTime;
       
        public int score;


        #if WINDOWS_PHONE
        public VibrateController dual_shock = VibrateController.Default;
        #endif

        public LogicState() : base() { }
        public LogicState(GameDifficulty difficulty, ContentManager Content) : base(difficulty,Content)
        {
             this.difficulty = difficulty;
             totalGameTime = 0.0;
             score = 0;

        }

        public void UpdateTime(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            totalGameTime += gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(dt);
     
        }

        private Ray calculateRay(Vector2 position, RenderingState renderingState)
        {
            /* 
             * Calculate from the tap position X and Y the ray
             * that intersects the polygon bounding sphere
             */
            Vector3 nearPoint = new Vector3(position.X, position.Y, 0);
            Vector3 farPoint = new Vector3(position.X, position.Y, 1);

            nearPoint = renderingState.spriteBatch.GraphicsDevice.Viewport.Unproject
                (nearPoint, renderingState.projection, renderingState.view, renderingState.world);

            farPoint = renderingState.spriteBatch.GraphicsDevice.Viewport.Unproject
                (farPoint, renderingState.projection, renderingState.view, renderingState.world);

            Vector3 direction = Vector3.Normalize(farPoint - nearPoint);

            return new Ray(nearPoint, direction);
        }

        private Polygon calculateTargetPolygon(Ray pickRay, RenderingState renderingState)
        {

            /*
             * check if the ray itersects a polygon on the screen
             */ 

            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;

            Polygon target = null;

            foreach (Polygon p in gs.polygons)
            {

                Vector3 position = p.coordinates;
                BoundingSphere boundingSphere = renderingState.boundingSphereCube;

                boundingSphere.Center = position;
                if (pickRay.Intersects(boundingSphere) != null)
                {
                    if (p.selected)
                    {
                        /*
                         * abort the selection of the polygon
                         */ 

                        p.selected = false;
                        target = null;
                        return target;

                    }
                    else
                    {
                        /*
                         * allow the selection of the polygon
                         */

                        AudioState aux = content.ServiceProvider.GetService(typeof(AudioState)) as AudioState;
                        aux.playRayPick();
                        p.selected = true;
                        target = p;
                        return target;
                    }



                   }

            }

            return target;

        }


        public void UpdateInput(RenderingState renderingState)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                var touchGesture = TouchPanel.ReadGesture();
                Ray pickRay;

                switch (touchGesture.GestureType)
                {
                    case GestureType.Tap:

                        Polygon target = null;

                        pickRay = calculateRay(new Vector2(touchGesture.Position.X, touchGesture.Position.Y), renderingState);

                        target = calculateTargetPolygon(pickRay, renderingState);

                        if (touchGesture.Position.X > 700 && touchGesture.Position.Y > 415)
                        {
                            AudioState aux = content.ServiceProvider.GetService(typeof(AudioState)) as AudioState;
                            aux.playConfirm();
                            ManageQuest(); 
                            CompleteQuest();
                        }



                        break;
                }

            }
        }



        public void UpdateInputWindows(RenderingState renderingState)
        {

            MouseState touchGesture = Mouse.GetState();
            Ray pickRay;


            /*
             * Grab mouse position
             * 
             */
            float clickX = touchGesture.X;
            float clickY = touchGesture.Y;

            if (touchGesture.LeftButton == ButtonState.Pressed)
            {
                Polygon target = null;

                pickRay = calculateRay(new Vector2(clickX, clickY), renderingState);

                target = calculateTargetPolygon(pickRay, renderingState);

                if (clickX > 700 && clickY > 415)
                {
                    AudioState aux = content.ServiceProvider.GetService(typeof(AudioState)) as AudioState;
                    aux.playConfirm();
                    ManageQuest();
                    CompleteQuest();
                }

            }

        }

        
      }
    }


  


