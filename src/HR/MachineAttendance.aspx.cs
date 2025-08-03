using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_MachineAttendance : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtMachineAttendance
    {
        get
        {
            return (DataTable)Session["dtMachineAttendance" + this.WinID];
        }

        set
        {
            Session["dtMachineAttendance" + this.WinID] = value;
        }
    }

    private int EditID
    {
        get
        {
            if (ViewState["EditID"] == null) return 0;
            return (int)ViewState["EditID"];
        }

        set
        {
            ViewState["EditID"] = value;
        }
    }

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
                this.Fill();
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

    protected void FilterEmployees(object sender, EventArgs e)
    {
        try
        {
            acEmployeeSrch.ContextKey = acDepartmentSrch.Value + ",," + acBranch.Value;
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.Fill();
            txtMachineIDSrch.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSrch_Click(object sender, EventArgs e)
    {
        try
        {
            txtFromDateSrch.Clear();
            txtToDateSrch.Clear();
            acEmployeeSrch.Clear();
            ddlStatusSrch.SelectedIndex = 0;
            txtMachineIDSrch.Clear();
            acDepartmentSrch.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            this.FilterEmployees(null, null);
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAttendance.PageIndex = e.NewPageIndex;
            gvAttendance.DataSource = this.dtMachineAttendance;
            gvAttendance.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAttendance_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtMachineAttendance.Select("ID=" + gvAttendance.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            acEmployee.Value = dr["Contact_ID"].ToExpressString();
            txtDate.Text = dr["Date"].ToDate().Value.ToString("d/M/yyyy");
            txtTime.Text = dr["Time"].ToDate().Value.ToString("hh:mm");
            ddlTime.SelectedValue = dr["Time"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            ddlStatus.SelectedValue = dr["DocStatus_ID"].ToExpressString();
            this.EditID = gvAttendance.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAttendance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_MachineTimeSheet_Delete(gvAttendance.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvAttendance.DataKeys[e.RowIndex]["ContactName"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
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
            txtDate.Clear();
            txtTime.Clear();
            acEmployee.Clear();
            ddlStatus.SelectedIndex = 0;
            this.EditID = 0;
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateNew.Show();
            }
            else
            {
                mpeCreateNew.Hide();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveNew_click(object sender, EventArgs e)
    {
        try
        {
            if (txtDate.Text.ToDate() > DateTime.Now.Date.AddDays(2))
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            int result = 0;
            DateTime datetime = DateTime.ParseExact(txtDate.Text + " " + txtTime.Text + " " + ddlTime.SelectedValue, "d/M/yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            DateTime time = DateTime.ParseExact("01/01/1900 " + txtTime.Text + " " + ddlTime.SelectedValue, "d/M/yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            if (this.EditID == 0) //insert
            {
                result = dc.usp_HR_MachineTimeSheet_Insert(acEmployee.Value.ToInt(), null, datetime, txtDate.Text.ToDate(), time, ddlStatus.SelectedValue.ToByte());
            }
            else
            {
                result = dc.usp_HR_MachineTimeSheet_Update(this.EditID, acEmployee.Value.ToInt(), datetime, txtDate.Text.ToDate(), time, ddlStatus.SelectedValue.ToByte());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            if (ddlStatus.SelectedValue == DocStatus.Approved.ToByte().ToExpressString())
            {
                dc.usp_HR_MachineTimeSheet_Approve(null, acEmployee.Value.ToInt(), datetime, txtDate.Text.ToDate(), time);
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, acEmployee.Text, dc);
            this.Fill();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            txtDate.Clear();
            txtTime.Clear();
            acEmployee.Clear();
            ddlStatus.SelectedIndex = 0;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void lnkSync_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (ddlMachines.Items.Count == 0) return;
            HR_Machine MachineData = (from data in dc.HR_Machines where data.ID == int.Parse(ddlMachines.SelectedValue) select data).FirstOrDefault();
            if (MachineData == null) return;

            zkemkeeper.CZKEMClass machine = new zkemkeeper.CZKEMClass();
            int port = MachineData.Port;
            string ip = MachineData.IPAddress;
            int? password = MachineData.Password;
            machine.Disconnect();

            if (password != null) machine.SetCommPassword(password.Value);
            if (!machine.Connect_Net(ip, port))
            {
                throw new Exception("Unable to connect the device, you can try again");
            }

            DataTable TimeSheetDT = new DataTable();
            TimeSheetDT.Columns.Add("DateTime", typeof(DateTime));
            TimeSheetDT.Columns.Add("Date", typeof(DateTime));
            TimeSheetDT.Columns.Add("Time", typeof(DateTime));
            TimeSheetDT.Columns.Add("MachineID", typeof(string));

            int idwTMachineNumber = 0;
            int idwEnrollNumber = 0;
            string idwEnrollNumberStr = string.Empty;
            int idwEMachineNumber = 0;
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int WorkCode = 0;
            DateTime datetime;
            DateTime date;
            DateTime time;
            DateTime currentMachineTime;

            machine.EnableDevice(1, false);//disable the device

            if (machine.ReadGeneralLogData(1))//read all the attendance records to the memory
            {
                if (MachineData.Model == 0)
                {
                    while (machine.GetGeneralLogData(1, ref idwTMachineNumber, ref idwEnrollNumber,
                            ref idwEMachineNumber, ref idwVerifyMode, ref idwInOutMode, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute))//get records from the memory
                    {
                        //if (idwEnrollNumber != MachineID || idwMonth != Convert.ToInt32(ddlMonth.SelectedValue) || idwYear != Convert.ToInt32(ddlYear.SelectedValue)) continue;
                        datetime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, 0);
                        date = new DateTime(idwYear, idwMonth, idwDay);
                        time = new DateTime(1900, 1, 1, idwHour, idwMinute, 0);

                        TimeSheetDT.Rows.Add(datetime, date, time, idwEnrollNumber);
                    }
                }
                else
                {
                    while (machine.SSR_GetGeneralLogData(1, out idwEnrollNumberStr,
                             out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref WorkCode))//get records from the memory
                    {
                        //if (idwEnrollNumber != MachineID || idwMonth != Convert.ToInt32(ddlMonth.SelectedValue) || idwYear != Convert.ToInt32(ddlYear.SelectedValue)) continue;
                        datetime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, 0);
                        date = new DateTime(idwYear, idwMonth, idwDay);
                        time = new DateTime(1900, 1, 1, idwHour, idwMinute, 0);

                        TimeSheetDT.Rows.Add(datetime, date, time, idwEnrollNumberStr);
                    }
                }
                machine.GetDeviceTime(1, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond);
                currentMachineTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
                machine.EnableDevice(1, true);//enable the device
                machine.Disconnect();
            }
            else
            {
                machine.EnableDevice(1, true);//enable the device
                machine.Disconnect();
                throw new Exception("Reading data from machine failed");
            }


            //XpressDataContext dc = new XpressDataContext();
            DateTime? lastSyncDate = MachineData.LastSyncDate;//(from data in dc.usp_Company_Select() where data.ID == 1 select data.LastHrAttSync).FirstOrDefault();

            (from row in TimeSheetDT.AsEnumerable()
             where row.Field<DateTime>("dateTime") <= lastSyncDate
             select row).ToList().ForEach(r => r.Delete());
            TimeSheetDT.AcceptChanges();
            TimeSheetDT.DefaultView.Sort = "datetime asc";
            TimeSheetDT = TimeSheetDT.DefaultView.ToTable();
            if (TimeSheetDT.Rows.Count > 0)
            {
                if (currentMachineTime.Subtract(Convert.ToDateTime(TimeSheetDT.Rows[TimeSheetDT.Rows.Count - 1]["datetime"])).TotalMinutes > 1)
                {
                    this.SaveTimeSheet(TimeSheetDT, dc, MachineData);
                }
                else
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.WaitOneMinuteBeforeSync, string.Empty);
                    trans.Rollback();
                    return;
                }
            }

            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            this.Fill();
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed + ". " + ex.Message, ex);
        }
    }

    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            foreach (DataRow r in this.dtMachineAttendance.Rows)
            {
                if (r["DocStatus_ID"].ToByte() == 1) dc.usp_HR_MachineTimeSheet_Approve(r["ID"].ToInt(), r["Contact_ID"].ToInt(), r["DateTime"].ToDate(), r["Date"].ToDate(), r["Time"].ToDate());
            }
            this.Fill();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    #endregion

    #region Private Methods

    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.FilterEmployees(null, null);

        acDepartmentSrch.ContextKey = string.Empty;
        acEmployee.ContextKey = "E," + MyContext.UserProfile.Branch_ID + ",,";
        ddlMachines.DataSource = dc.HR_Machines.Select(x => x);
        ddlMachines.DataTextField = "MachineName";
        ddlMachines.DataValueField = "ID";
        ddlMachines.DataBind();
        txtFromDateSrch.Text = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).ToString("d/M/yyyy");
    }

    private void Fill()
    {
        byte? DocStatus_ID = ddlStatusSrch.SelectedIndex == 0 ? (byte?)null : ddlStatusSrch.SelectedValue.ToByte();
        this.dtMachineAttendance = dc.usp_HR_MachineTimeSheet_Select(acEmployeeSrch.Value.ToNullableInt(), txtMachineIDSrch.TrimmedText, acDepartmentSrch.Value.ToNullableInt(), txtFromDateSrch.Text.ToDate(), txtToDateSrch.Text.ToDate(), DocStatus_ID, acBranch.Value.ToNullableInt()).CopyToDataTable();
        gvAttendance.DataSource = this.dtMachineAttendance;
        gvAttendance.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvAttendance.Columns[4].Visible = MyContext.PageData.IsEdit;
        gvAttendance.Columns[5].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        btnApprove.Visible = MyContext.PageData.IsApprove;
        ddlStatus.Enabled = MyContext.PageData.IsApprove;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    private void SaveTimeSheet(DataTable TimeSheetDT, XpressDataContext dc, HR_Machine MachineData)
    {
        int result = 0;
        foreach (DataRow row in TimeSheetDT.Rows)
        {
            result = dc.usp_HR_MachineTimeSheet_Insert(null, row["MachineID"].ToExpressString(), row["DateTime"].ToDate(), row["Date"].ToDate(), (DateTime)row["Time"].ToDate(), DocStatus.Current.ToByte());
        }
        MachineData.LastSyncDate = (DateTime)TimeSheetDT.Rows[TimeSheetDT.Rows.Count - 1]["datetime"];
        dc.SubmitChanges();
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvAttendance.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion
}