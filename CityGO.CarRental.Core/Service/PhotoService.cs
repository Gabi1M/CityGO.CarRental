using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CityGO.CarRental.Core.Models;
using CityGO.CarRental.Core.Utils;
using Npgsql;

namespace CityGO.CarRental.Core.Service
{
    public class PhotoService : IDisposable
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(AppSettings.ConnectionString);

        //============================================================
        public async Task<IEnumerable<Photo>> GetAsync()
        {
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"select * from photo;", _connection);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var result = await command.ExecuteReaderAsync();
            var photos = new List<Photo>();
            while (await result.ReadAsync())
            {
                photos.Add(new Photo(Convert.ToInt64(result["carid"]),result["path"].ToString(), Convert.ToInt64(result["id"])));
            }
            await _connection.CloseAsync();
            Logger.Log("Returning data for " + photos.Count + " results", LogType.Info);
            return photos;
        }

        //============================================================
        public async Task<long> SetAsync(Photo photo)
        {
            Logger.Log("Inserting photo: " + photo, LogType.Info);
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"insert into photo(carid, path) values (@carid, @path) returning id;", _connection);
            command.Parameters.AddWithValue("carid", photo.CarId);
            command.Parameters.AddWithValue("path", photo.Path);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var returned = Convert.ToInt64(await command.ExecuteScalarAsync());
            await _connection.CloseAsync();
            return returned;
        }

        //============================================================
        public async Task DeleteAsync(long? id)
        {
            Logger.Log("Deleting photo with id: " + id, LogType.Info);
            var photo = (await GetAsync()).First(x => x.Id == id);
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"delete from photo where id = @id", _connection);
            command.Parameters.AddWithValue("id", id);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            File.Delete(photo.Path);
        }

        //============================================================
        public async void Dispose()
        {
            await _connection.DisposeAsync();
        }
    }
}
