using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;

public partial class Contacts_CustomersList : UICulturePage
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
                gvCutomersList.Columns[6].Visible = this.MyContext.PageData.IsViewDoc;
                gvCutomersList.Columns[7].Visible = this.MyContext.PageData.IsDelete;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
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

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillCustomersList();
            if (acBranch.Enabled) acBranch.AutoCompleteFocus(); else ddlCurrency.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSrch_Click(object sender, EventArgs e)
    {
        try
        {
            txtAccountNumber.Clear();
            ddlCurrency.SelectedIndex = 0;
            txtSerialsrch.Clear();
            acArea.Clear();
            acName.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            this.FilterCustomers(null, null);
            this.FillCustomersList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterCustomers(object sender, EventArgs e)
    {
        try
        {
            acName.ContextKey = "C," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + "," + acArea.Value;
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvCutomersList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCutomersList.PageIndex = e.NewPageIndex;
            gvCutomersList.DataSource = this.dtCustomersList;
            gvCutomersList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvCutomersList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvCutomersList.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvCutomersList.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.FillCustomersList();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillCustomersList()
    {
        //int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        //this.dtCustomersList = dc.usp_CustomersList_Select(acBranch.Value.ToNullableInt(), Currency_ID, acArea.Value.ToNullableInt(), txtSerialsrch.TrimmedText, acName.Text, txtAccountNumber.TrimmedText).CopyToDataTable();
        //gvCutomersList.DataSource = this.dtCustomersList;
        //gvCutomersList.DataBind();


        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        var comp = dc.CompanySettings.FirstOrDefault();

        if (comp.IsCustViewInAllBranch.ToBooleanOrDefault())
        {
            this.dtCustomersList = dc.usp_CustomersList_Select(null, Currency_ID, acArea.Value.ToNullableInt(), txtSerialsrch.TrimmedText, acName.Text, txtAccountNumber.TrimmedText).CopyToDataTable();

        }
        else
        {
            this.dtCustomersList = dc.usp_CustomersList_Select(acBranch.Value.ToNullableInt(), Currency_ID, acArea.Value.ToNullableInt(), txtSerialsrch.TrimmedText, acName.Text, txtAccountNumber.TrimmedText).CopyToDataTable();

        }
        gvCutomersList.DataSource = this.dtCustomersList;
        gvCutomersList.DataBind();




    }

    private void LoadControls()
    {
        acArea.ContextKey = string.Empty;
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        ddlCurrency.Items.Insert(0, new ListItem(Resources.Labels.Select, "-1"));
        this.FilterCustomers(null, null);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvCutomersList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
        acArea.Visible = MyContext.Features.AreasEnabled;
        
    }

    #endregion

}