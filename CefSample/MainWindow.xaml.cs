using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CefSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CefHelper.Init();
            var vm = new ViewModel();
            Initialized += vm.Init;
            DataContext = vm;
            InitializeComponent();
        }
    }

    static class CefHelper
    {
        public static void Init()
        {
            var cefSettings = new CefSettings
            {
                UserAgent = "CefSample",
            };
            Cef.Initialize(cefSettings);
        }
    }

    class ViewModel
    {
        public ChromiumWebBrowser Browser { get; private set; }
        public ViewModel()
        {
            Browser = new ChromiumWebBrowser();
        }

        public async void Init(object sender, EventArgs e)
        {
            await WaitBrowserInit();
            // IsBrowserInitialized == trueでないとLoadが失敗する
            Browser.Load("http://www.ugtop.com/spill.shtml");
        }

        private async Task WaitBrowserInit()
        {
            var taskSource = new TaskCompletionSource<bool>();
            if (Browser.IsBrowserInitialized) return;
            Browser.IsBrowserInitializedChanged += (sender, e) =>
            {
                if (taskSource.Task.IsCompleted) return;
                taskSource.SetResult(true);
            };
            await taskSource.Task;
        }
    }
}
