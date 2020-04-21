using System;
using System.Collections.Generic;
using System.Linq;
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
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var result = await command.ExecuteReaderAsync();
            var clients = new List<Client>();
            while (await result.ReadAsync())
            {
                clients.Add(new Client(result["name"].ToString(), result["mail"].ToString(), result["password"].ToString(), Convert.ToInt32(result["numberofpastrentals"]), Convert.ToInt64(result["id"])));
            }
            await connection.CloseAsync();

            Logger.Log("Returning data for " + clients.Count + " results", LogType.Info);
            return clients;
        }

        //============================================================
        public async Task<long> SetAsync(Client client)
        {
            Logger.Log("Inserting client: " + client, LogType.Info);
            
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"insert into client(name, mail, password, numberofpastrentals) values (@name, @mail, @password, @numberofpastrentals) returning id;", connection);
            command.Parameters.AddWithValue("name", client.Name);
            command.Parameters.AddWithValue("mail", client.Mail);
            command.Parameters.AddWithValue("password", client.Password);
            command.Parameters.AddWithValue("numberofpastrentals", client.NumberOfPastRentals);

            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var returned = Convert.ToInt64(await command.ExecuteScalarAsync());
            await connection.CloseAsync();
            return returned;
        }

        //============================================================
        public async Task DeleteAsync(long? clientId)
        {
            Logger.Log("Deleting client with id: " + clientId, LogType.Info);
            
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"delete from client where id = @id", connection);
            command.Parameters.AddWithValue("id", clientId);

            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            await command.ExecuteReaderAsync();
            await connection.CloseAsync();
        }
        
        //============================================================
        public async Task<bool> LoginAsync(string mail, string password)
        {
            Logger.Log("Checking login information for mail: " + mail + ", password: " + password, LogType.Info);
            var clients = await GetAsync();
            return clients.Any(x => x.Mail.Trim() == mail.Trim() && x.Password.Trim() == password.Trim());
        }

        //============================================================
        public async void Dispose()
        {
            await connection.DisposeAsync();
        }
    }
}
