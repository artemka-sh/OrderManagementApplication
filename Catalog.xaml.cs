using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace OrderManagementApplication
{
    public partial class Catalog : Window
    {
        private Dictionary<string, List<RepairRequest>> userRequests = new Dictionary<string, List<RepairRequest>>();
        private string currentUser; // Имя текущего пользователя

        public Catalog(string username)
        {
            InitializeComponent();
            currentUser = username; // Сохраняем имя текущего пользователя
            LoadRequests(); // Загружаем запросы при инициализации
            UpdateRequestsList();
        }

        private void CreateCardButton_Click(object sender, RoutedEventArgs e)
        {
            var cardWindow = new CardWindow(null);
            cardWindow.RequestSaved += CardWindow_RequestSaved;
            cardWindow.ShowDialog();
        }

        private void CardWindow_RequestSaved(RepairRequest request)
        {
            if (!userRequests.ContainsKey(currentUser))
            {
                userRequests[currentUser] = new List<RepairRequest>();
            }

            userRequests[currentUser].Add(request);
            UpdateRequestsList();
            SaveRequests(); // Сохраняем запросы после добавления
        }

        private void RequestsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (RequestsListBox.SelectedItem is RepairRequest selectedRequest)
            {
                var cardWindow = new CardWindow(selectedRequest);
                cardWindow.RequestUpdated += CardWindow_RequestUpdated;
                cardWindow.ShowDialog();
            }
        }

        private void CardWindow_RequestUpdated(RepairRequest updatedRequest)
        {
            if (userRequests.ContainsKey(currentUser))
            {
                var index = userRequests[currentUser].IndexOf(updatedRequest);
                if (index >= 0)
                {
                    userRequests[currentUser][index] = updatedRequest;
                    UpdateRequestsList();
                    SaveRequests(); // Сохраняем запросы после обновления
                }
            }
        }

        private void UpdateRequestsList()
        {
            RequestsListBox.Items.Clear();
            if (userRequests.ContainsKey(currentUser))
            {
                foreach (var request in userRequests[currentUser])
                {
                    RequestsListBox.Items.Add(request);
                }
            }
        }

        private void SaveRequests()
        {
            var json = JsonConvert.SerializeObject(userRequests, Formatting.Indented);
            File.WriteAllText("repairRequests.json", json);
        }

        private void LoadRequests()
        {
            if (File.Exists("repairRequests.json"))
            {
                var json = File.ReadAllText("repairRequests.json");
                userRequests = JsonConvert.DeserializeObject<Dictionary<string, List<RepairRequest>>>(json) ?? new Dictionary<string, List<RepairRequest>>();
            }
        }
    }

    public class RepairRequest
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string FaultType { get; set; }
        public string Model { get; set; }

        public override string ToString()
        {
            return $"{FullName} - {Description}"; // Отображаем ФИО и описание в списке
        }
    }
}
