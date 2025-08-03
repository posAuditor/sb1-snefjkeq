using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;

public partial class Contacts_GenerateVendorNotHaveCard : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtCustomersList
    {
        get
        {
            return (DataTable)Session["dtCustomersList" + this.WinID];
        }

        set
        {
            Session["dtCustomersList" + this.WinID] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);


                this.LoadControls();
                this.FillCustomersList();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events

    #endregion

    #region Private Methods

    private void FillCustomersList()
    {

        this.dtCustomersList = dc.GetVendorNotHaveCard().CopyToDataTable();
        gvCutomersList.DataSource = this.dtCustomersList;
        gvCutomersList.DataBind();
    }

    private void LoadControls()
    {

    }

    private void CustomPage()
    {


    }

    #endregion

    protected void lnk_Click(object sender, EventArgs e)
    {

        var ChartofAccount_ID = ((sender as LinkButton).CommandArgument).ToIntOrDefault();
        var coa = dc.ChartOfAccounts.Where(x => x.ID == ChartofAccount_ID).FirstOrDefault();
        var Contact_ID = dc.usp_ContactWithTax_Insert(coa.Branch_ID, coa.Currency_ID, DocSerials.Customer.ToInt(), coa.Name, 'V',"", "", "",
            null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

        dc.usp_Vendors_Insert(Contact_ID, ChartofAccount_ID, null, null, null);
        this.FillCustomersList();

    }
}