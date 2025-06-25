using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ST10442012_POE
{
    public partial class Quiz : Window
    {
        // --------|| Internal Question Class ||--------
        private class QuizQuestion
        {
            public string Question { get; set; }
            public string[] Options { get; set; }
            public int CorrectIndex { get; set; }
            public string Feedback { get; set; }
        }

        // --------|| Quiz State Variables ||--------
        private List<QuizQuestion> questions;
        private int currentQuestion = 0;
        private int score = 0;

        // --------|| Constructor ||--------
        public Quiz()
        {
            InitializeComponent();
            LoadQuestions();     // Load and shuffle 15 questions
            ShowQuestion();      // Display the first question
        }

        // --------|| Load and Randomize Questions ||--------
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
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int selected = -1;
            if (OptionA.IsChecked == true) selected = 0;
            else if (OptionB.IsChecked == true) selected = 1;
            else if (OptionC.IsChecked == true) selected = 2;
            else if (OptionD.IsChecked == true) selected = 3;

            if (selected == -1)
            {
                FeedbackText.Text = "Please select an answer.";
                FeedbackText.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            var q = questions[currentQuestion];
            bool isCorrect = (selected == q.CorrectIndex);  // <-- Add this line to track correctness

            if (isCorrect)
            {
                score++;
                FeedbackText.Text = q.Feedback;
                FeedbackText.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                FeedbackText.Text = $"Incorrect. {q.Feedback}";
                FeedbackText.Foreground = System.Windows.Media.Brushes.Red;
            }

            // --- Add this logging call right here ---
            ActivityLog.AddLog(
                "Quiz Attempt",
                $"Q{currentQuestion + 1}: {q.Question} | Selected: '{q.Options[selected]}' | Correct: '{q.Options[q.CorrectIndex]}' | Result: {(isCorrect ? "Correct" : "Incorrect")}"
            );

            SubmitButton.IsEnabled = false;

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                currentQuestion++;
                ShowQuestion();
            };
            timer.Start();
        }


        // --------|| Display Final Score ||--------
        private void ShowFinalScore()
        {
            QuizPanel.Visibility = Visibility.Visible;
            QuestionText.Text = "Quiz Complete!";
            OptionA.Visibility = OptionB.Visibility = OptionC.Visibility = OptionD.Visibility = Visibility.Collapsed;
            SubmitButton.Visibility = Visibility.Collapsed;

            ScoreText.Visibility = Visibility.Visible;
            ScoreText.Text = $"Your Score: {score} / {questions.Count}";

            if (score >= 8)
                FeedbackText.Text = "Great job! You're a cybersecurity pro!";
            else if (score >= 5)
                FeedbackText.Text = "Good effort! Keep learning to stay safe online!";
            else
                FeedbackText.Text = "Keep practicing! Review the feedback to improve your cybersecurity knowledge.";

            FeedbackText.Foreground = System.Windows.Media.Brushes.Blue;

            // Show redo button after quiz ends
            RedoButton.Visibility = Visibility.Visible;
        }

        // --------|| Redo Quiz Button Logic ||--------
        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestion = 0;
            score = 0;
            LoadQuestions();
            ShowQuestion();

            RedoButton.Visibility = Visibility.Collapsed;
            ScoreText.Visibility = Visibility.Collapsed;
            SubmitButton.Visibility = Visibility.Visible;
        }

        // --------|| Navigation Bar Buttons ||--------
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            new Home().Show();
            this.Close();
        }

        private void ChatBotButton_Click(object sender, RoutedEventArgs e)
        {
            new ChatBot().Show();
            this.Close();
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            new Tasks().Show();
            this.Close();
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            // Already in Quiz window
        }

        private void ActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            new ActivityLog().Show();
            this.Close();
        }
    }
}
