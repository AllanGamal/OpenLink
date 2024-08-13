

class Reminders:

    def __init__(self):
        self.reminders = []

    def add_reminder(self, reminder):
        self.reminders.append(reminder)

    def get_reminders(self):
        return self.reminders

    def remove_reminder(self, reminder):
        self.reminders.remove(reminder)