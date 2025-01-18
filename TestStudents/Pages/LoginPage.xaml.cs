using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestStudents.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text;
            string password = PasswordBox.Password;

            using (var context = new Entities())
            {
                var user = context.Users
                                  .FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    var student = context.Students.FirstOrDefault(s => s.Id == user.Id);

                    if (student != null)
                    {
                        MessageBox.Show($"Добро пожаловать, {user.FullName}! Номер билета: {student.CardNumber}");
                        NavigationService.Navigate(new StudentPage());
                        return;
                    }

                    var teacher = context.Teachers.FirstOrDefault(t => t.Id == user.Id);

                    if (teacher != null)
                    {
                        MessageBox.Show($"Добро пожаловать, {user.FullName}!");
                        NavigationService.Navigate(new TeacherPage());
                        return;
                    }

                    MessageBox.Show("Ошибка: пользователь найден, но не является ни студентом, ни преподавателем.");
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
        }



        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}
