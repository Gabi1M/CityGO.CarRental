using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CityGO.CarRental.Core.Models;
using CityGO.CarRental.Core.Utils;
using Npgsql;

namespace CityGO.CarRental.Core.Service
{
    public class ClientService : IDisposable
    {
        private readonly NpgsqlConnection connection = new NpgsqlConnection(AppSettings.ConnectionString);

        //============================================================
        public async Task<IEnumerable<Client>> GetAsync()
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"select * from client;", connection);
            var result = await command.ExecuteReaderAsync();
            var clients = new List<Client>();
            while (await result.ReadAsync())
            {
                clients.Add(new Client(result["name"].ToString(), result["mail"].ToString(), result["password"].ToString(), Convert.ToInt32(result["numberofpastrentals"]), Convert.ToInt64(result["id"])));
            }
            await connection.CloseAsync();

            return clients;
        }

        //============================================================
        public async Task<long> SetAsync(Client client)
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"insert into client(name, mail, password, numberofpastrentals) values (@name, @mail, @password, @numberofpastrentals) returning id;", connection);
            command.Parameters.AddWithValue("name", client.Name);
            command.Parameters.AddWithValue("mail", client.Mail);
            command.Parameters.AddWithValue("password", client.Password);
            command.Parameters.AddWithValue("numberofpastrentals", client.NumberOfPastRentals);

            var returned = Convert.ToInt64(await command.ExecuteScalarAsync());
            await connection.CloseAsync();
            return returned;
        }

        //============================================================
        public async Task DeleteAsync(long? clientId)
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"delete from client where id = @id", connection);
            command.Parameters.AddWithValue("id", clientId);

            await command.ExecuteReaderAsync();
            await connection.CloseAsync();
        }

        //============================================================
        public async void Dispose()
        {
            await connection.DisposeAsync();
        }
    }
}
