using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using RollingRess.UWP.FileIO;

namespace GGHS_Todo
{
    public static class SaveTask
    {
        private static string FileName => "TodoXML.tks";
        private static StorageFolder storageFolder => ApplicationData.Current.LocalFolder;

        public static async System.Threading.Tasks.Task Save()
        {
            DataWriter<List<Task>> writer = new(FileName, MainPage.TaskList.List);
            await writer.WriteAsync();

            // 디버그 해볼것... 지금 파일 생성이 안 됨...
        }

        public static async System.Threading.Tasks.Task Load()
        {
            if (await storageFolder.TryGetItemAsync(FileName) is not StorageFile)
                return;

            DataReader<List<Task>> reader = new(FileName);
            MainPage.TaskList.List = await reader.ReadAsync();
        }
    }
}