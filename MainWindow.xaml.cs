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

namespace OrderManagementApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded; // Подписываемся на событие загрузки окна
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {



            await Task.Delay(1000); 


            
            registr_or_login authWindow = new registr_or_login();
            authWindow.Show();
            this.Close();
        }
    }
}