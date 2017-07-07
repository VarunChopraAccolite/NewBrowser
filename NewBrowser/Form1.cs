using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace NewBrowser
{
    public partial class Form1 : Form
    {
        public ChromiumWebBrowser chromeBrowser;
        public bool isBrowserInitialized;
        private DateTime _startDate;
        private System.Threading.Timer _timer;
        public Form1()
        {
            _startDate = DateTime.Now;
            InitializeComponent();
            
            // At the initialization start chromium
            InitializeChromium();

            // Register an object in javascript named "cefCustomObject" with function of the CefCustomObject class :3
            chromeBrowser.RegisterJsObject("cefCustomObject", new CefCustomObject(chromeBrowser, this));

            // Check browser is Initialized
            chromeBrowser.IsBrowserInitializedChanged += ChromeBrowser_IsBrowserInitializedChanged;
        }

        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();

            // Initialize Cef with the provided Settings
            Cef.Initialize(settings);

            // For Flash install System Wide Flash from link https://get.adobe.com/flashplayer/?no_ab=1

            // Now instead use startUrl as URL we'll use the "page" variable to load our local resource
            var page = $@"{Application.StartupPath}\html-resources\html\index.html";

            if (!File.Exists(page))
            {
                MessageBox.Show("Error The html file doesn't exists : " + page);
            }

            // Create a browser component and provide start url
            chromeBrowser = new ChromiumWebBrowser(page);

            // Add the browser to the form
            this.Controls.Add(chromeBrowser);

            // Make the browser fill the form
            chromeBrowser.Dock = DockStyle.Fill;

            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            chromeBrowser.BrowserSettings = browserSettings;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
            _timer.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogMemoryFootprint();
        }

        private void LogMemoryFootprint()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            _timer = new System.Threading.Timer(FootprintTimerCallback, _startDate, startTimeSpan, periodTimeSpan);
        }

        private void FootprintTimerCallback(object eventArgs)
        {
            string prcName = Process.GetCurrentProcess().ProcessName;
            var counter = new PerformanceCounter("Process", "Working Set - Private", prcName);
            using (StreamWriter sw = File.AppendText("../../Log" + _startDate.ToString("yyyy-dd-M-HH-mm-ss") + ".txt"))
            {
                sw.WriteLine("{0}K", counter.RawValue / 1024);
            }
        }

        private void ChromeBrowser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            isBrowserInitialized = e.IsBrowserInitialized;
        }

        private void generateJSAlertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isBrowserInitialized)
            {
                chromeBrowser.ExecuteScriptAsync("alert('test');");
            }
        }
    }
}
