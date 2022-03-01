#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using RollingRess.UWP.FileIO;
using static RollingRess.Serializer;

namespace GGHS_Todo
{
    public static class SaveTask
    {
        private static ApplicationDataContainer localSettings => ApplicationData.Current.LocalSettings;

        public static void Save()
        {
            localSettings.Values["Tasks"] = Serialize(MainPage.TaskList);
        }

        public static void Load()
        {
            if (Deserialize<TaskList?>(localSettings.Values["Tasks"]) is TaskList taskList)
                MainPage.TaskList = taskList;
        }
    }
}