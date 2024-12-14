using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace OrderManagementApplication
{
    public partial class CardWindow : Window
    {
        public delegate void RequestSavedHandler(RepairRequest request);
        public event RequestSavedHandler RequestSaved;

        public delegate void RequestUpdatedHandler(RepairRequest request);
        public event RequestUpdatedHandler RequestUpdated;

        public delegate void RequestDeletedHandler(RepairRequest request);
        public event RequestDeletedHandler RequestDeleted;

        private RepairRequest _request;
        private bool _isPhoneTextChanging; // Флаг для отслеживания изменения текста

        public CardWindow(RepairRequest request)
        {
            InitializeComponent();
            _request = request;

            if (_request != null)
            {
                FullNameTextBox.Text = _request.FullName;
                PhoneTextBox.Text = _request.Phone;
                EmailTextBox.Text = _request.Email;
                DescriptionTextBox.Text = _request.Description;
                FaultTypeTextBox.Text = _request.FaultType;
                ModelTextBox.Text = _request.Model;

                // Устанавливаем состояние заказа
                if (!string.IsNullOrEmpty(_request.OrderStatus))
                {
                    OrderStatusComboBox.SelectedItem = OrderStatusComboBox.Items
                        .Cast<ComboBoxItem>()
                        .FirstOrDefault(item => (string)item.Content == _request.OrderStatus);
                }
            }
        }

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void PhoneTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_isPhoneTextChanging) return; // Если изменение текста уже происходит, выходим

            var textBox = sender as System.Windows.Controls.TextBox;
            string input = textBox.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            if (input.Length > 10)
            {
                input = input.Substring(0, 10);
            }

            if (input.Length > 0)
            {
                _isPhoneTextChanging = true; // Устанавливаем флаг, чтобы предотвратить повторное выполнение

                string formatted = string.Empty;

                if (input.Length >= 3)
                {
                    formatted += "(" + input.Substring(0, 3) + ") ";
                    if (input.Length >= 6)
                    {
                        formatted += input.Substring(3, 3) + "-";
                        if (input.Length >= 10)
                        {
                            formatted += input.Substring(6, 4);
                        }
                        else
                        {
                            formatted += input.Substring(6);
                        }
                    }
                    else
                    {
                        formatted += input.Substring(3);
                    }
                }
                else
                {
                    formatted = input; // Если меньше 3 символов, просто выводим их
                }

                textBox.Text = formatted;
                textBox.SelectionStart = textBox.Text.Length; // Устанавливаем курсор в конец
                _isPhoneTextChanging = false; // Сбрасываем флаг
            }
        }

        private void EmailTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            string email = textBox.Text;

            if (!IsValidEmail(email))
            {
                // Здесь можно добавить логику для обработки некорректного ввода
            }
        }

        private bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text, @"^[0-9]+$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_request != null)
            {
                // Вызываем событие удаления и передаем текущий запрос
                RequestDeleted?.Invoke(_request);
            }
            this.Close();
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
            // Устанавливаем состояние заказа
            if (OrderStatusComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                _request.OrderStatus = (string)selectedItem.Content; // Получаем текстовое значение
            }
            else
            {
                _request.OrderStatus = null; // Если ничего не выбрано, устанавливаем null или другое значение по умолчанию
            }

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
