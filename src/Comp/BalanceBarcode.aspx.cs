using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Comp_BalanceBarcode : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var lst = dc.SettingBalanceBarcodes.ToList();
            if (lst.Any())
            {
                var SBB = lst.First();
                txtDecimal.Text = SBB.DecimalQtt != null ? SBB.DecimalQtt.Value.ToString() : "0";
                txtFirstCode.Text = SBB.FirstCode != null ? SBB.FirstCode : "0";
                txtLegthWeight.Text = SBB.LegthQty != null ? SBB.LegthQty.Value.ToExpressString() : "0";
                txtBarcode.Text = SBB.LengthBarcode != null ? SBB.LengthBarcode.Value.ToExpressString() : "0";


            }

            var item = dc.Settings.ToList().FirstOrDefault();
           // txtSerialPrefix.Text = item.Parcentage.ToExpressString();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        var lst = dc.SettingBalanceBarcodes.ToList();
        if (!lst.Any())
        {
            SettingBalanceBarcode SBB = new SettingBalanceBarcode();
            SBB.DecimalQtt = txtDecimal.Text.ToInt();
            SBB.FirstCode = txtFirstCode.Text;
            SBB.LegthQty = txtLegthWeight.Text.ToInt();
            SBB.LengthBarcode = txtBarcode.Text.ToInt();
            dc.SettingBalanceBarcodes.InsertOnSubmit(SBB);
            dc.SubmitChanges();
            LogAction(Actions.Add, "اعدادات الميزان", dc);
        }
        else
        {
            var SBB = lst.First();
            SBB.DecimalQtt = txtDecimal.Text.ToInt();
            SBB.FirstCode = txtFirstCode.Text;
            SBB.LegthQty = txtLegthWeight.Text.ToInt();
            SBB.LengthBarcode = txtBarcode.Text.ToInt();
            dc.SubmitChanges();
            LogAction(Actions.Edit, "اعدادات الميزان", dc);
        }

        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, "~/Comp/BalanceBarcode.aspx", PageLinks.ItemsList, PageLinks.Items);

    }
    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        txtDecimal.Text = string.Empty;
        txtFirstCode.Text = string.Empty;
        txtLegthWeight.Text = string.Empty;
        txtBarcode.Text = string.Empty;
    }



    protected void btnSaveSettingsPOss_Click(object sender, EventArgs e)
    {
        var item = dc.Settings.ToList().FirstOrDefault();
        item.Parcentage = txtSerialPrefix.Text.ToDecimalOrDefault();
        dc.SubmitChanges();
    }

}