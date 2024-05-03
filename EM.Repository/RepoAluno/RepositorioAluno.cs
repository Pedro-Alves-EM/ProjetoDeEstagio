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
                connection.Open();

                string query = "INSERT INTO Aluno (NOME, CPF, NASCIMENTO, SEXO, CIDADE_ID) " +
                               "VALUES (@Nome, @CPF, @Nascimento, @Sexo, @Cidade_Id)";

                using (FbCommand command = new FbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nome", aluno.Nome);
                    command.Parameters.AddWithValue("@CPF", aluno.CPF);
                    command.Parameters.AddWithValue("@Nascimento", aluno.Nascimento);
                    command.Parameters.AddWithValue("@Sexo", aluno.Sexo);
                    command.Parameters.AddWithValue("@Cidade_Id", aluno.Cidade.Cidade_Id);

                    command.ExecuteNonQuery();
                }
            }
        }




        public IEnumerable<Aluno> GetAll()
        {
            List<Aluno> alunos = new List<Aluno>();
            using (FbConnection con = new FbConnection(connectionString))
            {

                string query = @"SELECT A.Matricula, A.Nome, A.Sexo, A.Nascimento, A.CPF, C.UF
                 FROM Aluno A
                 INNER JOIN Cidades C ON A.Cidade_Id = C.Cidade_Id";

                FbCommand command = new FbCommand(query, con);
                try
                {
                    con.Open();
                    FbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Aluno aluno = new Aluno
                        {
                            Nome = reader["NOME"].ToString().ToUpper(),
                            Matricula = Convert.ToInt32(reader["MATRICULA"]),
                            CPF = reader["CPF"].ToString(),
                            Nascimento = (reader["NASCIMENTO"] != DBNull.Value) ? Convert.ToDateTime(reader["NASCIMENTO"]) : DateTime.MinValue,
                            Sexo = (EnumeradorSexo)Enum.Parse(typeof(EnumeradorSexo), reader["SEXO"].ToString()),
                            Cidade = new Cidade { Uf = reader["Uf"].ToString().ToUpper() } // Aqui está a referência à coluna "Uf"
                        };
                        alunos.Add(aluno);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao recuperar alunos do banco de dados.", ex);
                }
            }
            return alunos;
        }




        public void Update(Aluno aluno)
        {
            try
            {
                using (FbConnection connection = new FbConnection(connectionString))
                {
                    connection.Open();
                    string updateSql = "UPDATE Aluno SET NOME = @Nome, CPF = @CPF, Sexo = @Sexo, Nascimento = @Nascimento, Cidade_Id = @Cidade_Id WHERE Matricula = @Matricula";
                    using (FbCommand command = new FbCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@Nome", aluno.Nome);
                        command.Parameters.AddWithValue("@CPF", aluno.CPF);
                        command.Parameters.AddWithValue("@Sexo", aluno.Sexo);
                        command.Parameters.AddWithValue("@Nascimento", aluno.Nascimento);
                        command.Parameters.AddWithValue("@Matricula", aluno.Matricula);
                        command.Parameters.AddWithValue("@Cidade_Id", aluno.Cidade.Cidade_Id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro durante a atualização do Aluno:");
                Console.WriteLine(ex.Message);
            }
        }



        public void Remove(Aluno aluno)
        {
            using (FbConnection con = new FbConnection(connectionString))
            {
                string query = "DELETE FROM ALUNO WHERE MATRICULA = @MATRICULA";
                using (FbCommand command = new FbCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Matricula", aluno.Matricula);
                    con.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            return GetAll().Where(predicate.Compile());
        }


        public Aluno GetByMatricula(int matricula)
        {
            Aluno aluno = null;
            using (FbConnection con = new FbConnection(connectionString))
            {
                string query = @"
            SELECT A.Matricula, A.Nome, A.Sexo, A.Nascimento, A.CPF, C.UF
            FROM Aluno A
            INNER JOIN Cidades C ON A.Cidade_Id = C.Cidade_Id
            WHERE A.Matricula = @Matricula";

                FbCommand command = new FbCommand(query, con);
                command.Parameters.AddWithValue("@Matricula", matricula);

                try
                {
                    con.Open();
                    FbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        aluno = new Aluno
                        {
                            Nome = reader["NOME"].ToString().ToUpper(),
                            Matricula = Convert.ToInt32(reader["MATRICULA"]),
                            CPF = reader["CPF"].ToString(),
                            Nascimento = (reader["NASCIMENTO"] != DBNull.Value) ? Convert.ToDateTime(reader["NASCIMENTO"]) : DateTime.MinValue,
                            Sexo = (EnumeradorSexo)Enum.Parse(typeof(EnumeradorSexo), reader["SEXO"].ToString()),
                            Cidade = new Cidade { Uf = reader["Uf"].ToString().ToUpper() }
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao recuperar aluno do banco de dados.", ex);
                }
            }
            return aluno;
        }

        public List<Aluno> GetByNome(string parteNome)
        {
            parteNome = parteNome.ToLower();

            var alunosEncontrados = GetAll().Where(a => a.Nome.ToLower().Contains(parteNome)).ToList();

            return alunosEncontrados;
        }
        public List<Aluno> GetByUf(string uf)
        {
            uf = uf.ToUpper(); 

            var alunosEncontrados = GetAll().Where(a => a.Cidade.Uf == uf).ToList();

            return alunosEncontrados;
        }

    }
}


