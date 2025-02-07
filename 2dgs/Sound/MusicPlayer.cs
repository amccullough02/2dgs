using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace _2dgs;

public class MusicPlayer(ContentManager contentManager)
{
    private readonly Song _bgm1 = contentManager.Load<Song>("audio/origami_hairball");
    private readonly Song _bgm2 = contentManager.Load<Song>("audio/long_way_down");
    private readonly Song _bgm3 = contentManager.Load<Song>("audio/suspended_in_air");
    private List<Song> _songs;
    private int _currentSongIndex;

    public void Initialize()
    {
        _currentSongIndex = 0;
        MediaPlayer.Volume = 0.1f;
        MediaPlayer.MediaStateChanged += (_, _) =>
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                NextSong();
            }
        };
        
        _songs = [_bgm1, _bgm2, _bgm3];
        NextSong();
    }

    private void NextSong()
    {
        MediaPlayer.Play(_songs[_currentSongIndex]);
        _currentSongIndex = (_currentSongIndex + 1) % _songs.Count;
    }

    public void Dispose()
    {
        if (MediaPlayer.State != MediaState.Stopped) MediaPlayer.Stop();
        _bgm1.Dispose();
    }
}