<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintWeb.aspx.cs" Inherits="ProjectToYou.PrintWeb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .td1 {
            height: 70px;
            vertical-align: bottom;
            font-size: 24px;
        }

        .td2 {
            width: 80px;
            height: 70px;
            vertical-align: bottom;
            font-size: 24px;
        }

        .td3 {
            width: 200px;
            height: 70px;
            vertical-align: bottom;
            font-size: 24px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div><table><tr><td class="td1" colspan="3"></td></tr>
                <tr><td class="td2"></td><td class="td3"></td><td class="td2">mm</td></tr>
                <tr><td class="td2"></td><td class="td3"></td><td class="td2">Kg</td></tr>
                <tr><td class="td2"></td><td class="td3"></td><td class="td2">Kg</td></tr>
                <tr><td class="td2"></td><td class="td3"></td><td class="td2">Kg</td></tr>
                <tr><td class="td2"></td><td class="td3"></td><td class="td2"></td></tr>
                <tr><td class="td2"></td><td class="td3"></td><td class="td2"></td></tr>
                <tr><td class="td1" colspan="3"></td></tr></table></div>
    </form>
</body>
</html>
