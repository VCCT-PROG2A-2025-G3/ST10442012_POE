using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10442012_POE
{


    // ---------------------------------------------------------------------------
    // TaskItem Class
    //
    // This class represents a task item in the application, encapsulating
    // properties such as the task title, description, an optional reminder date,
    // and completion status. It also provides read-only properties to display
    // the reminder date as text and to show the current status ("Completed" or "Pending").
    //
    // Used for managing and displaying tasks in the task management feature of the app.
    //
    
    // ---------------------------------------------------------------------------

    class TaskItem
    {



        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public string ReminderText => ReminderDate?.ToShortDateString() ?? "";
        public string Status => IsCompleted ? "Completed" : "Pending";
    }

    }

