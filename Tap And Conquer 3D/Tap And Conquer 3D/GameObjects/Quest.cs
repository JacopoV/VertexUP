using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertexUP.GameObjects
{
    public class Quest
    {

        public string questString = "Vertex UP to :  ";
        public float timeActive;
        public int pointsQuest;
        public bool completedQuest;
        public int verticesQuest;
        public bool errorQuest;
        public float timeToComplete;

        public Quest() { }

        public Quest(int points_quest, int n_vertices_quest, float time)
        {
            this.pointsQuest = points_quest;
            completedQuest = false;
            this.verticesQuest = n_vertices_quest;

            questString += verticesQuest;

            timeActive = 0.0f;
            errorQuest = false;

            this.timeToComplete = time;
        }

    }
}
