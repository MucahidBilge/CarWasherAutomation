﻿using Program.DAL.Context;
using Program.DATA.Entities;
using Program.DATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Business.Repositories
{
    public class VehicleRepository
    {
        ProjectContext db;
        public VehicleRepository()
        {
            db = new ProjectContext();
        }
        public void Add(Vehicle vehicle)
        {
            db.Vehicles.Add(vehicle);
            db.SaveChanges();
        }
        public void Update(Vehicle vehicle)
        {
            vehicle.ModifiedDate = DateTime.Now;
            db.Vehicles.Update(vehicle);
            db.SaveChanges();
        }
        public void Delete(Vehicle vehicle)
        {
            vehicle.DeletedDate = DateTime.Now;
            vehicle.IsActive = false;
            db.Vehicles.Add(vehicle);
            db.SaveChanges();
        }

        public List<Vehicle> SearchVehicles(string text)
        {
            return db.Vehicles
                .Where(x=> x.Customer.Name.Contains(text) || x.Plate.StartsWith(text))
                .Where(x=>x.IsActive==true)
                .ToList();
        }
        public decimal GetPrice(Vehicle vehicle,string ProcessType)
        {
            decimal washingPrice =0, basePrice = 100,discount=1;
            switch (vehicle.BodyType)
            {
                case BodyType.Sedan:
                    washingPrice = basePrice * 1.2M;
                    break;
                case BodyType.Hatchback:
                    washingPrice = basePrice * 1;
                    break;
                case BodyType.SUV:
                    washingPrice = basePrice * 1.9M;
                    break;
                case BodyType.StationVagon:
                    washingPrice = basePrice * 1.8M;
                    break;
                case BodyType.Pickup:
                    washingPrice = basePrice * 2M;
                    break;
                case BodyType.Minivan:
                    washingPrice = basePrice * 2.1M;
                    break;
                case BodyType.Panelvan:
                    washingPrice = basePrice * 2.1M;
                    break;
                case BodyType.Coupe:
                    washingPrice = basePrice * 1.1M;
                    break;
                default:
                    break;
            }
            switch (ProcessType)
            {
                case "Interior":
                    washingPrice = washingPrice * 1.2M;
                    break;
                case "Exterior":
                    washingPrice = washingPrice * 1;
                    break;
                case "Full":
                    washingPrice = washingPrice * 1.7M;
                    break;
                default:
                    break;
            }
            if (vehicle.Customer.IsSubscriber == true)
            {
                if (vehicle.Customer.SubscribeType == SubscribeType.Basic)
                    discount = 0.1M;
                else if (vehicle.Customer.SubscribeType == SubscribeType.Classic)
                    discount = 0.25M;
                else if (vehicle.Customer.SubscribeType == SubscribeType.Premium)
                    discount = 0.4M;
            }
            washingPrice = washingPrice * (1 - discount);
            return washingPrice;
        }
    }
}