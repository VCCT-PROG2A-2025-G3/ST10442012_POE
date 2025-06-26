using ST10442012_POE;
using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;





//-------- Description --------

// The ChatBot.xaml.cs class represents the main chatbot interface window.
// It manages the chat conversation flow, collecting user details such as
// name, cybersecurity skill level, and favorite topic.
// The class interacts with the ChatbotLogic backend to process user input
// and generate responses, and provides text-to-speech output along with
// a typing animation for responses.
// It also handles UI updates for chat messages, thinking indicators, and 
// navigation between different application windows.
// The chat messages are stored in an ObservableCollection bound to the UI.
// SpeechSynthesizer is used for voice output of chatbot messages.










// This is the code-behind for the chatbot interface in my app. It handles everything the user does during the conversation.

// I store user details like their name, cybersecurity skill level, and favorite topic, and I use a SpeechSynthesizer to let the bot speak aloud.

// I also use an ObservableCollection to hold and display the chat messages in the UI.

// At the heart of this class is a connection to my ChatbotLogic class. ChatbotLogic handles all the decision-making and responses — so when a user types something, I send it to ChatbotLogic, and it gives back the right reply.

// The main method here is SendButton_Click. It checks what the user typed, collects their info if needed, or sends it to ChatbotLogic to process. I also handle the "exit" command to end the chat.

// To make the chatbot feel more human, I use a method called SpeakAndType that talks and types out the bot's response with a nice animation.

// I also have helper methods like AppendMessage, AppendThinking, and RemoveThinking to update the UI as messages come in.

// Finally, I include navigation methods to move between other windows like Home, Quiz, Tasks, and Activity Log."

//-------- End Description --------

namespace ST10442012_POE
{
    public partial class ChatBot : Window
    {
        // Instance of the chatbot logic class handling responses and validations
        private ChatbotLogic chatbot;

        // Variables to store user details collected during the conversation
        private string userName = "";
        private string userSkillLevel = "";
        private string favoriteTopic = "";

        // SpeechSynthesizer instance for text-to-speech functionalit
        private readonly SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        // Collection bound to the UI list control to display chat messages
        private ObservableCollection<ChatMessage> Messages = new ObservableCollection<ChatMessage>();


        // <summary>
        // Initializes the ChatBot window, sets up speech synthesizer,
        // binds message collection to the UI, and starts the chat with an initial greeting.
        // </summary>
        public ChatBot()
        {
            InitializeComponent();
            // Select a neutral voice for speech synthesis
            synthesizer.SelectVoiceByHints(VoiceGender.Neutral);

            // Bind the message list to the UI control to display chat messages
            ChatMessagesList.ItemsSource = Messages;

            // Initialize chatbot logic backend
            chatbot = new ChatbotLogic();
            // Start conversation by speaking and displaying the initial greeting message
            SpeakAndType(chatbot.GetInitialGreeting());
        }


        // <summary>
        // Handles the Send button click event.
        // Processes user input, updates UI, manages conversation flow including user info collection,
        // and retrieves chatbot responses asynchronously with speech and typing effects.
        // </summary>
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {

            // Ignore empty input
            string userInput = UserInputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            // Handle exit command to end chat and optionally restart
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

            // Append user message to chat window
            string senderName = string.IsNullOrEmpty(userName) ? "You" : userName;
            AppendMessage(senderName, userInput);
            UserInputTextBox.Clear();

            AppendThinking();
            await Task.Delay(800);

            // Validate and set skill level
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
                // Validate and set favorite topic
                string topicResponse = chatbot.ValidateFavoriteTopic(userInput, out string validTopic, userName); // Save the valid topic only once


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
                // After initial data collection, process general user inputs
                response = chatbot.HandleUserInput(userInput);
                ActivityLog.AddLog("Conversation", $"User: {userInput} | Bot: {response}");
            }

            RemoveThinking();
            await SpeakAndType(response);
        }


        // <summary>
        // Adds a new message to the chat message list with appropriate styling based on sender.
        // </summary>

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

        // <summary>
        // Adds a "Thinking..." placeholder message indicating the bot is processing.
        // </summary>

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
        // <summary>
        // Removes the "Thinking..." placeholder message from the chat if present.
        // </summary>
        private void RemoveThinking()
        {
            if (Messages.Count > 0 && Messages[Messages.Count - 1].Message == "Thinking...")
            {
                Messages.RemoveAt(Messages.Count - 1);
            }
        }

        // <summary>
        // Performs speech synthesis and simulates typing effect to display the chatbot's response.
        // Disables user input while speaking/typing.
        // </summary>
        private async Task SpeakAndType(string message)
        {
            // Disable UI inputs during bot response to avoid concurrent inputs
            SendButton.IsEnabled = false;
            UserInputTextBox.IsEnabled = false;

            // Speak the message asynchronously
            synthesizer.SpeakAsync(message);

            // Show typing animation with message
            await TypeEffect("CyboSecureBot", message);

            // Re-enable UI inputs after response
            SendButton.IsEnabled = true;
            UserInputTextBox.IsEnabled = true;
            UserInputTextBox.Focus(); // puts cursor back
        }


        // <summary>
        // Simulates a typing animation by gradually adding characters to the displayed message.
        // </summary>
        private async Task TypeEffect(string sender, string fullMessage)
        {
            string temp = "";
            foreach (char c in fullMessage)
            {
                temp += c;
                if (Messages.Count > 0 && Messages[Messages.Count - 1].Message == "Thinking...")
                {
                    // Replace "Thinking..." with partial message while typing
                    Messages[Messages.Count - 1].Message = temp;
                }
                else
                {
                    // Add new message or update last message with typing progress
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
        // <summary>
        // Scrolls the chat message list view to the most recent message.
        // </summary>
        private void ScrollToBottom()
        {
            if (ChatMessagesList.Items.Count > 0)
            {
                ChatMessagesList.ScrollIntoView(ChatMessagesList.Items[ChatMessagesList.Items.Count - 1]);
            }
        }
        // <summary>
        // Resets the chat state, clearing messages and user data,
        // and restarts the chatbot with the initial greeting.
        // </summary>
        private void RestartChat()
        {
            userName = "";
            userSkillLevel = "";
            favoriteTopic = "";
            Messages.Clear();
            chatbot = new ChatbotLogic();
            SpeakAndType(chatbot.GetInitialGreeting());
        }
        // <summary>
        // Event handler for Home button click.
        // Cancels any ongoing speech, opens the Home window, and closes this chat window.
        // </summary>
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();

            new Home().Show();  // Opens Home.xaml
            this.Close();       // Closes the current ChatBot window
        }

        // <summary>
        // Event handler for ChatBot button click.
        // Currently does nothing since the user is already on the ChatBot page.
        // </summary>
        private void ChatBotButton_Click(object sender, RoutedEventArgs e)
        {
            // Already on ChatBot, do nothing or show message
        }
        // <summary>
        // Event handler for Tasks button click.
        // Cancels speech, opens Tasks window, and closes this window.
        // </summary>
        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();

            new Tasks().Show(); // Opens Tasks.xaml
            this.Close();
        }

        // <summary>
        // Event handler for Quiz button click.
        // Cancels speech, opens Quiz window, and closes this window.
        //  </summary>
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();

            new Quiz().Show();  // Opens Quiz.xaml
            this.Close();
        }

        // <summary>
        // Event handler for Activity Log button click.
        // Cancels speech, opens ActivityLog window, and closes this window.
        // </summary>
        private void ActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();

            new ActivityLog().Show();  // Opens ActivityLog.xaml
            this.Close();
        }

    }


    // <summary>
    // Represents a chat message displayed in the chat window,
    // including sender, message content, styling, and alignment.
    // </summary>
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
//----------// End of ChatBot.xaml.cs //----------//