using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class RemapShortcutsDialog
{
    public static Dialog Create(Game game, SettingsMenuData settingsMenuData, SettingsSaveData settingsSaveData)
    {
        var dialog = UiComponents.StyledDialog("Remap Keyboard Shortcuts");
        
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 3, 8);
        
        var pauseKeyBindLabel = UiComponents.LightLabel("Pause Shortcut");
        Grid.SetColumn(pauseKeyBindLabel, 0);

        var pauseKeyBindPreview =
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(settingsSaveData.PauseShortcut));
        Grid.SetColumn(pauseKeyBindPreview, 1);
        
        var changePauseKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changePauseKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMenuData, changePauseKeyBind, pauseKeyBindPreview, "PauseShortcut");
        };
        Grid.SetColumn(changePauseKeyBind, 2);
        
        var speedUpKeyBindLabel = UiComponents.LightLabel("Sim Speed+");
        Grid.SetColumn(speedUpKeyBindLabel, 0);
        Grid.SetRow(speedUpKeyBindLabel, 1);
        
        var speedUpKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(settingsSaveData.SpeedUpShortcut));
        Grid.SetColumn(speedUpKeyBindPreview, 1);
        Grid.SetRow(speedUpKeyBindPreview, 1);
        
        var changeSpeedUpKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeSpeedUpKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMenuData, changeSpeedUpKeyBind, speedUpKeyBindPreview, "SpeedUpShortcut");
        };
        Grid.SetColumn(changeSpeedUpKeyBind, 2);
        Grid.SetRow(changeSpeedUpKeyBind, 1);
        
        var slowDownKeyBindLabel = UiComponents.LightLabel("Sim Speed-");
        Grid.SetColumn(slowDownKeyBindLabel, 0);
        Grid.SetRow(slowDownKeyBindLabel, 2);
        
        var slowDownKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(settingsSaveData.SpeedDownShortcut));
        Grid.SetColumn(slowDownKeyBindPreview, 1);
        Grid.SetRow(slowDownKeyBindPreview, 2);
        
        var changeSpeedDownKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeSpeedDownKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMenuData, changeSpeedDownKeyBind, slowDownKeyBindPreview, "SpeedDownShortcut");
        };
        Grid.SetColumn(changeSpeedDownKeyBind, 2);
        Grid.SetRow(changeSpeedDownKeyBind, 2);
        
        var toggleTrailsKeyBindLabel = UiComponents.LightLabel("Toggle Trails");
        Grid.SetColumn(toggleTrailsKeyBindLabel, 0);
        Grid.SetRow(toggleTrailsKeyBindLabel, 3);

        var toggleTrailsKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(settingsSaveData.TrailsShortcut));
        Grid.SetColumn(toggleTrailsKeyBindPreview, 1);
        Grid.SetRow(toggleTrailsKeyBindPreview, 3);
        
        var changeToggleTrailsKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleTrailsKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMenuData, changeToggleTrailsKeyBind, toggleTrailsKeyBindPreview, "TrailsShortcut");
        };
        Grid.SetColumn(changeToggleTrailsKeyBind, 2);
        Grid.SetRow(changeToggleTrailsKeyBind, 3);
        
        var toggleNamesKeyBindLabel = UiComponents.LightLabel("Toggle Names");
        Grid.SetColumn(toggleNamesKeyBindLabel, 0);
        Grid.SetRow(toggleNamesKeyBindLabel, 4);
        
        var toggleNamesKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(settingsSaveData.NamesShortcut));
        Grid.SetColumn(toggleNamesKeyBindPreview, 1);
        Grid.SetRow(toggleNamesKeyBindPreview, 4);
        
        var changeToggleNamesKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleNamesKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMenuData, changeToggleNamesKeyBind, toggleNamesKeyBindPreview, "NamesShortcut");
        };
        Grid.SetColumn(changeToggleNamesKeyBind, 2);
        Grid.SetRow(changeToggleNamesKeyBind, 4);
        
        var toggleGlowKeyBindLabel = UiComponents.LightLabel("Toggle Glow");
        Grid.SetColumn(toggleGlowKeyBindLabel, 0);
        Grid.SetRow(toggleGlowKeyBindLabel, 5);
        
        var toggleGlowKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(settingsSaveData.GlowShortcut));
        Grid.SetColumn(toggleGlowKeyBindPreview, 1);
        Grid.SetRow(toggleGlowKeyBindPreview, 5);
        
        var changeToggleGlowKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleGlowKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMenuData, changeToggleGlowKeyBind, toggleGlowKeyBindPreview, "GlowShortcut");
        };
        Grid.SetColumn(changeToggleGlowKeyBind, 2);
        Grid.SetRow(changeToggleGlowKeyBind, 5);
        
        var toggleEditModeKeyBindLabel = UiComponents.LightLabel("Edit Mode");
        Grid.SetColumn(toggleEditModeKeyBindLabel, 0);
        Grid.SetRow(toggleEditModeKeyBindLabel, 6);
        
        var toggleEditModeKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(settingsSaveData.EditShortcut));
        Grid.SetColumn(toggleEditModeKeyBindPreview, 1);
        Grid.SetRow(toggleEditModeKeyBindPreview, 6);
        
        var changeToggleEditModeKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleEditModeKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMenuData, changeToggleEditModeKeyBind, toggleEditModeKeyBindPreview, "EditShortcut");
        };
        Grid.SetColumn(changeToggleEditModeKeyBind, 2);
        Grid.SetRow(changeToggleEditModeKeyBind, 6);

        var screenshotKeyBindLabel = UiComponents.LightLabel("Screenshot");
        Grid.SetColumn(screenshotKeyBindLabel, 0);
        Grid.SetRow(screenshotKeyBindLabel, 7);
        
        var screenshotKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeybindString(settingsSaveData.ScreenshotShortcut));
        Grid.SetColumn(screenshotKeyBindPreview, 1);
        Grid.SetRow(screenshotKeyBindPreview, 7);
        
        var changeScreenShotKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeScreenShotKeyBind.Click += (_, _) =>
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

        dialog.ButtonOk.Click += (_, _) =>
        {
            UpdateShortcuts(settingsMenuData, settingsSaveData);
            game.SaveSystem.SaveSettings(settingsSaveData);
        };

        return dialog;
    }
    
    private static void RemapShortcut(SettingsMenuData settingsMenuData, Button button, Label label, string whichShortcut)
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

    private static void UpdateShortcuts(SettingsMenuData settingsMenuData, SettingsSaveData settingsSaveData)
    {
        var type = settingsSaveData.GetType();
        
        foreach (var shortcut in settingsMenuData.NewShortcuts)
        {
            if (shortcut.Value.Count == 0) continue;
            
            var property = type.GetProperty(shortcut.Key);
            
            if (property != null && property.CanWrite && property.PropertyType == typeof(List<Keys>))
            {
                property.SetValue(settingsSaveData, shortcut.Value);    
            }
        }
    }
}