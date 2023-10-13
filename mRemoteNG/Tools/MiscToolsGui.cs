using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using System.Drawing;
using System;
using System.Security;
using System.IO;
using mRemoteNG.Properties;
using System.Runtime.Versioning;
using System.Drawing.Imaging;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public static class MiscToolsGui
    {
        public static Icon GetIconFromFile(string FileName)
        {
            try
            {
                return File.Exists(FileName) == false ? null : Icon.ExtractAssociatedIcon(FileName);
            }
            catch (ArgumentException AEx)
            {
                RuntimeCommon.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "GetIconFromFile failed (Tools.Misc) - using default icon" +
                                                    Environment.NewLine + AEx.Message,
                                                    true);
                return Properties.Resources.mRemoteNG_Icon;
            }
            catch (Exception ex)
            {
                RuntimeCommon.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "GetIconFromFile failed (Tools.Misc)" + Environment.NewLine +
                                                    ex.Message, true);
                return null;
            }
        }

        public static Image TakeScreenshot(UI.Tabs.ConnectionTab sender)
        {
            try
            {
                if (sender != null)
                {
                    var bmp = new Bitmap(sender.Width, sender.Height, PixelFormat.Format32bppRgb);
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(sender.PointToScreen(System.Drawing.Point.Empty), System.Drawing.Point.Empty, bmp.Size, CopyPixelOperation.SourceCopy);
                    return bmp;
                }
            }
            catch (Exception ex)
            {
                RuntimeCommon.MessageCollector.AddExceptionStackTrace("Taking Screenshot failed", ex);
            }

            return null;
        }


        public static Optional<SecureString> PasswordDialog(string passwordName = null, bool verify = true)
        {
            //var splash = FrmSplashScreenNew.GetInstance();
            //TODO: something not right there 
            //if (PresentationSource.FromVisual(splash))
            //    splash.Close();

            var passwordForm = new FrmPassword(passwordName, verify);
            return passwordForm.GetKey();
        }
    }
}