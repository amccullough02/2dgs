using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsScene : Scene
{
    private readonly SettingsMediator _settingsMediator;
    private readonly TextureManager _textureManager;
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;
    private readonly SettingsMenuUi _settingsMenuUi;
    
    public SettingsScene(Game game)
    {
        _settingsMediator = new SettingsMediator();
        _settingsMediator.CurrentResolution = new Vector2(game.Graphics.PreferredBackBufferWidth, game.Graphics.PreferredBackBufferHeight);
        SetupDictionaries(_settingsMediator);
        _textureManager = new TextureManager(game.Content, game.GraphicsDevice);
        _settingsMenuUi = new SettingsMenuUi(game, _settingsMediator);
    }
    
    private static void SetupDictionaries(SettingsMediator settingsMediator)
    {
        settingsMediator.NewShortcuts.Add("PauseShortcut", []);
        settingsMediator.NewShortcuts.Add("SpeedUpShortcut", []);
        settingsMediator.NewShortcuts.Add("SpeedDownShortcut", []);
        settingsMediator.NewShortcuts.Add("TrailsShortcut", []);
        settingsMediator.NewShortcuts.Add("OrbitsShortcut", []);
        settingsMediator.NewShortcuts.Add("VectorsShortcut", []);
        settingsMediator.NewShortcuts.Add("NamesShortcut", []);
        settingsMediator.NewShortcuts.Add("GlowShortcut", []);
        settingsMediator.NewShortcuts.Add("EditShortcut", []);
        settingsMediator.NewShortcuts.Add("ScreenshotShortcut", []);
        
        settingsMediator.DefaultShortcuts.Add("PauseShortcut", [Keys.LeftControl, Keys.P]);
        settingsMediator.DefaultShortcuts.Add("SpeedUpShortcut", [Keys.LeftControl, Keys.Right]);
        settingsMediator.DefaultShortcuts.Add("SpeedDownShortcut", [Keys.LeftControl, Keys.Left]);
        settingsMediator.DefaultShortcuts.Add("TrailsShortcut", [Keys.LeftControl, Keys.T]);
        settingsMediator.DefaultShortcuts.Add("OrbitsShortcut", [Keys.LeftControl, Keys.O]);
        settingsMediator.DefaultShortcuts.Add("VectorsShortcut", [Keys.LeftControl, Keys.V]);
        settingsMediator.DefaultShortcuts.Add("NamesShortcut", [Keys.LeftControl, Keys.N]);
        settingsMediator.DefaultShortcuts.Add("GlowShortcut", [Keys.LeftControl, Keys.G]);
        settingsMediator.DefaultShortcuts.Add("EditShortcut", [Keys.LeftControl, Keys.E]);
        settingsMediator.DefaultShortcuts.Add("ScreenshotShortcut", [Keys.F11]);
    }

    public override void Update(GameTime gameTime)
    {
        if (_settingsMediator.Remapping)
        {
            _keyboardState = Keyboard.GetState();

            foreach (var key in _keyboardState.GetPressedKeys())
            {
                if (_previousKeyboardState.IsKeyDown(key)) continue;
                
                if (_settingsMediator.NewShortcuts.ContainsKey(_settingsMediator.WhichShortcut))
                {
                    _settingsMediator.NewShortcuts[_settingsMediator.WhichShortcut].Add(key);
                }
            }
            
            _previousKeyboardState = _keyboardState;

            _settingsMediator.ShortcutPreview  = StringTransformer.KeyBindString(_settingsMediator.NewShortcuts[_settingsMediator.WhichShortcut]);
        }

        if (_settingsMediator.ClearShortcut)
        {
            _settingsMediator.NewShortcuts[_settingsMediator.WhichShortcut].Clear();
            _settingsMediator.ClearShortcut = false;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var screenWidth = _settingsMediator.CurrentResolution.X;
        var screenHeight = _settingsMediator.CurrentResolution.Y;
        
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.SettingsBackground, TextureManager.PositionAtCenter(screenWidth, screenHeight, 
            _textureManager.SettingsBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            TextureManager.PositionAtCenter(screenWidth, screenHeight, _textureManager.Gradient), Color.White);
        spriteBatch.End();
        _settingsMenuUi.Draw();
    }
}