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

        public static Func<string, IEnumerable<string>> ReadLinesFromFile =
                (path) => 
                {
                    return File.ReadLines(path);
                };

        public Architecture Parse(Func<string, IEnumerable<string>> findFileContents)
        {
            AddProjects();

            ParseProjectFile(findFileContents);

            return architecture;
        }

        private void AddProjects()
        {
            foreach (var csProjPath in csProjPaths)
            {
                var project = new Project(csProjPath);

                architecture.AddProject(project);
            }
        }

        private void ParseProjectFile(Func<string, IEnumerable<string>> findFileContents)
        {
            foreach (var project in architecture.Projects)
            {
                var fileContents = findFileContents(project.Path.FullName);

                FindReferencedProjects(fileContents, project);

                DetermineProjectType(fileContents, project);

                ParseConfigFile(project, findFileContents);
            }
        }

        private void ParseConfigFile(Project project, Func<string, IEnumerable<string>> findFileContents)
        {
            if (File.Exists(project.Path.DirectoryName + "/Web.Config"))
            {
                var configContents = findFileContents(project.Path.DirectoryName + "/Web.Config");

                foreach (var servString in configContents.Where(c => c.ToLower().Contains("<service ")))
                {
                    var regex = new Regex(".*name=\"([^\"]+)\"?.*?");
                    var name = regex.Match(servString).Groups[1].Value;

                    project.Services.Add(name);
                }
            }
            
        }

        private void DetermineProjectType(IEnumerable<string> fileContents, Project project)
        {
            if (fileContents.Count(c => c.ToLower().Contains("<outputtype>exe</outputtype>")) > 0)
            {
                project.Type = Project.ProjectType.Console;
                return;
            }

            if (fileContents.Count(c => c.ToLower().Contains("<outputtype>winexe</outputtype>")) > 0)
            {
                project.Type = Project.ProjectType.WindowsExe;
                return;
            }
        }

        private void FindReferencedProjects(IEnumerable<string> fileContents, Project project)
        {
            foreach (var refProject in Parse(project, fileContents))
            {
                project.AddReferencedProject(refProject);
            }
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
