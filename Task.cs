using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individuella_projekt_1_To_doList
{
    public class Task
    {
        // properties
        public string TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskDueDate { get; set; }
        public string TaskProject { get; set; }
        public string TaskStatus { get; set; } // this will hold one of three values "TO DO" "IN PROGRESS" or "DONE"

        // empty constructor for JSON deserialisation
        public Task() { }

        // Constructor and its paramaters
        public Task(string taskid, string taskName, DateTime taskDueDate, string taskProject, string initialTaskStatus = "TO DO")
        {
            TaskID = taskid;
            TaskName = taskName;
            TaskDueDate = taskDueDate;
            TaskProject = taskProject;
            TaskStatus = initialTaskStatus;
        }
    }
}