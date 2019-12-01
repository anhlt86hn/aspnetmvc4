using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Globalization;
using MODEOUTLED.ViewModels;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Net;
using System.IO;
using PagedList;
using PagedList.Mvc;

namespace onsoft.Controllers
{
    public class HomeController : Controller
    {
        tuan_nanyspa_vn_dbEntities data = new tuan_nanyspa_vn_dbEntities();
        public ActionResult Index(int? page, string ProName, string currentProName)
        {
            string chuoi = "";
            string chuoilist = "";
            string chuoig = "";
            string chuoigitem = "";
            if (Session["CurrentProName"] != null)
            {
                currentProName = Session["CurrentProName"].ToString();
                Session["CurrentProName"] = null;
            }
            var list = (from p in data.News where p.Active == true && p.Index==true && p.Image.Length>0 orderby p.Id descending select p).Take(5).ToList();
            #region[Title, Keyword, Deskription]
            var listconfig = (from p in data.Configs select p).ToList();
            if (listconfig.Count > 0) { ViewBag.tit = listconfig[0].Title; ViewBag.des = listconfig[0].Description; ViewBag.key = listconfig[0].Keyword; }
            listconfig.Clear();
            listconfig = null;
            #endregion
            if (!String.IsNullOrEmpty(ProName))
            {
                list = list.Where(p => p.Name.ToUpper().Contains(ProName.ToUpper())).OrderByDescending(p => p.Id).ToList();
            }
            list = list.OrderByDescending(p => p.Id).ToList();
            if (list.Count > 0)
            {
                chuoi += "<div class=\"news-home-left-img\"><a href=\"/nghia-dong/" + list[0].Tag + "\" title=\"" + list[0].Name + "\"><img src=\"" + list[0].Image + "\" alt=\"" + list[0].Name + "\"/></a></div>";
                chuoi += "<a href=\"/nghia-dong/" + list[0].Tag + "\" title=\"" + list[0].Name + "\"><h2>" + list[0].Name + "</h2></a>";
                for (int i = 1; i < list.Count; i++)
                {
                    chuoilist+="<li>";
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
                chuoig += "<div class=\"head\"><a href=\"/danh-muc-tin/"+ listg[j].Tag +"\" title=\"" + listg[j].Name + "\"><h2>" + listg[j].Name + "</h2></a></div>";
                var listitem = (from p in data.News where p.Active == true && p.Index == true && p.IdGroup==gid orderby p.Id descending select p).Take(8).ToList();
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
    }
}