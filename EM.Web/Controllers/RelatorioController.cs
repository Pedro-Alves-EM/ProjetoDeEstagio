using EM.Web.Controllers.Relatorios;
using iTextSharp5.text.pdf;
using iTextSharp5.text;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EM.Domain.Aluno;
using EM.Repository;

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
            // Caminho onde o PDF será salvo
            string filePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\PDFSGerados\\documento.pdf";

            // Criar uma instância da classe CorpoPDF
            Corpo corpoPDF = new Corpo(_repositorioAluno); // Passando o repositório como parâmetro


            // Criar um documento PDF
            Document document = new Document(PageSize.A4);

            try
            {
                // Criar um escritor para o documento PDF
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                

                document.Open();
                document.Add(new Paragraph("\n\n\n\n\n"));
                writer.PageEvent = new Cabecalho();
                // Adicionar corpo do PDF
                PdfPTable table = corpoPDF.CriarTabelaDados();
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