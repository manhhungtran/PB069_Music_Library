using System;
using System.Windows;

namespace Application
{
    /// <summary>
    /// Interaction logic for AddPlaylist.xaml
    /// </summary>
    public partial class AddPlaylist
    {
        public AddPlaylist()
        {
            InitializeComponent();
        }

        private void Cancelino(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirmerino(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBox.Text))
            {
                DialogResult = true;
                return;
            }
            DialogResult = false;
        }
    }
}
