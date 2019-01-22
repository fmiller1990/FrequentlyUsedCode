using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Windows.Forms;
using PdfSharp.Drawing.Layout;

namespace FrequentlyUsedCode
{
    //requires PresentationFramework
    //requires PDFsharp
    //using PdfSharp.Pdf
    //using PdfSharp.Drawing;
    //using PdfSharp.Drawing.Layout;
    class WritePDF
    {
        string OutputFileName;

        //prepare outputFile
        private PdfDocument document = new PdfDocument();
        private XFont fontNormal = new XFont("Calibri", 18, XFontStyle.Regular);
        private XFont fontH1 = new XFont("Calibri", 24, XFontStyle.Bold);
        private XFont fontH3 = new XFont("Calibri", 18, XFontStyle.Bold);
        private XRect rectHeadline = new XRect(40, 100, 250, 220);
        private XRect rectHeadlineInputs = new XRect(40, 150, 250, 220);
        private XRect rectInputTextLeft = new XRect(40, 180, 250, 220);
        private XRect rectInputTextMid = new XRect(90, 180, 250, 220);
        private XRect rectInputTextRigth = new XRect(140, 180, 250, 220);
        private XRect rectHeadlineOutputs = new XRect(400, 150, 250, 220);
        private XRect rectOutputTextLeft = new XRect(400, 180, 250, 220);
        private XRect rectOutputTextMid = new XRect(470, 180, 250, 220);

        public WritePDF(string outputFileName) {
            OutputFileName = outputFileName;
        }

        public bool WriteOutputPDF() {
            DesignPDF();

            //Try Write 
            bool stopTryingToSave = false;
            while (!stopTryingToSave) {
                try {
                    //Write
                    document.Save(OutputFileName);
                    stopTryingToSave = true;
                }
                catch (Exception ex) {
                    //TODO Errorhandling
                    //File is currently opened
                    var mbr = System.Windows.Forms.MessageBox.Show("Bitte schließen sie die Datei\n" + OutputFileName + "\nund klicken sie OK oder brechen sie die Ausgabe ab.", "Datei in Verwendung", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning);
                    if (mbr == System.Windows.Forms.DialogResult.Cancel) {
                        MessageBox.Show("Ausgabe abgebrochen");
                        return false;
                    }
                }
            }
            return true;
        }

        private void DesignPDF() {
            foreach (var obj in ObjectCollection) {
                //TODO Logic

                PdfPage page_SB = document.AddPage();
                XGraphics gfx_SB = XGraphics.FromPdfPage(page_SB);
                XTextFormatter tf_SB = new XTextFormatter(gfx_SB);

                //TODO Logic
                //tf_SB.DrawString("Object: " + obj.Name, fontH1, XBrushes.Black, rectHeadline);
                //tf_SB.DrawString("Property: ", fontH3, XBrushes.Black, rectHeadlineInputs);
                //tf_SB.DrawString("Value 1", fontNormal, XBrushes.Black, rectInputTextLeft);
                //tf_SB.DrawString("Value 2", fontNormal, XBrushes.Black, rectInputTextMid);
                //tf_SB.DrawString("Value 3", fontNormal, XBrushes.Black, rectInputTextRigth);
            }
        }
    }
}
