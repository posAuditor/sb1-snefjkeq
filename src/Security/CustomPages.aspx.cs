using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

public partial class Security_CustomPages : UICulturePage
{
    #region Member Fields

    XpressDataContext dc = new XpressDataContext();
    List<usp_GroupsPermissions_SelectResult> Permissions;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                if (MyContext.UserProfile.UserName.ToLower().Trim() != "auditor") Response.Redirect(PageLinks.Authorization, true);
                this.LoadPage();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }

    #endregion

    #region Control Events

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            this.Save();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlWorkingMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string FileName = string.Empty;
            List<int> Pages = null;

            switch (ddlWorkingMode.SelectedValue)
            {
                case "0":
                    FileName = "Companies.xml";
                    break;
                case "1":
                    FileName = "SmallCompanies.xml";
                    break;
            }

            if (FileName != string.Empty) Pages = (from c in XElement.Load(Server.MapPath("~\\Plans\\") + FileName).Elements("Page")
                                                   select c.Attribute("ID").Value.ToInt()).ToList();

            foreach (TreeNode tn in tvPages.Nodes)
            {
                tn.Checked = Pages == null || Pages.Contains(tn.Value.ToInt());
                CheckRecursive(tn, Pages);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void LoadPage()
    {
        var features = dc.usp_Features_Select().ToList();
        chkBatchID.Checked = features.Where(x => x.ID == 2).First().IsActive.Value;
        chkPercentageDiscount.Checked = features.Where(x => x.ID == 5).First().IsActive.Value;
        chkCashDiscount.Checked = features.Where(x => x.ID == 6).First().IsActive.Value;
        ddlWorkingMode.SelectedValue = features.Where(x => x.ID == 7).First().Value.Value.ToString();

        // Retrieves application pages.
        DataTable dtPages = (from Pages
                                      in dc.usp_PageMenu_Select(-2)
                             where isPageExists(Pages.rawPageUrl)
                             select Pages).CopyToDataTable();

        TreeNode nodeModule;
        TreeNode nodePage;
        TreeNode nodeSubPage;


        foreach (DataRow rowModule in dtPages.Select("Parent_ID = 0"))
        {
            // Generate module node.

            nodeModule = new TreeNode();
            nodeModule.Text = this.GenerateNodeText((this.MyContext.CurrentCulture == ABCulture.Arabic ? rowModule["PageName"].ToString() : rowModule["PageName_en"].ToString()), rowModule["ID"].ToString().PadLeft(4, '0'));
            nodeModule.SelectAction = TreeNodeSelectAction.Expand;
            nodeModule.ShowCheckBox = (rowModule["ID"].ToInt() != 14); //التقارير
            nodeModule.Value = rowModule["ID"].ToString();
            nodeModule.Checked = rowModule["UseAdd"].ToBooleanOrDefault();
            // Get module child pages.
            DataRow[] rowPages = dtPages.Select("Parent_ID = " + rowModule["ID"] + " and Parent_ID <> 0");
            foreach (DataRow rowPage in rowPages)
            {
                // Generate page node.
                string NodeID = dtPages.Select("Parent_ID = " + rowPage["ID"] + " and Parent_ID <> 0").Length > 0 ? rowPage["ID"].ToString().PadLeft(4, '0') : rowPage["Parent_ID"].ToString().PadLeft(4, '0') + "-" + rowPage["ID"].ToString().PadLeft(4, '0');
                nodePage = new TreeNode();
                nodePage.Text = this.GenerateNodeText((this.MyContext.CurrentCulture == ABCulture.Arabic ? rowPage["PageName"].ToString() : rowPage["PageName_en"].ToString()), NodeID);
                nodePage.Value = rowPage["ID"].ToString();
                nodePage.SelectAction = TreeNodeSelectAction.None;
                nodePage.ShowCheckBox = true;// !string.IsNullOrEmpty(rowPage["PageUrl"].ToString());
                nodePage.Checked = rowPage["UseAdd"].ToBoolean();

                // Check if page has sub-pages.
                DataRow[] rowSubPages = dtPages.Select("Parent_ID = " + rowPage["ID"] + " and Parent_ID <> 0");
                foreach (DataRow rowSubPage in rowSubPages)
                {
                    NodeID = dtPages.Select("Parent_ID = " + rowSubPage["ID"] + " and Parent_ID <> 0").Length > 0 ? rowSubPage["ID"].ToString().PadLeft(4, '0') : rowSubPage["Parent_ID"].ToString().PadLeft(4, '0') + "-" + rowSubPage["ID"].ToString().PadLeft(4, '0');

                    nodePage.ShowCheckBox = true;
                    nodePage.SelectAction = TreeNodeSelectAction.Expand;

                    // Generate sub-page node.
                    nodeSubPage = new TreeNode();
                    nodeSubPage.Text = this.GenerateNodeText((this.MyContext.CurrentCulture == ABCulture.Arabic ? rowSubPage["PageName"].ToString() : rowSubPage["PageName_en"].ToString()), NodeID);
                    nodeSubPage.Value = rowSubPage["ID"].ToString();
                    nodeSubPage.SelectAction = TreeNodeSelectAction.None;
                    nodeSubPage.ShowCheckBox = !string.IsNullOrEmpty(rowSubPage["PageUrl"].ToString());
                    nodeSubPage.Checked = rowSubPage["UseAdd"].ToBoolean();
                    nodePage.ChildNodes.Add(nodeSubPage);
                }

                nodeModule.ChildNodes.Add(nodePage);
            }

            tvPages.Nodes.Add(nodeModule);
        }

        tvPages.CollapseAll();
    }

    private string GenerateNodeText(string NodeText, string NodeID)
    {
        string str = "<td  class='checkme ID" + "_" + NodeID + "' >" + NodeText + "</td>";
        return str;
    }

    private bool isPageExists(string url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return File.Exists(Server.MapPath(url));
    }

    private void SaveRecursive(TreeNode treeNode)
    {
        XpressDataContext dc = new XpressDataContext();
        foreach (TreeNode tn in treeNode.ChildNodes)
        {
            dc.usp_Pages_Update(tn.Value.ToInt(), tn.Checked);
            if (tn.ChildNodes.Count > 0)
            {
                SaveRecursive(tn);
            }
        }
    }

    private void Save()
    {
        try
        {
            foreach (TreeNode n in tvPages.Nodes)
            {
                dc.usp_Pages_Update(n.Value.ToInt(), n.Checked);
                SaveRecursive(n);
            }
            dc.usp_Features_Update(2, chkBatchID.Checked, null);
            dc.usp_Features_Update(5, chkPercentageDiscount.Checked, null);
            dc.usp_Features_Update(6, chkCashDiscount.Checked, null);
            dc.usp_Features_Update(7, null, ddlWorkingMode.SelectedValue.ToByte());
            UserMessages.Message(Resources.Labels.Data, Resources.UserInfoMessages.OperationSuccess, PageLinks.CustomPages);
        }
        catch
        {
            throw;
        }
    }

    private void CheckRecursive(TreeNode treeNode, List<int> Pages)
    {
        foreach (TreeNode tn in treeNode.ChildNodes)
        {
            if (tn.Value.ToInt() != 43) tn.Checked = Pages == null || Pages.Contains(tn.Value.ToInt());
            if (tn.ChildNodes.Count > 0)
            {
                CheckRecursive(tn, Pages);
            }
        }
    }

    #endregion

}
