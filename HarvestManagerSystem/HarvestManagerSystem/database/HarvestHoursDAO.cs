﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using HarvestManagerSystem.model;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace HarvestManagerSystem.database
{
    class HarvestHoursDAO : DAO
    {
        public const string TABLE_HOURS = "HarvestHours";
        public const string COLUMN_HOURS_ID = "HarvestHoursId";
        public const string COLUMN_HOURS_DATE = "HarvestDate";
        public const string COLUMN_HOURS_SM = "TimeStartMorning";
        public const string COLUMN_HOURS_EM = "TimeEndMorning";
        public const string COLUMN_HOURS_SN = "TimeStartNoon";
        public const string COLUMN_HOURS_EN = "TimeEndNoon";
        public const string COLUMN_HOURS_PRICE = "HourPrice";
        public const string COLUMN_HOURS_REMARQUE = "Remarque";
        public const string COLUMN_HOURS_EMPLOYEE_TYPE = "EmployeeType";
        public const string COLUMN_HOURS_EMPLOYEE_ID = "EmployeeId";
        public const string COLUMN_HOURS_TRANSPORT_ID = "TransportId";
        public const string COLUMN_HOURS_CREDIT_ID = "CreditId";
        public const string COLUMN_HOURS_PRODUCTION_ID = "ProductionId";


        private static HarvestHoursDAO instance = new HarvestHoursDAO();

        private HarvestHoursDAO() : base() { }

        public static HarvestHoursDAO getInstance()
        {
            if (instance == null)
            {
                instance = new HarvestHoursDAO();
            }
            return instance;
        }


        //*******************************
        //Get Hours data by production id
        //*******************************
        public List<HarvestHours> HoursDataByProductionId(Production production)
        {
            string selectStmt = "SELECT "
                + TABLE_HOURS + "." + COLUMN_HOURS_ID + ", "
                + TABLE_HOURS + "." + COLUMN_HOURS_DATE + ", "
                + TABLE_HOURS + "." + COLUMN_HOURS_SM + ", "
                + TABLE_HOURS + "." + COLUMN_HOURS_EM + ", "
                + TABLE_HOURS + "." + COLUMN_HOURS_SN + ", "
                + TABLE_HOURS + "." + COLUMN_HOURS_EN + ", "
                + TABLE_HOURS + "." + COLUMN_HOURS_PRICE + ", "
                + TABLE_HOURS + "." + COLUMN_HOURS_REMARQUE + ", "
                + TABLE_HOURS + "." + COLUMN_HOURS_EMPLOYEE_TYPE + ", "
                + EmployeeDAO.TABLE_EMPLOYEE + "." + EmployeeDAO.COLUMN_EMPLOYEE_ID + ", "
                + EmployeeDAO.TABLE_EMPLOYEE + "." + EmployeeDAO.COLUMN_EMPLOYEE_FIRST_NAME + ", "
                + EmployeeDAO.TABLE_EMPLOYEE + "." + EmployeeDAO.COLUMN_EMPLOYEE_LAST_NAME + ", "
                + TransportDAO.TABLE_TRANSPORT + "." + TransportDAO.COLUMN_TRANSPORT_ID + ", "
                + TransportDAO.TABLE_TRANSPORT + "." + TransportDAO.COLUMN_TRANSPORT_AMOUNT + ", "
                + CreditDAO.TABLE_CREDIT + "." + CreditDAO.COLUMN_CREDIT_ID + ", "
                + CreditDAO.TABLE_CREDIT + "." + CreditDAO.COLUMN_CREDIT_AMOUNT + ", "
                + ProductionDAO.TABLE_PRODUCTION + "." + ProductionDAO.COLUMN_PRODUCTION_ID + ", "
                + SupplierDAO.TABLE_SUPPLIER + "." + SupplierDAO.COLUMN_SUPPLIER_ID + ", "
                + SupplierDAO.TABLE_SUPPLIER + "." + SupplierDAO.COLUMN_SUPPLIER_NAME + ", "
                + FarmDAO.TABLE_FARM + "." + FarmDAO.COLUMN_FARM_ID + ", "
                + FarmDAO.TABLE_FARM + "." + FarmDAO.COLUMN_FARM_NAME + ", "
                + ProductDAO.TABLE_PRODUCT + "." + ProductDAO.COLUMN_PRODUCT_ID + ", "
                + ProductDAO.TABLE_PRODUCT + "." + ProductDAO.COLUMN_PRODUCT_NAME + ", "
                + ProductDetailDAO.TABLE_PRODUCT_DETAIL + "." + ProductDetailDAO.COLUMN_PRODUCT_DETAIL_ID + ", "
                + ProductDetailDAO.TABLE_PRODUCT_DETAIL + "." + ProductDetailDAO.COLUMN_PRODUCT_TYPE + ", "
                + ProductDetailDAO.TABLE_PRODUCT_DETAIL + "." + ProductDetailDAO.COLUMN_PRODUCT_CODE + " "
                + " FROM " + TABLE_HOURS + " "
                + " LEFT JOIN " + EmployeeDAO.TABLE_EMPLOYEE + " "
                + " ON " + EmployeeDAO.TABLE_EMPLOYEE + "." + EmployeeDAO.COLUMN_EMPLOYEE_ID + " = " + TABLE_HOURS + "." + COLUMN_HOURS_EMPLOYEE_ID
                + " LEFT JOIN " + TransportDAO.TABLE_TRANSPORT + " "
                + " ON " + TransportDAO.TABLE_TRANSPORT + "." + TransportDAO.COLUMN_TRANSPORT_ID + " = " + TABLE_HOURS + "." + COLUMN_HOURS_TRANSPORT_ID
                + " LEFT JOIN " + CreditDAO.TABLE_CREDIT + " "
                + " ON " + CreditDAO.TABLE_CREDIT + "." + CreditDAO.COLUMN_CREDIT_ID + " = " + TABLE_HOURS + "." + COLUMN_HOURS_CREDIT_ID
                + " LEFT JOIN " + ProductionDAO.TABLE_PRODUCTION + " "
                + " ON " + ProductionDAO.TABLE_PRODUCTION + "." + ProductionDAO.COLUMN_PRODUCTION_ID + " = " + TABLE_HOURS + "." + COLUMN_HOURS_PRODUCTION_ID
                + " LEFT JOIN " + SupplierDAO.TABLE_SUPPLIER + " "
                + " ON " + SupplierDAO.TABLE_SUPPLIER + "." + SupplierDAO.COLUMN_SUPPLIER_ID + " = " + ProductionDAO.TABLE_PRODUCTION + "." + ProductionDAO.COLUMN_PRODUCTION_SUPPLIER_ID
                + " LEFT JOIN " + FarmDAO.TABLE_FARM + " "
                + " ON " + FarmDAO.TABLE_FARM + "." + FarmDAO.COLUMN_FARM_ID + " = " + ProductionDAO.TABLE_PRODUCTION + "." + ProductionDAO.COLUMN_PRODUCTION_FARM_ID
                + " LEFT JOIN " + ProductDAO.TABLE_PRODUCT + " "
                + " ON " + ProductDAO.TABLE_PRODUCT + "." + ProductDAO.COLUMN_PRODUCT_ID + " = " + ProductionDAO.TABLE_PRODUCTION + "." + ProductionDAO.COLUMN_PRODUCTION_PRODUCT_ID
                + " LEFT JOIN " + ProductDetailDAO.TABLE_PRODUCT_DETAIL + " "
                + " ON " + ProductDetailDAO.TABLE_PRODUCT_DETAIL + "." + ProductDetailDAO.COLUMN_PRODUCT_DETAIL_ID + " = " + ProductionDAO.TABLE_PRODUCTION + "." + ProductionDAO.COLUMN_PRODUCTION_PRODUCT_DETAIL_ID
                + " WHERE " + TABLE_HOURS + "." + COLUMN_HOURS_PRODUCTION_ID + " = " + production.ProductionID + " "
                + " ORDER BY " + TABLE_HOURS + "." + COLUMN_HOURS_DATE + " DESC ;";
            try
            {
                SQLiteCommand sQLiteCommand = new SQLiteCommand(selectStmt, mSQLiteConnection);
                OpenConnection();
                SQLiteDataReader result = sQLiteCommand.ExecuteReader();
                return getHarvestHoursFromResultSet(result);
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.StackTrace);
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<HarvestHours> getHarvestHoursFromResultSet(SQLiteDataReader result)
        {
            List<HarvestHours> list = new List<HarvestHours>();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    HarvestHours harvestHours = new HarvestHours();
                    harvestHours.HarvestHoursID = Convert.ToInt32((result[COLUMN_HOURS_ID]).ToString());
                    harvestHours.HarvestDate = (DateTime)result[COLUMN_HOURS_DATE];
                    harvestHours.StartMorning = (DateTime)result[COLUMN_HOURS_SM];
                    harvestHours.EndMorning = (DateTime)result[COLUMN_HOURS_EM];
                    harvestHours.StartNoon = (DateTime)result[COLUMN_HOURS_SN];
                    harvestHours.EndMorning = (DateTime)result[COLUMN_HOURS_EN];
                    harvestHours.HourPrice = Convert.ToDouble((result[COLUMN_HOURS_PRICE]).ToString());
                    harvestHours.Remarque = (string)result[COLUMN_HOURS_REMARQUE];
                    harvestHours.EmployeeType = Convert.ToInt32((result[COLUMN_HOURS_EMPLOYEE_TYPE]).ToString());
                    harvestHours.Employee.EmployeeId = Convert.ToInt32((result[EmployeeDAO.COLUMN_EMPLOYEE_ID]).ToString());
                    harvestHours.Employee.FirstName = (string)result[EmployeeDAO.COLUMN_EMPLOYEE_FIRST_NAME];
                    harvestHours.Employee.LastName = (string)result[EmployeeDAO.COLUMN_EMPLOYEE_LAST_NAME];
                    harvestHours.Transport.TransportId = Convert.ToInt32((result[TransportDAO.COLUMN_TRANSPORT_ID]).ToString());
                    harvestHours.Transport.TransportAmount = Convert.ToDouble((result[TransportDAO.COLUMN_TRANSPORT_AMOUNT]).ToString());
                    harvestHours.Credit.CreditId = Convert.ToInt32((result[CreditDAO.COLUMN_CREDIT_ID]).ToString());
                    harvestHours.Credit.CreditAmount = Convert.ToDouble((result[CreditDAO.COLUMN_CREDIT_AMOUNT]).ToString());
                    harvestHours.Production.ProductionID = Convert.ToInt32((result[ProductionDAO.COLUMN_PRODUCTION_ID]).ToString());
                    harvestHours.Production.Supplier.SupplierId = Convert.ToInt32((result[SupplierDAO.COLUMN_SUPPLIER_ID]).ToString());
                    harvestHours.Production.Supplier.SupplierName = (string)result[SupplierDAO.COLUMN_SUPPLIER_NAME];
                    harvestHours.Production.Farm.FarmId = Convert.ToInt32((result[FarmDAO.COLUMN_FARM_ID]).ToString());
                    harvestHours.Production.Farm.FarmName = (string)result[FarmDAO.COLUMN_FARM_NAME];
                    harvestHours.Production.Product.ProductId = Convert.ToInt32((result[ProductDAO.COLUMN_PRODUCT_ID]).ToString());
                    harvestHours.Production.Product.ProductName = (string)result[ProductDAO.COLUMN_PRODUCT_NAME];
                    harvestHours.Production.ProductDetail.ProductDetailId = Convert.ToInt32((result[ProductDetailDAO.COLUMN_PRODUCT_DETAIL_ID]).ToString());
                    harvestHours.Production.ProductDetail.ProductType = (string)result[ProductDetailDAO.COLUMN_PRODUCT_TYPE];
                    harvestHours.Production.ProductDetail.ProductCode = (string)result[ProductDetailDAO.COLUMN_PRODUCT_CODE];
                    list.Add(harvestHours);
                }
            }

            return list;
        }


        //*******************************
        //Add new HarvestHours data 
        //*******************************
        public bool addData(HarvestHours harvestHours)
        {
            string insertStmt = "INSERT INTO " + TABLE_HOURS + " ("
                    + COLUMN_HOURS_DATE + ", "
                    + COLUMN_HOURS_SM + ", "
                    + COLUMN_HOURS_EM + ", "
                    + COLUMN_HOURS_SN + ", "
                    + COLUMN_HOURS_EN + ", "
                    + COLUMN_HOURS_PRICE + ", "
                    + COLUMN_HOURS_REMARQUE + ", "
                    + COLUMN_HOURS_EMPLOYEE_TYPE + ", "
                    + COLUMN_HOURS_EMPLOYEE_ID + ", "
                    + COLUMN_HOURS_TRANSPORT_ID + ", "
                    + COLUMN_HOURS_CREDIT_ID + ", "
                    + COLUMN_HOURS_PRODUCTION_ID +
                    " ) VALUES ( "
                    + "@" + COLUMN_HOURS_DATE + ", "
                    + "@" + COLUMN_HOURS_SM + ", "
                    + "@" + COLUMN_HOURS_EM + ", "
                    + "@" + COLUMN_HOURS_SN + ", "
                    + "@" + COLUMN_HOURS_EN + ", "
                    + "@" + COLUMN_HOURS_PRICE + ", "
                    + "@" + COLUMN_HOURS_REMARQUE + ", "
                    + "@" + COLUMN_HOURS_EMPLOYEE_TYPE + ", "
                    + "@" + COLUMN_HOURS_EMPLOYEE_ID + ", "
                    + "@" + COLUMN_HOURS_TRANSPORT_ID + ", "
                    + "@" + COLUMN_HOURS_CREDIT_ID + ", "
                    + "@" + COLUMN_HOURS_PRODUCTION_ID
                    + " )";
            try
            {
                SQLiteCommand sQLiteCommand = new SQLiteCommand(insertStmt, mSQLiteConnection);
                OpenConnection();
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_DATE, harvestHours.HarvestDate);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_SM, harvestHours.StartMorning);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_EM, harvestHours.EndMorning);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_SN, harvestHours.StartNoon);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_EN, harvestHours.EndNoon);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_PRICE, harvestHours.HourPrice);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_REMARQUE, harvestHours.Remarque);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_EMPLOYEE_TYPE, harvestHours.EmployeeType);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_EMPLOYEE_ID, harvestHours.Employee.EmployeeId);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_TRANSPORT_ID, harvestHours.Transport.TransportId);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_CREDIT_ID, harvestHours.Credit.CreditId);
                sQLiteCommand.Parameters.AddWithValue(COLUMN_HOURS_PRODUCTION_ID, harvestHours.Production.ProductionID);
                sQLiteCommand.ExecuteNonQuery();
                return true;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        //*******************************
        //Get Add Hours data
        //*******************************
        public List<HarvestHours> HarvestersData()
        {
            List<HarvestHours> list = new List<HarvestHours>();
            String selectStmt = "SELECT "
                + EmployeeDAO.TABLE_EMPLOYEE + "." + EmployeeDAO.COLUMN_EMPLOYEE_ID + ", "
                + EmployeeDAO.TABLE_EMPLOYEE + "." + EmployeeDAO.COLUMN_EMPLOYEE_FIRST_NAME + ", "
                + EmployeeDAO.TABLE_EMPLOYEE + "." + EmployeeDAO.COLUMN_EMPLOYEE_LAST_NAME + " "
                    + " FROM " + EmployeeDAO.TABLE_EMPLOYEE
                    + " WHERE " + EmployeeDAO.COLUMN_EMPLOYEE_STATUS + " = 1 "
                    + " AND " + EmployeeDAO.COLUMN_EMPLOYEE_IS_EXIST + " = 1 "
                    + " ORDER BY " + EmployeeDAO.COLUMN_EMPLOYEE_FIRST_NAME + " ASC;";

            try
            {
                SQLiteCommand sQLiteCommand = new SQLiteCommand(selectStmt, mSQLiteConnection);
                OpenConnection();
                SQLiteDataReader result = sQLiteCommand.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        HarvestHours harvestHours = new HarvestHours();
                        harvestHours.Employee.EmployeeId = Convert.ToInt32((result[EmployeeDAO.COLUMN_EMPLOYEE_ID]).ToString());
                        harvestHours.Employee.FirstName = (string)result[EmployeeDAO.COLUMN_EMPLOYEE_FIRST_NAME];
                        harvestHours.Employee.LastName = (string)result[EmployeeDAO.COLUMN_EMPLOYEE_LAST_NAME];
                        list.Add(harvestHours);
                    }
                }
                return list;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.StackTrace);
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }


        public void CreateTable()
        {
            string createStmt = "CREATE TABLE IF NOT EXISTS " + TABLE_HOURS + " ("
                + COLUMN_HOURS_ID + " INTEGER PRIMARY KEY, "
                + COLUMN_HOURS_DATE + " DATE NOT NULL, "
                + COLUMN_HOURS_SM + " REAL NOT NULL, "
                + COLUMN_HOURS_EM + " REAL NOT NULL, "
                + COLUMN_HOURS_SN + " REAL NOT NULL, "
                + COLUMN_HOURS_EN + " REAL NOT NULL, "
                + COLUMN_HOURS_PRICE + " REAL, "
                + COLUMN_HOURS_REMARQUE + " TEXT, "
                + COLUMN_HOURS_EMPLOYEE_TYPE + " INTEGER NOT NULL, "
                + COLUMN_HOURS_EMPLOYEE_ID + " INTEGER NOT NULL, "
                + COLUMN_HOURS_TRANSPORT_ID + " INTEGER NOT NULL, "
                + COLUMN_HOURS_CREDIT_ID + " INTEGER NOT NULL, "
                + COLUMN_HOURS_PRODUCTION_ID + " INTEGER NOT NULL, "
                + " FOREIGN KEY (" + COLUMN_HOURS_EMPLOYEE_ID + ") REFERENCES " + EmployeeDAO.TABLE_EMPLOYEE + " (" + EmployeeDAO.COLUMN_EMPLOYEE_ID + ")"
                + " FOREIGN KEY (" + COLUMN_HOURS_TRANSPORT_ID + ") REFERENCES " + TransportDAO.TABLE_TRANSPORT + " (" + TransportDAO.COLUMN_TRANSPORT_ID + ")"
                + " FOREIGN KEY (" + COLUMN_HOURS_CREDIT_ID + ") REFERENCES " + CreditDAO.TABLE_CREDIT + " (" + CreditDAO.COLUMN_CREDIT_ID + ")"
                + " FOREIGN KEY (" + COLUMN_HOURS_PRODUCTION_ID + ") REFERENCES " + ProductionDAO.TABLE_PRODUCTION + " (" + ProductionDAO.COLUMN_PRODUCTION_ID + ")"
                + ");";

            try
            {
                SQLiteCommand sQLiteCommand = new SQLiteCommand(createStmt, mSQLiteConnection);
                OpenConnection();
                sQLiteCommand.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.StackTrace);
                throw e;
            }
            finally
            {
                CloseConnection();
            }
        }

    }
}