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
using System.IO.IsolatedStorage;
using VertexUP.Resources;
using VertexUP.PersistentState;
using VertexUP.GameObjects;

#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif


namespace VertexUP
{
  enum TooltipState
  {
    Opening,
    Showing,
    Hidden,
    Closing
  }
  public class MainGameComponent : Microsoft.Xna.Framework.DrawableGameComponent
  {
    SpriteBatch spriteBatch;
    RenderingState renderingState;
    LogicState logicState;
    GameState gameState;
    WinLoseState winLoseState = WinLoseState.None;
    TooltipState tooltipState;
    GameDifficulty gameDifficulty;
    AudioState audio;

    float tooltipTimer;

    public event Action Back;
    public event Action Win;
    public event Action Lose;

    public MainGameComponent(Game game, bool forceNewGame, GameDifficulty difficulty)
      : base(game)
    {
      TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag;

      #if WINDOWS_PHONE
      PhoneApplicationService.Current.Closing += (s, e) => SaveGameState();
      PhoneApplicationService.Current.Deactivated += (s, e) => SaveGameState();
      #endif




      if (forceNewGame)
      {
          logicState = new LogicState(difficulty, Content);
          gameState = new GameState();
          if (GameOptions.AudioActive == true)
              audio = new AudioState(Content, MediaPlayer.GameHasControl);
          else
              audio = new AudioState(Content, false);
          Game.Services.RemoveService(typeof(GameState));
          Game.Services.AddService(typeof(GameState), gameState);
          Game.Services.RemoveService(typeof(AudioState));
          Game.Services.AddService(typeof(AudioState), audio);
          gameDifficulty = difficulty;

      }
      else
      {
#if WINDOWS_PHONE
          logicState = CreateOrResumeState(difficulty);
#endif
      }
    }

    ContentManager Content { get { return Game.Content; } }

    protected override void LoadContent()
    {
      renderingState = new RenderingState(Content);
      
      base.LoadContent();
    }


    public override void Initialize()
    {
        
        spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
      
        base.Initialize();
    }

#if WINDOWS_PHONE
    IsolatedStorageFile IS = IsolatedStorageFile.GetUserStoreForApplication();
    string savePath = "current.sav";
#else
      IsolatedStorageFile IS = IsolatedStorageFile.GetUserStoreForDomain();
    string savePath = "current.sav";
#endif


#if WINDOWS_PHONE
    private LogicState CreateOrResumeState(GameDifficulty createDifficulty)
    {
      if (IS.FileExists(savePath))
      {
        try
        {
          GameState gameState = null;
          using (var saveFile = IS.OpenFile(savePath, System.IO.FileMode.Open))
          {
            var deserializer = new XmlSerializer(typeof(GameState));
            gameState = deserializer.Deserialize(saveFile) as GameState;
            Game.Services.AddService(typeof(GameState), gameState);
            audio = new AudioState(Content, MediaPlayer.GameHasControl);
            Game.Services.AddService(typeof(AudioState), audio);
            
          }
          IS.DeleteFile(savePath);
          return logicState;
        }
        catch (Exception e)
        {
          if (IS.FileExists(savePath))
            IS.DeleteFile(savePath);
          return new LogicState(createDifficulty,Content);
        }
      }
      else
      {
        return new LogicState(createDifficulty,Content);
      }
    }
#endif

    private void SaveGameState()
    {
      if (winLoseState != WinLoseState.None)
        return;
#if WINDOWS_PHONE
      try
      {

        using (var saveFile = IS.CreateFile(savePath))
        {
          var serializer = new XmlSerializer(typeof(GameState));
          gameState = Content.ServiceProvider.GetService(typeof(GameState)) as GameState;
          serializer.Serialize(saveFile, gameState);
        }

      }
      catch (Exception)
      {
      }

#endif
    }



    public override void Update(GameTime gameTime)
    {
        
        GameState gs = (GameState)Game.Services.GetService(typeof(GameState));


        if (gs.quest != null && gs.quest.errorQuest)
        {
            winLoseState = WinLoseState.Lose;
        }
        
        
        UpdateMainGame(gameTime);
        base.Update(gameTime);
    }

    private void UpdateMainGame(GameTime gameTime)
    {

        if (logicState == null)
            logicState = new LogicState(gameDifficulty, Content);

      //if (tooltipState != TooltipState.Hidden)
      //  return;
      switch (winLoseState)
      {
        case WinLoseState.Win:
              
        case WinLoseState.Lose:
            audio.stopGameMusic();
            audio.playGameOver();
            
            Lose();
           
                HighScores.AddScore(logicState.difficulty, new HighScore()
                {
                    timeStamp = DateTime.Now,
                    score = logicState.score
                });
            

          while (TouchPanel.IsGestureAvailable)
          {
            var gs = TouchPanel.ReadGesture();
            if (gs.GestureType == GestureType.Tap)
            {
              logicState = new LogicState(logicState.difficulty,logicState.content);
              winLoseState = WinLoseState.None;
              SaveGameState();
            }
          }
          try
          {
#if WINDOWS_PHONE   
            if (IS.FileExists(savePath))
              IS.DeleteFile(savePath);
#endif 
          }
          catch (Exception)
          {
          }
          if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            if (Back != null)
              Back();

          break;
        case WinLoseState.None:
          
          logicState.UpdateTime(gameTime);

#if WINDOWS_PHONE
          logicState.UpdateInput(renderingState);
#endif
#if WINDOWS
          logicState.UpdateInputWindows(renderingState);
#endif
          //winLoseState = gameState.UpdateBlocks(gameTime, renderingState, random);
          
          if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
          {
            this.Enabled = false;
            Guide.BeginShowMessageBox(Strings.ConfirmExit, Strings.ConfirmExitMessage,
              new[] { Strings.Yes, Strings.No }, 0, MessageBoxIcon.Alert, new AsyncCallback(result =>
              {
                var dialogResult = Guide.EndShowMessageBox(result);
                this.Enabled = true;
                if (dialogResult.HasValue && dialogResult.Value == 0)
                  if (Back != null)
                  {
                    SaveGameState();
                    Back();
                  }
              }), null);
          }
          break;
      }
    }

    public override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin();

      if (winLoseState == WinLoseState.Lose)
        renderingState.DrawCentered(spriteBatch, Strings.GameLoseMessage,
          new Vector2(400, 240), Color.White);
      else
      {
          //DRAW GAME CALL WITH RENDERINGSTATE
      }
      var help = Strings.Help;
      switch (tooltipState)
      {
        case TooltipState.Opening:
          renderingState.DrawTooltipOpening(spriteBatch, help, new Vector2(400, 240), Color.White, tooltipTimer);
          break;
        case TooltipState.Showing:
          renderingState.DrawCentered(spriteBatch, help, new Vector2(400, 240), Color.White);
          break;
        case TooltipState.Hidden:
          break;
        case TooltipState.Closing:
          renderingState.DrawTooltipClosing(spriteBatch, help, new Vector2(400, 240), Color.White, 1.0f - tooltipTimer);
          break;
        default:
          break;
      }

      spriteBatch.End();

      renderingState.Draw();

      

      

      base.Draw(gameTime);

      }





  }
}
