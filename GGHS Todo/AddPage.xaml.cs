#nullable enable

using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using RollingRess;

namespace GGHS_Todo
{
    public sealed partial class AddPage : Page
    {
        public static TodoTask? Task { get; set; } = null;
        public static List<string> Grade1 => new()
        {
            "국어",
            "수학",
            "영어",
            "한국사",
            "통합사회",
            "통합과학",
            "과학탐구실험",
            "스포츠 생활",
            "미술창작",
            "음악연주",
            "전공기초 스페인어",
            "전공기초 일본어",
            "전공기초 중국어",
            "국제관계의 이해",
        };

        public static List<string> Grade2 => new()
        {
            "문학",
            "수학Ⅰ",
            "수학과제탐구",
            "경제",
            "세계사",
            "세계지리",
            "정치와 법",
            "동아시아사",
            "한국지리",
            "사회/문화",
            "생활과 윤리",
            "윤리와 사상",
            "물리학Ⅰ",
            "화학Ⅰ",
            "생명과학Ⅰ",
            "운동과 건강",
            "창의적문제해결기법",
            "스페인어Ⅰ",
            "중국어Ⅰ",
            "일본어Ⅰ",
            "비판적 영어 글쓰기와 말하기",
        };

        public static List<string> Grade3 => new()
        {
            "언어와 매체",
            "화법과 작문",
            "확률과 통계",
            "미적분",
            "영미 문학 읽기",
            "동아시아사",
            "한국지리",
            "사회/문화",
            "체육",
            "스페인어권 문화",
            "일본문화",
            "중국문화",
            "심화영어Ⅱ",
            "독서와 의사소통",
            "사회 탐구 방법",
            "한국 사회의 이해",
            "세계 문제와 미래사회",
            "윤리학 연습",
        };

        public List<string> Subjects { get; } = new();

        public AddPage()
        {
            InitializeComponent();

            Subjects = Grade1.Concat(Grade2).Concat(Grade3).Distinct().ToList();
            Subjects.Add("기타");

            GradeRadioButtons.SelectedIndex = MainPage.Grade switch
            {
                Grades.Grade1 => 0,
                Grades.Grade2 => 1,
                Grades.Grade3 => 2,
                Grades.None => -1,
                _ => throw new ArgumentException()
            };
            SubjectPicker.ItemsSource = Subjects;

            if (Task is not null) // Clicked a button
            {
                InitializeComponent();
                DeleteButton.Visibility = Visibility.Visible;
                mainText.Text = "Modify Task";
                DueDatePicker.Date = Task.DueDate;
                SubjectPicker.SelectedItem = Task.Subject;
                TitleTextBox.Text = Task.Title;
                if (Task.Body is not null)
                    BodyTextBox.Text = Task.Body;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DueDatePicker.SelectedDate is null || SubjectPicker.SelectedIndex is -1 || TitleTextBox.IsNullOrWhiteSpace())
            {
                await ShowMessageAsync("Date, subject and title are required.", "Error");
                return;
            }

            var date = DueDatePicker.Date.DateTime;
            TodoTask task = new(new(date.Year, date.Month, date.Day), SubjectPicker.SelectedItem as string, TitleTextBox.Text, 
                string.IsNullOrWhiteSpace(BodyTextBox.Text) ? null : BodyTextBox.IsNullOrWhiteSpace() ? null : BodyTextBox.Text);

            if (Task is not null)
            {
                int idx = MainPage.TaskList.FindIndex(x => x == Task);
                MainPage.TaskList[idx] = task;
            }
            else
            {
                MainPage.TaskList.Add(task);
            }
            MainPage.TaskList.Sort();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

        private void Close() { Task = null; Frame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo()); }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Modified)
            {
                await ShowMessageAsync("This task has been modified. Save or discard changes and try again.", "Couldn't delete");
                return;
            }

            await DeleteTask(TitleTextBox.Text, Task);
            Close();
        }

        public static async System.Threading.Tasks.Task DeleteTask(string taskName, TodoTask task)
        {
            const string title = "Delete";
            ContentDialog contentDialog = new()
            {
                Title = title,
                Content = $"Are you sure want to delete '{taskName}'?",
                PrimaryButtonText = "Yes",
                DefaultButton = ContentDialogButton.Primary,
                CloseButtonText = "No"
            };
            if (await contentDialog.ShowAsync() is not ContentDialogResult.Primary)
                return;

            MainPage.TaskList.Remove(task);
        }

        private bool Modified
        {
            get
            {
                if (Task is null)
                    return false;
                TodoTask task = new(DueDatePicker.Date.DateTime, SubjectPicker.SelectedItem as string, TitleTextBox.Text,
        string.IsNullOrWhiteSpace(BodyTextBox.Text) ? null : BodyTextBox.IsNullOrWhiteSpace() ? null : BodyTextBox.Text);
                return task != Task;
            }
        }

        private void RadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is RadioButtons rb)
            {
                if (rb.SelectedIndex is -1)
                    return;

                MainPage.Grade = rb.SelectedIndex switch
                {
                    0 => Grades.Grade1,
                    1 => Grades.Grade2,
                    2 => Grades.Grade3,
                    _ => throw new ArgumentException($"rb.SelectedIndex: Expected 0-2, but given {rb.SelectedIndex}."),
                };
                if (rb.SelectedItem is string str)
                {
                    var list = str switch
                    {
                        "Grade 1" => Grade1,
                        "Grade 2" => Grade2,
                        "Grade 3" => Grade3,
                        _ => throw new Exception($"rb.SelectedItem: expected \"Grade 1-3\", but given {str}."),
                    };
                    if (list.Contains("기타") is false)
                        list.Add("기타");
                    SubjectPicker.ItemsSource = list;
                }
            }
        }
    }
}
