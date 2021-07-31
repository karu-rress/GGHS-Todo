#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
// Enables using record types as tuple-like types.
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IsExternalInit { }
}

namespace GGHS_Todo
{
    public record Task(DateTime? DueDate, string Subject, string Title, string? Body);

    public enum Grades
    {
        Grade1,
        Grade2,
        Grade3,
        None,
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static List<Task> TaskList { get; set; } = new();

        private static readonly PackageVersion version = Package.Current.Id.Version;
        public static string Version => $"{version.Major}.{version.Minor}.{version.Build}";

        public static Grades Grade { get; set; } = Grades.None;

        public MainPage()
        {
            InitializeComponent();
            LoadTasks();
        }

        /// <summary>
        /// Adds task buttons to the grid.
        /// </summary>
        private void LoadTasks()
        {
            if (TaskList.IsNullOrEmpty())
                return;

            int buttons = 0;
            foreach (var task in TaskList)
            {
                TaskGrid.Children.Add(new TaskButton(task, TaskButton_Click, buttons++));
            }
        }

        private void ReloadTasks()
        {
            TaskGrid.Children.Clear();
            LoadTasks();
        }

        private void AddButton_Click(object _, RoutedEventArgs e) => Frame.Navigate(typeof(AddPage));

        private async System.Threading.Tasks.Task DeleteTasks(Predicate<Task>? match)
        {
            if (TaskList.IsNullOrEmpty())
            {
                await NothingToDelete();
                return;
            }

            int cnt = (match is null) ? TaskList.Count : TaskList.FindAll(match).Count;
            if (cnt is 0)
            {
                await NothingToDelete();
                return;
            }

            const string title = "Delete";
            ContentDialog contentDialog = new()
            {
                Content = $"Are you sure want to delete {cnt} {"task".putS(cnt)}?",
                Title = title,
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Yes, delete",
                DefaultButton = ContentDialogButton.Primary
            };
            if (await contentDialog.ShowAsync() is ContentDialogResult.None)
                return;

            if (match is null)
                TaskList.Clear();
            else
                TaskList.RemoveAll(match);

            ReloadTasks();
            contentDialog = new ContentMessageDialog($"Successfully deleted {cnt} {"task".putS(cnt)}.", title, "Close");
            await contentDialog.ShowAsync();

            static async System.Threading.Tasks.Task NothingToDelete()
            {
                ContentMessageDialog message = new("Nothing to delete.", "Delete");
                await message.ShowAsync();
            }
        }

        private async void DeletePastButton_Click(object _, RoutedEventArgs e)
            => await DeleteTasks(x => x.DueDate < DateTime.Now);

        private async void DeleteAllButton_Click(object _, RoutedEventArgs e) => await DeleteTasks(null);

        private async void SelectDate_Click(object _, RoutedEventArgs e)
        {
            DateSelectDialog dialog = new();
            if (await dialog.ShowAsync() is ContentDialogResult.None)
                return;

            var date = dialog.SelectedDate;
            await DeleteTasks(x => x.DueDate is not null && x.DueDate.Value == date);
        }

        private async void SelectSubject_Click(object _, RoutedEventArgs e)
        {
            SubjectSelectDialog dialog = new();
            if (await dialog.ShowAsync() is ContentDialogResult.None)
                return;

            await DeleteTasks(x => x.Subject == dialog.SelectedSubject);
        }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TaskButton tb)
            {
                AddPage.Task = tb.Task;
                Frame.Navigate(typeof(AddPage));
            }
        }

    }
}

//  TODO 함수 이름 재정리. 최적화

/*
Button b = new() { Height = ButtonHeight, Margin = new(0, 98 * buttons, 0, 0) };
b.Click += TaskButton_Click;

Grid inner = new()
{
Height = 80,
Width = 2560,
Margin = new(-12, 0, 0, 0)
};
Grid dday = new()
{ 
Width = 65,
Margin = new(10, 0, 0, 0),
};
TextBlock tb1 = new()
{
FontSize = 19,
Text = task.DueDate?.ToString("MM/dd"),
Margin = new(0, 10, 0, 46),
HorizontalAlignment = HorizontalAlignment.Center,
FontFamily = new("Segoe"),
FontWeight = FontWeights.Bold
};
TextBlock tb2 = new()
{ 
FontSize = 15,
Text = "D" + (DateTime.Now < task.DueDate.Value ? "" : "+") + (DateTime.Now - task.DueDate.Value).Days,
Margin = new(0, 44, 0, 12),
HorizontalAlignment = HorizontalAlignment.Center,
FontFamily = new("Consolas"),
FontWeight = FontWeights.Bold
};
dday.Children.Add(tb1);
dday.Children.Add(tb2);

TextBlock tb3 = new()
{ 
FontSize = 17,
Text = task.Subject,
Margin = new(80, 12, 0, 44),
Width = ButtonWidth
};
TextBlock tb4 = new()
{
FontSize = 15,
Text = task.Title,
Margin = new(80, 43, 0, 13),
HorizontalAlignment = HorizontalAlignment.Left,
Width = ButtonWidth,
Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xA4, 0xA4, 0xA4))
};
inner.Children.Add(dday);
inner.Children.Add(tb3);
inner.Children.Add(tb4);
b.Content = inner;
TaskGrid.Children.Add(b);

buttons++;
*/