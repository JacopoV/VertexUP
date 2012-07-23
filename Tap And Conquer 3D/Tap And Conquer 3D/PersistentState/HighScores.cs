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
using System.Xml.Serialization;


namespace VertexUP
{
  public struct HighScore
  {
    public int score;
    public DateTime timeStamp;

    public override string ToString()
    {
        return "      "+score + " on " + 
            timeStamp.ToString("dd-MM") + "       @ " + timeStamp.ToString("hh:mm");
    }
  }

  public struct HighScores
  {
    public List<HighScore> easyHighScores;
    public List<HighScore> mediumHighScores;
    public List<HighScore> hardHighScores;

    public static HighScores Current;
    private static string highScoresPath = "highScores";
    static HighScores()
    {
#if WINDOWS_PHONE
      var IS = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
#else
        var IS = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForDomain();
#endif
      try
      {
        using (var f = IS.OpenFile(highScoresPath, System.IO.FileMode.Open)) {
          var serializer = new XmlSerializer(typeof(HighScores));
          Current = (HighScores)serializer.Deserialize(f);
        }

      }
      catch (Exception)
      {
        Current = new HighScores()
        {
          easyHighScores = new List<HighScore>(),
          mediumHighScores = new List<HighScore>(),
          hardHighScores = new List<HighScore>()
        };
       
      }
    }

    public static void AddScore(GameDifficulty difficulty, HighScore newItem)
    {
      switch (difficulty)
      {
        case GameDifficulty.Easy:
          Current.easyHighScores.Add(newItem);
          Current.easyHighScores = Current.easyHighScores.OrderByDescending(h => h.score).Take(6).ToList();
          break;
        case GameDifficulty.Normal:
          Current.mediumHighScores.Add(newItem);
          Current.mediumHighScores = Current.mediumHighScores.OrderByDescending(h => h.score).Take(6).ToList();
          break;
        case GameDifficulty.Hard:
          Current.hardHighScores.Add(newItem);
          Current.hardHighScores = Current.hardHighScores.OrderByDescending(h => h.score).Take(6).ToList();
          break;
      }
#if WINDOWS_PHONE
      var IS = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
#else
      var IS = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForDomain();
#endif
      try
      {
        using (var f = IS.CreateFile(highScoresPath))
        {
          var serializer = new XmlSerializer(typeof(HighScores));
          serializer.Serialize(f, Current);
        }

      }
      catch (Exception)
      {
      }

    }
  }
}
