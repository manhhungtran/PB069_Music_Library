using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace ApiLogic
{
    public interface IPlaylistManager
    {
        /// <summary>
        /// Creates new playlist to library
        /// </summary>
        void CreatePlaylist(Playlist playlist);

        /// <summary>
        /// Edits playlist
        /// </summary>
        void EditPlaylist(Playlist playlist);

        /// <summary>
        /// Removes playlist from library
        /// </summary>
        void DeletePlaylist(int id);

        /// <summary>
        /// Returns playlist according to <paramref name="name"/>
        /// </summary>
        Playlist GetPlaylistByName(string name);

        /// <summary>
        /// Returns playlist according to <paramref name="id"/>
        /// </summary>
        Playlist GetPlaylistById(int id);

        /// <summary>
        /// Returns all playlists from library;
        /// </summary>
        List<Playlist> GetAllPlaylists();

        /// <summary>
        /// Adds song to playlist
        /// </summary>
        void AddSongToPlaylist(int idPlaylist, string song);

        /// <summary>
        /// Removes song from playlist
        /// </summary>
        void RemoveSongFromPlaylist(int idPlaylist, string song);

        /// <summary>
        ///Returns all songs from playlist given by <paramref name="id"/>
        /// </summary>
        List<Song> GetAllSongsInPlaylist(int id);

        /// <summary>
        ///Returns all songs from playlist given by <paramref name="name"/>
        /// </summary>
        List<Song> GetAllSongsInPlaylist(string name);

        /// <summary>
        /// Imports playlist from xml file.
        /// </summary>
        /// <param name="path"></param>
        void ImportPlaylist(string path);

        /// <summary>
        /// Exports playlist to xml file.
        /// </summary>
        void SavePlaylist(string path);
    }
}
