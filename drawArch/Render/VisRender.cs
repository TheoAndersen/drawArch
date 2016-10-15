using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using drawArch.Parser;
using System.IO;

namespace drawArch.Render
{
    public class VisRender
    {
        private Architecture architecture;

        public VisRender(Architecture architecture)
        {
            this.architecture = architecture;
        }

        internal string Render()
        {
            var template = VisTemplate.Html();
            return template.Replace("//{{CONTENT}}", renderContent());
        }

        private string renderContent()
        {
            var nodes = "var nodes = new vis.DataSet([" + Environment.NewLine;
            var edges = "var edges = new vis.DataSet([" + Environment.NewLine;

            foreach (var project in this.architecture.Projects)
            {
                nodes += renderNodeBox(project) + Environment.NewLine;

                foreach (var edge in project.References)
                {
                    var toProject = this.architecture.Projects.Single(p => p.Path == edge.Path); // to find the real ID
                    edges += renderEdgeBoxArray(project, toProject);
                }

                for (int i = 0; i < project.Services.Count; i++)
                {
                    nodes += renderService(project, project.Services[i], i);
                    edges += renderEdgeService(project, i);
                }
            }

            nodes += "]);" + Environment.NewLine;
            edges += "]);" + Environment.NewLine;

            return nodes + Environment.NewLine + Environment.NewLine +
                   edges;
        }

        private string renderEdgeService(Project from, int i)
        {
            return "{ from: " + from.Id + ", to: " + from.Id + "0" + i +", color: 'black'}," + Environment.NewLine;

        }

        private string renderService(Project project, string service, int serviceNumber)
        {
            return "{id: " + project.Id + "0" + serviceNumber +
                    ", size: 5 " + 
                    ", label: '" + service +
                    "', shape: 'dot', color: blackAndWhite},";
        }

        private string renderNodeBox(Project project)
        {
            string color = "blackAndLightGray";

            if(project.Services.Count > 0)
            {
                color = "blackAndGreen";
            }

            switch (project.Type)
            {
                case Project.ProjectType.Console:
                    color = "blackAndBlue";
                    break;
                case Project.ProjectType.WindowsExe:
                    color = "blackAndDarkerBlue";
                    break;
            }


            return "{id: " + project.Id + 
                   ", label: '" + project.Name + 
                   "', shape: 'box', color: " + color + "},";
        }

        private string renderEdgeBoxArray(Project from, Project to)
        {
            return "{ from: " + from.Id + ", to: " + to.Id + ", arrows: 'to', color: 'black'}," + Environment.NewLine;
        }

//        // create an array with nodes
//        var nodes = new vis.DataSet([
//       {id: 1, label: 'Assembly A', shape: 'box', color: blackAndWhite},
//       {id: 2, label: 'Assembly B', shape: 'box', color:blackAndWhite
//},
//       {id: 3, label: 'Assembly C', shape: 'box', color:blackAndWhite},
//       {id: 4, label: 'Assembly D', shape: 'box', color:blackAndWhite},
//       {id: 5, label: 'Assembly E', shape: 'box', color:blackAndWhite},
//       {id: 7, label: 'Database B', shape: 'database', color:{background:'orange', border: 'black'}},
//       {id: 11, label: 'Assembly F', shape: 'box', color:blackAndBlue},
//       {id: 12, label: 'Assembly G', shape: 'box', color:blackAndBlue},
//       {id: 13, label: 'Assembly H', shape: 'box', color:blackAndBlue},
//       {id: 21, size: 5, label: 'ServiceName', shape: 'dot', color: blackAndWhite}
//     ]);

//     // create an array with edges
//     var edges = new vis.DataSet([
//       {from: 1, to: 3, arrows: 'to', color: 'black'},
//       {from: 2, to: 3, arrows: 'to', color: 'black'},
//       {from: 1, to: 2, arrows: 'to', color: 'black'},
//       {from: 2, to: 4, arrows: 'to', color: 'black'},
//       {from: 2, to: 5, arrows: 'to', color: 'black'},
//       {from: 5, to: 7, arrows: 'to', dashes: true, color: 'gray'},
//       {from: 11, to: 13, arrows: 'to', color: 'black'},
//       {from: 11, to: 12, arrows: 'to', color: 'black'},
//       {from: 11, to: 21, color: 'black'},
//       {from: 4, to: 21, dashes: true, arrows: 'to'}
     //]);

    }
}
