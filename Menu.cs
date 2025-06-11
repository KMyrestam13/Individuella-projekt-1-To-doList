using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individuella_projekt_1_To_doList
{
    public static class Menu
    {
        // This method displays the main menu options
        public static void DisplayMainMenu()
        {
            HelperMethods.ClearConsole();
            Console.WriteLine("--- TO DO LIST APP ---");
            Console.WriteLine("1. Add New Project");
            Console.WriteLine("2. View All Projects");
            Console.WriteLine("3. Add Task to Project");
            Console.WriteLine("4. Update Project/Tasks");
            Console.WriteLine("5. Delete Project");
            Console.WriteLine("6. View All Tasks (Across ALL Projects)");
            Console.WriteLine("7. Save Data");
            Console.WriteLine("8. Load Data");
            Console.WriteLine("9. Exit");
            Console.WriteLine("-------------------------------------");
        }

        // this is what happens when you run the main menu
        public static void RunMainMenu(ListManager listManager, FileHandling fileHandler)
        {
            bool running = true;
            while (running)
            {
                DisplayMainMenu();
                int choice = HelperMethods.GetIntInput("Please enter your choice: ");

                switch (choice)
                {
                    case 1:
                        AddNewProject(listManager);
                        break;
                    case 2:
                        listManager.ViewProjectList();
                        break;
                    case 3:
                        AddTaskToProject(listManager);
                        break;
                    case 4:
                        listManager.UpdateProject();
                        break;
                    case 5:
                        listManager.DeleteProject();
                        break;
                    case 6:
                        HelperMethods.ClearConsole(); // Clear console before displaying all tasks
                        Console.WriteLine(" --- All Tasks Across ALL Projects ---");
                        HelperMethods.DisplayAllTasksAcrossProjectsInTable(listManager.Projects);
                        break;
                    case 7:
                        SaveData(listManager, fileHandler);
                        break;
                    case 8:
                        LoadData(listManager, fileHandler);
                        break;
                    case 9:
                        Console.WriteLine("Exiting application. Goodbye!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 9.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                }
            }
        }

        // this allows the user to add a new project
        private static void AddNewProject(ListManager listManager)
        {
            HelperMethods.ClearConsole();
            Console.WriteLine("--- Add New Project ---");
            string projectId = listManager.GetUniqueTaskId();
            string projectName = HelperMethods.GetStringInput("Enter project name: ");
            DateTime projectDueDate = HelperMethods.GetDateInput("Enter project due date (YYYY-MM-DD): ");

            Project newProject = new Project(projectId, projectName, projectDueDate);
            listManager.AddProject(newProject);
        }

        // this allows the user to add a task to a project
        private static void AddTaskToProject(ListManager listManager)
        {
            HelperMethods.ClearConsole();
            Console.WriteLine("--- Add Task to Existing Project ---");

            if (listManager.Projects == null || listManager.Projects.Count == 0)
            {
                Console.WriteLine("No projects available to add tasks to. Please add a project first.");
                return;
            }
            // this will give the user a list of projects and prompt them to say which one they want to add a task to.
            listManager.ViewProjectList();
            string projectId = HelperMethods.GetStringInput("Enter the ID of the project to add a task to: ");

            Project selectedProject = listManager.GetProjectById(projectId);

            if (selectedProject == null)
            {
                Console.WriteLine("Project not found with the given ID. Returning to main menu.");
                return;
            }

            Console.WriteLine($"--- Adding Task to Project: '{selectedProject.ProjectName}' (ID: {selectedProject.ProjectID}) ---");

            string taskId = HelperMethods.GenerateRandomId(5);
            string taskName = HelperMethods.GetStringInput("Enter task name: ");
            DateTime taskDueDate = HelperMethods.GetDateInput("Enter task due date (YYYY-MM-DD): ");
            string taskStatus = HelperMethods.GetStringInput("Enter task status (e.g., To Do, In Progress, Done): ").ToUpper();

            Task newTask = new Task(taskId, taskName, taskDueDate, selectedProject.ProjectName, taskStatus);

            selectedProject.AddTask(newTask);
            Console.WriteLine("Task added successfully!");
        }

        private static void SaveData(ListManager listManager, FileHandling fileHandler)
        {
            HelperMethods.ClearConsole();
            Console.WriteLine("--- Save Data ---");
            fileHandler.SaveProjects(listManager.Projects);
        }

        private static void LoadData(ListManager listManager, FileHandling fileHandler)
        {
            HelperMethods.ClearConsole();
            Console.WriteLine("--- Load Data ---");
            List<Project> loadedProjects = fileHandler.LoadProjects();
            listManager.Projects = loadedProjects;

            if (listManager.Projects != null) // this ensures that the completion percentage is based on the most recent data.
            {
                foreach (var project in listManager.Projects)
                {
                    project.UpdateProjectStatusBasedOnTasks();
                }
            }
            Console.WriteLine("Projects loaded successfully.");
        }

    }
}