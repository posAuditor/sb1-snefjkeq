using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class CustomControls_ucNewItemDescribed : System.Web.UI.UserControl
{

    XpressDataContext dc = new XpressDataContext();

    public delegate void NewItemDescribedEventHandler(string ItemDescribedID, string Price, int AttID);

    public event NewItemDescribedEventHandler NewItemDescribedCreated;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.mpeCreateItemDescribed.TargetControlID = this.TargetControlID;
        txtItemID.ValidationGroup = txtDescribed.ValidationGroup = txtPrice.ValidationGroup = ValidationSummary1.ValidationGroup = btnSaveDescribed.ValidationGroup = this.ID + "_NewDescribed";
    }

    public string TargetControlID
    {
        get
        {
            return mpeCreateItemDescribed.TargetControlID;
        }
        set
        {
            mpeCreateItemDescribed.TargetControlID = value;
        }
    }
    public string Title
    {
        get;

        set;

    }
    public string ItemID
    {
        get
        {
            return txtItemID.Text;
        }

        set
        {
            txtItemID.Text = value;
        }

    }
    protected void BtnClearAtt_OnClick(object sender, EventArgs e)
    {
        try
        {
            txtDescribed.Clear();

            mpeCreateItemDescribed.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveAtt_OnClick(object sender, EventArgs e)
    {
        try
        {
              
            int result = dc.usp_ItemsDescribed_Insert(txtItemID.Text.ToInt(), txtDescribed.TrimmedText,!string.IsNullOrEmpty(txtPrice.Text)? txtPrice.Text.ToDecimal():0);
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.BatchExists, string.Empty);
                mpeCreateItemDescribed.Show();
                return;
            }
            NewItemDescribedCreated(txtDescribed.TrimmedText, txtPrice.Text, result);
            txtDescribed.Clear();
            txtPrice.Clear();
            
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClosepopup_OnClick(object sender, EventArgs e)
    {
        try
        {
            txtDescribed.Clear();
            mpeCreateItemDescribed.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}