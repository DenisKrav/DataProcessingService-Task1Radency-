using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingService_Task1Radency_.Models
{
    //Клас платника, який призначений для проботи з json
    internal class Payer
    {
        public string Name { get; set; }
        public double Payment { get; set; }
        public DateTime Date { get; set; }
        public int Account_number { get; set; }

        public Payer(string name, double payment, DateTime date, int account_number)
        {
            Name = name;
            Payment = payment;
            Date = date;
            Account_number = account_number;
        }
    }
}
