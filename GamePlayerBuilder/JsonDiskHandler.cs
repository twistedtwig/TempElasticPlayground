using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GamePlayerBuilder.Helpers;
using Newtonsoft.Json;

namespace GamePlayerBuilder
{
    public class JsonDiskHandler
    {
        public string FilePath { get; set; }

        public JsonDiskHandler(string filePath)
        {
            FilePath = filePath;

            EnsureFolderExistsAndIsEmpty();
        }

        public void WriteToDisk<T>(List<T> data) where T : class
        {
            var ticks = DateTime.Now.Ticks;
            var splitList = data.SplitList(1000).ToList();

            var fileNameType = typeof(T).Name;
            for (int i = 0; i < splitList.Count; i++)
            {
                var json = JsonConvert.SerializeObject(splitList[i], Formatting.Indented);
                var fileName = $"{fileNameType}-{ticks}-{i:D9}.json";
                File.WriteAllText(Path.Combine(FilePath, fileName), json);
            }
        }

        private void EnsureFolderExistsAndIsEmpty()
        {
            if (Directory.Exists(FilePath))
            {
                Directory.Delete(FilePath, true);
            }

            Directory.CreateDirectory(FilePath);
        }
    }
}
