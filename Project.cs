using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace Individuella_projekt_1_To_doList
{
    public class Project
    {
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectDueDate { get; set; }
        public List<Task> ProjectTasks { get; set; }
        public string ProjectStatus { get; set; }
        public double CompletionPercentage { get; set; }

        public Project(string projectID, string projectName, DateTime projectDueDate, List<Task> projectTasks, string initialStatus = "Pending")
        {
            ProjectID = projectID;
            ProjectName = projectName;
            ProjectDueDate = projectDueDate;
            ProjectTasks = projectTasks;
            ProjectStatus = initialStatus;
        }
        public Project(string projectID, string projectName, DateTime projectDueDate)
        {
            ProjectID = projectID;
            ProjectName = projectName;
            ProjectDueDate = projectDueDate;
            ProjectTasks = new List<Task>(); // Initialize an empty list of tasks
            ProjectStatus = "Pending";      // Set initial status to Pending
            UpdateProjectStatusBasedOnTasks(); // Ensure completion percentage is calculated (0% when there are no tasks.)
        }

        public void DisplayProjectInfo() // Displays project information
        {
            UpdateProjectStatusBasedOnTasks();
            Console.WriteLine($" Project ID: {ProjectID}, Project Name: {ProjectName}, Project Due Date: {ProjectDueDate.ToShortDateString()}");
            Console.WriteLine($"Project Status {ProjectStatus} ({CompletionPercentage:F0}%)");
            DisplayProgressBar();
            Console.WriteLine();
        }

        public void AddTask(Task task) // adds a task to a project
        {
            if (ProjectTasks == null)
            {
                ProjectTasks = new List<Task>();
            }
            ProjectTasks.Add(task);
            Console.WriteLine($"Task '{task.TaskName}' (ID: {task.TaskID}) added to project '{ProjectName}'.");
            UpdateProjectStatusBasedOnTasks(); // Updates status after adding tasks

        }

        public void RemoveTask(Task task)// lets you remove a task from a project
        {
            if (ProjectTasks != null) // Only try to remove if the list exists
            {
                ProjectTasks.Remove(task);
                UpdateProjectStatusBasedOnTasks(); // Update status after removing tasks
            }
        }


        public void ListAllProjectTasks() // lists all the tasks associated with the project
        {
            Console.WriteLine($"--- Tasks for Project '{ProjectName}' (ID: {ProjectID}) ---");
            HelperMethods.DisplayTasksInTable(ProjectTasks);
        }


        public void UpdateProjectStatusBasedOnTasks() // updates status based on tasks
        {
            if (ProjectTasks == null || ProjectTasks.Count == 0)
            {
                CompletionPercentage = 0;
                ProjectStatus = "Pending";
                return;
            }

            int totalTasks = ProjectTasks.Count;
            int completedTasks = 0;

            foreach (var task in ProjectTasks) // adds to the number of completed tasks if the task status is changed to Done
            {
                if (task.TaskStatus == "Done")
                {
                    completedTasks++;
                }
            }
            CompletionPercentage = (double)completedTasks / totalTasks * 100; // calculates the percentage completed

            if (CompletionPercentage == 100)
            {
                ProjectStatus = "Done";
            }

            else if (CompletionPercentage >= 1)
            {
                ProjectStatus = "In Progress";
            }

            else
            {
                ProjectStatus = "Pending";
            }

        }
        // method displays a progress bar for how much of the project is completed.
        public void DisplayProgressBar()
        {
            const int barLength = 20; // Length of the bar
            int filledLength = (int)Math.Round(CompletionPercentage / 100 * barLength);
            string progressBar = "[" + new string('\u25A0', filledLength) + new string('-', barLength - filledLength) + "]";

            Console.WriteLine($"Progress: {progressBar}");
        }
    }



}