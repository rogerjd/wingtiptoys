using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Models;
using WingtipToys.Logic;
using System.Collections.Specialized;
using System.Collections;
using System.Web.ModelBinding;

namespace WingtipToys
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (ShoppingCartActions sca = new ShoppingCartActions())
            {
                decimal cartTotal = 0;
                cartTotal = sca.GetTotal();
                if (cartTotal > 0)
                {
                    lblTotal.Text = String.Format("{0:c}", cartTotal);
                }
                else
                {
                    LabelTotalText.Text = "";
                    lblTotal.Text = "";
                    ShoppingCartTitle.InnerText = "Shopping Cart is Empty";
                    UpdateBtn.Visible = false;
                }
            }
        }

        public List<CartItem> GetShoppingCartItems()
        {
            ShoppingCartActions sca = new ShoppingCartActions();
            return sca.GetCartItems();
        }

        public List<CartItem> UpdateCartItems()
        {
            using (ShoppingCartActions sca = new ShoppingCartActions())
            {
                String cartID = sca.GetCartID();
                ShoppingCartActions.ShoppingCartUpdates[] cartUpdts = new ShoppingCartActions.ShoppingCartUpdates[CartList.Rows.Count];
                for (int i = 0; i < CartList.Rows.Count; i++)
                {
                    IOrderedDictionary rowVals = new OrderedDictionary();
                    rowVals = GetValues(CartList.Rows[i]);
                    cartUpdts[i].ProductID = Convert.ToInt32(rowVals["ProductID"]);

                    CheckBox cbRemv = (CheckBox)CartList.Rows[i].FindControl("Remove");
                    cartUpdts[i].RemoveItem = cbRemv.Checked;

                    TextBox txtQty = (TextBox)CartList.Rows[i].FindControl("PurchaseQuantity");
                    cartUpdts[i].PurchaseQty = Convert.ToInt32(txtQty.Text);
                }
                sca.UpdateShoppingCartDatabase(cartID, cartUpdts);
                CartList.DataBind();
                lblTotal.Text = String.Format("{0:c}", sca.GetTotal());
                return sca.GetCartItems();
            }
        }

        public static IOrderedDictionary GetValues(GridViewRow row)
        {
            IOrderedDictionary vals = new OrderedDictionary();
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.Visible)
                {
                    cell.ContainingField.ExtractValuesFromCell(vals, cell, row.RowState, true);
                }
            }
            return vals;
        }

        protected void UpdateBtn_Click(object sender, EventArgs ea)
        {
            UpdateCartItems();
        }
    }
}