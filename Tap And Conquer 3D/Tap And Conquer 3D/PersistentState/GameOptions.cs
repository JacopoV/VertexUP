using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;


namespace VertexUP.PersistentState
{
  public struct GameOptions
  {
    public bool showTooltips;
    public bool audioActive;

    public static bool ShowTooltips { get { return Current.showTooltips; }
      set
      {
        Current.showTooltips = value;
        SaveCurrent();
      }
    }

    public static bool AudioActive
    {
        get { return Current.audioActive; }
        set
        {
            Current.audioActive = value;
            SaveCurrent();
        }
    }

    static GameOptions Current;
#if WINDOWS_PHONE
    static IsolatedStorageFile IS = IsolatedStorageFile.GetUserStoreForApplication();
#else
      static IsolatedStorageFile IS = IsolatedStorageFile.GetUserStoreForDomain();
#endif
    static string optionsPath = "options";
    static GameOptions()
    {
      try
      {
        using (var f = IS.OpenFile(optionsPath, System.IO.FileMode.Open))
        {
          var s = new XmlSerializer(typeof(GameOptions));
          Current = (GameOptions)s.Deserialize(f);
        }
      }
      catch (Exception)
      {
        Current.showTooltips = true;
        if (IS.FileExists(optionsPath))
          IS.DeleteFile(optionsPath);
        SaveCurrent();
      }
    }

    public static void SaveCurrent()
    {
      using (var f = IS.CreateFile(optionsPath))
      {
        var s = new XmlSerializer(typeof(GameOptions));
        s.Serialize(f, Current);
      }
    }

  }
}
