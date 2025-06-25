using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10442012_POE
{
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

