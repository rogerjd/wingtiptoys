using System.ComponentModel.DataAnnotations;

namespace WingtipToys.Models
{
    class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public string Username { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; }
    }
}
