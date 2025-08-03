using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Comp_EditPagesMenu : UICulturePage
{

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
    private DataTable dtItems
    {
        get
        {
            if (Session["dtItems_Pages" + this.WinID] == null)
            {
                using (XpressDataContext dc = new XpressDataContext())
                {

                   // Session["dtItems_Pages" + this.WinID] = dc.usp_PageMenuEdit_Select(MyContext.UserProfile.Contact_ID).ToList().Select(sideNav => sideNav);


                    Session["dtItems_Pages" + this.WinID] = (from sideNav in dc.usp_PageMenuEdit_Select(MyContext.UserProfile.Contact_ID).ToList()
                                                             select sideNav).OrderBy(c => c.Parent_ID.ToIntOrDefault()).ThenBy(c => c.DisplayOrder.ToInt()).CopyToDataTable();
                }
            }
            return (DataTable)Session["dtItems_Pages" + this.WinID];
        }

        set
        {
            Session["dtItems_Pages" + this.WinID] = value;
        }
    }
    private bool isPageExists(string url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return File.Exists(Server.MapPath(url));
    }
    private void BindItemsGrid()
    {

        gvItems.DataSource = this.dtItems;
        gvItems.DataBind();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindItemsGrid();
        }
    }

    protected void gvItems_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        this.EditID = gvItems.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
        txtName.Text = gvItems.DataKeys[e.NewSelectedIndex]["PageName"].ToExpressString();
        txtNameEn.Text = gvItems.DataKeys[e.NewSelectedIndex]["PageName_en"].ToExpressString();
        txtDisplayOrder.Text = gvItems.DataKeys[e.NewSelectedIndex]["DisplayOrder"].ToExpressString();
        mpeConfirm.Show();
    }

    protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvItems.PageIndex = e.NewPageIndex;
            this.BindItemsGrid();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        using (XpressDataContext dc = new XpressDataContext())
        {
            dc.usp_PageOrderAndName(this.EditID, txtName.TrimmedText, txtNameEn.TrimmedText, txtDisplayOrder.TrimmedText.ToIntOrDefault());
            Session["dtItems_Pages" + this.WinID] = (from sideNav in dc.usp_PageMenuEdit_Select(MyContext.UserProfile.Contact_ID).ToList()
                                                     select sideNav).OrderBy(c => c.Parent_ID.ToIntOrDefault()).ThenBy(c => c.DisplayOrder.ToInt()).CopyToDataTable();
        }
        this.BindItemsGrid();
    }
}