using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10442012_POE
{
    class ActivityLogger
    {


        public static List<string> LogEntries = new List<string>();

        public static void AddEntry(string entry)
        {
            string timestamp = DateTime.Now.ToString("HH:mm");
            LogEntries.Add($"[{timestamp}] {entry}");

            // Keep only last 10 entries
            if (LogEntries.Count > 10)
                LogEntries.RemoveAt(0);
        }



    }
}
