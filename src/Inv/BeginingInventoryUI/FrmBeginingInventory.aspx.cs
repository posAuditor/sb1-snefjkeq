using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Inv_BeginingInventoryUI_FrmBeginingInventory : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    public int TypeTax
    {
        get
        {
            if (Session["TypeTax_Invoice" + this.WinID] == null)
            {
                // Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetails_Select(null).CopyToDataTable();
                Session["TypeTax_Invoice" + this.WinID] = dc.usp_Company_Select().FirstOrDefault().TypeTax;
            }
            return (int)Session["TypeTax_Invoice" + this.WinID];
        }

        set
        {
            Session["TypeTax_Invoice" + this.WinID] = value;
        }
    }    
    
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!Page.IsPostBack)
        {
            this.LoadControls();
        }

    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }   

    public string GetOppositeAccountContextKey()
    {
        string acOppositeAccountContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,," + COA.Capital.ToInt().ToExpressString() + ",true";
        return acOppositeAccountContextKey;
    }   
     
    public string GeneralAttributesUOM()
    {
        string ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.UOM.ToInt().ToExpressString();
        return ContextKey;
    }

    private void LoadControls()
    {
        var currency = dc.usp_Currency_Select(false).ToList();
        ddlCurrency.DataSource = currency;
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();

        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
    }   
}