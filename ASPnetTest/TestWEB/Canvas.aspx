<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Canvas.aspx.cs" Inherits="TestWEB.Canvas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <p>Image to use:</p>
    <img id="scream_jpg" src="img_the_scream.jpg" alt="The Scream" width="220" height="277">
    <img id="scream_png" src="img_the_scream.png" alt="The Scream" width="220" height="277">
    <img id="scream_gif" src="img_the_scream.gif" alt="The Scream" width="220" height="277">
    <img id="scream_tif" src="img_the_scream.tif" alt="Try IE for TIF." width="220" height="277">
    <img id="scream_pct" src="img_the_scream.pct" alt="Try IE for PCT." width="220" height="277">
    <img id="scream_pcx" src="img_the_scream.pcx" alt="Try IE for PCX." width="220" height="277">

    <img id="scream_test" src='IISHandler1.ashx?img=imageId' alt="The Scream" width="220" height="277">

    <p>Canvas to fill:</p>
    <canvas id="canvas_id" width="250" height="300"
    style="border:1px solid #d3d3d3;">
    Your browser does not support the HTML5 canvas tag.</canvas>

    <p>
    <button onclick="DisplayInCanvas('scream_jpg')">Try with JS (JPG)</button>
    <button onclick="DisplayInCanvas('scream_png')">Try with JS (PNG)</button>
    <button onclick="DisplayInCanvas('scream_gif')">Try with JS (GIF)</button>
    <button onclick="DisplayInCanvas('scream_tif')">Try with JS (TIF)</button>
    <button onclick="DisplayInCanvas('scream_pct')">Try with JS (PCT)</button>
    <button onclick="DisplayInCanvas('scream_pcx')">Try with JS (PCX)</button>
    </p>

    <form id="form1" runat="server">
    <div>
            <asp:Button ID="Button1" runat="server" OnClientClick="CallWebMethod(); return false;"
            Text="Try with ASP.NET (JPG)" onclick="Button1_Click" />
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>

    <script type="text/javascript">
        function DisplayInCanvas(img_src) {
            var c = document.getElementById("canvas_id");
            var ctx = c.getContext("2d");
            var img = document.getElementById(img_src);
            ctx.drawImage(img, 10, 10);
        }

        function CallWebMethod() {
            PageMethods.CallWebMethod("", onSucess, onError);
            function onSucess(result) {
                alert('onSucess');
                var c = document.getElementById("canvas_id");
                var ctx = c.getContext("2d");
                var img = result;
                ctx.drawImage(img, 10, 10);
            }
            function onError(result) {
                alert('onError');
                var c = document.getElementById("canvas_id");
                var ctx = c.getContext("2d");
                var img = result;
                ctx.drawImage(img, 10, 10);
            }
        }

    </script>

</body>
</html>
