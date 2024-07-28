import os
import json
from datetime import datetime
from typing import List, Dict

class ShortTermMemoryModel:
    MaxTokens = 1000  # Du kan anpassa detta värde baserat på dina behov

    def __init__(self, id: str, msg: str):
        self.id = id
        self.msg = msg

class ShortTermMemoryService:
    FilePath = "../Data/ShortTermMemory.json"
    #FilePath = "backend/Data/ShortTermMemory.json"

    def __init__(self):
        # Antagande om konstruktionslogik om sådan finns
        pass

    def create_json(self, msg: str, sender: str):
        memory_data_list = self.load_memory_data()

        now = datetime.now()
        base_id = now.strftime("%Y-%m-%dT%H:%M")

        count = sum(1 for md in memory_data_list if md['Id'].startswith(base_id))

        new_memory_data = {
            'Id': f"{base_id}:{count}",
            'Msg': f"{sender}: \n {msg} \n\n"
        }
        memory_data_list.append(new_memory_data)

        self.save_memory_data(memory_data_list)

    def get_max_tokens(self) -> str:
        memory_data_list = self.load_memory_data()

        characters_per_token = 4
        max_characters = ShortTermMemoryModel.MaxTokens * characters_per_token
        all_msgs = ""

        for i in range(len(memory_data_list) - 1, -1, -1):
            all_msgs = memory_data_list[i]['Msg'] + all_msgs
            if i % 10 == 0 and len(all_msgs) > max_characters:
                break

        return all_msgs

    def load_memory_data(self) -> List[Dict[str, str]]:
        if os.path.exists(self.FilePath):
            with open(self.FilePath, 'r') as file:
                existing_data = file.read()
                return [] if not existing_data else json.loads(existing_data)
        return []

    def save_memory_data(self, memory_data_list: List[Dict[str, str]]):
        options = {
            'indent': 4,
            'ensure_ascii': False
        }
        
        

        string_to_write_to_json = json.dumps(memory_data_list, **options)
        with open(self.FilePath, 'w') as file:
            file.write(string_to_write_to_json)