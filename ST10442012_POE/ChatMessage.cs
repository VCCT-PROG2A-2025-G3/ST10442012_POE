using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;


//--------------------------|| Description ||--------------------------
// Represents a single chat message in the conversation between the user and the chatbot.
// Contains information about who sent the message (user or bot), the message content,
// and styling properties such as text alignment and colors for displaying the message in the UI.


namespace ST10442012_POE
{
    class ChatMessage_
    {
        public string Sender { get; set; }       // "User" or "Bot"
        public string Message { get; set; }      // The actual text message
        public HorizontalAlignment Alignment { get; set; } // Align left or right for UI
        public Brush BackgroundColor { get; set; } // Background color for the message bubble
        public Brush ForegroundColor { get; set; } // Text color for the message

    }
}
