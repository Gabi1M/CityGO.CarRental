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
        private readonly NpgsqlConnection connection = new NpgsqlConnection(AppSettings.ConnectionString);

        //============================================================
        public async Task<IEnumerable<Photo>> GetAsync()
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"select * from photo;", connection);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var result = await command.ExecuteReaderAsync();
            var photos = new List<Photo>();
            while (await result.ReadAsync())
            {
                photos.Add(new Photo(Convert.ToInt64(result["carid"]),result["path"].ToString(), Convert.ToInt64(result["id"])));
            }
            await connection.CloseAsync();

            Logger.Log("Returning data for " + photos.Count + " results", LogType.Info);
            return photos;
        }

        //============================================================
        public async Task<long> SetAsync(Photo photo)
        {
            Logger.Log("Inserting photo: " + photo, LogType.Info);
            
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"insert into photo(carid, path) values (@carid, @path) returning id;", connection);
            command.Parameters.AddWithValue("carid", photo.CarId);
            command.Parameters.AddWithValue("path", photo.Path);

            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var returned = Convert.ToInt64(await command.ExecuteScalarAsync());
            await connection.CloseAsync();
            return returned;
        }

        //============================================================
        public async Task DeleteAsync(long? photoId)
        {
            Logger.Log("Deleting photo with id: " + photoId, LogType.Info);
            
            var photo = (await GetAsync()).First(x => x.Id == photoId);
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"delete from photo where id = @id", connection);
            command.Parameters.AddWithValue("id", photoId);

            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            await command.ExecuteReaderAsync();
            await connection.CloseAsync();

            File.Delete(photo.Path);
        }

        //============================================================
        public async void Dispose()
        {
            await connection.DisposeAsync();
        }
    }
}
