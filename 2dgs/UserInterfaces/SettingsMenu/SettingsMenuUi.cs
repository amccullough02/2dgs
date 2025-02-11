using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using Point = Microsoft.Xna.Framework.Point;

namespace _2dgs;

public class SettingsMenuUi
{
    private readonly Desktop _desktop;
    private readonly SettingsSaveData _settingsSaveData;

    public SettingsMenuUi(Game game, SettingsSceneData settingsSceneData)
    {
        _settingsSaveData = new SettingsSaveData();
        _settingsSaveData = game.SaveSystem.LoadSettings();
        
        MyraEnvironment.Game = game;
        var rootContainer = new Panel();
        var settingsTitle = UiComponents.TitleLabel("Settings Menu");
        
        rootContainer.Widgets.Add(settingsTitle);
        rootContainer.Widgets.Add(Settings(game, settingsSceneData));
        rootContainer.Widgets.Add(ExitPanel(game));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel Settings(Game game, SettingsSceneData settingsSceneData)
    {
        
        var settingsPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(0),
            Padding = new Thickness(10),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Background = new SolidBrush(Color.Black * 0.5f),
            BorderThickness = new Thickness(1),
            Border = new SolidBrush(Color.White),
        };
        
        settingsPanel.Widgets.Add(DisplaySettings(game));
        settingsPanel.Widgets.Add(AudioPanel());
        settingsPanel.Widgets.Add(MiscSettings(game, settingsSceneData));
        
        return settingsPanel;
    }

    private VerticalStackPanel DisplaySettings(Game game)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 4);
        grid.ColumnSpacing = 40;
        
        var vsyncToggleLabel = UiComponents.LightLabel("Toggle VSync");
        Grid.SetRow(vsyncToggleLabel, 0);
        
        var vsyncToggleButton =
            UiComponents.ToggleButton("V-Sync Enabled", game.Graphics.SynchronizeWithVerticalRetrace);
        Grid.SetColumn(vsyncToggleButton, 1);
        vsyncToggleButton.HorizontalAlignment = HorizontalAlignment.Right;
        vsyncToggleButton.Click += (_, _) =>
        {
            game.Graphics.SynchronizeWithVerticalRetrace = vsyncToggleButton.IsToggled;
            game.Graphics.ApplyChanges();
            Console.WriteLine("DEBUG: V-sync toggled: " + vsyncToggleButton.IsToggled);
            ((Label)vsyncToggleButton.Content).Text = vsyncToggleButton.IsToggled ? "V-Sync Enabled" : "V-Sync Disabled";
            Console.WriteLine("Actual status of V-sync: " + game.Graphics.SynchronizeWithVerticalRetrace);
        };
        
        var resolutionLabel = UiComponents.LightLabel("Resolution");
        Grid.SetRow(resolutionLabel, 1);
        
        var resolutionOptions = UiComponents.ComboView();
        Grid.SetRow(resolutionOptions, 1);
        Grid.SetColumn(resolutionOptions, 1);
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("1920 x 1080"));
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("2560 x 1080"));
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("2560 x 1440"));
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("3440 x 1440"));
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("3840 x 2160"));

        resolutionOptions.SelectedIndex = _settingsSaveData.VerticalResolution switch
        {
            1080 when (_settingsSaveData.HorizontalResolution == 1920) => 0,
            1080 when (_settingsSaveData.HorizontalResolution == 2560) => 1,
            1440 when (_settingsSaveData.HorizontalResolution == 2560) => 2,
            1440 when (_settingsSaveData.HorizontalResolution == 3440) => 3,
            2160 when (_settingsSaveData.HorizontalResolution == 3840) => 4,
            _ => 0
        };
        
        resolutionOptions.Width = 150;
        
        var windowLabel = UiComponents.LightLabel("Window Mode");
        Grid.SetRow(windowLabel, 2);
        
        var windowOptions = UiComponents.ComboView();
        windowOptions.Widgets.Add(UiComponents.DropdownLabel("Windowed"));
        windowOptions.Widgets.Add(UiComponents.DropdownLabel("Fullscreen"));

        windowOptions.SelectedIndex = _settingsSaveData.Fullscreen switch
        {
            false => 0,
            true => 1,
        };
        
        windowOptions.Width = 150;
        Grid.SetRow(windowOptions, 2);
        Grid.SetColumn(windowOptions, 1);

        var confirmLabel = UiComponents.LightLabel("Confirm Selection");
        Grid.SetRow(confirmLabel, 3);
        
        var confirmButton = UiComponents.Button("Apply Settings", width: 150, height: 40);
        Grid.SetRow(confirmButton, 3);
        Grid.SetColumn(confirmButton, 1);

        confirmButton.Click += (_, _) =>
        {
            switch (resolutionOptions.SelectedIndex)
            {
                case 0:
                    Console.WriteLine("DEBUG: Switching to 1920 x 1080 resolution");
                    game.Graphics.PreferredBackBufferWidth = 1920;
                    game.Graphics.PreferredBackBufferHeight = 1080;
                    game.Graphics.ApplyChanges();
                    break;
                case 1:
                    Console.WriteLine("DEBUG: Switching to 2560 x 1080 resolution");
                    game.Graphics.PreferredBackBufferWidth = 2560;
                    game.Graphics.PreferredBackBufferHeight = 1080;
                    game.Graphics.ApplyChanges();
                    break;
                case 2:
                    Console.WriteLine("DEBUG: Switching to 2560 x 1440 resolution");
                    game.Graphics.PreferredBackBufferWidth = 2560;
                    game.Graphics.PreferredBackBufferHeight = 1440;
                    game.Graphics.ApplyChanges();
                    break;
                case 3:
                    Console.WriteLine("DEBUG: Switching to 3440 x 1440 resolution");
                    game.Graphics.PreferredBackBufferWidth = 3440;
                    game.Graphics.PreferredBackBufferHeight = 1440;
                    game.Graphics.ApplyChanges();
                    break;
                case 4:
                    Console.WriteLine("DEBUG: Switching to 3840 x 2160 resolution");
                    game.Graphics.PreferredBackBufferWidth = 3840;
                    game.Graphics.PreferredBackBufferHeight = 2160;
                    game.Graphics.ApplyChanges();
                    break;
            }
            
            switch (windowOptions.SelectedIndex)
            {
                case 0:
                    game.Graphics.IsFullScreen = false;
                    game.Graphics.ApplyChanges();
                    game.Window.Position = new Point(100, 100);
                    Console.WriteLine("DEBUG: Switching to windowed mode");
                    break;
                case 1:
                    game.Graphics.IsFullScreen = true;
                    game.Graphics.ApplyChanges();
                    Console.WriteLine("DEBUG: Switching to fullscreen mode");
                    break;
            }

            _settingsSaveData.VerticalResolution = game.Graphics.PreferredBackBufferHeight;
            _settingsSaveData.HorizontalResolution = game.Graphics.PreferredBackBufferWidth;
            _settingsSaveData.Fullscreen = game.Graphics.IsFullScreen;
            game.SaveSystem.SaveSettings(_settingsSaveData);
        };
        
        grid.Widgets.Add(vsyncToggleLabel);
        grid.Widgets.Add(vsyncToggleButton);
        grid.Widgets.Add(resolutionLabel);
        grid.Widgets.Add(resolutionOptions);
        grid.Widgets.Add(windowLabel);
        grid.Widgets.Add(windowOptions);
        grid.Widgets.Add(confirmLabel);
        grid.Widgets.Add(confirmButton);
        
        var sectionTitle = UiComponents.MediumLabel("Display Settings");
        sectionTitle.HorizontalAlignment = HorizontalAlignment.Center;
        var divider = UiComponents.HorizontalSeparator();
        
        var panel = new VerticalStackPanel();
        
        panel.Widgets.Add(sectionTitle);
        panel.Widgets.Add(divider);
        panel.Widgets.Add(grid);

        return panel;
    }

    private VerticalStackPanel MiscSettings(Game game, SettingsSceneData settingsSceneData)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 2);
        grid.ColumnSpacing = 40;
        
        var showFpsLabel = UiComponents.LightLabel("Toggle FPS Counter");
        
        var showFpsToggleButton = UiComponents.ToggleButton("FPS Enabled", true);
        showFpsToggleButton.Width = 150;
        showFpsToggleButton.Click += (_, _) =>
        {
            game.FpsCounter.ToggleFps();
            ((Label)showFpsToggleButton.Content).Text = showFpsToggleButton.IsToggled ? "FPS Enabled" : "FPS Disabled";
        };
        Grid.SetColumn(showFpsToggleButton, 1);
        
        var dialog = RemapShortcutsDialog.Create(game, settingsSceneData, _settingsSaveData);
        
        var keyBindLabel = UiComponents.LightLabel("Keyboard Shortcuts");
        Grid.SetRow(keyBindLabel, 1);
        
        var keyBindDialogButton = UiComponents.Button("Configure", width: 150, height: 40);
        keyBindDialogButton.Click += (_, _) =>
        {
            _settingsSaveData.Refresh(game.SaveSystem);
            settingsSceneData.ResetNewShortcuts();
            dialog.Show(_desktop);
        };
        Grid.SetColumn(keyBindDialogButton, 1);
        Grid.SetRow(keyBindDialogButton, 1);
        
        grid.Widgets.Add(showFpsLabel);
        grid.Widgets.Add(showFpsToggleButton);
        grid.Widgets.Add(keyBindLabel);
        grid.Widgets.Add(keyBindDialogButton);

        var sectionTitle = UiComponents.MediumLabel("Miscellaneous Settings");
        sectionTitle.HorizontalAlignment = HorizontalAlignment.Center;
        var divider = UiComponents.HorizontalSeparator();
        
        var panel = new VerticalStackPanel();
        panel.Widgets.Add(sectionTitle);
        panel.Widgets.Add(divider);
        panel.Widgets.Add(grid);
        
        return panel;
    }

    private VerticalStackPanel AudioPanel()
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 2);
        grid.ColumnSpacing = 40;
        
        var sliderVolume = (int)(MediaPlayer.Volume * 100.0f);
        var musicVolumeLabel = UiComponents.LightLabel($"Music Volume ({sliderVolume}%)");
        musicVolumeLabel.Width = 150;
        var musicVolumeSlider = UiComponents.HorizontalSlider(sliderVolume, 0, 100);
        musicVolumeSlider.Width = 150;

        musicVolumeSlider.ValueChanged += (_, _) =>
        {
            musicVolumeLabel.Text = $"Music Volume ({(int)musicVolumeSlider.Value}%)";
            var volume = musicVolumeSlider.Value / 100;
            MediaPlayer.Volume = volume;
        };
        
        Grid.SetColumn(musicVolumeLabel, 0);
        Grid.SetColumn(musicVolumeSlider, 1);
        
        var sfxVolumeLabel = UiComponents.LightLabel($"SFX Volume ({sliderVolume}%)");
        var sfxVolumeSlider = UiComponents.HorizontalSlider(sliderVolume, 0, 100);
        sfxVolumeSlider.Width = 150;

        sfxVolumeSlider.ValueChanged += (_, _) =>
        {
            sfxVolumeLabel.Text = $"SFX Volume ({(int)sfxVolumeSlider.Value}%)";
            var volume = sfxVolumeSlider.Value / 100;
            GlobalGameData.SfxVolume = volume;
        };
        
        Grid.SetColumn(sfxVolumeLabel, 0);
        Grid.SetRow(sfxVolumeLabel, 1);
        Grid.SetColumn(sfxVolumeSlider, 1);
        Grid.SetRow(sfxVolumeSlider, 1);
        
        grid.Widgets.Add(musicVolumeLabel);
        grid.Widgets.Add(musicVolumeSlider);
        grid.Widgets.Add(sfxVolumeLabel);
        grid.Widgets.Add(sfxVolumeSlider);
        
        var sectionTitle = UiComponents.MediumLabel("Audio Settings");
        sectionTitle.HorizontalAlignment = HorizontalAlignment.Center;
        var divider = UiComponents.HorizontalSeparator();
        var panel = new VerticalStackPanel();
        
        panel.Widgets.Add(sectionTitle);
        panel.Widgets.Add(divider);
        panel.Widgets.Add(grid);

        return panel;
    }
    
    private VerticalStackPanel ExitPanel(Game game)
    {
        var button = UiComponents.Button("Return to Main Menu");
        button.Click += (_, _) =>
        {
            game.SceneManager.PushScene(new FadeInScene(game, new MainMenuScene(game)));
        };

        var verticalStackPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(UiConstants.DefaultMargin),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };
        
        verticalStackPanel.Widgets.Add(button);
        
        return verticalStackPanel;
    }
    
    public void Draw()
    {
        _desktop.Render();
    }
}