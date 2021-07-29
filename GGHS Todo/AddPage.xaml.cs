#nullable enable

using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GGHS_Todo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPage : Page
    {
        public static Task? Task { get; set; } = null;
        public List<string> Grade1 { get; } = new()
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

        public List<string> Grade2 { get; } = new()
        {
            "독서",
            "수학Ⅱ",
            "수학과제탐구",
            "과학사",
            "생활과 과학",
            "운동과 건강",
            "창의적문제해결기법",
            "스페인어Ⅰ",
            "중국어Ⅰ",
            "일본어Ⅰ",
            "심화영어Ⅰ",
            "국제경제",
            "국제정치",
            "비교문화",
            "동양근대사",
            "세계 역사와 문화",
            "현대정치철학의 이해",
            "세계 지역 연구",
            "공간 정보와 공간 분석",
        };

        public List<string> Grade3 { get; } = new()
        {
            "체육",
            "논리적 글쓰기",
            "스페인어권 문화",
            "일본문화",
            "중국문화",
            "심화영어독해Ⅱ",
            "독서와 의사소통",
            "국제화시대의 한국어",
            "사회탐구방법",
            "한국사회의 이해",
            "통계로 바라보는 국제문제",
            "국제법",
            "세계 문제와 미래사회",
            "윤리학 연습",
        };

        public List<string> Subjects { get; } = new();

        public AddPage()
        {
            this.InitializeComponent();

            Subjects = Grade1.Concat(Grade2).Concat(Grade3).ToList();
            Subjects.Add("기타");
            SubjectPicker.ItemsSource = Subjects;

            if (Task is not null) // Clicked a button
            {
                InitializeComponent();
                DeleteButton.Visibility = Visibility.Visible;
                mainText.Text = "Modify Task";
                DueDatePicker.Date = Task.DueDate.Value;
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
                ContentDialog content = new()
                {
                    Title = "Error",
                    Content = "Date, subject and title are required.",
                    CloseButtonText = "OK",
                };
                await content.ShowAsync();
                return;
            }

            Task task = new(DueDatePicker.Date.DateTime, SubjectPicker.SelectedItem as string, TitleTextBox.Text, 
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
            MainPage.TaskList.Sort((x, y) => x.DueDate.Value.CompareTo(y.DueDate.Value));
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();


        private void Close() { Task = null; Frame.Navigate(typeof(MainPage)); }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Modified())
            {
                ContentDialog content = new()
                {
                    Title = "Couldn't delete",
                    Content = "This task has been modified. Save or discard changes and try again.",
                    CloseButtonText = "OK"
                };
                await content.ShowAsync();
                return;
            }

            ContentDialog contentDialog = new()
            {
                Title = "Delete",
                Content = $"Are you sure want to delete '{TitleTextBox.Text}'?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            if (await contentDialog.ShowAsync() is not ContentDialogResult.Primary)
                return;

            MainPage.TaskList.Remove(Task);
            contentDialog = new()
            {
                Title = "Removal Successful",
                Content = "Successfully deleted.",
                CloseButtonText = "OK",
            };
            await contentDialog.ShowAsync();
            Close();
        }

        private bool Modified()
        {
            if (Task is null)
                return false;
            Task task = new(DueDatePicker.Date.DateTime, SubjectPicker.SelectedItem as string, TitleTextBox.Text,
    string.IsNullOrWhiteSpace(BodyTextBox.Text) ? null : BodyTextBox.IsNullOrWhiteSpace() ? null : BodyTextBox.Text);
            return task != Task;
        }

        /*
         
         private void BackgroundColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (ExampleBorder != null && sender is muxc.RadioButtons rb)
    {
        string colorName = rb.SelectedItem as string;
        switch (colorName)
        {
            case "Red":
                ExampleBorder.Background = new SolidColorBrush(Colors.Red);
                break;
            case "Green":
                ExampleBorder.Background = new SolidColorBrush(Colors.Green);
                break;
            case "Blue":
                ExampleBorder.Background = new SolidColorBrush(Colors.Blue);
                break;
        }
    }
}
         
         */

        private void RadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is RadioButtons rb)
            {
                if (rb.SelectedItem is string str)
                {
                    var list = str switch
                    {
                        "Grade 1" => Grade1,
                        "Grade 2" => Grade2,
                        "Grade 3" => Grade3,
                    };
                    if (list.Contains("기타") is false)
                        list.Add("기타");
                    SubjectPicker.ItemsSource = list;
                }
            }
        }
    }
}
