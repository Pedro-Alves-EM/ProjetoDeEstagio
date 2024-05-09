using iTextSharp5.text.pdf;
using iTextSharp5.text;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EM.Domain.Aluno;
using EM.Repository;
using EM.Web.Controllers.Extensao;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EM.Web.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly IRepositorio<Aluno> _repositorioAluno;

        public RelatorioController(IRepositorio<Aluno> repositorioAluno)
        {
            _repositorioAluno = repositorioAluno;
        }

        private IActionResult GerarPDFComOrdenacao(string ordenacao, List<Aluno> alunos, string nomeArquivo, bool paisagem, bool zebrado)
        {
            string filePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\PDFSGerados\\" + nomeArquivo;

            Document document = paisagem ? new Document(PageSize.A4.Rotate()) : new Document();

            document.SetMargins(document.LeftMargin, document.RightMargin, 120f, 70f);

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Criar instância do cabeçalho e configurar o evento OnEndPage
            var cabecalho = new Cabecalho();
            writer.PageEvent = cabecalho;

            try
            {
                document.Open();

                PdfPTable table = new PdfPTable(6);
                table.DimensaoTabela(110f);
                table.SpacingAfter = 80;

                PdfContentByte cb = writer.DirectContent;

                // Defina a cor do contorno e a largura da linha
                cb.SetRGBColorStroke(0, 0, 0);
                cb.SetLineWidth(1f);

                // Calcule as coordenadas X e Y para o retângulo redondo no canto superior direito
                float larguraRetangulo = 130;
                float alturaRetangulo = 50;
                float margemDireita = 20; // Margem direita da página
                float margemSuperior = 40; // Margem superior da página
                float posicaoX = document.PageSize.Width - larguraRetangulo - margemDireita;
                float posicaoY = document.PageSize.Height - alturaRetangulo - margemSuperior;

                // Desenhe o retângulo redondo
                cb.RoundRectangle(posicaoX, posicaoY, larguraRetangulo, alturaRetangulo, 10);
                cb.Stroke();

                // Comece a adicionar texto
                cb.BeginText();

                // Defina a cor de preenchimento para o texto
                cb.SetRGBColorFill(0, 0, 0);

                float tamanhoFonte = 10f;
                cb.SetFontAndSize(BaseFont.CreateFont(), tamanhoFonte);

                DateTime agora = DateTime.Now;
                string dataHoraFormatada = agora.PegueDataHora();

                // Calcule a largura do texto
                float larguraTexto = cb.GetEffectiveStringWidth(dataHoraFormatada, false);

                // Calcule a posição X para centralizar o texto no retângulo
                float posX = posicaoX + (larguraRetangulo - larguraTexto) / 2;

                // Posição Y para o texto (um pouco abaixo do topo do retângulo)
                float posY = posicaoY + alturaRetangulo - 30;

                // Mostre o texto alinhado ao centro do retângulo
                cb.ShowTextAligned(Element.ALIGN_LEFT, dataHoraFormatada, posX, posY - 5, 0);

                cb.DefineEstiloData("Data Emissão", posX - 7, posY + 10);
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

                table.HeaderRows = 1;
                if (zebrado)
                {
                    table.ZebrarPDF(BaseColor.LIGHT_GRAY, BaseColor.WHITE);
                }

              

                document.Add(table);
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine($"Erro ao criar o PDF ordenado por {ordenacao}: {de.Message}");
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine($"Erro de I/O: {ioe.Message}");
            }
            finally
            {
                int totalPaginas = writer.PageNumber;

                cabecalho.SetTotalPaginas(totalPaginas);
                document.Close();
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf", nomeArquivo);
        }


        private PdfPCell CreateCell(string text, int alignment = Element.ALIGN_CENTER)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, new Font(Font.FontFamily.COURIER, 13f)));
            cell.HorizontalAlignment = alignment;
            cell.DefineAlturaCelulaTabela();
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }

        public IActionResult GerarPDF(string ordenarPor, string orientacaoPDF, string orientacaoPDFCores)
        {
            List<Aluno> alunosOrdenados = null;

            switch (ordenarPor)
            {
                case "Nome":
                    alunosOrdenados = _repositorioAluno.GetAll().OrdenarPorNome().ToList();
                    break;
                case "Estado":
                    alunosOrdenados = _repositorioAluno.GetAll().OrdenarPorEstado().ToList();
                    break;
                case "Idade":
                    alunosOrdenados = _repositorioAluno.GetAll().OrdenarPorIdade().ToList();
                    break;
                default:
                    alunosOrdenados = _repositorioAluno.GetAll().ToList();
                    break;
            }

            bool paisagem = orientacaoPDF == "paisagem";
            bool zebrado = orientacaoPDFCores == "zebrado";

            return GerarPDFComOrdenacao(ordenarPor, alunosOrdenados, $"pdf{ordenarPor}.pdf", paisagem, zebrado);
        }
    }
}
