using TrafficViolationApp.model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace TrafficViolationApp.dao
{
    class VehicleDAO : IDAOInterface<Vehicle, int>
    {
        private readonly TrafficDBContext context;

        public VehicleDAO()
        {
            context = new TrafficDBContext();
        }

        public int delete(Vehicle t)
        {
            var existingVehicle = context.Vehicles.Find(t.VehicleId);
            if (existingVehicle == null) return 0;
            context.Vehicles.Remove(t);
            return context.SaveChanges();
        }

        public int insert(Vehicle t)
        {
            context.Vehicles.Add(t);
            return context.SaveChanges();
        }

        public List<Vehicle> selectAll()
        {
            return context.Vehicles
                .Include(v => v.Owner)
                .ToList();
        }

        public Vehicle? selectById(int id)
        {
            return context.Vehicles
                .Include(v => v.Owner)
                .FirstOrDefault(v => v.VehicleId == id);
        }

        public int update(Vehicle t)
        {
            var existingVehicle = context.Vehicles.Find(t.VehicleId);
            if (existingVehicle == null) return 0;

            existingVehicle.PlateNumber = t.PlateNumber;
            existingVehicle.OwnerId = t.OwnerId;
            existingVehicle.Brand = t.Brand;
            existingVehicle.Model = t.Model;
            existingVehicle.ManufactureYear = t.ManufactureYear;

            return context.SaveChanges();
        }

        public Vehicle? selectByPlateNumber(string plateNumber)
        {
            return context.Vehicles
                .Include(v => v.Owner)
                .FirstOrDefault(v => v.PlateNumber == plateNumber);
        }
    }
}