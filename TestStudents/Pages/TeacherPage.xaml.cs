using System.Data.Entity;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestStudents.Pages
{
    public partial class TeacherPage : Page
    {
        private Entities _context;

        public TeacherPage()
        {
            InitializeComponent();
            _context = new Entities();
        }

        private void ShowTestingResultsButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridDisplay.ItemsSource = _context.TestingResults.Local;
            _context.TestingResults.Load();
        }

        private void ShowQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridDisplay.ItemsSource = _context.Questions.Local;
            _context.Questions.Load();
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
                _context.SaveChanges();
                MessageBox.Show("Изменения успешно сохранены.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }
    }
}
