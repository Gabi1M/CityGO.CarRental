using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CityGO.CarRental.Core.Models;
using CityGO.CarRental.Core.Utils;
using Npgsql;

namespace CityGO.CarRental.Core.Service
{
    //===========================================================//
    public class RentalService : IDisposable
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(AppSettings.ConnectionString);

        //===========================================================//
        public async Task<IEnumerable<Rental>> GetAsync()
        {
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"select * from rental;", _connection);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var result = await command.ExecuteReaderAsync();
            var rentals = new List<Rental>();
            while (await result.ReadAsync())
            {
                var rental = new Rental(Convert.ToInt64(result["carid"]), Convert.ToInt64(result["clientid"]),
                    Convert.ToDateTime(result["datetime"]), Convert.ToInt64(result["id"]));
                rentals.Add(rental);
            }

            await _connection.CloseAsync();
            Logger.Log("Returned data for " + rentals.Count + " values", LogType.Info);
            return rentals;
        }

        //===========================================================//
        public async Task<long> SetAsync(Rental rental)
        {
            Logger.Log("Inserting rental: " + rental, LogType.Info);

            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"insert into rental(clientid, carid, datetime) values (@clientid, @carid, @datetime) returning id", _connection);
            command.Parameters.AddWithValue("carid", rental.CarId);
            command.Parameters.AddWithValue("clientid", rental.ClientId);
            command.Parameters.AddWithValue("datetime", rental.DateTime);

            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var returned = Convert.ToInt64(await command.ExecuteScalarAsync());
            await _connection.CloseAsync();
            return returned;
        }

        //===========================================================//
        public async Task DeleteAsync(long id)
        {
            Logger.Log("Deleting rental with id: " + id, LogType.Info);
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"delete from rental where id = @id", _connection);
            command.Parameters.AddWithValue("id", id);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            await command.ExecuteReaderAsync();
            await _connection.CloseAsync();
        }

        //===========================================================//
        public async void Dispose()
        {
            await _connection.DisposeAsync();
        }
    }
}
