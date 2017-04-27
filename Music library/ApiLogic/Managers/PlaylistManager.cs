using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Base;

namespace ApiLogic
{
    public class PlaylistManager : IPlaylistManager
    {
        private readonly HashSet<Playlist> _playlists;

        private static readonly string PLAYLIST_DEFAULT_PATH = "Playlists.xml";

        public PlaylistManager()
        {
            if (File.Exists(PLAYLIST_DEFAULT_PATH))
            {
                try
                {
                    IImportExport<Playlist> worker = new XMLImportExport<Playlist>();
                    _playlists = worker.Import(PLAYLIST_DEFAULT_PATH).ToHashSet();
                }
                catch
                {
                    _playlists = new HashSet<Playlist>();
                }
            }
            else
            {
                _playlists = new HashSet<Playlist>();
            }
        }

        public void CreatePlaylist(Playlist playlist)
        {
            _playlists.Add(playlist);
        }

        public void EditPlaylist(Playlist playlist)
        {
            Playlist oldplaylist = _playlists.First(x => x.Name == playlist.Name);
            _playlists.Remove(oldplaylist);
            _playlists.Add(playlist);
        }

        public void DeletePlaylist(string name)
        {
            _playlists.RemoveWhere(x => x.Name == name);
        }

        public Playlist GetPlaylistByName(string name)
        {
            return _playlists.First(x => String.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public HashSet<Playlist> GetAllPlaylists()
        {
            return _playlists;
        }

        public void AddSongToPlaylist(string namePlaylist, string song)
        {
            GetPlaylistByName(namePlaylist).Songs.Add(song);
        }

        public void RemoveSongFromPlaylist(string namePlaylist, string song)
        {
            GetPlaylistByName(namePlaylist).Songs.Remove(song);
        }

        public HashSet<Song> GetAllSongsInPlaylist(string name)
        {
            return GetPlaylistByName(name).Songs.Select(Song.New).ToHashSet();
        }

        public void ImportPlaylist(string path)
        {
            _playlists.UnionWith(new XMLImportExport<Playlist>().Import(path).ToHashSet());
        }

        public void SavePlaylist(string path)
        {
            new XMLImportExport<Playlist>().Export(_playlists, path);
        }

        public void Save()
        {
            SavePlaylist(PLAYLIST_DEFAULT_PATH);
        }

        public void ExportPlaylist(Playlist playlist, string path = null)
        {
            new XMLImportExport<Playlist>().Export(new HashSet<Playlist> {playlist}, path);
        }
    }
}
