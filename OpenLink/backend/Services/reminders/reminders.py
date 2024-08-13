
from LLMService import askLLMAndGetResponse

class Reminders:

    def __init__(self):
        self.reminders = []

    def add_reminder(self, reminder):
        self.reminders.append(reminder)

    def create_reminder(self, conversation):
        templats: str = '''
        Based on the conversation given to you, create a reminder for the user.
        The reminder should be in the following json-format:
        ''' + "{" + '''
        "reminder": "*The description of the reminder, one sentence max*",
        "created": "*The date the reminder was created, YYYY-MM-DD, the same date as the conversation from which the reminder was created*",
        "date": "*The date of the reminder, YYYY-MM-DD*"
        ''' + "}" + '''
        You must only response with the json-format above.
        The conversation that you should base the reminder on is the following:

        '''
        question = templats + conversation

        askLLMAndGetResponse(question)


    def get_reminders(self):
        return self.reminders

    def remove_reminder(self, reminder):
        self.reminders.remove(reminder)