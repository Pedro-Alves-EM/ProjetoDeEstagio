using iTextSharp5.text.pdf;
using iTextSharp5.text;

namespace EM.Web.Controllers.Extensao
{
    public static class Fontes_cs
    {
        public static void DefineFonteCabecalhoTabela(this PdfPTable table)
        {
            float tamanhoFonteCabecalho = 16f; // Defina o tamanho da fonte do cabeçalho

            foreach (PdfPCell cell in table.Rows[0].GetCells())
            {
                cell.Phrase.Font.Size = tamanhoFonteCabecalho;
            }
        }
        
    }
}
