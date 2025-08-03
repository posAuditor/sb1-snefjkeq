using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Purchases_PurchaseOrderUI_FrmPurchaseOrder : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();  
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

    public string GetCurrentCulture()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString();
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

        var listSos = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList().Select(c => new PaymentMethodeCls { Name = c.Name, ID = c.ID }).ToList();
        listSos.Add(new PaymentMethodeCls { Name = "اجـل", ID = -1 });
        listSos.Add(new PaymentMethodeCls { Name = "نقدي", ID = 0 });
        ddlPaymentMethod.DataSource = listSos.OrderBy(c => c.ID).ToList();
        ddlPaymentMethod.DataTextField = "Name";
        ddlPaymentMethod.DataValueField = "ID";
        ddlPaymentMethod.DataBind();

        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
    }

    public MyContext GetContext()
    {
        var con = new MyContext(System.Web.Security.Membership.GetUser(), PageLinks.Customers, string.Empty);
        return con;
    }
}