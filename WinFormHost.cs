using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Noxico.Engine;
using SystemMessageBox = System.Windows.Forms.MessageBox;

namespace Noxico
{
    public static class Program
    {
        public static int Rows = 25;
        public static int Cols = 80;

        [STAThread]
        public static void Main(string[] args)
        {
            //Switch to Invariant so we get "�1,000.50" instead of "� 1.000,50" or "$1,000.50" by default.
            //Can't do this in certain cases, which should be inapplicable to this program.
            var customCulture =
                (System.Globalization.CultureInfo) System.Globalization.CultureInfo.InvariantCulture.Clone();
            customCulture.NumberFormat.CurrencySymbol = "\x13B";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            if (args.Contains("-spreadem"))
            {
                Mix.SpreadEm();
                return;
            }

            /*
            if (Program.CanWrite())
            {
                try
                {
                    var server = "http://helmet.kafuka.org/noxico/files/";
                    var expectedVersion = Application.ProductVersion.Substring(0, 5);
                    using (var wc = new System.Net.WebClient())
                    {
                        var gotVersion = wc.DownloadString(server + "version.txt");
                        if (gotVersion.Contains(expectedVersion))
                            Program.WriteLine("No update required.");
                        else
                        {
                            var answer = SystemMessageBox.Show("A new version of the game is available. Would you like to download it now?" + Environment.NewLine + Environment.NewLine + "This could take a while.", Application.ProductName, MessageBoxButtons.YesNo);
                            if (answer == DialogResult.Yes)
                            {
                                Application.Run(new UpdateForm());
                                return;
                            }
                        }
                    }
                }
                catch (System.Net.WebException)
                {
                    Program.WriteLine("Couldn't check for updates.");
                }
            }
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
            }
            catch (ObjectDisposedException)
            {
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Write(string format, params object[] arg)
        {
            Console.Write(format, arg);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void WriteLine(object value)
        {
            Console.WriteLine(value.ToString());
        }

        public static bool CanWrite()
        {
            //var fi = new FileInfo(Application.ExecutablePath);
            //var pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            //var isAdmin = UacHelper.IsProcessElevated;
            //var hereIsReadOnly = (fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            try
            {
                var test = "test.txt";
                File.WriteAllText(test, test);
                File.Delete(test);
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception x)
            {
                SystemMessageBox.Show(x.ToString());
                return false;
            }

            return true;
        }
    }
}