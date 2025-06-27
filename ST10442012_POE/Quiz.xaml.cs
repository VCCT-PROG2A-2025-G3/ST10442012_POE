using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;






// ---------------------------------------------------------------------------
// Quiz Window Class
//
// This class implements a cybersecurity quiz interface in a WPF application.
// It presents randomized multiple-choice questions to the user, handles answer
// submissions, provides feedback on answers, keeps track of the score, and
// displays the final result. The quiz also logs each attempt for activity tracking.
//
// Features:
// - Loads a randomized subset of 15 questions from a larger pool
// - Displays questions and multiple-choice answers with radio buttons
// - Validates user selection and gives immediate feedback
// - Keeps score of correct answers and shows final performance message
// - Allows retaking the quiz by resetting state and questions
// - Includes navigation buttons for moving between different app windows
//

// ---------------------------------------------------------------------------


namespace ST10442012_POE
{
    public partial class Quiz : Window
    {
        // --------|| Internal Question Class ||--------
        private class QuizQuestion
        {
            public string Question { get; set; } // The quiz question text
            public string[] Options { get; set; } // The possible answer options
            public int CorrectIndex { get; set; }   // Index of the correct answer option
            public string Feedback { get; set; }     // Feedback to show after answering
        }

        // --------|| Quiz State Variables ||--------
        private List<QuizQuestion> questions;   // List of questions to ask
        private int currentQuestion = 0;        // Current question index
        private int score = 0;                  //// Number of correct answers

        // --------|| Constructor ||--------
        public Quiz()
        {
            InitializeComponent();
            LoadQuestions();     // Load and shuffle 15 questions
            ShowQuestion();      // Display the first question
        }

        // --------|| Load and Randomize Questions ||--------
        // This method initializes the quiz with a random selection of questions
        // It selects 15 unique questions from a predefined list of 30.
        // This ensures variety in each quiz attempt.
        // // The questions are stored in a private list and shuffled to ensure randomness.
        private void LoadQuestions()
        {
            // Full list of 30 quiz questions
            var allQuestions = new List<QuizQuestion>
            {
                new QuizQuestion {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new[] { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Reporting phishing emails helps prevent scams."
                },
                new QuizQuestion {
                    Question = "True or False: You should use the same password for all your accounts.",
                    Options = new[] { "True", "False", "", "" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Using unique passwords increases your security."
                },
                new QuizQuestion {
                    Question = "Which of these is a strong password?",
                    Options = new[] { "password123", "Qw!7z$Lp9#", "123456", "myname2023" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Strong passwords use a mix of letters, numbers, and symbols."
                },
                new QuizQuestion {
                    Question = "What is the safest way to connect to public Wi-Fi?",
                    Options = new[] { "Use a VPN", "Turn off your firewall", "Share files", "Disable antivirus" },
                    CorrectIndex = 0,
                    Feedback = "Correct! A VPN encrypts your data on public Wi-Fi."
                },
                new QuizQuestion {
                    Question = "True or False: You should click links in emails from unknown senders.",
                    Options = new[] { "True", "False", "", "" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Never click suspicious links."
                },
                new QuizQuestion {
                    Question = "What is two-factor authentication?",
                    Options = new[] { "A type of malware", "A password manager", "A security method using two forms of verification", "A firewall" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Two-factor authentication adds an extra layer of security."
                },
                new QuizQuestion {
                    Question = "Which is a sign of a phishing website?",
                    Options = new[] { "HTTPS in the URL", "Misspelled words and suspicious links", "Official company logo", "Contact information" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Misspellings and suspicious links are red flags."
                },
                new QuizQuestion {
                    Question = "True or False: Antivirus software should be updated regularly.",
                    Options = new[] { "True", "False", "", "" },
                    CorrectIndex = 0,
                    Feedback = "Correct! Updates help protect against new threats."
                },
                new QuizQuestion {
                    Question = "What should you do if your device is infected with malware?",
                    Options = new[] { "Ignore it", "Run a malware scan and remove threats", "Unplug your device", "Send emails to friends" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Run a scan and remove any threats."
                },
                new QuizQuestion {
                    Question = "Which of these is a social engineering attack?",
                    Options = new[] { "Phishing", "Firewall", "Antivirus", "Encryption" },
                    CorrectIndex = 0,
                    Feedback = "Correct! Phishing is a common social engineering attack."
                },
                new QuizQuestion {
                    Question = "Which of these practices helps prevent identity theft?",
                    Options = new[] { "Sharing your PIN", "Using public computers for banking", "Shredding sensitive documents", "Using your pet's name as a password" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Shredding documents prevents personal info from being stolen."
                },
                new QuizQuestion {
                    Question = "What is a firewall used for?",
                    Options = new[] { "Cooling down hardware", "Monitoring employee behavior", "Blocking unauthorized access", "Encrypting email" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Firewalls block unauthorized access to your system."
                },
                new QuizQuestion {
                    Question = "Why is it risky to use outdated software?",
                    Options = new[] { "It takes up more space", "It looks old-fashioned", "It may have unpatched security flaws", "It runs faster" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Old software can be vulnerable to known exploits."
                },
                new QuizQuestion {
                    Question = "Which of these is a secure way to store your passwords?",
                    Options = new[] { "In a notebook", "In a plain text file", "In your browser only", "In a password manager" },
                    CorrectIndex = 3,
                    Feedback = "Correct! Password managers are designed to securely store passwords."
                },
                new QuizQuestion {
                    Question = "What does encryption do to your data?",
                    Options = new[] { "Deletes it", "Makes it unreadable to unauthorized users", "Sends it to others", "Hides it behind images" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Encryption makes data unreadable without a key."
                },
                new QuizQuestion {
                    Question = "What does a phishing email often try to do?",
                    Options = new[] { "Teach you security tips", "Trick you into giving personal info", "Clean your inbox", "Encrypt your files" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Phishing tries to steal your info."
                },
                new QuizQuestion {
                    Question = "Which of these is a common form of malware?",
                    Options = new[] { "Firewall", "Virus", "Router", "Browser" },
                    CorrectIndex = 1,
                    Feedback = "Correct! A virus is a type of malware."
                },
                new QuizQuestion {
                    Question = "How often should you change your passwords?",
                    Options = new[] { "Every 5 years", "Only when hacked", "Regularly and when compromised", "Never" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Change them regularly and when needed."
                },
                new QuizQuestion {
                    Question = "What is the purpose of a CAPTCHA?",
                    Options = new[] { "To block viruses", "To ensure user is human", "To reset passwords", "To speed up logins" },
                    CorrectIndex = 1,
                    Feedback = "Correct! CAPTCHAs help block bots."
                },
                new QuizQuestion {
                    Question = "What’s one way cybercriminals steal data?",
                    Options = new[] { "Browsing safely", "Installing security updates", "Using ransomware", "Using strong passwords" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Ransomware encrypts your data for ransom."
                },
                new QuizQuestion {
                    Question = "What is a secure connection protocol for websites?",
                    Options = new[] { "FTP", "HTTP", "HTTPS", "SMTP" },
                    CorrectIndex = 2,
                    Feedback = "Correct! HTTPS is secure."
                },
                new QuizQuestion {
                    Question = "What should you do if you receive a suspicious text message?",
                    Options = new[] { "Reply with 'Stop'", "Click the link to check", "Delete and report it", "Forward it to friends" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Delete and report such messages."
                },
                new QuizQuestion {
                    Question = "What is a digital footprint?",
                    Options = new[] { "Your shoe size", "Your online activity history", "An internet password", "Computer virus" },
                    CorrectIndex = 1,
                    Feedback = "Correct! It’s the trail of your online actions."
                },
                new QuizQuestion {
                    Question = "Which device is at risk without a password?",
                    Options = new[] { "Microwave", "Smartphone", "Fridge", "TV remote" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Smartphones must be locked and secure."
                },
                new QuizQuestion {
                    Question = "What is spyware?",
                    Options = new[] { "Software to improve performance", "Security software", "Software that secretly collects info", "Antivirus tool" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Spyware secretly collects your information."
                },
                new QuizQuestion {
                    Question = "Which of these is a bad security practice?",
                    Options = new[] { "Using two-factor authentication", "Sharing your login details", "Locking your devices", "Installing antivirus" },
                    CorrectIndex = 1,
                    Feedback = "Correct! Never share login details."
                },
                new QuizQuestion {
                    Question = "Which platform is often used for scams?",
                    Options = new[] { "Email", "Secure banking apps", "Official government sites", "Offline TV ads" },
                    CorrectIndex = 0,
                    Feedback = "Correct! Scammers love using email."
                },
                new QuizQuestion {
                    Question = "What is a common goal of a cyberattack?",
                    Options = new[] { "To entertain", "To fix bugs", "To steal or destroy data", "To update apps" },
                    CorrectIndex = 2,
                    Feedback = "Correct! Many attacks aim to steal data."
                },
                new QuizQuestion {
                    Question = "Which setting should be turned off in public places?",
                    Options = new[] { "Bluetooth and auto-connect Wi-Fi", "Dark mode", "Screen rotation", "Brightness" },
                    CorrectIndex = 0,
                    Feedback = "Correct! Disable auto-connect features in public."
                }
            };

            // Randomly select 15 unique questions
            var rnd = new Random();
            questions = allQuestions.OrderBy(q => rnd.Next()).Take(15).ToList();
        }

        // --------|| Display Current Question ||--------
        // This method updates the UI to show the current question and its options.
        // It also handles visibility of options based on whether they are empty.
        // It resets the feedback text and enables the submit button.
        private void ShowQuestion()
        {
            if (currentQuestion < questions.Count)
            {
                var q = questions[currentQuestion];
                QuestionText.Text = $"Q{currentQuestion + 1}: {q.Question}";
                OptionA.Content = q.Options[0];
                OptionB.Content = q.Options[1];
                OptionC.Content = string.IsNullOrWhiteSpace(q.Options[2]) ? null : q.Options[2];
                OptionD.Content = string.IsNullOrWhiteSpace(q.Options[3]) ? null : q.Options[3];

                OptionA.Visibility = string.IsNullOrWhiteSpace(q.Options[0]) ? Visibility.Collapsed : Visibility.Visible;
                OptionB.Visibility = string.IsNullOrWhiteSpace(q.Options[1]) ? Visibility.Collapsed : Visibility.Visible;
                OptionC.Visibility = string.IsNullOrWhiteSpace(q.Options[2]) ? Visibility.Collapsed : Visibility.Visible;
                OptionD.Visibility = string.IsNullOrWhiteSpace(q.Options[3]) ? Visibility.Collapsed : Visibility.Visible;

                OptionA.IsChecked = false;
                OptionB.IsChecked = false;
                OptionC.IsChecked = false;
                OptionD.IsChecked = false;

                FeedbackText.Text = "";
                SubmitButton.IsEnabled = true;
            }
            else
            {
                ShowFinalScore();
            }
        }

        // --------|| Handle Submit Button ||--------
        // This method processes the user's answer selection, checks correctness,
        // provides feedback, and logs the attempt.
        // It also manages the transition to the next question or final score display.

        

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //--------------------|| Get Selected Option ||--------------------
            // Default value -1 means no option selected yet
            int selected = -1;

            // Check which radio button is selected and assign its index
            if (OptionA.IsChecked == true) selected = 0;
            else if (OptionB.IsChecked == true) selected = 1;
            else if (OptionC.IsChecked == true) selected = 2;
            else if (OptionD.IsChecked == true) selected = 3;

            //--------------------|| Validate Selection ||--------------------
            if (selected == -1)
            {
                // If no option was selected, show warning and exit method
                FeedbackText.Text = "Please select an answer.";
                FeedbackText.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            //--------------------|| Check Answer ||--------------------
            var q = questions[currentQuestion]; // Get current question
            bool isCorrect = (selected == q.CorrectIndex); // Compare user's selection to the correct answer

            // Provide feedback based on whether the answer was correct
            if (isCorrect)
            {
                score++; // Increment score if correct
                FeedbackText.Text = q.Feedback;
                FeedbackText.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                FeedbackText.Text = $"Incorrect. {q.Feedback}";
                FeedbackText.Foreground = System.Windows.Media.Brushes.Red;
            }

            //--------------------|| Log Quiz Attempt ||--------------------
            ActivityLog.AddLog(
                "Quiz Attempt",
                $"Q{currentQuestion + 1}: {q.Question} | Selected: '{q.Options[selected]}' | Correct: '{q.Options[q.CorrectIndex]}' | Result: {(isCorrect ? "Correct" : "Incorrect")}"
            );

            //--------------------|| Disable Submit & Start Timer ||--------------------
            SubmitButton.IsEnabled = false; // Prevent multiple submissions

            // Create a timer to move to the next question after 2 seconds
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);

            timer.Tick += (s, args) =>
            {
                timer.Stop(); // Stop the timer once it ticks
                currentQuestion++; // Move to next question
                ShowQuestion(); // Display the next question
            };

            timer.Start(); // Start the timer
        }



        //--------------------|| ShowFinalScore Method ||--------------------
        // This method displays the final score and feedback after the user completes the quiz.
        // It hides the quiz options, shows the user's score, and displays a message based on performance.
        // The "Redo" button is also shown to allow retrying the quiz.
        //-------------------------------------------------------------------

        private void ShowFinalScore()
        {
            // --------|| Show Main Quiz Panel ||--------
            QuizPanel.Visibility = Visibility.Visible;

            // --------|| Update Question Text ||--------
            QuestionText.Text = "Quiz Complete!";

            // --------|| Hide Option Buttons & Submit ||--------
            OptionA.Visibility = OptionB.Visibility = OptionC.Visibility = OptionD.Visibility = Visibility.Collapsed;
            SubmitButton.Visibility = Visibility.Collapsed;

            // --------|| Show Score ||--------
            ScoreText.Visibility = Visibility.Visible;
            ScoreText.Text = $"Your Score: {score} / {questions.Count}";

            // --------|| Show Feedback Based on Score ||--------
            if (score >= 8)
                FeedbackText.Text = "Great job! You're a cybersecurity pro!";
            else if (score >= 5)
                FeedbackText.Text = "Good effort! Keep learning to stay safe online!";
            else
                FeedbackText.Text = "Keep practicing! Review the feedback to improve your cybersecurity knowledge.";

            FeedbackText.Foreground = System.Windows.Media.Brushes.Blue;

            // --------|| Show Redo Button ||--------
            RedoButton.Visibility = Visibility.Visible;
        }


        // --------------------|| Redo Quiz Button Logic ||--------------------
        // This method resets the quiz when the Redo button is clicked.
        // It sets the current question and score back to zero, reloads the questions,
        // displays the first question again, and resets the UI.
        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestion = 0;                // Reset to first question
            score = 0;                          // Reset score
            LoadQuestions();                   // Reload all quiz questions
            ShowQuestion();                    // Display the first question

            RedoButton.Visibility = Visibility.Collapsed;   // Hide redo button
            ScoreText.Visibility = Visibility.Collapsed;    // Hide score text
            SubmitButton.Visibility = Visibility.Visible;   // Show submit button again
        }

        // --------------------|| Navigation Bar Buttons ||--------------------

        // --------|| Home Button ||--------
        // Navigates to the Home window and closes the current Quiz window
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            new Home().Show();
            this.Close();
        }

        // --------|| ChatBot Button ||--------
        // Navigates to the ChatBot window and closes the current Quiz window
        private void ChatBotButton_Click(object sender, RoutedEventArgs e)
        {
            new ChatBot().Show();
            this.Close();
        }

        // --------|| Tasks Button ||--------
        // Navigates to the Tasks window and closes the current Quiz window
        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            new Tasks().Show();
            this.Close();
        }

        // --------|| Quiz Button ||--------
        // Does nothing since the user is already in the Quiz window
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            // Already in Quiz window
        }

        // --------|| Activity Log Button ||--------
        // Navigates to the Activity Log window and closes the current Quiz window
        private void ActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            new ActivityLog().Show();
            this.Close();
        }

    }
}
