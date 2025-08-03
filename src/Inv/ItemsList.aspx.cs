using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;

public partial class Inv_ItemsList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtItemsList
    {
        get
        {
            return (DataTable)Session["dtItemsList" + this.WinID];
        }

        set
        {
            Session["dtItemsList" + this.WinID] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvItemssList.FormatNumber = MyContext.FormatNumber;
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvItemssList.Columns[4].Visible = this.MyContext.PageData.IsViewDoc;
                gvItemssList.Columns[5].Visible = this.MyContext.PageData.IsDelete;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillItemsList();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Control Events

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillItemsList();
            txtBarcodeSrch.Focus();

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
            acNameSrch.Clear();
            ddlItemType.SelectedIndex = 0;
            acCategory.Clear();
            txtBarcodeSrch.Clear();
            this.FillItemsList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
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

    protected void acCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acNameSrch.ContextKey = (ddlItemType.SelectedIndex == 0 ? string.Empty : ddlItemType.SelectedValue) + "," + acCategory.Value + ",";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillItemsList()
    {   ////Commented Cz Price and SalesPrice View In dataTable

        //char? ItemType = ddlItemType.SelectedIndex == 0 ? (char?)null : ddlItemType.SelectedValue.ToCharArray()[0];
        //if (chkIsDescription.Checked)
        //{
        //    var lst2 = dc.usp_Items_Select(txtBarcodeSrch.TrimmedText, acNameSrch.Text, ItemType, acCategory.Value.ToNullableInt(), null, true);
        //    var lst = dc.usp_Items_Select("Described!" + txtBarcodeSrch.TrimmedText, acNameSrch.Text, ItemType, acCategory.Value.ToNullableInt(), null, true);
        //    this.dtItemsList = lst2.Union(lst).CopyToDataTable();
        //}
        //else
        //{
        //    var lst = dc.usp_Items_Select(txtBarcodeSrch.TrimmedText, acNameSrch.Text, ItemType, acCategory.Value.ToNullableInt(), null, true);
        //    this.dtItemsList = lst.CopyToDataTable();
        //}
        var lstItems = new List<usp_ItemsWidthPrice_SelectResult>();
        char? ItemType = ddlItemType.SelectedIndex == 0 ? (char?)null : ddlItemType.SelectedValue.ToCharArray()[0];
        if (chkIsDescription.Checked)
        {
            var lst2 = dc.usp_ItemsWidthPrice_Select(txtBarcodeSrch.TrimmedText, acNameSrch.Value.ToExpressString(), ItemType, acCategory.Value.ToNullableInt(), null, true).OrderBy(x => x.Barcode).ToList();
            var lst = dc.usp_ItemsWidthPrice_Select("Described!" + txtBarcodeSrch.TrimmedText, acNameSrch.Value.ToExpressString(), ItemType, acCategory.Value.ToNullableInt(), null, true).OrderBy(x => x.Barcode).ToList();
            lstItems.AddRange(lst2.Union(lst));
        }
        else
        {
            var lst = dc.usp_ItemsWidthPrice_Select("", acNameSrch.Value.ToExpressString(), ItemType, acCategory.Value.ToNullableInt(), null, true).OrderBy(x => x.Barcode);
            lstItems.AddRange(lst);
        }
        //foreach (var item in lstItems)
        //{
        //    item.SalesPrice = int.Parse(dc.fun_GetItemDefaultPriceByUOM(item.ID, item.UOM_ID, null).Value.ToString("0.####"));
        //}

        this.dtItemsList = lstItems.OrderBy(x => x.Barcode).CopyToDataTable();
        gvItemssList.DataSource = this.dtItemsList;
        gvItemssList.DataBind();

    }

    private void LoadControls()
    {
        acCategory.ContextKey = string.Empty;
        acNameSrch.ContextKey = ",,,true";
        acTax.ContextKey = string.Empty;
    }

    #endregion

    protected void lnkItemsUpdateTax_Click(object sender, EventArgs e)
    {

    }
    protected void txtBarcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acNameSrch.Clear();
            if (txtBarcodeSrch.IsNotEmpty)
            {
                var item = dc.usp_ItemsBarcode_Select(txtBarcodeSrch.TrimmedText).FirstOrDefault();
                if (item != null) acNameSrch.Value = item.id.ToString();
            }



        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnUpdateItemTax_Click(object sender, EventArgs e)
    {
        if (acTax.HasValue)
        {
            dc.usp_ItemTax_Update(acTax.Value.ToInt());
        }

    }

    protected void lnkRelated_Click(object sender, EventArgs e)
    {
        int Item_ID = (sender as LinkButton).CommandArgument.ToInt();
        gvMaterial.DataSource = dc.usp_ItemMaterielByItem_ID(Item_ID);
        gvMaterial.DataBind();
        mpeMaterial.Show();
    }


}