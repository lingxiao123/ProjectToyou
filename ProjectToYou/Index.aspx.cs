using System;
using System.Web;
using System.Xml;
using FineUI;
using ProjectToYou.Code;
namespace ProjectToYou
{
    public partial class _Index : PageBase
    {
        #region Page_Init

        private string _menuType = "menu";
        private bool _showOnlyNew = false;
        private int _examplesCount = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            HttpCookie menuCookie = Request.Cookies["MenuStyle_v4"];
            if (menuCookie != null)
            {
                _menuType = menuCookie.Value;
            }

            // 从Cookie中读取是否仅显示最新示例
            HttpCookie menuShowOnlyNew = Request.Cookies["ShowOnlyNew_v4"];
            if (menuShowOnlyNew != null)
            {
                _showOnlyNew = Convert.ToBoolean(menuShowOnlyNew.Value);
            }


            if (_menuType == "accordion")
            {
                InitAccordionMenu();
            }
            else
            {
                InitTreeMenu();
            }


            if (_showOnlyNew)
            {
                leftPanel.Title = String.Format("全部菜单（{0}）", _examplesCount);
            }
            else
            {
                leftPanel.Title = String.Format("全部菜单（{0}）", _examplesCount);
            }

        }

        private Accordion InitAccordionMenu()
        {
            Accordion accordionMenu = new Accordion();
            accordionMenu.ID = "accordionMenu";
            accordionMenu.ShowBorder = false;
            accordionMenu.ShowHeader = false;
            leftPanel.Items.Add(accordionMenu);


            XmlDocument xmlDoc = XmlDataSource1.GetXmlDocument();
            XmlNodeList xmlNodes = xmlDoc.SelectNodes("/Tree/TreeNode");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                if (xmlNode.HasChildNodes)
                {
                    AccordionPane accordionPane = new AccordionPane();
                    accordionPane.Title = xmlNode.Attributes["Text"].Value;
                    accordionPane.Layout = Layout.Fit;
                    accordionPane.ShowBorder = false;
                    accordionPane.BodyPadding = "2px 0 0 0";
                    accordionMenu.Items.Add(accordionPane);

                    Tree innerTree = new Tree();
                    innerTree.ShowBorder = false;
                    innerTree.ShowHeader = false;
                    innerTree.EnableIcons = true;
                    innerTree.AutoScroll = true;
                    innerTree.EnableSingleClickExpand = true;
                    accordionPane.Items.Add(innerTree);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(String.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Tree>{0}</Tree>", xmlNode.InnerXml));
                    ResolveXmlDocument(doc);

                    // 绑定AccordionPane内部的树控件
                    innerTree.NodeDataBound += treeMenu_NodeDataBound;
                    innerTree.PreNodeDataBound += treeMenu_PreNodeDataBound;
                    innerTree.DataSource = doc;
                    innerTree.DataBind();
                }
            }

            return accordionMenu;
        }

        private Tree InitTreeMenu()
        {
            Tree treeMenu = new Tree();
            treeMenu.ID = "treeMenu";
            treeMenu.ShowBorder = false;
            treeMenu.ShowHeader = false;
            treeMenu.EnableIcons = true;
            treeMenu.AutoScroll = true;
            treeMenu.EnableSingleClickExpand = true;
            leftPanel.Items.Add(treeMenu);

            XmlDocument doc = XmlDataSource1.GetXmlDocument();
            ResolveXmlDocument(doc);

            // 绑定 XML 数据源到树控件
            treeMenu.NodeDataBound += treeMenu_NodeDataBound;
            treeMenu.PreNodeDataBound += treeMenu_PreNodeDataBound;
            treeMenu.DataSource = doc;
            treeMenu.DataBind();

            return treeMenu;
        }

        #region ResolveXmlDocument

        private void ResolveXmlDocument(XmlDocument doc)
        {
            ResolveXmlDocument(doc, doc.DocumentElement.ChildNodes);
        }

        private int ResolveXmlDocument(XmlDocument doc, XmlNodeList nodes)
        {
            // nodes 中渲染到页面上的节点个数
            int nodeVisibleCount = 0;

            foreach (XmlNode node in nodes)
            {
                // Only process Xml elements (ignore comments, etc)
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlAttribute removedAttr;

                    // 是否叶子节点
                    bool isLeaf = node.ChildNodes.Count == 0;


                    // 所有过滤条件均是对叶子节点而言，而是否显示目录，要看是否存在叶子节点
                    if (isLeaf)
                    {
                        // 如果仅显示最新示例
                        if (_showOnlyNew)
                        {
                            XmlAttribute isNewAttr = node.Attributes["IsNew"];
                            if (isNewAttr == null)
                            {
                                removedAttr = doc.CreateAttribute("Removed");
                                removedAttr.Value = "true";

                                node.Attributes.Append(removedAttr);

                            }
                        }
                    }

                    // 存在子节点
                    if (!isLeaf)
                    {
                        // 递归
                        int childVisibleCount = ResolveXmlDocument(doc, node.ChildNodes);

                        if (childVisibleCount == 0)
                        {
                            removedAttr = doc.CreateAttribute("Removed");
                            removedAttr.Value = "true";

                            node.Attributes.Append(removedAttr);
                        }
                    }


                    removedAttr = node.Attributes["Removed"];
                    if (removedAttr == null)
                    {
                        nodeVisibleCount++;
                    }
                }
            }

            return nodeVisibleCount;
        }

        #endregion

        /// <summary>
        /// 树节点的绑定后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeMenu_NodeDataBound(object sender, TreeNodeEventArgs e)
        {
            string isNewHtml = GetIsNewHtml(e.XmlNode);
            if (!String.IsNullOrEmpty(isNewHtml))
            {
                e.Node.Text += isNewHtml;
            }

            // 如果仅显示最新示例 并且 当前节点不是子节点，则展开当前节点
            if (_showOnlyNew && !e.Node.Leaf)
            {
                e.Node.Expanded = true;
            }

        }


        /// <summary>
        /// 树节点的预绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeMenu_PreNodeDataBound(object sender, TreePreNodeEventArgs e)
        {
            /*
            // 如果仅显示最新示例
            if (showOnlyNew)
            {
                string isNewHtml = GetIsNewHtml(e.XmlNode);
                if (String.IsNullOrEmpty(isNewHtml))
                {
                    e.Cancelled = true;
                }
            }

            // 更新示例总数
            if (e.XmlNode.ChildNodes.Count == 0)
            {
                examplesCount++;
            }
            */

            // 是否叶子节点
            bool isLeaf = e.XmlNode.ChildNodes.Count == 0;

            XmlAttribute removedAttr = e.XmlNode.Attributes["Removed"];
            if (removedAttr != null)
            {
                e.Cancelled = true;
            }

            if (isLeaf && !e.Cancelled)
            {
                _examplesCount++;
            }
        }


        private string GetIsNewHtml(XmlNode node)
        {
            string result = String.Empty;

            XmlAttribute isNewAttr = node.Attributes["IsNew"];
            if (isNewAttr != null)
            {
                if (Convert.ToBoolean(isNewAttr.Value))
                {
                    result = "&nbsp;<span class=\"isnew\">New!</span>";
                }
            }

            return result;
        }


        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定基础数据
                //BindBaseDataSource();
                InitMenuStyleButton();
                InitLangMenuButton();
                InitThemeMenuButton();
                litOnlineUserCount.Text = Application["OnlineUserCount"].ToString();
            }
        }
        private void InitMenuStyleButton()
        {
            string menuStyleID = "MenuStyleTree";

            HttpCookie menuStyleCookie = Request.Cookies["MenuStyle_v4"];
            if (menuStyleCookie != null)
            {
                switch (menuStyleCookie.Value)
                {
                    case "menu":
                        menuStyleID = "MenuStyleTree";
                        break;
                    case "accordion":
                        menuStyleID = "MenuStyleAccordion";
                        break;
                }
            }


            SetSelectedMenuID(MenuStyle, menuStyleID);
        }

        private void InitLangMenuButton()
        {
            string langMenuID = "MenuLangZHCN";

            string langValue = PageManager1.Language.ToString().ToLower();
            switch (langValue)
            {
                case "zh_cn":
                    langMenuID = "MenuLangZHCN";
                    break;
                case "zh_tw":
                    langMenuID = "MenuLangZHTW";
                    break;
                case "en":
                    langMenuID = "MenuLangEN";
                    break;
            }


            SetSelectedMenuID(MenuLang, langMenuID);
        }

        private void InitThemeMenuButton()
        {
            string themeMenuID = "MenuThemeBlue";

            string themeValue = PageManager1.Theme.ToString().ToLower();
            switch (themeValue)
            {
                case "blue":
                    themeMenuID = "MenuThemeBlue";
                    break;
                case "gray":
                    themeMenuID = "MenuThemeGray";
                    break;
                case "access":
                    themeMenuID = "MenuThemeAccess";
                    break;
                case "neptune":
                    themeMenuID = "MenuThemeNeptune";
                    break;
            }

            SetSelectedMenuID(MenuTheme, themeMenuID);
        }

        private void SetSelectedMenuID(MenuButton menuButton, string selectedMenuID)
        {
            foreach (MenuItem item in menuButton.Menu.Items)
            {
                MenuCheckBox menu = (item as MenuCheckBox);
                if (menu != null && menu.ID == selectedMenuID)
                {
                    menu.Checked = true;
                }
                else
                {
                    menu.Checked = false;
                }
            }
        }

        #endregion

        
        public void DoAPI()
        {
            OpenAPI api = new OpenAPI("http://127.0.0.1/TPlus/api/v1/", new Credentials()
            {
                AppKey = "myAppKey",
                AppSecret = "myAppSecret",
                UserName = "demo",
                Password = "",
                AccountNumber = "1",
                LoginDate = DateTime.Now.ToString("yyyy-MM-dd")
            });                                //创建OpenAPI的调用对象(传入服务地址，及认证信息)

            api.GetToken();                   //申请访问授权

            api.Call("doc/Create",            //调用凭证的创建服务
            @"{  
                dto: {
                    ExternalCode:""002"",
                    DocType: { Code: ""记"" }, 
                    VoucherDate: """ + DateTime.Now.ToString("yyyy-MM-dd") + @""",
                    Entrys: [
                    { 
                        Account: { ""Code"": ""1001"" },
                        Currency: { ""Code"": ""RMB"" },
                        Summary: ""提现"", 
                        AmountCr: ""100""
                    },
                    { 
                        Account: { ""Code"": ""1002"" },
                        Currency: { ""Code"": ""RMB"" },
                        Summary: ""提现"", 
                        AmountDr: ""100""
                    }]
                }
            }");
        }
    }
}