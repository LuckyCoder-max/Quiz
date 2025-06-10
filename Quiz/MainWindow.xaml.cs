using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Quiz
{
    public partial class MainWindow : Window
    {

        private class Question
        {
            public string Text { get; set; } = "";
            public string[] Options { get; set; } = Array.Empty<string>();
            public int CorrectIndex { get; set; }
        }


        private readonly Dictionary<string, List<Question>> quizzes = new()
        {
            ["Общее"] = new List<Question>
            {
                new Question
                {
                    Text = "Какой сейчас год?",
                    Options = new [] { DateTime.Now.Year.ToString(), "2020", "2019", "2021" },
                    CorrectIndex = 0
                },
                new Question
                {
                    Text = "Сколько дней в неделе?",
                    Options = new [] { "5", "6", "7", "8" },
                    CorrectIndex = 2
                },
                new Question
                {
                    Text = "Столица Франции?",
                    Options = new [] { "Париж", "Берлин", "Лондон", "Мадрид" },
                    CorrectIndex = 0
                }
            },
            ["IT"] = new List<Question>
            {
                new Question
                {
                    Text = "Что означает HTML?",
                    Options = new [] { "HyperText Markup Language", "HighText Markup Language", "HyperText Markdown Language", "HyperTransfer Markup Language" },
                    CorrectIndex = 0
                },
                new Question
                {
                    Text = "Какой язык используется для стилизации веб-страниц?",
                    Options = new [] { "HTML", "C#", "CSS", "JavaScript" },
                    CorrectIndex = 2
                },
                new Question
                {
                    Text = "Какой метод запускает программу в C#?",
                    Options = new [] { "Start", "Main", "Run", "Init" },
                    CorrectIndex = 1
                }
            },
            ["Математика"] = new List<Question>
            {
                new Question
                {
                    Text = "Сколько будет 5 + 7?",
                    Options = new [] { "10", "11", "12", "13" },
                    CorrectIndex = 2
                },
                new Question
                {
                    Text = "Корень из 81?",
                    Options = new [] { "7", "8", "9", "10" },
                    CorrectIndex = 2
                },
                new Question
                {
                    Text = "Сколько градусов в прямом угле?",
                    Options = new [] { "45", "90", "180", "360" },
                    CorrectIndex = 1
                }
            }
        };

        private string currentCategory = "";
        private int currentQuestionIndex = 0;
        private bool answered = false;

        public MainWindow()
        {
            InitializeComponent();
            ShowQuizSelection();
        }


        private void ShowQuizSelection()
        {
            QuizSelectionPanel.Visibility = Visibility.Visible;
            QuizPanel.Visibility = Visibility.Collapsed;
            FeedbackText.Text = "";
            RestartButton.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                currentCategory = btn.Content.ToString() ?? "";
                currentQuestionIndex = 0;
                ShowQuestion();
            }
        }


        private void ShowQuestion()
        {
            QuizSelectionPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Visible;
            FeedbackText.Text = "";
            NextButton.Visibility = Visibility.Collapsed;
            RestartButton.Visibility = Visibility.Collapsed;
            answered = false;

            if (!quizzes.ContainsKey(currentCategory))
                return;

            var questions = quizzes[currentCategory];
            if (currentQuestionIndex >= questions.Count)
            {
                QuestionText.Text = "Викторина завершена! Спасибо за участие.";
                HideAnswerButtons();
                RestartButton.Visibility = Visibility.Visible;
                return;
            }

            var q = questions[currentQuestionIndex];
            QuestionText.Text = q.Text;

            var buttons = new[] { AnswerButton1, AnswerButton2, AnswerButton3, AnswerButton4 };
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Content = q.Options[i];
                buttons[i].Background = Brushes.LightGray;
                buttons[i].IsEnabled = true;
                buttons[i].Visibility = Visibility.Visible;
                buttons[i].Foreground = Brushes.Black;
            }
        }

        // Скрыть кнопки ответов
        private void HideAnswerButtons()
        {
            var buttons = new[] { AnswerButton1, AnswerButton2, AnswerButton3, AnswerButton4 };
            foreach (var btn in buttons)
            {
                btn.Visibility = Visibility.Collapsed;
            }
        }
        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (answered)
                return;

            if (sender is Button clickedButton)
            {
                answered = true;

                var buttons = new[] { AnswerButton1, AnswerButton2, AnswerButton3, AnswerButton4 };
                int selectedIndex = Array.IndexOf(buttons, clickedButton);

                var q = quizzes[currentCategory][currentQuestionIndex];


                foreach (var btn in buttons)
                    btn.IsEnabled = false;

                if (selectedIndex == q.CorrectIndex)
                {
                    clickedButton.Background = Brushes.LightGreen;
                    FeedbackText.Foreground = Brushes.Green;
                    FeedbackText.Text = "Правильно!";
                }
                else
                {
                    clickedButton.Background = Brushes.IndianRed;
                    buttons[q.CorrectIndex].Background = Brushes.LightGreen;
                    FeedbackText.Foreground = Brushes.Red;
                    FeedbackText.Text = $"Неправильно! Правильный ответ: {q.Options[q.CorrectIndex]}";
                }

                NextButton.Visibility = Visibility.Visible;
                RestartButton.Visibility = Visibility.Visible;
            }
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            ShowQuestion();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ShowQuizSelection();
        }
    }
}
