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
            RoleComboBox.SelectionChanged += RoleComboBox_SelectionChanged;
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRole = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            StudentFieldsPanel.Visibility = selectedRole == "Student" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text;
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string cardNumber = CardNumberTextBox.Text;
            string group = GroupTextBox.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            if (role == "Student" && (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(group)))
            {
                MessageBox.Show("Для студентов номер билета и группа обязательны.");
                return;
            }

            try
            {
                using (Entities db = new Entities())
                {
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
                        Password = password
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    if (role == "Student")
                    {
                        var newStudent = new Students
                        {
                            Id = newUser.Id,
                            CardNumber = cardNumber,
                            Group = group
                        };
                        db.Students.Add(newStudent);
                    }
                    else if (role == "Teacher")
                    {
                        var newTeacher = new Teachers
                        {
                            Id = newUser.Id
                        };
                        db.Teachers.Add(newTeacher);
                    }

                    db.SaveChanges();
                    MessageBox.Show("Регистрация прошла успешно.");
                    NavigationService.Navigate(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
            }
        }
    }
}
