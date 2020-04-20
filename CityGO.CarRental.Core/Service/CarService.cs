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
        private readonly NpgsqlConnection connection = new NpgsqlConnection(AppSettings.ConnectionString);

        //============================================================
        public async Task<IEnumerable<Car>> GetAsync()
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"select * from car;", connection);
            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var result = await command.ExecuteReaderAsync();
            var cars = new List<Car>();
            while (await result.ReadAsync())
            {
                var coordinates = new Coordinates
                {
                    X = Convert.ToDouble(result["coordinate_x"]),
                    Y = Convert.ToDouble(result["coordinate_y"])
                };
                cars.Add(new Car(result["manufacturer"].ToString(), result["model"].ToString(), Convert.ToInt32(result["numberofseats"]), Convert.ToInt32(result["price"]), coordinates, (CarState) Enum.Parse<CarState>(result["state"].ToString().Trim()), Convert.ToInt64(result["id"])));
            }
            await connection.CloseAsync();

            Logger.Log("Returned data for " + cars.Count + " values", LogType.Info);
            return cars;
        }

        //============================================================
        public async Task<long> SetAsync(Car car)
        {
            Logger.Log("Inserting car: " + car, LogType.Info);
            
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"insert into car(manufactuxrer, model, numberofseats, price, coordinate_x, coordinate_y, state) values (@manufacturer, @model, @numberofseats, @price, @x, @y, @state) returning id;", connection);
            command.Parameters.AddWithValue("manufacturer", car.Manufacturer);
            command.Parameters.AddWithValue("model", car.Model);
            command.Parameters.AddWithValue("numberofseats", car.NumberOfSeats);
            command.Parameters.AddWithValue("price", car.Price);
            command.Parameters.AddWithValue("state", car.State.ToString());
            command.Parameters.AddWithValue("x", car.Coordinates.X);
            command.Parameters.AddWithValue("y", car.Coordinates.Y);

            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            var returned = Convert.ToInt64(await command.ExecuteScalarAsync());
            await connection.CloseAsync();
            return returned;
        }

        //============================================================
        public async Task DeleteAsync(long carId)
        {
            Logger.Log("Deleting car with id: " + carId, LogType.Info);
            
            await connection.OpenAsync();
            var command = new NpgsqlCommand(@"delete from car where id = @id", connection);
            command.Parameters.AddWithValue("id", carId);

            Logger.Log("Executing sql command: " + command.CommandText, LogType.Info);
            await command.ExecuteReaderAsync();
            await connection.CloseAsync();

            using var photoService = new PhotoService();
            var photos = (await photoService.GetAsync()).Where(x => x.CarId == carId);
            foreach (var ph in photos)
            {
                await photoService.DeleteAsync(ph.Id);
            }
        }

        //============================================================
        public async void Dispose()
        {
            await connection.DisposeAsync();
        }
    }
}
