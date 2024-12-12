using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace OrderManagementApplication
{
    public partial class registr_or_login : Window
    {
        private const string usersFilePath = "users.json"; // Путь к файлу для хранения пользователей
        private List<User> users = new List<User>();

        public registr_or_login()
        {
            InitializeComponent();
            LoadUsers(); // Загружаем пользователей при инициализации
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

            // Проверяем, существует ли пользователь с таким именем и паролем
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                MessageBox.Show($"Вход:\nИмя пользователя: {username}", "Информация о входе");
                Catalog catalogwin = new Catalog(username); // Передаем имя пользователя
                catalogwin.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка входа");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = RegisterUsernameTextBox.Text;
            string password = RegisterPasswordBox.Password;

            // Проверяем, существует ли уже пользователь с таким именем
            if (users.Any(u => u.Username == username))
            {
                MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка регистрации");
                return;
            }

            // Создаем нового пользователя и добавляем его в список
            var newUser = new User { Username = username, Password = password };
            users.Add(newUser);
            SaveUsers(); // Сохраняем пользователей после регистрации

            MessageBox.Show($"Регистрация:\nИмя пользователя: {username}", "Информация о регистрации");
        }

        private void SaveUsers()
        {
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(usersFilePath, json);
        }

        private void LoadUsers()
        {
            if (File.Exists(usersFilePath))
            {
                try
                {
                    var json = File.ReadAllText(usersFilePath);
                    users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
