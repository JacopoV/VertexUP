using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VertexUP.GameObjects
{
    public class Polygon
    {

        public Vector3 degrees;
        public int objectType;   // 0 = Cube, 1 = Tetra
        public int vertices;
        public Vector3 coordinates;
        public bool selected;

        public Polygon() { }


        public Polygon(int objectType,Vector3 coord, Vector3 degrees)
        {
            /*
             * indicates the values for rotation on X, Y, Z axis
             */
            this.objectType = objectType;

            switch (objectType)
            {
                case 0:
                    vertices = 8;
                    break;
                    
                case 1:
                    vertices = 4;
                    break;
            }

            this.coordinates = coord;
            this.degrees = degrees;
            this.selected = false;
        }

    }
}
