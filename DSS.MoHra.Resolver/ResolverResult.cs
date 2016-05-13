using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS.MoHra.Resolver
{
    public class ResolverResult
    {
        private List<string> _log;

        public List<ResolverFact> Facts { set; get; }
        public ReadOnlyCollection<string> Log { get { return _log.AsReadOnly(); } }
        public string Summary { set; get; }

        public ResolverResult()
        {
            _log = new List<string>();
            Facts = new List<ResolverFact>();
        }

        public void Add(string text)
        {
            _log.Add(text);
        }
    }
}
