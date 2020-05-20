using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CityGO.CarRental.Core.Models;
using CityGO.CarRental.Core.Utils;
using Npgsql;

namespace CityGO.CarRental.Core.Service
{
    public class CommentService : IDisposable
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(AppSettings.ConnectionString);

        //============================================================
        public async Task<IEnumerable<Comment>> GetAsync()
        {
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"select * from comment;", _connection);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var result = await command.ExecuteReaderAsync();
            var comments = new List<Comment>();
            while (await result.ReadAsync())
            {
                var comment = new Comment(result["mail"].ToString(), result["content"].ToString(),
                    Convert.ToDateTime(result["datetime"]), Convert.ToInt64(result["id"]));
                comments.Add(comment);
            }

            await _connection.CloseAsync();

            Logger.Log("Returned data for " + comments.Count + " values", LogType.Info);
            return comments;
        }

        //============================================================
        public async Task<long> SetAsync(Comment comment)
        {
            Logger.Log("Inserting comment: " + comment, LogType.Info);

            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"insert into comment(mail, content, datetime) values (@mail, @content, @datetime) returning id;", _connection);
            command.Parameters.AddWithValue("mail", comment.Mail.Trim());
            command.Parameters.AddWithValue("content", comment.Content.Trim());
            command.Parameters.AddWithValue("datetime", comment.DateTime);

            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var returned = Convert.ToInt64(await command.ExecuteScalarAsync());
            await _connection.CloseAsync();
            return returned;
        }

        //============================================================
        public async Task DeleteAsync(long id)
        {
            Logger.Log("Deleting comment with id: " + id, LogType.Info);
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"delete from comment where id = @id", _connection);
            command.Parameters.AddWithValue("id", id);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            await command.ExecuteReaderAsync();
            await _connection.CloseAsync();
        }

        //============================================================
        public async void Dispose()
        {
            await _connection.DisposeAsync();
        }
    }
}
