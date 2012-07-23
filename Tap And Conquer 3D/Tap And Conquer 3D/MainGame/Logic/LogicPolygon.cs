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
    public class LogicPolygon
    {
        public ContentManager content;
        public GameDifficulty difficulty;


        public int maxPolygonGeneration = 12;
        public int spanPolyX = 800;
        public int spanPolyY = 100;
        public int spanPolyZ = 700;
        public Vector3 positionPoly;
        

        public LogicPolygon() { }

        public LogicPolygon(GameDifficulty Difficulty,ContentManager Content) 
        {

            this.content = Content;
            this.difficulty = Difficulty;

            positionPoly = new Vector3(-1200,-900,-750);

        }


        private void ResetSelectedPolygons()
        {
            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;

            for (int i = 0; i < gs.polygons.Count; i++)
            {
                gs.polygons.ElementAt(i).selected = false;

            }

        }


        private void PolygonRotation(int index, int degree)
        {
            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;

            Vector3 coord = gs.polygons.ElementAt(index).degrees;

            
            if (gs.polygons.ElementAt(index).degrees.X < 360)
                {
                    coord.X += degree;
                    coord.Y += degree;
                    coord.Z += degree;
                }

                else
                {
                    coord.X = 0;
                    coord.Y = 0;
                    coord.Z = 0;

                }

            gs.polygons.ElementAt(index).degrees = coord;

        }


        private void PolygonZoom(int index)
        {
            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;

            if (gs.polygons.ElementAt(index).selected)
            {

                gs.polygons.ElementAt(index).coordinates.Y = -700;
                
            }
            else
            {
                gs.polygons.ElementAt(index).coordinates.Y = -900;
            }

        }



        public void Update(float dt)
        {

            GameState gs = content.ServiceProvider.GetService(typeof(GameState)) as GameState;


            if (gs.quest != null)
            {
                if (gs.quest.completedQuest)
                {

                    ResetSelectedPolygons();
                    gs.quest = null;
                    ////gs.polygons.RemoveRange(0, 11);
                    gs.polygons = new List<Polygon>();
                 }
            }


            if (gs.polygons.Count == 0)
            {
                /*
                 *
                 * generate 12 polygons randomly at every quest
                 * 
                 */

                Random num = new Random();
                Random deg = new Random();

                for (int i = 0; i < maxPolygonGeneration; i++)
                {

                    int degree = deg.Next(15, 315);
                    Vector3 randomDegree = new Vector3(degree, degree, degree);
                    if (i > 0 && i < 4)
                    {
                        positionPoly.X += spanPolyX;

                    }

                    if (i == 4)
                    {
                        positionPoly.X = -1200;
                        positionPoly.Z += spanPolyZ;
                    }

                    if (i > 4 && i < 8)
                    {

                        positionPoly.X += spanPolyX;
                    }

                    if (i == 8)
                    {
                        positionPoly.X = -1200;
                        positionPoly.Z += spanPolyZ;
                    }

                    if (i > 8 && i < 12)
                    {
                        positionPoly.X += spanPolyX;
                    }

                    int objectType = num.Next(1, 5);

                    gs.polygons.Add(new Polygon(objectType % 2, positionPoly, randomDegree));  // generazione random sullo schermo controllando che non siano sovrapposti

                    if (i == 11)
                    {

                        positionPoly = new Vector3(-1200, -900, -750);

                    }
                }


            }
            else
            {
                for (int i = 0; i < maxPolygonGeneration; i++)
                {
                    PolygonRotation(i, 5);
                    PolygonZoom(i);

                }

            }


        }

        }
    }

