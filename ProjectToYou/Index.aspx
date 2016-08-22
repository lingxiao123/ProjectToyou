<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ProjectToYou._Index" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>T+电子秤数据采集系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <style>
        #header {
            position: relative;
            padding: 10px 10px 8px;
        }

            #header a.logo {
                display: inline-block;
                margin-right: 5px;
            }

            #header a.title {
                font-weight: bold;
                font-size: 24px;
                text-decoration: none;
                line-height: 36px;
                color: #fff;
            }


            #header .themeroller {
                position: absolute;
                top: 10px;
                right: 10px;
            }

                #header .themeroller a {
                    font-size: 20px;
                    text-decoration: none;
                    line-height: 36px;
                    color: #fff;
                }


        #logo {
            position: absolute;
            bottom: 20px;
            right: 0;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            opacity: 0.8;
            z-index: 100000;
        }

        .isnew {
            color: red;
        }

        .bottomtable {
            width: 100%;
            font-size: 12px;
        }

        /* 修正选项卡标题中放置红色[New!]时，底部出现的一行空白线 */
        .f-theme-neptune .x-tab .x-tab-inner {
            line-height: 16px !important;
        }


        /* 主题相关样式 - neptune */
        .f-theme-neptune #header,
        .f-theme-neptune .bottomtable,
        .f-theme-neptune .x-splitter {
            background-color: #1475BB;
            color: #fff;
        }

            .f-theme-neptune #header a,
            .f-theme-neptune .bottomtable a {
                color: #fff;
            }


        /* 主题相关样式 - blue */
        .f-theme-blue #header,
        .f-theme-blue .bottomtable {
            background-color: #DFE8F6;
            color: #000;
        }

            .f-theme-blue #header a,
            .f-theme-blue .bottomtable a {
                color: #000;
            }

        /* 主题相关样式 - gray */
        .f-theme-gray #header,
        .f-theme-gray .bottomtable {
            background-color: #E0E0E0;
            color: #333;
        }

            .f-theme-gray #header a,
            .f-theme-gray .bottomtable a {
                color: #333;
            }

        /* 主题相关样式 - access */
        .f-theme-access #header,
        .f-theme-access .bottomtable {
            background-color: #3F4757;
            color: #fff;
        }

            .f-theme-access #header a,
            .f-theme-access .bottomtable a {
                color: #fff;
            }

        .f-theme-access .maincontent .x-panel-body {
            background-image: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false"
                    Position="Top" Layout="Fit" runat="server">
                    <Content>
                        <div id="header">
                            <table>
                                <tr>
                                    <td>
                                        <img src="Imgs/tplusstd_skinOne.png" alt="T+电子秤数据采集系统" />
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;T+电子秤数据采集系统
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </Content>
                </f:Region>
                <f:Region ID="leftPanel" RegionSplit="true" Width="220px" ShowHeader="true" ShowBorder="true" Title="菜单栏"
                    EnableCollapse="true" Layout="Fit" Collapsed="false" RegionPosition="Left" runat="server">
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" ShowBorder="false" Position="Center"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                            <Tabs>
                                <f:Tab Title="首页" Layout="Fit" Icon="House" CssClass="maincontent" runat="server">
                                    <Items>
                                        <f:ContentPanel ShowBorder="false" BodyPadding="10px" ShowHeader="false"
                                            runat="server">
                                            <div style="display: none; position: fixed; bottom: 30px; right: 10px; text-align: center; border: solid 1px #ddd; padding: 10px; background-color: #efefef;">
                                                <div style="margin-bottom: 5px;">
                                                    扫描二维码，关注 赣州通友 微信公众号
                                                </div>
                                                <img src="http://fineui.com/images/weixin_fineui.jpg" style="width: 150px;">
                                            </div>
                                        </f:ContentPanel>
                                    </Items>
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>
                <f:Region ID="bottomPanel" RegionPosition="Bottom" ShowBorder="false" ShowHeader="false" EnableCollapse="false" runat="server" Layout="Fit">
                    <Items>
                        <f:ContentPanel runat="server" ShowBorder="false" ShowHeader="false">
                            <table class="bottomtable">
                                <tr>
                                    <td style="width: 300px;"></td>
                                    <td style="text-align: center;">Copyright &copy; 2016 赣州通友科技有限公司</td>
                                    <td style="width: 300px; text-align: right;">在线人数：<asp:Literal runat="server" ID="litOnlineUserCount"></asp:Literal>&nbsp;</td>
                                </tr>
                            </table>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="windowSourceCode" Icon="PageWhiteCode" Title="源代码" Hidden="true" EnableIFrame="true"
            runat="server" IsModal="true" Width="950px" Height="550px" EnableClose="true"
            EnableMaximize="true" EnableResize="true">
        </f:Window>
        <f:Menu ID="menuSettings" runat="server">
            <f:MenuButton ID="btnExpandAll" IconUrl="~/res/images/expand-all.gif" Text="展开菜单" EnablePostBack="false"
                runat="server">
            </f:MenuButton>
            <f:MenuButton ID="btnCollapseAll" IconUrl="~/res/images/collapse-all.gif" Text="折叠菜单"
                EnablePostBack="false" runat="server">
            </f:MenuButton>
            <f:MenuSeparator ID="MenuSeparator4" runat="server">
            </f:MenuSeparator>
            <%--<f:MenuCheckBox runat="server" ID="cbxShowOnlyNew" Text="仅显示最新示例">
            </f:MenuCheckBox>--%>
            <f:MenuSeparator ID="MenuSeparator1" runat="server">
            </f:MenuSeparator>
            <f:MenuButton EnablePostBack="false" Text="菜单样式" ID="MenuStyle" runat="server">
                <Menu runat="server">
                    <f:MenuCheckBox Text="树菜单" ID="MenuStyleTree" Checked="true" GroupName="MenuStyle" runat="server">
                    </f:MenuCheckBox>
                    <f:MenuCheckBox Text="手风琴+树菜单" ID="MenuStyleAccordion" GroupName="MenuStyle" runat="server">
                    </f:MenuCheckBox>
                </Menu>
            </f:MenuButton>
            <f:MenuButton EnablePostBack="false" Text="语言" ID="MenuLang" runat="server">
                <Menu ID="Menu2" runat="server">
                    <f:MenuCheckBox Text="简体中文" ID="MenuLangZHCN" Checked="true" GroupName="MenuLang" runat="server">
                    </f:MenuCheckBox>
                    <f:MenuCheckBox Text="繁體中文" ID="MenuLangZHTW" GroupName="MenuLang" runat="server">
                    </f:MenuCheckBox>
                    <f:MenuCheckBox Text="English" ID="MenuLangEN" GroupName="MenuLang" runat="server">
                    </f:MenuCheckBox>
                </Menu>
            </f:MenuButton>
            <f:MenuButton ID="MenuTheme" EnablePostBack="false" Text="主题" runat="server">
                <Menu ID="Menu4" runat="server">
                    <f:MenuCheckBox Text="Neptune" ID="MenuThemeNeptune" GroupName="MenuTheme" runat="server">
                    </f:MenuCheckBox>
                    <f:MenuCheckBox Text="Blue" ID="MenuThemeBlue" GroupName="MenuTheme" runat="server">
                    </f:MenuCheckBox>
                    <f:MenuCheckBox Text="Gray" ID="MenuThemeGray" GroupName="MenuTheme" runat="server" Checked="true">
                    </f:MenuCheckBox>
                    <f:MenuCheckBox Text="Access" ID="MenuThemeAccess" GroupName="MenuTheme" runat="server">
                    </f:MenuCheckBox>
                </Menu>
            </f:MenuButton>
        </f:Menu>
        <asp:XmlDataSource ID="XmlDataSource1" runat="server" EnableCaching="false" DataFile="~/common/menu.xml"></asp:XmlDataSource>
    </form>
    <script src="./res/js/jquery.min.js"></script>
    <script>

        var btnExpandAllClientID = '<%= btnExpandAll.ClientID %>';
        var btnCollapseAllClientID = '<%= btnCollapseAll.ClientID %>';
        var leftPanelClientID = '<%= leftPanel.ClientID %>';
        var mainTabStripClientID = '<%= mainTabStrip.ClientID %>';
        var windowSourceCodeClientID = '<%= windowSourceCode.ClientID %>';
        var menuSettingsClientID = '<%= menuSettings.ClientID %>';
        <%--var cbxShowOnlyNewClientID = '<%= cbxShowOnlyNew.ClientID %>';--%>

        var MenuStyleClientID = '<%= MenuStyle.ClientID %>';
        var MenuLangClientID = '<%= MenuLang.ClientID %>';
        var MenuThemeClientID = '<%= MenuTheme.ClientID %>';


        F.ready(function () {
            var btnExpandAll = F(btnExpandAllClientID);
            var btnCollapseAll = F(btnCollapseAllClientID);
            var leftPanel = F(leftPanelClientID);
            var mainTabStrip = F(mainTabStripClientID);
            var windowSourceCode = F(windowSourceCodeClientID);
            var menuSettings = F(menuSettingsClientID);
            //var cbxShowOnlyNew = F(cbxShowOnlyNewClientID);

            var MenuStyle = F(MenuStyleClientID);
            var MenuLang = F(MenuLangClientID);
            var MenuTheme = F(MenuThemeClientID);

            var mainMenu = leftPanel.items.getAt(0);
            var menuType = 'accordion';
            if (mainMenu.isXType('treepanel')) {
                menuType = 'menu';
            }

            // 当前展开的手风琴面板
            function getExpandedPanel() {
                var panel = null;
                mainMenu.items.each(function (item) {
                    if (!item.getCollapsed()) {
                        panel = item;
                    }
                });
                return panel;
            }

            // 点击展开菜单
            btnExpandAll.on('click', function () {
                if (menuType == 'menu') {
                    // 左侧为树控件
                    mainMenu.expandAll();
                } else {
                    // 左侧为树控件+手风琴控件
                    var expandedPanel = getExpandedPanel();
                    if (expandedPanel) {
                        expandedPanel.items.getAt(0).expandAll();
                    }
                }
            });

            // 点击折叠菜单
            btnCollapseAll.on('click', function () {
                if (menuType == 'menu') {
                    // 左侧为树控件
                    mainMenu.collapseAll();
                } else {
                    // 左侧为树控件+手风琴控件
                    var expandedPanel = getExpandedPanel();
                    if (expandedPanel) {
                        expandedPanel.items.getAt(0).collapseAll();
                    }
                }
            });


            // 点击菜单样式
            function MenuStyleItemCheckChange(cmp, checked) {
                if (checked) {
                    var menuStyle = 'accordion';
                    if (cmp.id.indexOf('MenuStyleTree') >= 0) {
                        menuStyle = 'tree';
                    }
                    F.cookie('MenuStyle_v4', menuStyle, {
                        expires: 100 // 单位：天
                    });
                    top.window.location.reload();
                }
            }
            MenuStyle.menu.items.each(function (item, index) {
                item.on('checkchange', MenuStyleItemCheckChange);
            });


            // 切换语言
            function MenuLangItemCheckChange(cmp, checked) {
                if (checked) {
                    var lang = 'en';
                    if (cmp.id.indexOf('MenuLangZHCN') >= 0) {
                        lang = 'zh_CN';
                    } else if (cmp.id.indexOf('MenuLangZHTW') >= 0) {
                        lang = 'zh_TW';
                    }

                    F.cookie('Language_v4', lang, {
                        expires: 100 // 单位：天
                    });
                    top.window.location.reload();
                }
            }
            MenuLang.menu.items.each(function (item, index) {
                item.on('checkchange', MenuLangItemCheckChange);
            });


            // 切换主题
            function MenuThemeItemCheckChange(cmp, checked) {
                if (checked) {
                    var lang = 'neptune';
                    if (cmp.id.indexOf('MenuThemeBlue') >= 0) {
                        lang = 'blue';
                    } else if (cmp.id.indexOf('MenuThemeGray') >= 0) {
                        lang = 'gray';
                    } else if (cmp.id.indexOf('MenuThemeAccess') >= 0) {
                        lang = 'access';
                    }

                    F.cookie('Theme_v4', lang, {
                        expires: 100 // 单位：天
                    });
                    top.window.location.reload();
                }
            }
            MenuTheme.menu.items.each(function (item, index) {
                item.on('checkchange', MenuThemeItemCheckChange);
            });


            function createToolbar(tabConfig) {

                // 由工具栏上按钮获得当前标签页中的iframe节点
                function getCurrentIFrameNode(btn) {
                    return $('#' + btn.id).parents('.f-tab').find('iframe');
                }

                var sourcecodeButton = new Ext.Button({
                    text: '源代码',
                    type: 'button',
                    icon: './res/icon/page_white_code.png',
                    listeners: {
                        click: function () {
                            var iframeNode = getCurrentIFrameNode(this);
                            var iframeWnd = iframeNode[0].contentWindow

                            var files = [iframeNode.attr('src')];
                            var sourcefilesNode = $(iframeWnd.document).find('head meta[name=sourcefiles]');
                            if (sourcefilesNode.length) {
                                $.merge(files, sourcefilesNode.attr('content').split(';'));
                            }
                            windowSourceCode.f_show('./common/source.aspx?files=' + encodeURIComponent(files.join(';')));
                        }
                    }
                });

                var openNewWindowButton = new Ext.Button({
                    text: '新标签页中打开',
                    type: 'button',
                    icon: './res/icon/tab_go.png',
                    listeners: {
                        click: function () {
                            var iframeNode = getCurrentIFrameNode(this);
                            window.open(iframeNode.attr('src'), '_blank');
                        }
                    }
                });

                var refreshButton = new Ext.Button({
                    text: '刷新',
                    type: 'button',
                    icon: './res/icon/reload.png',
                    listeners: {
                        click: function () {
                            var iframeNode = getCurrentIFrameNode(this);
                            iframeNode[0].contentWindow.location.reload();
                        }
                    }
                });

                var toolbar = new Ext.Toolbar({
                    items: ['->', sourcecodeButton, '-', refreshButton, '-', openNewWindowButton]
                });

                //tabConfig['tbar'] = toolbar;
            }



            // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
            // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
            // mainTabStrip： 选项卡实例
            // createToolbar： 创建选项卡前的回调函数（接受tabConfig参数）
            // updateLocationHash: 切换Tab时，是否更新地址栏Hash值
            // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
            // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame
            F.util.initTreeTabStrip(mainMenu, mainTabStrip, createToolbar, true, false, false);



            // 添加示例标签页
            window.addExampleTab = function (id, url, text, icon, refreshWhenExist) {
                // 动态添加一个标签页
                // mainTabStrip： 选项卡实例
                // id： 选项卡ID
                // url: 选项卡IFrame地址 
                // text： 选项卡标题
                // icon： 选项卡图标
                // addTabCallback： 创建选项卡前的回调函数（接受tabConfig参数）
                // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
                F.util.addMainTab(mainTabStrip, id, url, text, icon, null, refreshWhenExist);
            };

            // 移除选中标签页
            window.removeActiveTab = function () {
                var activeTab = mainTabStrip.getActiveTab();
                mainTabStrip.removeTab(activeTab.id);
            };


            // 添加工具图标，并在点击时显示上下文菜单
            leftPanel.addTool({
                type: 'gear',
                //tooltip: '系统设置',
                handler: function (event) {
                    menuSettings.showBy(this);
                }
            });

        });

        
    </script>
</body>
</html>
