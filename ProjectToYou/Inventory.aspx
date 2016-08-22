<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="ProjectToYou._Inventory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="Scripts/jquery.js"></script>
    <script type="text/javascript" src="Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="Lodop/LodopFuncs.js"></script>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0"
        height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0" pluginspage="install_lodop32.exe"></embed>
    </object>
    <style type="text/css">
        .BigLab span {
            font-size:50px;
        }
    </style>
</head>
<body>
    <script>
        var LODOP; //声明为全局变量    
        function PrintIframeByURL() {
            LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
            LODOP.PRINT_INIT("打印");
            LODOP.ADD_PRINT_URL(0, 0, "100%", "100%", "ImgWeb.aspx");
            LODOP.PRINT();
        };
        document.onkeyup = function (event) {
            var e = event || window.event;
            var keyCode = e.keyCode || e.which;
            switch (keyCode) {
                case 49:
                    document.getElementById("btnGetWeight-btnInnerEl").click();
                    //$("#btnGetWeight-btnInnerEl").click();
                    break;
                case 50:
                    //$("#btnGetWeight2-btnInnerEl").click();
                    document.getElementById("btnGetWeight2-btnInnerEl").click();
                    break;
                case 51:
                    //$("#btnPrint-btnInnerEl").click();
                    document.getElementById("btnPrint-btnInnerEl").click();
                    break;
                case 52:
                    //$("#btnAddInfo-btnInnerEl").click();
                    document.getElementById("btnAddInfo-btnInnerEl").click();
                    break;
                case 53:
                    //$("#Panel1_panelCenterRegion_sform2_row5_grdProductList_toolb1_btnImport-btnInnerEl").click();
                    document.getElementById("Panel1_panelCenterRegion_sform2_row5_grdProductList_toolb1_btnImport-btnInnerEl").click();
                    break;
                default:
                    break;
            }
        }
        function printPage(preview) {
            try {
                var content = window.document.body.innerHTML;
                var oricontent = content;
                while (content.indexOf("{$printhide}") >= 0) content = content.replace("{$printhide}", "style='display:none'");
                if (content.indexOf("ID=\"PrintControl\"") < 0) content = content + "<OBJECT ID=\"PrintControl\" WIDTH=0 HEIGHT=0 CLASSID=\"CLSID:8856F961-340A-11D0-A96B-00C04FD705A2\"></OBJECT>";
                window.document.body.innerHTML = content;
                //PrintControl.ExecWB(7,1)打印预览，(1,1)打开，(4,1)另存为，(17,1)全选，(10,1)属性，(6,1)打印，(6,6)直接打印，(8,1)页面设置 
                if (preview == null || preview == false) PrintControl.ExecWB(6, 1);
                else PrintControl.ExecWB(7, 1); //OLECMDID_PRINT=7; OLECMDEXECOPT_DONTPROMPTUSER=6/OLECMDEXECOPT_PROMPTUSER=1 
                window.document.body.innerHTML = oricontent;
            }
            catch (ex) { alert("执行Javascript脚本出错。"); }
        }
        function printConten(preview, html) {
            try {
                var content = html;
                var oricontent = window.document.body.innerHTML;
                while (content.indexOf("{$printhide}") >= 0) content = content.replace("{$printhide}", "style='display:none'");
                if (content.indexOf("ID=\"PrintControl\"") < 0) content = content + "<OBJECT ID=\"PrintControl\" WIDTH=0 HEIGHT=0 CLASSID=\"CLSID:8856F961-340A-11D0-A96B-00C04FD705A2\"></OBJECT>";
                window.document.body.innerHTML = content;
                //PrintControl.ExecWB(7,1)打印预览，(1,1)打开，(4,1)另存为，(17,1)全选，(10,1)属性，(6,1)打印，(6,6)直接打印，(8,1)页面设置 
                if (preview == null || preview == false) PrintControl.ExecWB(6, 1);
                else PrintControl.ExecWB(7, 1); //OLECMDID_PRINT=7; OLECMDEXECOPT_DONTPROMPTUSER=6/OLECMDEXECOPT_PROMPTUSER=1 
                window.document.body.innerHTML = oricontent;
            }
            catch (ex) { alert("执行Javascript脚本出错。"); }
        }
        function Print(preview) {
            var text = document.getElementById("content").innerHTML;
            printConten(preview, text);
        }
        function danji() {
            $.post("ImgWeb.aspx", {}, function (data) {
                WebBrowser.ExecWB(6, 6);
            })
        }
    </script>
    <object id="WebBrowser" width="0" height="0" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2">
    </object>
    <form id="form1" runat="server">
        <f:Timer ID="timerGet" runat="server" Enabled="true" EnableAjaxLoading="false" EnableAjax="true" Interval="1" OnTick="timerGet_Tick"></f:Timer>
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region">
            <Items>
                <f:Panel runat="server" ID="panelLeftRegion" RegionPosition="top" RegionSplit="true" EnableCollapse="false"
                    Width="210px" Title="选择数据" ShowBorder="true" ShowHeader="true" BodyPadding="5px">
                    <Items>
                        <f:Form ID="sform1" runat="server" ShowHeader="false" ShowBorder="true" BodyPadding="5px">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlBusType" runat="server" Label="业务类型" LabelWidth="100px"></f:DropDownList>
                                        <f:DropDownList ID="ddlStore" runat="server" Label="仓库" LabelWidth="100px"></f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlInventory" runat="server" Label="存货" LabelWidth="100px" AutoPostBack="true"
                                            CompareType="String" EnableEdit="true" OnSelectedIndexChanged="ddlInventory_SelectedIndexChanged">
                                        </f:DropDownList>
                                        <f:DropDownList ID="ddlDep" runat="server" Label="生产车间" LabelWidth="100px"></f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:Panel>
                <f:Panel runat="server" ID="panelCenterRegion" RegionPosition="Center" RegionSplit="true" EnableCollapse="false"
                    Width="80%" Title="数据采集" ShowBorder="true" ShowHeader="true" BodyPadding="5px">
                    <Items>
                        <f:Form ID="sform2" runat="server" ShowHeader="false" ShowBorder="true" BodyPadding="5px">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></f:ToolbarSeparator>
                                        <f:Button ID="btnGetWeight" runat="server" Text="准备接收(1)" ClientIDMode="Static" EnablePostBack="true"
                                            OnClick="btnGetWeight_Click" Icon="PageCopy">
                                        </f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator6" runat="server"></f:ToolbarSeparator>
                                        <f:Button ID="btnGetWeight2" runat="server" Text="获取数量(2)" ClientIDMode="Static" EnablePostBack="true"
                                            OnClick="btnGetWeight2_Click" Icon="PageCopy">
                                        </f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></f:ToolbarSeparator>
                                        <f:Button ID="btnPrint" runat="server" Text="打印(3)" OnClientClick="PrintIframeByURL()" ClientIDMode="Static" Icon="Printer"></f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator7" runat="server"></f:ToolbarSeparator>
                                        <f:Button ID="btnAddInfo" runat="server" Text="保存(4)" OnClick="btnAddInfo_Click" ClientIDMode="Static" Icon="SystemSave"></f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></f:ToolbarSeparator>
                                        <%--<f:Button ID="btnPrint2" runat="server" Text="打印" OnClientClick="danji()"></f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator8" runat="server"></f:ToolbarSeparator>
                                        <f:Button ID="btnPrint3" runat="server" Text="打印lodop" OnClientClick="PrintIframeByURL()"></f:Button>--%>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Rows>
                                <f:FormRow ID="FormRow2" runat="server">
                                    <Items>
                                        <f:TextArea ID="txtLog" runat="server" Height="150px" Label="Label" Text="程序运行日志：" ShowLabel="false"></f:TextArea>
                                        <f:ContentPanel ID="ContentPanel2" runat="server" ShowBorder="false" Height="150px" ShowHeader="false">
                                            <br />
                                                <f:Label ID="lblWeight" runat="server" ColumnWidth="200px" CssClass="BigLab" Text="232" Label="重量：" ShowLabel="true" EncodeText="false" Height="150px" Width="300px"></f:Label>
                                            
                                        </f:ContentPanel>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="row4" runat="server">
                                    <Items>
                                        <f:TextBox ID="txtTWeight" runat="server" Label="桶(箱)重" LabelWidth="100px" AutoPostBack="true"
                                            OnTextChanged="txtWeight_TextChanged">
                                        </f:TextBox>
                                        <f:TextBox ID="txtJWeight" runat="server" Label="净重" LabelWidth="100px" AutoPostBack="true"
                                            OnTextChanged="txtWeight_TextChanged" Readonly="true">
                                        </f:TextBox>
                                        <f:TextBox ID="txtSpecification" runat="server" Label="规格型号" LabelWidth="100px" AutoPostBack="true"
                                            ColumnWidth="300px" OnTextChanged="txtWeight_TextChanged" Readonly="true">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="row1" runat="server">
                                    <Items>
                                        <f:TextBox ID="txtName" runat="server" Label="产品名称" Text="" Readonly="true" CompareMessage="请选择存货"></f:TextBox>
                                        <f:TextBox ID="txtSNo" runat="server" Label="批号" LabelWidth="100px" Readonly="true"></f:TextBox>
                                        <f:TextBox ID="txtWeight" runat="server" Label="重量" LabelWidth="100px" AutoPostBack="true"
                                            OnTextChanged="txtWeight_TextChanged">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="row2" runat="server">
                                    <Items>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="row3" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlSerialName" runat="server" Label="当前串口"></f:DropDownList>
                                        <f:TextBox ID="txtUserID" runat="server" Label="T+登录名" Text="18879715660"></f:TextBox>
                                        <f:TextBox ID="txtDate" runat="server" Label="生产日期" LabelWidth="100px" Readonly="true" AutoPostBack="true">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="row5" runat="server">
                                    <Items>
                                        <f:Grid ID="grdProductList" runat="server" Title="采集结果" ShowHeader="true" ShowBorder="true"
                                            AllowSorting="true" DataKeyNames="id,Pro_code" EnableRowLines="true" EnableTextSelection="true">
                                            <Toolbars>
                                                <f:Toolbar ID="toolb1" runat="server">
                                                    <Items>
                                                        <f:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></f:ToolbarSeparator>
                                                        <f:Button ID="btnImport" runat="server" Text="产成品入库单批量导入T+(5)"
                                                            OnClick="btnImport_Click" Icon="Accept">
                                                        </f:Button>
                                                        <f:ToolbarSeparator ID="ToolbarSeparator5" runat="server"></f:ToolbarSeparator>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Columns>
                                                <f:BoundField ID="id" DataField="id" HeaderText="数据库序号"></f:BoundField>
                                                <f:BoundField ID="Pro_code" DataField="Pro_code" HeaderText="产品编码"></f:BoundField>
                                                <f:BoundField ID="Pro_name" DataField="Pro_name" HeaderText="产品名称"></f:BoundField>
                                                <f:BoundField ID="Pro_store" DataField="Pro_store" HeaderText="仓库"></f:BoundField>
                                                <f:BoundField ID="Pro_unit" DataField="Pro_unit" HeaderText="计量单位"></f:BoundField>
                                                <f:BoundField ID="Pro_count" DataField="Pro_count" HeaderText="数量"></f:BoundField>
                                                <f:BoundField ID="Pro_dep" DataField="Pro_dep" HeaderText="生产车间"></f:BoundField>
                                                <f:BoundField ID="Pro_bustype" DataField="Pro_bustype" HeaderText="业务类型"></f:BoundField>
                                                <f:BoundField ID="Pro_sno" DataField="Pro_sno" HeaderText="批号"></f:BoundField>
                                            </Columns>
                                        </f:Grid>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:Panel>
                <%--<f:Panel runat="server" ID="panelBottomRegion" RegionPosition="Bottom" RegionSplit="true" EnableCollapse="false"
                    Width="80%" Title="打印区域" ShowBorder="true" ShowHeader="true" BodyPadding="5px">
                    <Content>
                        <div id="PrintDiv" runat="server">
                            <img id="img1" runat="server" src="ImgWeb.aspx" />
                        </div>
                    </Content>
                </f:Panel>--%>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
