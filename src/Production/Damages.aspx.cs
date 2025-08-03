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

public partial class Production_Damages : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtProductionOrderDamages
    {
        get
        {
            return (DataTable)Session["dtProductionOrderDamages" + this.WinID];
        }

        set
        {
            Session["dtProductionOrderDamages" + this.WinID] = value;
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

    private decimal CalculatedSalesCost
    {
        get
        {
            if (ViewState["CalculatedSalesCost"] == null) return 0;
            return (decimal)ViewState["CalculatedSalesCost"];
        }

        set
        {
            ViewState["CalculatedSalesCost"] = value;
        }
    }

    private decimal ReturnCalculatedSalesCost
    {
        get
        {
            if (ViewState["ReturnCalculatedSalesCost"] == null) return 0;
            return (decimal)ViewState["ReturnCalculatedSalesCost"];
        }

        set
        {
            ViewState["ReturnCalculatedSalesCost"] = value;
        }
    }

    private DateTime OperationDate
    {
        get
        {
            if (ViewState["OperationDate"] == null) return DateTime.Now.Date;
            return (DateTime)ViewState["OperationDate"];
        }

        set
        {
            ViewState["OperationDate"] = value;
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
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Control Events

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




    #endregion

    #region Private Methods

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        int Detail_ID = 0;
        decimal DamageQty = 0;
        DataRow r = null;

        this.CalculatedSalesCost = 0;
        this.ReturnCalculatedSalesCost = 0;

        foreach (GridViewRow gvRow in gvItems.Rows)
        {
            DamageQty = ((TextBox)gvRow.FindControl("txtDamageQty")).Text.ToDecimalOrDefault();

            Detail_ID = gvItems.DataKeys[gvRow.RowIndex]["ID"].ToInt();
            dc.usp_ProductionOrderDetails_Update(Detail_ID, null, null, null, null, DamageQty, null);

            if (IsApproving && DamageQty != 0)
            {
                r = this.dtProductionOrderDamages.Select("ID=" + Detail_ID.ToExpressString())[0];
                if (!this.InsertICJ(Detail_ID, r, DamageQty))
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
        if (IsApproving) dc.usp_ICJ_NegativeRefill(this.OperationDate, this.Branch_ID, DocumentsTableTypes.ProductionOrder.ToInt(), this.ProductionOrder_ID);
        if (IsApproving) this.InsertOperation();
        if (IsApproving) dc.usp_ProductionOrderExtraData_Update(this.ProductionOrder_ID, null, this.CalculatedSalesCost - this.ReturnCalculatedSalesCost, null, null, null);

        LogAction(IsApproving ? Actions.Approve : Actions.Edit, lblOrderNumber.Text, dc);
        this.Fill();
        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.ProductionOrderList);
        return true;
    }

    private void InsertOperation()
    {
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();

        if (this.CalculatedSalesCost > 0)
        {
            int Result = dc.usp_Operation_Insert(this.Branch_ID, this.OperationDate, ref serial, DocStatus.Approved.ToByte(), OperationTypes.DamagedItemsMaterialsOut.ToInt(), company.Currency_ID, this.CalculatedSalesCost, this.CalculatedSalesCost, 1, string.Empty);

            //المخزون دائن
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, 0, this.CalculatedSalesCost, 0, this.CalculatedSalesCost, null, this.ProductionOrder_ID, DocumentsTableTypes.ProductionOrder.ToInt());

            //الحساب المقابل
            dc.usp_OperationDetails_Insert(Result, COA.RawMaterialExpenses.ToInt(), this.CalculatedSalesCost, 0, this.CalculatedSalesCost, 0, null, this.ProductionOrder_ID, DocumentsTableTypes.ProductionOrder.ToInt());
        }
        if (this.ReturnCalculatedSalesCost > 0)
        {
            int Result = dc.usp_Operation_Insert(this.Branch_ID, this.OperationDate, ref serial, DocStatus.Approved.ToByte(), OperationTypes.ReturnItemsMaterials.ToInt(), company.Currency_ID, this.ReturnCalculatedSalesCost, this.ReturnCalculatedSalesCost, 1, string.Empty);

            //الحساب المقابل
            dc.usp_OperationDetails_Insert(Result, COA.RawMaterialExpenses.ToInt(), 0, this.ReturnCalculatedSalesCost, 0, this.ReturnCalculatedSalesCost, null, this.ProductionOrder_ID, DocumentsTableTypes.ProductionOrder.ToInt());

            //المخزون 
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, this.ReturnCalculatedSalesCost, 0, this.ReturnCalculatedSalesCost, 0, null, this.ProductionOrder_ID, DocumentsTableTypes.ProductionOrder.ToInt());
        }
    }

    private bool InsertICJ(int Detail_ID, DataRow row, decimal DamageQty)
    {
        decimal? Cost = 0;
        int result = 0;
        if (DamageQty > 0)
        {
            result = dc.usp_ICJ_ProductionOrder_Out(this.OperationDate, DamageQty, row["Item_ID"].ToInt(), null, 0, null, row["Store_ID"].ToInt(), this.Branch_ID, DocumentsTableTypes.ProductionOrder.ToInt(), this.ProductionOrder_ID, Detail_ID, ref Cost);
            this.CalculatedSalesCost += Cost.Value;
        }
        else if (DamageQty < 0)
        {
            DamageQty *= -1;
            result = dc.usp_ICJ_ProductionOrder_Return(this.OperationDate, DamageQty, row["Item_ID"].ToInt(), null, 0, null, row["Store_ID"].ToInt(), this.Branch_ID, DocumentsTableTypes.ProductionOrder.ToInt(), this.ProductionOrder_ID, Detail_ID, DocumentsTableTypes.ProductionOrder.ToInt(), Detail_ID, ref Cost);
            this.ReturnCalculatedSalesCost += Cost.Value;
        }

        if (result == -32)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        if (result == -4)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + ")", string.Empty);
            return false;
        }
        if (result == -5)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CantReturnMoreOriginal + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        return true;
    }

    private void LoadControls()
    {
        this.ProductionOrder_ID = Request["ProductionOrder_ID"].ToInt();
        var Order = dc.usp_ProductionOrder_SelectByID(this.ProductionOrder_ID).FirstOrDefault();
        this.Branch_ID = Order.Branch_ID;
        lblOrderNumber.Text = Order.Serial;
        this.OperationDate = Order.OperationDate.Value;

        btnSave.Visible = !Order.IsReceived.Value && Order.DamagesCost == null && MyContext.PageData.IsEdit;
        btnApprove.Visible = !Order.IsReceived.Value && Order.DamagesCost == null && MyContext.PageData.IsApprove; ;
    }

    private void Fill()
    {
        this.dtProductionOrderDamages = dc.usp_ProductionOrderDetails_Select(this.ProductionOrder_ID).CopyToDataTable();
        gvItems.DataSource = this.dtProductionOrderDamages;
        gvItems.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
    }



    #endregion
}