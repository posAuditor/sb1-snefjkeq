using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class CustomControls_ucNewBatch : System.Web.UI.UserControl
{
    XpressDataContext dc = new XpressDataContext();

    public delegate void NewBatchEventHandler(string BatchID, string ProductionDate, string ExpirationDAte, int AttID);

    public event NewBatchEventHandler NewBatchCreated;

    public string TargetControlID
    {
        get
        {
            return mpeCreateBatch.TargetControlID;
        }
        set
        {
            mpeCreateBatch.TargetControlID = value;
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

    public int Y
    {
        get
        {
            return mpeCreateBatch.Y;
        }

        set
        {
            mpeCreateBatch.Y = value;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.mpeCreateBatch.TargetControlID = this.TargetControlID;
        txtItemID.ValidationGroup = txtBatchID.ValidationGroup = ValidationSummary1.ValidationGroup = btnSaveAtt.ValidationGroup =
             txtProductionDate.ValidationGroup = txtExirationDate.ValidationGroup = this.ID + "_NewBatch";
    }

    protected void btnSaveAtt_click(object sender, EventArgs e)
    {
        try
        {
            if (txtExirationDate.Text.ToDate() < txtProductionDate.Text.ToDate())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.ItemBatchProdExpDate, string.Empty);
                mpeCreateBatch.Show();
                return;
            }

            int result = dc.usp_ItemsBatch_Insert(txtItemID.Text.ToInt(), txtBatchID.TrimmedText, txtProductionDate.Text.ToDate(), txtExirationDate.Text.ToDate());
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.BatchExists, string.Empty);
                mpeCreateBatch.Show();
                return;
            }
            NewBatchCreated(txtBatchID.TrimmedText, txtProductionDate.Text, txtExirationDate.Text, result);
            txtBatchID.Clear();
            txtProductionDate.Clear();
            txtExirationDate.Clear();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearAtt_click(object sender, EventArgs e)
    {
        try
        {
            txtBatchID.Clear();
            txtProductionDate.Clear();
            txtExirationDate.Clear();
            mpeCreateBatch.Show();
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
            txtBatchID.Clear();
            txtProductionDate.Clear();
            txtExirationDate.Clear();
            mpeCreateBatch.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}