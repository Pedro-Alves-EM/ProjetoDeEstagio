using iTextSharp5.text.pdf;
using iTextSharp5.text;

public class Cabecalho : PdfPageEventHelper
{
    public override void OnStartPage(PdfWriter writer, Document document)
    {
        base.OnStartPage(writer, document);



        // Define o texto do cabeçalho
        Phrase phrase = new Phrase("Relatório de Alunos", new Font(Font.FontFamily.COURIER, 25));



        // Obtém o conteúdo direto para escrever na página
        PdfContentByte cb = writer.DirectContent;
        float positionY = document.PageSize.Height - 50;
        // Adiciona o texto ao cabeçalho
        ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, phrase, document.PageSize.Width / 2, positionY, 0);
        string imagePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\EM.Web\\wwwroot\\Imagens\\imgbackground.png";
        Image img = Image.GetInstance(imagePath);
        img.ScaleToFit(100, 100);
        img.SetAbsolutePosition(15, 725);
        cb.AddImage(img);
    }
}