using EM.Domain.Interface;

namespace EM.Domain.Cidade
{
    public class Cidade : IEntidade
    {
        public string Nome { get; set; }
        public string Uf { get; set; }
        public int Cidade_Id { get; set; }

        public Cidade(string nome, string uf)
        {
            Uf = uf;
        }
        public Cidade() { }
    }
}

