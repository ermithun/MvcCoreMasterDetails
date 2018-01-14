using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreMasterDetails.Models.OrderMgmt.ViewModels
{
    public class OrderDetlViewModel
    {
        public int Id { get; set; }
        public int MastId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public Flag Flag { get; set; }
    }

    public enum Flag
    {
        New=0,
        Deleted=1
    }
}
