using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using drawArch;
using drawArch.Parser;

namespace Test
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void CanParseCsProjFileNames()
        {
            var csProjs = new string[] 
                {
                    @"C:\dev\edx\EDX\EDXServer\EDXServer.csproj",
                    @"C:\dev\edx\EDX\OtherTestProject\OtherTestProject.csproj"
                };

            var projects = new List<Project>();

            foreach (var projectPath in csProjs)
            {
                projects.Add(new Project(projectPath));
            }

            Assert.AreEqual(2, projects.Count());
            Assert.AreEqual("EDXServer", projects[0].Name);
            Assert.AreEqual("OtherTestProject", projects[1].Name);
        }

        //[TestMethod]
        //public void CanFindReferencesToOtherProjectsInProjectFile()
        //{
        //    var proj = @"C:\dev\edx\EDX\EDXServer\EDXServer.csproj";

        //    var contents = File.ReadLines(proj);

            
        //    Assert.AreEqual(3, project.References.Count());
        //    Assert.AreEqual("ClientAPIClasses", project.References[0].Name);
        //    Assert.AreEqual("CustomizeAPI", project.References[1].Name);
        //    Assert.AreEqual("Utilities", project.References[2].Name);
        //}

        [TestMethod]
        public void CanFetchAllCsProjFilesRecursively()
        {
            var files = CsProjParser.FindAllProjects("c:/dev/edx/");

            Assert.IsTrue(files.Count() > 0);
        }
    }
}
