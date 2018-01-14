using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreMasterDetails.Models.OrderMgmt.Models
{
    public class OrderMast
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderDetl> OrderDetl { get; set; }
    }
}
