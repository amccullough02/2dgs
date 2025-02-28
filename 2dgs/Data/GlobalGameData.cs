namespace _2dgs;

/// <summary>
/// Class used to store globally accessible values.
/// </summary>
public static class GlobalGameData
{
    /// <summary>
    /// The default volume of sound effects in 2DGS.
    /// </summary>
    public static float SfxVolume { get; set; } = 0.1f;
    /// <summary>
    /// The default fade speed for fade in and fade out transitions.
    /// </summary>
    public const float FadeSpeed = 5f;
}