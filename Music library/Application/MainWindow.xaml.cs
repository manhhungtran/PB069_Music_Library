using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
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
        private bool _shuffle = false;
        private bool _repeat = false;

        public MainWindow()
        {
            InitializeComponent();

            _libraryManager = new LibraryManager();
            _playlistManager = new PlaylistManager();
           _libraryManager.InitializeLibrary();
        }

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
        }

        private void ShowPlaylists(object sender, RoutedEventArgs e)
        {
            List.DataContext = _playlistManager.GetAllPlaylists();
        }

        private void ImportPlaylist(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {DefaultExt = "xml"};
            if (dialog.ShowDialog() == true)
            {
                _playlistManager.ImportPlaylist(dialog.FileName);
            }
        }

        private void ExportPlaylist(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                _playlistManager.SavePlaylist(dialog.FileName);
            }
        }

        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            MyPlayer.Source = new Uri(
                _libraryManager.GetSongManager()
                    .GetAllSongs()
                    .First(songName => songName.Name == DetailListing.SelectedItem.ToString())
                    .Path
                );
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

        private void LibraryList(object sender, RoutedEventArgs e)
        {
            List.Items.Clear();
            DetailListing.Items.Clear();

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

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    _libraryManager.AddRootPath(dialog.SelectedPath);
                    LibraryList(sender, e);
                }
                catch
                {
                    // ignored
                }
            }

        }

        private void RemoveRootFolder(object sender, RoutedEventArgs e)
        {
            _libraryManager.RemoveRootPath(List.SelectedItem.ToString());
            LibraryList(sender, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _libraryManager.Save();
            System.Windows.Application.Current.Shutdown(0);
        }
    }
}
