using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace music_bebra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Torrent
        {
            public string topic;
            public string topic_id;
        }
        public class Torrents
        {
            public Torrent[] songs;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("http://91.202.78.164:1337/get_songs?name=" + searchTxtBox.Text, "123");

            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create("http://91.202.78.164:1337/get_songs?name=" + searchTxtBox.Text);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            Torrents res = JsonConvert.DeserializeObject<Torrents>(responseFromServer);

            songName.Content = res.songs[0].topic_id;

            foreach (var val in resultListBox.Items)
            {
                resultListBox.Items.Remove(val);
            }

            for (int i = 0; i < res.songs.Length; i++)
            {
                ListBoxItem elem = new ListBoxItem();
                elem.Content = "<Button x:Name=\"" + res.songs[i].topic_id + "\" Content=\"" + res.songs[i].topic + "\">";
                resultListBox.Items.Add(elem);
            }

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

        }
    }
}
