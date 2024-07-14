namespace OpenLink.Services
{
    public class LLMService
    {
        private readonly List<LLMModel> _llms = new List<LLMModel>();

        public IEnumerable<LLMModel> GetAllLLMs()
        {
            return _llms;
        }

        public LLMModel GetLLMById(int id)
        {
            return _llms.FirstOrDefault(l => l.Id == id);
        }

        public void CreateLLM(LLMModel llm)
        {
            _llms.Add(llm);
        }

        public void UpdateLLM(LLMModel llm)
        {
            var existingLLM = GetLLMById(llm.Id);
            if (existingLLM != null)
            {
                existingLLM.Data = llm.Data;
            }
        }

        public void DeleteLLM(int id)
        {
            var llm = GetLLMById(id);
            if (llm != null)
            {
                _llms.Remove(llm);
            }
        }
    }
}
