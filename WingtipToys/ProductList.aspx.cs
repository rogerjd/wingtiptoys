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
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Tst()
        {

        }

        public IQueryable<Product> GetProducts([QueryString("id")] int? categoryID)
        {
            var _db = new WingtipToys.Models.ProductContext();
            IQueryable<Product> query = _db.Products;
            if (categoryID.HasValue && categoryID > 0)
            {
                query = query.Where(p => p.CategoryID == categoryID);
            }
            return query;
        }
    }
}