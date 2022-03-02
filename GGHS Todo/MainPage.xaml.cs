#nullable enable
global using static RollingRess.StaticClass;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;
using RollingRess;
using Windows.UI.Xaml.Media.Animation;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
// Enables using record types as tuple-like types.
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IsExternalInit { }
}

namespace GGHS_Todo
{
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

        /// <summary>
        /// Shows the version of GGHS Todo, format in "X.X.X"
        /// </summary>
        public static string Version => $"{version.Major}.{version.Minor}.{version.Build}";

        public static Grades Grade { get; set; } = Grades.None;

        public MainPage()
        {
            InitializeComponent(); 
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(AppTitleBar);
            
            LoadTasks();
        }

        /// <summary>
        /// Adds task buttons to the grid.
        /// </summary>
        private void LoadTasks()
        {
            if (TaskList.IsNullOrEmpty)
                return;

            foreach (var task in TaskList)
                TaskGrid.Children.Add(new TaskButton(task, TaskButton_Click));
        }

        private void ReloadTasks()
        {
            TaskGrid.Children.Clear();
            LoadTasks();
        }

        private void AddButton_Click(object _, RoutedEventArgs e) => Frame.Navigate(typeof(AddPage), null, new DrillInNavigationTransitionInfo());

        private async System.Threading.Tasks.Task DeleteTasks(Predicate<TodoTask>? match)
        {
            if (TaskList.IsNullOrEmpty)
            {
                await NothingToDelete();
                return;
            }
            int cnt = TaskList.CountAll(match);
            if (cnt is 0)
            {
                await NothingToDelete();
                return;
            }

            const string title = "Delete";
            ContentDialog contentDialog = new()
            {
                Content = $"Are you sure want to delete {cnt} {"task".PutS(cnt)}?",
                Title = title,
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Yes, delete",
                DefaultButton = ContentDialogButton.Primary
            };
            if (await contentDialog.ShowAsync() is ContentDialogResult.None)
                return;

            TaskList.RemoveAll(match);

            ReloadTasks();
            await ShowMessageAsync($"Successfully deleted {cnt} {"task".PutS(cnt)}.", title);

            static async System.Threading.Tasks.Task NothingToDelete()
            {
                await ShowMessageAsync("Nothing to delete.", "Delete");
            }
        }

        private async void DeletePastButton_Click(object _, RoutedEventArgs e)
            => await DeleteTasks(x => x.DueDate.Date < DateTime.Now.Date);

        private async void DeleteAllButton_Click(object _, RoutedEventArgs e) => await DeleteTasks(null);

        private async void SelectDate_Click(object _, RoutedEventArgs e)
        {
            DateSelectDialog dialog = new();
            if (await dialog.ShowAsync() is ContentDialogResult.None)
                return;

            var date = dialog.SelectedDate;
            await DeleteTasks(x => x.DueDate == date);
        }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TaskButton tb)
            {
                AddPage.Task = tb.Task;
                Frame.Navigate(typeof(AddPage));
            }
        }

        private async void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            int result = TaskList.Undo();
            if (result is 0)
            {
                await ShowMessageAsync("Nothing to restore.", "Undo Delete");
                return;
            }
            ReloadTasks();
            await ShowMessageAsync($"Successfully restored {result} {"item".PutS(result)}.", "Undo Delete");
        }
    }
}