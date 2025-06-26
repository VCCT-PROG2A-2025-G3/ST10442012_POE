using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ST10442012_POE
{
    public partial class ActivityLog : Window
    {
        // --------------------|| Activity Entry Class ||--------------------
       
        // This class manages the Activity Log window, which displays a list of recent
        // user actions across the application such as task updates, quiz attempts,
        // and chatbot interactions. It supports paging with a "Show More" feature,
        // keeps a maximum number of log entries for performance, and provides
        // navigation to other parts of the app.
        //
        // Logs are stored in a shared collection accessible throughout the app,
        // enabling consistent tracking and review of user activities.
        // ---------------------------------------------------------------

        public class ActivityEntry
        {
            public string Time { get; set; }     // Timestamp of the log entry
            public string Action { get; set; }   // Type of action performed
            public string Details { get; set; }  // Additional description of the action
        }

        // --------------------|| Log Collections ||--------------------
        // Stores all log entries globally (shared across the app)
        public static ObservableCollection<ActivityEntry> AllLogEntries { get; } = new ObservableCollection<ActivityEntry>();

        // Stores only currently visible/paged entries in the UI
        private ObservableCollection<ActivityEntry> visibleEntries = new ObservableCollection<ActivityEntry>();

        // Number of log entries to show initially (and when "Show More" is clicked)
        private int displayCount = 10;

        // Limit total logs to avoid high memory usage
        private const int MaxLogEntries = 1000;

        // --------------------|| Constructor ||--------------------
        // Initializes UI and loads the first set of log entries
        public ActivityLog()
        {
            InitializeComponent();

            // Bind the ListView to the visible entries
            ActivityListView.ItemsSource = visibleEntries;

            // Load the initial set of logs into the visible list
            LoadInitialEntries();
        }

        // --------------------|| Load Initial Log Entries ||--------------------
        // Loads the top 'displayCount' logs into the UI from the full collection
        private void LoadInitialEntries()
        {
            visibleEntries.Clear();

            foreach (var entry in AllLogEntries.Take(displayCount))
            {
                visibleEntries.Add(entry);
            }
        }

        // --------------------|| Show More Button Logic ||--------------------
        // Adds 10 more logs to the visible list when "Show More" is clicked
        private void ShowMore_Click(object sender, RoutedEventArgs e)
        {
            displayCount += 10;
            LoadInitialEntries();
        }

        // --------------------|| Add New Log Entry (Static Method) ||--------------------
        // Allows other classes to log actions into the activity log
        public static void AddLog(string action, string details)
        {
            // Remove the oldest log if we've hit the max limit
            if (AllLogEntries.Count >= MaxLogEntries)
            {
                AllLogEntries.RemoveAt(AllLogEntries.Count - 1);
            }

            // Insert the new log at the top
            AllLogEntries.Insert(0, new ActivityEntry
            {
                Time = DateTime.Now.ToString("g"),
                Action = action,
                Details = details
            });
        }

        // --------------------|| Navigation Buttons ||--------------------
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            new Home().Show();
            Close();
        }

        private void ChatBotButton_Click(object sender, RoutedEventArgs e)
        {
            new ChatBot().Show();
            Close();
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            new Tasks().Show();
            Close();
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            new Quiz().Show();
            Close();
        }

        private void ActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            // Already on ActivityLog, no action needed
        }
    }
}
