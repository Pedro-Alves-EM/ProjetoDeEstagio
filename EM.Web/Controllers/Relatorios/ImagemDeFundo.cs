using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Web.Controllers.Relatorios
{
    public class ImagemDeFundo : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // Define a cor de fundo da página (opcional)
            PdfContentByte contentByte = writer.DirectContentUnder;
            contentByte.SetColorFill(new BaseColor(255, 255, 255)); // Cor branca como exemplo
            contentByte.Rectangle(0, 0, document.PageSize.Width, document.PageSize.Height);
            contentByte.Fill();

       
            // Adicione a imagem de fundo
            string imagePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\EM.Web\\wwwroot\\Imagens\\imgbackground2.png";
            Image backgroundImage = Image.GetInstance(imagePath);

            float novaLargura = backgroundImage.Width * 0.5f; // Reduz para metade a largura
            float novaAltura = backgroundImage.Height * 0.6f; // Reduz para metade a altura

            backgroundImage.ScaleAbsolute(novaLargura, novaAltura);
            // Define a posição central da imagem na página
            backgroundImage.SetAbsolutePosition((document.PageSize.Width - backgroundImage.ScaledWidth) / 2, (document.PageSize.Height - backgroundImage.ScaledHeight) / 2);

            contentByte.SetRGBColorFill(255, 245, 238); // Cor de fundo no formato RGB
            contentByte.Rectangle(0, 0, document.PageSize.Width, document.PageSize.Height);
            contentByte.Fill();
            // Define a posição da imagem

            // Adiciona a imagem à página
            contentByte.AddImage(backgroundImage);
        }
    }
}
