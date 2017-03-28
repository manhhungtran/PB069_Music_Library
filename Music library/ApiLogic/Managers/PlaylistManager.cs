using System.Collections.Generic;
using System.Linq;
using Base;

namespace ApiLogic
{
    public class PlaylistManager : IPlaylistManager
    {
        private readonly HashSet<Playlist> _playlists;

        public PlaylistManager()
        {
            _playlists = new HashSet<Playlist>();
        }

        public PlaylistManager(string path)
        {
            IImportExport<Playlist> worker = new XMLImportExport<Playlist>();
            _playlists = worker.Import(path).ToHashSet();
        }

        public void CreatePlaylist(Playlist playlist)
        {
            _playlists.Add(playlist);
        }

        public void EditPlaylist(Playlist playlist)
        {
            Playlist oldplaylist = _playlists.First(x => x.Id == playlist.Id);
            _playlists.Remove(oldplaylist);
            _playlists.Add(playlist);
        }

        public void DeletePlaylist(int id)
        {
            _playlists.RemoveWhere(x => x.Id == id);
        }

        public Playlist GetPlaylistByName(string name)
        {
            return _playlists.First(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public Playlist GetPlaylistById(int id)
        {
            return _playlists.First(x => x.Id == id);
        }

        public HashSet<Playlist> GetAllPlaylists()
        {
            return _playlists;
        }

        public void AddSongToPlaylist(int idPlaylist, string song)
        {
            GetPlaylistById(idPlaylist).Songs.Add(song);
        }

        public void RemoveSongFromPlaylist(int idPlaylist, string song)
        {
            GetPlaylistById(idPlaylist).Songs.Remove(song);
        }

        public HashSet<Song> GetAllSongsInPlaylist(int id)
        {
            return GetPlaylistById(id).Songs.Select(Song.New).ToHashSet();
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
    }
}
