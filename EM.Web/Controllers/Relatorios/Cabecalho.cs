using iTextSharp5.text.pdf;
using iTextSharp5.text;

public class Cabecalho : PdfPageEventHelper
{
    public override void OnStartPage(PdfWriter writer, Document document)
    {
        base.OnStartPage(writer, document);

        Phrase phrase = new Phrase("Relatório de Alunos", new Font(Font.FontFamily.COURIER, 26));

        float marginTop = 50f; 

        float novaPosicaoY = document.PageSize.Height - -15 - marginTop;

        PdfContentByte conteudoDireto = writer.DirectContent;

        ColumnText.ShowTextAligned(conteudoDireto, Element.ALIGN_CENTER, phrase, document.PageSize.Width / 2, novaPosicaoY, 0);

        string imagePath2 = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\EM.Web\\wwwroot\\Imagens\\imgbackground2.png";
        Image img2 = Image.GetInstance(imagePath2);
        img2.ScaleToFit(document.PageSize.Width, document.PageSize.Height / 2);
        float posicaoY = (document.PageSize.Height - img2.ScaledHeight) / 2.2f;
        float posicaoX = (document.PageSize.Width - img2.ScaledWidth) / 2;
        img2.SetAbsolutePosition(posicaoX, posicaoY);
        writer.DirectContentUnder.AddImage(img2);

        PdfContentByte cb = writer.DirectContent;
        float positionY = document.PageSize.Height - 20;

        string imagePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\EM.Web\\wwwroot\\Imagens\\imgbackground.png";
        Image img = Image.GetInstance(imagePath);
        img.ScaleToFit(100, 100);
        img.SetAbsolutePosition(15, 725);
        cb.AddImage(img);
    }

    private Image imagemRodape;

    public Cabecalho()
    {

        string imagePath = "C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\EM.Web\\wwwroot\\Imagens\\solucoes-193.png";
        imagemRodape = Image.GetInstance(imagePath);
    }

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        base.OnEndPage(writer, document);

  
        float fatorEscala = 0.6f;


        PdfContentByte contentByte = writer.DirectContent;
        float larguraImagem = imagemRodape.ScaledWidth * fatorEscala;
        float alturaImagem = imagemRodape.ScaledHeight * fatorEscala;
        float larguraPagina = document.PageSize.Width;
        float posicaoX = (larguraPagina - larguraImagem) / 2;
        float posicaoYImagem = 10; 
        float posicaoYNumeros = 15; 

        imagemRodape.ScaleAbsoluteWidth(larguraImagem);
        imagemRodape.ScaleAbsoluteHeight(alturaImagem);
        imagemRodape.SetAbsolutePosition(posicaoX, posicaoYImagem);
        contentByte.AddImage(imagemRodape);

        int paginaAtual = writer.PageNumber;
        ColumnText.ShowTextAligned(contentByte, Element.ALIGN_RIGHT, new Phrase(paginaAtual.ToString()), larguraPagina - 10, posicaoYNumeros, 0);
    }
}
