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
        public string TaskStatus { get; set; } // this will hold one of three values "Pending" "In Progress" or "Done"

        // empty constructor for JSON deserialisation
        public Task() { }

        // Constructor and its paramaters
        public Task(string taskid, string taskName, DateTime taskDueDate, string taskProject, string initialTaskStatus = "Pending")
        {
            TaskID = taskid;
            TaskName = taskName;
            TaskDueDate = taskDueDate;
            TaskProject = taskProject;
            TaskStatus = initialTaskStatus;
        }

        // method for what the task info displays
        public void DisplayTaskInfo()
        {
            Console.WriteLine($" ID {TaskID}, Task Name: {TaskName}, Due Date: {TaskDueDate.ToShortDateString()}, Project: {TaskProject},  Status: {TaskStatus}");
        }

        //Method for marking a task as "Pending"
        public void MarkAsPending()
        {
            TaskStatus = "PENDING";
        }

        // Method for marking status as "in progress"
        public void MarkAsInProgress()
        {
            TaskStatus = "IN PROGRESS";
        }

        // method for changing status when task is marked complete.
        public void MarkAsDone()
        {
            TaskStatus = "DONE";
        }
    }
}