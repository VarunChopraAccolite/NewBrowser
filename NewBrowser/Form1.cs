using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public Form1()
        {
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void ChromeBrowser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            isBrowserInitialized = e.IsBrowserInitialized;
        }
    }
}
