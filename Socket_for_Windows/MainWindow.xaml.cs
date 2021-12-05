using System.Windows;

namespace Socket_for_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Network.refreshHost();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            General.isClosing = true;
        }
    }
}
