using System;
using System.Collections.Generic;
using System.Text;

namespace Individuella_projekt_1_To_doList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // ensures that my unicode for the bar chart will display properly

            FileHandling myFileHandler = new FileHandling(); // checks if there is a saved file and uses it if there is.

            List<Project> initialProjects = myFileHandler.LoadProjects();

            ListManager myProjectManager = new ListManager(initialProjects);

            Menu.RunMainMenu(myProjectManager, myFileHandler); // calls the main menu and runs it.

            Console.WriteLine("Application ended. Press any key to close.");// good bye message after the user exits the program
            Console.ReadKey();
        }
    }
}