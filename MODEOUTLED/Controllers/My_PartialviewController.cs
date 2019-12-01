using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;

namespace MODEOUTLED.Controllers
{
    public class My_PartialviewController : Controller
    {
        tuan_nanyspa_vn_dbEntities db = new tuan_nanyspa_vn_dbEntities();
        #region [Menu top]
        public ActionResult _MenuTop()
        {
            string chuoi = "";
            var cat = db.Menus.Where(m => m.Level.Length == 5).OrderBy(m => m.Ord).ToList();
            
            for (int i = 0; i < cat.Count; i++)
            {
                List<Menu> menus = db.Menus.ToList();
                List<Menu> catsub = new List<Menu>();
                string levelm = cat[i].Level;
                catsub = db.Menus.Where(m => m.Level.Length > 5 && m.Level.Substring(0, 5) == levelm).OrderBy(m => m.Ord).ToList();
                if (catsub.Count > 0)
                {
                    if ((Request.Url.ToString().IndexOf(cat[i].Link) > 0 && cat[i].Link != "/") || (cat[i].Link == "/" && (Request.Url.ToString() == "http://localhost:1584/" || Request.Url.ToString() =="http://ilovestyle.vn/")))
                        {
                            if (i == 0)
                            {
                                chuoi += "<li style=\"background: none\"><a class=\"active\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";
                            }
                            else
                            {
                                chuoi += "<li><a class=\"active\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                chuoi += "<li style=\"background: none\"><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";
                            }
                            else
                            {
                                chuoi += "<li><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";
                            }
                        }
                        chuoi += "<ul class=\"navSub\">";
                        for (int k = 0; k < catsub.Count; k++)
                        {
                            chuoi += "<li><a href=\"" + catsub[k].Link + "\">" + catsub[k].Name + "</a></li>";
                        }
                        chuoi += "</ul></li>";
                }
                else
                {
                    if ((Request.Url.ToString().IndexOf(cat[i].Link) > 0 && cat[i].Link != "/") || (cat[i].Link == "/" && (Request.Url.ToString() == "http://localhost:1584/" || Request.Url.ToString() == "http://ilovestyle.vn/")))
                        {
                            if (i == 0)
                            {
                                chuoi += "<li style=\"background: none\"><a class=\"active\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                            }
                            else
                            {
                                chuoi += "<li><a class=\"active\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                chuoi += "<li style=\"background: none\"><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                            }
                            else
                            {
                                chuoi += "<li><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                            }
                        }
                }
            }
           
            ViewBag.Cat = chuoi;
            return PartialView();
        }
        #endregion

        #region[Login - Register Form]
        public ActionResult _Login()
        {
            return PartialView();
        }
        #endregion

        #region[_MenuSearch]
        public ActionResult _Search()
        {
            return PartialView();
        }
        #endregion

        #region[_Logo]
        public ActionResult _Logo()
        {
            string chuoi = "";
            var list = db.Advertises.Where(m => m.Position == 0 && m.Active == true).Take(1).ToList();
            if (list.Count > 0)
            {
               chuoi += "<img src=\"" + list[0].Image + "\" alt=\""+ list[0].Name +"" + "\" />";
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion
        #region[Quảng cáo bên phải phía trên]
        public ActionResult _AdvLeft()
        {
            string chuoi = "";
            var list = db.Advertises.Where(m => m.Position == 1 && m.Active == true).OrderBy(m => m.Orders).Take(10).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Image!=null && list[i].Image != "")
                {
                    chuoi += "<a href=\"" + list[i].Link + "\" title=\"" + list[i].Name + "\" >";
                    chuoi += "<img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\" />";
                    chuoi += "</a>";
                }
                else
                {
                    chuoi += list[i].Description;
                } 
            }
            ViewBag.View = chuoi;
            var listc = db.Configs.Where(m => m.Mail_Info.Length>0).ToList();
            string chuoic = "";
            if (listc.Count > 0)
            {
                chuoic = "<a class=\"hopthu\" href=\"mailto:" + listc[0].Mail_Info + "\" title=\"Liên hệ từ: " + listc[0].PlaceHead + "\" rel=\"nofollow\"> " + listc[0].Mail_Info + "</a></div>";
            }
            else
            {
                chuoic = "<a class=\"hopthu\" href=\"mailto:banbientap@nghiadong.gov.vn\" title=\"Liên hệ từ: nghiadong.gov.vn\" rel=\"nofollow\"> banbientap@nghiadong.gov.vn</a></div>";
            }
            ViewBag.config = chuoic;
            listc.Clear();
            listc = null;
            list.Clear();
            list = null;
            return PartialView();
        }
        #endregion
        #region[Quảng cáo bên phải phía dưới]
        public ActionResult _AdvRightBottom()
        {
            string chuoi = "";
            var list = db.Advertises.Where(m => m.Position == 2 && m.Active == true).OrderBy(m => m.Orders).Take(10).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Image != null && list[i].Image != "")
                {
                    chuoi += "<a href=\"" + list[i].Link + "\" title=\"" + list[i].Name + "\" >";
                    chuoi += "<img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\" />";
                    chuoi += "</a>";
                }
                else
                {
                    chuoi += list[i].Description;
                }
            }
            ViewBag.View = chuoi;
            return PartialView();
        }
        #endregion
        #region[_CopyRight]
        public ActionResult _CopyRight()
        {
            string chuoi = "";
            var list = db.Configs.Where(m => m.Id >0).Take(1).ToList();
            if (list.Count > 0)
            {
                ViewBag.View = list[0].Copyright;
            }
            return PartialView();
        }
        #endregion

        #region[News-Right]
        public ActionResult _NewsRight()
        {
            string chuoi = "";
            var listg = (from g in db.v_GroupNewsInNews where g.Priority == true && g.Active == true orderby g.Level select g).ToList();
            for (int j = 0; j < listg.Count; j++)
            {
                int gid = listg[j].Id;
                chuoi += "<div class=\"box-news-right\">";
                chuoi += "<div class=\"head\"><a href=\"/danh-muc-tin/" + listg[j].Tag + "\">" + listg[j].Name + "</a></div>";
                chuoi += "<ul class=\"highlights\">";
                var listitem = (from p in db.News where p.Active == true && p.Index == true && p.IdGroup == gid orderby p.Id descending select p).Take(4).ToList();
                for (int i = 0; i < listitem.Count; i++)
                {
                    if (i == 0)
                    {
                        chuoi += "<li>";
                        chuoi += "<a href=\"/nghia-dong/" + listitem[0].Tag + "\" title=\"" + listitem[0].Name + "\">";
                        chuoi += "<div class=\"image\"><img src=\"" + listitem[0].Image + "\" alt=\"" + listitem[0].Name + "\"></a></div>";
                        chuoi += "<div class=\"item_title_fo\">";
                        chuoi += "<a href=\"/nghia-dong/" + listitem[0].Tag + "\" title=\"" + listitem[0].Name + "\">" + listitem[0].Name + "</a></div></li></ul>";
                        chuoi += "<ul class=\"item-right\">";
                    }
                    else
                    {
                        chuoi += "<li><a href=\"/nghia-dong/" + listitem[i].Tag + "\" title=\"" + listitem[i].Name + "\"><img width=\"60\" height=\"44\" src=\"" + listitem[i].Image + "\" alt=\"" + listitem[i].Name + "\">" + listitem[i].Name + "</a></li>";
                    }
                }
                 chuoi += "</ul>";
                chuoi += "<div class=\"news-right-more\"><a href=\"/danh-muc-tin/"+ listg[j].Tag +"\" title=\"\"><span class=\"icon_bt\" style=\"font-family: &quot;FontAwesome&quot;;\"></span> Xem thêm</a></div>";
                chuoi += "</div>";
                listitem.Clear();
                listitem = null;
            }
            ViewBag.chuoi = chuoi;
            listg.Clear();
            listg = null;
            return PartialView();
        }
        #endregion
    }
}
