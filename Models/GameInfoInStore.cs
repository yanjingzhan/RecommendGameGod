using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class GameInfoInStore
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public string Updated { set; get; }
        public string Publisher { set; get; }
        public string Image { set; get; }
        public List<string> screenshots { set; get; }
        public string Category { set; get; }
        public string Version { set; get; }
        public string PackageSize { set; get; }

        public string ClientTypes { set; get; }
    }
}
