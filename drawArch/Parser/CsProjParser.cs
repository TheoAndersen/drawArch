using drawArch.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace drawArch
{
    public class CsProjParser
    {
        private string[] csProjPaths;
        private Architecture architecture;

        public CsProjParser(string[] csProjPaths)
        {
            this.csProjPaths = csProjPaths;
            this.architecture = new Architecture();
        }

        public Architecture Parse(Func<string, IEnumerable<string>> findFileContents)
        {
            foreach (var csProjPath in csProjPaths)
            {
                var project = new Project(csProjPath);

                architecture.AddProject(project);
            }

            foreach (var project in architecture.Projects)
            {
                foreach (var refProject in Parse(project, findFileContents(project.Path.FullName)))
                {
                    project.AddReferencedProject(refProject);
                }
            }

            return architecture;
        }

        private IList<Project> Parse(Project thisProject, IEnumerable<string> contents)
        {
            List<Project> projects = new List<Project>();    

            foreach (var referenceXmlString in contents.Where(l => l.Contains("<ProjectReference ")))
            {
                var regex = new Regex(".*\"(.*)\".*");
                var includePath = regex.Match(referenceXmlString).Groups[1].Value;

                var thisFileInfo = thisProject.Path;
                var refFileInfo = new FileInfo(thisFileInfo.Directory + "\\" + includePath);
                try
                {
                    projects.Add(architecture.getRefProject(refFileInfo));
                }
                catch(Exception e)
                {
                    Console.WriteLine("Cannot seem to find referenced project (" + refFileInfo.FullName + "), from csproj (" + thisProject.Path + ")");
                }
            }

            return projects;
        }

        public static string[] FindAllProjects(string path)
        {
            return Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
        }
    }
}
