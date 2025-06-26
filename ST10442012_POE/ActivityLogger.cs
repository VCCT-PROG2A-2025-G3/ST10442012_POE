using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10442012_POE
{
    // --------------------|| ActivityLogger Class ||--------------------
    // This class is responsible for keeping a simple activity log.
    // It stores time-stamped string entries in a static list.
    // Only the latest 10 entries are kept to limit memory usage.

    class ActivityLogger
    {
        // --------------------|| Log Storage ||--------------------
        // Static list to store log entries globally.
        public static List<string> LogEntries = new List<string>();

        // --------------------|| Add Log Entry ||--------------------
        // Adds a new log entry with a timestamp.
        // If more than 10 entries exist, the oldest one is removed.
        public static void AddEntry(string entry)
        {
            // Create a timestamp for the current time (hour and minutes)
            string timestamp = DateTime.Now.ToString("HH:mm");

            // Add the formatted log entry to the list
            LogEntries.Add($"[{timestamp}] {entry}");

            // Keep only the last 10 entries to avoid clutter
            if (LogEntries.Count > 10)
                LogEntries.RemoveAt(0);
        }
    }
}
