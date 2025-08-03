<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Security_Users" %>

<%@ Register TagPrefix="asp" Namespace="XPRESS.ServerControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="AutoComplete" Src="~/CustomControls/ucAutoCompleteTextValue.ascx"
    TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-blue">
        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="<%$Resources:Labels,Groups %>">
            <ContentTemplate>
                <asp:LinkButton ID="lnkSearchRoles" runat="server" CssClass="collapse_search_link"
                    Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
                <asp:LinkButton ID="lnkAddNewRole" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlSearchRoles"
                    CollapsedSize="0" Collapsed="True" ExpandControlID="lnkSearchRoles" CollapseControlID="lnkSearchRoles"
                    AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
                    ExpandDirection="Vertical" SuppressPostBack="true">
                </asp:CollapsiblePanelExtender>
                <div style="clear: both;">
                </div>
                <asp:Panel ID="pnlSearchRoles" CssClass="pnlSearch" runat="server" DefaultButton="btnSearchRoles">
                    <div class="tcat">
                        <%=Resources.Labels.Search %>
                    </div>
                    <div class="content form">
                        <p>
                            <label>
                                <%=Resources.Labels.Name %></label>
                            <asp:ABFTextBox ID="txtRoleNamesrch" CssClass="field" runat="server">
                            </asp:ABFTextBox>
                        </p>
                        <div class="btnDiv">
                            <asp:Button ID="btnSearchRoles" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                                OnClick="btnSearchRoles_click" />
                            <asp:Button ID="btnClearRoleSrch" runat="server" CssClass="button" OnClick="btnClearRoleSrch_Click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvRoles" GridViewStyle="BlueStyle" DataKeyNames="roleID,RoleName"
                    OnSelectedIndexChanging="gvRoles_SelectedIndexChanging" OnRowDeleting="gvRoles_RowDeleting"
                    OnPageIndexChanging="gvRoles_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="RoleName" HeaderText="<%$Resources:Labels,GroupName %>" />
                        <asp:BoundField DataField="UserInRole" HeaderText="<%$Resources:Labels,Users %>" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("GroupPermissions.aspx?ID={0}&RoleName={1}", Eval("RoleID"),Eval("RoleName") ) %>'
                                    Text="<%$ Resources:Labels, GroupPermissions %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/edit_grid.gif" runat="server" CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="../images/delete_grid.gif" runat="server" CommandName="Delete"
                                    OnClientClick="return ConfirmSure();" Visible='<%# Convert.ToBoolean( new Guid(Eval("RoleID").ToString())!= new Guid("c0d9540f-3e4b-4a75-817f-107a6d33909c")) && Convert.ToBoolean( new Guid(Eval("RoleID").ToString())!= new Guid("ef5dfc9e-c034-47f1-8622-7befede90136"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="<%$Resources:Labels,Users %>">
            <ContentTemplate>
                <asp:LinkButton ID="lnkSearchUsers" runat="server" CssClass="collapse_search_link"
                    Text="<%$ Resources:Labels,Search %>"></asp:LinkButton>
                <asp:LinkButton ID="lnkAddNewUser" runat="server" CssClass="collapse_add_link" Text="<%$ Resources:Labels,AddNew %>"></asp:LinkButton>
                <asp:CollapsiblePanelExtender ID="cpeSearch" runat="Server" TargetControlID="pnlSearch"
                    CollapsedSize="0" Collapsed="True" ExpandControlID="lnkSearchUsers" CollapseControlID="lnkSearchUsers"
                    AutoCollapse="False" AutoExpand="false" ScrollContents="false" TextLabelID="lblSrch"
                    ExpandDirection="Vertical" SuppressPostBack="true">
                </asp:CollapsiblePanelExtender>
                <div style="clear: both;">
                </div>
                <asp:Panel ID="pnlSearch" CssClass="pnlSearch" runat="server" DefaultButton="btnSearch">
                    <div class="tcat">
                        <%=Resources.Labels.Search %>
                    </div>
                    <div class="content form">
                        <p>
                            <label>
                                <%=Resources.Labels.Name %></label>
                            <asp:ABFTextBox ID="txtUserNameSrch" CssClass="field" runat="server">
                            </asp:ABFTextBox>
                        </p>
                        <div class="btnDiv">
                            <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="<%$ Resources:Labels, Search %>"
                                OnClick="btnSearch_click" />
                            <asp:Button ID="btnResetSearch" runat="server" CssClass="button" OnClick="btnResetSearch_Click"
                                Text="<%$ Resources:Labels, Clear %>" />
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <br />
                <asp:ABFGridView runat="server" ID="gvUsers" GridViewStyle="BlueStyle" DataKeyNames="userID"
                    OnSelectedIndexChanging="gvUsers_SelectedIndexChanging" OnRowDeleting="gvUsers_RowDeleting"
                    OnPageIndexChanging="gvUsers_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="<%$Resources:Labels,UserName %>" />
                        <asp:BoundField DataField="EmployeeName" HeaderText="<%$Resources:Labels,EmployeeName %>" />
                        <asp:BoundField DataField="Email" HeaderText="<%$Resources:Labels,Email %>" />
                        <asp:BoundField DataField="RoleName" HeaderText="<%$Resources:Labels,Group %>" />
                        <asp:BoundField DataField="LastActivityDate" HeaderText="<%$Resources:Labels,LastActivityDate %>"
                            DataFormatString="{0:d/M/yyyy hh:mm tt}" />
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Edit %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" ImageUrl="../images/edit_grid.gif" runat="server"
                                    CommandName="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Labels, Delete %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../images/delete_grid.gif" runat="server"
                                    CommandName="Delete" Visible='<%# Convert.ToBoolean( Eval("userID").ToString() != "3fe2b6eb-cce6-404d-af1b-9751376c3683") %>'
                                    OnClientClick="return ConfirmSure();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:ABFGridView>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

    <asp:HiddenField ID="hfmpeCreateUser" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateUser" runat="server" TargetControlID="TabContainer1$TabPanel1$lnkAddNewUser"
        PopupControlID="pnlAdd" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAdd" CssClass="pnlPopUp" runat="server" Width="600">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="close_popup" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=this.MyContext.PageData.PageTitle %></span>
        </div>
        <div class="content">
            <div class="form">
                <div class="right_col">
                    <label>
                        <%=Resources.Labels.Name %></label>
                    <asp:ABFTextBox ID="txtNewUserName" CssClass="field" runat="server"
                        IsRequired="true" ValidationGroup="NewUser" ErrorMessage="<%$Resources:Validations,rfvUserName %>">
                    </asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.Password %></label>
                    <asp:ABFTextBox ID="txtPassword" CssClass="field" runat="server" IsRequired="true"
                        ValidationGroup="NewUser" ErrorMessage="<%$Resources:Validations,rfvPassword %>"
                        TextMode="Password">
                    </asp:ABFTextBox>
                    <label>
                        <%=Resources.Labels.EmployeeName %></label>
                    <asp:AutoComplete runat="server" ID="acEmployeeName" ServiceMethod="GetUsersEmployeeNames"
                        IsRequired="true" ValidationGroup="NewUser" ErrorMessage="<%$ Resources:Validations, rfvEmployeeName %>"
                        InlineRender="true"></asp:AutoComplete>
                </div>
                <div class="left_col">
                    <label>
                        <%=Resources.Labels.Email %></label>
                    <asp:ABFTextBox ID="txtEmail" CssClass="field" runat="server" IsRequired="true"
                        DataType="Email" ValidationGroup="NewUser" ErrorMessage="<%$Resources:Validations,rfvEmail %>">
                    </asp:ABFTextBox>

                    <label>
                        <%=Resources.Labels.FavLang %></label>
                    <asp:DropDownList ID="ddlFavLang" CssClass="field" runat="server">
                        <asp:ListItem Text="<%$Resources:Labels,Arabic %>" Value="0">
                        </asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Labels,English %>" Value="1">
                        </asp:ListItem>
                    </asp:DropDownList>
                    <label>
                        <%=Resources.Labels.Group %></label>
                    <asp:AutoComplete runat="server" ID="acGroup" ServiceMethod="GetGroupNames"
                        IsRequired="true" ValidationGroup="NewUser" ErrorMessage="<%$ Resources:Validations, rfvGroup %>"
                        InlineRender="true"></asp:AutoComplete>
                </div>

            </div>
            <div style="clear: both;">
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary runat="server" ValidationGroup="NewUser" />
            </div>
            <br />
            <div class="btnDiv">
                <asp:Button ID="btnSaveNewUser" CssClass="button default_button" runat="server" OnClick="btnSaveNewUser_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewUser" />
                <asp:Button ID="BtnClearNewUser" runat="server" CssClass="button" OnClick="BtnClearNewUser_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="hfmpeCreateRole" runat="server" />
    <asp:ModalPopupExtender ID="mpeCreateRole" runat="server" TargetControlID="TabContainer1$TabPanel2$lnkAddNewRole"
        PopupControlID="pnlAddNewRole" BackgroundCssClass="modal_bg" RepositionMode="RepositionOnWindowResize"
        BehaviorID="showPopUp" Y="200">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlAddNewRole" CssClass="pnlPopUp" runat="server" Width="280">
        <div class="tcat">
            <asp:Button runat="server" class="close-btn" ID="Button1" OnClick="ClosePopup_Click"></asp:Button>
            <span>
                <%=Resources.Labels.Groups%></span>
        </div>
        <div class="content">
            <div class="form">
                <label>
                    <%=Resources.Labels.Name %></label>
                <asp:ABFTextBox ID="txtRoleName" CssClass="field" runat="server"
                    IsRequired="true" ValidationGroup="NewRole" ErrorMessage="<%$Resources:Validations,rfvName %>">
                </asp:ABFTextBox>
            </div>
            <div class="validationSummary">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="NewRole" />
            </div>
            <div class="btnDiv">
                <asp:Button ID="btnSaveRole" CssClass="button default_button" runat="server" OnClick="btnSaveRole_click"
                    Text="<%$ Resources:Labels, Save %>" ValidationGroup="NewRole" />
                <asp:Button ID="BtnClearRole" runat="server" CssClass="button" OnClick="BtnClearRole_click"
                    Text="<%$ Resources:Labels, Clear %>" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
