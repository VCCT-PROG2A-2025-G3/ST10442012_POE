using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ST10442012_POE
{
    public partial class ChatBot : Window
    {
        private ChatbotLogic chatbot;
        private string userName = "";
        private string userSkillLevel = "";
        private string favoriteTopic = "";
        private readonly SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        private ObservableCollection<ChatMessage> Messages = new ObservableCollection<ChatMessage>();

        public ChatBot()
        {
            InitializeComponent();
            synthesizer.SelectVoiceByHints(VoiceGender.Neutral);
            ChatMessagesList.ItemsSource = Messages;
            chatbot = new ChatbotLogic();

            SpeakAndType(chatbot.GetInitialGreeting());
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            // Exit command handling
            if (userInput.ToLower() == "exit")
            {
                AppendMessage(userName == "" ? "You" : userName, userInput);
                RemoveThinking();
                await SpeakAndType($"Goodbye {userName}! Stay safe online.");

                var result = MessageBox.Show("Chat ended. Start a new conversation?", "Exit", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    RestartChat();
                }
                return;
            }

            string senderName = string.IsNullOrEmpty(userName) ? "You" : userName;
            AppendMessage(senderName, userInput);
            UserInputTextBox.Clear();

            AppendThinking();
            await Task.Delay(800);

            string response = "";

            if (string.IsNullOrEmpty(userName))
            {
                response = chatbot.SetUserName(userInput);
                if (response.Contains("Welcome"))
                {
                    userName = userInput;
                    RemoveThinking();
                    await SpeakAndType(response);
                    AppendThinking();
                    await Task.Delay(800);
                    response = "Please enter your cybersecurity skill level (Beginner, Intermediate, or Advanced):";

                    ActivityLog.AddLog("Conversation", $"User: {userInput} | Bot: {response}");
                }
            }
            else if (string.IsNullOrEmpty(userSkillLevel))
            {
                response = chatbot.ValidateSkillLevel(userInput, out userSkillLevel);
                if (!string.IsNullOrEmpty(userSkillLevel))
                {
                    RemoveThinking();
                    await SpeakAndType(response);
                    AppendThinking();
                    await Task.Delay(800);
                    response = "What is your favorite cybersecurity topic? (Passwords, Phishing, Malware, Safe Browsing, Social Engineering, Mobile Security, or Data Privacy):";


                    ActivityLog.AddLog("Conversation", $"User: {userInput} | Bot: {response}");
                }
            }
            else if (string.IsNullOrEmpty(favoriteTopic))
            {
                string topicResponse = chatbot.ValidateFavoriteTopic(userInput, out string validTopic, userName);


                if (!string.IsNullOrEmpty(validTopic))
                {
                    favoriteTopic = validTopic; // Only set this once!
                }
                chatbot.SetFavoriteTopic(favoriteTopic);


                RemoveThinking();
                await SpeakAndType(topicResponse);

                if (!string.IsNullOrEmpty(validTopic))
                {
                    AppendThinking();
                    await Task.Delay(800);
                    ActivityLog.AddLog("Conversation", $"User: {userInput} | Bot: {topicResponse}");
                }
            }

            else
            {
                response = chatbot.HandleUserInput(userInput);
                ActivityLog.AddLog("Conversation", $"User: {userInput} | Bot: {response}");
            }

            RemoveThinking();
            await SpeakAndType(response);
        }

        private void AppendMessage(string sender, string message)
        {
            bool isUser = sender != "CyboSecureBot";

            Messages.Add(new ChatMessage
            {
                Sender = sender,
                Message = message,
                IsUser = isUser,
                Alignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                BackgroundColor = isUser ? "#000000" : "#ECECEC", // black for user, light gray for bot
                ForegroundColor = isUser ? "#FFFFFF" : "#000000"  // white text for user, black text for bot
            });

            ScrollToBottom();
        }



        private void AppendThinking()
        {
            Messages.Add(new ChatMessage
            {
                Sender = "CyboSecureBot",
                Message = "Thinking...",
                IsUser = false,
                Alignment = HorizontalAlignment.Left,
                BackgroundColor = "#ECECEC",
                ForegroundColor = "#888888"
            });
            ScrollToBottom();
        }

        private void RemoveThinking()
        {
            if (Messages.Count > 0 && Messages[Messages.Count - 1].Message == "Thinking...")
            {
                Messages.RemoveAt(Messages.Count - 1);
            }
        }
        private async Task SpeakAndType(string message)
        {
            // 🔒 Disable UI input during bot response
            SendButton.IsEnabled = false;
            UserInputTextBox.IsEnabled = false;

            synthesizer.SpeakAsync(message);
            await TypeEffect("CyboSecureBot", message);

            // ✅ Re-enable UI after bot is done
            SendButton.IsEnabled = true;
            UserInputTextBox.IsEnabled = true;
            UserInputTextBox.Focus(); // puts cursor back
        }



        private async Task TypeEffect(string sender, string fullMessage)
        {
            string temp = "";
            foreach (char c in fullMessage)
            {
                temp += c;
                if (Messages.Count > 0 && Messages[Messages.Count - 1].Message == "Thinking...")
                {
                    Messages[Messages.Count - 1].Message = temp;
                }
                else
                {
                    if (Messages.Count == 0 || Messages[Messages.Count - 1].Sender != sender || Messages[Messages.Count - 1].Message == fullMessage)
                    {
                        Messages.Add(new ChatMessage
                        {
                            Sender = sender,
                            Message = temp,
                            IsUser = false,
                            Alignment = HorizontalAlignment.Left,
                            BackgroundColor = "#ECECEC",
                            ForegroundColor = "#000000"
                        });
                    }
                    else
                    {
                        Messages[Messages.Count - 1].Message = temp;
                    }
                }

                ChatMessagesList.Items.Refresh();
                ScrollToBottom();
                await Task.Delay(60); // SLOWER typing effect for better sync
            }
        }

        private void ScrollToBottom()
        {
            if (ChatMessagesList.Items.Count > 0)
            {
                ChatMessagesList.ScrollIntoView(ChatMessagesList.Items[ChatMessagesList.Items.Count - 1]);
            }
        }

        private void RestartChat()
        {
            userName = "";
            userSkillLevel = "";
            favoriteTopic = "";
            Messages.Clear();
            chatbot = new ChatbotLogic();
            SpeakAndType(chatbot.GetInitialGreeting());
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();

            new Home().Show();  // Opens Home.xaml
            this.Close();       // Closes the current ChatBot window
        }

        private void ChatBotButton_Click(object sender, RoutedEventArgs e)
        {
            // Already on ChatBot, do nothing or show message
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();

            new Tasks().Show(); // Opens Tasks.xaml
            this.Close();
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();

            new Quiz().Show();  // Opens Quiz.xaml
            this.Close();
        }

        private void ActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();

            new ActivityLog().Show();  // Opens ActivityLog.xaml
            this.Close();
        }

    }

    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public bool IsUser { get; set; }
        public HorizontalAlignment Alignment { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
    }
}
