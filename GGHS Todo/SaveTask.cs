using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace GGHS_Todo
{
    public static class SaveTask
    {
        private const string fileName = "gtdtasks.tks";
        private static StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        private const string token = "!*^#$^*#%";
        private const string endLineToken = "@#&^*$@*#";
        public static async void Save()
        {


            var saveFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using var stream = await saveFile.OpenAsync(FileAccessMode.ReadWrite);
            using var outputStream = stream.GetOutputStreamAt(0);
            using var dataWriter = new DataWriter(outputStream);

            StringBuilder sb = new();
            foreach (var task in MainPage.TaskList)
            {
                sb.AppendLine($"{task.DueDate ?? null:MM/dd}{token}{task.Subject}{token}{task.Title}{token}{task.Body}{endLineToken}");
            }
            dataWriter.WriteString(sb.ToString());
            await dataWriter.StoreAsync();
            await outputStream.FlushAsync();
        }

        public static async System.Threading.Tasks.Task Load()
        {
            if (await storageFolder.TryGetItemAsync(fileName) is not StorageFile saveFile)
                return;
            using var stream = await saveFile.OpenAsync(FileAccessMode.Read);
            var size = stream.Size;
            using var inputStream = stream.GetInputStreamAt(0);
            using var dataReader = new DataReader(inputStream);
            uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
            string text = dataReader.ReadString(numBytesLoaded);

            if (string.IsNullOrWhiteSpace(text))
                return;

            string[] lines = text.Split(endLineToken);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    break;
                DateTime? due = null;
                string subject;
                string title;
                string body = null;
                string[] date;
                string[] data = line.Split(token);
                // 0: due, 1: subject, 2: title, 3: body
                if (data.Length is not 4 || string.IsNullOrEmpty(data[3]) is false)
                    body = data[3];

                if (string.IsNullOrEmpty(data[0]) is false)
                {
                    date = data[0].Split("/");
                    due = new(DateTime.Now.Year, int.Parse(date[0]), int.Parse(date[1]));
                }
                (subject, title) = (data[1], data[2]);
                MainPage.TaskList.Add(new(due, subject, title, body));
            }
        }
    }
}
