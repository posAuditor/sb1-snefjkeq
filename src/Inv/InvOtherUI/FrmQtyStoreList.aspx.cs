using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Inv_InvOtherUI_FrmQtyStoreList : UICulturePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dgvDiv.Visible = false;
            lblDiv.Visible = false;
            this.FillQtyStoreGroupList();
        }
    }

    private void FillQtyStoreGroupList()
    {
        int? itemId = Request.QueryString["ItemId"].ToNullableInt();
        var dc = new XpressDataContext();
        var str = dc.Stores.Where(c => c.ID == MyContext.UserProfile.Store_ID).FirstOrDefault();
        if (MyContext.UserProfile.Store_ID != null && str != null)
        {
            var lstQtyList = dc.usp_GetQtyItemeStoreGroup_Select(itemId).Where(c => c.StoreName.Trim() == str.Name.Trim()).ToList();
            var dtQtyItemeStoreGroup = lstQtyList.CopyToDataTable();
            gvQtyStoreList1.DataSource = dtQtyItemeStoreGroup;
            gvQtyStoreList1.DataBind();  
            if(dtQtyItemeStoreGroup!=null && dtQtyItemeStoreGroup.Rows.Count > 0)
            {
                dgvDiv.Visible = true;
                lblDiv.Visible = false;
            }
            else
            {
                dgvDiv.Visible = false;
                lblDiv.Visible = true;
            }
        }
        else
        {
            var lstQtyList = dc.usp_GetQtyItemeStoreGroup_Select(itemId).ToList();
            var dtQtyItemeStoreGroup = lstQtyList.CopyToDataTable();
            gvQtyStoreList1.DataSource = dtQtyItemeStoreGroup;
            gvQtyStoreList1.DataBind();
            if (dtQtyItemeStoreGroup != null && dtQtyItemeStoreGroup.Rows.Count > 0)
            {
                dgvDiv.Visible = true;
                lblDiv.Visible = false;
            }
            else
            {
                dgvDiv.Visible = false;
                lblDiv.Visible = true;
            }
        }
    }

    public string GetCurrentCulture()
    {
        return this.MyContext.CurrentCulture.ToByte().ToExpressString();
    }

    public int GetContactID()
    {
        return (MyContext.UserProfile.HasPermissionShow == false ? 0 : MyContext.UserProfile.Contact_ID);
    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }
}