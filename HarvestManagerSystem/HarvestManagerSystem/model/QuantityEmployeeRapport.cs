using System;
using System.Collections.Generic;
using System.Text;

namespace HarvestManagerSystem.model
{
    class QuantityEmployeeRapport
    {
        private DateTime harvestDate;
        private double allQuantity;
        private double badQuantity;
        private double productPrice;
        private double penaltyGeneral;
        private double damageGeneral;
        private int harvestType;

        private Employee employee = new Employee();
        private Transport transport = new Transport();
        private Credit credit = new Credit();
        private Production production = new Production();

        public DateTime HarvestDate { get => harvestDate; set => harvestDate = value; }
        public double AllQuantity { get => allQuantity; set => allQuantity = value; }
        public double BadQuantity { get => badQuantity; set => badQuantity = value; }
        public double GoodQuantity { get => AllQuantity - BadQuantity;}
        public double ProductPrice { get => productPrice; set => productPrice = value; }
        public double PenaltyGeneral { get => penaltyGeneral; set => penaltyGeneral = value; }
        public double DamageGeneral { get => damageGeneral; set => damageGeneral = value; }
        public int HarvestType { get => harvestType; set => harvestType = value; }
        public double Payment { get => getPayment();}
        public Employee Employee { get => employee; set => employee = value; }
        internal Transport Transport { get => transport; set => transport = value; }
        internal Credit Credit { get => credit; set => credit = value; }
        internal Production Production { get => production; set => production = value; }

        public double getPayment()
        {
            double pay = 0;
            try
            {
                pay = ((AllQuantity - BadQuantity - PenaltyGeneral - DamageGeneral) * ProductPrice) - Transport.TransportAmount - Credit.CreditAmount;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return pay;
        }

        public string EmployeeName { get => Employee.FullName; }

        public double TransportAmount
        {
            get => Transport.TransportAmount; set => Transport.TransportAmount = value;
        }

        public double CreditAmount
        {
            get => Credit.CreditAmount; set => Credit.CreditAmount = value;
        }

        public enum HarvestCategory
        {
            GROUPE, INDIVIDUAL, UNKNOWN
        };

        public HarvestCategory HarvestCat
        {
            get => (HarvestType == 1) ? HarvestCategory.INDIVIDUAL : HarvestCategory.GROUPE;
        }
    }
}
