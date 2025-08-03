using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Inv_InvOtherUI_FrmItemPriceList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.FillItemeList_price();
        }
    }

    private void FillItemeList_price()
    {
        int? itemId = Request.QueryString["ItemId"].ToNullableInt();
        var dc = new XpressDataContext();
        var lstInvoices1 = dc.usp_GetItemPrice_Select(itemId).ToList();
        var dtItemePrice = lstInvoices1.CopyToDataTable();
        gvPriceList1.DataSource = dtItemePrice;
        gvPriceList1.DataBind();        
    }
}