﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using MODEOUTLED.ViewModels;


namespace MODEOUTLED.Controllers.Cart
{
    public class CartController : Controller
    {
        tuan_nanyspa_vn_dbEntities db = new tuan_nanyspa_vn_dbEntities();
        [ChildActionOnly]
        public ActionResult CartIcon()
        {
            return PartialView();
        }
        
        #region[View gio hang Top]
        [HttpPost]
        public JsonResult command(string co, string color, string size)
        {
            if (color == null || color == "") { color = "0"; }
            if (size == null || size == "") { size = "0"; }
            string anh = "";
            var re = co.Replace("\":", ":").Replace(",\"", ":").Replace("{\"", ":").Replace("}", ":");
            var tr = re.Split(':');
            string chuoi = "";
            int count = 0;
            List<int> numCart = new List<int>();
            List<Product> obj = new List<Product>();
            chuoi += "<ul>";
            for (int i = 0; i < tr.Length; i++)
            {
                if (i == 0 || i == (tr.Length - 1))
                { }
                else if (i % 2 != 0)
                {
                    int idpd=int.Parse(tr[i]);
                    var list = db.Products.Where(m => m.Id == idpd).FirstOrDefault();
                    if (list != null)
                    {
                        chuoi += "<li class='proCart clearfix'>";
                        chuoi += "<img src='" + list.Image + "'/>";
                        chuoi += "<a href='/Home/Detail/" + list.Tag + "'>" + list.Code + " <span>x " + tr[(i + 1)] + "</span></a>";
                        chuoi += "</li>";
                    }
                    obj.Add(list);
                    count += Convert.ToInt32(tr[(i + 1)]);
                    numCart.Add(Convert.ToInt32(tr[(i + 1)]));
                }
            }
            Session["proId"] = obj;
            chuoi += "<li><p class='pCartView'><a class='btn-view-cart' href='/Home/checkout'>Xem giỏ hàng (" + count + " sản phẩm)</a></p></li>";
            chuoi += "<ul>";
            Session["count"] = numCart;
            AddToCart(tr, color, size);
            return Json(new { success = chuoi, html = count });
        }
        #endregion
        #region[Them moi sp vao gio hang]
        int cartTotal;
        ShoppingCartViewModel shoppCart;
        public void AddToCart(Array tr, string color, string size)
        {
            if (color == null || color == "") { color = "0"; }
            if (size == null || size == "") { size = "0"; }
            var soluong = (List<int>)Session["count"];//so luong sp co trong gio hang tuong ung voi tung sp trong Session["proId"]
            var data = (List<Product>)Session["proId"];//luu thong tin sp co trong gio hang
            for (int i = 0; i < data.Count; i++)
            {
                Product obj = data[i];
                if (obj != null)
                {
                    int gia = 0;
                    string gias = "0";
                    if (obj.PriceRetail != null)
                    {
                        gias = obj.PriceRetail.ToString();
                        gia = int.Parse(gias);
                    }
                    int tong = (soluong[i] * gia);
                    int flag = -2;
                    if (Session["ShoppingCart"] == null)
                    {
                        InitShoppingCartSession();
                    }
                    shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
                    int dem = GetCartItem(shoppCart, obj.Id, soluong[i]);
                    if (dem == flag)
                    {
                        osCart cartItem;
                        cartItem = new osCart
                        {
                            productId = obj.Id,
                            productImage = obj.Image,
                            productName = obj.Name,
                            productTag = obj.Tag,
                            price = obj.PriceRetail.ToString(),
                            count = soluong[i],
                            //proweight = int.Parse(obj.Weight.ToString()),
                            idsize = int.Parse(size),
                            idcolor = int.Parse(color),
                            total = tong
                        };
                        shoppCart.CartItems.Add(cartItem);
                    }
                    else
                    {
                        if (dem != -1)
                        {
                            shoppCart.CartItems[i].count = soluong[i];
                            shoppCart.CartItems[i].total = Convert.ToInt32(shoppCart.CartItems[i].price) * shoppCart.CartItems[i].count;
                        }
                    }
                }
            }
            if (shoppCart != null)
            {
                if (shoppCart.CartItems.Count > 0)
                {
                    for (int k = 0; k < shoppCart.CartItems.Count; k++)
                    {
                        cartTotal += shoppCart.CartItems[k].total;
                    }
                }
                shoppCart.CartTotal = cartTotal;
                Session["ShoppingCart"] = shoppCart;
            }
        }
        #endregion
        #region[Tao moi gio hang]
        public void InitShoppingCartSession()
        {
            var shoppingCart = new ShoppingCartViewModel();
            Session["ShoppingCart"] = shoppingCart;
        }
        #endregion
        #region[Kiem tra sp ton tai trong gio hang]
        private int GetCartItem(ShoppingCartViewModel cart, int key, int count)
        {
            int found = -2;
            if (cart.CartItems.Count > 0)
            {
                for (int i = 0; i < cart.CartItems.Count; i++)
                {
                    if (cart.CartItems[i].productId == key && cart.CartItems[i].count == count)
                    {
                        found = -1;
                    }
                    else if (cart.CartItems[i].productId == key && cart.CartItems[i].count != count)
                    {
                        found = 0;
                    }
                }
            }
            return found;
        }
        #endregion
        #region[Xoa sp khoi gio hang]
        [HttpPost]
        public void RemoveFromCart(int id)
        {
            ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
            for (int i = 0; i < shoppCart.CartItems.Count; i++)
            {
                if (shoppCart.CartItems[i].productId == id)
                {
                    shoppCart.CartItems.RemoveAt(i);
                    break;
                }
            }
            if (shoppCart.CartItems.Count > 0)
            {
                for (int j = 0; j < shoppCart.CartItems.Count; j++)
                {
                    cartTotal += shoppCart.CartItems[j].total;
                }
                shoppCart.CartTotal = cartTotal;
            }
            else
            {
                shoppCart.CartTotal = 0;
            }

            Session["ShoppingCart"] = shoppCart;
        }
        #endregion
        #region[Cap nhat so luong trong gio hang]
        [HttpPost]
        public void UpdateCartCountItem(int id, int cartCount)
        {
            ShoppingCartViewModel shoppCart = (ShoppingCartViewModel)Session["ShoppingCart"];
            for (int i = 0; i < shoppCart.CartItems.Count; i++)
            {
                if (shoppCart.CartItems[i].productId == id)
                {
                    shoppCart.CartItems[i].count = cartCount;
                    shoppCart.CartItems[i].total = Convert.ToInt32(shoppCart.CartItems[i].price) * cartCount;
                    break;
                }
            }
            for (int j = 0; j < shoppCart.CartItems.Count; j++)
            {
                cartTotal += shoppCart.CartItems[j].total;
            }
            shoppCart.CartTotal = cartTotal;
            Session["ShoppingCart"] = shoppCart;
        }
        #endregion
        #region[Them vao gio hang Top]
        [HttpPost]
        public ActionResult UpdateTopCart(string co, int type, string color, string size)
        {
            string anh = "";
            var re = co.Replace("\":", ":").Replace(",\"", ":").Replace("{\"", ":").Replace("}", ":");
            var tr = re.Split(':');
            string chuoi = "";
            int count = 0;
            List<int> numCart = new List<int>();
            List<Product> obj = new List<Product>();
            chuoi += "<ul>";
            for (int i = 0; i < tr.Length; i++)
            {
                if (i == 0 || i == (tr.Length - 1))
                { }
                else if (i % 2 != 0)
                {
                    int idpd = int.Parse(tr[i]);
                    var list = db.Products.Where(m => m.Id == idpd).FirstOrDefault();
                    if (list != null)
                    {
                        chuoi += "<li class='proCart clearfix'>";
                        chuoi += "<img src='" + list.Image + "'/>";
                        chuoi += "<a href='/Home/Detail/" + list.Tag + "'>" + list.Code + " <span>x " + tr[(i + 1)] + "</span></a>";
                        chuoi += "</li>";
                    }
                    obj.Add(list);
                    count += Convert.ToInt32(tr[(i + 1)]);
                    numCart.Add(Convert.ToInt32(tr[(i + 1)]));
                }
            }
            chuoi += "</ul>";
            Session["proId"] = obj;
            chuoi += "<p class='pCartView'><a class='btn-view-cart' href='/Home/checkout'>Xem giỏ hàng (" + count + " sản phẩm)</a></p>";
            Session["count"] = numCart;
            AddToCart(tr, color, size);
            if (type == 0)
            {
                return Json(new { success = chuoi, html = count });
            }
            else
            {
                return Redirect("/Home/checkout");
            }
        }
        #endregion
        #region[_siteMap]
        public ActionResult _siteMap()
        {
            string pathway = "";
            var tag = RouteData.Values["id"];
            if (tag != null)
            {
                pathway += "<div class=\"pathway\">";
                var pro = db.Products.Where(m => m.Tag == tag.ToString()).FirstOrDefault();
                if (pro != null)
                {
                    var Cat = db.Categories.Where(m => m.Id == pro.IdCategory).FirstOrDefault();
                    var CatL2 = db.Categories.Where(m => m.Id == pro.IdCategory2).FirstOrDefault();
                    if (CatL2 != null)
                    {
                        pathway += "<a href=\"/Home/Index\">Trang chủ</a><a href=\"/sanpham/sp/" + Cat.Tag + "\">" + Cat.Name + "</a><a href=\"/sanpham/sp/" + CatL2.Tag + "\">" + CatL2.Name + "</a>";
                    }
                    else
                    {
                        pathway += "<a href=\"/Home/Index\">Trang chủ</a><a href=\"/sanpham/sp/" + Cat.Tag + "\">" + Cat.Name + "</a>";
                    }
                }
                else
                {
                    var Cat = db.Categories.Where(m => m.Tag == tag.ToString()).FirstOrDefault();
                    if (Cat != null)
                    {
                        if (Cat.Level.Length > 5)
                        {
                            var CatL1 = db.Categories.Where(m => m.Level.Length == 5 && m.Level == Cat.Level.Substring(0, 5)).FirstOrDefault();
                            pathway += "<a href=\"/Home/Index\">Trang chủ</a><a href=\"/sanpham/sp/" + CatL1.Tag + "\">" + CatL1.Name + "</a><a href=\"/sanpham/sp/" + Cat.Tag + "\">" + Cat.Name + "</a>";
                        }
                        else
                        {
                            pathway += "<a href=\"/Home/Index\">Trang chủ</a><a href=\"/sanpham/sp/" + Cat.Tag + "\">" + Cat.Name + "</a>";
                        }
                    }
                }
                pathway += "</div>";
            }
            ViewBag.View = pathway;
            return PartialView();
        }
        #endregion
        #region[Cat chuoi text de hien thi]
        protected string FormatContentNews(string value, int count)
        {
            string _value = value;
            if (_value.Length >= count)
            {
                string ValueCut = _value.Substring(0, count - 3);
                string[] valuearray = ValueCut.Split(' ');
                string valuereturn = "";
                for (int i = 0; i < valuearray.Length - 1; i++)
                {
                    valuereturn = valuereturn + " " + valuearray[i];
                }
                return valuereturn;
            }
            else
            {
                return _value;
            }
        }
        #endregion
        #region[view chon lua dang nhap hoac mua hang luon k can tai khoan dk]
        public ActionResult notLogon()
        {
            return View();
        }
        #endregion
    }
}
