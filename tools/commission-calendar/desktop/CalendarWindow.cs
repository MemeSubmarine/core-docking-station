using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace CommissionCalendar
{
    internal sealed class CalendarWindow : Form
    {
        private const string Page = "https://calendar.internal.invalid/index.html";
        private readonly WebView2 surface = new WebView2 { Dock = DockStyle.Fill };

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CalendarWindow());
        }

        private CalendarWindow()
        {
            Text = "画师委托台历";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Dpi;
            MinimumSize = new Size(380, 400);
            var workArea = Screen.PrimaryScreen.WorkingArea;
            Size = new Size(Math.Min(1280, workArea.Width), Math.Min(820, workArea.Height));
            Controls.Add(surface);
            Shown += InitializeCalendar;
        }

        private async void InitializeCalendar(object sender, EventArgs args)
        {
            try
            {
                CoreWebView2Environment.SetLoaderDllFolderPath(AppDomain.CurrentDomain.BaseDirectory);
                var profile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CommissionCalendar", "Profile");
                var environment = await CoreWebView2Environment.CreateAsync(null, profile);
                if (IsDisposed) return;
                await surface.EnsureCoreWebView2Async(environment);
                if (IsDisposed) return;
                var web = surface.CoreWebView2;
                web.Settings.IsStatusBarEnabled = false;
                web.Settings.AreHostObjectsAllowed = false;
                web.Settings.IsWebMessageEnabled = false;
                web.NavigationStarting += (s, e) => { e.Cancel = e.Uri != Page; };
                web.NewWindowRequested += (s, e) => { e.Handled = true; };
                web.AddWebResourceRequestedFilter("https://calendar.internal.invalid/*", CoreWebView2WebResourceContext.All);
                web.WebResourceRequested += (s, e) =>
                {
                    var isPage = e.Request.Uri == Page;
                    Stream content = isPage
                        ? Assembly.GetExecutingAssembly().GetManifestResourceStream("Calendar.html")
                        : Stream.Null;
                    e.Response = environment.CreateWebResourceResponse(content,
                        isPage ? 200 : 404, isPage ? "OK" : "Not Found",
                        "Content-Type: text/html; charset=utf-8\r\nCache-Control: no-store");
                };
                web.Navigate(Page);
            }
            catch (WebView2RuntimeNotFoundException)
            {
                if (IsDisposed) return;
                MessageBox.Show(this, "请安装 Microsoft Edge WebView2 Evergreen 运行时（x64），然后重新打开台历。下载地址见压缩包内的使用说明。",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                if (IsDisposed) return;
                MessageBox.Show(this, "台历启动失败。请先完整解压下载包，并确认本地用户目录可以写入。\n\n" + ex.Message,
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
