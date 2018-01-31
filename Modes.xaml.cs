using ESO_Assistant.Classes;
using System.IO.Compression;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace MultiTools
{
    /// <summary>
    /// Логика взаимодействия для Modes.xaml
    /// </summary>
    public partial class Modes : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
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


        SoundPlayer Click = new SoundPlayer(Application.GetResourceStream(new Uri("Click.wav", UriKind.Relative)).Stream);

        public Modes()
        {
            DataContext = this;
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
            InitializeComponent();
            using (RegistryKey R = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
            {
                object P = R.GetValue("setuppath");
                if (P != null)
                {
                    FilePath = P.ToString();
                    if (File.Exists(Path.Combine(FilePath, "DataPY.bar")))
                        if (GetMD5(Path.Combine(FilePath, "DataPY.bar")) == "e6‌​c0‌​d4‌​0d‌​08‌​86‌​f5‌​97‌​a0‌​85‌​46‌​63‌​2b‌​53‌​6a‌​5d")
                        {
                            bDisableEnable.IsChecked = false;
                        }
                        else
                        {
                            bDisableEnable.Checked -= bEnable_Click;
                            bDisableEnable.IsChecked = true;
                            bDisableEnable.Checked += bEnable_Click;
                        }
                }
                else
                {
                    bDisableEnable.IsChecked = false;
                }
            }
            AoE = new Cursor(Application.GetResourceStream(new Uri("Cursor/AoE.cur", UriKind.Relative)).Stream);
            Colors = typeof(Brushes).GetProperties();
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

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Click.Play();

        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private async void bEnable_Click(object sender, RoutedEventArgs e)
        {
            bDisableEnable.IsEnabled = false;
            try
            {
                if (File.Exists(Path.Combine(FilePath, "DataPY.bar")))
                {
                    using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Mod.DataPY.zip")))
                    {
                        await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                        {
                            if (x.Name == "")
                                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(FilePath, x.FullName)));
                            else
                                x.ExtractToFile(Path.Combine(FilePath, x.FullName), true);
                        }));
                    }
                }
            }
            catch { }
            bDisableEnable.IsEnabled = true;

        }

        private async void bDisable_Click(object sender, RoutedEventArgs e)
        {
            bDisableEnable.IsEnabled = false;
            try
            {
                if (File.Exists(Path.Combine(FilePath, "DataPY.bar")))
                {
                    using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Original.DataPY.zip")))
                    {
                        await Task.Run(() => zip.Entries.ToList().ForEach(x =>
                        {
                            if (x.Name == "")
                                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(FilePath, x.FullName)));
                            else
                                x.ExtractToFile(Path.Combine(FilePath, x.FullName), true);
                        }));
                    }
                }
            }
            catch { }
            bDisableEnable.IsEnabled = true;
        }

        private void bNote_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((string)Application.Current.FindResource("This mod will level up your homecity by 100.") + Environment.NewLine +
(string)Application.Current.FindResource("I am not responsible for the consequences of using this mod!") + Environment.NewLine + Environment.NewLine +
(string)Application.Current.FindResource("1. Find a partner with the same mod.") + Environment.NewLine +
(string)Application.Current.FindResource("2. Enable mod.") + Environment.NewLine +
(string)Application.Current.FindResource("3. Start the game.") + Environment.NewLine +
(string)Application.Current.FindResource("4. Play a game on fast mod.") + Environment.NewLine +
(string)Application.Current.FindResource("5. Wait 2 minutes and resign (the winner will receive the XP).") + Environment.NewLine +
(string)Application.Current.FindResource("6. Close the game.") + Environment.NewLine +
(string)Application.Current.FindResource("7. Disable mod.") + Environment.NewLine +
(string)Application.Current.FindResource("8. Done!"));
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (gGraphics.Visibility == Visibility.Collapsed && gInterface.Visibility == Visibility.Collapsed && gMods.Visibility == Visibility.Collapsed)
            {
                rbDefault.IsChecked = true;
            }
            rbEkantaUI.IsChecked = Paths.isEkantaInstalled();
            rbJammsUI.IsChecked = Paths.isJammsInstalled();
            rbQazUI.IsChecked = Paths.isQazInstalled();
            tbArty.IsChecked = Paths.isArtyInstalled();
            tbKeys.IsChecked = Paths.isKeysInstalled();


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
            udMin.ValueChanged -= udMin_ValueChanged;
            udNorm.ValueChanged -= udNorm_ValueChanged;

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
                        int z1 = S.FindIndex(x => x.StartsWith("maxZoom"));

                        if (z1 >= 0)
                            udMax.Value = Double.Parse(S[z1].Split('=')[1]);
                        else
                            udMax.Value = 60;
                        int z2 = S.FindIndex(x => x.StartsWith("normalZoom"));
                        if (z2 >= 0)
                            udNorm.Value = Double.Parse(S[z2].Split('=')[1]);
                        else
                            udNorm.Value = 50;
                        int z3 = S.FindIndex(x => x.StartsWith("minZoom"));
                        if (z3 >= 0)
                            udMin.Value = Double.Parse(S[z3].Split('=')[1]);
                        else
                            udMin.Value = 29;
                    }
                    else
                    {
                        udMax.Value = 60;
                        udNorm.Value = 50;
                        udMin.Value = 29;
                    }

                    if (File.Exists(Path.Combine(P.ToString(), "Startup", "user.con")))
                    {
                        string M = File.ReadAllText(Path.Combine(P.ToString(), "Startup", "user.con"));
                        miRotator.IsChecked = M.Contains("uiWheelRotatePlacedUnit");
                    }
                }
            }
            udMax.ValueChanged += udMax_ValueChanged;
            udMin.ValueChanged += udMin_ValueChanged;
            udNorm.ValueChanged += udNorm_ValueChanged;
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

        private void udNorm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
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
                            int z2 = S.FindIndex(x => x.StartsWith("normalZoom="));
                            if (z2 >= 0)
                                S.RemoveAt(z2);
                            S.Add("normalZoom=" + Math.Round(udNorm.Value).ToString());
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg"), S.ToArray());
                        }
                        else
                        {
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.cfg"), true))
                            {
                                w.WriteLine("normalZoom=" + Math.Round(udMax.Value).ToString());
                            }
                        }

                }
            }
            catch { }
        }

        private void udMin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
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
                            int z3 = S.FindIndex(x => x.StartsWith("minZoom="));
                            if (z3 >= 0)
                                S.RemoveAt(z3);
                            S.Add("minZoom=" + Math.Round(udMin.Value).ToString());
                            File.WriteAllLines(Path.Combine(P.ToString(), "Startup", "user.cfg"), S.ToArray());
                        }
                        else
                        {
                            using (StreamWriter w = new StreamWriter(Path.Combine(P.ToString(), "Startup", "user.cfg"), true))
                            {
                                w.WriteLine("minZoom=" + Math.Round(udMax.Value).ToString());
                            }
                        }

                }
            }
            catch { }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
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
                            udMin.ValueChanged -= udMin_ValueChanged;
                            udNorm.ValueChanged -= udNorm_ValueChanged;
                            udMax.Value = 60;
                            udNorm.Value = 50;
                            udMin.Value = 29;
                            udMax.ValueChanged += udMax_ValueChanged;
                            udMin.ValueChanged += udMin_ValueChanged;
                            udNorm.ValueChanged += udNorm_ValueChanged;
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
                            udMin.ValueChanged -= udMin_ValueChanged;
                            udNorm.ValueChanged -= udNorm_ValueChanged;
                            udMax.Value = 60;
                            udNorm.Value = 50;
                            udMin.Value = 29;
                            udMax.ValueChanged += udMax_ValueChanged;
                            udMin.ValueChanged += udMin_ValueChanged;
                            udNorm.ValueChanged += udNorm_ValueChanged;
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

        private void RadioButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            gMods.Visibility = Visibility.Collapsed;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            gMods.Visibility = Visibility.Visible;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            gGraphics.Visibility = Visibility.Visible;
        }

        private void RadioButton_Unchecked_2(object sender, RoutedEventArgs e)
        {
            gGraphics.Visibility = Visibility.Collapsed;

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

        private void Button_Click(object sender, RoutedEventArgs e)
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

        private async void tbKeys_Checked(object sender, RoutedEventArgs e)
        {
            tbKeys.IsEnabled = false;
            try
            {
                using (RegistryKey AS = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\microsoft games\\age of empires 3 expansion pack 2\\1.0"))
                {
                    object P = AS.GetValue("setuppath");

                    if (P != null)
                    {
                        if (!Paths.isKeysInstalled())
                        {
                            using (ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("MultiTools.Mods.Keys.zip")))
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
            tbKeys.IsEnabled = true;
        }

        private void tbKeys_Unchecked(object sender, RoutedEventArgs e)
        {
            tbKeys.IsEnabled = false;
            try
            {
                Paths.RemoveKeysIfInstalled();
            }
            catch { }
            tbKeys.IsEnabled = true;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
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

        private async void Button_Click_2(object sender, RoutedEventArgs e)
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
            this.Close();
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
    }
}
