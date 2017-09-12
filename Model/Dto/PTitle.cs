using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    [Table("PTitle")]
    public class PTitle
    {
        [Key]
        public int id { get; set; }

        public string className { get; set; }
        public string labelName { get; set; }
        public string showVale { get; set; }
    }
    
}
