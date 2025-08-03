using ASP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
public partial class Inv_ItemUI_FrmItemMain : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();    

    public bool GetEnableEInvoice()
    {
        bool EnableEInvoice = false;
        var comp = dc.usp_Company_Select().First();
        if(comp!=null && comp.EnableEInvoice!=null)
        EnableEInvoice = comp.EnableEInvoice.Value;
        return EnableEInvoice;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ddlItemTypeItemeCard.SelectedIndex = 0; 
        }
    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }

    public string GeneralAttributesUOM()
    {
        string ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.UOM.ToInt().ToExpressString();               
        return ContextKey;
    }

    public string GetAccountRelatedContext()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,,,true";
    }

    public string GetPriceNameContextKey()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Prices.ToInt().ToExpressString();        
    }

    public string GetUOMContextKey()
    {        
        return this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.UOM.ToInt().ToExpressString(); ;
    }
}