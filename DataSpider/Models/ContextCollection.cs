using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSpider.Models
{
    public class ContextCollection
    {
        public List<Context> NewContexts { get; set; } = new List<Context>();
        public List<Context> UpdateContexts { get; set; } = new List<Context>();

        public bool IsValid
        {
            get
            {
                return NewContexts.Count > 0 || UpdateContexts.Count > 0;
            }
        }

    }
}
