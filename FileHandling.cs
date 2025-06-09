using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Individuella_projekt_1_To_doList
{
    //this was made with assistance of AI, I checked several links to learn more about how this works, as I felt that this was the best and easiest method for saving
    //to a file. This was done before I noticed that you posted an example for how to do this.
    public class FileHandling
    {
        // constant for data file name
        private const string DataFileName = "projects_data.json";

        public void SaveProjects(List<Project> projects)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(projects, options);

                File.WriteAllText(DataFileName, jsonString);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\u2713 Projects saved to '{DataFileName}'. ");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error saving projects: {ex.Message}");
                Console.WriteLine("Please check if you can access the file, or if your harddrive space is full.");
                Console.ResetColor();
            }
        }

        public List<Project> LoadProjects()
        {
            if (!File.Exists(DataFileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No data found in '{DataFileName}'. Starting with an empty list of projects.");
                Console.ResetColor();
                return new List<Project>();
            }

            try
            {
                string jsonString = File.ReadAllText(DataFileName);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<Project>? loadedProjects = JsonSerializer.Deserialize<List<Project>>(jsonString, options); 
                return loadedProjects ?? new List<Project>();
            }
            catch (JsonException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error reading project data (JSON format issue): {ex.Message}"); 
                Console.WriteLine("Starting with an empty list of projects to prevent further issues.");
                Console.ResetColor();
                return new List<Project>();
            }
            catch (IOException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error accessing file '{DataFileName}': {ex.Message}");
                Console.WriteLine("Starting with an empty list of projects. Please check file permissions."); 
                Console.ResetColor();
                return new List<Project>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An unexpected error occurred while loading projects: {ex.Message}");
                Console.WriteLine("Starting with an empty list of projects."); 
                Console.ResetColor();
                return new List<Project>(); 
            }
        }
    }
}
