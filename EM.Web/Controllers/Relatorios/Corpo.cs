using EM.Domain.Aluno;
using EM.Repository;
using EM.Web.Controllers.Extensao;
using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Web.Controllers.Relatorios
{
    public class Corpo
    {
        private readonly IRepositorio<Aluno> _repositorioAluno;
        private readonly PdfWriter _writer; // Adicionando o campo para armazenar o PdfWriter

        public Corpo(IRepositorio<Aluno> repositorioAluno, PdfWriter writer)
        {
            _repositorioAluno = repositorioAluno;
            _writer = writer;
        }

        public PdfPTable CriarTabelaDados()
        {
            PdfPTable table = new PdfPTable(6);
            table.DimensaoTabela(110f);
            table.SpacingBefore = 20;
            PdfContentByte cb = _writer.DirectContent;
            cb.SetRGBColorStroke(0, 0, 0);
            cb.SetLineWidth(1f);

            cb.RoundRectangle(445, 775, 130, 50, 10);
            cb.Stroke();

            cb.BeginText();
            cb.SetRGBColorFill(0, 0, 0);

            float tamanhoFonte = 10f;
            cb.SetFontAndSize(BaseFont.CreateFont(), tamanhoFonte);


            DateTime agora = DateTime.Now;
            string dataHoraFormatada = agora.PegueDataHora();

            float larguraTexto = cb.GetEffectiveStringWidth(dataHoraFormatada, false);

            float posX = 440 + (145 - larguraTexto) / 2;

            cb.ShowTextAligned(Element.ALIGN_LEFT, dataHoraFormatada, posX, 790, 0);

            cb.DefineEstiloData();
            
            cb.EndText();

            Font fonteFormatadaCabecalho = new Font(Font.FontFamily.COURIER, 24f);
            

            // Definindo a largura das colunas
            float[] columnWidths = { 1.4f, 3f, 1.8f, 0.5f, 1.3f, 0.7f };
            table.SetWidths(columnWidths);

            // Adicionando células com texto centralizado
            PdfPCell celulaTituloTabela;
           

            celulaTituloTabela = new PdfPCell(new Phrase("MATRÍCULA", fonteFormatadaCabecalho));
            celulaTituloTabela.HorizontalAlignment = Element.ALIGN_CENTER;
            celulaTituloTabela.DefineCorDeFundoTabelaCabecalho();
            celulaTituloTabela.DefineCorTexto();
            table.AddCell(celulaTituloTabela);

            celulaTituloTabela = new PdfPCell(new Phrase("NOME", fonteFormatadaCabecalho));
            celulaTituloTabela.HorizontalAlignment = Element.ALIGN_LEFT;
            celulaTituloTabela.DefineCorDeFundoTabelaCabecalho();
            celulaTituloTabela.DefineCorTexto();
            table.AddCell(celulaTituloTabela);

            celulaTituloTabela = new PdfPCell(new Phrase("CPF", fonteFormatadaCabecalho));
            celulaTituloTabela.HorizontalAlignment = Element.ALIGN_CENTER;
            celulaTituloTabela.DefineCorDeFundoTabelaCabecalho();
            celulaTituloTabela.DefineCorTexto();
            table.AddCell(celulaTituloTabela);

            celulaTituloTabela = new PdfPCell(new Phrase("UF", fonteFormatadaCabecalho));
            celulaTituloTabela.HorizontalAlignment = Element.ALIGN_CENTER;
            celulaTituloTabela.DefineCorDeFundoTabelaCabecalho();
            celulaTituloTabela.DefineCorTexto();
            table.AddCell(celulaTituloTabela);

            celulaTituloTabela = new PdfPCell(new Phrase("NASC", fonteFormatadaCabecalho));
            celulaTituloTabela.HorizontalAlignment = Element.ALIGN_CENTER;
            celulaTituloTabela.DefineCorDeFundoTabelaCabecalho();
            celulaTituloTabela.DefineCorTexto();
            table.AddCell(celulaTituloTabela);

            celulaTituloTabela = new PdfPCell(new Phrase("SEXO", fonteFormatadaCabecalho));
            celulaTituloTabela.HorizontalAlignment = Element.ALIGN_CENTER;
            celulaTituloTabela.DefineCorDeFundoTabelaCabecalho();
            celulaTituloTabela.DefineCorTexto();
            table.AddCell(celulaTituloTabela);

           
            table.DefineAlturaCabecalhoTabela();
            table.DefineFonteCabecalhoTabela();
            table.CentralizeCabecalhoTabela();


            // Obtendo os alunos do repositório
            var alunos = _repositorioAluno.GetAll().ToList();

            // Preenchendo a tabela com os dados dos alunos
            foreach (var aluno in alunos)
            {
                PdfPCell celulaTabela;
                Font fonteFormatadaTabela = new Font(Font.FontFamily.COURIER, 13f);
                // Adicionando células à tabela
                celulaTabela = new PdfPCell(new Phrase(aluno?.Matricula.ToString() ?? "", fonteFormatadaTabela));
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER;
                celulaTabela.DefineAlturaCelulaTabela();
                celulaTabela.VerticalAlignment = Element.ALIGN_MIDDLE; // Centraliza verticalmente
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER; // Centraliza horizontalmente
                table.AddCell(celulaTabela);

                celulaTabela = new PdfPCell(new Phrase(aluno?.Nome ?? "", fonteFormatadaTabela));
                celulaTabela.HorizontalAlignment = Element.ALIGN_LEFT;
                celulaTabela.DefineAlturaCelulaTabela();
                celulaTabela.VerticalAlignment = Element.ALIGN_MIDDLE; // Centraliza verticalmente
                table.AddCell(celulaTabela);

                celulaTabela = new PdfPCell(new Phrase(aluno?.CPF ?? "", fonteFormatadaTabela));
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER;
                celulaTabela.DefineAlturaCelulaTabela();
                celulaTabela.VerticalAlignment = Element.ALIGN_MIDDLE; // Centraliza verticalmente
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER; // Centraliza horizontalmente
                table.AddCell(celulaTabela);

                celulaTabela = new PdfPCell(new Phrase(aluno?.Cidade?.Uf ?? "", fonteFormatadaTabela));
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER;
                celulaTabela.DefineAlturaCelulaTabela();
                celulaTabela.VerticalAlignment = Element.ALIGN_MIDDLE; // Centraliza verticalmente
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER; // Centraliza horizontalmente
                table.AddCell(celulaTabela);

                celulaTabela = new PdfPCell(new Phrase(aluno?.Nascimento.ToShortDateString() ?? "", fonteFormatadaTabela));
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER;
                celulaTabela.DefineAlturaCelulaTabela();
                celulaTabela.VerticalAlignment = Element.ALIGN_MIDDLE; // Centraliza verticalmente
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER; // Centraliza horizontalmente
                table.AddCell(celulaTabela);

                // Adicionei o método DefineAlturaCelulaTabela para a célula de sexo também, como você mencionou
                celulaTabela = new PdfPCell(new Phrase(aluno.Sexo.ObterSiglaSexo(), fonteFormatadaTabela));
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER;
                celulaTabela.DefineAlturaCelulaTabela();
                celulaTabela.VerticalAlignment = Element.ALIGN_MIDDLE; // Centraliza verticalmente
                celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER; // Centraliza horizontalmente
                table.AddCell(celulaTabela);
            }
            

            return table;

        }
    }
}
