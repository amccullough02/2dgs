using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace _2dgs;

public class SoundEffectPlayer
{
    private readonly SoundEffectInstance _collisionInstance;

    public SoundEffectPlayer(ContentManager contentManager)
    {
        var collision = contentManager.Load<SoundEffect>("audio/explosion_sfx");
        _collisionInstance = collision.CreateInstance();
        _collisionInstance.Volume = GlobalGameData.SfxVolume;
    }
    
    public void PlayCollisionSfx()
    {
        _collisionInstance.Play();
    }
}