using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace _2dgs;

/// <summary>
/// Used to play sound effects in the 2DGS application.
/// </summary>
public class SoundEffectPlayer
{
    /// <summary>
    /// An instance of the collision sound effect that plays during body collisions/deletions.
    /// </summary>
    private readonly SoundEffectInstance _collisionInstance;

    /// <summary>
    /// A constructor for the SoundEffect Player.
    /// </summary>
    /// <param name="contentManager">A reference to MonoGame's ContentManager.</param>
    public SoundEffectPlayer(ContentManager contentManager)
    {
        var collision = contentManager.Load<SoundEffect>("audio/explosion_sfx");
        _collisionInstance = collision.CreateInstance();
        _collisionInstance.Volume = GlobalGameData.SfxVolume;
    }
    
    /// <summary>
    /// A public method to play the collision sound effect.
    /// </summary>
    public void PlayCollisionSfx()
    {
        _collisionInstance.Play();
    }
}