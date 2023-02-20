using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingService_Task1Radency_.Classes.Models
{
    //Клас міста, який призначений для проботи з json
    internal class City
    {
        public string Name { get; set; }
        public List<Service> Services { get; set; }
        public int Total { get; set; } = 0;

        public City(string name, List<Service> services)
        {
            Name = name;
            Services = services;
        }
    }
}
