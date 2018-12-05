using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Uow.Models;
using Dapper.Contrib.Extensions;

namespace DataSpider.Models
{
    [Table("T_ContextData")]
    public class ContextData : BaseModel
    {
        [Key]
        public int ID { get; set; }

        public int DeclareID { get; set; }

        public string HtmlContext { get; set; }
    }
}
