using System.Collections.Generic;
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
        void DeletePlaylist(string name);

        /// <summary>
        /// Returns playlist according to <paramref name="name"/>
        /// </summary>
        Playlist GetPlaylistByName(string name);

        /// <summary>
        /// Returns all playlists from library;
        /// </summary>
        HashSet<Playlist> GetAllPlaylists();

        /// <summary>
        /// Adds song to playlist
        /// </summary>
        void AddSongToPlaylist(string namePlaylist, string song);

        /// <summary>
        /// Removes song from playlist
        /// </summary>
        void RemoveSongFromPlaylist(string namePlaylist, string song);

        /// <summary>
        ///Returns all songs from playlist given by <paramref name="name"/>
        /// </summary>
        HashSet<Song> GetAllSongsInPlaylist(string name);

        /// <summary>
        /// Imports playlist from xml file.
        /// </summary>
        /// <param name="path"></param>
        void ImportPlaylist(string path);

        /// <summary>
        /// Exports one playlist to the xml file.
        /// </summary>
        /// <param name="playlist"></param>
        /// <param name="path"></param>
        void ExportPlaylist(Playlist playlist, string path = null);

        /// <summary>
        /// Exports all playlists to given xml file.
        /// </summary>
        void SavePlaylist(string path);

        /// <summary>
        /// Exports all playlists to the default file.
        /// </summary>
        void Save();


    }
}
