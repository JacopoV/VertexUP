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
using VertexUP.GameObjects;

namespace VertexUP
{
    public class LogicQuest : LogicPolygon
    {

        public float timeToComplete;
        public int selectedVertices;


        public LogicQuest() : base() { }

        public LogicQuest(GameDifficulty difficulty, ContentManager Content) : base(difficulty,Content) 
        {
            selectedVertices = 0;

            switch (difficulty)
            {

                case GameDifficulty.Easy:
                    timeToComplete = 15.0f;
                    break;

                case GameDifficulty.Normal:
                    timeToComplete = 10.0f;
                    break;

                case GameDifficulty.Hard:
                    timeToComplete = 8.0f;
                    break;
            }
        }


        public void ManageQuest()
        {

            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;

            for (int i = 0; i < gs.polygons.Count; i++)
            {
                if (gs.polygons.ElementAt(i).selected)
                {
                    selectedVertices += gs.polygons.ElementAt(i).vertices;
                }

            }

        }

        public void CompleteQuest()
        {

            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;

            if (selectedVertices == gs.quest.verticesQuest)
            {

                gs.quest.completedQuest = true;
                gs.player.questSolved += 1;
                gs.player.score += gs.quest.pointsQuest;
            }
            else
            {
                gs.quest.errorQuest = true;

            }


            selectedVertices = 0;

        }



        public void Update(float dt)
        {

            base.Update(dt);

            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;


            /*
             * quest need to be updated
             */ 


            if (gs.quest != null)
            {

                gs.quest.timeActive += dt;

                if (gs.quest.timeActive > gs.quest.timeToComplete)
                {
                    gs.quest.errorQuest = true; 

                }


            }

            
            /*
             * reduce the time to select polygons if player is good
             */

            if (gs.player.questSolved % 15 == 0 && gs.player.questSolved > 0)
            {
                if(timeToComplete>5.0f)
                 timeToComplete -= 1;
            }




            /*
             * that's mean that there is NO quest, so generate it
             */

            Random nPoly = new Random();
            Random index = new Random();

            if (gs.quest == null)
            {

                /*
                 * NOTE : in base alla prosecuzione del gioco (TEMPO) aumentare i punti in modo logaritmico
                 * 
                 */

               

                int nPolyToSelect = nPoly.Next(1, 12);  // select between 1 and 12 polygons
                int indexToSelect = index.Next(0, 11);  // select the index from 0 to 11

                int totalVertices = 0;

                for (int i = 0; i < nPolyToSelect; i++)
                {

                    totalVertices += gs.polygons.ElementAt(indexToSelect).vertices;
                    indexToSelect = index.Next(0, 11);
                }


                 int pointsQuest = 5 + totalVertices/3 ;

                /*
                 * generate && add the quest (solution comes from the polygons generated randomically)
                 */ 

                gs.quest = new Quest(pointsQuest, totalVertices, timeToComplete);

            }

            
        }
    }
}
