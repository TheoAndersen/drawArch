using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Test
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void CanFetchAllCsProjFilesRecursively()
        {
            var files = FindAllProjects();

            Assert.IsTrue(files.Count() > 0);
        }

        class Project
        {
            public string Name { get; private set; }

            public IList<Project> References { get; private set; }

            public Project(string Name)
            {
                this.References = new List<Project>();
                this.Name = Name;
            }

            public void AddReferencedProject(Project project)
            {
                References.ToList().Add(project);
            }
        }

        public class ProjectParser
        {

        }

        [TestMethod]
        public void CanFindReferencesToOtherProjectsInPRojectFile()
        {
            var proj = @"C:\dev\edx\EDX\EDXServer\EDXServer.csproj";

            var contents = File.ReadLines(proj);

            var projects = new List<Project>();

            var project = new Project("EDXServer");

            foreach (var referenceXmlString in contents.Where(l => l.Contains("<ProjectReference ")))
            {
                var regex = new Regex(".*\\\\(.*)\\.csproj"); //<ProjectReference Include="..\Utilities\Utilities.csproj">
                projects.Add(new Project(regex.Match(referenceXmlString).Groups[1].Value));
            }

            Assert.AreEqual(3, projects.Count());
            Assert.AreEqual("ClientAPIClasses", projects[0].Name);
            Assert.AreEqual("CustomizeAPI", projects[1].Name);
            Assert.AreEqual("Utilities", projects[2].Name);
        }

        private static string[] FindAllProjects()
        {
            return Directory.GetFiles("c:/dev/edx/", "*.csproj", SearchOption.AllDirectories);
        }
    }
}
