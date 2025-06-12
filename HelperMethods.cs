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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input cannot be empty. Please try again.");
                    Console.ResetColor();
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a whole number.");
                    Console.ResetColor();
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
            return -1;
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.");
                    Console.ResetColor();
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
            //stores original console colors.
            ConsoleColor originalForColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;

            // Defines the alternating colors for the rows in a table.
            ConsoleColor row1BgColor = ConsoleColor.Black;
            ConsoleColor row1FgColor = ConsoleColor.White;

            ConsoleColor row2BgColor = ConsoleColor.Gray;
            ConsoleColor row2FgColor = ConsoleColor.DarkBlue;

            // Define colors for overdue items
            ConsoleColor overdueBgColor = ConsoleColor.DarkRed; // A dark red background
            ConsoleColor overdueFgColor = ConsoleColor.White;

            //row counter. 
            int rowIndex = 0;

            Console.WriteLine(
                "ID".PadRight(idWidth) + " | " +
                "Task Name".PadRight(nameWidth) + " | " +
                "Due Date".PadRight(dueDateWidth) + " | " +
                "Status".PadRight(statusWidth)
            );
            Console.WriteLine(new string('-', totalWidth));

            if (tasks == null || tasks.Count == 0) // checks if tasks list is empty
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No tasks to display.".PadRight(totalWidth));
                Console.ResetColor();
            }
            else
            {
                foreach (var task in tasks) // Print each task row
                {
                    if (HelperMethods.IsTaskOverDue(task))
                    {

                        Console.BackgroundColor = overdueBgColor;
                        Console.ForegroundColor = overdueFgColor;

                    }
                    else if (rowIndex % 2 == 0) // says what colors even number rows should be 
                    {
                        Console.BackgroundColor = row1BgColor;
                        Console.ForegroundColor = row1FgColor;
                    }
                    else  // says what colors odd rows should be
                    {
                        Console.BackgroundColor = row2BgColor;
                        Console.ForegroundColor = row2FgColor;
                    }
                    Console.WriteLine(
                        task.TaskID.PadRight(idWidth) + " | " +
                        task.TaskName.PadRight(nameWidth) + " | " +
                        task.TaskDueDate.ToShortDateString().PadRight(dueDateWidth) + " | " +
                        task.TaskStatus.PadRight(statusWidth)
                    );
                    rowIndex++;// moves the index so that the row count increases
                }
                Console.BackgroundColor = originalBackColor; // resets colors after each row
                Console.ForegroundColor = originalForColor;
                Console.WriteLine(new string('-', totalWidth));
                Console.WriteLine(); // Add an extra line for spacing
            }
        }

        public static void DisplayProjectsInTable(List<Project> projects) // New method to display projects in a table
        {
            int idWidth = 8; // Define column headers and widths
            int nameWidth = 25;
            int dueDateWidth = 12;
            int notDoneWidth = 16;
            int percentDoneWidth = 10;
            int delimiterWidth = 3; // spacer bars for header

            int totalWidth = idWidth + nameWidth + dueDateWidth + notDoneWidth + percentDoneWidth + (delimiterWidth * 4); // Calculate total width including delimiters

            Console.WriteLine(
                "ID".PadRight(idWidth) + " | " +
                "Project Name".PadRight(nameWidth) + " | " +
                "Due Date".PadRight(dueDateWidth) + " | " +
                "Incomplete Tasks".PadRight(notDoneWidth) + " | " +
                "% Done".PadRight(percentDoneWidth)
            );
            Console.WriteLine(new string('-', totalWidth));


            //stores original console colors.
            ConsoleColor originalForColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;

            // Defines the alternating colors for the rows in a table.
            ConsoleColor row1BgColor = ConsoleColor.Black;
            ConsoleColor row1FgColor = ConsoleColor.White;

            ConsoleColor row2BgColor = ConsoleColor.Gray;
            ConsoleColor row2FgColor = ConsoleColor.DarkBlue;

            // Define colors for overdue items
            ConsoleColor overdueBgColor = ConsoleColor.DarkRed; // A dark red background
            ConsoleColor overdueFgColor = ConsoleColor.White;   // White text on dark red for contrast

            //row counter. 
            int rowIndex = 0;


            if (projects == null || projects.Count == 0) // also checks if projects list is empty
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No projects to display.".PadRight(totalWidth));
                Console.ResetColor();
            }
            else
            {
                foreach (var project in projects) // Print each project row
                {
                    project.UpdateProjectStatusBasedOnTasks();

                    if (HelperMethods.IsProjectOverDue(project))
                    {

                        Console.BackgroundColor = overdueBgColor;
                        Console.ForegroundColor = overdueFgColor;

                    }
                    else if (rowIndex % 2 == 0)
                    {
                        Console.BackgroundColor = row1BgColor;
                        Console.ForegroundColor = row1FgColor;
                    }
                    else
                    {
                        Console.BackgroundColor = row2BgColor;
                        Console.ForegroundColor = row2FgColor;
                    }

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
                        project.UpdateProjectStatusBasedOnTasks();
                        completedTasks = project.ProjectTasks.Count(t => t.TaskStatus == "DONE");
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
                    rowIndex++;
                }
            }
            Console.BackgroundColor = originalBackColor;
            Console.ForegroundColor = originalForColor;
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

            //stores original console colors.
            ConsoleColor originalForColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;

            // Defines the alternating colors for the rows in a table.
            ConsoleColor row1BgColor = ConsoleColor.Black;
            ConsoleColor row1FgColor = ConsoleColor.White;

            ConsoleColor row2BgColor = ConsoleColor.Gray;
            ConsoleColor row2FgColor = ConsoleColor.DarkBlue;

            // Define colors for overdue items
            ConsoleColor overdueBgColor = ConsoleColor.DarkRed; // A dark red background
            ConsoleColor overdueFgColor = ConsoleColor.White;   // White text on dark red for contrast

            //row counter. 
            int rowIndex = 0;

            if (projects != null)
            {
                foreach (var project in projects)
                {
                    project.UpdateProjectStatusBasedOnTasks();

                    if (project.ProjectTasks != null && project.ProjectTasks.Count > 0) // also checks if tasks list is empty
                    {
                        anyTasks = true;
                        foreach (var task in project.ProjectTasks) // Print each task row within project
                        {
                            if (HelperMethods.IsTaskOverDue(task))
                            {

                                Console.BackgroundColor = overdueBgColor;
                                Console.ForegroundColor = overdueFgColor;

                            }
                            else if (rowIndex % 2 == 0)
                            {
                                Console.BackgroundColor = row1BgColor;
                                Console.ForegroundColor = row1FgColor;
                            }
                            else
                            {
                                Console.BackgroundColor = row2BgColor;
                                Console.ForegroundColor = row2FgColor;
                            }
                            Console.WriteLine(
                                project.ProjectID.PadRight(projectIdWidth) + " | " +
                                project.ProjectName.PadRight(projectNameWidth) + " | " +
                                task.TaskID.PadRight(taskIdWidth) + " | " +
                                task.TaskName.PadRight(taskNameWidth) + " | " +
                                task.TaskDueDate.ToShortDateString().PadRight(taskDueDateWidth) + " | " +
                                task.TaskStatus.PadRight(taskStatusWidth)
                            );

                            rowIndex++; // adds to the row counter for the next row
                        }
                    }
                }
            }
            Console.BackgroundColor = originalBackColor;
            Console.ForegroundColor = originalForColor;
            if (!anyTasks) // If no tasks across all projects
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No tasks from any project to display.".PadRight(totalWidth));
                Console.ResetColor();
            }
            Console.WriteLine(new string('-', totalWidth));
            Console.WriteLine(); // Add an extra line for spacing
        }

        public static bool IsTaskOverDue(Task task)
        {
            return task.TaskDueDate.Date < DateTime.Now.Date && task.TaskStatus.ToUpper() != "DONE";
        }

        public static bool IsProjectOverDue(Project project)
        {
            return project.ProjectDueDate.Date < DateTime.Now.Date && project.ProjectStatus.ToUpper() != "DONE";
        }

    }
}
    

