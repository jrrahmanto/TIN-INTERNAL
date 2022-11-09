using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

/// <summary>
/// Summary description for BarcodeHelper
/// </summary>
public class BarcodeHelper
{
    public BarcodeHelper() { }

    public static void CreateQR(string filename, string plain)
    {
        BarcodeWriter barcodeWriter = new BarcodeWriter();
        EncodingOptions encodingOptions = new EncodingOptions() { Width = 350, Height = 350, Margin = 0, PureBarcode = false };
        encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
        barcodeWriter.Renderer = new BitmapRenderer();
        barcodeWriter.Options = encodingOptions;
        barcodeWriter.Format = BarcodeFormat.QR_CODE;
        Bitmap bitmap = barcodeWriter.Write(plain);
        Bitmap logo = new Bitmap(@"F:\\KBI\\QRCode\\kbi.png");
        Graphics g = Graphics.FromImage(bitmap);
        g.DrawImage(logo, new Point((bitmap.Width - logo.Width) / 2, (bitmap.Height - logo.Height) / 2));
        bitmap.Save(@"F:\KBI\QRCode\" + filename + ".png");
    }
}