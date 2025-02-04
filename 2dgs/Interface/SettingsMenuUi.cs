using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Point = Microsoft.Xna.Framework.Point;

namespace _2dgs;

public class SettingsMenuUi
{
    private readonly Desktop _desktop;
    private readonly SettingsSaveData _settingsSaveData;

    public SettingsMenuUi(Game game, SettingsMenuData settingsMenuData)
    {
        _settingsSaveData = new SettingsSaveData();
        _settingsSaveData = game.SaveSystem.LoadSettings();
        
        MyraEnvironment.Game = game;
        var rootContainer = new Panel();
        
        rootContainer.Widgets.Add(Settings(game, settingsMenuData));
        rootContainer.Widgets.Add(ExitPanel(game));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel Settings(Game game, SettingsMenuData settingsMenuData)
    {
        var settingsTitle = UiComponents.TitleLabel("Settings Menu");
        
        var settingsPanel = UiComponents.VerticalStackPanel(8,
            HorizontalAlignment.Center,
            VerticalAlignment.Top,
            new Thickness(0));
        
        settingsPanel.Widgets.Add(settingsTitle);
        settingsPanel.Widgets.Add(DisplaySettings(game));
        settingsPanel.Widgets.Add(AudioPanel());
        settingsPanel.Widgets.Add(MiscSettings(game, settingsMenuData));
        
        return settingsPanel;
    }

    public Widget GetRoot()
    {
        return _desktop.Root;
    }

    private VerticalStackPanel DisplaySettings(Game game)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 4);
        
        var vsyncToggleLabel = UiComponents.LightLabel("Toggle VSync");
        Grid.SetRow(vsyncToggleLabel, 0);
        
        var vsyncToggleButton =
            UiComponents.ToggleButton("V-Sync Enabled", game.Graphics.SynchronizeWithVerticalRetrace);
        Grid.SetColumn(vsyncToggleButton, 1);
        vsyncToggleButton.HorizontalAlignment = HorizontalAlignment.Right;
        vsyncToggleButton.Click += (s, e) =>
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

        confirmButton.Click += (s, e) =>
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
        
        var sectionTitle = UiComponents.LightLabel("Display Settings");
        sectionTitle.HorizontalAlignment = HorizontalAlignment.Center;
        var divider = UiComponents.HorizontalSeparator();
        
        var panel = new VerticalStackPanel();
        
        panel.Widgets.Add(sectionTitle);
        panel.Widgets.Add(divider);
        panel.Widgets.Add(grid);

        return panel;
    }

    private VerticalStackPanel MiscSettings(Game game, SettingsMenuData settingsMenuData)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 2);
        
        var showFpsLabel = UiComponents.LightLabel("Toggle FPS Counter");
        
        var showFpsToggleButton = UiComponents.ToggleButton("FPS Enabled", true);
        showFpsToggleButton.HorizontalAlignment = HorizontalAlignment.Right;
        showFpsToggleButton.Click += (s, e) =>
        {
            game.FpsCounter.ToggleFps();
            ((Label)showFpsToggleButton.Content).Text = showFpsToggleButton.IsToggled ? "FPS Enabled" : "FPS Disabled";
        };
        Grid.SetColumn(showFpsToggleButton, 1);

        var dialog = RemapShortcutsDialog(game, settingsMenuData);
        
        var keyBindLabel = UiComponents.LightLabel("Keyboard Shortcuts");
        Grid.SetRow(keyBindLabel, 1);
        
        var keyBindDialogButton = UiComponents.Button("Configure", width: 150, height: 40);
        keyBindDialogButton.Click += (s, e) =>
        {
            dialog.Show(_desktop);
        };
        Grid.SetColumn(keyBindDialogButton, 1);
        Grid.SetRow(keyBindDialogButton, 1);
        
        grid.Widgets.Add(showFpsLabel);
        grid.Widgets.Add(showFpsToggleButton);
        grid.Widgets.Add(keyBindLabel);
        grid.Widgets.Add(keyBindDialogButton);

        var sectionTitle = UiComponents.LightLabel("Miscellaneous Settings");
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
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 1, 2);
        var sliderVolume = (int)(MediaPlayer.Volume * 100.0f);
        var volumeSliderLabel = UiComponents.LightLabel($"Music Volume ({sliderVolume}%)");
        var volumeSlider = UiComponents.HorizontalSlider(sliderVolume, 0, 100);
        volumeSlider.Width = 150;

        volumeSlider.ValueChanged += (s, e) =>
        {
            volumeSliderLabel.Text = $"Music Volume ({(int)volumeSlider.Value}%)";
            var volume = volumeSlider.Value / 100;
            MediaPlayer.Volume = volume;
        };
        
        Grid.SetColumn(volumeSliderLabel, 0);
        Grid.SetColumn(volumeSlider, 1);
        
        grid.Widgets.Add(volumeSliderLabel);
        grid.Widgets.Add(volumeSlider);
        
        var sectionTitle = UiComponents.LightLabel("Audio Settings");
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
        button.Click += (s, e) =>
        {
            game.GameStateManager.ChangeState(new MainMenu(game));
        };

        var verticalStackPanel = UiComponents.VerticalStackPanel(8, HorizontalAlignment.Left, 
            VerticalAlignment.Bottom, new Thickness(UiConstants.DefaultMargin));
        
        verticalStackPanel.Widgets.Add(button);
        
        return verticalStackPanel;
    }

    private Dialog RemapShortcutsDialog(Game game, SettingsMenuData settingsMenuData)
    {
        var dialog = UiComponents.StyledDialog("Remap Keyboard Shortcuts");
        
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 3, 8);
        
        var pauseKeyBindLabel = UiComponents.LightLabel("Pause Shortcut");
        Grid.SetColumn(pauseKeyBindLabel, 0);

        var pauseKeyBindPreview =
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(_settingsSaveData.PauseShortcut));
        Grid.SetColumn(pauseKeyBindPreview, 1);
        
        var changePauseKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changePauseKeyBind.Click += (s, e) =>
        {
            RemapShortcut(settingsMenuData, changePauseKeyBind, pauseKeyBindPreview, "PauseShortcut");
        };
        Grid.SetColumn(changePauseKeyBind, 2);
        
        var speedUpKeyBindLabel = UiComponents.LightLabel("Sim Speed+");
        Grid.SetColumn(speedUpKeyBindLabel, 0);
        Grid.SetRow(speedUpKeyBindLabel, 1);
        
        var speedUpKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(_settingsSaveData.SpeedUpShortcut));
        Grid.SetColumn(speedUpKeyBindPreview, 1);
        Grid.SetRow(speedUpKeyBindPreview, 1);
        
        var changeSpeedUpKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeSpeedUpKeyBind.Click += (s, e) =>
        {
            RemapShortcut(settingsMenuData, changeSpeedUpKeyBind, speedUpKeyBindPreview, "SpeedUpShortcut");
        };
        Grid.SetColumn(changeSpeedUpKeyBind, 2);
        Grid.SetRow(changeSpeedUpKeyBind, 1);
        
        var slowDownKeyBindLabel = UiComponents.LightLabel("Sim Speed-");
        Grid.SetColumn(slowDownKeyBindLabel, 0);
        Grid.SetRow(slowDownKeyBindLabel, 2);
        
        var slowDownKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(_settingsSaveData.SpeedDownShortcut));
        Grid.SetColumn(slowDownKeyBindPreview, 1);
        Grid.SetRow(slowDownKeyBindPreview, 2);
        
        var changeSpeedDownKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeSpeedDownKeyBind.Click += (s, e) =>
        {
            RemapShortcut(settingsMenuData, changeSpeedDownKeyBind, slowDownKeyBindPreview, "SpeedDownShortcut");
        };
        Grid.SetColumn(changeSpeedDownKeyBind, 2);
        Grid.SetRow(changeSpeedDownKeyBind, 2);
        
        var toggleTrailsKeyBindLabel = UiComponents.LightLabel("Toggle Trails");
        Grid.SetColumn(toggleTrailsKeyBindLabel, 0);
        Grid.SetRow(toggleTrailsKeyBindLabel, 3);

        var toggleTrailsKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(_settingsSaveData.TrailsShortcut));
        Grid.SetColumn(toggleTrailsKeyBindPreview, 1);
        Grid.SetRow(toggleTrailsKeyBindPreview, 3);
        
        var changeToggleTrailsKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleTrailsKeyBind.Click += (s, e) =>
        {
            RemapShortcut(settingsMenuData, changeToggleTrailsKeyBind, toggleTrailsKeyBindPreview, "TrailsShortcut");
        };
        Grid.SetColumn(changeToggleTrailsKeyBind, 2);
        Grid.SetRow(changeToggleTrailsKeyBind, 3);
        
        var toggleNamesKeyBindLabel = UiComponents.LightLabel("Toggle Names");
        Grid.SetColumn(toggleNamesKeyBindLabel, 0);
        Grid.SetRow(toggleNamesKeyBindLabel, 4);
        
        var toggleNamesKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(_settingsSaveData.NamesShortcut));
        Grid.SetColumn(toggleNamesKeyBindPreview, 1);
        Grid.SetRow(toggleNamesKeyBindPreview, 4);
        
        var changeToggleNamesKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleNamesKeyBind.Click += (s, e) =>
        {
            RemapShortcut(settingsMenuData, changeToggleNamesKeyBind, toggleNamesKeyBindPreview, "NamesShortcut");
        };
        Grid.SetColumn(changeToggleNamesKeyBind, 2);
        Grid.SetRow(changeToggleNamesKeyBind, 4);
        
        var toggleGlowKeyBindLabel = UiComponents.LightLabel("Toggle Glow");
        Grid.SetColumn(toggleGlowKeyBindLabel, 0);
        Grid.SetRow(toggleGlowKeyBindLabel, 5);
        
        var toggleGlowKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(_settingsSaveData.GlowShortcut));
        Grid.SetColumn(toggleGlowKeyBindPreview, 1);
        Grid.SetRow(toggleGlowKeyBindPreview, 5);
        
        var changeToggleGlowKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleGlowKeyBind.Click += (s, e) =>
        {
            RemapShortcut(settingsMenuData, changeToggleGlowKeyBind, toggleGlowKeyBindPreview, "GlowShortcut");
        };
        Grid.SetColumn(changeToggleGlowKeyBind, 2);
        Grid.SetRow(changeToggleGlowKeyBind, 5);
        
        var toggleEditModeKeyBindLabel = UiComponents.LightLabel("Edit Mode");
        Grid.SetColumn(toggleEditModeKeyBindLabel, 0);
        Grid.SetRow(toggleEditModeKeyBindLabel, 6);
        
        var toggleEditModeKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(_settingsSaveData.EditShortcut));
        Grid.SetColumn(toggleEditModeKeyBindPreview, 1);
        Grid.SetRow(toggleEditModeKeyBindPreview, 6);
        
        var changeToggleEditModeKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleEditModeKeyBind.Click += (s, e) =>
        {
            RemapShortcut(settingsMenuData, changeToggleEditModeKeyBind, toggleEditModeKeyBindPreview, "EditShortcut");
        };
        Grid.SetColumn(changeToggleEditModeKeyBind, 2);
        Grid.SetRow(changeToggleEditModeKeyBind, 6);

        var screenshotKeyBindLabel = UiComponents.LightLabel("Screenshot");
        Grid.SetColumn(screenshotKeyBindLabel, 0);
        Grid.SetRow(screenshotKeyBindLabel, 7);
        
        var screenshotKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(_settingsSaveData.ScreenshotShortcut));
        Grid.SetColumn(screenshotKeyBindPreview, 1);
        Grid.SetRow(screenshotKeyBindPreview, 7);
        
        var changeScreenShotKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeScreenShotKeyBind.Click += (s, e) =>
        {
            RemapShortcut(settingsMenuData, changeScreenShotKeyBind, screenshotKeyBindPreview, "ScreenshotShortcut");
        };
        Grid.SetColumn(changeScreenShotKeyBind, 2);
        Grid.SetRow(changeScreenShotKeyBind, 7);
        
        grid.Widgets.Add(pauseKeyBindLabel);
        grid.Widgets.Add(pauseKeyBindPreview);
        grid.Widgets.Add(changePauseKeyBind);
        grid.Widgets.Add(speedUpKeyBindLabel);
        grid.Widgets.Add(speedUpKeyBindPreview);
        grid.Widgets.Add(changeSpeedUpKeyBind);
        grid.Widgets.Add(slowDownKeyBindLabel);
        grid.Widgets.Add(slowDownKeyBindPreview);
        grid.Widgets.Add(changeSpeedDownKeyBind);
        grid.Widgets.Add(toggleTrailsKeyBindLabel);
        grid.Widgets.Add(toggleTrailsKeyBindPreview);
        grid.Widgets.Add(changeToggleTrailsKeyBind);
        grid.Widgets.Add(toggleNamesKeyBindLabel);
        grid.Widgets.Add(toggleNamesKeyBindPreview);
        grid.Widgets.Add(changeToggleNamesKeyBind);
        grid.Widgets.Add(toggleGlowKeyBindLabel);
        grid.Widgets.Add(toggleGlowKeyBindPreview);
        grid.Widgets.Add(changeToggleGlowKeyBind);
        grid.Widgets.Add(toggleEditModeKeyBindLabel);
        grid.Widgets.Add(toggleEditModeKeyBindPreview);
        grid.Widgets.Add(changeToggleEditModeKeyBind);
        grid.Widgets.Add(screenshotKeyBindLabel);
        grid.Widgets.Add(screenshotKeyBindPreview);
        grid.Widgets.Add(changeScreenShotKeyBind);
        
        dialog.Content = grid;

        dialog.ButtonOk.Click += (s, e) =>
        {
            _settingsSaveData.PauseShortcut = 
                UpdateShortcut(settingsMenuData, _settingsSaveData.PauseShortcut, "PauseShortcut");
            _settingsSaveData.SpeedUpShortcut = 
                UpdateShortcut(settingsMenuData, _settingsSaveData.SpeedUpShortcut, "SpeedUpShortcut");
            _settingsSaveData.SpeedDownShortcut = 
                UpdateShortcut(settingsMenuData, _settingsSaveData.SpeedDownShortcut, "SpeedDownShortcut");
            _settingsSaveData.TrailsShortcut = 
                UpdateShortcut(settingsMenuData, _settingsSaveData.TrailsShortcut, "TrailsShortcut");
            _settingsSaveData.NamesShortcut = 
                UpdateShortcut(settingsMenuData, _settingsSaveData.NamesShortcut, "NamesShortcut");
            _settingsSaveData.GlowShortcut = 
                UpdateShortcut(settingsMenuData, _settingsSaveData.GlowShortcut, "GlowShortcut");
            _settingsSaveData.EditShortcut = 
                UpdateShortcut(settingsMenuData, _settingsSaveData.EditShortcut, "EditShortcut");
            _settingsSaveData.ScreenshotShortcut = 
                UpdateShortcut(settingsMenuData, _settingsSaveData.ScreenshotShortcut, "ScreenshotShortcut");

            game.SaveSystem.SaveSettings(_settingsSaveData);
        };

        return dialog;
    }
    
    private void RemapShortcut(SettingsMenuData settingsMenuData, Button button, Label label, string whichShortcut)
    {
        settingsMenuData.Remapping = !settingsMenuData.Remapping;
        settingsMenuData.WhichShortcut = whichShortcut;
        ((Label)button.Content).Text = settingsMenuData.Remapping ? "Finish" : "Start";
        if (settingsMenuData.Remapping)
        {
            settingsMenuData.ClearShortcut = true;
            label.Text = "Working...";
        }
        if (!settingsMenuData.Remapping)
        {
            label.Text = settingsMenuData.ShortcutPreview;
        }
    }

    private List<Keys> UpdateShortcut(SettingsMenuData settingsMenuData, List<Keys> oldShortcut, string shortcutName)
    {
        if (settingsMenuData.NewShortcuts.TryGetValue(shortcutName, out List<Keys> shortcut) && shortcut.Count > 0)
        {
            return shortcut;
        }
        
        return oldShortcut;
    }
    
    public void Draw()
    {
        _desktop.Render();
    }
}