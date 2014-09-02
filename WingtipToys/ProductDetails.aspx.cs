using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Models;
using System.Web.ModelBinding;

namespace WingtipToys
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // The id parameter should match the DataKeyNames value set on the control
        // or be decorated with a value provider attribute, e.g. [QueryString]int id
        public IQueryable<Product> GetProduct([QueryString("productID")] int? productID)
        {
            var db = new WingtipToys.Models.ProductContext();
            IQueryable<Product> query = db.Products;
            if (productID.HasValue && productID > 0)
            {
                query = query.Where(p => p.ProductID == productID);
            }
            else
            {
                query = null;
            }
            return query;
        }
    }
}