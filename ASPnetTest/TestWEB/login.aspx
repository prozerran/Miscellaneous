<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="TestWEB.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <p>
        <asp:LoginName ID="LoginName1" runat="server" />
    </p>
    <p>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox7" runat="server" style="margin-top: 0px"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox8" runat="server" style="margin-top: 0px"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox9" runat="server" style="margin-top: 0px"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox10" runat="server" style="margin-top: 0px"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox11" runat="server" style="margin-top: 0px"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="TextBox12" runat="server" style="margin-top: 0px"></asp:TextBox>
    </p>
    <p>
        <asp:Login ID="Login1" runat="server" Height="115px" TitleText="" Width="464px" 
            BackColor="#FFFBD6" BorderColor="#FFDFAD" BorderPadding="4" BorderStyle="Solid" 
            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333" 
            onauthenticate="Login1_Authenticate" oninit="Login1_Init" 
            TextLayout="TextOnTop">
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <LoginButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" />
            <TextBoxStyle Font-Size="0.8em" />
            <TitleTextStyle BackColor="#990000" Font-Bold="True" Font-Size="0.9em" 
                ForeColor="White" />
        </asp:Login>
    </p>
    <p>
        Windows loging displays <strong>DomainName\Username</strong> when IIS has 
        enabled Windows Authentication and disabled Anonymous login<br />
    </p>
    </form>
</body>
</html>
