using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;

public partial class Inv_BeginingInventoryList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.gvItemssList.FormatNumber = MyContext.FormatNumber;
        if (!IsPostBack)
        {
            acCategory.ContextKey = string.Empty;
            FillList();

        }
    }


    private DataTable dtInventoryDocument
    {
        get
        {
            return (DataTable)Session["dtItems_InventoryDocument" + this.WinID];
        }

        set
        {
            Session["dtItems_InventoryDocument" + this.WinID] = value;
        }
    }


    protected void gvCommission_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvItemssList.PageIndex = e.NewPageIndex;
            //gvItemssList.DataSource = this.dtItemsList;
            //gvItemssList.DataBind();

            byte EntryType = 2;
            byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
            gvItemssList.DataSource = dc.usp_InventoryDocumentBegining_Select(null, "", MyContext.FiscalYearStartDate, MyContext.FiscalYearEndDate, "", DocStatus_ID, 0, 2, acCategory.Value.ToNullableInt()).CopyToDataTable();
            gvItemssList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }



    protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvItemssList.DataKeys[e.RowIndex]["ID"].ToInt();    

            var result = dc.usp_InventoryDocument_Delete(ID);
            if (result == -1)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, "لا يمكنك الحذف لانها معتمدة", string.Empty);
               
            }
            else
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
                 this.FillList();
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }



    protected void gvInstallmentsList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //try
        //{
        //    var id = gvItemssList.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
        //    //int result = dc.usp_Customers_Delete(gvInstallmentsList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());
        //    // dc.usp_inv
        //    var result = dc.usp_InventoryDocument_Delete(id);
        //    if (result == -1)
        //    {
        //        UserMessages.Message(this.MyContext.PageData.PageTitle, "لا يمكنك الحذف لانها معتمدة", string.Empty);

        //    }
        //    else
        //    {
        //        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);

        //    }
        //}

        //catch (Exception ex)
        //{
        //    Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        //}
    }


    protected void gvItems_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            //gvItemssList.PageIndex = e.NewPageIndex;
            ////gvItemssList.DataSource = this.dtItemsList;
            ////gvItemssList.DataBind();

            //byte EntryType = 2;
            //byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
            //gvItemssList.DataSource = dc.usp_InventoryDocumentBegining_Select(null, "", MyContext.FiscalYearStartDate, MyContext.FiscalYearEndDate, "", DocStatus_ID, 0, 2).CopyToDataTable();
            //gvItemssList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    //protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        int ID = gvItemssList.DataKeys[e.RowIndex]["ID"].ToInt();
    //        DataRow dr = this.dtInventoryDocument.Select("ID=" + ID.ToExpressString())[0];
    //        dr.Delete();

    //        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
    //    }
    //}


    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillList();
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
            ddlStatus.SelectedIndex = 0;
            this.FillList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FillList()
    {
        byte EntryType = 2;
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        gvItemssList.DataSource = dc.usp_InventoryDocumentBegining_Select(null, "", MyContext.FiscalYearStartDate, MyContext.FiscalYearEndDate, "", DocStatus_ID, 0, 2, acCategory.Value.ToNullableInt()).CopyToDataTable();
        gvItemssList.DataBind();
    }
}