using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individuella_projekt_1_To_doList
{
    public class ListManager
    {
        // creates list of project objects
        public List<Project> Projects { get; set; } // stores the project objects.

        //constructor for the project list
        public ListManager(List<Project> projects)
        {
            Projects = projects ?? new List<Project>();
            foreach (var project in Projects)
            {
                project.UpdateProjectStatusBasedOnTasks();
            }
        }

        // gets ID for project and for tasks
        public string GetUniqueTaskId()
        {
            string newId;
            bool isUnique;
            do
            {
                newId = HelperMethods.GenerateRandomId(5); // Generate a 5-character ID
                isUnique = true;

                //verify that the ID doesnt already exist.
                foreach (var project in Projects)
                {
                    if (project.ProjectTasks != null)
                    {
                        foreach (var task in project.ProjectTasks)
                        {
                            if (task.TaskID == newId)
                            {
                                isUnique = false;
                                break; //breaks because the ID is already in use
                            }
                        }
                    }
                    if (!isUnique) break;
                }
            } while (isUnique == false); // Keep generating until a new ID is found

            return newId;
        }

        // adds a project
        public void AddProject(Project project)
        {
            Projects.Add(project);
            Console.WriteLine($"Your project: '{project.ProjectName}' has been added to the list."); // confirms that the project was added
        }

        // lets user see a list of projects
        public void ViewProjectList()
        {
            HelperMethods.ClearConsole();
            if (Projects.Count == 0) // checks if the list is empty
            {
                Console.WriteLine("There are currently no Projects to display.");
                return;
            }

            Console.WriteLine(" --- Current Projects ---");

            // Prompt user for sorting preference
            Console.WriteLine("Sort projects by:");
            Console.WriteLine("1. Project Name (Alphabetical)");
            Console.WriteLine("2. Due Date (Ascending)");
            int sortChoice = HelperMethods.GetIntInputNoRetry("Enter your sorting choice: ");

            
            List<Project> projectsToDisplay = new List<Project>(Projects); 
            switch (sortChoice)
            {
                case 1:
                    // Sort by Project Name (Alphabetical)
                    projectsToDisplay.Sort((p1, p2) => string.Compare(p1.ProjectName.ToLowerInvariant(), p2.ProjectName.ToLowerInvariant()));
                    Console.WriteLine("--- Projects Sorted by Project Name ---");
                    break;
                case 2:
                    //sort projects by due date
                    projectsToDisplay = projectsToDisplay.OrderBy(p => p.ProjectDueDate).ToList();
                    Console.WriteLine("--- Projects Sorted by Due Date ---");
                    break;
                default:
                    // Default sort: Project Name then Due Date using direct comparison
                    Console.WriteLine("Invalid choice. Displaying projects sorted by Name then Due Date by default.");
                    projectsToDisplay = projectsToDisplay.OrderBy(p => p.ProjectName).ThenBy(a => a.ProjectDueDate).ToList();
                    
                    break;
            }

            // Display the sorted list
            HelperMethods.DisplayProjectsInTable(projectsToDisplay);
        }

        // lets user update a project
        public void UpdateProject()
        {
            HelperMethods.ClearConsole();
            if (Projects == null || Projects.Count == 0)
            {
                Console.WriteLine("No projects available to update. Please add a project first.");
                return;
            }

            // uses the helper method to get user input
            ViewProjectList(); // Show projects for selection
            string projectIDToUpdate = HelperMethods.GetStringInput("Please enter the Project ID:");

            // looks for the project in the project list
            Project projectToUpdate = Projects.FirstOrDefault(pToUpdate => pToUpdate.ProjectID.ToLowerInvariant() == projectIDToUpdate.ToLowerInvariant());

            // validate that product is in list
            if (projectToUpdate == null)
            {
                Console.WriteLine($"Project with ID '{projectIDToUpdate}' not found.");
                return;
            }

            // user chooses what they want to update.
            while (true)
            {
                HelperMethods.ClearConsole();
                Console.WriteLine("--- Update Project / Task ---");
                Console.WriteLine($"Project selected: {projectToUpdate.ProjectName} (ID: {projectToUpdate.ProjectID})");
                Console.WriteLine("What would you like to update?");
                Console.WriteLine("1. Project Name");
                Console.WriteLine("2. Project Due Date");
                Console.WriteLine("3. Manage Tasks within this Project (Add/Update/Delete)");
                Console.WriteLine("4. Done Updating");

                int userchoice = HelperMethods.GetIntInput("Please enter your choice:");

                switch (userchoice)
                {
                    case 1:
                        // checks for and updates project name
                        Console.WriteLine($"Updating name for {projectToUpdate.ProjectName}.");
                        Console.WriteLine();
                        string newName = HelperMethods.GetStringInput($"Current Name {projectToUpdate.ProjectName}. Please enter new name: ");
                        projectToUpdate.ProjectName = newName; // Assign the new name
                        Console.WriteLine("Project name updated.");
                        break;

                    case 2:
                        // checks for and updates Project Due Date
                        Console.WriteLine($"Updating Due Date for {projectToUpdate.ProjectName}.");
                        DateTime newDueDate = HelperMethods.GetDateInput($"Current Due Date: {projectToUpdate.ProjectDueDate.ToShortDateString()}. Please enter new due date (YYYY-MM-DD): ");
                        projectToUpdate.ProjectDueDate = newDueDate;
                        Console.WriteLine("Project Due Date updated.");
                        break;

                    case 3:
                        ManageProjectTasks(projectToUpdate);
                        break;

                    case 4:
                        // user wants to stop updating
                        Console.WriteLine("Finished updating project details.");
                        return;

                    default:
                        // deals with invalid choices
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                        break;
                }
            }
        }
        // allows the user to update a task from within the project interface
        private void ManageProjectTasks(Project project)
        {
            bool managingTasks = true;
            while (managingTasks)
            {
                HelperMethods.ClearConsole();
                Console.WriteLine($"--- Manage Tasks for Project: '{project.ProjectName}' ---");
                project.DisplayProjectInfo();
                project.ListAllProjectTasks();

                Console.WriteLine("Task Management Options:");
                Console.WriteLine("1. Add New Task to Project");
                Console.WriteLine("2. Update Existing Task");
                Console.WriteLine("3. Delete Task");
                Console.WriteLine("4. Return to Project Update Menu");

                int choice = HelperMethods.GetIntInput("Enter your choice: ");

                switch (choice)
                {
                    case 1:
                        string newTaskId = GetUniqueTaskId();
                        string taskName = HelperMethods.GetStringInput("Enter task name: ");
                        DateTime taskDueDate = HelperMethods.GetDateInput("Enter task due date (YYYY-MM-DD): ");
                        string taskStatus = HelperMethods.GetStringInput("Enter task status (e.g., To Do, In Progress, Done): ");
                        Task newTask = new Task(newTaskId, taskName, taskDueDate, project.ProjectName, taskStatus);
                        project.AddTask(newTask);
                        Console.WriteLine($"Task '{newTask.TaskName}' (ID: {newTask.TaskID}) added to '{project.ProjectName}'.");
                        break;
                    case 2:
                        UpdateSpecificTaskInProject(project);
                        break;
                    case 3:
                        DeleteTask(project);
                        break;
                    case 4:
                        managingTasks = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                project.UpdateProjectStatusBasedOnTasks();
                if (managingTasks)
                {
                    Console.WriteLine("Press any key to continue managing tasks...");
                    Console.ReadKey();
                }
            }
        }
        // updates tasks in a specific project. First it lists all the tasks in that project then it allows the user to choose one.
        private void UpdateSpecificTaskInProject(Project project)
        {
            if (project.ProjectTasks == null || project.ProjectTasks.Count == 0) 
            {
                Console.WriteLine("No tasks in this project to update.");
                return;
            }

            project.ListAllProjectTasks();
            string taskIdToUpdate = HelperMethods.GetStringInput("Enter the ID of the task to update: ");
            Task taskToUpdate = project.ProjectTasks.FirstOrDefault(t => t.TaskID.ToLowerInvariant() == taskIdToUpdate.ToLowerInvariant());

            if (taskToUpdate == null)
            {
                Console.WriteLine("Task not found in this project.");
                return;
            }

            bool updatingTask = true;
            while (updatingTask)
            {
                Console.WriteLine($"--- Updating Task: '{taskToUpdate.TaskName}' (ID: {taskToUpdate.TaskID}) ---");
                Console.WriteLine("1. Update Task Name");
                Console.WriteLine("2. Update Task Due Date");
                Console.WriteLine("3. Update Task Status");
                Console.WriteLine("4. Back to Task Management Menu");
                int choice = HelperMethods.GetIntInput("Enter your choice: ");

                switch (choice)
                {
                    case 1:
                        string newName = HelperMethods.GetStringInput($"Enter new name for task '{taskToUpdate.TaskName}': ");
                        taskToUpdate.TaskName = newName;
                        Console.WriteLine("Task name updated.");
                        break;
                    case 2:
                        DateTime newDueDate = HelperMethods.GetDateInput($"Enter new due date for task '{taskToUpdate.TaskName}' (YYYY-MM-DD): ");
                        taskToUpdate.TaskDueDate = newDueDate;
                        Console.WriteLine("Task due date updated.");
                        break;
                    case 3:
                        string newStatus = HelperMethods.GetStringInput($"Enter new status for task '{taskToUpdate.TaskName}' (e.g., To Do, In Progress, Done): ");
                        if (newStatus.Equals("To Do", StringComparison.OrdinalIgnoreCase) ||
                            newStatus.Equals("In Progress", StringComparison.OrdinalIgnoreCase) ||
                            newStatus.Equals("Done", StringComparison.OrdinalIgnoreCase))
                        {
                            taskToUpdate.TaskStatus = newStatus;
                            Console.WriteLine("Task status updated.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid status. Please enter 'To Do', 'In Progress', or 'Done'.");
                        }
                        break;
                    case 4:
                        updatingTask = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                if (updatingTask)
                {
                    Console.WriteLine("Press any key to continue updating this task...");
                    Console.ReadKey();
                }
            }
        }


        // allows a user to delete a project
        public void DeleteProject()
        {
            // clears console of clutter
            HelperMethods.ClearConsole();
            Console.WriteLine(" --- Delete Project ---");

            // uses the helper method to get user input
            string projectIDToDelete = HelperMethods.GetStringInput("Please enter the Project ID:");

            // looks for the product in the product list
            Project projectToDelete = Projects.FirstOrDefault(pToDelete => pToDelete.ProjectID.ToLowerInvariant() == projectIDToDelete.ToLowerInvariant());

            // validate that project is in list
            if (projectToDelete == null)
            {
                Console.WriteLine($"Project with ID '{projectIDToDelete}' not found.");
                return;
            }
            // confirms that the user wants to delete the project
            Console.WriteLine($"Found Project {projectToDelete.ProjectName}(ID: {projectToDelete.ProjectID})");
            string confirmation = HelperMethods.GetStringInput("Are you sure you want to delete this project (yes/no):").ToLower();

            if (confirmation == "yes")
            {
                Projects.Remove(projectToDelete);

                Console.WriteLine($"Project '{projectToDelete.ProjectName}' (ID: {projectToDelete.ProjectID}) has been deleted.");
            }

            else
            {
                Console.WriteLine($" Deletion cancelled. Project '{projectToDelete.ProjectName}' (ID: {projectToDelete.ProjectID}) was not deleted.");
            }
        }

        // allows a user to delete a task, either directly or within a project context
        public void DeleteTask(Project projectContext = null)
        {
            // clears console of clutter
            HelperMethods.ClearConsole();
            Console.WriteLine(" --- Delete Task ---");

            Project currentProject;
            string projectIDToFind;

            // Determine if a project context was provided
            if (projectContext != null)
            {
                currentProject = projectContext;
                Console.WriteLine($"Deleting task from Project: {currentProject.ProjectName} (ID: {currentProject.ProjectID})");
            }
            else // No project context provided, ask user to find the project
            {
                // uses the helper method to get user input for the project first
                projectIDToFind = HelperMethods.GetStringInput("Please enter the Project ID that contains the task: ");

                // looks for the project in the project list
                currentProject = Projects.FirstOrDefault(p => p.ProjectID.ToLowerInvariant() == projectIDToFind.ToLowerInvariant());

                // validate that project is in list
                if (currentProject == null)
                {
                    Console.WriteLine($"Project with ID '{projectIDToFind}' not found. Cannot delete task.");
                    return;
                }
            }

            // Display tasks in the found project so the user knows what to delete
            Console.WriteLine($"--- Tasks in Project '{currentProject.ProjectName}' (ID: {currentProject.ProjectID}) ---");
            currentProject.ListAllProjectTasks();

            // Uses the helper method to get user input for the task to delete
            string taskIDToDelete = HelperMethods.GetStringInput("Please enter the Task ID to delete: ");

            // Looks for the task within the selected project's task list
            Task taskToDelete = null;
            if (currentProject.ProjectTasks != null && currentProject.ProjectTasks.Count > 0) // Replaced .Any() with .Count > 0
            {
                
                taskToDelete = currentProject.ProjectTasks.FirstOrDefault(task => task.TaskID.ToLowerInvariant() == taskIDToDelete.ToLowerInvariant());
            }

            // Validate that task is found within the project
            if (taskToDelete == null)
            {
                Console.WriteLine($"Task with ID '{taskIDToDelete}' not found in project '{currentProject.ProjectName}'.");
                return;
            }

            // Confirms that the user wants to delete the task
            Console.WriteLine($"Found Task '{taskToDelete.TaskName}' (ID: {taskToDelete.TaskID}) in Project '{currentProject.ProjectName}'.");
            string confirmation = HelperMethods.GetStringInput("Are you sure you want to delete this task (yes/no):").ToLower();

            if (confirmation == "yes")
            {
                currentProject.RemoveTask(taskToDelete); // Call the RemoveTask method on the project
                currentProject.UpdateProjectStatusBasedOnTasks(); // Update project status after task deletion

                Console.WriteLine($"Task '{taskToDelete.TaskName}' (ID: {taskToDelete.TaskID}) has been deleted from project '{currentProject.ProjectName}'.");
            }
            else
            {
                Console.WriteLine($"Deletion cancelled. Task '{taskToDelete.TaskName}' (ID: {taskToDelete.TaskID}) was not deleted.");
            }
        }

        public Project GetProjectById(string projectId)
        {
            
            return Projects.FirstOrDefault(p => p.ProjectID.ToLowerInvariant() == projectId.ToLowerInvariant());
        }
    }
}