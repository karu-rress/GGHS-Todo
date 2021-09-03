#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;
using RollingRess;

namespace GGHS_Todo
{
    public sealed partial class AddPage : Page
    {
        public static async System.Threading.Tasks.Task DeleteTask(string taskName, Task task)
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
    }

    public sealed partial class MainPage : Page
    {
        private async System.Threading.Tasks.Task DeleteTasks(Predicate<Task>? match)
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
                Content = $"Are you sure want to delete {cnt} {"task".putS(cnt)}?",
                Title = title,
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Yes, delete",
                DefaultButton = ContentDialogButton.Primary
            };
            if (await contentDialog.ShowAsync() is ContentDialogResult.None)
                return;

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
    }
}