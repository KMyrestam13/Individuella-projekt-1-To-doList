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

                Console.WriteLine($"\u2713 Projects saved to '{DataFileName}'. ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving projects: {ex.Message}");
                Console.WriteLine("Please check if you can access the file, or if your harddrive space is full.");
            }
        }

        public List<Project> LoadProjects()
        {
            if (!File.Exists(DataFileName))
            {
                Console.WriteLine($"No data found in '{DataFileName}'. Starting with an empty list of projects."); 
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
                Console.WriteLine($"\nError reading project data (JSON format issue): {ex.Message}"); 
                Console.WriteLine("Starting with an empty list of projects to prevent further issues."); 
                return new List<Project>();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"\nError accessing file '{DataFileName}': {ex.Message}");
                Console.WriteLine("Starting with an empty list of projects. Please check file permissions."); 
                return new List<Project>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn unexpected error occurred while loading projects: {ex.Message}");
                Console.WriteLine("Starting with an empty list of projects."); 
                return new List<Project>(); 
            }
        }
    }
}
