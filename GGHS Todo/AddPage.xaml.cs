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


        public AddPage()
        {
            InitializeComponent();


            if (Task is not null) // Clicked a button
            {
                InitializeComponent();
                DeleteButton.Visibility = Visibility.Visible;
                mainText.Text = "Modify Task";
                DueDatePicker.Date = Task.DueDate;
                TitleTextBox.Text = Task.Title;
                if (Task.Body is not null)
                    BodyTextBox.Text = Task.Body;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DueDatePicker.SelectedDate is null || TitleTextBox.IsNullOrWhiteSpace())
            {
                await ShowMessageAsync("Date and title are required.", "Error");
                return;
            }

            var date = DueDatePicker.Date.DateTime;
            TodoTask task = new(new(date.Year, date.Month, date.Day), TitleTextBox.Text, 
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
                TodoTask task = new(DueDatePicker.Date.DateTime, TitleTextBox.Text,
        string.IsNullOrWhiteSpace(BodyTextBox.Text) ? null : BodyTextBox.IsNullOrWhiteSpace() ? null : BodyTextBox.Text);
                return task != Task;
            }
        }
    }
}
