function CheckLodop() {
    var oldVersion = LODOP.Version;
    newVerion = "5.0.2.3";
    if (oldVersion == null)
        document.write("<h3><font color='#FF00FF'>打印控件未安装!点击这里<a href='../Lodop/install_lodop.exe' target='_blank' rel='nofollow'>安装</a></font></h3>");
    if (oldVersion < newVerion)
        document.write("<h3><font color='#FF00FF'>打印控件需要升级!点击这里<a href='../Lodop/install_lodop.exe' target='_blank' rel='nofollow'>安装</a></font></h3>");
}