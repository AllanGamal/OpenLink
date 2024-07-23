using OpenLink.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OpenLink.Services
{
    public class ShortTermMemoryService
    {
        private const string FilePath = "backend/Data/ShortTermMemory.json";

        public ShortTermMemoryService()
        {
            // Assuming constructor logic if any
        }

        public void CreateJson(string msg, string sender)
        {
            List<ShortTermMemoryModel> memoryDataList = LoadMemoryData();

            DateTime now = DateTime.Now;
            string baseId = now.ToString("yyyy-MM-ddTHH:mm");

            int count = memoryDataList.Count(md => md.Id.StartsWith(baseId));

            ShortTermMemoryModel newMemoryData = new ShortTermMemoryModel
            {
                Id = $"{baseId}:{count}",
                Msg = $"{sender}: \n {msg} \n\n"
            };
            memoryDataList.Add(newMemoryData);

            SaveMemoryData(memoryDataList);
        }

        public string GetMaxTokens()
        {
            List<ShortTermMemoryModel> memoryDataList = LoadMemoryData();

            const int charactersPerToken = 4;
            const int maxCharacters = ShortTermMemoryModel.MaxTokens * charactersPerToken;
            string allMsgs = "";

            for (int i = memoryDataList.Count - 1; i >= 0; i--)
            {
                allMsgs = memoryDataList[i].Msg + allMsgs;
                if (i % 10 == 0 && allMsgs.Length > maxCharacters)
                {
                    break;
                }
            }

            return allMsgs;
        }

        private List<ShortTermMemoryModel> LoadMemoryData()
        {
            if (File.Exists(FilePath))
            {
                string existingData = File.ReadAllText(FilePath);
                return string.IsNullOrEmpty(existingData) ? new List<ShortTermMemoryModel>() : JsonSerializer.Deserialize<List<ShortTermMemoryModel>>(existingData);
            }

            return new List<ShortTermMemoryModel>();
        }

        private void SaveMemoryData(List<ShortTermMemoryModel> memoryDataList)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string stringToWriteToJson = JsonSerializer.Serialize(memoryDataList, options);
            File.WriteAllText(FilePath, stringToWriteToJson);
        }
    }
}