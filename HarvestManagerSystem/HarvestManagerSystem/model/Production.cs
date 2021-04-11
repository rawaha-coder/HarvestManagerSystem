﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HarvestManagerSystem.model
{
    class Production
    {

        private int productionID;
        private DateTime productionDate;
        private int totalEmployee;
        private double totalQuantity;
        private long totalMinutes;
        private double price;
        private int productionType;
        private Supplier supplier = new Supplier();
        private Farm farm = new Farm();
        private Product product = new Product();
        private ProductDetail productDetail = new ProductDetail();

        public int ProductionID { get => productionID; set => productionID = value; }
        public DateTime ProductionDate { get => productionDate; set => productionDate = value; }
        public int TotalEmployee { get => totalEmployee; set => totalEmployee = value; }
        public double TotalQuantity { get => totalQuantity; set => totalQuantity = value; }
        public long TotalMinutes { get => totalMinutes; set => totalMinutes = value; }
        public double Price { get => price; set => price = value; }
        public int ProductionType { get => productionType; set => productionType = value; }
        internal Supplier Supplier { get => supplier;}
        internal Farm Farm { get => farm;}
        internal Product Product { get => product;}
        internal ProductDetail ProductDetail { get => productDetail;}
    }
}
