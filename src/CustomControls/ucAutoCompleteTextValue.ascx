<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucAutoCompleteTextValue.ascx.cs"
    Inherits="CustomControls_ucAutocomplete" %>
<div id="AutoCompleteMainContainer" runat="server" class="LabelAutoComplete">
    <label id="label" runat="server">
        <%=this.LabelText %></label>

   
    <asp:TextBox ID="txtAutoCompleteText" autocomplete="off" runat="server" CssClass="a1c462cb-ba6c-435e-a0ab-8813dacb08bcAutoCompleteText _AutoComplete"></asp:TextBox>
    <asp:LinkButton ID="_xxHideControlxx" runat="server" CssClass="hide-control" Text="" Visible="false"></asp:LinkButton>
     
    
      <asp:LinkButton ID="_xxPlusBtnxx" runat="server" CssClass="PlusBtnInclude" Text="" Visible="false"></asp:LinkButton>
     <asp:TextBox ID="hfAutocomplete" runat="server" Text="" CssClass="_AutoComplete"
        Style="display: none;" />
    <asp:RequiredFieldValidator ID="rfvAutoComplete" runat="server" ControlToValidate="hfAutocomplete"
        ForeColor="Red" Enabled="false"  ToolTip="" ErrorMessage="" Width="5px" Display="None"></asp:RequiredFieldValidator>
    <div id="AutocompleteListContainer" runat="server" style="color:black;" class="a1c462cb-ba6c-435e-a0ab-8813dacb08bcAutoCompleteListContainer">
    </div>
</div>
