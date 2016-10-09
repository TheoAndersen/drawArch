using drawArch.Render;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawArch
{
    class Program
    {
        static void Main(string[] args)
        { 
            var csProjPaths = CsProjParser.FindAllProjects(@"c:/dev/edx/"); //Directory.GetCurrentDirectory()

            var projects = new CsProjParser(csProjPaths).Parse((path) => { return File.ReadLines(path); });

            foreach (var project in projects.Projects)
            {
                Console.WriteLine(project.Name);

                foreach (var referencedProject in project.References)
                {
                    Console.WriteLine(" - " + referencedProject.Name);
                }
            }


            var html = new VisRender(projects).Render();

            File.WriteAllText(@"c:/output.html", html);

            Console.ReadKey();
        }
    }
}
