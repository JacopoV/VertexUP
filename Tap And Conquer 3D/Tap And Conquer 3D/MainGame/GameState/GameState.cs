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
  
    public class GameState
    {
      public Player player;
      public List<Polygon> polygons;
      public Quest quest;

      public int total_score;

      public GameState() {

          player = new Player();
          polygons = new List<Polygon>();
          quest = null;

          total_score = 0;
     }

  }
}

