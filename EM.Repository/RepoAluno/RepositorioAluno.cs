using EM.Domain.Aluno;
using EM.Domain.Cidade;
using FirebirdSql.Data.FirebirdClient;
using System.Linq.Expressions;

namespace EM.Repository
{
    public class RepositorioAluno : IRepositorio<Aluno>
    {
        private string connectionString;
        public RepositorioAluno(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(Aluno aluno)
        {
            using (FbConnection connection = new FbConnection(connectionString))
            {
                string query = "INSERT INTO Aluno (Nome, Matricula, CPF, Nascimento, Sexo, Cidade) " +
                    "VALUES (@Nome, @Matricula, @CPF, @Nascimento, @Sexo, @Cidade)";
                using (FbCommand command = connection.CreateCommand())
                {
                    command.Parameters.AddWithValue("@Nome", aluno.Nome);
                    command.Parameters.AddWithValue("@Matricula", aluno.Matricula);
                    command.Parameters.AddWithValue("@CPF", aluno.CPF);
                    command.Parameters.AddWithValue("@Nascimento", aluno.Nascimento);
                    command.Parameters.AddWithValue("@Sexo", aluno.Sexo);
                    command.Parameters.AddWithValue("@Cidade", aluno.Cidade.Nome);
                    command.Parameters.AddWithValue("@Cidade", aluno.Cidade.Uf);
                    command.ExecuteNonQuery();
                }
            }
        }

     
        public IEnumerable<Aluno> GetAll()
        {
            List<Aluno> alunos = new List<Aluno>();
            using (FbConnection con = new FbConnection(connectionString))
            {
                string query = "SELECT * FROM ALUNO";
                FbCommand command = new FbCommand( query, con);
                con.Open();
                FbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Aluno aluno = new Aluno
                    {
                        Nome = reader["NOME"].ToString().ToUpper(),
                        Matricula = Convert.ToInt32(reader["MATRICULA"]),
                        CPF = reader["CPF"].ToString(),
                        Nascimento = Convert.ToDateTime(reader["NASCIMENTO"]),
                        Sexo = (EnumeradorSexo)Enum.Parse(typeof(EnumeradorSexo), reader["SEXO"].ToString()),
                        Cidade = new Cidade { Nome = reader["CIDADE"].ToString() }
                    };
                    alunos.Add(aluno);
                }
            }
            return alunos;
        }

        public void Update(Aluno aluno)
        {
            throw new NotImplementedException();
        }

        public void Remove(Aluno aluno)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}



//public void Remove(int cidadeId)
//{
//    using (FbConnection connection = new FbConnection(connectionString))
//    {
//        string query = "DELETE FROM CIDADES WHERE CIDADE_ID = @Cidade_Id";

//        using (FbCommand command = new FbCommand(query, connection))
//        {
//            command.Parameters.AddWithValue("@Cidade_Id", cidadeId);
//            connection.Open();
//            command.ExecuteNonQuery();
//        }
//    }