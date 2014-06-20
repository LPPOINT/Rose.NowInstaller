using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Rose.NowInstaller.Core.Installation
{
    public class InstallProgramInfo
    {

        public InstallProgramInfo()
        {
            Metadata = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public Guid Guid { get; set; }

        public IDictionary<string, string> Metadata { get; private set; }

        public void ToXml(XmlWriter writer)
        {
            
        }

    }
}
