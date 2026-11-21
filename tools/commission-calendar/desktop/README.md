# 画师委托台历 · Windows x64

完整解压 CommissionCalendar-Windows-x64.zip，双击 CommissionCalendar.exe 启动。可以右键为它创建桌面快捷方式。界面、图标和字体随程序提供，参考图由用户自己导入。

需要 .NET Framework 4.6.2 或更新版本，以及 Microsoft Edge WebView2 Evergreen Runtime。如果提示缺少 WebView2，可安装微软提供的 x64 运行时：https://developer.microsoft.com/microsoft-edge/webview2/

窗口可拖动、最大化和缩放。点右上角切换左右双页和上下双页。点日期添加订单和便签；点参考图可放大；在装饰模式中导入和拖动月历贴纸。

订单、便签、参考图及窗口内的布局偏好保存在当前 Windows 用户的 %LOCALAPPDATA%\CommissionCalendar\Profile 中。移动软件文件夹后仍使用同一份数据。需要备份时，先关闭台历，再复制整个 Profile 文件夹。桌面版使用独立数据目录，原网页版的数据不会自动迁移。

开发者可在 Windows 上安装 .NET SDK 和 .NET Framework 4.6.2 targeting pack，在此目录运行 dotnet build -c Release。项目把上级目录的 index.html 内嵌到 EXE，使用 WinForms 提供窗口、WebView2 绘制界面。
