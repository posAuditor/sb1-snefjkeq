using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Main_Print : UICulturePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ifViewer.Attributes.Add("src", Request["File"].ToStringOrEmpty());
        lnkBack.NavigateUrl = Request.UrlReferrer.ToString();
    }
}