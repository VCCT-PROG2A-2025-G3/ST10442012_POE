||-------Cybersecurity Awareness  Chatbot  Application-------||

Project Overview
This project is a comprehensive Windows desktop application developed using WPF (Windows Presentation Foundation) aimed at improving users' cybersecurity awareness. The application offers interactive components such as a Cybersecurity Quiz, Task Manager, Chatbot, and Activity Log to educate users on important cybersecurity concepts while helping them stay organized and engaged.

Important:
Parts 1 and 2 of this project were originally implemented as console-based applications, focusing on foundational cybersecurity awareness tasks and chatbot logic. This final version integrates those functionalities into a rich graphical interface for improved usability and interaction.

The Chatbot incorporates basic Natural Language Processing (NLP) features, including keyword matching, synonym recognition, typo tolerance, clarification detection, and sentiment analysis. These features enable the chatbot to better understand user input nuances and respond more naturally and accurately to cybersecurity-related queries.

||-------Key Features-------||

1. Cybersecurity Quiz
A randomized quiz with 15 multiple-choice questions selected from a larger question pool.

Immediate feedback on answers, including explanations.

Score tracking with performance messages.

Option to retake the quiz with a fresh set of questions.

2. Task Manager
Add, view, mark as completed, and delete cybersecurity-related tasks.

Optional reminders for tasks using date pickers.

Dynamic task list updates using data binding.

Activity logging for all task actions.

3. Chatbot
Interactive chatbot focused on cybersecurity topics.

Keyword and synonym-based question detection.

Typo tolerance for common misspellings.

Clarification detection to refine user queries.

Sentiment detection to recognize user tone and adjust responses.

Speech synthesis features for accessibility and engagement.

Provides educational responses on phishing, malware, password security, social engineering, mobile security, data privacy, and more.

4. Activity Log
Chronological record of user activities across the app.

Entries include timestamps, action types, and detailed descriptions.

Supports incremental loading (“Show More”) for easier navigation.

Provides audit trail for tasks, quiz attempts, and chatbot interactions.

5. Intuitive Navigation
Consistent bottom navigation bar throughout the app for quick access to Home, Chatbot, Tasks, Quiz, and Activity Log.

Clean, fixed-size windows with a consistent white background and easy-to-read fonts.

||-------Technical Details-------||

Developed in C# using WPF for the graphical user interface.

Implements ObservableCollection for dynamic data binding in task and activity logs.

Uses a custom QuizQuestion class to manage quiz content.

Activity logging is centralized with a static class accessible from any part of the application.

Chatbot uses a combination of keyword lists, NLP techniques including sentiment detection, typo tolerance, and clarification logic to understand and respond to user inputs.

Asynchronous UI updates using timers to handle quiz feedback delays.

||-------Getting Started-------||

Prerequisites
Windows OS with .NET Framework (version 4.7.2 or later recommended)

Visual Studio (for building and running the project)

Running the Application
Clone or download the project repository.

Open the solution file (.sln) in Visual Studio.

Build the solution to restore dependencies and compile the code.

Run the project; the Home window will appear first.

Use the navigation bar to explore features: Quiz, Tasks, Chatbot, Activity Log.

||-------Usage Notes-------||

In the Quiz, answer by selecting the correct option and clicking “Submit Answer.” Feedback will show before advancing.

In Tasks, you can add new cybersecurity tasks, optionally set reminders, mark tasks completed, or delete them.

The Chatbot answers cybersecurity-related queries and provides explanations using advanced NLP techniques including sentiment detection.

The Activity Log tracks all significant user actions for review.

The “Show More” button in the Activity Log loads additional entries in increments of 10.

||-------Future Improvements-------||

Enhance NLP with machine learning for more natural conversations.

Add user authentication for personalized task and quiz tracking.

Implement data persistence for tasks and activity logs between sessions.

Integrate multimedia tutorials or videos within the app.

Add localization support for multiple languages.

||-------Author Information-------||

Kaveer Lala
Student Number: ST10442012
Email: st10442012@vcconnect.edu.za

Cybersecurity Awareness Chatbot  Application 

