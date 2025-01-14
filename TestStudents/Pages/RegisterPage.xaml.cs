using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestStudents.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text;
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            try
            {
                Entities db = new Entities();

                var existingUser = db.Users.FirstOrDefault(u => u.Login == login);

                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }

                var newUser = new Users
                {
                    FullName = fullName,
                    Login = login,
                    Password = password,
                    Role = role,
                    CardNumber = role == "Student" ? GenerateStudentCardNumber() : null
                };

                db.Users.Add(newUser);
                db.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно.");
                NavigationService.Navigate(new LoginPage()); // Навигация на страницу авторизации
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
            }
        }

        private string GenerateStudentCardNumber()
        {
            Random random = new Random();
            return "STU" + random.Next(10000, 99999).ToString();
        }
    }
}
