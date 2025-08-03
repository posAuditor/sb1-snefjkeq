using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Production_ProductionOrdersList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtProductionOrdersList
    {
        get
        {
            return (DataTable)Session["dtProductionOrdersList" + this.WinID];
        }

        set
        {
            Session["dtProductionOrdersList" + this.WinID] = value;
        }
    }



    #endregion

    #region ViewState
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

    private int? Bracnh_ID
    {
        get
        {
            return (int?)ViewState["Bracnh_ID"];
        }

        set
        {
            ViewState["Bracnh_ID"] = value;
        }
    }

    private decimal IncomingCost
    {
        get
        {
            if (ViewState["IncomingCost"] == null) return 0;
            return (decimal)ViewState["IncomingCost"];
        }

        set
        {
            ViewState["IncomingCost"] = value;
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
                gvProductionOrdersList.Columns[7].Visible = this.MyContext.PageData.IsViewDoc;
                gvProductionOrdersList.Columns[8].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillProductionOrdersList();
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

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            txtReceiveDate.Clear();
            txtQuantity.Clear();
            acStore.Clear();
            mpeReceive.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void lnkReceive_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            this.ProductionOrder_ID = gvProductionOrdersList.DataKeys[Index]["ID"].ToInt();
            var Order = dc.usp_ProductionOrder_SelectByID(this.ProductionOrder_ID).FirstOrDefault();
            lblProductionOrderSerial.Text = Order.Serial;
            txtReceiveDate.Text = DateTime.Now.Date.ToString("d/M/yyyy");
            txtQuantity.Text = Order.Quantity.ToExpressString();
            acStore.ContextKey = string.Empty + Order.Branch_ID;
            this.Bracnh_ID = Order.Branch_ID;
            mpeReceive.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnOkReceive_click(object sender, EventArgs e)
    {
        try
        {
            System.Data.Common.DbTransaction trans;
            dc.Connection.Open();
            trans = dc.Connection.BeginTransaction();
            dc.Transaction = trans;
            try
            {
                this.IncomingCost = 0;
                if (txtReceiveDate.Text.ToDate() > DateTime.Now.Date)
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                    trans.Rollback();
                    mpeReceive.Show();
                    return;
                }
                if (!this.InsertICJ())
                {
                    trans.Rollback();
                    return;
                }
                dc.usp_ICJ_NegativeRefill(txtReceiveDate.Text.ToDate(), this.Bracnh_ID, DocumentsTableTypes.ProductionOrder.ToInt(), this.ProductionOrder_ID);
                this.InsertOperation();
                dc.usp_ProductionOrderExtraData_Update(this.ProductionOrder_ID, null, null, true, txtReceiveDate.Text.ToDate(), acStore.Value.ToInt());
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
                trans.Commit();
                this.ClosePopup_Click(null, null);
                this.FillProductionOrdersList();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillProductionOrdersList();
            ddlStatus.Focus();
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
            txtDateFromSrch.Clear();
            txtDateToSrch.Clear();
            txtSerialsrch.Clear();
            txtUserRefNo.Clear();
            ddlStatus.SelectedIndex = 0;
            acItem.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            this.FillProductionOrdersList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvProductionOrdersList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvProductionOrdersList.PageIndex = e.NewPageIndex;
            gvProductionOrdersList.DataSource = this.dtProductionOrdersList;
            gvProductionOrdersList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvProductionOrdersList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvProductionOrdersList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int InvDoc_ID = gvProductionOrdersList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvProductionOrdersList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\ProductionOrder_Print.rpt"));
            doc.SetParameterValue("@ID", InvDoc_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, "ProductionOrder"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void InsertOperation()
    {
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        var Expenses = dc.usp_ProductionOrderExpenses_Select(this.ProductionOrder_ID).ToList();

        int Result = dc.usp_Operation_Insert(this.Bracnh_ID, txtReceiveDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.ReceiveFinalProduct.ToInt(), company.Currency_ID, this.IncomingCost, this.IncomingCost, 1, string.Empty);
        //المخزون مدين
        dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, this.IncomingCost, 0, this.IncomingCost, 0, null, this.ProductionOrder_ID, DocumentsTableTypes.ProductionOrder.ToInt());
        //الحساب المقابل
        dc.usp_OperationDetails_Insert(Result, COA.RawMaterialExpenses.ToInt(), 0, this.IncomingCost, 0, this.IncomingCost, null, this.ProductionOrder_ID, DocumentsTableTypes.ProductionOrder.ToInt());

        foreach (var exp in Expenses)
        {
            Result = dc.usp_Operation_Insert(this.Bracnh_ID, exp.OperationDate, ref serial, DocStatus.Approved.ToByte(), OperationTypes.ProductionExpenses.ToInt(), company.Currency_ID, exp.ExpenseValue, exp.ExpenseValue, 1, exp.Notes);
            //حساب المخزون
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, exp.ExpenseValue, 0, exp.ExpenseValue, 0, null, exp.ID, DocumentsTableTypes.ProductionOrderExpenses.ToInt());
            //الحساب المقابل
            dc.usp_OperationDetails_Insert(Result, exp.OppositeAccount_ID, 0, exp.ExpenseValue, 0, exp.ExpenseValue, null, exp.ID, DocumentsTableTypes.ProductionOrderExpenses.ToInt());
        }
    }

    private bool InsertICJ()
    {
        decimal? Cost = 0;
        int result = 0;

        result = dc.usp_ICJ_ProductionOrder_IN(txtReceiveDate.Text.ToDate(), txtQuantity.Text.ToDecimal(), null, null, ref Cost, null, acStore.Value.ToInt(), this.Bracnh_ID, DocumentsTableTypes.ProductionOrder.ToInt(), this.ProductionOrder_ID, null, null);
        this.IncomingCost += Cost ?? 0;
        if (result == -6)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CantReceiveFinalItem, string.Empty);
            return false;
        }
        if (result == -666)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.VeryLowCost, string.Empty);
            return false;
        }
        return true;
    }

    private void FillProductionOrdersList()
    {
        lnkadd.NavigateUrl = PageLinks.ProductionOrder;
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        this.dtProductionOrdersList = dc.usp_ProductionOrder_Select(acBranch.Value.ToNullableInt(), acItem.Value.ToNullableInt(), txtSerialsrch.TrimmedText, txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte()).CopyToDataTable();
        gvProductionOrdersList.DataSource = this.dtProductionOrdersList;
        gvProductionOrdersList.DataBind();
    }

    private void LoadControls()
    {
        acItem.ContextKey = "c,,";
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvProductionOrdersList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

}