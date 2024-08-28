from Services.LLMService import LLMService
from Services.ShortTermMemoryService import ShortTermMemoryService
from Services.LongTermMemoryService import LongTermMemoryService

llmservice = LLMService(ShortTermMemoryService(), LongTermMemoryService())

store_memory = llmservice.analyze_query_for_action("One fundemental thing about me that you should know about me is that i just love cats!")
print("store_memory: " + store_memory)

create_reminder = llmservice.analyze_query_for_action("Can you please remind me to buy milk on the 20th of December, 2021.")
print("create_reminder: " + create_reminder)

get_reminder = llmservice.analyze_query_for_action("Do I have any reminders for next week?")
print("get_reminder: " + get_reminder)  

