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
        public int currentDatabaseId = 1010;
        public IList<Database> Databases { get; private set; }
        public Architecture()
        {
            this.Projects = new List<Project>();
            this.Databases = new List<Database>();
        }

        public Database AddDatabase(string dbName, string wholedbString)
        {
            if (Databases.Count(db => db.Name.ToLower() == dbName.ToLower()) == 0)
            {
                currentDatabaseId++;

                var db = new Database()
                {
                    Id = currentDatabaseId,
                    Name = dbName,
                    ToolTip = wholedbString
                };

                this.Databases.Add(db);
                return db;
            }

            return Databases.First(db => db.Name.ToLower() == dbName.ToLower());
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
