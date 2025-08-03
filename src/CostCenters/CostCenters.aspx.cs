using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class CostCenters_CostCenters : UICulturePage
{
    #region Member fields

    XpressDataContext dc = new XpressDataContext();
    private TreeNode _targetNode = null;

    #endregion

    #region Page events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
                this.FillCostCentersTree();
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

    protected void acBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acParentCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",true," + acBranch.Value;
            if (sender != null) mpeCreateCostCenter.Show();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            hfEditCostCenter.Value = string.Empty;
            acBranch.Enabled = (MyContext.UserProfile.Branch_ID == null);
            acBranch.Value = MyContext.UserProfile.Branch_ID.ToStringOrEmpty();
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateCostCenter.Show();
            }
            else
            {
                mpeCreateCostCenter.Hide();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveCostCenter_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            int result = 0;
            if (hfEditCostCenter.Value != string.Empty)
            {
                result = dc.usp_CostCenter_Update(hfEditCostCenter.Value.ToInt(), txtCostCenterName.TrimmedText, txtCostCenterNameEN.TrimmedText);
                if (result == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ReHide", "$(HideAddOnlyPart())", true);
                    mpeCreateCostCenter.Show();
                    trans.Rollback();
                    return;
                }
                this.FillCostCentersTree();
                TreeNode n = FindNode(tvCostCenters, hfEditCostCenter.Value);
                if (n != null)
                {
                    int depth = n.Depth;
                    for (int i = 0; i < depth; i++)
                    {
                        n = n.Parent;
                        n.Expand();
                    }
                }
                LogAction(Actions.Edit, txtCostCenterName.TrimmedText + " : " + txtCostCenterNameEN.TrimmedText, dc);
            }
            else
            {
                result = dc.usp_CostCenter_Insert(txtCostCenterName.TrimmedText, txtCostCenterNameEN.TrimmedText, acParentCostCenter.Value.ToInt(), ddlType.SelectedValue.ToBoolean(), acBranch.Value.ToNullableInt(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate());
                if (result == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                    trans.Rollback();
                    mpeCreateCostCenter.Show();
                    return;
                }
                if (result == -30)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvalidParentBranch, string.Empty);
                    trans.Rollback();
                    mpeCreateCostCenter.Show();
                    return;
                }
                if (txtStartFrom.Text.ToDate() > DateTime.Now.Date && txtOpenBalance.Text.ToDecimalOrDefault() > 0)
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                    trans.Rollback();
                    mpeCreateCostCenter.Show();
                    return;
                }
                LogAction(Actions.Add, txtCostCenterName.TrimmedText + " : " + txtCostCenterNameEN.TrimmedText, dc);
                this.FillCostCentersTree();
            }

            this.ClosePopup_Click(null, null);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearCostCenter_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            mpeCreateCostCenter.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnExpand_Click(object sender, EventArgs e)
    {
        try
        {
            tvCostCenters.ExpandAll();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnCollapse_Click(object sender, EventArgs e)
    {
        try
        {
            tvCostCenters.CollapseAll();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            this.DeleteCheckedNode(tvCostCenters);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess + " (" + Resources.UserInfoMessages.DataRelatedToAnotherIgnored + ")", string.Empty);
            this.FillCostCentersTree();
            LogAction(Actions.Delete, string.Empty, dc);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtOpenBalance_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acBranch.IsRequired = txtStartFrom.IsRequired = (txtOpenBalance.Text.ToDecimalOrDefault() != 0);
            if (sender != null) this.FocusNextControl(sender);
            if (sender != null) mpeCreateCostCenter.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtOpenBalance.Clear();
            txtOpenBalance.Enabled = txtStartFrom.Enabled = (ddlType.SelectedIndex == 0);
            txtStartFrom.IsRequired = (txtOpenBalance.Text.ToDecimalOrDefault() != 0);
            this.FocusNextControl(sender);
            mpeCreateCostCenter.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void ClearForm()
    {
        txtCostCenterName.Clear();
        txtCostCenterNameEN.Clear();
        txtOpenBalance.Clear();
        acParentCostCenter.Clear();
        txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
        txtStartFrom.IsRequired = false;
        if (acBranch.Enabled) acBranch.Clear();
        this.acBranch_SelectedIndexChanged(null, null);
        this.txtOpenBalance_TextChanged(null, null);
    }

    private void FillCostCentersTree()
    {
        acParentCostCenter.Refresh();
        tvCostCenters.Nodes.Clear();
        DataTable dtCOA = dc.usp_CostCenterTree_Select(this.MyContext.CurrentCulture.ToByte(), MyContext.UserProfile.Branch_ID).CopyToDataTable();

        if (dtCOA.Rows.Count > 1000)
        {
            tvCostCenters.EnableClientScript = false;
            btnExpand.OnClientClick = string.Empty;
            btnCollapse.OnClientClick = string.Empty;
        }

        // Get tree min level.
        int minLevel = 1;
        foreach (DataRow dr in dtCOA.Rows)
        {
            int accountLevel = dr.Field<int>("AccountLevel");
            minLevel = Math.Min(minLevel, accountLevel);
        }

        TreeNode node = null;

        // Loop through all tree accounts.            
        foreach (DataRow dr in dtCOA.Rows)
        {
            node = new TreeNode(this.GetNodeText(dr.Field<string>("Account"), dr.Field<string>("Name_ar"), dr.Field<string>("Name_en"), dr["Total"].ToExpressString(), dr.Field<int>("AccountLevel"), dr["ID"].ToExpressString(), dr["IsSystem"].ToBoolean()), dr["ID"].ToExpressString());
            node.ShowCheckBox = dr["IsDeletable"].ToBoolean();

            // Top-Level account (Parent Account).
            if (dr.Field<int>("AccountLevel") == minLevel)
            {
                tvCostCenters.Nodes.Add(node);
            }
            else
            {
                // Child account.
                // Get account parent and add it to its child nodes.
                this.Recurse(tvCostCenters.Nodes, dr["ParentId"].ToExpressString());
                if (this._targetNode != null)
                {
                    this._targetNode.ChildNodes.Add(node);
                }
            }
        }

        tvCostCenters.CollapseAll();

    }

    private string GetNodeText(string accountName, string accountNameAR, string accountNameEN, string accountValue, int accountLevel, string accountID, bool IsSystem)
    {

        if ((!string.IsNullOrEmpty(accountValue)) && decimal.Parse(accountValue) < 0)
        {
            accountValue = "(" + (decimal.Parse(accountValue) * -1) + ")";
        }

        int redColorVal = 0 + (accountLevel * 10);
        int greenColorVal = 94 + (accountLevel * 10);
        int blueColorVal = 134 + (accountLevel * 10);

        if (IsSystem || !this.MyContext.PageData.IsEdit)
        {
            return "<td onclick=\"postBackByObject('" + accountNameAR.Substring(accountNameAR.IndexOf("-") + 1).Trim() + "','" + accountNameEN.Substring(accountNameEN.IndexOf("-") + 1).Trim() + "',0)\" width='" + (400 - (accountLevel * 20)) + "'class='treeNode' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9;'>" + accountName + "</td><td width='100' class='treeNode2' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9;'>" + accountValue + "</td>";
        }
        else
        {
            return "<td onclick=\"postBackByObject('" + accountNameAR.Substring(accountNameAR.IndexOf("-") + 1).Trim() + "','" + accountNameEN.Substring(accountNameEN.IndexOf("-") + 1).Trim() + "'," + accountID + ")\" width='" + (400 - (accountLevel * 20)) + "' class='treeNode' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9;'>" + accountName + "</td><td width='100' class='treeNode2' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9;'>" + accountValue + "</td>";
        }

    }

    private void Recurse(TreeNodeCollection nodeCollection, string nodeValue)
    {
        foreach (TreeNode node in nodeCollection)
        {
            if (node.Value == nodeValue)
            {
                this._targetNode = node;
                return;
            }

            if (node.ChildNodes.Count > 0)
            {
                this.Recurse(node.ChildNodes, nodeValue);
            }
        }
    }

    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        if (MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.acBranch_SelectedIndexChanged(null, null);
        txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
    }

    private TreeNode FindNode(TreeView tvSelection, string value)
    {
        foreach (TreeNode node in tvSelection.Nodes)
        {
            if (node.Value == value)
            {
                return node;
            }
            else
            {
                TreeNode nodeChild = FindChildNode(node, value);
                if (nodeChild != null) return nodeChild;
            }
        }
        return (TreeNode)null;
    }

    private TreeNode FindChildNode(TreeNode Node, string value)
    {
        foreach (TreeNode node in Node.ChildNodes)
        {
            if (node.Value == value)
            {
                return node;
            }
            else
            {
                TreeNode nodeChild = FindChildNode(node, value);
                if (nodeChild != null) return nodeChild;
            }
        }
        return (TreeNode)null;
    }

    private void DeleteCheckedNode(TreeView tvSelection)
    {
        foreach (TreeNode node in tvSelection.Nodes)
        {
            if (node.Checked)
            {
                dc.usp_CostCenter_Delete(node.Value.ToInt());
            }
            else
            {
                this.DeleteChildCheckedNode(node);
            }
        }
    }

    private void DeleteChildCheckedNode(TreeNode Node)
    {
        foreach (TreeNode node in Node.ChildNodes)
        {
            if (node.Checked)
            {
                dc.usp_CostCenter_Delete(node.Value.ToInt());
            }
            else
            {
                this.DeleteChildCheckedNode(node);
            }
        }
    }

    private void CheckSecurity()
    {
        lnkAddNewCostCenter.Visible = this.MyContext.PageData.IsAdd;
        mpeCreateCostCenter.TargetControlID = lnkAddNewCostCenter.Visible ? lnkAddNewCostCenter.UniqueID : hfEditCostCenter.UniqueID;
        lnkDeleteCostCenter.Visible = this.MyContext.PageData.IsDelete;
        if (!this.MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
    }

    #endregion
}