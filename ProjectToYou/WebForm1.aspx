<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ProjectToYou.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script language="Javascript">
        function preview() {
            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint-->";
            eprnstr = "<!--endprint-->";
            prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
        }


        var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        var LODOP = getLodop();

    </script>
    <script language="javascript" src="LodopFuncs.js"></script>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
    </object>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="button" id="js_print" value="打印" onclick="preview()" />
        </div>
        <div>
            <!--startprint-->
            <div>外层镀锡钢线</div>
            <div style="margin-left: 80px;">0.50</div>
            <div style="margin-left: 80px;">60</div>
            <div style="margin-left: 80px;">1</div>
            <div style="margin-left: 80px;">59</div>
            <div style="margin-left: 80px;">2016-07-22</div>
            <div style="margin-left: 80px;">160722-001</div>
            <!--endprint-->
        </div>
    </form>
</body>
</html>
