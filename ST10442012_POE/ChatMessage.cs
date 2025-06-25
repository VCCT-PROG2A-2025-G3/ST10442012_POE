using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace ST10442012_POE
{
    class ChatMessage_
    {
        public string Sender { get; set; }       // "User" or "Bot"
        public string Message { get; set; }      // The actual text message
        public HorizontalAlignment Alignment { get; set; } // Align left or right for UI
        public Brush BackgroundColor { get; set; }
        public Brush ForegroundColor { get; set; }
    }
}
