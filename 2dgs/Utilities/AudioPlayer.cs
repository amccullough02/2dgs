using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace _2dgs;

public class AudioPlayer(ContentManager contentManager)
{
    private readonly Song _bgm = contentManager.Load<Song>("audio/perfect_beauty_zakhar_valaha");

    public void PlayBgm()
    {
        if (MediaPlayer.State != MediaState.Stopped) MediaPlayer.Stop();
        MediaPlayer.Volume = 0.2f;
        MediaPlayer.Play(_bgm);
    }

    public void Dispose()
    {
        if (MediaPlayer.State != MediaState.Stopped) MediaPlayer.Stop();
        _bgm.Dispose();
    }
}