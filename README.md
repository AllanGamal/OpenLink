**OpenLink**


# LLM-based Intelligent Assistant

## Overview

This project aims to develop an infrastructure for a Large Language Model (LLM) with different memory capacities and access to personal tools like reminders and calendar events.

## Structural Overview

1. **LLM Core:**
   - Centralized engine for natural language processing and user interaction.

2. **Database System:**
   - Combination of various databases to handle long-term memory, short-term memory, and general memory.

3. **Tool Integration:**
   - API integrations to manage reminders, calendar events, and other personal tools.

## Memory Structures

### 1. Long-Term Memory
- **Description:** Stores important information over long periods, based on the LLM's assessment of valuable data.
- **Functionality:**
  - [ ] **Summaries:** Generates and stores summaries of important conversations.
  - [ ] **Indexing:** Indexes and stores information in a semantically searchable database.
  - [ ] **Access:** Ability to recall previous conversations to provide context in future interactions.

### 2. General Memory
- **Description:** A dynamic memory that tracks user preferences, habits, and long-term goals.
- **Functionality:**
  - [ ] **User Profile:** Creation and continuous updating of a user profile with information like "user dislikes X and Y."
  - [ ] **Goal Tracking:** Tracking and reminding of specific goals set by the user.
  - [ ] **Continuous Adaptation:** Adapting suggestions and advice based on the user's changing preferences and feedback.

### 3. Short-Term Memory
- **Description:** A temporary memory that keeps track of information during an ongoing conversation.
- **Functionality:**
  - [x] **Contextual Understanding:** Keeps track of the conversation's topic and recent exchanges to provide relevant answers.
  - [] **Temporary Notes:** Ability to store and use temporary notes that may be useful during an ongoing session but don't need long-term storage.

## Tool Integrations

1. **Reminders:**
   - **API:** Integration with services like Google Keep, Microsoft To-Do, or other similar services.
   - **Functionality:** Create, manage, and remind the user of tasks and deadlines.
     - [ ] **Create Reminders**
     - [ ] **Manage Reminders**
     - [ ] **Remind User of Tasks** Ability for the system to remind the user of a relevant reminder, even withoyt the user prompting the system.

2. **Calendar:**
   - **API:** Integration with calendar platforms like Google Calendar and Microsoft Outlook.
   - **Functionality:** Manage and synchronize calendar events, schedule meetings, and remind the user of upcoming events.
     - [ ] **Manage Calendar Events**
     - [ ] **Synchronize Events**
     - [ ] **Remind User of Events** Ability for the system to remind the user of a relevant event, even without the user prompting the system.

## Technical Architecture

1. **Backend:**
   - **Database:** Combination of SQL (for structured data) and NoSQL (for unstructured data and fast access).
     - [ ] **SQL Database Integration**
     - [ ] **NoSQL Database Integration**

2. **Frontend:**
   - **User Interface:** Development of an intuitive interface where users can interact with the LLM and manage their settings.
     - [ ] **Dekstop application** Through an desktop app (Tauri)



## Checklist

### Memory Structures
- [ ] Long-Term Memory
  - [ ] Summaries
  - [ ] Indexing
  - [ ] Access
- [ ] General Memory
  - [ ] User Profile
  - [ ] Goal Tracking
  - [ ] Continuous Adaptation
- [ ] Short-Term Memory
  - [x] Contextual Understanding
  - [ ] Temporary Notes

### Tool Integrations
- [ ] Reminders
  - [ ] Create Reminders
  - [ ] Manage Reminders
  - [ ] Remind User of Tasks
- [ ] Calendar
  - [ ] Manage Calendar Events
  - [ ] Synchronize Events
  - [ ] Remind User of Events

### Technical Architecture
- [ ] Backend
  - [ ] SQL Database Integration
- [ ] Frontend
  - [ ] Dekstop application

