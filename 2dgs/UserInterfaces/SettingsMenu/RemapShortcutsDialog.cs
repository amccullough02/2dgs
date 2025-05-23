﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A class used to organise UI boilerplate for the dialog enabling keyboard shortcut remapping.
/// </summary>
public static class RemapShortcutsDialog
{
    /// <summary>
    /// A method that creates a 2DGS dialog with shortcut remapping functionality.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="settingsMediator">A reference to the SettingsMediator class.</param>
    /// <param name="settingsSaveData">A reference to the SettingsSaveData class.</param>
    /// <returns>The Shortcut Remapping dialog.</returns>
    public static Dialog Create(Game game, SettingsMediator settingsMediator, SettingsSaveData settingsSaveData)
    {
        var dialog = UiComponents.StyledDialog("Remap Keyboard Shortcuts");
        
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 3, 8);
        
        var pauseKeyBindLabel = UiComponents.LightLabel("Pause Shortcut");
        Grid.SetColumn(pauseKeyBindLabel, 0);

        var pauseKeyBindPreview =
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.PauseShortcut));
        Grid.SetColumn(pauseKeyBindPreview, 1);
        
        var changePauseKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changePauseKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changePauseKeyBind, pauseKeyBindPreview, "PauseShortcut");
        };
        Grid.SetColumn(changePauseKeyBind, 2);
        
        var speedUpKeyBindLabel = UiComponents.LightLabel("Sim Speed+");
        Grid.SetColumn(speedUpKeyBindLabel, 0);
        Grid.SetRow(speedUpKeyBindLabel, 1);
        
        var speedUpKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.SpeedUpShortcut));
        Grid.SetColumn(speedUpKeyBindPreview, 1);
        Grid.SetRow(speedUpKeyBindPreview, 1);
        
        var changeSpeedUpKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeSpeedUpKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeSpeedUpKeyBind, speedUpKeyBindPreview, "SpeedUpShortcut");
        };
        Grid.SetColumn(changeSpeedUpKeyBind, 2);
        Grid.SetRow(changeSpeedUpKeyBind, 1);
        
        var slowDownKeyBindLabel = UiComponents.LightLabel("Sim Speed-");
        Grid.SetColumn(slowDownKeyBindLabel, 0);
        Grid.SetRow(slowDownKeyBindLabel, 2);
        
        var slowDownKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.SpeedDownShortcut));
        Grid.SetColumn(slowDownKeyBindPreview, 1);
        Grid.SetRow(slowDownKeyBindPreview, 2);
        
        var changeSpeedDownKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeSpeedDownKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeSpeedDownKeyBind, slowDownKeyBindPreview, "SpeedDownShortcut");
        };
        Grid.SetColumn(changeSpeedDownKeyBind, 2);
        Grid.SetRow(changeSpeedDownKeyBind, 2);
        
        var toggleTrailsKeyBindLabel = UiComponents.LightLabel("Toggle Trails");
        Grid.SetColumn(toggleTrailsKeyBindLabel, 0);
        Grid.SetRow(toggleTrailsKeyBindLabel, 3);

        var toggleTrailsKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.TrailsShortcut));
        Grid.SetColumn(toggleTrailsKeyBindPreview, 1);
        Grid.SetRow(toggleTrailsKeyBindPreview, 3);
        
        var changeToggleTrailsKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleTrailsKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeToggleTrailsKeyBind, toggleTrailsKeyBindPreview, "TrailsShortcut");
        };
        Grid.SetColumn(changeToggleTrailsKeyBind, 2);
        Grid.SetRow(changeToggleTrailsKeyBind, 3);
        
        var toggleOrbitsKeyBindLabel = UiComponents.LightLabel("Toggle Orbits");
        Grid.SetColumn(toggleOrbitsKeyBindLabel, 0);
        Grid.SetRow(toggleOrbitsKeyBindLabel, 4);
        
        var toggleOrbitsKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.OrbitsShortcut));
        Grid.SetColumn(toggleOrbitsKeyBindPreview, 1);
        Grid.SetRow(toggleOrbitsKeyBindPreview, 4);
        
        var changeToggleOrbitsKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleOrbitsKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeToggleOrbitsKeyBind, toggleOrbitsKeyBindPreview, "OrbitsShortcut");
        };
        Grid.SetColumn(changeToggleOrbitsKeyBind, 2);
        Grid.SetRow(changeToggleOrbitsKeyBind, 4);
        
        var toggleVectorsKeyBindLabel = UiComponents.LightLabel("Toggle Vectors");
        Grid.SetColumn(toggleVectorsKeyBindLabel, 0);
        Grid.SetRow(toggleVectorsKeyBindLabel, 5);
        
        var toggleVectorsKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.VectorsShortcut));
        Grid.SetColumn(toggleVectorsKeyBindPreview, 1);
        Grid.SetRow(toggleVectorsKeyBindPreview, 5);
        
        var changeToggleVectorsKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleVectorsKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeToggleVectorsKeyBind, toggleVectorsKeyBindPreview, "VectorsShortcut");
        };
        Grid.SetColumn(changeToggleVectorsKeyBind, 2);
        Grid.SetRow(changeToggleVectorsKeyBind, 5);
        
        var toggleNamesKeyBindLabel = UiComponents.LightLabel("Toggle Names");
        Grid.SetColumn(toggleNamesKeyBindLabel, 0);
        Grid.SetRow(toggleNamesKeyBindLabel, 6);
        
        var toggleNamesKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.NamesShortcut));
        Grid.SetColumn(toggleNamesKeyBindPreview, 1);
        Grid.SetRow(toggleNamesKeyBindPreview, 6);
        
        var changeToggleNamesKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleNamesKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeToggleNamesKeyBind, toggleNamesKeyBindPreview, "NamesShortcut");
        };
        Grid.SetColumn(changeToggleNamesKeyBind, 2);
        Grid.SetRow(changeToggleNamesKeyBind, 6);
        
        var toggleGlowKeyBindLabel = UiComponents.LightLabel("Toggle Glow");
        Grid.SetColumn(toggleGlowKeyBindLabel, 0);
        Grid.SetRow(toggleGlowKeyBindLabel, 7);
        
        var toggleGlowKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.GlowShortcut));
        Grid.SetColumn(toggleGlowKeyBindPreview, 1);
        Grid.SetRow(toggleGlowKeyBindPreview, 7);
        
        var changeToggleGlowKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleGlowKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeToggleGlowKeyBind, toggleGlowKeyBindPreview, "GlowShortcut");
        };
        Grid.SetColumn(changeToggleGlowKeyBind, 2);
        Grid.SetRow(changeToggleGlowKeyBind, 7);
        
        var toggleEditModeKeyBindLabel = UiComponents.LightLabel("Edit Mode");
        Grid.SetColumn(toggleEditModeKeyBindLabel, 0);
        Grid.SetRow(toggleEditModeKeyBindLabel, 8);
        
        var toggleEditModeKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.EditShortcut));
        Grid.SetColumn(toggleEditModeKeyBindPreview, 1);
        Grid.SetRow(toggleEditModeKeyBindPreview, 8);
        
        var changeToggleEditModeKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeToggleEditModeKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeToggleEditModeKeyBind, toggleEditModeKeyBindPreview, "EditShortcut");
        };
        Grid.SetColumn(changeToggleEditModeKeyBind, 2);
        Grid.SetRow(changeToggleEditModeKeyBind, 8);

        var screenshotKeyBindLabel = UiComponents.LightLabel("Screenshot");
        Grid.SetColumn(screenshotKeyBindLabel, 0);
        Grid.SetRow(screenshotKeyBindLabel, 9);
        
        var screenshotKeyBindPreview = 
            UiComponents.KeyBindLabel(StringTransformer.KeyBindString(settingsSaveData.ScreenshotShortcut));
        Grid.SetColumn(screenshotKeyBindPreview, 1);
        Grid.SetRow(screenshotKeyBindPreview, 9);
        
        var changeScreenShotKeyBind = UiComponents.Button("Start", width: 100, height: 30);
        changeScreenShotKeyBind.Click += (_, _) =>
        {
            RemapShortcut(settingsMediator, changeScreenShotKeyBind, screenshotKeyBindPreview, "ScreenshotShortcut");
        };
        Grid.SetColumn(changeScreenShotKeyBind, 2);
        Grid.SetRow(changeScreenShotKeyBind, 9);
        
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
        grid.Widgets.Add(toggleOrbitsKeyBindLabel);
        grid.Widgets.Add(toggleOrbitsKeyBindPreview);
        grid.Widgets.Add(changeToggleOrbitsKeyBind);
        grid.Widgets.Add(toggleVectorsKeyBindLabel);
        grid.Widgets.Add(toggleVectorsKeyBindPreview);
        grid.Widgets.Add(changeToggleVectorsKeyBind);
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
        
        var verticalStackPanel = new VerticalStackPanel();

        var horizontalSeperator = UiComponents.HorizontalSeparator();
        
        var resetGrid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 1);
        resetGrid.ColumnSpacing = 70;

        var resetLabel = UiComponents.MediumLabel("Reset to Default Bindings");
        Grid.SetColumn(resetLabel, 0);
        
        var resetButton = UiComponents.Button("Reset", width: 100, height: 30);
        resetButton.Click += (_, _) =>
        {
            string GetPreview(string preview) => StringTransformer.KeyBindString(settingsMediator.DefaultShortcuts[preview]);

            pauseKeyBindPreview.Text = GetPreview("PauseShortcut");
            speedUpKeyBindPreview.Text = GetPreview("SpeedUpShortcut");
            slowDownKeyBindPreview.Text = GetPreview("SpeedDownShortcut");
            toggleTrailsKeyBindPreview.Text = GetPreview("TrailsShortcut");
            toggleOrbitsKeyBindPreview.Text = GetPreview("OrbitsShortcut");
            toggleVectorsKeyBindPreview.Text = GetPreview("VectorsShortcut");
            toggleNamesKeyBindPreview.Text = GetPreview("NamesShortcut");
            toggleGlowKeyBindPreview.Text = GetPreview("GlowShortcut");
            toggleEditModeKeyBindPreview.Text = GetPreview("EditShortcut");
            screenshotKeyBindPreview.Text = GetPreview("ScreenshotShortcut");
            settingsMediator.ResetShortcuts = true;
            resetButton.Enabled = false;
        };
        Grid.SetColumn(resetButton, 1);
        
        resetGrid.Widgets.Add(resetLabel);
        resetGrid.Widgets.Add(resetButton);
        
        verticalStackPanel.Widgets.Add(grid);
        verticalStackPanel.Widgets.Add(horizontalSeperator);
        verticalStackPanel.Widgets.Add(resetGrid);
        
        dialog.Content = verticalStackPanel;

        dialog.ButtonOk.Click += (_, _) =>
        {
            if (settingsMediator.ResetShortcuts)
            {
                UpdateShortcuts(settingsMediator.DefaultShortcuts, settingsSaveData);
                game.SaveSystem.SaveSettings(settingsSaveData);
                settingsMediator.ResetShortcuts = false;
                resetButton.Enabled = true;
            }
            else
            {
                UpdateShortcuts(settingsMediator.NewShortcuts, settingsSaveData);
                game.SaveSystem.SaveSettings(settingsSaveData);
            }
        };

        return dialog;
    }
    
    /// <summary>
    /// A method called when a shortcut is being remapped, responsible for visual changes.
    /// </summary>
    /// <param name="settingsMediator">A reference to the SettingsMediator class.</param>
    /// <param name="button">The button clicked to start the remapping process, its text will change appropriately depending on the state of the remapping
    /// (start or finish).</param>
    /// <param name="label">The label that provides a preview of what the new shortcut will consist of.</param>
    /// <param name="whichShortcut">A text ID of the shortcut that is being remapped.</param>
    private static void RemapShortcut(SettingsMediator settingsMediator, Button button, Label label, string whichShortcut)
    {
        settingsMediator.Remapping = !settingsMediator.Remapping;
        settingsMediator.WhichShortcut = whichShortcut;
        ((Label)button.Content).Text = settingsMediator.Remapping ? "Finish" : "Start";
        if (settingsMediator.Remapping)
        {
            settingsMediator.ClearShortcut = true;
            label.Text = "Working...";
        }
        if (!settingsMediator.Remapping)
        {
            label.Text = settingsMediator.ShortcutPreview;
        }
    }

    /// <summary>
    /// A method that updates the saved shortcuts via reflection.
    /// </summary>
    /// <param name="dictionary">The dictionary containing the new set of shortcuts.</param>
    /// <param name="settingsSaveData">A reference to the SettingsSaveData class.</param>
    private static void UpdateShortcuts(Dictionary<string, List<Keys>> dictionary, SettingsSaveData settingsSaveData)
    {
        var type = settingsSaveData.GetType();
        
        foreach (var shortcut in dictionary)
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