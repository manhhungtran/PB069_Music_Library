using System.Collections.Generic;
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
        /// Edits song parameters
        /// </summary>
        void EditSong(Song song);

        /// <summary>
        /// Removes song from library
        /// </summary>
        void RemoveSong(string path);

        /// <summary>
        /// Returns song according to its <paramref name="path"/>
        /// </summary>
        Song GetSong(string path);

        /// <summary>
        /// Returns all songs from library
        /// </summary>
        List<Song> GetAllSongs();

    }
}
