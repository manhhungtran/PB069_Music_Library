using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using ApiLogic;
using Base;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ILibraryManager _libraryManager;
        private readonly IPlaylistManager _playlistManager;
        private bool _shuffle;
        private bool _repeat;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                _libraryManager = new LibraryManager();
                _playlistManager = new PlaylistManager();

                _libraryManager.InitializeLibrary();
            }
            catch (Exception ex)
            {
                new AlertMessage(ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        #region PlayerStuff

        private void ButtonStop_OnClick(object sender, RoutedEventArgs e)
        {
            MyPlayer.Stop();
            MyPlayer.Source = null;
        }

        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DetailListing.SelectedItem == null || MyPlayer == null) return;

                Song selectedSong =
                    _libraryManager
                    .GetSongManager()
                    .GetSongByName(DetailListing.SelectedItem.ToString());

                MyPlayer.Source = new Uri(selectedSong.Path);
                MyPlayer.Play();
            }
            catch (Exception ex)
            {
                new AlertMessage(ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void ButtonPause_OnClick(object sender, RoutedEventArgs e)
        {
            MyPlayer.Pause();
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            if (_shuffle)
            {
                DetailListing.SelectedIndex = new Random().Next(0, DetailListing.Items.Count);
            }
            else if (DetailListing.SelectedIndex < DetailListing.Items.Count - 1)
            {
                DetailListing.SelectedIndex = DetailListing.SelectedIndex + 1;
            }
            else if (_repeat)
            {
                DetailListing.SelectedIndex = 0;
            }
            ButtonPlay_OnClick(sender, e);
        }

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
        {
            if (DetailListing.SelectedIndex > 0)
            {
                DetailListing.SelectedIndex = DetailListing.SelectedIndex - 1;
            }
            ButtonPlay_OnClick(sender, e);
        }

        private void ButtonShuffle_OnClick(object sender, RoutedEventArgs e)
        {
            MenuShuffle.IsChecked = !MenuShuffle.IsChecked;
            ButtonShuffle.Background = _shuffle ? Brushes.White : Brushes.DarkGray;
            _shuffle = !_shuffle;

        }

        private void ButtonRepeat_OnClick(object sender, RoutedEventArgs e)
        {
            MenuRepeat.IsChecked = !MenuRepeat.IsChecked;
            ButtonRepeat.Background = _repeat ? Brushes.White : Brushes.DarkGray;
            _repeat = !_repeat;
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MyPlayer != null)
            {
                MyPlayer.Volume = ButtonVolumeSlider?.Value ?? 0;
            }
        }

        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int sliderValue = (int)ButtonTimelineSlider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, sliderValue);
            MyPlayer.Position = ts;
        }

        private void UpdateLabel(object sender, EventArgs e)
        {
            if (MyPlayer.NaturalDuration.HasTimeSpan)
            {
                ButtonTimelineSlider.Maximum = MyPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                ButtonTimelineSlider.Value = 0;
            }
        }

        #endregion

        #region PlaylistStuff

        private void NewPlaylist(object sender, RoutedEventArgs e)
        {
            var dialog = new AddPlaylist();
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                var playlist = new Playlist
                {
                    Name = dialog.TextBox.Text,
                    Songs = new List<string>()
                };
                _playlistManager.CreatePlaylist(playlist);
            }
            ShowPlaylists(sender, e);
        }

        private void ShowPlaylists(object sender, RoutedEventArgs e)
        {
            List.Items.Clear();
            DetailListing.Items.Clear();

            MenuShowPlaylists.IsChecked = true;
            MenuShowLibraryList.IsChecked = false;

            foreach (Playlist playlist in _playlistManager.GetAllPlaylists())
            {
                List.Items.Add(playlist);
            }
        }

        private void ImportPlaylist(object sender, RoutedEventArgs e)
        {
            if (!MenuShowPlaylists.IsChecked) return;

            try
            {
                OpenFileDialog dialog = new OpenFileDialog {DefaultExt = "xml"};
                if (dialog.ShowDialog() == true)
                {
                    _playlistManager.ImportPlaylist(dialog.FileName);
                }

                ShowPlaylists(sender, e);
            }
            catch (Exception ex)
            {
                new AlertMessage(ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void ExportPlaylist(object sender, RoutedEventArgs e)
        {
            if (!MenuShowPlaylists.IsChecked) return;

            try
            {
                SaveFileDialog dialog = new SaveFileDialog {DefaultExt = "xml"};
                if (dialog.ShowDialog() == true)
                {
                    _playlistManager.ExportPlaylist(
                        _playlistManager.GetPlaylistByName(List.SelectedItem.ToString()),
                        dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                new AlertMessage(ex.Message, ex.StackTrace).ShowDialog();
            }
        }


        private void OpenAllSongsFromPlaylist(object sender, MouseButtonEventArgs e)
        {
            if (MenuShowPlaylists.IsChecked)
            {
                Playlist selectedPlaylist = _playlistManager.GetPlaylistByName(List.SelectedItem.ToString());

                DetailListing.Items.Clear();

                foreach (string song in selectedPlaylist.Songs)
                {
                    DetailListing.Items.Add(_libraryManager.GetSongManager().GetSong(song).Name);
                }
            }
        }

        private void AddSongToPlaylist(object sender, RoutedEventArgs e)
        {
            AddSongToPlaylist dialog = new AddSongToPlaylist(_playlistManager);
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                try
                {
                    string namePlaylist = dialog.ListBox.SelectedItem.ToString();
                    string pathToSong = _libraryManager.GetSongManager().GetSongByName(DetailListing.SelectedItem.ToString()).Path;

                    _playlistManager.AddSongToPlaylist(namePlaylist, pathToSong);
                }
                catch (Exception ex)
                {
                    new AlertMessage(ex.Message, ex.StackTrace).ShowDialog();
                }
            }
        }

        private void DeleteSelectedPlaylist(object sender, RoutedEventArgs e)
        {
            if (MenuShowPlaylists.IsChecked && List.SelectedItem != null)
            {
                _playlistManager.DeletePlaylist(List.SelectedItem.ToString());
                ShowPlaylists(sender, e);
            }
            else
            {
                new AlertMessage("You must select playlist first.", "").ShowDialog();
            }
        }

        #endregion

        #region LibraryStuff

        private void ShowLibraryList(object sender, RoutedEventArgs e)
        {
            List.Items.Clear();
            DetailListing.Items.Clear();

            MenuShowLibraryList.IsChecked = true;
            MenuShowPlaylists.IsChecked = false;

            foreach (string root in _libraryManager.GetAllRoots())
            {
                List.Items.Add(root);
            }

            foreach (Song song in _libraryManager.GetSongManager().GetAllSongs())
            {
                DetailListing.Items.Add(song);
            }
        }

        private void AddRootFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            try
            {
                if (MyPlayer.Source != null)
                {
                    new AlertMessage("You should stop media player first.", String.Empty).ShowDialog();
                    return;
                }
                _libraryManager.AddRootPath(dialog.SelectedPath);
                ShowLibraryList(sender, e);
            }
            catch (Exception ex)
            {
                new AlertMessage(ex.Message, ex.StackTrace).ShowDialog();
            }
        }

        private void RemoveRootFolder(object sender, RoutedEventArgs e)
        {
            if (List.SelectedItem == null || !MenuShowLibraryList.IsChecked)
            {
                new AlertMessage("You must pick some folder first.", String.Empty).ShowDialog();
                return;
            }

            if (MyPlayer.Source != null)
            {
                new AlertMessage("You should stop media player first.", String.Empty).ShowDialog();
                return;
            }

            _libraryManager.RemoveRootPath(List.SelectedItem.ToString());
            ShowLibraryList(sender, e);
        }

        #endregion

        #region General events

        private void ListDelete(object sender, RoutedEventArgs e)
        {
            if (MenuShowLibraryList.IsChecked)
            {
                RemoveRootFolder(sender, e);
            }

            if (MenuShowPlaylists.IsChecked)
            {
                DeleteSelectedPlaylist(sender, e);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _libraryManager.Save();
            _playlistManager.Save();
            System.Windows.Application.Current.Shutdown(0);
        }

        private void ShowSongDetails(object sender, EventArgs e)
        {
            Song selectedSong = _libraryManager
                .GetSongManager()
                .GetAllSongs()
                .First(song => song.Name == DetailListing.SelectedItem.ToString());

            new AlertMessage(selectedSong.ToStringAll(), String.Empty).ShowDialog();
        }

        #endregion

    }
}
