using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ArvutihooldusMVC.Models
{
    public class Order
    {
        public int ID { get; set; }
        [DisplayName("Computer Type")]
        public ComputerType ComputerType { get; set; }
        public Service Service { get; set; }
        public double Price { get; set; }
        [DisplayName("Service Receiver")]
        public string? Client { get; set; }
        public bool Complete { get; set; }
        public bool Paid { get; set; }

        public static double GetPrice(ComputerType computerType, Service service)
        {
            double[] priceMultiplierType = { 1.25, 1 };
            double[] priceService = { 19.99, 59.99, 25.99, 15.99, 15.99 };
            return priceMultiplierType[(int)computerType] * priceService[(int)service];
        }
    }
}