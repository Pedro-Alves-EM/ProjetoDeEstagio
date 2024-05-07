using iTextSharp5.text;
using iTextSharp5.text.pdf;

namespace EM.Web.Controllers.Relatorios
{
    public class Rodape : PdfPageEventHelper
    {

        private Image imagemRodape;

        public Rodape()
        {
            // Carregue a imagem do rodapé
            string imagePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\EM.Web\\wwwroot\\Imagens\\solucoes-193.png";
            imagemRodape = Image.GetInstance(imagePath);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // Fator de escala para diminuir o tamanho da imagem
            float fatorEscala = 0.6f; // Ajuste este valor conforme necessário

            // Adicione a imagem do rodapé
            PdfContentByte contentByte = writer.DirectContent;
            float larguraImagem = imagemRodape.ScaledWidth * fatorEscala;
            float alturaImagem = imagemRodape.ScaledHeight * fatorEscala;
            float larguraPagina = document.PageSize.Width;
            float posicaoX = (larguraPagina - larguraImagem) / 2;
            float posicaoYImagem = 10; // Mantenha a posição Y da imagem no canto inferior
            float posicaoYNumeros = 15; // Margem de 10 unidades para o número da página

            imagemRodape.ScaleAbsoluteWidth(larguraImagem);
            imagemRodape.ScaleAbsoluteHeight(alturaImagem);
            imagemRodape.SetAbsolutePosition(posicaoX, posicaoYImagem);
            contentByte.AddImage(imagemRodape);

            // Adicione o número da página
            int paginaAtual = writer.PageNumber;
            ColumnText.ShowTextAligned(contentByte, Element.ALIGN_RIGHT, new Phrase(paginaAtual.ToString()), larguraPagina - 10, posicaoYNumeros, 0);
        }
    }
}