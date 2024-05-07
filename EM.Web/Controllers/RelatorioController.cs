using EM.Web.Controllers.Relatorios;
using iTextSharp5.text.pdf;
using iTextSharp5.text;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EM.Domain.Aluno;
using EM.Repository;
using EM.Web.Controllers.Extensao;

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

            // Criar um documento PDF
            Document document = new Document();
            document.DimensaoDocumento();
            
            try
            {
                // Criar um escritor para o documento PDF
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                writer.PageEvent = new Cabecalho();

                document.Open();
                document.Add(new Paragraph("\n\n\n\n\n\n"));
                writer.PageEvent = new Cabecalho();
                


                Corpo corpoPDF = new Corpo(_repositorioAluno, writer); // Passando o PdfWriter
                

                // Adicionar corpo do PDF
                PdfPTable table = corpoPDF.CriarTabelaDados();
               

                writer.PageEvent = new ImagemDeFundo();

                writer.PageEvent = new Rodape();

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
