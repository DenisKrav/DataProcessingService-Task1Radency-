using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingService_Task1Radency_.Classes.Models
{
    //Клас сервіса, який призначений для проботи з json
    internal class Service
    {
        public string Name { get; set; }
        public List<Payer> Payers { get; set; }
        public int Total { get; set; } = 0;

        public Service(string name, List<Payer> payers) 
        {
            Name = name;
            Payers = payers;
        }
    }
}
