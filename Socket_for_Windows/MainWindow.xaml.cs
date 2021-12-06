using Mono.Data.Sqlite;
using System.Windows;
using System.Windows.Media;

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

            SqliteConnection sql = new SqliteConnection("Data Source=Database.db");
            sql.Open();
            var cur = sql.CreateCommand();
            cur.CommandText = "Select * from Background";
            var reader = cur.ExecuteReader();
            while(reader.Read())
            {
                switch((string)reader[0])
                {
                    case "chatroomlist":
                        if(reader[1] != System.DBNull.Value)
                        {
                            chatRoomList.ChangeColor(null, Transfer.String2Color(reader[1]) );
                        }
                        break;
                    case "room":
                        if (reader[1] != System.DBNull.Value)
                        {
                            Room.ChangeColor(null, Transfer.String2Color(reader[1]));
                        }
                        break;
                    case "joinroom":
                        if (reader[1] != System.DBNull.Value)
                        {
                            joinGird.ChangeColor(null, Transfer.String2Color(reader[1]));
                        }
                        break;
                }
            }
            cur.Cancel();
            reader.Close();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            General.isClosing = true;
            for (int i = 0; i < General.activeSocket.Count; i++)
            {
                try
                {
                    General.activeSocket[i].Close();
                }
                catch { }
            }
        }
    }
}
