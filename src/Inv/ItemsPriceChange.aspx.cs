using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
public partial class Inv_ItemsPriceChange : UICulturePage
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
        var lstItems = new List<usp_ItemsCalculate_SelectResult>();
        char? ItemType = ddlItemType.SelectedIndex == 0 ? (char?)null : ddlItemType.SelectedValue.ToCharArray()[0];
        if (chkIsDescription.Checked)
        {
            var lst2 = dc.usp_ItemsCalculate_Select(txtBarcodeSrch.TrimmedText, acNameSrch.Value.ToExpressString(), ItemType, acCategory.Value.ToNullableInt(), null, true).OrderBy(x => x.Barcode).ToList();
            var lst = dc.usp_ItemsCalculate_Select("Described!" + txtBarcodeSrch.TrimmedText, acNameSrch.Value.ToExpressString(), ItemType, acCategory.Value.ToNullableInt(), null, true).OrderBy(x => x.Barcode).ToList();
            lstItems.AddRange(lst2.Union(lst));
        }
        else
        {
            var lst = dc.usp_ItemsCalculate_Select(txtBarcodeSrch.TrimmedText, acNameSrch.Value.ToExpressString(), ItemType, acCategory.Value.ToNullableInt(), null, true).OrderBy(x => x.Barcode);
            lstItems.AddRange(lst);
        }
        //foreach (var item in lstItems)
        //{
        //    item.SalesPrice = int.Parse(dc.fun_GetItemDefaultPriceByUOM(item.ID, item.UOM_ID, null).Value.ToString(NbrHashNeerDecimal));
        //}

        this.dtItemsList = lstItems.OrderBy(x => x.Barcode).CopyToDataTable();
        gvItemssList.DataSource = this.dtItemsList;
        gvItemssList.DataBind();

    }

    private void LoadControls()
    {
        acCategory.ContextKey = string.Empty;
        acNameSrch.ContextKey = ",,,true";
    }

    #endregion

    protected void lnkItemsUpdateTax_Click(object sender, EventArgs e)
    {

    }
    protected void btnUpdateItemTax_Click(object sender, EventArgs e)
    {


        for (int i = 0; i < this.dtItemsList.Rows.Count; i++)
        {
            var InitPrice = this.dtItemsList.Rows[i]["DefaultPrice"].ToDecimalOrDefault() - (txtParcentTaxInit.Text.ToDecimalOrDefault()) * this.dtItemsList.Rows[i]["DefaultPrice"].ToDecimalOrDefault() / (txtParcentTaxInit.Text.ToDecimalOrDefault() + 100);

            var NewPrice = InitPrice + (txtParcentTaxNew.Text.ToDecimalOrDefault()) * InitPrice / (txtParcentTaxNew.Text.ToDecimalOrDefault() + 100);

            this.dtItemsList.Rows[i]["taxInit"] = txtParcentTaxInit.Text.ToDecimalOrDefault();
            this.dtItemsList.Rows[i]["TaxNew"] = txtParcentTaxNew.Text.ToDecimalOrDefault();
            if (ddlRounding.SelectedValue == "-1")
            {
                this.dtItemsList.Rows[i]["NewPrice"] = NewPrice.ToExpressString();
            }
            else if (ddlRounding.SelectedValue == "0")
            {
                this.dtItemsList.Rows[i]["NewPrice"] = Math.Round(NewPrice, MidpointRounding.AwayFromZero).ToExpressString();
            }
            else if (ddlRounding.SelectedValue == "1")
            {
                this.dtItemsList.Rows[i]["NewPrice"] = Math.Round(NewPrice, 1, MidpointRounding.AwayFromZero).ToExpressString();
            }


        }

        gvItemssList.DataSource = this.dtItemsList;
        gvItemssList.DataBind();


    }
    protected void lnkUpdatePrice_Click(object sender, EventArgs e)
    {
        var itsList = dc.Items.ToList();
        for (int i = 0; i < this.dtItemsList.Rows.Count; i++)
        {
            if (this.dtItemsList.Rows[i]["NewPrice"].ToDecimalOrDefault() > 0)
            {
                var id = this.dtItemsList.Rows[i]["Id"].ToInt();
                var obj = itsList.Where(x => x.ID == id).FirstOrDefault();
                obj.DefaultPrice = this.dtItemsList.Rows[i]["NewPrice"].ToDecimalOrDefault(); ;
            }

        }
        dc.SubmitChanges();
    }
    protected void lnkItemsUpdateTax_Click1(object sender, EventArgs e)
    {
        mpeUpdateItemTax.Show();

    }
}