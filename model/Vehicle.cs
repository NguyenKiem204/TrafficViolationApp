using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficViolationApp.model
{
    public class Vehicle
    {
        private int vehicleID;
        private string plateNumber;
        private int ownerID;
        private string brand;
        private string model;
        private int manufactureYear;

        public Vehicle() { }

        public Vehicle(int vehicleID, string plateNumber, int ownerID, string brand, string model, int manufactureYear)
        {
            this.VehicleID = vehicleID;
            this.PlateNumber = plateNumber;
            this.OwnerID = ownerID;
            this.Brand = brand;
            this.Model = model;
            this.ManufactureYear = manufactureYear;
        }

        public int VehicleID { get => vehicleID; set => vehicleID = value; }
        public string PlateNumber { get => plateNumber; set => plateNumber = value; }
        public int OwnerID { get => ownerID; set => ownerID = value; }
        public string Brand { get => brand; set => brand = value; }
        public string Model { get => model; set => model = value; }
        public int ManufactureYear { get => manufactureYear; set => manufactureYear = value; }
    }

}
