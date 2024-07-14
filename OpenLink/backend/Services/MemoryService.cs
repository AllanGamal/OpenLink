namespace OpenLink.Services
{
    public class MemoryService
    {
        private readonly List<MemoryModel> _memories = new List<MemoryModel>();

        public IEnumerable<MemoryModel> GetAllMemories()
        {
            return _memories;
        }

        public MemoryModel GetMemoryById(int id)
        {
            return _memories.FirstOrDefault(m => m.Id == id);
        }

        public void CreateMemory(MemoryModel memory)
        {
            _memories.Add(memory);
        }

        public void UpdateMemory(MemoryModel memory)
        {
            var existingMemory = GetMemoryById(memory.Id);
            if (existingMemory != null)
            {
                existingMemory.Data = memory.Data;
            }
        }

        public void DeleteMemory(int id)
        {
            var memory = GetMemoryById(id);
            if (memory != null)
            {
                _memories.Remove(memory);
            }
        }
    }
}
