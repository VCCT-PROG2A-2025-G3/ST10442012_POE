using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


//------------------------|| Description ||------------------------
// This is the code-behind for the Home window of the Cybersecurity Awareness Chatbot application.
// It manages the main menu, allowing users to navigate to different sections: ChatBot, Quiz, Tasks, and Activity Log.
// When a menu button is clicked, it opens the corresponding window and hides or closes the Home window
// to ensure smooth user experience and proper window management.
//---------------------------------------------------------------

namespace ST10442012_POE
{
    // <summary>
    // Interaction logic for MainWindow.xaml
    // </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        // Handles the ChatBot button click event
        // Opens the ChatBot window and hides the current Home window
        private void ChatBot_Click(object sender, RoutedEventArgs e)
        {
            ChatBot chatbotWindow = new ChatBot();
            chatbotWindow.Show();
            this.Hide(); // Hide the Home window to keep it in memory but not visible
        }

        // Handles the Quiz button click event
        // Opens the Quiz window and closes the Home window
        private void Quiz_Click(object sender, RoutedEventArgs e)
        {
            Quiz quizWindow = new Quiz();
            quizWindow.Show();
            this.Close(); // Close the Home window as Quiz window takes focus
        }

        // Handles the Tasks button click event
        // Opens the Tasks window and closes the Home window
        private void Tasks_Click(object sender, RoutedEventArgs e)
        {
            Tasks tasksWindow = new Tasks();
            tasksWindow.Show();
            this.Close(); // Close the Home window when navigating away
        }

        // Handles the Activity Log button click event
        // Opens the ActivityLog window and closes the Home window
        private void ActivityLog_Click(object sender, RoutedEventArgs e)
        {
            ActivityLog activityLogWindow = new ActivityLog();
            activityLogWindow.Show();
            this.Close(); // Close Home window to focus on Activity Log
        }

    }
}