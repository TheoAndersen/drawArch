using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawArch.Parser
{
    public class Database
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ToolTip { get; internal set; }
    }
}
