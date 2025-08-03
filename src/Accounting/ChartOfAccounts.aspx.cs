using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Accounting_ChartOfAccounts : UICulturePage
{
    #region Member fields

    XpressDataContext dc = new XpressDataContext();
    private TreeNode _targetNode = null;

    #endregion

    #region Page events

    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(this);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
                this.FillCOATree();
                this.FixUnbalancedOperations();
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

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            hfEditAccount.Value = string.Empty;
            acBranch.Enabled = (MyContext.UserProfile.Branch_ID == null);
            acBranch.Value = MyContext.UserProfile.Branch_ID.ToStringOrEmpty();
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateAccount.Show();
            }
            else
            {
                mpeCreateAccount.Hide();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveAccount_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            int result = 0;
            if (hfEditAccount.Value != string.Empty)
            {

                #region Old Code
                //result = dc.usp_ChartOfAccount_Update(hfEditAccount.Value.ToInt(), txtAccountName.TrimmedText, txtAccountNameEN.TrimmedText);
                //if (result == -2)
                //{
                //    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.AccountNameAlreadyExists, string.Empty);
                //    trans.Rollback();
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ReHide", "$(HideAddOnlyPart())", true);
                //    mpeCreateAccount.Show();
                //    return;
                //}
                //this.FillCOATree();
                //TreeNode n = FindNode(tvAccounts, hfEditAccount.Value);
                //if (n != null)
                //{
                //    int depth = n.Depth;
                //    for (int i = 0; i < depth; i++)
                //    {
                //        n = n.Parent;
                //        n.Expand();
                //    }
                //}


                #endregion

                result = dc.usp_ChartOfAccountException_Update(hfEditAccount.Value.ToInt(), txtAccountName.TrimmedText, txtAccountNameEN.TrimmedText, acParentAccount.Value.ToInt(), false, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToInt(), ddlType.SelectedValue.ToBoolean(), null, false);
                if (result == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.AccountNameAlreadyExists, string.Empty);
                    trans.Rollback();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ReHide", "$(HideAddOnlyPart())", true);
                    mpeCreateAccount.Show();
                    return;
                }

                if (result == -3)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.AccountOperation, string.Empty);
                    trans.Rollback();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ReHide", "$(HideAddOnlyPart())", true);
                    mpeCreateAccount.Show();
                    return;
                }


                if (result == -4)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.accounthaschildren, string.Empty);
                    trans.Rollback();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ReHide", "$(HideAddOnlyPart())", true);
                    mpeCreateAccount.Show();
                    return;
                }


                this.FillCOATree();
                TreeNode n = FindNode(tvAccounts, hfEditAccount.Value);
                if (n != null)
                {
                    int depth = n.Depth;
                    for (int i = 0; i < depth; i++)
                    {
                        n = n.Parent;
                        n.Expand();
                    }
                }

                LogAction(Actions.Edit, txtAccountName.TrimmedText + " : " + txtAccountNameEN.TrimmedText, dc);
            }
            else
            {

                if (acParentAccount.HasValue)
                {
                    if (dc.IsPermetCreateAccount(1, acParentAccount.Value.ToInt(), null) == -1)
                    {
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.AccountAlreadyFoundOperation, string.Empty);
                        trans.Rollback();
                        mpeCreateAccount.Show();
                        return;
                    }
                }

                result = dc.usp_ChartOfAccountMain_Insert(txtAccountName.TrimmedText, txtAccountNameEN.TrimmedText, acParentAccount.Value.ToInt(), false, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToInt(), ddlType.SelectedValue.ToBoolean(), null,DateTime.Now, false);
                if (result == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.AccountNameAlreadyExists, string.Empty);
                    trans.Rollback();
                    mpeCreateAccount.Show();
                    return;

                }
                if (result == -30)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvalidParentBranch, string.Empty);
                    trans.Rollback();
                    mpeCreateAccount.Show();
                    return;

                }

                if (result == -3)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.AccountAlreadyFoundOperation, string.Empty);
                    trans.Rollback();
                    mpeCreateAccount.Show();
                    return;

                }

                if (txtStartFrom.Text.ToDate() > DateTime.Now.Date && txtOpenBalance.Text.ToDecimalOrDefault() > 0)
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                    trans.Rollback();
                    mpeCreateAccount.Show();
                    return;
                }
                LogAction(Actions.Add, txtAccountName.TrimmedText + " : " + txtAccountNameEN.TrimmedText, dc);
                this.FillCOATree();
            }

            this.ClosePopup_Click(null, null);
            mpeCreateAccount.Hide();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearAccount_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            mpeCreateAccount.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtStartFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtStartFrom.Text.ToDate());
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
            if (sender != null) mpeCreateAccount.Show();
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
            var val = txtIdParentAccount.Text;
            acParentAccount.Value = val;
            acBranch.Value = txtBranch.Text;
            acBranch.IsRequired = txtRatio.IsRequired = txtStartFrom.IsRequired = (txtOpenBalance.Text.ToDecimalOrDefault() != 0);
            //acParentAccount.Enabled = false;
            if (sender != null) this.FocusNextControl(sender);
            if (sender != null) mpeCreateAccount.Show();
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
            tvAccounts.ExpandAll();
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
            tvAccounts.CollapseAll();
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
            this.DeleteCheckedNode(tvAccounts);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess + " (" + Resources.UserInfoMessages.DataRelatedToAnotherIgnored + ")", string.Empty);
            this.FillCOATree();
            LogAction(Actions.Delete, string.Empty, dc);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acParentAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value;
            if (sender != null) mpeCreateAccount.Show();
            if (sender != null) this.FocusNextControl(sender);
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
        txtAccountName.Clear();
        txtAccountNameEN.Clear();
        txtOpenBalance.Clear();
        acParentAccount.Clear();
        txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
        txtRatio.Clear();
        txtRatio.IsRequired = txtStartFrom.IsRequired = false;
        ddlCurrency.SelectedIndex = 0;
        if (acBranch.Enabled) acBranch.Clear();
        this.acBranch_SelectedIndexChanged(null, null);
        this.txtStartFrom_TextChanged(null, null);
        this.txtOpenBalance_TextChanged(null, null);
    }

    private void FillCOATree()
    {
        acParentAccount.Refresh();
        tvAccounts.Nodes.Clear();
        //DataTable dtCOA = dc.usp_ChartOfAccountTree_Select(this.MyContext.CurrentCulture.ToByte(), MyContext.UserProfile.Branch_ID).CopyToDataTable();
        DataTable dtCOA = dc.usp_ChartOfAccountLevelTree_Select(this.MyContext.CurrentCulture.ToByte(), MyContext.UserProfile.Branch_ID, DropTreeLevel.SelectedValue.ToInt()).CopyToDataTable();
        if (dtCOA.Rows.Count > 1000)
        {
            tvAccounts.EnableClientScript = false;
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

            node = new TreeNode(this.GetNodeText(dr.Field<string>("Account"),
                                                 dr.Field<string>("Name_ar"),
                                                 dr.Field<string>("Name_en"),
                                                 dr["Total"].ToExpressString(),
                                                 dr.Field<int>("AccountLevel"),
                                                 dr["ID"].ToExpressString(),
                                                 dr["IsSystem"].ToBoolean(),
                                                 dr["ModuleRelated"].ToBoolean(),
                                                 dr["IsMain"].ToBooleanOrDefault(),
                                                 dr.Field<int?>("Branch_ID"),
                                                 dr.Field<decimal>("OpenBalance").ToDecimalOrDefault(),
                                                 dr.Field<int?>("ParentId"),
                                                 dr.Field<int>("HaveChildCount")),
                                                 dr["ID"].ToExpressString()
                                                 );
            node.ShowCheckBox = dr["IsDeletable"].ToBoolean();

            // Top-Level account (Parent Account).
            if (dr.Field<int>("AccountLevel") == minLevel)
            {
                tvAccounts.Nodes.Add(node);
            }
            else
            {
                // Child account.
                // Get account parent and add it to its child nodes.
                this.Recurse(tvAccounts.Nodes, dr["ParentId"].ToExpressString());
                if (this._targetNode != null)
                {
                    this._targetNode.ChildNodes.Add(node);
                }
            }
        }

        tvAccounts.CollapseAll();

    }

    private string GetNodeText(string accountName, string accountNameAR, string accountNameEN, string accountValue, int accountLevel, string accountID, bool IsSystem, bool ModuleRelated, bool isMain, int? Branch_ID, decimal OpenBalance,
                                                  int? ParentId, int HaveChildCount)
    {

        if (HaveChildCount > 0)
        {
            isMain = true;
        }
        if ((!string.IsNullOrEmpty(accountValue)) && decimal.Parse(accountValue) < 0)
        {
            accountValue = "(" + (decimal.Parse(accountValue) * -1) + ")";
        }

        int redColorVal = 0 + (accountLevel * 10);
        int greenColorVal = 94 + (accountLevel * 10);
        int blueColorVal = 134 + (accountLevel * 10);

        if (IsSystem || ModuleRelated || !this.MyContext.PageData.IsEdit)
        {

            return "<td onclick=\"postBackByObject('" + accountNameAR.Substring(accountNameAR.IndexOf("-") + 1).Trim() + "','" + accountNameEN.Substring(accountNameEN.IndexOf("-") + 1).Trim() + "','" + accountID + "','" + Branch_ID + "','" + ParentId + "','0','" + OpenBalance.ToString("G29") + "','0','" + isMain.ToExpressString() + "')\" width='" + (400 - (accountLevel * 20)) + "'class='treeNode' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9'>" + accountName + "</td><td width='100' class='treeNode2' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9;'>" + accountValue + "</td>";
        }
        else
        {

            return "<td onclick=\"postBackByObject('" + accountNameAR.Substring(accountNameAR.IndexOf("-") + 1).Trim() + "','" + accountNameEN.Substring(accountNameEN.IndexOf("-") + 1).Trim() + "','" + accountID + "','" + Branch_ID + "','" + ParentId + "','0','" + OpenBalance.ToString("G29") + "','0','" + isMain.ToExpressString() + "')\" width='" + (400 - (accountLevel * 20)) + "' class='treeNode' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9'>" + accountName + "</td><td width='100' class='treeNode2' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9;'>" + accountValue + "</td>";
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

        acOppsiteAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,," + COA.Capital.ToInt().ToExpressString() + ",true";
        acOppsiteAccount.Value = COA.Capital.ToInt().ToExpressString();
        acBranch.ContextKey = string.Empty;

        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        if (MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.acBranch_SelectedIndexChanged(null, null);
        txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
        this.txtStartFrom_TextChanged(null, null);
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
                dc.usp_ChartOfAccount_Delete(node.Value.ToInt());
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
                dc.usp_ChartOfAccount_Delete(node.Value.ToInt());
            }
            else
            {
                this.DeleteChildCheckedNode(node);
            }
        }
    }

    private void CheckSecurity()
    {
        lnkAddNewAccount.Visible = this.MyContext.PageData.IsAdd;
        mpeCreateAccount.TargetControlID = lnkAddNewAccount.Visible ? lnkAddNewAccount.UniqueID : hfEditAccount.UniqueID;
        lnkDeleteAccount.Visible = this.MyContext.PageData.IsDelete;
        if (!this.MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
    }

    protected void acParentAccount_OnSelectedIndexChanged(object sender, AutoCompleteEventArgs e)
    {
        txtOpenBalance.Focus();

    }
    protected void DropTreeLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.FillCOATree();
        }
        catch
        {

        }
    }
    #endregion


    private Tuple<TreeNode, string> FindNode1(TreeView tvSelection, string matchText)
    {
        var nd = dc.usp_ChartOfAccountSearch(Server.HtmlEncode(ValueText.Text)).ToList();
        if (nd.Any())
        {
            var obj = nd.FirstOrDefault();
            var id = obj.ID;

            foreach (TreeNode node in tvSelection.Nodes)
            {
                if (node.Value.ToString() == id.ToExpressString())
                {
                    return new Tuple<TreeNode, string>(node, obj.ID.ToExpressString());
                }
                else
                {
                    TreeNode nodeChild = FindChildNode(node, id.ToExpressString());
                    if (nodeChild != null) return new Tuple<TreeNode, string>(nodeChild, obj.ID.ToExpressString()); ;
                }
            }
            return new Tuple<TreeNode, string>((TreeNode)null, string.Empty); ;
        }
        return new Tuple<TreeNode, string>((TreeNode)null, string.Empty); ;

    }


    private void PrintRecursive(TreeNode treeNode)
    {

        // Print each node recursively.  
        foreach (TreeNode tn in treeNode.ChildNodes)
        {

            var Text = tn.Text.Replace("color: red", "color: #f9f9f9");
            Text = Text.Replace("background-color: blue", "background-color: rgb(150, 152, 153)");
            tn.Text = Text;

            PrintRecursive(tn);
        }
    }

    // Call the procedure using the TreeView.  
    private void CallRecursive(TreeView treeView)
    {
        // Print each node recursively.  
        TreeNodeCollection nodes = treeView.Nodes;
        foreach (TreeNode node1 in nodes)
        {

            var Text = node1.Text.Replace("color: red", "color: #f9f9f9");
            Text = Text.Replace("background-color: blue", "background-color: rgb(150, 152, 153)");
            node1.Text = Text;

            PrintRecursive(node1);
        }
    }


    protected void Submit_Click(object sender, EventArgs e)
    {
        try
        {
            CallRecursive(tvAccounts);
            // TreeNode node = FindNode1(tvAccounts, Server.HtmlEncode(ValueText.Text));
            var node = FindNode1(tvAccounts, Server.HtmlEncode(ValueText.Text));
            if (node != null)
            {
                var Text = node.Item1.Text.Replace("color: #f9f9f9", "color: red");
                Text = Text.Replace("background-color: rgb(150, 152, 153)", "background-color: blue");
                node.Item1.Text = Text;
                node.Item1.Select();

                tvAccounts.ExpandAll();
                // node.Selected = true;

                TreeNode tx = tvAccounts.SelectedNode;
                if (tx != null)
                {
                    tx.Select();
                    tx.Expanded = true; //optional if you want to show it expanded.
                }


                // SelectTreeView(this.tvAccounts, node.Item2);



                ValueText.Text = "";
            }
        }
        catch 
        {

        }
    }
}