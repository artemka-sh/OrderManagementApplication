using System.Collections.Generic;
using System.Windows;

namespace OrderManagementApplication
{
    public partial class Catalog : Window
    {
        private List<RepairRequest> repairRequests = new List<RepairRequest>();

        public Catalog()
        {
            InitializeComponent();
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
            repairRequests.Add(request);
            UpdateRequestsList();
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
            var index = repairRequests.IndexOf(updatedRequest);
            if (index >= 0)
            {
                repairRequests[index] = updatedRequest;
                UpdateRequestsList();
            }
        }

        private void UpdateRequestsList()
        {
            RequestsListBox.Items.Clear();
            foreach (var request in repairRequests)
            {
                RequestsListBox.Items.Add(request);
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
