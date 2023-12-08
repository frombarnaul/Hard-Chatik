using Hard_Chatik;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Hard_Chatik_WPF
{
    public partial class MainWindow : Window
    {
        private int userID;
        public MainWindow()
        {
            InitializeComponent();
            GetHardWareInfo();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DbAccess dbAccess = new DbAccess();
            chatListBox.ItemsSource = dbAccess.LoadChatMessages();
        }


        private void ButtonReg(object sender, RoutedEventArgs e)
        {
            DbAccess dbAccess = new DbAccess();
            if (loginBox.Text.Length >= 6 && passwordBox.Text.Length >= 6)
            {
                dbAccess.UserReg(loginBox.Text, passwordBox.Text);
            }
            else
            {
                MessageBox.Show("Пароль и логин должны быть не менее 6 символов");
            }

        }
        private void ButtonAuth(object sender, RoutedEventArgs e)
        {
            DbAccess dbAccess = new DbAccess();

            if(dbAccess.UserAuth(loginBox.Text, passwordBox.Text, out userID))
            {
                circle.Fill = new SolidColorBrush(Colors.Green);
                statusBox.Visibility = Visibility.Hidden;
                loginText.Text = "Авторизован под:";
                regButton.IsEnabled = false;
                passwordText.Visibility = Visibility.Hidden;
                authButton.Visibility = Visibility.Hidden;
                regButton.Visibility = Visibility.Hidden;
                loginBox.IsEnabled = false;
                passwordBox.Visibility = Visibility.Hidden;
                chatText.IsEnabled = true;
                buttonSendMessage.IsEnabled = true;

                var timer = new DispatcherTimer();
                var timerTick = 1;
                timer.Interval = TimeSpan.FromSeconds(timerTick);
                timer.Tick += Timer_Tick;

                timer.Start();
            }
        }

        private void ButtonSendMessage(object sender, RoutedEventArgs e)
        {
            DbAccess dbAccess = new DbAccess();
            dbAccess.SendMessage(userID, chatText.Text);
            chatText.Text = "";



        }

        private void GetHardWareInfo()
        {
            HardWare hardware = new HardWare();
            baseText.Text = hardware.GetBaseInfo();
            procText.Text = hardware.GetProcessorInfo();
            videoText.Text = hardware.GetVideoInfo();
            ramText.Text = hardware.GetRamInfo();
            memoryText.Text = hardware.GetMemoryInfo();
        }
    }
}