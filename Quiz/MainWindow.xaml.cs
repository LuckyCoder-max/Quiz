﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Quiz
{
    public partial class MainWindow : Window
    {
        private class Question
        {
            public string Text { get; set; }
            public string[] Options { get; set; }
            public int CorrectIndex { get; set; }
        }

        private readonly Dictionary<string, List<Question>> quizzes = new()
        {
            ["Общее"] = new()
            {
                new Question { Text = "Какой сейчас год?", Options = new[] { DateTime.Now.Year.ToString(), "2020", "2019", "2021" }, CorrectIndex = 0 },
                new Question { Text = "Сколько дней в неделе?", Options = new[] { "5", "6", "7", "8" }, CorrectIndex = 2 },
                new Question { Text = "Столица Франции?", Options = new[] { "Париж", "Берлин", "Лондон", "Мадрид" }, CorrectIndex = 0 }
            },
            ["IT"] = new()
            {
                new Question { Text = "Что означает HTML?", Options = new[] { "HyperText Markup Language", "HighText Markup Language", "HyperText Markdown Language", "HyperTransfer Markup Language" }, CorrectIndex = 0 },
                new Question { Text = "Какой язык используется для стилизации веб-страниц?", Options = new[] { "HTML", "C#", "CSS", "JavaScript" }, CorrectIndex = 2 },
                new Question { Text = "Какой метод запускает программу в C#?", Options = new[] { "Start", "Main", "Run", "Init" }, CorrectIndex = 1 }
            },
            ["Математика"] = new()
            {
                new Question { Text = "Сколько будет 5 + 7?", Options = new[] { "10", "11", "12", "13" }, CorrectIndex = 2 },
                new Question { Text = "Корень из 81?", Options = new[] { "7", "8", "9", "10" }, CorrectIndex = 2 },
                new Question { Text = "Сколько градусов в прямом угле?", Options = new[] { "45", "90", "180", "360" }, CorrectIndex = 1 }
            }
        };

        private string currentCategory = "";
        private int currentQuestionIndex = 0;
        private int score = 0;
        private bool answered = false;

        private Button[] AnswerButtons;


        public MainWindow()
        {
            InitializeComponent();
            AnswerButtons = new[] { AnswerButton0, AnswerButton1, AnswerButton2, AnswerButton3 };
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                currentCategory = btn.Content.ToString();
                currentQuestionIndex = 0;
                score = 0;
                CategoryPanel.Visibility = Visibility.Collapsed;
                ShowQuestion();
            }
        }

        private void ShowQuestion()
        {
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
                QuestionText.Text = $"Викторина завершена!\nВаш результат: {score} баллов";
                foreach (var btn in AnswerButtons)
                    btn.Visibility = Visibility.Collapsed;
                RestartButton.Visibility = Visibility.Visible;
                return;
            }

            var q = questions[currentQuestionIndex];

            QuestionText.Text = q.Text;

            for (int i = 0; i < AnswerButtons.Length; i++)
            {
                AnswerButtons[i].Content = q.Options[i];
                AnswerButtons[i].Background = (Brush)new BrushConverter().ConvertFromString("#2E2E3E");
                AnswerButtons[i].IsEnabled = true;
                AnswerButtons[i].Visibility = Visibility.Visible;
            }

            UpdateScoreDisplay();
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (answered) return;

            if (sender is Button clickedButton)
            {
                answered = true;

                int selectedIndex = Array.IndexOf(AnswerButtons, clickedButton);
                var q = quizzes[currentCategory][currentQuestionIndex];

                foreach (var btn in AnswerButtons)
                    btn.IsEnabled = false;

                if (selectedIndex == q.CorrectIndex)
                {
                    clickedButton.Background = Brushes.LightGreen;
                    FeedbackText.Foreground = Brushes.Green;
                    FeedbackText.Text = "Правильно!";
                    score += 2;
                }
                else
                {
                    clickedButton.Background = Brushes.IndianRed;
                    AnswerButtons[q.CorrectIndex].Background = Brushes.LightGreen;
                    FeedbackText.Foreground = Brushes.Red;
                    FeedbackText.Text = $"Неправильно! Правильный ответ: {q.Options[q.CorrectIndex]}";
                    score = Math.Max(0, score - 1);
                }

                UpdateScoreDisplay();
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
            QuizPanel.Visibility = Visibility.Collapsed;
            CategoryPanel.Visibility = Visibility.Visible;
            currentCategory = "";
            currentQuestionIndex = 0;
            score = 0;
            answered = false;
        }

        private void UpdateScoreDisplay()
        {
            ScoreText.Text = $"Очки: {score}";
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
