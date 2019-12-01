using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using MODEOUTLED.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace MODEOUTLED.Controllers.Default
{
    public class DefaultController : Controller
    {
        tuan_nanyspa_vn_dbEntities data = new tuan_nanyspa_vn_dbEntities();
        public ActionResult Album()
        {
            string chuoi = "";
            var listalbum = (from p in data.GroupProducts where p.Active == true orderby p.Id descending select p).ToList();
            for (int j = 0; j < listalbum.Count; j++)
            {
                ViewBag.link = "<li class='Last'><span>Danh mục sản phẩm</span></li>";
                int gid = listalbum[j].Id;
                var list = (from p in data.Products where p.IdCategory==gid orderby p.Id descending select p).ToList();
                if(list.Count>0)
                {
                    if (j % 4 != 0)
                    {
                        chuoi += "<li>";
                    }
                    else
                    {
                        chuoi += "<li class=\"right\">";
                    }
                    chuoi += "<div class=\"image\"><a href=\"/danh-muc/" + listalbum[j].Tag + "\"><img src=\"" + list[0].Image + "\" alt='' /></a></div>";
                    chuoi += "<div class=\"names\">";
                    chuoi += "<a href=\"/danh-muc/" + listalbum[j].Tag + "\">" + listalbum[j].Name + "</a><br />";
                    chuoi += "<span>" + list.Count + " sản phẩm</span>";
                    chuoi += "</div>";
                    chuoi += "</li>";
                }
                list.Clear();
                list = null;
            }
            #region[Title, Keyword, Deskription]
            var listconfig = (from p in data.Configs select p).ToList();
            if (listconfig.Count > 0) { ViewBag.tit = listconfig[0].Title; ViewBag.des = listconfig[0].Description; ViewBag.key = listconfig[0].Keyword; }
            listconfig.Clear();
            listconfig = null;
            #endregion
            ViewBag.pro = chuoi;
            listalbum.Clear();
            listalbum = null;
            return View();
        }
        public ActionResult Albums(int? page, string tag)
        {
            if (Request.HttpMethod == "GET")
            {
                
            }
            else
            {
                page = 1;
            }
            #region[Phân trang]
            int pageSize = 16;
            int pageNumber = (page ?? 1);

            // Thiết lập phân trang
            PagedListRenderOptions ship = new PagedListRenderOptions();

            ship.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToLastPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToNextPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToIndividualPages = true;
            ship.DisplayPageCountAndCurrentLocation = false;
            ship.MaximumPageNumbersToDisplay = 5;
            ship.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            ship.EllipsesFormat = "&#8230;";
            ship.LinkToFirstPageFormat = "Trang đầu";
            ship.LinkToPreviousPageFormat = "«";
            ship.LinkToIndividualPageFormat = "{0}";
            ship.LinkToNextPageFormat = "»";
            ship.LinkToLastPageFormat = "Trang cuối";
            ship.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            ship.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            ship.FunctionToDisplayEachPageNumber = null;
            ship.ClassToApplyToFirstListItemInPager = null;
            ship.ClassToApplyToLastListItemInPager = null;
            ship.ContainerDivClasses = new[] { "pagination-container" };
            ship.UlElementClasses = new[] { "pagination" };
            ship.LiElementClasses = Enumerable.Empty<string>();

            ViewBag.ship = ship;
            #endregion
            var g = data.GroupProducts.Where(m => m.Tag == tag).SingleOrDefault();
            if (g != null)
            {
                ViewBag.link = "<li class='Last'><span>" + g.Name + "</span></li>";
                int gid = g.Id;
                var list = (from p in data.Products where p.Active == true && p.IdCategory==gid orderby p.Id descending select p).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
                list.Clear();
                list = null;
                #region[Title, Keyword, Deskription]
                if (g.Title.Length > 0) { ViewBag.tit = g.Title; } else { ViewBag.tit = g.Name; }
                if (g.Description.Length > 0) { ViewBag.des = g.Description; } else { ViewBag.des = g.Name; }
                if (g.Keyword.Length > 0) { ViewBag.key = g.Keyword; } else { ViewBag.key = g.Name; }
                #endregion
            }
            else
            {
                return View();
            }
        }
        public ActionResult ilovestylepage(string tag)
        {
            string chuoi = "";
            var list = data.Menus.Where(m => m.Tag == tag).SingleOrDefault();
            if (list != null)
            {
                ViewBag.link = "<li class='Last'><span>" + list.Name + "</span></li>";
                #region[Facebook]
                string chuoifbg = "";
                string email = "";
                var config = (from n in data.Configs select n).ToList();
                if (config.Count > 0)
                {
                    if (config[0].Mail_Info != null && config[0].Mail_Info != "")
                    {
                        email = config[0].Mail_Info;
                    }
                    if (config[0].PlaceHead != null && config[0].PlaceHead != "")
                    {
                        chuoifbg += "<div class=\"fb-like\" style=\"float: left; margin-right: 25px\" data-href=\"" + config[0].PlaceHead + "\" data-layout=\"button_count\" data-action=\"like\" data-show-faces=\"true\" data-share=\"true\"></div>";
                    }
                    else
                    {
                        chuoifbg += "<div class=\"fb-like\" style=\"float: left; margin-right: 25px\" data-href=\"https://www.facebook.com/groups/nghiadongmotgoctroique/\" data-layout=\"button_count\" data-action=\"like\" data-show-faces=\"true\" data-share=\"true\"></div>";
                        chuoifbg += "<div class=\"title_face\">Theo dõi nghiadong.gov.vn qua FB, G+:</div>";
                    }
                    if (config[0].Contact != null && config[0].Contact != "")
                    {
                        chuoifbg += "<div style=\"float: left;\" class=\"plugright\"><g:plusone size=\"medium\" href=\"" + config[0].Contact + "\"></g:plusone></div>";
                    }
                    else
                    {
                        chuoifbg += "<div style=\"float: left;\" class=\"plugright\"><g:plusone size=\"medium\" href=\"http://onsoft.vn/\"></g:plusone></div>";
                    }
                    ViewBag.fbg = chuoifbg;
                }
                string chuoifb = "";
                string ulrweb = Request.Url.ToString();
                chuoifb += "<div class=\"fb-like\" style=\"float: left; margin-right: 25px\" data-href=\"" + ulrweb + "\" data-layout=\"button_count\" data-action=\"like\" data-show-faces=\"true\" data-share=\"true\"></div>";
                chuoifb += "<div style=\"float: left;\" class=\"plugright\"><g:plusone size=\"medium\" href=\"" + ulrweb + "\"></g:plusone></div>";
                ViewBag.fbitem = chuoifb;
                #endregion
                #region[View thông tin chi tiết tin]
                chuoi += "<div class=\"news-detail-head\">";
                chuoi += "<h1>" + list.Name + "</h1>";
                chuoi += "<div class=\"date\">" + chuoifb + "</div>";
                chuoi += "</div>";
                chuoi += "<div class=\"news-detail-content\"><p>" + list.Content + "</p></div>";
                chuoi += "<div class=\"news-detail-detail\"><p>" + list.Detail + "</p>";
                chuoi += "<div class=\"messenger_single\"><span class=\"icon_bt\" style=\"font-family: &quot;FontAwesome&quot;;\"></span>Bài viết, video, hình ảnh đóng góp cho chuyên mục vui lòng gửi về <br /> <a href=\"mailto:" + email + "?subject=Bài viết: " + list.Name + "\">" + email + "</a></div>";
                #endregion
                #region[Title, Keyword, Deskription]
                if (list.Title.Length > 0) { ViewBag.tit = list.Title; } else { ViewBag.tit = list.Name; }
                if (list.Description.Length > 0) { ViewBag.des = list.Description; } else { ViewBag.des = list.Name; }
                if (list.Keyword.Length > 0) { ViewBag.key = list.Keyword; } else { ViewBag.key = list.Name; }
                #endregion
            }
            ViewBag.newsdetail = chuoi;
            list = null;
            return View();
        }
    }
}
