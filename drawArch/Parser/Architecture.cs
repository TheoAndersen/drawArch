using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawArch.Parser
{
    public class Architecture
    {
        public IList<Project> Projects { get; }
        private int currentId = 0;

        public Architecture()
        {
            this.Projects = new List<Project>();
        }

        public void AddProject(Project project)
        {
            if(project.Name.ToLower().Contains("test"))
            {
                return;
            }

            currentId++;
            project.Id = currentId;
            Projects.Add(project);
        }

        internal Project getRefProject(FileInfo refProjectPath)
        {
            if(this.Projects.Count(p => p.Path.FullName == refProjectPath.FullName) == 1)
            {
                return this.Projects.Single(p => p.Path.FullName == refProjectPath.FullName);
            }

            throw new ArgumentException("Cannot find the real project " + refProjectPath);
        }
    }
}
