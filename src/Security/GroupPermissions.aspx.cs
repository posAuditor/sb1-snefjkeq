using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.IO;

public partial class Security_GroupPermissions : UICulturePage
{
    #region Member Fields

    XpressDataContext dc = new XpressDataContext();
    List<GroupPermissionsEntry> lst = new List<GroupPermissionsEntry>();
    List<usp_GroupsPermissions_SelectResult> Permissions;

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                //if (!this.MyContext.PageData.IsEdit) Response.Redirect(PageLinks.Authorization, true);
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

    #endregion

    #region Private Methods   

    private void DrawTreeOfRoles(DataTable Roles, Int64 Id, TreeView Tree, TreeNode CurrentItem)
    {
        for (int i = 0; i < Roles.Rows.Count; i++)
        {
            Int64 ParentId = Convert.ToInt64(Roles.Rows[i]["Parent_ID"]);
            if (ParentId == Id)
            {                
                TreeNode NewItem = new TreeNode();  
                string PageNameCol = this.MyContext.CurrentCulture == ABCulture.Arabic ? "PageName" : "PageName_en";
                NewItem.Text = Roles.Rows[i][PageNameCol].ToString();
                NewItem.Value = Roles.Rows[i]["ID"].ToString();
                //NewItem.Checked = Convert.ToBoolean(Roles.Rows[i]["Value"]);
                NewItem.ToolTip = Roles.Rows[i][PageNameCol].ToString();
                if (Id == 0)
                    Tree.Nodes.Add(NewItem);
                else CurrentItem.ChildNodes.Add(NewItem);                

                DataRow[] Childs = Roles.Select("Parent_ID = " + Roles.Rows[i]["ID"] + " and Parent_ID <> 0");
                if (Childs != null && Childs.Length > 0)
                {                    
                    NewItem.SelectAction = TreeNodeSelectAction.Expand;
                    NewItem.ShowCheckBox = false;                    

                    DrawTreeOfRoles(Roles, Convert.ToInt64(Roles.Rows[i]["ID"]), Tree, NewItem);
                }
                else
                {
                    this.AddPermissionsNode(NewItem, Roles.Rows[i]);
                    NewItem.SelectAction = TreeNodeSelectAction.None;
                    NewItem.ShowCheckBox = !string.IsNullOrEmpty(Roles.Rows[i]["PageUrl"].ToString());
                }
            }
        }
    }

    private void LoadPage()
    {
        //lblGroupName.Text = Request["RoleName"].ToString();

        // Retrieves application pages.
        DataTable dtPages = (from Pages in dc.usp_PageMenu_Select(-1)
                             where isPageExists(Pages.rawPageUrl)
                             select Pages).CopyToDataTable();

        Permissions = dc.usp_GroupsPermissions_Select(new Guid(Request["ID"].ToString())).ToList<usp_GroupsPermissions_SelectResult>();

        this.DrawTreeOfRoles(dtPages, 0, tvPages, null);

        /*TreeNode nodeModule;
        TreeNode nodePage;
        TreeNode nodeSubPage;        

        foreach (DataRow rowModule in dtPages.Select("Parent_ID = 0"))
        {
            // Generate module node.
            nodeModule = new TreeNode((this.MyContext.CurrentCulture == ABCulture.Arabic ? rowModule["PageName"].ToString() : rowModule["PageName_en"].ToString()));
            nodeModule.SelectAction = TreeNodeSelectAction.Expand;
            nodeModule.ShowCheckBox = false;
            nodeModule.Value = rowModule["ID"].ToString();

            // Get module child pages.
            DataRow[] rowPages = dtPages.Select("Parent_ID = " + rowModule["ID"] + " and Parent_ID <> 0");
            foreach (DataRow rowPage in rowPages)
            {
                // Generate page node.
                nodePage = new TreeNode();
                nodePage.Text = this.GenerateNodeText((this.MyContext.CurrentCulture == ABCulture.Arabic ? rowPage["PageName"].ToString() : rowPage["PageName_en"].ToString()), rowPage["ID"].ToString());
                nodePage.Value = rowPage["ID"].ToString();
                nodePage.SelectAction = TreeNodeSelectAction.None;
                nodePage.ShowCheckBox = !string.IsNullOrEmpty(rowPage["PageUrl"].ToString());

                this.AddPermissionsNode(nodePage, rowPage);

                // Check if page has sub-pages.
                DataRow[] rowSubPages = dtPages.Select("Parent_ID = " + rowPage["ID"] + " and Parent_ID <> 0");
                foreach (DataRow rowSubPage in rowSubPages)
                {
                    nodePage.ShowCheckBox = false;
                    nodePage.SelectAction = TreeNodeSelectAction.Expand;

                    // Generate sub-page node.
                    nodeSubPage = new TreeNode();
                    nodeSubPage.Text = this.GenerateNodeText((this.MyContext.CurrentCulture == ABCulture.Arabic ? rowSubPage["PageName"].ToString() : rowSubPage["PageName_en"].ToString()), rowSubPage["ID"].ToString());
                    nodeSubPage.Value = rowSubPage["ID"].ToString();
                    nodeSubPage.SelectAction = TreeNodeSelectAction.None;
                    nodeSubPage.ShowCheckBox = !string.IsNullOrEmpty(rowSubPage["PageUrl"].ToString());

                    this.AddPermissionsNode(nodeSubPage, rowSubPage);
                    nodePage.ChildNodes.Add(nodeSubPage);
                }

                nodeModule.ChildNodes.Add(nodePage);
            }

            tvPages.Nodes.Add(nodeModule);
        }*/
        
        tvPages.CollapseAll();
    }

    private void AddPermissionsNode(TreeNode node, DataRow rowPage)
    {
        var perm = (from per in Permissions
                    where per.PageSecurityEntity_ID == node.Value.ToInt()
                    select per).FirstOrDefault();

        if (rowPage["useAdd"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText(Resources.Labels.Add, node.Value, "0"), "0");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = perm.AllowAdd.Value;
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["useEdit"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText(Resources.Labels.Edit, node.Value, "1"), "1");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = perm.AllowEdit.Value;
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["useApprove"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText(Resources.Labels.Approve, node.Value, "2"), "2");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = perm.AllowApprove.Value;
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["useNotApprove"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("الغاء الاعتماد", node.Value, "8"), "8");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = (perm.AllowNotApprove!=null? perm.AllowNotApprove.Value:true);
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["useViewDoc"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText(Resources.Labels.ViewDoc, node.Value, "3"), "3");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = perm.AllowViewDoc.Value;
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["useViewList"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText(Resources.Labels.ViewList, node.Value, "4"), "4");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = perm.AllowViewList.Value;
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["useDelete"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText(Resources.Labels.DeleteCancel, node.Value, "5"), "5");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = perm.AllowDelete.Value;
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["usePrint"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText(Resources.Labels.Print, node.Value, "6"), "6");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = perm.AllowPrint.Value;
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["useAttach"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText(Resources.Labels.Attach, node.Value, "7"), "7");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = perm.AllowAttach.Value;
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["UseDeleteAttch"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("حذف المرفق", node.Value, "9"), "9");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked =  (perm.AllowDeleteAttach != null ? perm.AllowDeleteAttach.Value : false);
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["UseViewAttach"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("مشاهدة المرفق", node.Value, "10"), "10");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = (perm.AllowViewAttch != null ? perm.AllowViewAttch.Value : false);
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["UseViewJE"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("مشاهدة  القيد", node.Value, "11"), "11");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = (perm.AllowViewJE != null ? perm.AllowViewJE.Value : false);
            if (n.Checked) n.Parent.Checked = true;
        }


        if (rowPage["ViewPriceIteme"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("اسعار الصنف", node.Value, "12"), "12");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = (perm.AllowViewPriceIteme != null ? perm.AllowViewPriceIteme.Value : false);
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["ViewBalanceIteme"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("جرد الصنف", node.Value, "13"), "13");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = (perm.AllowViewBalanceIteme != null ? perm.AllowViewBalanceIteme.Value : false);
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["UseApproveAccounting"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("إعتماد مالي", node.Value, "14"), "14");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = (perm.AllowApproveAccounting != null ? perm.AllowApproveAccounting.Value : false);
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["IsPrice"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("تغيير السعر", node.Value, "15"), "15");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = (perm.AllowsPrice != null ? perm.AllowsPrice.Value : false);
            if (n.Checked) n.Parent.Checked = true;
        }

        if (rowPage["UseReorderGrid"].ToBooleanOrDefault())
        {
            TreeNode n = new TreeNode(this.GeneratePermissionNodeText("ترتيب الحقول", node.Value, "16"), "16");
            n.SelectAction = TreeNodeSelectAction.None;
            n.ShowCheckBox = true;
            node.ChildNodes.Add(n);
            if (perm != null) n.Checked = (perm.AllowReorderGrid != null ? perm.AllowReorderGrid.Value : false);
            if (n.Checked) n.Parent.Checked = true;
        }
    }

    private string GenerateNodeText(string NodeText, string NodeID)
    {
        string str = "<td class='EntityNode' id='EntityID" + "_" + NodeID + "' >" + NodeText + "</td>";
        return str;
    }

    private string GeneratePermissionNodeText(string NodeText, string NodeID, string PermissionID)
    {
        string str = "<td class='PermissionNode' id='EntityID" + "_" + NodeID + "-" + PermissionID + "' >" + NodeText + "</td>";
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
            if (tn.ChildNodes.Count > 0 && tn.ChildNodes[0].ChildNodes.Count == 0 && tn.ShowCheckBox.Value)
            {
                GroupPermissionsEntry gpe = new GroupPermissionsEntry();
                gpe.PageSecurityEntity_ID = tn.Value.ToInt();

                foreach (TreeNode n in tn.ChildNodes)
                {
                    if (n.Checked && n.Value == "0") gpe.AllowAdd = true;
                    if (n.Checked && n.Value == "1") gpe.AllowEdit = true;
                    if (n.Checked && n.Value == "2") gpe.AllowApprove = true;
                    if (n.Checked && n.Value == "3") gpe.AllowViewDoc = true;
                    if (n.Checked && n.Value == "4") gpe.AllowViewList = true;
                    if (n.Checked && n.Value == "5") gpe.AllowDelete = true;
                    if (n.Checked && n.Value == "6") gpe.AllowPrint = true;
                    if (n.Checked && n.Value == "7") gpe.AllowAttach = true;
                    if (n.Checked && n.Value == "8") gpe.AllowNotApprove = true; 
                    if (n.Checked && n.Value == "9") gpe.AllowDeleteAttach = true;
                    if (n.Checked && n.Value == "10") gpe.AllowViewAttach = true;
                    if (n.Checked && n.Value == "11") gpe.AllowViewJE = true;
                    if (n.Checked && n.Value == "12") gpe.AllowViewPriceIteme = true;
                    if (n.Checked && n.Value == "13") gpe.AllowViewBalanceIteme = true;
                    if (n.Checked && n.Value == "14") gpe.AllowApproveAccounting = true;
                    if (n.Checked && n.Value == "15") gpe.AllowsPrice = true;
                    if (n.Checked && n.Value == "16") gpe.AllowReorderGrid = true;
                }
                lst.Add(gpe);
            }
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
                SaveRecursive(n);
            }
            Guid Role_ID = new Guid(Request.QueryString["ID"].ToString());
            dc.usp_GroupsPermissions_Delete(Role_ID);

            foreach (var gpe in lst)
            {
                dc.usp_GroupsPermissions_Insert(Role_ID, gpe.PageSecurityEntity_ID, gpe.AllowAdd, gpe.AllowEdit, gpe.AllowApprove, gpe.AllowViewDoc, gpe.AllowViewList, gpe.AllowDelete, gpe.AllowPrint, gpe.AllowAttach, gpe.AllowNotApprove, gpe.AllowDeleteAttach, gpe.AllowViewAttach, gpe.AllowViewJE, gpe.AllowViewPriceIteme, gpe.AllowViewBalanceIteme, gpe.AllowApproveAccounting, gpe.AllowsPrice,gpe.AllowReorderGrid);
            }
            this.LogAction(Actions.Edit, lblGroupName.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.GroupPermissions + Request.Url.Query, PageLinks.Users);
        }
        catch
        {
            throw;
        }
    }

    #endregion
}

public class GroupPermissionsEntry
{
    public int PageSecurityEntity_ID;
    public bool AllowAdd = false;
    public bool AllowEdit = false;
    public bool AllowApprove = false;
    public bool AllowViewDoc = false;
    public bool AllowViewList = false;
    public bool AllowDelete = false;
    public bool AllowPrint = false;
    public bool AllowAttach = false;
    public bool AllowNotApprove = false;
    public bool AllowDeleteAttach = false;
    public bool AllowViewAttach = false;
    public bool AllowViewJE = false;
    public bool AllowViewPriceIteme = false;
    public bool AllowViewBalanceIteme = false;
    public bool AllowApproveAccounting = false;
    public bool AllowsPrice = false;
    public bool AllowReorderGrid = false;
}