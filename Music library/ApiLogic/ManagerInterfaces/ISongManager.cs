using System.Collections.Generic;
using System.Collections.ObjectModel;
using Base;

namespace ApiLogic
{
    public interface ISongManager
    {
        /// <summary>
        /// Adds song to the library
        /// </summary>
        void CreateSong(Song song);

        /// <summary>
        /// Removes song from library
        /// </summary>
        void RemoveSong(string path);

        /// <summary>
        /// Returns song according to its <paramref name="path"/>
        /// </summary>
        Song GetSong(string path);

        /// <summary>
        /// Returns first song match by <paramref name="name"/>.
        /// </summary>
        Song GetSongByName(string name);

        /// <summary>
        /// Returns all songs from library
        /// </summary>
        ReadOnlyCollection<Song> GetAllSongs();

        /// <summary>
        /// Deletes all cached songs in the memory (physical files will remain)
        /// </summary>
        void ClearSongs();
    }
}
