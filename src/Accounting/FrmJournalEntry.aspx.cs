using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Accounting_FrmJournalEntry : UICulturePage
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

    public string GetCostCenterContextKey()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false,";
    }

    public string GetCostomerContextKey()
    {
        string acCustomer = string.Empty;
        if (!MyContext.UserProfile.SalesRepToCustomer.ToBooleanOrDefault())
        {
            acCustomer = ddlCurrency.SelectedValue + ",";
        }
        else acCustomer = ddlCurrency.SelectedValue + "," + MyContext.UserProfile.Area_ID.ToExpressString();
        return acCustomer;
    }

    public string GetAcAddressContextKey()
    {
        string acAddress = ContactDetailsTypes.ShipAddress.ToInt().ToExpressString();
        return acAddress;
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