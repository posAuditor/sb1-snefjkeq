using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

public partial class Production_Expenses : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtProductionOrderExpenses
    {
        get
        {
            return (DataTable)Session["dtProductionOrderExpenses" + this.WinID];
        }

        set
        {
            Session["dtProductionOrderExpenses" + this.WinID] = value;
        }
    }

    private int ProductionOrder_ID
    {
        get
        {
            if (ViewState["ProductionOrder_ID"] == null) return 0;
            return (int)ViewState["ProductionOrder_ID"];
        }

        set
        {
            ViewState["ProductionOrder_ID"] = value;
        }
    }

    private int EditID
    {
        get
        {
            if (ViewState["EditID"] == null) return 0;
            return (int)ViewState["EditID"];
        }

        set
        {
            ViewState["EditID"] = value;
        }
    }

    private int? Branch_ID
    {
        get
        {
            return (int?)ViewState["Branch_ID"];
        }

        set
        {
            ViewState["Branch_ID"] = value;
        }
    }

    private bool PopupOpen
    {
        get
        {
            if (ViewState["PopupOpen"] == null) return false;
            return (bool)ViewState["PopupOpen"];
        }

        set
        {
            ViewState["PopupOpen"] = value;
        }
    }

    #endregion

    #region Page events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                this.CheckSecurity();
                this.LoadControls();
                this.Fill();
            }
            if (this.PopupOpen) mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Control Events

    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            this.PopupOpen = true;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ucNewExpenseName_NewAttributeCreated(string AttName, int AttID)
    {
        try
        {
            acExpense.Refresh();
            acExpense.Value = AttID.ToExpressString();
            txtAmount.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvExpenses.PageIndex = e.NewPageIndex;
            gvExpenses.DataSource = this.dtProductionOrderExpenses;
            gvExpenses.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExpenses_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtProductionOrderExpenses.Select("ID=" + gvExpenses.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            acExpense.Value = dr["Expense_ID"].ToExpressString();
            acOppositeAccount.Value = dr["OppositeAccount_ID"].ToExpressString();
            txtAmount.Text = dr["ExpenseValue"].ToExpressString();
            txtDate.Text = dr["Operationdate"].ToDate().Value.ToString("d/M/yyyy");
            this.FilterByBranchAndCurrency();
            txtNotes.Text = dr["Notes"].ToExpressString();
            this.EditID = gvExpenses.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            this.PopupOpen = true;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_ProductionOrderExpenses_Delete(gvExpenses.DataKeys[e.RowIndex]["ID"].ToInt());
            LogAction(Actions.Delete, gvExpenses.DataKeys[e.RowIndex]["ExpenseName"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            this.EditID = 0;
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateNew.Show();
                this.PopupOpen = true;
            }
            else
            {
                mpeCreateNew.Hide();
                this.PopupOpen = false;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSave_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (this.Save(false, trans))
                trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnApprove_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (this.Save(true, trans))
                trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            this.PopupOpen = true;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }



    #endregion

    #region Private Methods

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        int result = 0;

        if (txtDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        if (this.EditID == 0) //insert
        {
            result = dc.usp_ProductionOrderExpenses_Insert(this.ProductionOrder_ID, txtDate.Text.ToDate(), acExpense.Value.ToInt(), txtAmount.Text.ToDecimal(), txtNotes.Text, acOppositeAccount.Value.ToInt());
            LogAction(IsApproving ? Actions.Approve : Actions.Add, acExpense.Text, dc);
        }
        else
        {
            result = dc.usp_ProductionOrderExpenses_Update(this.EditID, txtDate.Text.ToDate(), acExpense.Value.ToInt(), txtAmount.Text.ToDecimal(), txtNotes.Text, acOppositeAccount.Value.ToInt());
            LogAction(IsApproving ? Actions.Approve : Actions.Edit, acExpense.Text, dc);
        }

        this.Fill();
        this.ClosePopup_Click(null, null);
        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        return true;
    }

    private void ClearForm()
    {
        acExpense.Clear();
        txtDate.Clear();
        txtAmount.Clear();
        txtNotes.Clear();
        acOppositeAccount.Clear();
    }

    private void LoadControls()
    {
        var company = dc.usp_Company_Select().FirstOrDefault();
        this.ProductionOrder_ID = Request["ProductionOrder_ID"].ToInt();
        var Order = dc.usp_ProductionOrder_SelectByID(this.ProductionOrder_ID).FirstOrDefault();
        this.Branch_ID = Order.Branch_ID;
        acExpense.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.ProductionOrderExpenses.ToInt();
        acOppositeAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + this.Branch_ID + "," + company.Currency_ID + ",,true";
        lblOrderNumber.Text = Order.Serial;
        this.FilterByBranchAndCurrency();
        if (Order.IsReceived.Value)
        {
            lnkAddNew.Visible = false;
            gvExpenses.Columns[5].Visible = false;
            gvExpenses.Columns[6].Visible = false;
        }
    }

    private void Fill()
    {
        this.dtProductionOrderExpenses = dc.usp_ProductionOrderExpenses_Select(this.ProductionOrder_ID).CopyToDataTable();
        gvExpenses.DataSource = this.dtProductionOrderExpenses;
        gvExpenses.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvExpenses.Columns[5].Visible = MyContext.PageData.IsEdit;
        gvExpenses.Columns[6].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
    }

    private void FilterByBranchAndCurrency()
    {
        try
        {
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion
}