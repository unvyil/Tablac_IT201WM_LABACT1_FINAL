using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TABLAC_IT201WM_LABACT1_FINAL
{
    public partial class Receipt : Form
    {
        public string PlateNumber { get; set; }
        public string VehicleType { get; set; }
        public int HoursParked { get; set; }
        public string SlotNumber { get; set; }
        public string DiscountType { get; set; }

        public const decimal ServiceCharge = 20.00m;
        private const decimal OvertimeFlatFee = 50.00m;

        public Receipt(string plateNumber, string vehicleType, int hoursParked, string slotNumber, string discountType)
        {
            PlateNumber = plateNumber;
            VehicleType = vehicleType;
            HoursParked = hoursParked;
            SlotNumber = slotNumber;
            DiscountType = discountType;

            InitializeComponent();
        }

        public decimal ComputeBaseFee()
        {
            decimal ratePerHour;

            if (VehicleType == "Motorcycle") ratePerHour = 30.00m;
            else if (VehicleType == "Car") ratePerHour = 50.00m;
            else if (VehicleType == "Van") ratePerHour = 70.00m;
            else ratePerHour = 50.00m;

            decimal subTotal = ratePerHour * HoursParked;

            if (HoursParked > 8)
            {
                subTotal += OvertimeFlatFee;
            }

            return subTotal;
        }

        public decimal ComputeTotalFee()
        {
            decimal baseFee = ComputeBaseFee();

            if (DiscountType == "Senior Citizen" || DiscountType == "Employee")
            {
                baseFee -= (baseFee * 0.20m);
            }

            return baseFee + ServiceCharge;
        }



        public decimal ComputeHourlyRate()
        {
            // Calculates only the hours * rate
            decimal ratePerHour;
            if (VehicleType == "Motorcycle") ratePerHour = 30.00m;
            else if (VehicleType == "Car") ratePerHour = 50.00m;
            else if (VehicleType == "Van") ratePerHour = 70.00m;
            else ratePerHour = 50.00m;

            return ratePerHour * HoursParked;
        }

        public decimal ComputeOvertimeFee()
        {
            // Returns 50 if over 8 hours, otherwise 0
            return (HoursParked > 8) ? OvertimeFlatFee : 0.00m;
        }
    }
}
