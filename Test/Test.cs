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
        /// Yeah, I'm sorry i know - all the tests refer to a project i had on my local
        /// machine while creating them.. tests should always create theyr own data
        /// ... and these will.. eventually...

        [TestMethod]
        public void CanDetectConnectionStringsForDatabaseInConfigurations()
        {
            var csProjs = new string[]
                {
                    @"C:\dev\edx\EDX\ClientAPI\ClientAPI.csproj",
                };

            var architecture = new CsProjParser(csProjs).Parse(CsProjParser.ReadLinesFromFile);
            var projects = architecture.Projects;

            Assert.AreEqual(1, projects.Count());
            Assert.AreEqual(2, projects[0].Databases.Count);
            Assert.AreEqual("{1}", projects[0].Databases[0].Name);
        }

        [TestMethod]
        public void CanDetectProjectsWithReferToEndpoints()
        {
            var csProjs = new string[]
                {
                    @"C:\dev\edx\EDX\GUI Shell\GUI Shell.csproj",
                };

            var architecture = new CsProjParser(csProjs).Parse(CsProjParser.ReadLinesFromFile);
            var projects = architecture.Projects;

            Assert.AreEqual(1, projects.Count());
            Assert.AreEqual("GUI Shell", projects[0].Name);
            Assert.AreEqual(7, projects[0].Endpoints.Count);
            Assert.AreEqual("wsSecurity", projects[0].Endpoints[0]);
        }

        [TestMethod]
        public void CanDetectProjectsThatServeWCFServices()
        {
            var csProjs = new string[]
                {
                    @"C:\dev\edx\EDX\ClientAPI\ClientAPI.csproj",
                };

            var architecture = new CsProjParser(csProjs).Parse(CsProjParser.ReadLinesFromFile);
            var projects = architecture.Projects;

            Assert.AreEqual(1, projects.Count());
            Assert.AreEqual("ClientAPI", projects[0].Name);
            Assert.AreEqual(7, projects[0].Services.Count());
            Assert.AreEqual("ClientAPI.wsReport", projects[0].Services[0]);
            Assert.AreEqual(0, projects[0].Endpoints.Count, "The endpoint tag for each service, shouln't be confused with a real endpoint");
        }

        [TestMethod]
        public void CanDetectIfOutputTypeOfProjectIsConsole()
        {
            var csProjs = new string[]
                {
                    @"C:\dev\edx\EDX\EdxServerConsoleRecorderRunner\EdxServerConsoleRecorderRunner.csproj",
                    @"C:\dev\edx\EDX\EDXServer\EDXServer.csproj",
                    @"c:\dev\edx\edx\ExecutionEngineService\ExecutionEngineService.csproj"
                };

            var architecture = new CsProjParser(csProjs).Parse(CsProjParser.ReadLinesFromFile);
            var projects = architecture.Projects;

            Assert.AreEqual(3, projects.Count());
            Assert.AreEqual("EdxServerConsoleRecorderRunner", projects[0].Name);
            Assert.AreEqual(Project.ProjectType.Library, projects[1].Type);
            Assert.AreEqual(Project.ProjectType.Console, projects[0].Type);
            Assert.AreEqual(Project.ProjectType.WindowsExe, projects[2].Type);


        }

        [TestMethod]
        public void CanParseCsProjFileNames()
        {
            var csProjs = new string[] 
                {
                    @"C:\dev\edx\EDX\EDXServer\EDXServer.csproj",
                    @"C:\dev\edx\EDX\GUI Shell\GUI Shell.csproj"
                };

            var architecture = new CsProjParser(csProjs).Parse(CsProjParser.ReadLinesFromFile);
            var projects = architecture.Projects;

            Assert.AreEqual(2, projects.Count());
            Assert.AreEqual("EDXServer", projects[0].Name);
            Assert.AreEqual("GUI Shell", projects[1].Name);
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
