using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WingtipToys.Models;

namespace WingtipToys.Logic
{
    public class ShoppingCartActions : IDisposable
    {
        public string ShoppingCartID { get; set; }

        private ProductContext _db = new ProductContext();

        public const string CartSessionKey = "CartID";

        public void AddToCart(int id)
        {
            ShoppingCartID = GetCartID();

            var cartItem = _db.ShoppingCartItems.SingleOrDefault(
                c => c.CartID == ShoppingCartID && c.ProductID == id);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ItemID = Guid.NewGuid().ToString(),
                    ProductID = id,
                    CartID = ShoppingCartID,
                    Product = _db.Products.SingleOrDefault(p => p.ProductID == id),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                _db.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;

            }
            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        public string GetCartID()
        {
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tmpCartID = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tmpCartID.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public List<CartItem> GetCartItems()
        {
            ShoppingCartID = GetCartID();
            return _db.ShoppingCartItems.Where(c => c.CartID == ShoppingCartID).ToList();
        }

        public decimal GetTotal()
        {
            ShoppingCartID = GetCartID();
            decimal? tot = decimal.Zero;
            tot = (decimal?)(from ci in _db.ShoppingCartItems
                             where ci.CartID == ShoppingCartID
                             select (int?)ci.Quantity * ci.Product.UnitPrice).Sum();
            return tot ?? decimal.Zero;
        }

        public ShoppingCartActions GetCart(HttpContext context)
        {
            using (var cart = new ShoppingCartActions())
            {
                cart.ShoppingCartID = cart.GetCartID();
                return cart;
            }
        }

        public void UpdateShoppingCartDatabase(String cartID, ShoppingCartUpdates[] CartItemUpdates)
        {
            try
            {
                int CartItemCount = CartItemUpdates.Count();
                List<CartItem> myCart = GetCartItems();
                foreach (var cartItem in myCart)
                {
                    for (int i = 0; i < CartItemCount; i++)
                    {
                        if (cartItem.Product.ProductID == CartItemUpdates[i].ProductID)
                        {
                            if (CartItemUpdates[i].PurchaseQty < 1 || CartItemUpdates[i].RemoveItem)
                            {
                                RemoveItem(cartID, cartItem.ProductID);
                            }
                            else
                            {
                                UpdateItem(cartID, cartItem.ProductID, CartItemUpdates[i].PurchaseQty);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void RemoveItem(string remvCartID, int remvProdID)
        {
            using (var db = new WingtipToys.Models.ProductContext())
            {
                try
                {
                    var myItem = (from c in db.ShoppingCartItems where c.CartID == remvCartID && c.Product.ProductID == remvProdID select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        db.ShoppingCartItems.Remove(myItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void UpdateItem(string CartID, int ProdID, int qty)
        {
            using (var db = new WingtipToys.Models.ProductContext())
            {
                try
                {
                    var myItem = (from c in db.ShoppingCartItems where c.CartID == CartID && c.Product.ProductID == ProdID select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        myItem.Quantity = qty;
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void EmptyCart()
        {
            ShoppingCartID = GetCartID();
            var cartItems = _db.ShoppingCartItems.Where(c => c.CartID == ShoppingCartID);
            foreach (var cartItem in cartItems)
            {
                _db.ShoppingCartItems.Remove(cartItem);
            }
            _db.SaveChanges();
        }

        public int GetCount()
        {
            ShoppingCartID = GetCartID();
            int? count = (from cartItems in _db.ShoppingCartItems
                          where cartItems.CartID == ShoppingCartID
                          select (int?)cartItems.Quantity).Sum();
            return count ?? 0;

        }

        public struct ShoppingCartUpdates
        {
            public int ProductID;
            public int PurchaseQty;
            public bool RemoveItem;
        }

    }
}