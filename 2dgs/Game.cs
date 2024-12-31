using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class Game : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private FontSystem _fontSystem;
    
    private Test test;
    private Desktop _desktop;

    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        test = new Test();
    }

    protected override void Initialize()
    {
        Window.Title = "2DGS - Alpha";
        test.RunAllTests(_graphics, Window.Title);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _fontSystem = new FontSystem();
        _fontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/orbitron_light.ttf"));
        
        MyraEnvironment.Game = this;

        var grid = new Grid
        {
            RowSpacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        // COLUMNS
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        
        // ROWS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // TITLE
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // SIMS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // SETTINGS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // QUIT

        var title = new Label
        {
            Id = "title",
            Text = "2DGS",
            Font = _fontSystem.GetFont(70),
            HorizontalAlignment = HorizontalAlignment.Center
        };
        Grid.SetRow(title, 0);
        grid.Widgets.Add(title);
        
        var simulationMenu = new Button
        {
            Width=300,
            Height=75,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Simulations",
                Font = _fontSystem.GetFont(20)
            }
        };
        Grid.SetRow(simulationMenu, 1);

        simulationMenu.Click += (s, a) =>
        {
            Console.WriteLine("This will navigate you to the simulations menu...");
        };

        grid.Widgets.Add(simulationMenu);
        
        var settingsMenu = new Button
        {
            Width=300,
            Height=75,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Settings",
                Font = _fontSystem.GetFont(20)
            }
        };
        Grid.SetRow(settingsMenu, 2);

        settingsMenu.Click += (s, a) =>
        {
            Console.WriteLine("This will navigate you to the settings menu...");
        };

        grid.Widgets.Add(settingsMenu);
        
        var quitButton = new Button
        {
            Width=300,
            Height=75,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Quit",
                Font = _fontSystem.GetFont(20)
            }
        };
        Grid.SetRow(quitButton, 3);

        quitButton.Click += (s, a) =>
        {
            Exit();
        };

        grid.Widgets.Add(quitButton);
        
        _desktop = new Desktop();
        _desktop.Root = grid;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _desktop.Render();

        base.Draw(gameTime);
    }
}