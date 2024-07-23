using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OpenLink.Models
{
    
    public class ShortTermMemoryModel
    {
        public const int MaxTokens = 8000;

        public string Id { get; set; }
        public string Msg { get; set; }
    }

   public class MemoryData
    {
        public string Id { get; set; }
        public string Msg { get; set; }
    }
}