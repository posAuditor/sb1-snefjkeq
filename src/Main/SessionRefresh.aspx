<%@ Page Language="C#" %>
<%@ OutputCache NoStore="true" Location="None"  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        meta.Attributes.Add("content", "300;url=SessionRefresh.aspx?ID=" + DateTime.Now.Ticks.ToString());
        Session["_RefreshXpress"] = 1;
        base.OnLoad(e);
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <meta http-equiv="refresh" runat="server" id="meta"  /> 
</head>
<body>

</body>
</html>
