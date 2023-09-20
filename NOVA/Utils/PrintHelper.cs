using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace NOVA.Utils
{
    public static class PrintHelper
    {
        public static void Print(string FilePath, string PrinterName) 
        {
            try
            {
                string applicationPath = "";

                var printApplicationRegistryPaths = new[]
                {
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Acrobat.exe",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\AcroRD32.exe"
                };

                foreach (var printApplicationRegistryPath in printApplicationRegistryPaths)
                {
                    using (var regKeyAppRoot = Registry.LocalMachine.OpenSubKey(printApplicationRegistryPath))
                    {
                        if (regKeyAppRoot == null)
                        {
                            continue;
                        }

                        applicationPath = (string)regKeyAppRoot.GetValue(null);

                        if (!string.IsNullOrEmpty(applicationPath))
                        {
                            break;
                        }
                    }
                }

                // Print to Acrobat
                const string flagNoSplashScreen = "/s";
                const string flagOpenMinimized = "/h";

                var flagPrintFileToPrinter = string.Format("/n /t \"{0}\" \"{1}\"", FilePath, PrinterName);

                var args = string.Format("{0} {1} {2}", flagNoSplashScreen, flagOpenMinimized, flagPrintFileToPrinter);

                var startInfo = new ProcessStartInfo
                {
                    FileName = applicationPath,
                    Arguments = args,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                var process = Process.Start(startInfo);

                if (process != null)
                {
                    process.WaitForInputIdle();
                    process.CloseMainWindow();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);  
            }
        }
    }
}