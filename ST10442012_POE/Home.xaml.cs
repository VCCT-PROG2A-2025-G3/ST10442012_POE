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
// It handles user interactions with the main menu buttons and navigates to different windows accordingly.
// Each button click event creates and shows a new window (ChatBot, Quiz, Tasks, Activity Log).
// The Home window is either hidden or closed when navigating to another window to manage the user experience.

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

        private void ChatBot_Click(object sender, RoutedEventArgs e)
        {
            ChatBot chatbotWindow = new ChatBot();
            chatbotWindow.Show();
            this.Hide(); // Optional: hides the Home window
        }



        private void Quiz_Click(object sender, RoutedEventArgs e)
        {
            Quiz quizWindow = new Quiz();
            quizWindow.Show();
            this.Close();
        }

        private void Tasks_Click(object sender, RoutedEventArgs e)
        {
            Tasks tasksWindow = new Tasks();
            tasksWindow.Show();
            this.Close();
        }


        private void ActivityLog_Click(object sender, RoutedEventArgs e)
        {
            ActivityLog activityLogWindow = new ActivityLog();
            activityLogWindow.Show();
            this.Close();
        }

    }
}
