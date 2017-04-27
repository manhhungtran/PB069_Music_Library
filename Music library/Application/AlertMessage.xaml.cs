using System.Text;
using System.Windows;

namespace Application
{
    /// <summary>
    /// Interaction logic for AlertMessage.xaml
    /// </summary>
    public partial class AlertMessage
    {
        public AlertMessage()
        {
            InitializeComponent();
        }

        public AlertMessage(string message, string stackTrace)
        {
            InitializeComponent();

            var result = new StringBuilder();
            result.Append(message);
            result.AppendLine();
            result.AppendLine("--------------");
            result.Append(stackTrace);

            Message.Content = result.ToString();
        }

        private void CloseThis(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
