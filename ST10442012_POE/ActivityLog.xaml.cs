using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ST10442012_POE
{
    public partial class ActivityLog : Window
    {
        public class ActivityEntry
        {
            public string Time { get; set; }
            public string Action { get; set; }
            public string Details { get; set; }
        }

        public static ObservableCollection<ActivityEntry> AllLogEntries { get; } = new ObservableCollection<ActivityEntry>();
        private ObservableCollection<ActivityEntry> visibleEntries = new ObservableCollection<ActivityEntry>();
        private int displayCount = 10;
        private const int MaxLogEntries = 1000; // Optional safety cap

        public ActivityLog()
        {
            InitializeComponent();
            ActivityListView.ItemsSource = visibleEntries;
            LoadInitialEntries();
        }

        private void LoadInitialEntries()
        {
            visibleEntries.Clear();
            foreach (var entry in AllLogEntries.Take(displayCount))
            {
                visibleEntries.Add(entry);
            }
        }

        private void ShowMore_Click(object sender, RoutedEventArgs e)
        {
            displayCount += 10;
            LoadInitialEntries();
        }

        public static void AddLog(string action, string details)
        {
            // Optional: Prevent excessive memory usage
            if (AllLogEntries.Count >= MaxLogEntries)
            {
                AllLogEntries.RemoveAt(AllLogEntries.Count - 1); // remove oldest
            }

            AllLogEntries.Insert(0, new ActivityEntry
            {
                Time = DateTime.Now.ToString("g"),
                Action = action,
                Details = details
            });
        }

        // ------------ Navigation Buttons ------------

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
            // Already on ActivityLog
        }
    }
}
