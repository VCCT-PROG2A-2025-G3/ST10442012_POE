using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ST10442012_POE
{
    public partial class ActivityLog : Window
    {

        // Represents a single activity log entry
        public class ActivityEntry
        {
            public string Time { get; set; } // Timestamp of the log entry
            public string Action { get; set; } // Type of action performed (e.g. Task Created)
            public string Details { get; set; } // Additional details about the action
        }

        // Static collection that holds all log entries application-wide
       
        public static ObservableCollection<ActivityEntry> AllLogEntries { get; } = new ObservableCollection<ActivityEntry>();
        // Collection of entries currently visible in the UI (paged)
        private ObservableCollection<ActivityEntry> visibleEntries = new ObservableCollection<ActivityEntry>();

        // Number of entries to display initially and incrementally when "Show More" is clicked
        private int displayCount = 10;

        // Optional max cap to prevent excessive memory use in case of lots of logs
        private const int MaxLogEntries = 1000; // Optional safety cap


        // Constructor - sets up data binding and loads the first page of logs
        public ActivityLog()
        {
            InitializeComponent();

            // Bind the ListView's items source to the visible entries collection
            ActivityListView.ItemsSource = visibleEntries;
            // Load initial batch of log entries to display
            LoadInitialEntries();
        }


        // Loads the first 'displayCount' entries from the full log into visibleEntries
        private void LoadInitialEntries()
        {
            visibleEntries.Clear();
            // Take the first 'displayCount' entries from AllLogEntries and add to visibleEntries
            foreach (var entry in AllLogEntries.Take(displayCount))
            {
                visibleEntries.Add(entry);
            }
        }

        // Event handler for "Show More" button click
        // Increases the number of visible entries by 10 and reloads the list
        private void ShowMore_Click(object sender, RoutedEventArgs e)
        {
            displayCount += 10;
            LoadInitialEntries();
        }

        // Static method to add a new log entry from anywhere in the application
        public static void AddLog(string action, string details)
        {
            // Optional: Prevent excessive memory usage
            if (AllLogEntries.Count >= MaxLogEntries)
            {
                AllLogEntries.RemoveAt(AllLogEntries.Count - 1); // remove oldest
            }

            // Insert new entry at the top of the list (most recent first)
            AllLogEntries.Insert(0, new ActivityEntry
            {
                Time = DateTime.Now.ToString("g"),
                Action = action,
                Details = details
            });
        }

        // ------------ Navigation Buttons ------------

        // Navigate to Home window and close this one
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            new Home().Show();
            Close();
        }

        // Navigate to ChatBot window and close this one
        private void ChatBotButton_Click(object sender, RoutedEventArgs e)
        {
            new ChatBot().Show();
            Close();
        }

        // Navigate to Tasks window and close this one
        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            new Tasks().Show();
            Close();
        }

        // Navigate to Quiz window and close this one
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            new Quiz().Show();
            Close();
        }
        // ActivityLog button clicked while already on ActivityLog, so do nothing
        private void ActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            // Already on ActivityLog
        }
    }
}
