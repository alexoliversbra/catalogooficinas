using CatalogoOficinas.Entities;
using CatalogoOficinas.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoUsuarios.Repositories
{
    public class UsuarioSqlServerRepository : IUsuarioRepository
    {
        private const string Status = "Ativo";
        private readonly SqlConnection sqlConnection;

        public UsuarioSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Usuarios"));
        }

        public async Task<List<Usuario>> Obter(int pagina, int quantidade)
        {
            var usuarios = new List<Usuario>();

            var comando = $"select * from Usuarios order by id offset {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                usuarios.Add(new Usuario
                {
                    Id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Email = (string)sqlDataReader["Email"],
                    Cpf = (string)sqlDataReader["Cpf"],
                    Nasc = (string)sqlDataReader["Nasc"],
                    Telefone = (string)sqlDataReader["Telefone"],
                    Status = (string)sqlDataReader["Status"]
                });
            }

            await sqlConnection.CloseAsync();

            return usuarios;
        }

        public async Task<Usuario> Obter(Guid id)
        {
            Usuario usuario = null;

            var comando = $"select * from Usuarios where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                usuario = new Usuario
                {
                    Id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Email = (string)sqlDataReader["Email"],
                    Cpf = (string)sqlDataReader["Cpf"],
                    Nasc = (string)sqlDataReader["Nasc"],
                    Telefone = (string)sqlDataReader["Telefone"],
                    Status = (string)sqlDataReader["Status"]
                };
            }
            await sqlConnection.CloseAsync();

            return usuario;
        }

        public async Task<List<Usuario>> Obter(string nome, string cpf)
        {
            var usuarios = new List<Usuario>();

            var comando = $"select * from Usuarios where Nome = '{nome}' and Cpf = '{cpf}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                usuarios.Add(new Usuario
                {
                    Id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Email = (string)sqlDataReader["Email"],
                    Cpf = (string)sqlDataReader["Cpf"],
                    Nasc = (string)sqlDataReader["Nasc"],
                    Telefone = (string)sqlDataReader["Telefone"],
                    Status = (string)sqlDataReader["Status"]
                });
            }

            await sqlConnection.CloseAsync();

            return usuarios;
        }

        public async Task Inserir(Usuario usuario)
        {
            var comando = $"insert Usuarios (Id, Nome, Email, Cpf, Nasc, Telefone, Status) values ('{usuario.Id}','{usuario.Nome}','{usuario.Email}','{usuario.Cpf}','{usuario.Nasc}','{usuario.Telefone}','{usuario.Status = Status}')";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Atualizar(Usuario usuario)
        {
            var comando = $"update Usuarios set Nasc = '{usuario.Nasc}', Telefone ='{usuario.Telefone}', Status ='{usuario.Status}' where Id ='{usuario.Id}'";
            
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Remover(Guid id)
        {
            
            var comando = $"delete from Usuarios where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();

            //System.InvalidOperationException: The connection was not closed. The connection's current state is open.
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
