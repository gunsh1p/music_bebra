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
        public bool IsPlaying { get; private set; }
        private bool IsLoaded = false;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private string[] songs = new string[] {
            "Перемотка - Встречная.mp3",
            "Zatox - Intensity.mp3",
            "GAYAZOV$ BROTHER$ - МАЛИНОВАЯ ЛАДА.mp3",
            "ЕГОР КРИД - Отпускаю.mp3",
            "System of A Down - Innervision.mp3",
            "Ислам Итляшев - Ай-яй-яй Дикая.mp3",
            "Depeche Mode - Never Let Me Down Again Single Version.mp3",
            "DZHIVAN - Автор.mp3",
            "Джаро & Ханза - Слышь, Малая.mp3",
            "Depeche Mode - Halo 2006 Digital Remaster.mp3",
            "Мурат Тхагалегов - Я с ней кайфую.mp3",
            "Ислам Итляшев - Кобра.mp3",
            "Асия - Спасибо.mp3",
            "ЕГОР КРИД - Будильник.mp3",
            "Monflame - Living Shadows.mp3",
            "алёна швец. - Волосы.mp3",
            "Султан Лагучев - Люблю и ненавижу.mp3",
            "Matt Ess - Stay Away.mp3",
            "Мурат Тхагалегов - Калым.mp3",
            "Макс Корж - Не твой.mp3",
            "Metallica - Fade To Black Remastered.mp3",
            "Depeche Mode - Enjoy The Silence Single Version.mp3",
            "Scorpions - Wind Of Change.mp3"
        };
        private string playing_song;
        Random random = new Random();
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
            IsPlaying = false;
            playing_song = songs[random.Next(0, songs.Length)];
            songName.Content = playing_song.Replace(".mp3", "");
        }

        private static void openit(string x)
        {
            System.Diagnostics.Process.Start("cmd", "/C start" + " " + x);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button elem = (Button)sender;
            openit("http://91.202.78.164:1337/get_torrent?id=" + elem.Name);
        }

        private void PausePlay(object sender, MouseButtonEventArgs e)
        {
            IsPlaying = !IsPlaying;
            PlayButton.Kind = !IsPlaying ? MaterialDesignThemes.Wpf.PackIconKind.Play : MaterialDesignThemes.Wpf.PackIconKind.Pause;
            if (IsPlaying && !IsLoaded)
            {
                //играем пока одну песню пока без бека
                // Разбирайся сам с этой ебаниной кароче
                // тебе нужно как то это подгужать же еще я ваще хз
                //mediaPlayer.Open(new Uri(@"pack://siteoforigin:,,,/testmusic.wav"));
                mediaPlayer.Open(new Uri(@"http://91.202.78.164:45047/songs/" + playing_song));
                IsLoaded = true;
            }
            if (IsPlaying)
            {
                mediaPlayer.Play();
            }
            else
            {
                mediaPlayer.Pause();
            }
        }

        private void searchBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WebRequest request = WebRequest.Create("http://91.202.78.164:1337/get_songs?name=" + searchTxtBox.Text);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            Torrents res = JsonConvert.DeserializeObject<Torrents>(responseFromServer);

            resultListBox.Items.Clear();

            int count = 0;

            for (int i = 0; i < res.songs.Length; i++)
            {
                Button elem = new Button();
                elem.Name = "_" + res.songs[i].topic_id;
                elem.Click += Button_Click;
                elem.Content = res.songs[i].topic;
                resultListBox.Items.Add(elem);
                count++;
            }

            if (count == 0)
            {
                ListBoxItem elem = new ListBoxItem();
                elem.Content = "Песня не найдена";
                resultListBox.Items.Add(elem);
            }

            reader.Close();
            dataStream.Close();
            response.Close();
        }

        private void PreviousPackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int id = Array.IndexOf(songs, playing_song);
            id--;
            if (id == -1)
            {
                id = songs.Length - 1;
            }
            playing_song = songs[id];
            songName.Content = playing_song.Replace(".mp3", "");
            mediaPlayer.Open(new Uri(@"http://91.202.78.164:45047/songs/" + playing_song));
            if (IsPlaying)
            {
                mediaPlayer.Play();
            }
        }

        private void NextPackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int id = Array.IndexOf(songs, playing_song);
            id++;
            if (id == songs.Length)
            {
                id = 0;
            }
            playing_song = songs[id];
            songName.Content = playing_song.Replace(".mp3", "");
            mediaPlayer.Open(new Uri(@"http://91.202.78.164:45047/songs/" + playing_song));
            if (IsPlaying)
            {
                mediaPlayer.Play();
            }
        }

        private void NOPE(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("НЕТ");
        }

    }
}
