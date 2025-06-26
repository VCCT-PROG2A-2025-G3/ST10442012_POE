----------|| CYBERSECURITY AWARENESS ChatBot APPLICATION ||---------

-------|| Overview ||---------

This Cybersecurity Awareness Application is a Windows desktop program built with WPF, designed to educate users on cybersecurity topics through interactive features. It integrates a Chatbot, Cybersecurity Quiz, Task Manager, and an Activity Log to promote better online safety habits.

Parts 1 and 2 of this project were originally developed as console-based applications, focusing on chatbot interaction and quiz functionality. These console apps included NLP features such as keyword detection, synonym handling, typo tolerance, and sentiment detection.

This final version merges those components into a cohesive graphical user interface with improved usability and expanded capabilities.

The chatbot, in particular, leverages Natural Language Processing (NLP) techniques and sentiment analysis to provide personalized, context-aware cybersecurity advice.

Developed by Kaveer Lala as part of PROG6221 POE .

-------|| Features ||---------

Interactive Chatbot: Provides detailed cybersecurity information and guidance, using NLP to understand user input better.

Sentiment Detection: Recognizes emotional tones such as confusion or worry to respond supportively.

Clarification Handling: Detects when users ask for more information and offers follow-up explanations.

User Personalization: Remembers user details like name, skill level, and favorite topics for a tailored experience.

Cybersecurity Quiz: Randomized 15-question multiple-choice quiz with immediate feedback and scoring.

Task Manager: Allows adding, completing, and deleting tasks with optional reminders to encourage safe cybersecurity habits.

Activity Log: Chronological record of user actions within the app for review and audit purposes.

Speech Synthesis & Audio: The chatbot can speak responses aloud, enhancing accessibility.

Visual Enhancements: ASCII art in chatbot console parts, WPF UI with intuitive layout and navigation.

Navigation Bar: Consistent bottom bar linking Home, Chatbot, Tasks, Quiz, and Activity Log for seamless user movement.

-------|| Project Structure ||---------

Chatbot Module (Originally Console-Based)

Implements NLP features including keyword matching, typo tolerance, synonym detection, sentiment, and clarification detection.

Provides interactive Q&A with multimedia (audio greetings, ASCII art) in early console versions.

Migrated to WPF with speech synthesis and UI enhancements.

Quiz Module

Multiple-choice cybersecurity quiz with randomized questions from a larger pool.

Immediate answer validation, feedback, scoring, and retake option.

Task Manager

Create, view, update, and delete cybersecurity-related tasks.

Optional reminder dates and status tracking.

Activity Log

Tracks all user actions (chatbot queries, quiz attempts, task changes) with timestamps and details.

Supports incremental loading (“Show More”) for usability.

Navigation & Main UI

Consistent layout with a clean white background and fixed window size for all modules.

Bottom navigation bar allows quick switching between features.

-------|| How It Works ||---------

Startup
Loads the main window with navigation bar. The user selects Chatbot, Quiz, Tasks, or Activity Log.

Chatbot Interaction
User inputs cybersecurity-related questions or comments. NLP and sentiment analysis process input to generate personalized responses. Speech synthesis optionally reads answers aloud.

Quiz
Presents questions one-by-one with multiple choice answers. Provides immediate feedback and tallies score. Displays performance summary after completion.

Task Management
Users add tasks with optional reminder dates. Tasks can be marked complete or deleted. All changes are logged.

Activity Logging
All significant user actions are recorded with timestamps, action types, and details. Users can browse the log and load more entries as needed.

-------|| Installation and Setup ||---------

Requirements:

Windows OS

.NET Framework 4.7.2 or higher

Visual Studio 2022 (recommended)

Setup Steps:

Clone or download the project repository.

Open the solution file (.sln) in Visual Studio 2022.

Restore NuGet packages if necessary.

Build the solution to compile.

Run the application; the Home screen will launch first.

Navigate through features using the bottom navigation bar.

-------|| Key Functional Highlights ||---------

Console-based origins with expanded GUI integration.

Advanced NLP with typo tolerance, synonym recognition, clarification handling.

Sentiment detection for empathetic chatbot responses.

Multimedia enhancements: ASCII art, audio greetings, text-to-speech.

Dynamic data binding for tasks and activity logs.

Randomized quiz questions with feedback and retake option.

Activity logging for audit and review of user interactions.

-------|| Future Enhancements ||---------

Integrate machine learning-based NLP for more natural chatbot conversations.

Add user login and profile management for personalized experience and data persistence.

Implement persistent storage for tasks, logs, and quiz history using a database.

Develop multilingual support for wider accessibility.

Include multimedia tutorials and cybersecurity resources within the app.

-------|| Author ||---------

Kaveer Lala
Student Number: ST0442012
Email: ST10442012@vcconnect.edu.za
Institution: Varsity College, Cape Town

-------|| License ||---------

For educational purposes only. All rights reserved.
