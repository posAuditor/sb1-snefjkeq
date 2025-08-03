using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Inv_ItemUI_ListAll : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    DataTable dtItemsList;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.FillItemsList();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }

    protected void gvItemssList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvItemssList.PageIndex = e.NewPageIndex;
            gvItemssList.DataSource = this.dtItemsList;
            gvItemssList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvItemssList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = dc.usp_Items_Delete(gvItemssList.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvItemssList.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.FillItemsList();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FillItemsList()
    {        
        var lstItems = new List<usp_ItemsWidthPrice_SelectResult>();
        //int ddlItemType = string.IsNullOrWhiteSpace(Request.QueryString["ddlItemType"]) ? 0 : Request.QueryString["ddlItemType"].ToInt();
        var ItemType = string.IsNullOrWhiteSpace(Request.QueryString["ddlItemType"]) ? (char?)null : Request.QueryString["ddlItemType"].ToCharArray()[0];
        if (ItemType == '0')
            ItemType = null;
        if (string.IsNullOrWhiteSpace(Request.QueryString["IsDescription"]) && Request.QueryString["IsDescription"].ToBoolean())
        {
            var lst2 = dc.usp_ItemsWidthPrice_Select(Request.QueryString["Barcode"], Request.QueryString["ItemName"], ItemType, Request.QueryString["Category"].ToNullableInt(), null, true).OrderBy(x => x.Barcode).ToList();
            var lst = dc.usp_ItemsWidthPrice_Select(Request.QueryString["Barcode"], Request.QueryString["ItemName"], ItemType, Request.QueryString["Category"].ToNullableInt(), null, true).OrderBy(x => x.Barcode).ToList();
            lstItems.AddRange(lst2.Union(lst));
        }
        else
        {
            //var lst = dc.usp_ItemsWidthPrice_Select("", acNameSrch.Value.ToExpressString(), ItemType, acCategory.Value.ToNullableInt(), null, true).OrderBy(x => x.Barcode);
            var lst = dc.usp_ItemsWidthPrice_Select("", Request.QueryString["ItemName"], ItemType, Request.QueryString["Category"].ToNullableInt(), null, true).OrderBy(x => x.Barcode);
            lstItems.AddRange(lst);
        }        

        this.dtItemsList = lstItems.OrderBy(x => x.Barcode).CopyToDataTable();
        gvItemssList.DataSource = this.dtItemsList;
        gvItemssList.DataBind();
    }
}
