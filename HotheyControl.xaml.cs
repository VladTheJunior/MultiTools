using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
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
using System.Windows.Threading;
using Tyrrrz.Extensions;

namespace MultiTools
{
    /// <summary>
    /// Логика взаимодействия для Hothey.xaml
    /// </summary>
    /// 

    public class UsedByConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            
            if (string.IsNullOrEmpty((string)parameter))
                return Visibility.Visible;
            if (string.IsNullOrEmpty((string)value))
                return Visibility.Visible;
            var curCiv = parameter.ToString();
            var usedBy = value.ToString().Split(',');
            if (usedBy.Contains(curCiv))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class Hotkey
    {



        public Key Key { get; }

        public ModifierKeys Modifiers { get; }

        public Hotkey(Key key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            if (Modifiers.HasFlag(ModifierKeys.Control))
                str.Append("Ctrl+");
            if (Modifiers.HasFlag(ModifierKeys.Shift))
                str.Append("Shift+");
            if (Modifiers.HasFlag(ModifierKeys.Alt))
                str.Append("Alt+");

            str.Append(Key);

            return str.ToString();
        }
    }
    public partial class HotheyControl : UserControl
    {

        public string UsedBy
        {
            get { return (string)GetValue(UsedByProperty); }
            set { SetValue(UsedByProperty, value); }
        }
        public string HotkeyName
        {
            get { return (string)GetValue(HotkeyNameProperty); }
            set { SetValue(HotkeyNameProperty, value); }
        }
        public ImageSource HotkeyIcon
        {
            get { return (ImageSource)GetValue(HotkeyIconProperty); }
            set { SetValue(HotkeyIconProperty, value); }
        }
        public Hotkey Hotkey
        {
            get { return (Hotkey)GetValue(HotkeyProperty); }
            set { SetValue(HotkeyProperty, value); }
        }

        public bool needSound
        {
            get { return (bool)GetValue(needSoundProperty); }
            set { SetValue(needSoundProperty, value); }
        }



        public HotheyControl()
        {
            InitializeComponent();
        }

        private static readonly SoundPlayer Click = new SoundPlayer(Application.GetResourceStream(new Uri("Click.wav", UriKind.Relative)).Stream);
        public static DependencyProperty HotkeyNameProperty = DependencyProperty.Register("HotkeyName", typeof(string), typeof(UserControl));
        public static DependencyProperty UsedByProperty = DependencyProperty.Register("UsedBy", typeof(string), typeof(UserControl));
        public static DependencyProperty HotkeyIconProperty = DependencyProperty.Register("HotkeyIcon", typeof(ImageSource), typeof(UserControl));
        public static DependencyProperty needSoundProperty = DependencyProperty.Register("needSound", typeof(bool), typeof(UserControl));
        public static DependencyProperty HotkeyProperty =
        DependencyProperty.Register(nameof(Hotkey), typeof(Hotkey), typeof(UserControl),
            new FrameworkPropertyMetadata(default(Hotkey), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (needSound == false)
                Click.Play();
            Dispatcher.BeginInvoke(
      new Action(
         delegate
         {

             if (hk.IsKeyboardFocused)
             {

                 (Application.Current.MainWindow as MainWindow).Audio.Focus();
                 Keyboard.ClearFocus();
             }
             else
             {
                 hk.Focus();
                 Keyboard.Focus(hk);
             }
         }
      )
);


        }
        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Don't let the event pass further
            // because we don't want standard textbox shortcuts working
            e.Handled = true;

            // Get modifiers and key data
            var modifiers = Keyboard.Modifiers;
            var key = e.Key;

            // When Alt is pressed, SystemKey is used instead
            if (key == Key.System)
            {
                key = e.SystemKey;
            }

            // Pressing delete, backspace or escape without modifiers clears the current value
            if (modifiers == ModifierKeys.None && key.IsEither(Key.Escape))
            {
                Hotkey = null;
                (Application.Current.MainWindow as MainWindow).Audio.Focus();
                Keyboard.ClearFocus();

                return;
            }

            // If no actual key was pressed - return
            if (key.IsEither(
                Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt,
                Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin,
                Key.Clear, Key.OemClear, Key.Apps, Key.Tab))
            {
                return;
            }

            // Set values
            Hotkey = new Hotkey(key, modifiers);
            (Application.Current.MainWindow as MainWindow).Audio.Focus();
            Keyboard.ClearFocus();

        }

        private void Hk_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
