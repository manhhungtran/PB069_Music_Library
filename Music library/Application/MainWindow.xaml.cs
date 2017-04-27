using System;
using System.Linq;
using System.Windows;
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
            InitializeComponent();

            _libraryManager = new LibraryManager();
            _playlistManager = new PlaylistManager();

            _libraryManager.InitializeLibrary();

            MyPlayer.Volume = ButtonVolumeSlider.Value;
        }

        #region PlayerStuff

        private void ButtonStop_OnClick(object sender, RoutedEventArgs e)
        {
            MyPlayer.Stop();
        }

        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            string pathOfSelectedSong = _libraryManager.GetSongManager()
                .GetSongByName(DetailListing.SelectedItem.ToString())
                .Path;

            MyPlayer.Source = new Uri(pathOfSelectedSong);
            MyPlayer.Play();
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
            MyPlayer.Volume = ButtonVolumeSlider.Value;
        }

        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int sliderValue = (int)ButtonTimelineSlider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, sliderValue);
            MyPlayer.Position = ts;
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
                    Name = dialog.TextBox.Text
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
                    _playlistManager.AddSongToPlaylist(
                        dialog.ListBox.SelectedItem.ToString(),
                        _libraryManager.GetSongManager()
                            .GetSongByName(DetailListing.SelectedItem.ToString())
                            .Path);
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
            _libraryManager.RemoveRootPath(List.SelectedItem.ToString());
            ShowLibraryList(sender, e);
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            _libraryManager.Save();
            _playlistManager.Save();
            System.Windows.Application.Current.Shutdown(0);
        }

        private void ShowSongDetails(object sender, MouseButtonEventArgs e)
        {
            Song selectedSong = _libraryManager
                .GetSongManager()
                .GetAllSongs()
                .First(song => song.Name == DetailListing.SelectedItem.ToString());

            new AlertMessage(selectedSong.ToStringAll(), String.Empty).ShowDialog();
        }
    }
}
