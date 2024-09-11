import os
current_dir = os.getcwd()
print("Current directory:", current_dir)


class Reminders:

    def __init__(self):
        self.reminders = []

    def add_reminder(self, reminder):
        self.reminders.append(reminder)

    def create_reminder_prompt_template(self, conversation):
        templats: str = '''
        Based on the end of the conversation given to you, create a reminder for the user.
        The reminder should be in the following json-format:
        ''' + "{" + '''
        "reminder": "*The description of the reminder, one sentence max, the date should not be included here*",
        "created": "*The date the reminder was created, YYYY-MM-DD, the same date as the conversation from which the reminder was created*",
        "date": "*The date of the reminder, YYYY-MM-DD*"
        ''' + "}" + '''
        You must only response with the json-format above.
        The end of the conversation that you should base the reminder on is the following:

        '''
        question = templats + conversation

        return question



    def get_reminders(self):
        return self.reminders

    def remove_reminder(self, reminder):
        self.reminders.remove(reminder)
'''
reminders = Reminders()
reminders.create_reminder("The user asked me to remind them to buy milk on the 20th of December, 2021.")
'''
# 