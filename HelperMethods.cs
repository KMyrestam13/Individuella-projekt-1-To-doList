using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individuella_projekt_1_To_doList
{
    public static class HelperMethods
    {
        // method to create a random ID that can be assigned
        //to the project and the task.
        private static Random random = new Random();

        private const string AlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomId(int length)
        {
            char[] idChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                idChars[i] = AlphanumericChars[random.Next(AlphanumericChars.Length)];
            }
            return new string(idChars);
        }

        public static void ClearConsole() // Method to clear the console screen
        {
            Console.Clear();
        }

        public static string GetStringInput(string prompt) // Method to get string input from the user
        {
            string input;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        public static int GetIntInput(string prompt) // Method to get integer input from the user
        {
            int input;
            while (true)
            {
                Console.WriteLine(prompt);
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a whole number.");
                }
            }
        }

        public static int GetIntInputNoRetry(string prompt) // Method to get integer input from the user
        {
            int input;
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out input))
            {
                return input;
            }
            return 0;
        }

        public static double GetDoubleInput(string prompt) // Method to get double input from the user
        {
            double input;
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }

        public static DateTime GetDateInput(string prompt) // Method to get date input from the user
        {
            DateTime date;
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out date))
                {
                    return date;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
                }
            }
        }

        public static void DisplayTasksInTable(List<Task> tasks) // Method to display tasks in a table
        {
            int idWidth = 8; // Define column headers and widths
            int nameWidth = 25;
            int dueDateWidth = 12;
            int statusWidth = 12;
            int delimiterWidth = 3; // spacer bars for header

            int totalWidth = idWidth + nameWidth + dueDateWidth + statusWidth + (delimiterWidth * 3); // Calculate total width including bars

            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine(
                "ID".PadRight(idWidth) + " | " +
                "Task Name".PadRight(nameWidth) + " | " +
                "Due Date".PadRight(dueDateWidth) + " | " +
                "Status".PadRight(statusWidth)
            );
            Console.WriteLine(new string('-', totalWidth));

            if (tasks == null || tasks.Count == 0) // checks if tasks list is empty
            {
                Console.WriteLine("No tasks to display.".PadRight(totalWidth));
            }
            else
            {
                foreach (var task in tasks) // Print each task row
                {
                    Console.WriteLine(
                        task.TaskID.PadRight(idWidth) + " | " +
                        task.TaskName.PadRight(nameWidth) + " | " +
                        task.TaskDueDate.ToShortDateString().PadRight(dueDateWidth) + " | " +
                        task.TaskStatus.PadRight(statusWidth)
                    );
                }
            }
            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine(); // Add an extra line for spacing
        }

        public static void DisplayProjectsInTable(List<Project> projects) // New method to display projects in a table
        {
            int idWidth = 8; // Define column headers and widths
            int nameWidth = 25;
            int dueDateWidth = 12;
            int notDoneWidth = 15;
            int percentDoneWidth = 10;
            int delimiterWidth = 3; // spacer bars for header

            int totalWidth = idWidth + nameWidth + dueDateWidth + notDoneWidth + percentDoneWidth + (delimiterWidth * 4); // Calculate total width including delimiters

            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine(
                "ID".PadRight(idWidth) + " | " +
                "Project Name".PadRight(nameWidth) + " | " +
                "Due Date".PadRight(dueDateWidth) + " | " +
                "Incomplete Tasks".PadRight(notDoneWidth) + " | " +
                "% Done".PadRight(percentDoneWidth)
            );
            Console.WriteLine(new string('-', totalWidth));

            if (projects == null || projects.Count == 0) // also checks if projects list is empty
            {
                Console.WriteLine("No projects to display.".PadRight(totalWidth));
            }
            else
            {
                foreach (var project in projects) // Print each project row
                {
                    int totalTasks;
                    if (project.ProjectTasks != null)
                    {
                        totalTasks = project.ProjectTasks.Count;
                    }
                    else
                    {
                        totalTasks = 0;
                    }

                    int completedTasks;
                    if (project.ProjectTasks != null)
                    {
                        completedTasks = project.ProjectTasks.Count(t => t.TaskStatus == "Done");
                    }
                    else
                    {
                        completedTasks = 0;
                    }

                    int notDoneTasks = totalTasks - completedTasks;
                    double percentCompleted;

                    if (totalTasks > 0)
                    {
                        percentCompleted = (double)completedTasks / totalTasks * 100;
                    }
                    else
                    {
                        percentCompleted = 0.0;
                    }

                    Console.WriteLine(
                        project.ProjectID.PadRight(idWidth) + " | " +
                        project.ProjectName.PadRight(nameWidth) + " | " +
                        project.ProjectDueDate.ToShortDateString().PadRight(dueDateWidth) + " | " +
                        notDoneTasks.ToString().PadRight(notDoneWidth) + " | " +
                        (percentCompleted.ToString("F0") + "%").PadRight(percentDoneWidth)
                    );
                }
            }
            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine(); // Add an extra line for spacing
        }

        public static void DisplayAllTasksAcrossProjectsInTable(List<Project> projects) // New method to display all tasks across all projects in a table
        {
            int projectIdWidth = 10; // Define column headers and widths
            int projectNameWidth = 20;
            int taskIdWidth = 8;
            int taskNameWidth = 25;
            int taskDueDateWidth = 12;
            int taskStatusWidth = 12;
            int delimiterWidth = 3; // spacer bars for header

            int totalWidth = projectIdWidth + projectNameWidth + taskIdWidth + taskNameWidth + taskDueDateWidth + taskStatusWidth + (delimiterWidth * 5); // Calculate total width including delimiters

            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine(
                "Project ID".PadRight(projectIdWidth) + " | " +
                "Project Name".PadRight(projectNameWidth) + " | " +
                "Task ID".PadRight(taskIdWidth) + " | " +
                "Task Name".PadRight(taskNameWidth) + " | " +
                "Due Date".PadRight(taskDueDateWidth) + " | " +
                "Status".PadRight(taskStatusWidth)
            );
            Console.WriteLine(new string('-', totalWidth));

            bool anyTasks = false;
            if (projects != null)
            {
                foreach (var project in projects)
                {
                    if (project.ProjectTasks != null && project.ProjectTasks.Count > 0) // also checks if tasks list is empty
                    {
                        anyTasks = true;
                        foreach (var task in project.ProjectTasks) // Print each task row within project
                        {
                            Console.WriteLine(
                                project.ProjectID.PadRight(projectIdWidth) + " | " +
                                project.ProjectName.PadRight(projectNameWidth) + " | " +
                                task.TaskID.PadRight(taskIdWidth) + " | " +
                                task.TaskName.PadRight(taskNameWidth) + " | " +
                                task.TaskDueDate.ToShortDateString().PadRight(taskDueDateWidth) + " | " +
                                task.TaskStatus.PadRight(taskStatusWidth)
                            );
                        }
                    }
                }
            }

            if (anyTasks == false) // If no tasks across all projects
            {
                Console.WriteLine("No tasks from any project to display.".PadRight(totalWidth));
            }
            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine(); // Add an extra line for spacing
        }
    }
}