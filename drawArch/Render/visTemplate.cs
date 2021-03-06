﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawArch.Render
{
    public static class VisTemplate
    {
        public static string Html()
        {
            return @"
<!doctype html>
<html>
<head>
    <title>Network | Basic usage</title>

    <script type=""text/javascript"" src=""https://cdnjs.cloudflare.com/ajax/libs/vis/4.16.1/vis.min.js""></script>
    <link href=""https://cdnjs.cloudflare.com/ajax/libs/vis/4.16.1/vis.min.css"" rel=""stylesheet"" type=""text/css"" />
     

    <style type=""text/css"">
        #mynetwork {
            width: 1200px;
            height: 700px;
            border: 1px solid lightgray;
        }
    </style>
</head>
<body>

    <p>
        Create a simple network with some nodes and edges.
    </p>

    <div id=""mynetwork""></div>

    <!--
       *x find all assemblies, and their relation
    *** plot in all solutions(Grouping)
     *** x assemblies which exposes services
     *** x mark each assembly which accesses a database
     *** assembly which accesses another via an svc proxy
       -->

    <script type=""text/javascript"">

    var blackAndLightGray = { background: 'lightgray', border: 'black' };
    var blackAndYellow = { background: '#ffcc00', border: 'black' };
    var blackAndBlue = { background: 'lightblue', border: 'black' };
    var blackAndWhite = { background: 'white', border: 'black' };
    var blackAndGreen = { background: 'lightgreen', border: 'black' };
    var blackAndDarkerBlue = { background: '#9999ff', border: 'black' };

//{{CONTENT}}

    // create a network
    var container = document.getElementById(""mynetwork"");
    var data = {
       nodes: nodes,
       edges: edges
     };
    var options = { };
    var network = new vis.Network(container, data, options);
    </script>
</body>
</html>
                

                ";
        }
    }
}
