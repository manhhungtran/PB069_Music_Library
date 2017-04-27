using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ApiLogic;
using Base;

namespace Application
{
    /// <summary>
    /// Interaction logic for AddSongToPlaylist.xaml
    /// </summary>
    public partial class AddSongToPlaylist
    {
        public AddSongToPlaylist(IPlaylistManager manager)
        {
            InitializeComponent();

            foreach (Playlist playlist in manager.GetAllPlaylists())
            {
                ListBox.Items.Add(playlist.Name);
            }
        }

        private void SelectThisPlaylist(object sender, RoutedEventArgs routedEventArgs)
        {
            DialogResult = ListBox.SelectedItem != null;
        }
    }
}
