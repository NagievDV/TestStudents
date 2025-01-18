using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestStudents.Pages
{
    public partial class TeacherPage : Page
    {
        private Entities _context;
        private List<object> _originalData; 

        public TeacherPage()
        {
            InitializeComponent();
            _context = new Entities();
            _originalData = new List<object>();
        }

        private void ShowTestingResultsButton_Click(object sender, RoutedEventArgs e)
        {
            _context.TestingResults.Load();
            _originalData = _context.TestingResults.Local.ToList<object>(); 
            DataGridDisplay.ItemsSource = _originalData;
        }

        private void ShowQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            _context.Questions.Load();
            _originalData = _context.Questions.Local.ToList<object>(); 
            DataGridDisplay.ItemsSource = _originalData;
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем текущий источник данных
                if (DataGridDisplay.ItemsSource is ICollection<object> items)
                {
                    // Определяем тип данных (TestingResults или Questions)
                    if (items.Any() && items.First() is TestingResults)
                    {
                        foreach (var item in items)
                        {
                            var entity = item as TestingResults;
                            if (entity != null && _context.Entry(entity).State == System.Data.Entity.EntityState.Detached)
                            {
                                _context.TestingResults.Add(entity);
                            }
                        }
                    }
                    else if (items.Any() && items.First() is Questions)
                    {
                        foreach (var item in items)
                        {
                            var entity = item as Questions;
                            if (entity != null && _context.Entry(entity).State == System.Data.Entity.EntityState.Detached)
                            {
                                _context.Questions.Add(entity);
                            }
                        }
                    }
                }

                // Сохраняем изменения в контексте
                _context.SaveChanges();
                MessageBox.Show("Изменения успешно сохранены.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter(FilterTextBox.Text);
        }

        private void ApplyFilter(string filterText)
        {
            if (string.IsNullOrWhiteSpace(filterText))
            {
                DataGridDisplay.ItemsSource = _originalData;
                return;
            }

            DataGridDisplay.ItemsSource = _originalData
                .Where(item =>
                {
                    foreach (var property in item.GetType().GetProperties())
                    {
                        var value = property.GetValue(item)?.ToString();
                        if (!string.IsNullOrEmpty(value) && value.ToLower().Contains(filterText.ToLower()))
                            return true;
                    }
                    return false;
                })
                .ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TestReport());
        }
    }
}
