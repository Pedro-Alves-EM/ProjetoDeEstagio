using EM.Domain.Aluno;
using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Web.Controllers.Extensao
{
    public static class EspecificacoesPdf
    {
        public static void DimensaoDocumento(this Document document)
        {
            float alturaA4 = PageSize.A4.Height;
            document.SetPageSize(new Rectangle(0, 0, PageSize.A4.Width, alturaA4));
        }

        // Método de extensão para definir a largura da tabela como uma porcentagem do espaço disponível
        public static void DimensaoTabela(this PdfPTable table, float percentual)
        {
            percentual = 110f;
            table.TotalWidth = table.WidthPercentage = percentual;
        }
        public static string PegueDataHora(this DateTime dateTime)
        {
            string formato = "dd/MM/yyyy HH:mm:ss";

            return dateTime.ToString(formato);
        }
        public static void AdicionarCelula(this PdfPTable table, string texto, int alinhamento)
        {
            PdfPCell cell = new PdfPCell(new Phrase(texto));
            cell.HorizontalAlignment = alinhamento;
            table.AddCell(cell);
        }

        public static string ObterSiglaSexo(this EnumeradorSexo sexo)
        {
            switch (sexo)
            {
                case EnumeradorSexo.Masculino:
                    return "M";
                case EnumeradorSexo.Feminino:
                    return "F";
                default:
                    return ""; // Ou outra lógica, se aplicável
            }
        }
        public static void DefineAlturaCabecalhoTabela(this PdfPTable table)
        {
            float alturaFixa = 35f; 

            foreach (PdfPCell cell in table.Rows.SelectMany(r => r.GetCells()))
            {
                cell.FixedHeight = alturaFixa;

            }
        }
        public static void DefineCorTexto(this PdfPCell cell)
        {
            BaseColor cor = BaseColor.WHITE; // Cor padrão, você pode alterar conforme necessário
            cell.Phrase.Font.Color = cor;
        }
        public static void DefineAlturaCelulaTabela(this PdfPCell cell)
        {
            float alturaFixa = 25f; // Defina a altura fixa desejada para a célula

            cell.FixedHeight = alturaFixa;
        }
        public static void CentralizeCabecalhoTabela(this PdfPTable table)
        {
            foreach (PdfPCell cell in table.Rows[0].GetCells())
            {
                cell.HorizontalAlignment = Element.ALIGN_CENTER; // Centraliza no eixo X
                cell.VerticalAlignment = Element.ALIGN_MIDDLE; // Centraliza no eixo Y
            }
        }
        public static void DefineEstiloData(this PdfContentByte cb)
        {
            BaseFont fonte = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(fonte, 20);

            cb.SetRGBColorFill(0, 0, 0); // Cor preta

            // Exemplo: mudar o texto 'Data:' para 'Data' e posicionar no canto superior esquerdo
            cb.ShowTextAligned(Element.ALIGN_LEFT, "DATA", 485, 805, 0);
        }
    }
}
