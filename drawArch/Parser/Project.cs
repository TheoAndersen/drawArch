using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace drawArch.Parser
{
    public class Project
    {
        public enum ProjectType
        {
            Library,
            Console,
            WindowsExe
        }

        public ProjectType Type { get; set; }

        public Project(string path)
        {
            this.Name = ParseNameFromCsProjPath(path);
            this.Path = new FileInfo(path);
            this.References = new List<Project>();
            this.Services = new List<string>();
            this.Endpoints = new List<string>();
            this.Databases = new List<Database>();
        }

        public string Name { get; private set; }

        public IList<Project> References { get; private set; }
        public FileInfo Path { get; private set; }
        public int Id { get; internal set; }
        public IList<string> Services { get; set; }
        public IList<string> Endpoints { get; set; }
        public IList<Database> Databases { get; set; }

        public void AddReferencedProject(Project project)
        {
            References.Add(project);
        }

        private static string ParseNameFromCsProjPath(string csProjPath)
        {
            var regex = new Regex(".*\\\\(.*)\\.csproj"); //<ProjectReference Include="..\Utilities\Utilities.csproj">
            return regex.Match(csProjPath).Groups[1].Value;
        }
    }
}
