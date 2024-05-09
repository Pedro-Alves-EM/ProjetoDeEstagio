using iTextSharp5.text.pdf;
using iTextSharp5.text;

namespace EM.Web.Controllers.Extensao
{
    public static class Cores
    {
        public static void DefineCorDeFundoTabelaCabecalho(this PdfPCell cell)
        {
            cell.BackgroundColor = new BaseColor(72, 61, 139); // Cor fixa
        }
       
    }
}
