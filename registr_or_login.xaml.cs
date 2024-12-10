using System.Windows;


namespace OrderManagementApplication
{
    public partial class registr_or_login : Window
    {
        public registr_or_login()
        {
            InitializeComponent();
        }

        private void ShowLoginPanel(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowRegisterPanel(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginUsernameTextBox.Text;
            string password = LoginPasswordBox.Password;

            // Выводим введенные данные
            MessageBox.Show($"Вход:\nИмя пользователя: {username}\nПароль: {password}", "Информация о входе");
            Catalog catalogwin = new Catalog();
            catalogwin.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = RegisterUsernameTextBox.Text;
            string password = RegisterPasswordBox.Password;

            // Выводим введенные данные
            MessageBox.Show($"Регистрация:\nИмя пользователя: {username}\nПароль: {password}", "Информация о регистрации");
        }
    }
}
