using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class ProColorInfo	{		#region[Declare variables]		private string  _Id;		private string  _IdPro;		private string  _IdColor;		private string  _SpTon;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string IdPro{ get { return _IdPro; } set { _IdPro = value; } }		public string IdColor{ get { return _IdColor; } set { _IdColor = value; } }		public string SpTon{ get { return _SpTon; } set { _SpTon = value; } }		#endregion			}}