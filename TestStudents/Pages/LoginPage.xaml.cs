using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            using (var context = Entities.GetContext())
            {
                var user = context.Users
                                  .FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    if (user.Role == "Student")
                    {
                        MessageBox.Show("Добро пожаловать, студент!");
                    }
                    else if (user.Role == "Teacher")
                    {
                        MessageBox.Show("Добро пожаловать, преподаватель!");
                    }
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
