#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;
using RollingRess;
using Windows.UI.Xaml.Media.Animation;
using System.Linq;

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
        public static TaskList TaskList { get; set; } = new();

        private static readonly PackageVersion version = Package.Current.Id.Version;
        public static string Version => $"{version.Major}.{version.Minor}.{version.Build}";

        public static Grades Grade { get; set; } = Grades.None;

        public MainPage()
        {
            InitializeComponent();
            LoadTasks();
        }

        private enum TaskLoadType
        {
            Default,
            OnlyToday,
        }


        /// <summary>
        /// Adds task buttons to the grid.
        /// </summary>
        private void LoadTasks(TaskLoadType loadType = TaskLoadType.Default)
        {
            if (TaskList.IsNullOrEmpty)
                return;

            int buttons = 0;
            List<Task> taskButtons = loadType switch
            {
                TaskLoadType.Default => TaskList.List,
                TaskLoadType.OnlyToday => (from task in TaskList.List 
                                           where task.DueDate?.Date == DateTime.Now.Date 
                                           select task).ToList(),
            };
            foreach (var task in taskButtons)
            {
                TaskGrid.Children.Add(new TaskButton(task, TaskButton_Click, buttons++));
            }
        }

        private void ReloadTasks(TaskLoadType loadType = TaskLoadType.Default)
        {
            TaskGrid.Children.Clear();
            LoadTasks(loadType);
        }

        private void AddButton_Click(object _, RoutedEventArgs e) => Frame.Navigate(typeof(AddPage), null, new DrillInNavigationTransitionInfo());

        private async void DeletePastButton_Click(object _, RoutedEventArgs e)
            => await DeleteTasks(x => x.DueDate.Value.Date < DateTime.Now.Date);

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
                Frame.Navigate(typeof(AddPage), null, new DrillInNavigationTransitionInfo());
            }
        }

        private async void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            int result = TaskList.Undo();
            if (result is 0)
            {
                var message = new ContentMessageDialog("Nothing to restore.", "Undo Delete");
                await message.ShowAsync();
                return;
            }
            ReloadTasks();
            ContentMessageDialog msg = new($"Successfully restored {result} {"item".putS(result)}.", "Undo Delete");
            await msg.ShowAsync();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (TodayToggle.IsOn) // Today only
            {
                ReloadTasks(TaskLoadType.OnlyToday);
            }
            else
            {
                ReloadTasks(TaskLoadType.Default);
            }
        }
    }
}