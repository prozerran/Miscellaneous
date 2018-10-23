<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestWEB._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="">
<head runat="server">
    <title></title>
</head>
<body ng-init="names=['Yee Hsu', 'John Doe', 'Monica', 'Seven']">
    <form id="form1" runat="server">
    <strong>Add/Configure SQL Server ASP.NET Web Deployment on IIS</strong><br />
    <a href="http://stackoverflow.com/questions/11278114/enable-remote-connections-for-sql-server-express-2012">
    Enable remote connection for SQL server</a><br />
    <a href="http://support.webecs.com/kb/a868/how-do-i-configure-sql-server-express-to-allow-remote.aspx">
    Config SQL Sever to allow remote access</a><br />
    <a href="http://brendan.enrick.com/post/Enabling-Mixed-Mode-Authentication-in-SQL-Server">
    Enable mixed-mode Authentication</a><br />
    <a href="http://dbatricksworld.com/how-to-open-firewall-ports-on-windows-server-2008-r2/">
    Open Windows Firewall Port SQL Server port 1433</a><br />
    <a href="http://support.microsoft.com/kb/555332">Configure SQL Authentication</a><br />
    <a href="http://msdn.microsoft.com/en-us/library/015103yb(v=vs.140).aspx?cs-save-lang=1&amp;cs-lang=csharp#code-snippet-1">
    Add ASP.NET Code Behind codes</a><br />
    <a href="http://www.asp.net/web-forms/overview/deployment/configuring-server-environments-for-web-deployment/configuring-a-web-server-for-web-deploy-publishing-(remote-agent)">
    Configure Server Env for Web Deploy</a><br />
    <a href="https://www.youtube.com/watch?v=ewaR-yFmi4w">Youtube video for Web 
    Deploy with SQL schema/data sync</a><br />
    <a href="SomeTest.htm">Test Other HTML stuffs</a><br />
    ---&gt; <a href="login.aspx">LOGIN</a><br />
    <br />
    Windows Login:
    <asp:TextBox ID="TextBox2" runat="server" BackColor="#CCCCCC" ReadOnly="True"></asp:TextBox>
    <asp:Button ID="Button13" runat="server" onclick="Button13_Click" 
        Text="Login using P/Invoke" />
    <asp:Button ID="Button14" runat="server" onclick="Button14_Click" 
        Text="Login using LDAP" />
    <br />
    Password: 
    <asp:TextBox ID="TextBox3" runat="server" TextMode="Password" Width="157px"></asp:TextBox>
    <asp:TextBox ID="TextBox4" runat="server" BackColor="#CCCCCC" ReadOnly="True" 
        Width="330px"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Connect TestDatabase1 C# .NET" />
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Update" />
    <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
        style="margin-bottom: 0px" Text="Empty" />
    <asp:Button ID="Button4" runat="server" onclick="Button4_Click" Text="Clear" />
        <asp:Button ID="Button6" runat="server" Text="Update From Javascript" 
        OnClientClick="UpdateDatabase1FromJS(); return false;" onclick="Button6_Click" />
    <br />
    <asp:Button ID="Button7" runat="server" onclick="Button7_Click" 
        Text="Connect TestDatabase2 VB .NET" />
    <asp:Button ID="Button8" runat="server" onclick="Button8_Click" Text="Update" />
    <asp:Button ID="Button9" runat="server" onclick="Button9_Click" Text="Empty" />
    <asp:Button ID="Button10" runat="server" onclick="Button10_Click" 
        Text="Clear" />
    <asp:Button ID="Button11" runat="server" onclick="Button11_Click" 
        onclientclick="UpdateDatabase2FromJS(); return false;" 
        Text="Update From Javascript" />
    <br />
    <br />
    <asp:Button ID="Button12" runat="server" Text="Get C# .NET Store Procedure" 
        onclick="Button12_Click" />
    <asp:TextBox ID="TextBox1" runat="server" Width="59px" TextMode="Number">0</asp:TextBox>
    <br />

    <script type="text/javascript">
        function HandleIT() 
        {
            var name = document.getElementById('<%=txtname.ClientID %>').value;
            PageMethods.ProcessIT(name, onSucess, onError);
            function onSucess(result) 
            {
                alert(result);
            }
            function onError(result) 
            {
                alert('Something wrong.');
            }
        }

        function UpdateDatabase1FromJS() 
        {
            PageMethods.UpdateDatabase1FromJS();
        }

        function UpdateDatabase2FromJS() 
        {
            PageMethods.UpdateDatabase2FromJS();
        }
    </script>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <br />
        <br />
        Name&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtname" runat="server"></asp:TextBox>
        <asp:Button ID="btnCreateAccount" runat="server" Text="Signup" OnClientClick="HandleIT(); return false;" />

        <br />

        <!-- Update Database -->
    
    </div class="container">
        Angular JS Demonstration : 
        <input type="text" ng-model="name" /> {{ name }}
        <ul>
            <li ng-repeat="personName in names">{{ personName }}</li>
        </ul>
        <script src="Scripts/angular.min.js"></script>
    </div>

    <br />
    <br />

    Results Here:<br />
    <br />
    <asp:GridView ID="GridView1" runat="server" EnableModelValidation="True">
    </asp:GridView>

    </form>
</body>
</html>
