using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeHeMo.DTO
{
    public class DTO_TEST
    {
        public Guid ID { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public bool? Succeed { get; set; }
        public DateTime? Date { get; set; }
    }
}
