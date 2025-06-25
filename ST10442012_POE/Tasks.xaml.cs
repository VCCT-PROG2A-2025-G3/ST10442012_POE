using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ST10442012_POE
{
    public partial class Tasks : Window
    {
        // --------|| Task List Collection ||--------
        // This ObservableCollection updates the ListView automatically when items are added/removed

        private ObservableCollection<TaskItem> taskList = new ObservableCollection<TaskItem>();


        // --------|| Constructor ||--------
        public Tasks()
        {
            InitializeComponent();
            lvTasks.ItemsSource = taskList;   // Link the list to the ListView

            // Enable/Disable reminder DatePicker based on checkbox
            chkSetReminder.Checked += (s, e) => dpReminderDate.IsEnabled = true;
            chkSetReminder.Unchecked += (s, e) => dpReminderDate.IsEnabled = false;
        }



        // --------|| Add Task Button Click ||-------
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {

            // Get user input
            string title = txtTaskTitle.Text.Trim();
            string desc = txtTaskDescription.Text.Trim();

            // Validate: title must not be empty

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Optional reminder

            DateTime? reminder = null;
            if (chkSetReminder.IsChecked == true)
            {
                if (dpReminderDate.SelectedDate == null)
                {
                    MessageBox.Show("Please select a reminder date or uncheck the reminder option.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                reminder = dpReminderDate.SelectedDate;
            }

            // Create and add new task
            var newTask = new TaskItem
            {
                Title = title,
                Description = desc,
                ReminderDate = reminder,
                IsCompleted = false
            };

            // Add to list
            taskList.Add(newTask);



            // Log the action in Activity Log
            ActivityLog.AddLog("Task Created", $"Title: {title}" +
                (reminder.HasValue ? $", Reminder: {reminder.Value.ToShortDateString()}" : ""));

            // Clear inputs after adding
            txtTaskTitle.Clear();
            txtTaskDescription.Clear();
            chkSetReminder.IsChecked = false;
            dpReminderDate.SelectedDate = null;

            // Show confirmation
            MessageBox.Show($"Task '{title}' added successfully!", "Task Added", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // --------|| Mark Task as Completed ||--------
        private void MarkCompleted_Click(object sender, RoutedEventArgs e)
        {

            // Check if a task is selected
            if (lvTasks.SelectedItem is TaskItem selectedTask)
            {
                if (selectedTask.IsCompleted)
                {
                    MessageBox.Show("Task is already marked as completed.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                // Mark task as done
                selectedTask.IsCompleted = true;
                lvTasks.Items.Refresh();


                // Log the action
                ActivityLog.AddLog("Task Completed", $"Title: {selectedTask.Title}");
            }
            else
            {
                MessageBox.Show("Please select a task to mark as completed.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // --------|| Delete Task ||--------
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (lvTasks.SelectedItem is TaskItem selectedTask)
            {
                // Confirm before deleting
                var result = MessageBox.Show($"Are you sure you want to delete the task '{selectedTask.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    taskList.Remove(selectedTask);

                    // Log the deletion
                    ActivityLog.AddLog("Task Deleted", $"Title: {selectedTask.Title}");
                }
            }
            else
            {
                MessageBox.Show("Please select a task to delete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // --------|| Navigation Buttons ||--------

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            var homeWindow = new Home();
            homeWindow.Show();
            this.Close();
        }

        private void ChatBotButton_Click(object sender, RoutedEventArgs e)
        {
            var chatBotWindow = new ChatBot();
            chatBotWindow.Show();
            this.Close();
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            // Already on Tasks page, do nothing or optionally refresh
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            var quizWindow = new Quiz();
            quizWindow.Show();
            this.Close();
        }

        private void ActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            var activityLogWindow = new ActivityLog();
            activityLogWindow.Show();
            this.Close();
        }
    }
}


//----------// End of Tasks.xaml.cs //----------//