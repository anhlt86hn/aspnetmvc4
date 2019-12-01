using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;
using PagedList.Mvc;

namespace MODEOUTLED.Controllers.NewsView
{
    public class NewsViewController : Controller
    {
        tuan_nanyspa_vn_dbEntities data = new tuan_nanyspa_vn_dbEntities();
        #region[Danh sách tin tức]
        public ActionResult List()
        {
            string chuoi = "";
            string chuoilist = "";
            string chuoig = "";
            string chuoigitem = "";
            var list = (from p in data.News where p.Active == true && p.Index == true && p.Image.Length > 0 orderby p.Id descending select p).Take(15).ToList();
            #region[Title, Keyword, Deskription]
            var listconfig = (from p in data.Configs select p).ToList();
            if (listconfig.Count > 0) { ViewBag.tit = listconfig[0].Title; ViewBag.des = listconfig[0].Description; ViewBag.key = listconfig[0].Keyword; }
            listconfig.Clear();
            listconfig = null;
            #endregion
            list = list.OrderByDescending(p => p.Id).ToList();
            if (list.Count > 0)
            {
                chuoi += "<div class=\"news-home-left-img\"><a href=\"/nghia-dong/" + list[0].Tag + "\" title=\"" + list[0].Name + "\"><img src=\"" + list[0].Image + "\" alt=\"" + list[0].Name + "\"/></a></div>";
                chuoi += "<a href=\"/nghia-dong/" + list[0].Tag + "\" title=\"" + list[0].Name + "\"><h2>" + list[0].Name + "</h2></a>";
                for (int i = 1; i < list.Count; i++)
                {
                    chuoilist += "<li>";
                    chuoilist += "<a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\"/></a>";
                    chuoilist += "<a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><h3>" + list[i].Name + "</h3></a>";
                    chuoilist += "</li>";
                }
                ViewBag.top1 = chuoi;
                ViewBag.top4 = chuoilist;
            }
            var listg = (from g in data.v_GroupNewsInNews where g.Index == true && g.Active == true orderby g.Level select g).ToList();
            for (int j = 0; j < listg.Count; j++)
            {
                int gid = listg[j].Id;
                chuoig += "<div class=\"news-home-mid\">";
                chuoig += "<div class=\"head\"><a href=\"/danh-muc-tin/" + listg[j].Tag + "\" title=\"" + listg[j].Name + "\"><h2>" + listg[j].Name + "</h2></a></div>";
                var listitem = (from p in data.News where p.Active == true && p.Index == true && p.IdGroup == gid orderby p.Id descending select p).Take(8).ToList();
                for (int i = 0; i < listitem.Count; i++)
                {
                    if (i == 0)
                    {
                        chuoig += "<div class=\"news-home-mid-top-left\">";
                        chuoig += "<div class=\"news-home-mid-top-left-img\"><a href=\"/nghia-dong/" + listitem[i].Tag + "\" title=\"" + listitem[i].Name + "\"><img src=\"" + listitem[i].Image + "\" alt=\"" + listitem[i].Name + "\"/></a></div>";
                        chuoig += "<a href=\"/nghia-dong/" + listitem[i].Tag + "\" title=\"" + listitem[i].Name + "\"><h2>" + listitem[i].Name + "</h2></a>";
                        chuoig += "<p>" + listitem[i].Content + "</p>";
                        chuoig += "</div>";
                    }
                    else if (i == 1)
                    {
                        chuoig += "<div class=\"news-home-mid-top-right\">";
                        chuoig += "<div class=\"news-home-mid-top-right-img\"><a href=\"/nghia-dong/" + listitem[i].Tag + "\" title=\"" + listitem[i].Name + "\"><img src=\"" + listitem[i].Image + "\" alt=\"" + listitem[i].Name + "\"/></a></div>";
                        chuoig += "<a href=\"/nghia-dong/" + listitem[i].Tag + "\" title=\"" + listitem[i].Name + "\"><h2>" + listitem[i].Name + "</h2></a>";
                        chuoig += "<p>" + listitem[i].Content + "</p>";
                        chuoig += "</div>";
                    }

                    if (i > 1)
                    {
                        if (i == 2)
                        {
                            chuoig += "</div>";
                            chuoig += "<div class=\"news-home-bottom\"><ul>";
                            chuoig += "<li><a href=\"/nghia-dong/" + listitem[i].Tag + "\" title=\"" + listitem[i].Name + "\"><h3>" + listitem[i].Name + "</h3></a></li>";
                        }
                        else
                        {
                            chuoig += "<li><a href=\"/nghia-dong/" + listitem[i].Tag + "\" title=\"" + listitem[i].Name + "\"><h3>" + listitem[i].Name + "</h3></a></li>";
                        }
                        if (i == listitem.Count - 1)
                        {
                            chuoig += "</ul></div>";
                        }
                    }
                }
                if (listitem.Count < 3)
                {
                    chuoig += "</div>";
                }
                listitem.Clear();
                listitem = null;
            }
            ViewBag.chuoig = chuoig;
            return View();
            list.Clear();
            list = null;
            listg.Clear();
            listg = null;
        }
        #endregion
        #region[Danh sách tin theo nhóm]
        public ActionResult ListNews(int? page,string tag)
        {
            if (Request.HttpMethod == "GET")
            {
            }
            else
            {
                page = 1;
            }

            #region[Phân trang]
            int pageSize = 15;
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
            var g = data.GroupNews.Where(m => m.Tag == tag).SingleOrDefault();
            if (g != null)
            {
                ViewBag.link = "<li class='Last'><span>" + g.Name + "</span></li>";
                int gid = g.Id;
                var list = (from n in data.News where n.Active == true && n.IdGroup==gid orderby n.Id descending select n).ToList();
                string chuoi = "";
                string chuoi1 = "";
                string chuoi2 = "";
                chuoi += "<div class=\"head\"><a href=\"/danh-muc-tin/"+ g.Tag +"\"><h2>"+ g.Name +"</h2></a></div>";
                for (int i = 0; i < list.ToPagedList(pageNumber, pageSize).Count; i++)
                {
                    if (i == 0)
                    {
                        chuoi+="<div class=\"news-home-mid-top-left\">";
                        chuoi += "<div class=\"news-home-mid-top-left-img\"><a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\"/></a></div>";
                        chuoi += "<a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><h2>"+ list[i].Name +"</h2></a>";
                            chuoi+="<p>"+ list[i].Content +"</p>";
                        chuoi+="</div>";
                    }
                    if (i == 1)
                    {
                        chuoi += "<div class=\"news-home-mid-top-right\">";
                        chuoi += "<div class=\"news-home-mid-top-right-img\"><a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\"/></a></div>";
                        chuoi += "<a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><h2>" + list[i].Name + "</h2></a>";
                        chuoi += "<p>" + list[i].Content + "</p>";
                        chuoi += "</div>";
                    }
                    if (i > 1 && i < 8)
                    {
                         chuoi1 += "<li>";
                         chuoi1 += "<a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><h2>" + list[i].Name + "</h2></a>";
                         chuoi1 += "<a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><img src=\"" + list[i].Image + "\" /></a>";
                         chuoi1 += "<p>" + list[i].Content + "</p>";
                        chuoi1 += "</li>";
                    }
                    if (i > 7)
                    {
                        chuoi2 += "<li><a href=\"/nghia-dong/" + list[i].Tag + "\" title=\"" + list[i].Name + "\"><h3>" + list[i].Name + "</h3></a></li>";
                    }
                }
                ViewBag.chuoi = chuoi;
                ViewBag.chuoi1 = chuoi1;
                ViewBag.chuoi2 = chuoi2;
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
        #endregion
        #region[Chi tiết tin]
        public ActionResult NewsDetail(string tag)
        {
            string chuoi = "";
            string chuoilink = "";
            int IdGroup = 0;
            int NewId = 0;
            var news = (from n in data.News where n.Tag == tag select n).ToList();
            if (news.Count > 0)
            {
                NewId = news[0].Id;
                IdGroup = news[0].IdGroup;
                var Group = (from g in data.GroupNews where g.Id == IdGroup select g).ToList();
                if (Group.Count > 0)
                {
                    chuoilink = "<li class='Last'><a href='/danh-muc-tin/" + Group[0].Tag + "'>" + Group[0].Name + "</a></li>";
                }
                
                ViewBag.link = chuoilink;
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
                chuoi += "<h1>"+ news[0].Name +"</h1>";
                chuoi += "<div class=\"date\">"+ DateTimeClass.ConvertDateTimeddMMyyHHmm(news[0].SDate) +" (GMT+7) " + chuoifb + "</div>";
                chuoi += "</div>";
                chuoi += "<div class=\"news-detail-content\">"+ news[0].Content +"</div>";
                chuoi += "<div class=\"news-detail-detail\">" + news[0].Detail + "";
                chuoi += "<div class=\"messenger_single\"><span class=\"icon_bt\" style=\"font-family: &quot;FontAwesome&quot;;\"></span>Bài viết, video, hình ảnh đóng góp cho chuyên mục vui lòng gửi về <br /> <a href=\"mailto:" + email + "?subject=Bài viết: " + news[0].Name + "\">" + email + "</a></div>";
                #endregion
                
                #region[Title, Keyword, Deskription]
                string t = news[0].Title;
                string tt = news[0].Description;
                string ttt = news[0].Keyword;
                if (news[0].Title != null && news[0].Title.Length > 0) { ViewBag.tit = news[0].Title; } else { ViewBag.tit = news[0].Name; }
                if (news[0].Description != null && news[0].Description.Length > 0) { ViewBag.des = news[0].Description; } else { ViewBag.des = news[0].Name; }
                if (news[0].Keyword != null && news[0].Keyword.Length > 0) { ViewBag.key = news[0].Keyword; } else { ViewBag.key = news[0].Name; }
                #endregion
                #region[Tin khác]
                var newsother = (from n in data.News where n.IdGroup == IdGroup && n.Id != NewId select n).Take(10).ToList();
                string chuoiothertop = "";
                string chuoiotherbot = "";
                for (int i = 0; i < newsother.Count; i++)
                {
                    if (i < 4)
                    {
                        chuoiothertop += "<li>";
                        chuoiothertop += "<a href=\"/nghia-dong/" + newsother[i].Tag + "\" title=\"" + newsother[i].Image + "\"><h2>" + newsother[i].Name + "</h2></a>";
                        chuoiothertop += "<img src=\"" + newsother[i].Image + "\" alt=\"" + newsother[i].Image + "\"/>";
                        chuoiothertop += "<p>" + newsother[i].Content + "</p>";
                        chuoiothertop += "</li>";
                    }
                    else
                    {
                        chuoiotherbot += "<li><a href=\"/nghia-dong/" + newsother[i].Tag + "\" title=\"" + newsother[i].Image + "\"><h3>" + newsother[i].Name + "</h3></a></li>";
                    }
                }
                ViewBag.othertop = chuoiothertop;
                ViewBag.otherbot = chuoiotherbot;
                #endregion
                newsother.Clear();
                newsother = null;
                Group.Clear();
                Group = null;
            }
            ViewBag.newsdetail = chuoi;
            news.Clear();
            news = null;
            return View();
        }
        #endregion
    }
}
