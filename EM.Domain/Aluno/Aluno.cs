using EM.Domain.Interface;

namespace EM.Domain.Aluno
{
    public class Aluno : IEntidade
    {
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime Nascimento { get; set; }
        public EnumeradorSexo Sexo { get; set; }
        public Cidade.Cidade Cidade { get; set; }
    }
}
