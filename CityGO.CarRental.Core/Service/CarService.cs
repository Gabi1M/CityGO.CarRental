using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityGO.CarRental.Core.Enums;
using CityGO.CarRental.Core.Models;
using CityGO.CarRental.Core.Utils;
using Npgsql;

namespace CityGO.CarRental.Core.Service
{
    public class CarService : IDisposable
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(AppSettings.ConnectionString);

        //============================================================
        public async Task<IEnumerable<Car>> GetAsync()
        {
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"select * from car;", _connection);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var result = await command.ExecuteReaderAsync();
            var cars = new List<Car>();
            while (await result.ReadAsync())
            {
                var coordinates = new Coordinates(Convert.ToDouble(result["coordinate_x"]), Convert.ToDouble(result["coordinate_y"]));
                cars.Add(new Car(result["manufacturer"].ToString(), result["model"].ToString(), Convert.ToInt32(result["numberofseats"]), Convert.ToInt32(result["price"]), coordinates, (CarState) Enum.Parse<CarState>(result["state"].ToString().Trim()), Convert.ToInt64(result["id"])));
            }

            await _connection.CloseAsync();
            Logger.Log("Returned data for " + cars.Count + " values", LogType.Info);
            return cars;
        }

        //============================================================
        public async Task<long> SetAsync(Car car)
        {
            Logger.Log("Inserting car: " + car, LogType.Info);
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"insert into car(manufacturer, model, numberofseats, price, coordinate_x, coordinate_y, state) values (@manufacturer, @model, @numberofseats, @price, @x, @y, @state) returning id;", _connection);
            command.Parameters.AddWithValue("manufacturer", car.Manufacturer.Trim());
            command.Parameters.AddWithValue("model", car.Model.Trim());
            command.Parameters.AddWithValue("numberofseats", car.NumberOfSeats);
            command.Parameters.AddWithValue("price", car.Price);
            command.Parameters.AddWithValue("state", car.State.ToString());
            command.Parameters.AddWithValue("x", car.Coordinates.X);
            command.Parameters.AddWithValue("y", car.Coordinates.Y);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var returned = Convert.ToInt64(await command.ExecuteScalarAsync());
            await _connection.CloseAsync();
            return returned;
        }

        //============================================================
        public async Task UpdateCar(Car car)
        {
            Logger.Log("Updating car: " + car, LogType.Info);
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"update car set manufacturer = @manufacturer, model = @model, numberofseats = @numberofseats, price = @price, coordinate_x = @x, coordinate_y = @y, state = @state where id = @id", _connection);
            command.Parameters.AddWithValue("manufacturer", car.Manufacturer);
            command.Parameters.AddWithValue("model", car.Model);
            command.Parameters.AddWithValue("numberofseats", car.NumberOfSeats);
            command.Parameters.AddWithValue("price", car.Price);
            command.Parameters.AddWithValue("x", car.Coordinates.X);
            command.Parameters.AddWithValue("y", car.Coordinates.Y);
            command.Parameters.AddWithValue("state", car.State.ToString());
            command.Parameters.AddWithValue("id", car.Id);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
        }

        //============================================================
        public async Task DeleteAsync(long id)
        {
            Logger.Log("Deleting car with id: " + id, LogType.Info);
            await _connection.OpenAsync();
            var command = new NpgsqlCommand(@"delete from car where id = @id", _connection);
            command.Parameters.AddWithValue("id", id);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            using var photoService = new PhotoService();
            var photos = (await photoService.GetAsync()).Where(x => x.CarId == id);
            foreach (var ph in photos)
            {
                await photoService.DeleteAsync(ph.Id);
            }
        }

        //============================================================
        public async void Dispose()
        {
            await _connection.DisposeAsync();
        }
    }
}
