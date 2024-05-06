using EM.Domain.Aluno;
using EM.Repository;
using iTextSharp5.text;
using iTextSharp5.text.pdf;
using System.Collections.Generic;
using System.Linq;

namespace EM.Web.Controllers.Relatorios
{
    public class Corpo
    {
        private readonly IRepositorio<Aluno> _repositorioAluno;

        public Corpo(IRepositorio<Aluno> repositorioAluno)
        {
            _repositorioAluno = repositorioAluno;
        }

        public PdfPTable CriarTabelaDados()
        {
            PdfPTable table = new PdfPTable(6);

            // Definindo a largura das colunas
            float[] columnWidths = { 1.2f, 3f, 1.5f, 0.5f, 1f, 0.7f };
            table.SetWidths(columnWidths);

            // Adicionando células com texto centralizado
            AdicionarCelula(table, "Matrícula", Element.ALIGN_CENTER);
            AdicionarCelula(table, "Nome", Element.ALIGN_LEFT); // Nome alinhado à esquerda
            AdicionarCelula(table, "CPF", Element.ALIGN_CENTER);
            AdicionarCelula(table, "Uf", Element.ALIGN_CENTER);
            AdicionarCelula(table, "Dt. Nasc", Element.ALIGN_CENTER);
            AdicionarCelula(table, "Sexo", Element.ALIGN_CENTER);

            // Obtendo os alunos do repositório
            var alunos = _repositorioAluno.GetAll().ToList();

            // Preenchendo a tabela com os dados dos alunos
            foreach (var aluno in alunos)
            {
                AdicionarCelula(table, aluno?.Matricula.ToString() ?? "", Element.ALIGN_CENTER);
                AdicionarCelula(table, aluno?.Nome ?? "", Element.ALIGN_LEFT); // Nome alinhado à esquerda
                AdicionarCelula(table, aluno?.CPF ?? "", Element.ALIGN_CENTER);
                AdicionarCelula(table, aluno?.Cidade?.Uf ?? "", Element.ALIGN_CENTER);
                AdicionarCelula(table, aluno?.Nascimento.ToShortDateString() ?? "", Element.ALIGN_CENTER);

                // Adicionando a sigla do sexo centralizada
                AdicionarCelula(table, ObterSiglaSexo(aluno.Sexo), Element.ALIGN_CENTER);
            }

            return table;
        }

        // Função para adicionar uma célula com alinhamento específico à tabela
        private void AdicionarCelula(PdfPTable table, string texto, int alinhamento)
        {
            PdfPCell cell = new PdfPCell(new Phrase(texto));
            cell.HorizontalAlignment = alinhamento;
            table.AddCell(cell);
        }

        // Função para obter a sigla do sexo
        private string ObterSiglaSexo(EnumeradorSexo sexo)
        {
            switch (sexo)
            {
                case EnumeradorSexo.Masculino:
                    return "M";
                case EnumeradorSexo.Feminino:
                    return "F";
                default:
                    return ""; // Ou outra lógica, se aplicável
            }
        }
    }
}
