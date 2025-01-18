using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestStudents.Pages
{
    public partial class TestReport : Page
    {
        private readonly Entities _context;

        public TestReport()
        {
            InitializeComponent();
            _context = new Entities();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SearchTextBox.Text, out int testId))
            {
                var results = _context.TestingResults
                                      .Where(r => r.TestId == testId)
                                      .ToList();

                if (results.Any())
                {
                    var testDate = results.First().TestDate;
                    var StudentCard = results.First().CardNumber;
                    var testDuration = results.First().TestDurationMinutes;
                    var totalQuestions = results.First().TotalQuestions;
                    var correctAnswers = results.First().CorrectAnswersCount;
                    var grade = results.First().Grade;

                    TestDateText.Text = testDate.ToString("dd.MM.yyyy");
                    TestDurationText.Text = testDuration.ToString();
                    TotalQuestionsText.Text = totalQuestions.ToString();
                    CorrectAnswersText.Text = correctAnswers.ToString();
                    GradeText.Text = grade.ToString();
                    StudentNumberText.Text = StudentCard;
                }
                else
                {
                    MessageBox.Show("Результаты для указанного теста не найдены.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Введите корректный ID теста.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                ClearFields();
            }
        }

        private void ClearFields()
        {
            TestDateText.Text = string.Empty;
            TestDurationText.Text = string.Empty;
            TotalQuestionsText.Text = string.Empty;
            CorrectAnswersText.Text = string.Empty;
            GradeText.Text = string.Empty;
            StudentNumberText.Text = string.Empty;
        }
    }
}
