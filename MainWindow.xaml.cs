using ESO_Assistant;
using ESO_Assistant.Classes;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PacketDotNet;
using Renci.SshNet;
using SharpPcap;
using System;
//using NetFwTypeLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace MultiTools
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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
                    if (ModesWindow.IsVisible)
                        ModesWindow.Hide();
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
        SoundPlayer Click = new SoundPlayer(Application.GetResourceStream(new Uri("Click.wav", UriKind.Relative)).Stream);
        SoundPlayer Sound1 = new SoundPlayer(Application.GetResourceStream(new Uri("music.wav", UriKind.Relative)).Stream);


        Modes ModesWindow = new Modes();

        async Task<string> HttpGetAsync(string URI)
        {
            try
            {
                HttpClient hc = new HttpClient();
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
            GamesOpen = Process.GetProcessesByName("age3").Length != 0 || Process.GetProcessesByName("age3y").Length != 0 || Process.GetProcessesByName("age3f").Length != 0 || Process.GetProcessesByName("age3t").Length != 0 || Process.GetProcessesByName("age3x").Length != 0;
            if (GamesOpen && !LastGame)
            {
                EPVisible = Visibility.Collapsed;
                TPVisible = Visibility.Collapsed;
                NVisible = Visibility.Collapsed;
                XVisible = Visibility.Collapsed;
                YVisible = Visibility.Collapsed;
            }
            if (!GamesOpen && LastGame)
            {
                EPVisible = Paths.IsEPInstalled();
                TPVisible = Paths.IsTPInstalled();
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


        private DispatcherTimer SQLTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(30000)
        };

        private void SQLTimer_Tick(object sender, EventArgs e)
        {
            SQLTimer.Stop();
            Thread myThread = new Thread(async delegate ()
            {
                await Upload();
                await Download();
            });
            myThread.Start();
            SQLTimer.Start();
        }


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

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            Paths.OpenTAD("age3f.exe");
        }

        private void Image_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            Paths.OpenTAD("age3t.exe");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!ModesWindow.IsVisible)
                ModesWindow.Show();
            else
                ModesWindow.Activate();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Paths.OpenESOA();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Process.Start("https://vk.com/pages?oid=-4243069&p=%D0%9F%D0%BE%D0%BC%D0%BE%D1%89%D1%8C%20%D0%B8%20%D1%81%D0%BE%D0%B2%D0%B5%D1%82%D1%8B%20%D0%B8%D0%B3%D1%80%D0%BE%D0%BA%D0%B0%D0%BC");
        }


        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //       const string guidFWPolicy2 = "{E2B3C97F-6AE1-41AC-817A-F6F92166D7DD}";
            //     const string guidRWRule = "{2C5BC43E-3369-4C33-AB0C-BE9469677AF4}";

            //    FWCtrl ctrl = new FWCtrl();
            //  ctrl.Setup();
            /*
                            Type typeFWPolicy2 = Type.GetTypeFromCLSID(new Guid(guidFWPolicy2));
                            Type typeFWRule = Type.GetTypeFromCLSID(new Guid(guidRWRule));
                            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(typeFWPolicy2);
                            INetFwRule newRule = (INetFwRule)Activator.CreateInstance(typeFWRule);
                            newRule.Name = "InBound_Rule";
                            newRule.Description = "Block inbound traffic from 192.168.0.2 over TCP port 4000";
                            newRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY;
                        //    newRule.LocalPorts = "2300";
                            newRule.RemoteAddresses = "23.124.156.176";
                            newRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                            newRule.Enabled = true;
                            newRule.Grouping = "@firewallapi.dll,-23255";
                            newRule.Profiles = fwPolicy2.CurrentProfileTypes;
                            newRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                            fwPolicy2.Rules.Add(newRule);*/
            Process.Start("http://eso-community.net");
        }


        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Click.Play();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                captureDevice.Close();
            }
            catch
            {
            }
        }

        private void Image_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            Paths.OpenTWC("age3x.exe");
        }


        class Stat
        {
            public string FormatPR(string PR)
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

            public string GetAvatarFromID(string ID)
            {
                if (ID == "8bf82e325d864aab8fb6bc227a09f226")
                    return "pack://application:,,,/Avatars/avatar_tier1_01-sm.(0,0,4,1).jpg";

                if (ID == "3cb647657009445fbc06132c79baabab")
                    return "pack://application:,,,/Avatars/avatar_tier1_02-sm.(0,0,4,1).jpg";

                if (ID == "4f33c4c6640a4dfcab6475cbcd94403f")
                    return "pack://application:,,,/Avatars/avatar_tier1_03-sm.(0,0,4,1).jpg";

                if (ID == "718e486223a842c2ab6fd35307ef02b1")
                    return "pack://application:,,,/Avatars/avatar_tier1_04-sm.(0,0,4,1).jpg";

                if (ID == "07c2cb652a1d4893b9a2d99f958e6d31")
                    return "pack://application:,,,/Avatars/avatar_tier1_05-sm.(0,0,4,1).jpg";

                if (ID == "ec538ed0aa954cffb66e1e52d0ef3d64")
                    return "pack://application:,,,/Avatars/avatar_tier1_06-sm.(0,0,4,1).jpg";

                if (ID == "0218ef8aad7d4b83aa94b7d3659e2337")
                    return "pack://application:,,,/Avatars/avatar_tier1_07-sm.(0,0,4,1).jpg";

                if (ID == "96354d77298944bda2d2366259ed706e")
                    return "pack://application:,,,/Avatars/avatar_tier1_08-sm.(0,0,4,1).jpg";

                if (ID == "7037F010793C44699ABDA6EF6E6CF895")
                    return "pack://application:,,,/Avatars/avatar_tier1_09-sm.(0,0,4,1).jpg";

                if (ID == "0A6F27904D9E46e8A83B8651F2C084EE")
                    return "pack://application:,,,/Avatars/avatar_tier1_10-sm.(0,0,4,1).jpg";

                if (ID == "1ED65B1AB7124f998DC32F921284B2C5")
                    return "pack://application:,,,/Avatars/avatar_tier1_11-sm.(0,0,4,1).jpg";

                if (ID == "91E0744C321E4ebcB1F2BB61864C0ECC")
                    return "pack://application:,,,/Avatars/avatar_tier1_12-sm.(0,0,4,1).jpg";

                if (ID == "A632EBB2CC2940099F60C79CE82A3782")
                    return "pack://application:,,,/Avatars/avatar_tier1_13-sm.(0,0,4,1).jpg";

                if (ID == "6BF1CDA1CB9E4c53BC6B79E28899F117")
                    return "pack://application:,,,/Avatars/avatar_tier1_14-sm.(0,0,4,1).jpg";

                if (ID == "4EDC2767117E4b058CB84EEC3108BCCE")
                    return "pack://application:,,,/Avatars/avatar_tier1_15-sm.(0,0,4,1).jpg";

                if (ID == "571A6711431541158F911C607CBE4F64")
                    return "pack://application:,,,/Avatars/avatar_tier1_16-sm.(0,0,4,1).jpg";

                if (ID == "6983279CC5FA496f9128E3CD50BA48F7")
                    return "pack://application:,,,/Avatars/avatar_tier1_17-sm.(0,0,4,1).jpg";

                if (ID == "69060A7953A847d5AC1B310CC796C4EC")
                    return "pack://application:,,,/Avatars/avatar_tier1_18-sm.(0,0,4,1).jpg";

                if (ID == "53A2885492E34794854C5DEF656A2242")
                    return "pack://application:,,,/Avatars/avatar_tier1_19-sm.(0,0,4,1).jpg";

                if (ID == "E0973385606549fe80D8749AE138860E")
                    return "pack://application:,,,/Avatars/avatar_tier1_20-sm.(0,0,4,1).jpg";

                if (ID == "453F555167B04d1cBD4F7524BB3D3DA8")
                    return "pack://application:,,,/Avatars/avatar_tier2_01-sm.(0,0,4,1).jpg";

                if (ID == "E58738AC49B244308348AEF89F7A1D74")
                    return "pack://application:,,,/Avatars/avatar_tier2_02-sm.(0,0,4,1).jpg";

                if (ID == "F4360548A23E47a7B99655034A142AB7")
                    return "pack://application:,,,/Avatars/avatar_tier2_03-sm.(0,0,4,1).jpg";

                if (ID == "393D02B47D2F493fBD74169B5AD46782")
                    return "pack://application:,,,/Avatars/avatar_tier2_04-sm.(0,0,4,1).jpg";

                if (ID == "117BBDB1A97F4909B063B9CFD6AEF5C0")
                    return "pack://application:,,,/Avatars/avatar_tier2_05-sm.(0,0,4,1).jpg";

                if (ID == "2F02B13EF0414c5cA65985FF6124F617")
                    return "pack://application:,,,/Avatars/avatar_tier2_06-sm.(0,0,4,1).jpg";

                if (ID == "E642DA47766D4c76B2ACE88293D7C5E7")
                    return "pack://application:,,,/Avatars/avatar_tier2_07-sm.(0,0,4,1).jpg";

                if (ID == "99919C71EE364f96970C92BA01C3BCFD")
                    return "pack://application:,,,/Avatars/avatar_tier2_08-sm.(0,0,4,1).jpg";

                if (ID == "1E1354A9EC4C4873A204AB80FEB06DC2")
                    return "pack://application:,,,/Avatars/avatar_tier2_09-sm.(0,0,4,1).jpg";

                if (ID == "2D4E8F6255F24af882646CF6DB059A8E")
                    return "pack://application:,,,/Avatars/avatar_tier2_10-sm.(0,0,4,1).jpg";

                if (ID == "2808F16EFAE7425cB6EF551F69CE747A")
                    return "pack://application:,,,/Avatars/avatar_tier2_11-sm.(0,0,4,1).jpg";

                if (ID == "DAB93CF1BB4F4ca9908500C56967F121")
                    return "pack://application:,,,/Avatars/avatar_tier2_12-sm.(0,0,4,1).jpg";

                if (ID == "203969335E0048da911714FC6841D042")
                    return "pack://application:,,,/Avatars/avatar_tier2_13-sm.(0,0,4,1).jpg";

                if (ID == "92D802EC707C4ca3B059347FA4770971")
                    return "pack://application:,,,/Avatars/avatar_tier2_14-sm.(0,0,4,1).jpg";

                if (ID == "FFC3672624084b62BDF579D129C1357C")
                    return "pack://application:,,,/Avatars/avatar_tier2_15-sm.(0,0,4,1).jpg";

                if (ID == "555564B6D5984f1aB1D7052E8EB03F3B")
                    return "pack://application:,,,/Avatars/avatar_tier2_16-sm.(0,0,4,1).jpg";

                if (ID == "9DEB1EA8A4AF44d1932462FB086F4589")
                    return "pack://application:,,,/Avatars/avatar_tier2_17-sm.(0,0,4,1).jpg";

                if (ID == "65085E77B9F34e17926D060A71ECDDEA")
                    return "pack://application:,,,/Avatars/avatar_tier2_18-sm.(0,0,4,1).jpg";

                if (ID == "A4E28F5B734F4bd6BC7B0BEDFCB794B1")
                    return "pack://application:,,,/Avatars/avatar_tier2_19-sm.(0,0,4,1).jpg";

                if (ID == "7775DA3F0B9C4f99915A0D3A03C3C47E")
                    return "pack://application:,,,/Avatars/avatar_tier2_20-sm.(0,0,4,1).jpg";

                if (ID == "D78B62A673704fe4B728CBA8EECCDB39")
                    return "pack://application:,,,/Avatars/avatar_tier2_21-sm.(0,0,4,1).jpg";

                if (ID == "CB9614DDEC584f13B316A7374DAAC8BC")
                    return "pack://application:,,,/Avatars/avatar_tier2_22-sm.(0,0,4,1).jpg";

                if (ID == "056E7483851847cb80BE385D7B1CC73E")
                    return "pack://application:,,,/Avatars/avatar_tier2_23-sm.(0,0,4,1).jpg";

                if (ID == "6F41B230195B4a53AEA974E0054F8381")
                    return "pack://application:,,,/Avatars/avatar_tier2_24-sm.(0,0,4,1).jpg";

                if (ID == "1FE3245B419D4b9e8EFBE3C554FEE608")
                    return "pack://application:,,,/Avatars/avatar_tier2_25-sm.(0,0,4,1).jpg";

                if (ID == "DD335A1A31B8491aAD29DC5AA8F300EA")
                    return "pack://application:,,,/Avatars/avatar_tier2_26-sm.(0,0,4,1).jpg";

                if (ID == "883FA8D0B2AE42aa988DF45CF045D203")
                    return "pack://application:,,,/Avatars/avatar_tier3_01-sm.(0,0,4,1).jpg";

                if (ID == "8816CF4DDAEA43bbB25B43BD80B0B4E3")
                    return "pack://application:,,,/Avatars/avatar_tier3_02-sm.(0,0,4,1).jpg";

                if (ID == "EB0DA7CD545B41c1ADB5217DFCD7A1A4")
                    return "pack://application:,,,/Avatars/avatar_tier3_03-sm.(0,0,4,1).jpg";

                if (ID == "5E89742837574e1195251E096C72975F")
                    return "pack://application:,,,/Avatars/avatar_tier3_04-sm.(0,0,4,1).jpg";

                if (ID == "B3780CE346994f178167C03F21D5AF27")
                    return "pack://application:,,,/Avatars/avatar_tier3_05-sm.(0,0,4,1).jpg";

                if (ID == "1DB54546B3474921B875743869FC7E72")
                    return "pack://application:,,,/Avatars/avatar_tier3_06-sm.(0,0,4,1).jpg";

                if (ID == "49ACAD53D85844eb97BB701D31290BA4")
                    return "pack://application:,,,/Avatars/avatar_tier3_07-sm.(0,0,4,1).jpg";

                if (ID == "DF66A29A174140c5AED343770399A36D")
                    return "pack://application:,,,/Avatars/avatar_tier3_08-sm.(0,0,4,1).jpg";

                if (ID == "AD476EAE4798422a9DB2CD58BC192D2E")
                    return "pack://application:,,,/Avatars/avatar_tier3_09-sm.(0,0,4,1).jpg";

                if (ID == "2695D251FEAC43a88C8DEB33F446DE01")
                    return "pack://application:,,,/Avatars/avatar_tier3_10-sm.(0,0,4,1).jpg";

                if (ID == "23BD8CB8AADD4a1bB798D18F1AE8D2B5")
                    return "pack://application:,,,/Avatars/avatar_tier3_11-sm.(0,0,4,1).jpg";

                if (ID == "29DCE1286B0B4b31A03D5EF92A333958")
                    return "pack://application:,,,/Avatars/avatar_tier3_12-sm.(0,0,4,1).jpg";

                if (ID == "C4F44F57D68C4e338BC1848364A474FD")
                    return "pack://application:,,,/Avatars/avatar_tier3_13-sm.(0,0,4,1).jpg";

                if (ID == "A4C0A7AB7C7041d5B6680997A33A071F")
                    return "pack://application:,,,/Avatars/avatar_tier3_14-sm.(0,0,4,1).jpg";

                if (ID == "8F43A431AF48419080358B97BE9F9E40")
                    return "pack://application:,,,/Avatars/avatar_tier3_15-sm.(0,0,4,1).jpg";

                if (ID == "494F56E8C3B341a596D8FB15E18A654B")
                    return "pack://application:,,,/Avatars/avatar_tier3_16-sm.(0,0,4,1).jpg";

                if (ID == "502BE28D06EF41379C5F5635820B6929")
                    return "pack://application:,,,/Avatars/avatar_tier3_17-sm.(0,0,4,1).jpg";

                if (ID == "CC3C8BEEC9054a20B1665CADE1D4089A")
                    return "pack://application:,,,/Avatars/avatar_tier3_18-sm.(0,0,4,1).jpg";

                if (ID == "A9199968CDBB4f338687FA43AE943264")
                    return "pack://application:,,,/Avatars/avatar_tier3_19-sm.(0,0,4,1).jpg";

                if (ID == "BCF28F8B25BE4d17850920EE23EF0BD6")
                    return "pack://application:,,,/Avatars/avatar_tier3_20-sm.(0,0,4,1).jpg";

                if (ID == "30BE325A2D4B4fd0B38A69DF0F253E27")
                    return "pack://application:,,,/Avatars/avatar_tier3_21-sm.(0,0,4,1).jpg";

                if (ID == "E888B33E303249419B0795F4AE67AB45")
                    return "pack://application:,,,/Avatars/avatar_tier3_22-sm.(0,0,4,1).jpg";

                if (ID == "987EB6A563774309A5DD51E8F5A81C62")
                    return "pack://application:,,,/Avatars/avatar_tier3_23-sm.(0,0,4,1).jpg";

                if (ID == "B1F3C0149F294af78530E0299384A2A0")
                    return "pack://application:,,,/Avatars/avatar_tier3_24-sm.(0,0,4,1).jpg";

                if (ID == "8EAF68F6EA4440fa954C79CDA4A0301D")
                    return "pack://application:,,,/Avatars/avatar_tier3_25-sm.(0,0,4,1).jpg";

                if (ID == "50AEA6ABAB664a0d82C5BC3BFB4D6347")
                    return "pack://application:,,,/Avatars/avatar_tier3_26-sm.(0,0,4,1).jpg";

                if (ID == "C223306B63CA4a7f8B75800346F2C5D3")
                    return "pack://application:,,,/Avatars/avatar_tier3_27-sm.(0,0,4,1).jpg";

                if (ID == "AD9F800F4ACA4391A7309A03EEC0A8E9")
                    return "pack://application:,,,/Avatars/avatar_tier3_28-sm.(0,0,4,1).jpg";

                if (ID == "8C713428E6DD414cA50526A17420A140")
                    return "pack://application:,,,/Avatars/avatar_tier3_29-sm.(0,0,4,1).jpg";

                if (ID == "DBCF1E781B454187BF7A4AAC0255041F")
                    return "pack://application:,,,/Avatars/avatar_tier3_30-sm.(0,0,4,1).jpg";

                if (ID == "2a8fab35fd694ec7a6c7c14794b457eb")
                    return "pack://application:,,,/Avatars/avatarX1-sm.(0,0,4,1).jpg";

                if (ID == "29015d3b05a74a9bb3bf68dd242334a6")
                    return "pack://application:,,,/Avatars/avatarX2-sm.(0,0,4,1).jpg";

                if (ID == "e35df47c1b4e448da3840e5259ffd311")
                    return "pack://application:,,,/Avatars/avatarX3-sm.(0,0,4,1).jpg";

                if (ID == "5dd2c4059e4e4f1d960e35018ac8b26c")
                    return "pack://application:,,,/Avatars/avatarX4-sm.(0,0,4,1).jpg";

                if (ID == "9619a60f601641d19e9a33810d78c4c7")
                    return "pack://application:,,,/Avatars/avatarX5-sm.(0,0,4,1).jpg";

                if (ID == "c49e19280aa548bb9bfc6d5089f66743")
                    return "pack://application:,,,/Avatars/avatarX6-sm.(0,0,4,1).jpg";

                if (ID == "cd4be3e0b51b429787d0ad7c0ce1b0fe")
                    return "pack://application:,,,/Avatars/avatarX7-sm.(0,0,4,1).jpg";

                if (ID == "0a4349e3cb964366aea4959997f916d9")
                    return "pack://application:,,,/Avatars/avatarX8-sm.(0,0,4,1).jpg";

                if (ID == "8a844f16769a4d49b7fa6103ccc9a1f0")
                    return "pack://application:,,,/Avatars/avatarX9-sm.(0,0,4,1).jpg";

                if (ID == "f0e3b33595d044a0a1cae7b1be18c9ea")
                    return "pack://application:,,,/Avatars/avatarX10-sm.(0,0,4,1).jpg";

                if (ID == "67094b986d244e209c8c284d7b7771fe")
                    return "pack://application:,,,/Avatars/avatarX11-sm.(0,0,4,1).jpg";

                if (ID == "226c27899ff14e27af4ef306483153de")
                    return "pack://application:,,,/Avatars/avatarX12-sm.(0,0,4,1).jpg";

                if (ID == "5ec3d08b58114cc390802dee80223251")
                    return "pack://application:,,,/Avatars/avatarX13-sm.(0,0,4,1).jpg";

                if (ID == "c229aefa362a4f5393e5c1bd581912d1")
                    return "pack://application:,,,/Avatars/avatarX14-sm.(0,0,4,1).jpg";

                if (ID == "80a8865664384ec0aa1db21a3e757608")
                    return "pack://application:,,,/Avatars/avatarX15-sm.(0,0,4,1).jpg";

                if (ID == "a340e036956440e588f89eda1bb8c61d")
                    return "pack://application:,,,/Avatars/avatarX16-sm.(0,0,4,1).jpg";

                if (ID == "23da8e7ee1bd41eaa0298ada6cd5e68b")
                    return "pack://application:,,,/Avatars/avatarX17-sm.(0,0,4,1).jpg";

                if (ID == "e5f3f9ee6c0a49a9be29c12836cf4a13")
                    return "pack://application:,,,/Avatars/avatarX18-sm.(0,0,4,1).jpg";

                if (ID == "35a1e06319c74383903fcbc4798832ea")
                    return "pack://application:,,,/Avatars/avatarX19-sm.(0,0,4,1).jpg";

                if (ID == "db67c6899dd0469f89ee12dab6f90281")
                    return "pack://application:,,,/Avatars/avatarX20-sm.(0,0,4,1).jpg";

                if (ID == "eccebb63e6784ac184fb02077b4e198f")
                    return "pack://application:,,,/Avatars/avatarX21-sm.(0,0,4,1).jpg";

                if (ID == "f2939cd118bd4011986055fd040e07df")
                    return "pack://application:,,,/Avatars/avatarX22-sm.(0,0,4,1).jpg";

                if (ID == "b201d5fb4f7f43cebac5a1a518033536")
                    return "pack://application:,,,/Avatars/avatarX23-sm.(0,0,4,1).jpg";

                if (ID == "9e499080bde343438d3db1217458f835")
                    return "pack://application:,,,/Avatars/avatarX24-sm.(0,0,4,1).jpg";

                if (ID == "aa4e4ea8cb644398a3216c6c8eefcdf3")
                    return "pack://application:,,,/Avatars/avatarX25-sm.(0,0,4,1).jpg";

                if (ID == "698b74216a394aa9aaa708be5cf65b0c")
                    return "pack://application:,,,/Avatars/avatarX26-sm.(0,0,4,1).jpg";

                if (ID == "764b464793674a518e88526cf4e77801")
                    return "pack://application:,,,/Avatars/avatarX27-sm.(0,0,4,1).jpg";

                if (ID == "b34746fde9ae4d4488c8ad8a0fe3abc2")
                    return "pack://application:,,,/Avatars/avatarX28-sm.(0,0,4,1).jpg";

                if (ID == "acb93a843e95409ba4f61bfb9b56b973")
                    return "pack://application:,,,/Avatars/avatarX29-sm.(0,0,4,1).jpg";

                if (ID == "578f3a59dc7f40ba944a09d03cd6e9a9")
                    return "pack://application:,,,/Avatars/avatarX30-sm.(0,0,4,1).jpg";

                if (ID == "0efdbb5b250e434485228b006d4b1c4c")
                    return "pack://application:,,,/Avatars/avatarX31-sm.(0,0,4,1).jpg";

                if (ID == "578261e4ccc445bb8d1e488b2ea73347")
                    return "pack://application:,,,/Avatars/avatarX32-sm.(0,0,4,1).jpg";

                if (ID == "3662b9c67eac494c93d31f77bf8e66f6")
                    return "pack://application:,,,/Avatars/avatarX33-sm.(0,0,4,1).jpg";

                if (ID == "fb3d3bc888ed43a9ae7eec98c6cacba8")
                    return "pack://application:,,,/Avatars/avatarX34-sm.(0,0,4,1).jpg";

                if (ID == "faeb7020e0b9412f9293f96fbafb4eae")
                    return "pack://application:,,,/Avatars/avatarX35-sm.(0,0,4,1).jpg";

                if (ID == "90f5907eabbe40f3b20014b2a3007f12")
                    return "pack://application:,,,/Avatars/avatarX36-sm.(0,0,4,1).jpg";

                if (ID == "f2e89069a7694bd58705943bc536c0f9")
                    return "pack://application:,,,/Avatars/avatarX37-sm.(0,0,4,1).jpg";

                if (ID == "13288c65d272482fbe7d728c98b46038")
                    return "pack://application:,,,/Avatars/avatarX38-sm.(0,0,4,1).jpg";

                if (ID == "642620e4b96946b69c9df09212783de2")
                    return "pack://application:,,,/Avatars/avatarX39-sm.(0,0,4,1).jpg";

                if (ID == "8af12a39a2324c32ac860654a70cf4f1")
                    return "pack://application:,,,/Avatars/avatarX40-sm.(0,0,4,1).jpg";

                if (ID == "7b1b131582e645fdade348c0b1dbaa39")
                    return "pack://application:,,,/Avatars/avatarX41-sm.(0,0,4,1).jpg";

                if (ID == "3882cfd23ce74c3caa395f2660cbaff8")
                    return "pack://application:,,,/Avatars/avatarX42-sm.(0,0,4,1).jpg";

                if (ID == "7d18a487d85b4d739ef2cd3575390bf8")
                    return "pack://application:,,,/Avatars/avatarX43-sm.(0,0,4,1).jpg";

                if (ID == "bad6168cc6ad4283b6de10ab71a14388")
                    return "pack://application:,,,/Avatars/avatarX44-sm.(0,0,4,1).jpg";

                if (ID == "0c3f004631d84155bfe3f5ffb42c994f")
                    return "pack://application:,,,/Avatars/avatarX45-sm.(0,0,4,1).jpg";

                if (ID == "1d4ad522-6af1-415d-9bdf-6632f2f055ac")
                    return "pack://application:,,,/Avatars/avatarY1-sm.(0,0,4,1).jpg";

                if (ID == "38fcd4ff-ed1c-4462-853c-4cf641e7aa94")
                    return "pack://application:,,,/Avatars/avatarY2-sm.(0,0,4,1).jpg";

                if (ID == "ae0f56f3-b24f-4a82-afec-53911109a1e7")
                    return "pack://application:,,,/Avatars/avatarY3-sm.(0,0,4,1).jpg";

                if (ID == "e77b9abd-9ad8-4a32-87ea-632fa47cf7b0")
                    return "pack://application:,,,/Avatars/avatarY4-sm.(0,0,4,1).jpg";

                if (ID == "ee51b2d8-191a-406a-86eb-eb012c701eab")
                    return "pack://application:,,,/Avatars/avatarY5-sm.(0,0,4,1).jpg";

                if (ID == "d8c9dc25-1e5d-41db-91d7-8be58431042d")
                    return "pack://application:,,,/Avatars/avatarY6-sm.(0,0,4,1).jpg";

                if (ID == "c65aa1f7-43ec-4543-b21f-83a838d71ddb")
                    return "pack://application:,,,/Avatars/avatarY7-sm.(0,0,4,1).jpg";

                if (ID == "88e7634a-ad5f-4a1b-977d-56de1b9271cf")
                    return "pack://application:,,,/Avatars/avatarY8-sm.(0,0,4,1).jpg";

                if (ID == "7c76d9f5-c2ef-45fb-be7a-734da1371270")
                    return "pack://application:,,,/Avatars/avatarY9-sm.(0,0,4,1).jpg";

                if (ID == "5d7e7128-4338-4644-8101-12ec012afe77")
                    return "pack://application:,,,/Avatars/avatarY10-sm.(0,0,4,1).jpg";

                if (ID == "0b98a7d2-3088-48cf-9c8a-a2b1512a43d8")
                    return "pack://application:,,,/Avatars/avatarY11-sm.(0,0,4,1).jpg";

                if (ID == "1378c867-bf9e-4088-8d4b-6da8c3e2f6c5")
                    return "pack://application:,,,/Avatars/avatarY12-sm.(0,0,4,1).jpg";

                if (ID == "29d10719-b3b2-4b9e-8098-2064af3c911e")
                    return "pack://application:,,,/Avatars/avatarY13-sm.(0,0,4,1).jpg";

                if (ID == "0e3f2fe4-0c79-4500-bb68-4fdb7f4c47b4")
                    return "pack://application:,,,/Avatars/avatarY14-sm.(0,0,4,1).jpg";

                if (ID == "c53b642d-6f73-4e19-b224-b382d10cf160")
                    return "pack://application:,,,/Avatars/avatarY15-sm.(0,0,4,1).jpg";

                if (ID == "d94a0fa6-0cf8-4de0-85be-6aa4e9c02034")
                    return "pack://application:,,,/Avatars/avatarY16-sm.(0,0,4,1).jpg";

                if (ID == "d70ec3fb-a3c5-468c-b311-07994e1c1d8d")
                    return "pack://application:,,,/Avatars/avatarY17-sm.(0,0,4,1).jpg";

                if (ID == "745a3e78-5d11-484b-b7c3-6775db79bcdc")
                    return "pack://application:,,,/Avatars/avatarY18-sm.(0,0,4,1).jpg";

                if (ID == "ddd65676-58ad-4dc4-837d-5f4cecba1a3f")
                    return "pack://application:,,,/Avatars/avatarY19-sm.(0,0,4,1).jpg";

                if (ID == "2094631f-011a-4ce6-9eac-7a5e8d866715")
                    return "pack://application:,,,/Avatars/avatarY20-sm.(0,0,4,1).jpg";

                if (ID == "c467e531-eda9-46bf-9849-706a2f1b827f")
                    return "pack://application:,,,/Avatars/avatarY21-sm.(0,0,4,1).jpg";

                if (ID == "3b8a41db-06d6-4847-bfd2-9f560250e20d")
                    return "pack://application:,,,/Avatars/avatarY22-sm.(0,0,4,1).jpg";

                if (ID == "bef1c47d-49b7-474d-9427-fd09c6e03ad3")
                    return "pack://application:,,,/Avatars/avatarY23-sm.(0,0,4,1).jpg";

                if (ID == "ce2cbd88-2f86-4939-85c3-1a06f7a18164")
                    return "pack://application:,,,/Avatars/avatarY24-sm.(0,0,4,1).jpg";

                if (ID == "74bc7cbd-c4f1-4c76-b46c-f67b5d38ba4e")
                    return "pack://application:,,,/Avatars/avatarY25-sm.(0,0,4,1).jpg";

                if (ID == "eda5e165-4d9b-423c-89ba-24142895a406")
                    return "pack://application:,,,/Avatars/avatarY26-sm.(0,0,4,1).jpg";

                if (ID == "e08e656b-f064-4046-91de-db2f51a43676")
                    return "pack://application:,,,/Avatars/avatarY27-sm.(0,0,4,1).jpg";

                if (ID == "4cd87d4f-4638-45f8-9b5e-799a94c1c925")
                    return "pack://application:,,,/Avatars/avatarY28-sm.(0,0,4,1).jpg";

                if (ID == "14eefce3-80bf-49eb-99d2-295747760abf")
                    return "pack://application:,,,/Avatars/avatarY29-sm.(0,0,4,1).jpg";

                if (ID == "9801b5b3-c9ba-4cf1-834b-0518e3810661")
                    return "pack://application:,,,/Avatars/avatarY30-sm.(0,0,4,1).jpg";

                if (ID == "f8d40623-a83c-409d-a8b4-c549e22c972a")
                    return "pack://application:,,,/Avatars/avatarY31-sm.(0,0,4,1).jpg";

                if (ID == "9fcf71ac-f40c-4b36-bb80-623caa1949f3")
                    return "pack://application:,,,/Avatars/avatarY32-sm.(0,0,4,1).jpg";

                if (ID == "1caa08bd-3b59-4c5b-bbf0-5cec331d6e16")
                    return "pack://application:,,,/Avatars/avatarY33-sm.(0,0,4,1).jpg";

                if (ID == "fc1a26d9-bde2-4cb5-b844-8ff18ef1838b")
                    return "pack://application:,,,/Avatars/avatarY34-sm.(0,0,4,1).jpg";

                if (ID == "8cb00f49-4120-454d-b78b-d886e55a9d05")
                    return "pack://application:,,,/Avatars/avatarY35-sm.(0,0,4,1).jpg";

                if (ID == "94f210bb-aaf7-4166-887a-2dafaa229dc6")
                    return "pack://application:,,,/Avatars/avatarY36-sm.(0,0,4,1).jpg";

                if (ID == "5807abf7-2b57-4c33-8a84-449728f70f4c")
                    return "pack://application:,,,/Avatars/avatarY37-sm.(0,0,4,1).jpg";

                if (ID == "ad3226b5-5da6-479c-b696-d1650da97fd6")
                    return "pack://application:,,,/Avatars/avatarY38-sm.(0,0,4,1).jpg";

                if (ID == "8470e885-8887-457d-a489-be55f9f95055")
                    return "pack://application:,,,/Avatars/avatarY39-sm.(0,0,4,1).jpg";

                if (ID == "6abbfff5-c4cd-461e-a65a-3a5efc919cc2")
                    return "pack://application:,,,/Avatars/avatarY40-sm.(0,0,4,1).jpg";

                if (ID == "431d0124-0ef3-411d-999e-70257f99e63b")
                    return "pack://application:,,,/Avatars/avatarY41-sm.(0,0,4,1).jpg";

                if (ID == "615bf7b3-4e96-4fa5-b378-e0080073d236")
                    return "pack://application:,,,/Avatars/avatarY42-sm.(0,0,4,1).jpg";

                if (ID == "76b55472-b2d9-445e-ad1e-c4642103f860")
                    return "pack://application:,,,/Avatars/avatarY43-sm.(0,0,4,1).jpg";

                if (ID == "e96b8a53-b61d-4c0c-b9c9-0e52bd8ac02d")
                    return "pack://application:,,,/Avatars/avatarY44-sm.(0,0,4,1).jpg";

                if (ID == "c5d17f17-78b5-4b46-9a95-d78bc43272d8")
                    return "pack://application:,,,/Avatars/avatarY45-sm.(0,0,4,1).jpg";

                if (ID == "ea6022fb-19ab-4ecb-ac3d-c4469429a71f")
                    return "pack://application:,,,/Avatars/avatarY46-sm.(0,0,4,1).jpg";

                if (ID == "c21080a0-b345-474c-909b-04cb9afb718a")
                    return "pack://application:,,,/Avatars/avatarY47-sm.(0,0,4,1).jpg";

                if (ID == "5a7ce673-634d-4a37-ad43-5545a0a9e9d1")
                    return "pack://application:,,,/Avatars/avatarY48-sm.(0,0,4,1).jpg";

                if (ID == "746075d1-6254-4dec-9127-ce7ec73827da")
                    return "pack://application:,,,/Avatars/avatarY49-sm.(0,0,4,1).jpg";

                if (ID == "9140248b-5545-4954-87a5-ab463fcc7ed6")
                    return "pack://application:,,,/Avatars/avatarY50-sm.(0,0,4,1).jpg";

                if (ID == "b899b821-e586-4698-a21c-d598325dc8b5")
                    return "pack://application:,,,/Avatars/avatarY51-sm.(0,0,4,1).jpg";

                if (ID == "0c182d86-f9e0-4208-8074-0ce427e40a84")
                    return "pack://application:,,,/Avatars/avatarY52-sm.(0,0,4,1).jpg";
                return "pack://application:,,,/Avatars/cpai_avatar_random.(0,0,4,1).png";
            }

        }

        public class VPNInfo
        {
            public string ASN { get; set; }
            public string ISP { get; set; }
            public string country_code { get; set; }
            public string region { get; set; }
            public string city { get; set; }
            public string organization { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public bool is_crawler { get; set; }
            public string timezone { get; set; }
            public bool mobile { get; set; }
            public string host { get; set; }
            public bool proxy { get; set; }
            public bool vpn { get; set; }
            public bool tor { get; set; }
            public bool success { get; set; }
            public string message { get; set; }
            public float fraud_score { get; set; }
            public string request_id { get; set; }
        }

        public class City
        {
            public string id { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string name_ru { get; set; }
            public string name_en { get; set; }
            public string name_de { get; set; }
            public string name_fr { get; set; }
            public string name_it { get; set; }
            public string name_es { get; set; }
            public string name_pt { get; set; }
            public string okato { get; set; }
            public string vk { get; set; }
            public string population { get; set; }
        }

        public class Region
        {
            public string id { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string name_ru { get; set; }
            public string name_en { get; set; }
            public string name_de { get; set; }
            public string name_fr { get; set; }
            public string name_it { get; set; }
            public string name_es { get; set; }
            public string name_pt { get; set; }
            public string iso { get; set; }
            public string timezone { get; set; }
            public string okato { get; set; }
            public string auto { get; set; }
            public string vk { get; set; }
            public string utc { get; set; }
        }

        public class Country
        {
            public string id { get; set; }
            public string iso { get; set; }
            public string continent { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string name_ru { get; set; }
            public string name_en { get; set; }
            public string name_de { get; set; }
            public string name_fr { get; set; }
            public string name_it { get; set; }
            public string name_es { get; set; }
            public string name_pt { get; set; }
            public string timezone { get; set; }
            public string area { get; set; }
            public string population { get; set; }
            public string capital_id { get; set; }
            public string capital_ru { get; set; }
            public string capital_en { get; set; }
            public string cur_code { get; set; }
            public string phone { get; set; }
            public string neighbours { get; set; }
            public string vk { get; set; }
            public string utc { get; set; }
        }

        public class IPInfo
        {
            public string ip { get; set; }
            public City city { get; set; }
            public Region region { get; set; }
            public Country country { get; set; }
            public string error { get; set; }
            public string request { get; set; }
            public string created { get; set; }
            public string timestamp { get; set; }
        }

        // IP CHECKER//
        ///////////////
        ///////////////
        ///////////////
        public class Row : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

            }
            private string ip = "";
            private string eso = "";
            public string IP
            {
                get { return ip; }
                set
                {
                    if (ip != value)
                    {
                        ip = value;
                        NotifyPropertyChanged("IP");
                    }
                }
            }
            public string ESO
            {
                get { return eso; }
                set
                {
                    if (eso != value)
                    {
                        eso = value;
                        NotifyPropertyChanged("ESO");
                    }
                }
            }
        }

        public ObservableCollection<Row> FullIP = new ObservableCollection<Row>();
        public ObservableCollection<Row> BufferIP = new ObservableCollection<Row>();
        private ObservableCollection<ListItem> MyView = new ObservableCollection<ListItem>();
        public class ListItem : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

            }
            private string ip = "";
            private string eso = "";
            private string pr = "";
            private string avatar = "pack://application:,,,/Avatars/cpai_avatar_random.(0,0,4,1).png";
            private string firstupdate = "";
            private string lastupdate = "";
            private string hint = (string)Application.Current.FindResource("Hintdefault");

            private VPN Fvpn;
            private string ping = "---";
            private IP Fip;

            public VPN Vpn
            {
                get { return Fvpn; }
                set
                {
                    if (Fvpn.Perc != value.Perc)
                    {
                        Fvpn.Perc = value.Perc;
                        Fvpn.Label = value.Label;
                        NotifyPropertyChanged("Vpn");
                        NotifyPropertyChanged("HintIP");
                        NotifyPropertyChanged("Vpnperc");
                    }
                }
            }

            public string Ping
            {
                get { return ping; }
                set
                {
                    if (ping != value)
                    {
                        ping = value;
                        NotifyPropertyChanged("Ping");
                    }
                }
            }

            public IP Ip
            {
                get
                {
                    return Fip;
                }
                set
                {
                    Fip.city = value.city;
                    Fip.country = value.country;
                    Fip.flag = value.flag;
                    Fip.hour = value.hour;
                    NotifyPropertyChanged("Ip");
                    NotifyPropertyChanged("HintIP");
                    NotifyPropertyChanged("Flag");
                }
            }
            public string Vpnperc
            {
                get
                {
                    if (string.IsNullOrEmpty(Vpn.Perc))
                        Fvpn.Perc = "0 %";
                    return Fvpn.Perc;
                }
            }

            public string HintIP
            {
                get
                {
                    if (string.IsNullOrEmpty(Fip.country))
                        Fip.country = (string)Application.Current.FindResource("● Coutry: unknown");
                    if (string.IsNullOrEmpty(Fip.city))
                        Fip.city = (string)Application.Current.FindResource("● City: unknown");
                    if (string.IsNullOrEmpty(Fip.hour))
                        Fip.hour = (string)Application.Current.FindResource("● Hours Between: unknown");
                    if (string.IsNullOrEmpty(Fvpn.Label))
                        Fvpn.Label = (string)Application.Current.FindResource("No VPN detected!");
                    return Fip.country + "\n" + Fip.city + "\n" + Fip.hour + "\n\n" + Fvpn.Label;
                }
            }

            public string IP
            {
                get { return ip; }
                set
                {
                    if (ip != value)
                    {
                        ip = value;
                        NotifyPropertyChanged("IP");
                        NotifyPropertyChanged("IPHidden");
                    }
                }
            }
            public string IPHidden
            {
                get { return new string('●', ip.Length); }
            }
            public string ESO
            {
                get { return eso.Replace("_", "__"); }
                set
                {
                    if (eso != value)
                    {
                        eso = value;
                        NotifyPropertyChanged("ESO");
                    }
                }
            }
            public string PR
            {
                get { return pr; }
                set
                {
                    if (pr != value)
                    {
                        pr = value;
                        NotifyPropertyChanged("PR");
                    }
                }
            }

            public string Avatar
            {
                get { return avatar; }
                set
                {
                    if (avatar != value)
                    {
                        avatar = value;
                        NotifyPropertyChanged("Avatar");
                    }
                }
            }


            public string Flag
            {
                get
                {
                    if (string.IsNullOrEmpty(Fip.flag))
                    {
                        Fip.flag = "pack://application:,,,/Resources/_unknown.png";
                    }
                    return Fip.flag;
                }
            }

            /* public string HomeCity
             {
                 get { return homecity; }
                 set
                 {
                     if (homecity != value)
                     {
                         homecity = value;
                         NotifyPropertyChanged("HomeCity");
                     }
                 }
             }*/
            public string FirstUpdate
            {
                get { return firstupdate; }
                set
                {
                    if (firstupdate != value)
                    {
                        firstupdate = value;
                        NotifyPropertyChanged("FirstUpdate");
                        NotifyPropertyChanged("Combined");
                    }
                }
            }
            public string LastUpdate
            {
                get { return lastupdate; }
                set
                {
                    if (lastupdate != value)
                    {
                        lastupdate = value;
                        NotifyPropertyChanged("LastUpdate");
                        NotifyPropertyChanged("Combined");
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

            public string Combined
            {
                get { return string.Format("{0}" + Environment.NewLine + "{1}", FirstUpdate, LastUpdate); }
            }
        }

        private DispatcherTimer GeneralTimer = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };

        public SharpPcap.LibPcap.LibPcapLiveDevice captureDevice = null;
        private string adresseLocale = null;
        bool isHost = false;

        public string AdresseLocale { get => adresseLocale; set => adresseLocale = value; }

        private byte CompareIP(string ip1, string ip2)
        {
            byte R = 0;
            try
            {
                byte[] IP1 = IPAddress.Parse(ip1).GetAddressBytes();
                byte[] IP2 = IPAddress.Parse(ip2).GetAddressBytes();
                /*  if (BitConverter.IsLittleEndian)
                  {
                      Array.Reverse(IP1);
                      Array.Reverse(IP2);
                  }*/
                byte A = IP1[0];
                byte B = IP1[1];
                byte C = IP1[2];
                byte D = IP1[3];

                byte A2 = IP2[0];
                byte B2 = IP2[1];
                byte C2 = IP2[2];
                byte D2 = IP2[3];

                if (A == A2)
                    R = 1;
                else
                    return R;
                if (B == B2)
                    R = 2;// "Medium probability";
                if (C == C2)
                    R = 3;// "High probability";
                if (D == D2)
                    R = 4;// "100% probability";
            }
            catch
            {
                R = 0;
            }

            return R;
        }

        string LevelToString(byte level)
        {
            switch (level)
            {
                case 2: return (string)Application.Current.FindResource("Medium probability");
                case 3: return (string)Application.Current.FindResource("High probability");
                case 4: return (string)Application.Current.FindResource("100% probability");
            }
            return "";
        }
        private int posNull(int start, byte[] data)
        {
            if (data != null)
                for (int i = start; i < data.Length - 1; i++)
                    if (data[i] == 0 && data[i + 1] == 0)
                        if (data[start + 1] == 0)
                            return i + 1;
                        else
                            return i;
            return data.Length;
        }
        private string GetPR(byte[] data)
        {
            string R = "";
            int count = 0;
            if (data != null)
            {
                for (int i = 0; i < data.Length - 6; i++)
                {
                    if (data[i] == 0 && data[i + 1] == 0 && data[i + 2] == 6 || data[i + 2] == 4 && data[i + 3] == 0 && data[i + 4] > 47 && data[i + 4] < 58)
                        count++;

                    if (count == 2)
                    {

                        byte[] P = new byte[2];
                        P[0] = data[i + 4];
                        if (data[i + 6] != 0)
                            P[1] = data[i + 6];
                        return Encoding.ASCII.GetString(P);

                    }
                }
            }
            return R;
        }

        private string GetAvatar(byte[] data)
        {
            string R = "";
            int start = 0;
            //     int end = 0;
            if (data != null)
            {
                for (int i = 0; i < data.Length - 3; i++)
                {
                    if (data[i] == 0 && data[i + 1] == 0 && data[i + 2] == 2 && data[i + 3] == 0)
                    {
                        start = i + 8;
                        try
                        {
                            return Encoding.Unicode.GetString(data, start, posNull(start, data) - start);
                        }
                        catch
                        { }
                    }
                }
            }
            return R;
        }

        private string GetHomeCity(byte[] data)
        {
            string R = "";
            int start = 0;
            //     int end = 0;
            if (data != null)
            {
                for (int i = 0; i < data.Length - 3; i++)
                {
                    if (data[i] == 0 && data[i + 1] == 0 && (data[i + 2] == 16) && data[i + 3] == 0)
                    {
                        start = i + 4;
                        try
                        {
                            return Encoding.Unicode.GetString(data, start, posNull(start, data) - start);
                        }
                        catch { }
                    }

                }


            }
            return R;
        }


        private string GetPair(string ip)
        {
            StringBuilder Names = new StringBuilder();
            Dictionary<string, byte> Buffer = new Dictionary<string, byte>();
            for (int i = 0; i < FullIP.Count; i++)
            {
                byte Level = CompareIP(FullIP[i].IP, ip);
                if (Buffer.ContainsKey(FullIP[i].ESO))
                {
                    if (Buffer[FullIP[i].ESO] < Level)
                        Buffer[FullIP[i].ESO] = Level;
                }
                else
                    Buffer.Add(FullIP[i].ESO, Level);
            }
            foreach (var item in Buffer)
                if (item.Value > 1)
                    Names.AppendLine(item.Key + " : " + LevelToString(item.Value));
            if (Names.Length == 0)
                return (string)Application.Current.FindResource("Hintdefault");
            else
            {
                string R = Names.ToString();
                return R.Remove(R.Length - Environment.NewLine.Length);
            }
        }

        private void ParseData(byte[] byteData, string srcIP, string dstIP)
        {
            Stat S = new Stat();
            string localIP = AdresseLocale;
            string IP = localIP;

            if (srcIP == localIP)
                IP = dstIP;
            if (dstIP == localIP)
                IP = srcIP;
            if (IP != localIP)
                if (byteData != null && (byteData[0] == 3 || byteData[0] == 4))
                {
                    if (!MyView.Any(x => x.IP == IP))
                    {

                        var LI = new ListItem()
                        {
                            IP = IP
                        };
                        // Этот пакет отдается 1 раз при заходе в комнату
                        if (byteData[0] == 3 && byteData[8] == 6)
                        {
                            // Вычисляем имя хоста, если хост не мы
                            isHost = (localIP == srcIP);
                            if (!isHost)
                                if (LI.ESO == "")
                                {
                                    LI.ESO = Encoding.Unicode.GetString(byteData, 13, posNull(13, byteData) - 13);
                                }
                        }
                        if (byteData[0] == 3 && byteData[7] == 0 && byteData[8] == 1 && byteData[9] == 19 && byteData[10] == 10)
                            if (isHost)
                            {

                                if (byteData[11] == 106 || byteData[11] == 130 || byteData[11] == 154 || byteData[11] == 178 || byteData[11] == 202 || byteData[11] == 226 || byteData[11] == 250)
                                    if (LI.ESO == "")
                                        LI.ESO = Encoding.Unicode.GetString(byteData, 19, posNull(19, byteData) - 19);

                                if (byteData[11] == 118 || byteData[11] == 142 || byteData[11] == 166 || byteData[11] == 190 || byteData[11] == 214 || byteData[11] == 238 || (byteData[11] == 6 && byteData[12] == 1))
                                    if (LI.PR == "")
                                        LI.PR = S.FormatPR(Encoding.Unicode.GetString(byteData, 19, posNull(19, byteData) - 19));

                                /*   if (byteData[11] == 125 || byteData[11] == 149 || byteData[11] == 173 || byteData[11] == 197 || byteData[11] == 221 || byteData[11] == 245 || (byteData[11] == 13 && byteData[12] == 1))
                                       if (Encoding.Unicode.GetString(byteData, 19, posNull(19, byteData) - 19) != "")
                                           LI.HomeCity = (Encoding.Unicode.GetString(byteData, 19, posNull(19, byteData) - 19));*/

                                if (byteData[11] == 116 || byteData[11] == 140 || byteData[11] == 164 || byteData[11] == 188 || byteData[11] == 212 || byteData[11] == 236 || (byteData[11] == 4 && byteData[12] == 1))
                                    if (LI.Avatar == "pack://application:,,,/Avatars/cpai_avatar_random.(0,0,4,1).png")
                                        LI.Avatar = S.GetAvatarFromID(Encoding.Unicode.GetString(byteData, 19, posNull(19, byteData) - 19));

                            }
                        // Если мы не хост определяем рейтинг и аватар
                        if (byteData[0] == 4)
                            if (!isHost)
                            {
                                if (LI.PR == "")
                                    LI.PR = S.FormatPR(GetPR(byteData));
                                if (LI.Avatar == "pack://application:,,,/Avatars/cpai_avatar_random.(0,0,4,1).png")
                                    LI.Avatar = S.GetAvatarFromID(GetAvatar(byteData));
                                /*if (GetHomeCity(byteData) != "")
                                    LI.HomeCity = GetHomeCity(byteData);*/
                            }

                        //ShowCustomBalloon("User " + msg + " is connected!", LI.Avatar);

                        string Date = DateTime.Now.ToLongTimeString();
                        LI.FirstUpdate = Date;
                        LI.LastUpdate = Date;
                        LI.Hint = GetPair(IP);
                        Dispatcher.Invoke((Action)(async () =>
                        {
                                 // История
                                 try
                            {
                                if (LI.ESO != "")
                                {
                                    var R = new Row()
                                    {
                                        IP = LI.IP,
                                        ESO = LI.ESO
                                    };
                                    if (!FullIP.Any(p => p.ESO == R.ESO && p.IP == R.IP))
                                        FullIP.Add(R);
                                    if (!BufferIP.Any(p => p.ESO == R.ESO && p.IP == R.IP))
                                        BufferIP.Add(R);
                                    Debug.WriteLine("1 " + R.ESO);
                                }
                            }
                            catch { }
                                 // Добавление
                                 try
                            {
                                if (!MyView.Any(x => x.IP == IP))
                                {
                                    MyView.Add(LI);
                                    Debug.WriteLine("2 " + LI.ESO);
                                    Sound1.Play();

                                    LI.Vpn = await GetVPN("https://www.ipqualityscore.com/api/json/ip/AGb3QZuZgJ9Z6l5n00Eym9k9VMKS2ETi/" + IP + "?strictness=1");
                                    LI.Ip = await GetIPInfo("http://api.sypexgeo.net/json/" + IP);
                                    GetPing(IP);

                                }

                            }
                            catch { Debug.WriteLine("Ошибка добавления нового игрока"); }
                        }));
                    }
                    else
                    {
                        string bESO = "";
                        string bPR = "";
                        string bAvatar = "";
                        string bHint = "";

                        if (byteData[0] == 3 && byteData[8] == 6)
                        {
                            isHost = (localIP == srcIP);
                            if (!isHost)
                                bESO = Encoding.Unicode.GetString(byteData, 13, posNull(13, byteData) - 13);
                        }
                        if (byteData[0] == 3 && byteData[7] == 0 && byteData[8] == 1 && byteData[9] == 19 && byteData[10] == 10)
                            if (isHost)
                            {

                                if (byteData[11] == 106 || byteData[11] == 130 || byteData[11] == 154 || byteData[11] == 178 || byteData[11] == 202 || byteData[11] == 226 || byteData[11] == 250)
                                    bESO = Encoding.Unicode.GetString(byteData, 19, posNull(19, byteData) - 19);
                                if (byteData[11] == 118 || byteData[11] == 142 || byteData[11] == 166 || byteData[11] == 190 || byteData[11] == 214 || byteData[11] == 238 || (byteData[11] == 6 && byteData[12] == 1))
                                    bPR = S.FormatPR(Encoding.Unicode.GetString(byteData, 19, posNull(19, byteData) - 19));

                                if (byteData[11] == 116 || byteData[11] == 140 || byteData[11] == 164 || byteData[11] == 188 || byteData[11] == 212 || byteData[11] == 236 || (byteData[11] == 4 && byteData[12] == 1))
                                    bAvatar = S.GetAvatarFromID(Encoding.Unicode.GetString(byteData, 19, posNull(19, byteData) - 19));
                            }

                        if (byteData[0] == 4)
                            if (!isHost)
                            {
                                bPR = S.FormatPR(GetPR(byteData));
                                bAvatar = S.GetAvatarFromID(GetAvatar(byteData));
                                /*if (GetHomeCity(byteData) != "")
                                    LI.HomeCity = GetHomeCity(byteData);*/

                            }
                        bHint = GetPair(IP);
                        string Date = DateTime.Now.ToLongTimeString();
                        Dispatcher.Invoke(() =>
                       {
                           try
                           {


                               if (bESO != "")
                               {
                                   var R = new Row()
                                   {
                                       IP = IP,
                                       ESO = bESO
                                   };
                                   if (!FullIP.Any(p => p.ESO == R.ESO && p.IP == R.IP))
                                       FullIP.Add(R);
                                   if (!BufferIP.Any(p => p.ESO == R.ESO && p.IP == R.IP))
                                       BufferIP.Add(R);
                                   Debug.WriteLine("3 " + R.ESO);
                               }
                           }
                           catch { }
                           try
                           {
                               if (MyView.FirstOrDefault(x => x.IP == IP).LastUpdate != Date)
                                   MyView.FirstOrDefault(x => x.IP == IP).LastUpdate = Date;
                               if (MyView.FirstOrDefault(x => x.IP == IP).Avatar == "pack://application:,,,/Avatars/cpai_avatar_random.(0,0,4,1).png" && bAvatar != "")
                                   MyView.FirstOrDefault(x => x.IP == IP).Avatar = bAvatar;
                               if (MyView.FirstOrDefault(x => x.IP == IP).PR == "" && bPR != "")
                                   MyView.FirstOrDefault(x => x.IP == IP).PR = bPR;
                               if (MyView.FirstOrDefault(x => x.IP == IP).ESO == "" && bESO != "")
                                   MyView.FirstOrDefault(x => x.IP == IP).ESO = bESO;
                               MyView.FirstOrDefault(x => x.IP == IP).Hint = bHint;
                               Debug.WriteLine("4 " + bESO);
                           }
                           catch { Debug.WriteLine("XXXXXXXXXXdsfsdfsdfXXXXXXXXXXXXXX"); }
                       });
                    }
                }
        }

        private void Program_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            Packet packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            var udpPacket = (UdpPacket)packet.Extract(typeof(UdpPacket));
            var ipPacket = (IpPacket)packet.Extract(typeof(IpPacket));
            //    captureDevice.StopCapture();
            if (udpPacket != null && ipPacket != null)
            {
                var srcIP = ipPacket.SourceAddress.ToString();
                var dstIP = ipPacket.DestinationAddress.ToString();
                var srcPort = udpPacket.SourcePort;
                var data = udpPacket.PayloadData;
                if (srcPort == 2300 || srcPort == 2301)
                {
                    ParseData(data, srcIP, dstIP);

                }

            }
            //    captureDevice.StartCapture();
        }

        private void GeneralTimer_Tick(object sender, EventArgs e)
        {
            GeneralTimer.Stop();
            try
            {
                for (int i = 0; i < MyView.Count; i++)
                    if (MyView[i].LastUpdate != null)
                        if ((int)DateTime.Now.Subtract(DateTime.Parse(MyView[i].LastUpdate)).TotalMilliseconds > 2500)
                        {
                            MyView.RemoveAt(i);
                            break;
                        }
            }
            catch { }
            GeneralTimer.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread myThread = new Thread(async delegate ()
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

            myThread.Start();


            byte[] strIP = null;
            IPHostEntry HosyEntry = Dns.GetHostEntry((Dns.GetHostName()));
            if (HosyEntry.AddressList.Length > 0)
            {
                foreach (IPAddress ip in HosyEntry.AddressList)
                {
                    strIP = ip.GetAddressBytes();
                    if (
                        (strIP[0] == 10) || // 10.
                        (strIP[0] == 172 && strIP[1] > 15 && strIP[1] < 32) || // 172.16 - 172.31
                        (strIP[0] == 192 && strIP[1] == 168) // 192.168
                        )
                        AdresseLocale = ip.ToString();
                }
            }

            foreach (SharpPcap.LibPcap.LibPcapLiveDevice dev in SharpPcap.LibPcap.LibPcapLiveDeviceList.Instance)
            {
                for (int i = 0; i < dev.Addresses.Count; i++)
                {
                    var ip = dev.Addresses[i].Addr.ipAddress;
                    if (ip != null)
                        if (ip.ToString() == AdresseLocale)
                        {
                            captureDevice = dev;
                            break;
                        }

                }
            }
            if (captureDevice != null)
            {
                captureDevice.OnPacketArrival += new PacketArrivalEventHandler(Program_OnPacketArrival);
                captureDevice.Open(DeviceMode.Promiscuous, 1000);
                captureDevice.Filter = "ip and udp";
                captureDevice.StartCapture();

            }
        }

        public MainWindow()
        {
            GamesOpen = false;
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));
            DataContext = this;
            InitializeComponent();
            SQLTimer.Tick += SQLTimer_Tick;
            SQLTimer.Start();
            GeneralTimer.Tick += GeneralTimer_Tick;
            GeneralTimer.Start();
            listView1.ItemsSource = MyView;
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

        }
        public struct VPN
        {
            public string Perc;
            public string Label;
        }
        async Task<VPN> GetVPN(string URL)
        {
            VPN res;
            res.Perc = "N/A";
            res.Label = "";
            string json = await HttpGetAsync(URL);
            try
            {
                VPNInfo vpnInfo = JsonConvert.DeserializeObject<VPNInfo>(json);
                if (!vpnInfo.success)
                    return res;
                else
                {
                    if (vpnInfo.proxy)
                    {
                        res.Perc = "100 %";
                        res.Label = (string)Application.Current.FindResource("VPN detected!");
                    }
                    else
                    {
                        double R = vpnInfo.fraud_score;
                        if (R >= 0)
                            res.Perc = Math.Round(R).ToString() + " %";
                        else
                            res.Perc = "N/A";
                        if (R >= 0 && R < 75)
                        {
                            res.Label = (string)Application.Current.FindResource("No VPN detected!");
                        }
                        else
                        {
                            res.Label = (string)Application.Current.FindResource("VPN: High probability!");
                        }

                    }
                }
            }

            catch
            {
                res.Perc = "N/A";
                res.Label = "";
            }
            return res;
        }
        public struct IP
        {
            public string flag;
            public string country;
            public string city;
            public string hour;
        }

        async Task<string> GetPingTracert(string IP)
        {
            string ping = "";
            Ping pingSender = new Ping();
            Ping pingSender2 = new Ping();
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            for (int ttl = 1; ttl <= 30; ttl++)
            {
                PingReply reply = await pingSender.SendPingAsync(IP, 1000, buffer, new PingOptions(ttl, true));
                if (reply.Status == IPStatus.Success)
                {
                    ping = Math.Min(reply.RoundtripTime, 999).ToString() + " ms";
                    break;
                }
                if (reply.Status == IPStatus.TtlExpired)
                {
                    PingReply reply2 = await pingSender2.SendPingAsync(reply.Address, 1000, buffer);
                    if (reply2.Status == IPStatus.Success)
                    {
                        ping = Math.Min(reply2.RoundtripTime, 999).ToString() + " ms";
                    }
                    continue;
                }
                if (reply.Status == IPStatus.TimedOut)
                {
                    continue;
                }
                break;
            }
            if (ping == "")
                ping = "N/A";
            return ping;
        }




        void GetPing(string IP)
        {
            Ping pingSender = new Ping();
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            pingSender.PingCompleted += async delegate (object sender, PingCompletedEventArgs e)
            {
                try
                {
                    if (e.Reply != null && e.Reply.Status == IPStatus.Success)
                    {
                        string ping = Math.Min(e.Reply.RoundtripTime, 999).ToString() + " ms";
                        Debug.WriteLine("SUCCESS");
                        {
                            await Dispatcher.InvokeAsync((Action)(() =>
                            {
                                try
                                {
                                    Debug.WriteLine("CHANGE");
                                    MyView.FirstOrDefault(x => x.IP == IP).Ping = ping;
                                }
                                catch { }
                            }));
                            await Task.Delay(1000);
                            if (MyView.Any(x => x.IP == IP))
                                GetPing(IP);
                        }
                    }
                    else
                   if (e.Reply.Status == IPStatus.TtlExpired || e.Reply.Status == IPStatus.TimedOut)
                    {
                        Debug.WriteLine("TRACERT");
                        {
                            string p = await GetPingTracert(IP);
                            await Dispatcher.InvokeAsync(() =>
                            {
                                try
                                {
                                    MyView.FirstOrDefault(x => x.IP == IP).Ping = p;
                                }
                                catch { }
                            });
                            await Task.Delay(1000);
                            if (MyView.Any(x => x.IP == IP))
                                GetPing(IP);

                        }
                    }
                    else
                    {
                        Debug.WriteLine("ELSE");
                        {
                            await Dispatcher.InvokeAsync((Action)(() =>
                            {
                                try
                                {
                                    MyView.FirstOrDefault(x => x.IP == IP).Ping = "N/A";
                                }
                                catch { }
                            }));
                            if (MyView.Any(x => x.IP == IP))
                                GetPing(IP);
                        }
                    }
                }
                catch
                {
                    Debug.WriteLine("CATCH");
                    {
                        await Dispatcher.InvokeAsync((Action)(() =>
                        {
                            try
                            {
                                MyView.FirstOrDefault(x => x.IP == IP).Ping = "N/A";
                            }
                            catch { }

                        }));
                        if (MyView.Any(x => x.IP == IP))
                            GetPing(IP);
                    }
                }
            };
            pingSender.SendAsync(IP, 1000, buffer, IP);
        }
        async Task<IP> GetIPInfo(string URI)
        {
            IP res;
            res.city = "";
            res.country = "";
            res.flag = "";
            // res.time = "";
            res.hour = "";
            string json = await HttpGetAsync(URI);
            try
            {
                IPInfo ipInfo = JsonConvert.DeserializeObject<IPInfo>(json);
                string ip = ipInfo.ip;
                string city = ipInfo.city.name_en;
                string country = ipInfo.country.name_en;
                string flag = ipInfo.country.iso;
                string utc = ipInfo.region.utc;
                if (city == "")
                    utc = ipInfo.country.utc;
                if (country != "")
                    res.country = (string)Application.Current.FindResource("● Country: ") + " " + country;
                else
                    res.country = (string)Application.Current.FindResource("● Coutry: unknown");
                if (city == "")
                    res.city = (string)Application.Current.FindResource("● City: unknown");
                else
                    res.city = (string)Application.Current.FindResource("● City: ") + " " + city;
                if (utc == "")
                {
                    //   res.time = (string)Application.Current.FindResource("● Local Time: unknown");
                    res.hour = (string)Application.Current.FindResource("● Hours Between: unknown");
                }
                else
                {
                    DateTime Times = TimeZone.CurrentTimeZone.ToUniversalTime(DateTime.Now.AddMinutes(Double.Parse(utc, new CultureInfo("en-us")) * 60));
                    //  res.time = "● Local Time: " + Times.ToString();
                    double Hours = Double.Parse(utc, new CultureInfo("en-us")) - TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
                    if (Hours > 0)
                        res.hour = (string)Application.Current.FindResource("● Hours Between: +") + Hours.ToString();
                    else
                        res.hour = (string)Application.Current.FindResource("● Hours Between: ") + " " + Hours.ToString();
                }
                try
                {
                    res.flag = "pack://application:,,,/Resources/" + flag + ".png";
                }
                catch
                {
                    res.flag = "pack://application:,,,/Resources/_unknown.png";

                }
            }
            catch
            {

            }
            return res;
        }


        async Task Upload()
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
        }

        async Task Download()
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
        }

        private async void Window_Closing(object sender, CancelEventArgs e)
        {
            await Upload();
            Environment.Exit(0);
        }

        private void Sound_Loaded(object sender, RoutedEventArgs e)
        {
            AnimationSound.Play();
        }

        private void Sound_MediaEnded(object sender, RoutedEventArgs e)
        {
            AnimationSound.Position = TimeSpan.FromSeconds(0);
        }

        private void Animation_Loaded(object sender, RoutedEventArgs e)
        {
            Animation.Play();
        }

        private void Animation_MediaEnded(object sender, RoutedEventArgs e)
        {
            Animation.Position = TimeSpan.FromSeconds(0);
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Label).Content.ToString() != "")
                Process.Start("http://aoe3.jpcommunity.com/rating2/player?n=" + (sender as Label).Content.ToString());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            App.Language = new CultureInfo("ru-RU");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            App.Language = new CultureInfo("en-US");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            App.Language = new CultureInfo("uk-UA");
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            App.Language = new CultureInfo("fr-FR");
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            App.Language = new CultureInfo("de-DE");
        }

    }
}
