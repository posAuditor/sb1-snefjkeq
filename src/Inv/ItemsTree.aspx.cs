using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Accounting_ItemsTree : UICulturePage
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

   

  

    #endregion

    #region Private Methods
     
    private void FillCOATree()
    {
        
        tvAccounts.Nodes.Clear();
        // var settingsChartOdAccount = dc.ChartOfAccountSettings.FirstOrDefault();
        //DataTable dtCOA = dc.usp_ChartOfAccountTree_Select(this.MyContext.CurrentCulture.ToByte(), MyContext.UserProfile.Branch_ID).Where(x => x.ParentId == COA.Customers.ToInt()).CopyToDataTable();
        DataTable dtCOA = dc.usp_ItemsTree().CopyToDataTable();
        if (dtCOA.Rows.Count > 1000)
        {
            tvAccounts.EnableClientScript = false;
            
        }

        // Get tree min level.
        int minLevel = 1;
        foreach (DataRow dr in dtCOA.Rows)
        {
            int accountLevel = dr.Field<int>("Level");
            minLevel = Math.Min(minLevel, accountLevel);
        }
        TreeNode node = null;

        // Loop through all tree accounts.            
        foreach (DataRow dr in dtCOA.Rows)
        {
            
            node = new TreeNode(this.GetNodeText( dr.Field<string>("Name") , dr.Field<int>("Level")), dr["ID"].ToExpressString());
            

            // Top-Level account (Parent Account).
            if (dr.Field<int>("Level") == minLevel)
            {
                tvAccounts.Nodes.Add(node);
            }
            else
            {
                // Child account.
                // Get account parent and add it to its child nodes.
                this.Recurse(tvAccounts.Nodes, dr["Parent_Id"].ToExpressString());
                if (this._targetNode != null)
                {
                    this._targetNode.ChildNodes.Add(node);
                }
            }
        }

        tvAccounts.CollapseAll();

    }

    private string GetNodeText(string Name, int accountLevel)
    {

        

        int redColorVal = 0 + (accountLevel * 10);
        int greenColorVal = 94 + (accountLevel * 10);
        int blueColorVal = 134 + (accountLevel * 10);



        return "<td width='" + (400 - (accountLevel * 20)) + "' class='treeNode' style='background-color: rgb(" + 150 + ", " + 152 + ", " + 153 + "); color: #f9f9f9'>" + Name + "</td>";
        

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
        
        if (!this.MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
    }

    private void CustomPage()
    {
        
    }

   

    #endregion
}