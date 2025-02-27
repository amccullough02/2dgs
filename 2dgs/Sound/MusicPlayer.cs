using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace _2dgs;

/// <summary>
/// Used to manage the background music in the 2DGS application.
/// </summary>
/// <param name="contentManager">A reference to MonoGame's ContentManager class.</param>
public class MusicPlayer(ContentManager contentManager)
{
    /// <summary>
    /// The first song in the playlist: Origami Hairball by Krzysztof Pikes
    /// </summary>
    private readonly Song _bgm1 = contentManager.Load<Song>("audio/origami_hairball");
    /// <summary>
    /// The second song in the playlist: Long Way Down by Krzysztof Pikes
    /// </summary>
    private readonly Song _bgm2 = contentManager.Load<Song>("audio/long_way_down");
    /// <summary>
    /// The third song in the playlist: Suspended In Air by Krzysztof Pikes
    /// </summary>
    private readonly Song _bgm3 = contentManager.Load<Song>("audio/suspended_in_air");
    /// <summary>
    /// A list of all the songs.
    /// </summary>
    private List<Song> _songs;
    /// <summary>
    /// The index of the song currently being played.
    /// </summary>
    private int _currentSongIndex;

    /// <summary>
    /// Initializes the class (the MusicPlayer class is intended to be implemented as a singleton).
    /// </summary>
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

    /// <summary>
    /// Transitions to the next song in the playlist.
    /// </summary>
    private void NextSong()
    {
        MediaPlayer.Play(_songs[_currentSongIndex]);
        _currentSongIndex = (_currentSongIndex + 1) % _songs.Count;
    }

    /// <summary>
    /// Disposes the MusicPlayer object, called when the game is closed.
    /// </summary>
    public void Dispose()
    {
        if (MediaPlayer.State != MediaState.Stopped) MediaPlayer.Stop();
        _bgm1.Dispose();
    }
}