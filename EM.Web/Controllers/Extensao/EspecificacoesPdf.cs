﻿using EM.Domain.Aluno;
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
        public static void DefineEstiloData(this PdfContentByte cb, string texto, float posX, float posY)
        {
            BaseFont fonte = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(fonte, 15);

            cb.SetRGBColorFill(0, 0, 0); // Cor preta

            cb.ShowTextAligned(Element.ALIGN_LEFT, texto, posX, posY, 0);
        }

        public static string CalcularIdade(this Aluno aluno)
        {
            DateTime dataNascimento = aluno.Nascimento;
            TimeSpan diferenca = DateTime.Now - dataNascimento;
            int anos = (int)(diferenca.Days / 365.25);
            int meses = (int)((diferenca.Days % 365.25) / 30.4375);
            int dias = (int)(diferenca.Days % 365.25 % 30.4375);

            return $"{anos} Anos, {meses}m {dias}d";
        }
        public static IEnumerable<Aluno> Ordenar(this IEnumerable<Aluno> alunos, string criterio)
        {
            switch (criterio)
            {
                case "Estado":
                    return alunos.OrdenarPorEstado();
                case "Nome":
                    return alunos.OrdenarPorNome();
                case "Idade":
                    return alunos.OrdenarPorIdade();
                default:
                    throw new ArgumentException("Criterio de ordenação inválido", nameof(criterio));
            }
        }

        public static IEnumerable<Aluno> OrdenarPorEstado(this IEnumerable<Aluno> alunos)
        {
            return alunos.OrderBy(aluno => aluno.Cidade?.Uf);
        }

        public static IEnumerable<Aluno> OrdenarPorNome(this IEnumerable<Aluno> alunos)
        {
            return alunos.OrderBy(aluno => aluno.Nome);
        }

        public static IEnumerable<Aluno> OrdenarPorIdade(this IEnumerable<Aluno> alunos)
        {
            return alunos.OrderBy(aluno => aluno.CalcularIdade());
        }
        public static void ZebrarPDF(this PdfPTable table, BaseColor corPar, BaseColor corImpar)
        {
            bool isPar = false;

            for (int i = 1; i < table.Rows.Count; i++)
            {
                PdfPRow row = table.Rows[i];
                foreach (PdfPCell cell in row.GetCells())
                {
                    cell.BackgroundColor = isPar ? corPar : corImpar;
                }
                isPar = !isPar;
            }

        }

    }

}
