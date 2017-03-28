using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Base;

namespace ApiLogic
{
    public class SongManager : ISongManager
    {
        private readonly List<Song> _songs;

        public SongManager()
        {
            _songs = new List<Song>();
        }

        public void CreateSong(Song song)
        {
            _songs.Add(song);
        }

        public void RemoveSong(string path)
        {
            _songs.Remove(GetSong(path));
        }

        public Song GetSong(string path)
        {
            return _songs.First(song => song.Path == path);
        }

        public ReadOnlyCollection<Song> GetAllSongs()
        {
            return _songs.AsReadOnly();
        }
    }
}
