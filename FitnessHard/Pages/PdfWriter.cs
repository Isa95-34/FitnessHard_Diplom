using System;
using System.IO;
using System.Reflection.Metadata;

namespace FitnessHard.Pages
{
    internal class PdfWriter
    {
        private string pdfPath;

        public PdfWriter(string pdfPath)
        {
            this.pdfPath = pdfPath;
        }

        internal static PdfWriter GetInstance(Document doc, FileStream stream)
        {
            throw new NotImplementedException();
        }
    }
}