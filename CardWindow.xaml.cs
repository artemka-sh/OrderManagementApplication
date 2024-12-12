using System.Windows;

namespace OrderManagementApplication
{
    public partial class CardWindow : Window
    {
        public delegate void RequestSavedHandler(RepairRequest request);
        public event RequestSavedHandler RequestSaved;

        public delegate void RequestUpdatedHandler(RepairRequest request);
        public event RequestUpdatedHandler RequestUpdated;

        private RepairRequest _request;

        public CardWindow(RepairRequest request)
        {
            InitializeComponent();
            _request = request;

            if (_request != null)
            {
                // Заполняем поля данными из заявки
                FullNameTextBox.Text = _request.FullName;
                PhoneTextBox.Text = _request.Phone;
                EmailTextBox.Text = _request.Email;
                DescriptionTextBox.Text = _request.Description;
                FaultTypeTextBox.Text = _request.FaultType;
                ModelTextBox.Text = _request.Model;
            }
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (_request == null)
            {
                _request = new RepairRequest();
            }

            // Сохраняем данные из полей
            _request.FullName = FullNameTextBox.Text;
            _request.Phone = PhoneTextBox.Text;
            _request.Email = EmailTextBox.Text;
            _request.Description = DescriptionTextBox.Text;
            _request.FaultType = FaultTypeTextBox.Text;
            _request.Model = ModelTextBox.Text;

            // Вызываем соответствующее событие в зависимости от того, создается ли новая заявка или обновляется существующая
            if (_request.FullName != null && _request.Description != null) // Проверка на наличие данных
            {
                if (RequestSaved != null && _request != null)
                {
                    RequestSaved(_request);
                }
                else if (RequestUpdated != null && _request != null)
                {
                    RequestUpdated(_request);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            this.Close();
        }
    }
}
