using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OpenLink.Models
{
    public class ShortTermMemoryModel
    {
        private const int maxTokens = 8000;
        public ShortTermMemoryModel()
        {
            // Assuming constructor logic if any
        }

        public static void CreateJson(string msg, string sender)
        {
            string filePath = "backend/Data/ShortTermMemory.json";
            List<MemoryData>? memoryDataList;

            
            if (File.Exists(filePath))
            {
                string existingData = File.ReadAllText(filePath);
                memoryDataList = string.IsNullOrEmpty(existingData) ? new List<MemoryData>() : JsonSerializer.Deserialize<List<MemoryData>>(existingData);
            }
            else
            {
                memoryDataList = new List<MemoryData>();
            }

            DateTime now = DateTime.Now;
            string baseId = now.ToString("yyyy-MM-ddTHH:mm");

            int count = memoryDataList.Count(md => md.Id.StartsWith(baseId));
            
            MemoryData newMemoryData = new MemoryData
            {
                Id = $"{baseId}:{count}",
                Msg = $"{sender}: \n {msg} \n\n"
            };
            memoryDataList.Add(newMemoryData);

            // Serialize and write back to file
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string stringToWriteToJson = JsonSerializer.Serialize(memoryDataList, options);
            File.WriteAllText(filePath, stringToWriteToJson);
        }

        public string GetMaxTokens() {

            string filePath = "backend/Data/ShortTermMemory.json";
            List<MemoryData>? memoryDataList;

            // Read existing data
            if (File.Exists(filePath))
            {
                string existingData = File.ReadAllText(filePath);
                memoryDataList = string.IsNullOrEmpty(existingData) ? new List<MemoryData>() : JsonSerializer.Deserialize<List<MemoryData>>(existingData);
            }
            else
            {
                memoryDataList = new List<MemoryData>();
            }

            
            const int charachtersPerToken = 4;
            const int maxCharachters = maxTokens * charachtersPerToken;
            string allMsgs = "";

            for (int i = memoryDataList.Count - 1; i >= 0; i--)
            {
                allMsgs = memoryDataList[i].Msg + allMsgs;
                if (i % 10 == 0) {
                    if (allMsgs.Length > maxCharachters) {
                        break;
                    }
                }
            }

            return allMsgs;
        }

       
    }

 

    public class MemoryData
    {
        public string Id { get; set; }
        public string Msg { get; set; }
    }
}