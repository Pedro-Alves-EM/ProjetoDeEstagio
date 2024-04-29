using EM.Domain.Cidade;
using FirebirdSql.Data.FirebirdClient;
using System.Linq.Expressions;

namespace EM.Repository.RepoCidade
{
    public class RepositorioCidade : IRepositorio<Cidade>
    {
        private string connectionString;

        public RepositorioCidade(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(Cidade cidade)
        {
            using (FbConnection connection = new FbConnection(connectionString))
            {
                string query = "INSERT INTO Cidades (nome, UF) " +
                               "VALUES (@Nome, @UF)";
                using (FbCommand command = new FbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nome", cidade.Nome);
                    command.Parameters.AddWithValue("@UF", cidade.Uf);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Cidade> GetAll()
        {
            List<Cidade> cidades = new List<Cidade>();

            using (FbConnection connection = new FbConnection(connectionString))
            {
                string query = "SELECT CIDADE_ID, NOME, UF FROM CIDADES";
                FbCommand command = new FbCommand(query, connection);

                connection.Open();
                FbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Cidade cidade = new Cidade
                    {
                        Cidade_Id = Convert.ToInt32(reader["CIDADE_ID"]),
                        Nome = reader["NOME"].ToString().ToUpper(),
                        Uf = reader["UF"].ToString().ToUpper()
                    };

                    cidades.Add(cidade);
                }
            }

            return cidades;
        }
        public IEnumerable<Cidade> Get(Expression<Func<Cidade, bool>> predicate)
        {
            return GetAll().Where(predicate.Compile());
        }


        public void Update(Cidade cidade)
        {
            try
            {
                using (FbConnection connection = new FbConnection(connectionString))
                {
                    {
                        string updateSql = "UPDATE Cidades SET NOME = @Nome, UF = @Uf WHERE CIDADE_ID = @Cidade_Id";
                        using (FbCommand command = new FbCommand(updateSql, connection))
                        {
                            connection.Open();
                            command.Parameters.AddWithValue("Nome", cidade.Nome);
                            command.Parameters.AddWithValue("Uf", cidade.Uf);
                            command.Parameters.AddWithValue("Cidade_Id", cidade.Cidade_Id);

                            command.ExecuteNonQuery();

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro durante a atualização da cidade:");
                Console.WriteLine(ex.Message);
            }
        }



    }
}
