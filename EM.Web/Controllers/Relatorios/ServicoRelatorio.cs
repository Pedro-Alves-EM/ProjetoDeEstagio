using System.IO;
using iTextSharp5.text.pdf;
using iTextSharp5.text;

namespace EM.Web.Controllers.Relatorios
{
    public class ServicoRelatorio
    {
        public void GerarPDF(string filePath)
        {
            // Criar um novo documento PDF
            Document document = new Document();

            try
            {
                // Criar um escritor para o documento PDF
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                // Abrir o documento
                document.Open();

                // Adicionar conteúdo ao documento
                document.Add(new Paragraph("Este é um documento PDF gerado pelo iTextSharp."));
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
        }
    }
}
