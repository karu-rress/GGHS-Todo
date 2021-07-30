#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.ApplicationModel;
using Windows.UI.Popups;

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
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static List<Task> TaskList { get; set; } = new();

        private static readonly PackageVersion version = Package.Current.Id.Version;
        public static string Version => $"{version.Major}.{version.Minor}.{version.Build}";

        public Grades Grade;

        public MainPage()
        {
            InitializeComponent();
            LoadTasks();
        }

        /*
<Button Click="TaskButton_Click">
    <Grid Height="80" Width="2560" Margin="-12,0,0,0" >
        <Grid>
            <TextBlock>
            <TextBlock>
        </Grid>
        <TextBlock FontSize="17" Text="독서" Margin="80,12,0,44" Width="2560"/>
        <TextBlock FontSize="15" Text="매 시간이 수행평가" Margin="80,43,0,13" HorizontalAlignment="Left" Width="2560" Foreground="#ffa4a4a4"/>
    </Grid>
</Button>   
         */

        private void LoadTasks()
        {
            if (TaskList?.Any() is false)
            {
                return;
            }

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

        private async System.Threading.Tasks.Task DeleteTasksByExpression(Predicate<Task>? match)
        {
            if (TaskList is null || TaskList.Count is 0)
            {
                await NothingToDelete();
                return;
            }

            int cnt =
                (match is null) ? TaskList.Count
                : TaskList.FindAll(match).Count;

            if (cnt is 0)
            {
                await NothingToDelete();
                return;
            }

            const string title = "Delete Past";
            ContentDialog contentDialog = new()
            {
                Content = $"Are you sure want to delete {cnt} {((cnt == 1) ? "item" : "items")}?",
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
            contentDialog = new()
            {
                Content = $"Successfully deleted {cnt} {(cnt == 1 ? "task" : "task")}.",
                Title = title,
                CloseButtonText = "Close",
            };
            await contentDialog.ShowAsync();

            static async System.Threading.Tasks.Task NothingToDelete()
            {
                ContentDialog content = new()
                {
                    Content = "Nothing to delete.",
                    Title = "Delete",
                    CloseButtonText = "OK",
                };
                await content.ShowAsync();
            }
        }


        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            await DeleteTasksByExpression(x => x.DueDate < DateTime.Now);
        }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TaskButton tb)
            {
                AddPage.Task = tb.Task;
                Frame.Navigate(typeof(AddPage));
            }
        }

        private async void RemoveAllButton_Click(object sender, RoutedEventArgs e)
        {
            await DeleteTasksByExpression(null);
        }

        private async void SelectDate_Click(object sender, RoutedEventArgs e)
        {
            DateSelectDialog dialog = new();
            await dialog.ShowAsync();

            var date = dialog.SelectedDate;
            await DeleteTasksByExpression(x => x.DueDate.Value == date);
        }

        private async void SelectSubject_Click(object sender, RoutedEventArgs e)
        {
            SubjectSelectDialog dialog = new();
            await dialog.ShowAsync();
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