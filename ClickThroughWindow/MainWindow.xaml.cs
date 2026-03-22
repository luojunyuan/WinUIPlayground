using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.UI.WindowsAndMessaging;
using WinRT.Interop;
using WinUIEx;
using static Windows.Win32.PInvoke;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClickThroughWindow
{
    internal static partial class PInvokeEx
    {
        // https://github.com/microsoft/CsWin32/issues/528

        [LibraryImport("user32.dll", EntryPoint = "SetWindowLongW", SetLastError = true)]
        private static partial int SetWindowLong32(nint hWnd, int nIndex, int dwNewLong);

        [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        private static partial nint SetWindowLongPtr64(nint hWnd, int nIndex, nint dwNewLong);

        /// <inheritdoc cref="SetWindowLong(HWND, WINDOW_LONG_PTR_INDEX, int)" />
        public static nint SetWindowLong(HWND hWnd, WINDOW_LONG_PTR_INDEX nIndex, nint dwNewLong) =>
            nint.Size == 8
                ? SetWindowLongPtr64(hWnd, (int)nIndex, dwNewLong)
                : unchecked(SetWindowLong32(hWnd, (int)nIndex, (int)dwNewLong));
    }

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        /// <summary>
        /// 设置窗口的 Owner 窗口
        /// </summary>
        public static void SetOwnerWindowEx(nint child, int parent)
        {
            Marshal.SetLastPInvokeError(0);

            if (PInvokeEx.SetWindowLong(new(child), WINDOW_LONG_PTR_INDEX.GWL_HWNDPARENT, parent) == 0 &&
                Marshal.GetLastPInvokeError() != 0)
                throw new Win32Exception();
        }


        public MainWindow()
        {
            InitializeComponent();

            this.SystemBackdrop = new WinUIEx.TransparentTintBackdrop();
            WinUIEx.WindowExtensions.ToggleWindowStyle(this, false, WinUIEx.WindowStyle.TiledWindow);

            var hwnd = new HWND(this.GetWindowHandle());
            this.SetExtendedWindowStyle(ExtendedWindowStyle.Layered | ExtendedWindowStyle.Transparent);
        }
    }
}
