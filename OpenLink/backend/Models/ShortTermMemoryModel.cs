using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OpenLink.Models
{
    public class ShortTermMemoryModel
    {
        public ShortTermMemoryModel()
        {
            
        }

        public string ToJson(MemoryData memoryData)
        {
            return JsonSerializer.Serialize(memoryData, new JsonSerializerOptions { WriteIndented = true });
        }

        public void CreateJson(String msg)
        {
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

            DateTime now = DateTime.Now;
            string baseId = now.ToString("yyyy-MM-ddTHH:mm");

            int count = memoryDataList.Count(md => md.Id.StartsWith(baseId));
            
            MemoryData newMemoryData = new MemoryData
            {
                Id = $"{baseId}:{count}",
                Msg = msg
            };
            memoryDataList.Add(newMemoryData);

            // Serialize and write back to file
            string stringToWriteToJson = JsonSerializer.Serialize(memoryDataList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, stringToWriteToJson);
        }
    }

    public class MemoryData
    {
        public string Id { get; set; }
        public string Msg { get; set; }
    }
}