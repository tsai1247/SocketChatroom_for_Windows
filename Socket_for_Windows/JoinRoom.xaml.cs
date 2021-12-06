using Mono.Data.Sqlite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Socket_for_Windows
{
    /// <summary>
    /// JoinRoom.xaml 的互動邏輯
    /// </summary>
    public partial class JoinRoom : UserControl
    {
        public JoinRoom()
        {
            InitializeComponent();
        }
        
        private void Join_Click(object sender, RoutedEventArgs e)
        {
            Address address = new Address(Host.Text, Port.Text);
            if (!address.IsValid()) return;
            if (General.clientRoom.ContainsKey(address)) return;
            OneClientRoomInfo oneClientRoomInfo = new OneClientRoomInfo(address);
            General.GetMainWindow().chatRoomList.AddNewRoom(address, oneClientRoomInfo);
            General.GetMainWindow().chatRoomList.nickyName.IsEnabled = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var host = Network.localHost.Split(".");
            Host.Text = Network.localHost;
            //Host.Text = string.Format("{0}.{1}.", host[0], host[1]);
        }

        bool isChangingBG = false;
        private void Background_Click(object sender, RoutedEventArgs e)
        {
            if (!isChangingBG)
            {
                isChangingBG = true;
                ColorPicker color = new ColorPicker();
                color.Width = 60;
                color.Height = 50;
                color.FontSize = 25;
                color.SelectedColor = Transfer.Brush2Color(mainGrid.Background);
                color.SelectedColorChanged += Color_SelectedColorChanged;
                mainGrid.Children.Add(color);
            }
        }

        private void Color_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color color = (Color)(sender as ColorPicker).SelectedColor;
            ChangeColor(sender, color);

        }

        public void ChangeColor(object sender, Color color)
        {
            mainGrid.Background = Transfer.Color2Brush(color);

            if (IsDark(color))
            {
                script_Host.Foreground = Transfer.Color2Brush(Color.FromRgb(0, 0, 0));
                script_Port.Foreground = Transfer.Color2Brush(Color.FromRgb(0, 0, 0));
            }
            else
            {
                script_Host.Foreground = Transfer.Color2Brush(Color.FromRgb(255, 255, 255));
                script_Port.Foreground = Transfer.Color2Brush(Color.FromRgb(255, 255, 255));
            }

            if (sender != null)
                mainGrid.Children.Remove(sender as ColorPicker);
            isChangingBG = false;


            if (sender != null)
            {
                SqliteConnection sql = new SqliteConnection("Data Source = Database.db");
                sql.Open();
                var cur = sql.CreateCommand();
                cur.CommandText = "Update Background set Color = @para1 where Name = 'joinroom'";
                cur.Parameters.AddWithValue("@para1", color);
                cur.ExecuteNonQuery();
                sql.Close();
            }
        }

        float bias = 0.9f;
        private bool IsDark(Color color)
        {
            return (color.R + color.G + color.B) > 255 * 3 / 2 * bias;
        }
    }
}
