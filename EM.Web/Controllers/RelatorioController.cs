using iTextSharp5.text.pdf;
using iTextSharp5.text;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EM.Domain.Aluno;
using EM.Repository;
using EM.Web.Controllers.Extensao;
using System;
using System.Linq;

namespace EM.Web.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly IRepositorio<Aluno> _repositorioAluno;

        public RelatorioController(IRepositorio<Aluno> repositorioAluno)
        {
            _repositorioAluno = repositorioAluno;
        }

        public IActionResult GerarPDF()
        {
            string filePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\PDFSGerados\\documento.pdf";

            Document document = new Document();
            document.SetMargins(document.LeftMargin, document.RightMargin, 120f, 70f); 

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            writer.PageEvent = new Cabecalho();

            try
            {
                document.Open();
               


                PdfPTable table = new PdfPTable(6);
                table.DimensaoTabela(110f);
                table.SpacingAfter = 80;
                PdfContentByte cb = writer.DirectContent;
                cb.SetRGBColorStroke(0, 0, 0);
                cb.SetLineWidth(1f);

                cb.RoundRectangle(445, 740, 130, 50, 10);
                cb.Stroke();

                cb.BeginText();
                cb.SetRGBColorFill(0, 0, 0);

                float tamanhoFonte = 10f;
                cb.SetFontAndSize(BaseFont.CreateFont(), tamanhoFonte);

                DateTime agora = DateTime.Now;
                string dataHoraFormatada = agora.PegueDataHora();

                float larguraTexto = cb.GetEffectiveStringWidth(dataHoraFormatada, false);

                float posX = 435 + (145 - larguraTexto) / 2;

                cb.ShowTextAligned(Element.ALIGN_LEFT, dataHoraFormatada, posX, 755, 0);

                cb.DefineEstiloData();
                cb.EndText();

                Font fonteFormatadaCabecalho = new Font(Font.FontFamily.COURIER, 24f);
                table.SetWidths(new float[] { 0.5f, 3f, 1.8f, 0.5f, 2f, 0.7f });
                string[] headers = { "MA", "NOME", "CPF", "UF", "Idade", "SEXO" };

                // Adiciona cabeçalhos à tabela
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, new Font(Font.FontFamily.COURIER, 24f)));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.DefineCorDeFundoTabelaCabecalho();
                    cell.DefineCorTexto();
                    table.AddCell(cell);
                }

                table.DefineAlturaCabecalhoTabela();
                table.DefineFonteCabecalhoTabela();
                table.CentralizeCabecalhoTabela();

                // Obtendo os alunos do repositório
                var alunos = _repositorioAluno.GetAll().ToList();

                // Preenchendo a tabela com os dados dos alunos
                foreach (var aluno in alunos)
                {
                    table.AddCell(CreateCell(aluno?.Matricula.ToString() ?? ""));
                    table.AddCell(CreateCell(aluno?.Nome ?? "", Element.ALIGN_LEFT)); // Alinhado à esquerda
                    table.AddCell(CreateCell(aluno?.CPF ?? ""));
                    table.AddCell(CreateCell(aluno?.Cidade?.Uf ?? ""));
                    table.AddCell(CreateCell(aluno.CalcularIdade()));
                    table.AddCell(CreateCell(aluno.Sexo.ObterSiglaSexo()));
                }
                // Adiciona a tabela ao documento
                document.Add(table);
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine("Erro ao criar o documento: " + de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine("Erro de I/O: " + ioe.Message);
            }
            finally
            {
                // Fechar o documento
                document.Close();
            }

            // Retorna o arquivo PDF para download
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf", "documento.pdf");

        }
        private PdfPCell CreateCell(string text, int alignment = Element.ALIGN_CENTER)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, new Font(Font.FontFamily.COURIER, 13f)));
            cell.HorizontalAlignment = alignment;
            cell.DefineAlturaCelulaTabela();
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
        public IActionResult GerarPDFPorNome()
        {
            // Obter os alunos ordenados por nome

            string filePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\PDFSGerados\\documento.pdf";

            Document document = new Document();
            document.SetMargins(document.LeftMargin, document.RightMargin, 120f, 70f);

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            writer.PageEvent = new Cabecalho();

            try
            {
                document.Open();



                PdfPTable table = new PdfPTable(6);
                table.DimensaoTabela(110f);
                table.SpacingAfter = 80;
                PdfContentByte cb = writer.DirectContent;
                cb.SetRGBColorStroke(0, 0, 0);
                cb.SetLineWidth(1f);

                cb.RoundRectangle(445, 740, 130, 50, 10);
                cb.Stroke();

                cb.BeginText();
                cb.SetRGBColorFill(0, 0, 0);

                float tamanhoFonte = 10f;
                cb.SetFontAndSize(BaseFont.CreateFont(), tamanhoFonte);

                DateTime agora = DateTime.Now;
                string dataHoraFormatada = agora.PegueDataHora();

                float larguraTexto = cb.GetEffectiveStringWidth(dataHoraFormatada, false);

                float posX = 435 + (145 - larguraTexto) / 2;

                cb.ShowTextAligned(Element.ALIGN_LEFT, dataHoraFormatada, posX, 755, 0);

                cb.DefineEstiloData();
                cb.EndText();

                Font fonteFormatadaCabecalho = new Font(Font.FontFamily.COURIER, 24f);

                float[] columnWidths = { 0.5f, 3f, 1.8f, 0.5f, 2f, 0.7f };
                table.SetWidths(columnWidths);

                PdfPCell celulaTituloTabela;
                celulaTituloTabela = new PdfPCell(new Phrase("MA", fonteFormatadaCabecalho));
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

                celulaTituloTabela = new PdfPCell(new Phrase("Idade", fonteFormatadaCabecalho));
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
                var alunos = _repositorioAluno.GetAll().OrdenarPorNome().ToList();

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

                    string idade = aluno.CalcularIdade();

                    celulaTabela = new PdfPCell(new Phrase(idade, fonteFormatadaTabela));
                    celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER;
                    celulaTabela.DefineAlturaCelulaTabela();
                    celulaTabela.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(celulaTabela);

                    // Adicionei o método DefineAlturaCelulaTabela para a célula de sexo também, como você mencionou
                    celulaTabela = new PdfPCell(new Phrase(aluno.Sexo.ObterSiglaSexo(), fonteFormatadaTabela));
                    celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER;
                    celulaTabela.DefineAlturaCelulaTabela();
                    celulaTabela.VerticalAlignment = Element.ALIGN_MIDDLE; // Centraliza verticalmente
                    celulaTabela.HorizontalAlignment = Element.ALIGN_CENTER; // Centraliza horizontalmente
                    table.AddCell(celulaTabela);
                }

                // Adiciona a tabela ao documento
                document.Add(table);
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine("Erro ao criar o documento: " + de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine("Erro de I/O: " + ioe.Message);
            }
            finally
            {
                // Fechar o documento
                document.Close();
            }

            // Retorna o arquivo PDF para download
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf", "documento.pdf");

        }

    }
}
