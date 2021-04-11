﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HarvestManagerSystem.model;
using HarvestManagerSystem.database;
using System.Linq;


namespace HarvestManagerSystem.view
{
    public partial class FormAddHours : Form
    {
        private bool isEditHours = false;
        private HarvestHours mHarvestHours = new HarvestHours();

        private TransportDAO transportDAO = TransportDAO.getInstance();
        private EmployeeDAO employeeDAO = EmployeeDAO.getInstance();
        private FarmDAO farmDAO = FarmDAO.getInstance();
        private SupplierDAO supplierDAO = SupplierDAO.getInstance();
        private ProductDAO productDAO = ProductDAO.getInstance();
        private ProductDetailDAO productDetailDAO = ProductDetailDAO.getInstance();
        private HarvestHoursDAO harvestHoursDAO = HarvestHoursDAO.getInstance();

        private Dictionary<string, Supplier> mSupplierDictionary = new Dictionary<string, Supplier>();
        private Dictionary<string, Farm> mFarmDictionary = new Dictionary<string, Farm>();
        private Dictionary<string, Product> mProductDictionary = new Dictionary<string, Product>();
        private Dictionary<string, ProductDetail> mProductDetailDictionary = new Dictionary<string, ProductDetail>();

        //List<HarvestHours> HarvesterList = new List<HarvestHours>();
        //BindingList<HarvestHours> mList = new BindingList<HarvestHours>();


        private HarvestMS harvestMS;
        private static FormAddHours instance;

        public FormAddHours(HarvestMS harvestMS)
        {
            this.harvestMS = harvestMS;
            InitializeComponent();
        }

        private void FormAddHours_FormClosed(object sender, FormClosedEventArgs e)
        {
            wipeFields();
            instance = null;
        }

        public static FormAddHours getInstance(HarvestMS harvestMS)
        {
            if (instance == null)
            {
                instance = new FormAddHours(harvestMS);
            }
            return instance;
        }

        public void ShowFormAdd()
        {
            if (instance != null)
            {
                instance.BringToFront();
            }
            else
            {
                instance = new FormAddHours(harvestMS);

            }
            instance.Show();
        }

        static List<HarvestHours> HarvesterList = new List<HarvestHours>();
        BindingSource bindingSourceHarvesterList = new System.Windows.Forms.BindingSource { DataSource = HarvesterList };
        
        private void FormAddHours_Load(object sender, EventArgs e)
        {
            SupplierNameList();
            FarmNameList();
            ProductNameList();
            HarvesterRadioButton.Checked = true;
            AddHarvestHoursDataGridView.DataSource = bindingSourceHarvesterList;
            HarvesterList = harvestHoursDAO.HarvestersData();
            bindingSourceHarvesterList.DataSource = HarvesterList;

            AddHarvestHoursDataGridView.Columns["HarvestHoursIDColumn"].DisplayIndex = 0;
            AddHarvestHoursDataGridView.Columns["HarvestDateColumn"].DisplayIndex = 1;
            AddHarvestHoursDataGridView.Columns["EmployeeNameColumn"].DisplayIndex = 2;
            AddHarvestHoursDataGridView.Columns["StartMorningColumn"].DisplayIndex = 3;
            AddHarvestHoursDataGridView.Columns["EndMorningColumn"].DisplayIndex = 4;
            AddHarvestHoursDataGridView.Columns["StartNoonColumn"].DisplayIndex = 5;
            AddHarvestHoursDataGridView.Columns["EndNoonColumn"].DisplayIndex = 6;
            AddHarvestHoursDataGridView.Columns["TimeStartMorningColumn"].DisplayIndex = 7;
            AddHarvestHoursDataGridView.Columns["TimeEndMorningColumn"].DisplayIndex = 8;
            AddHarvestHoursDataGridView.Columns["TimeStartNoonColumn"].DisplayIndex = 9;
            AddHarvestHoursDataGridView.Columns["TimeEndNoonColumn"].DisplayIndex = 10;
            AddHarvestHoursDataGridView.Columns["TotalMinutesColumn"].DisplayIndex = 11;
            AddHarvestHoursDataGridView.Columns["HourPriceColumn"].DisplayIndex = 12;
            AddHarvestHoursDataGridView.Columns["TransportStatusColumn"].DisplayIndex = 13;
            AddHarvestHoursDataGridView.Columns["CreditColumn"].DisplayIndex = 14;
            AddHarvestHoursDataGridView.Columns["PaymentColumn"].DisplayIndex = 15;
            AddHarvestHoursDataGridView.Columns["RemarqueColumn"].DisplayIndex = 16;

        }

        private void ValidateAddHarvestHours()
        {
            //PreferencesDAO preferencesDAO = PreferencesDAO.getInstance();
            double price = 10;
            double totalMinute = 0; double totalTransport = 0.0; double totalCredit = 0.0; double totalPayment = 0.0;

            foreach (HarvestHours h in HarvesterList)
            {
                h.StartMorning = SMHoursDateTimePicker.Value;
                h.EndMorning = EMHoursDateTimePicker.Value;
                h.StartNoon = SNHoursDateTimePicker.Value;
                h.EndNoon = ENHoursDateTimePicker.Value;
                h.EmployeeType = getEmployeeType();
                h.HourPrice = price;
                totalMinute += h.TotalMinutes;
                h.Transport.TransportAmount = (h.TransportStatus) ? 10 : 0;
                totalTransport += h.Transport.TransportAmount;
                totalCredit += h.Credit.CreditAmount;
                totalPayment += h.Payment;
            }

            TotalEmployeeTextBox.Text = Convert.ToString(HarvesterList.Count);
            TotalMinutesTextBox.Text = Convert.ToString(totalMinute);
            HourPriceTextBox.Text = Convert.ToString(price);
            TotalTransportTextBox.Text = Convert.ToString(totalTransport);
            TotalCreditTextBox.Text = Convert.ToString(totalCredit);
            TotalPaymentTextBox.Text = Convert.ToString(totalPayment);
            AddHarvestHoursDataGridView.Refresh();

            
        }

        private void SupplierNameList()
        {
            List<string> NamesList = new List<string>();
            mSupplierDictionary.Clear();
            try
            {
                mSupplierDictionary = supplierDAO.SupplierDictionary();
                NamesList.AddRange(mSupplierDictionary.Keys);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (NamesList != null)
            {
                SupplierHarvestHoursComboBox.DataSource = NamesList;
            }
        }

        private void FarmNameList()
        {
            List<string> NamesList = new List<string>();
            mFarmDictionary.Clear();
            try
            {
                mFarmDictionary = farmDAO.FarmDictionary();
                NamesList.AddRange(mFarmDictionary.Keys);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (NamesList != null)
            {
                FarmHarvestHoursComboBox.DataSource = NamesList;
            }
        }

        private void ProductNameList()
        {
            List<string> NamesList = new List<string>();
            mProductDictionary.Clear();
            try
            {
                mProductDictionary = productDAO.ProductDictionary();
                NamesList.AddRange(mProductDictionary.Keys);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (NamesList != null)
            {
                ProductHarvestHoursComboBox.DataSource = NamesList;
            }
        }

        private void ProductHarvestHoursComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = ProductHarvestHoursComboBox.SelectedIndex;
            if (i < mProductDictionary.Values.Count && i >=0 )
            {
                DisplayProductDetailData(mProductDictionary.ElementAt(i).Value);
            }
        }

        private void DisplayProductDetailData(Product product)
        {
            List<string> CodeList = new List<string>();
            mProductDetailDictionary.Clear();
            try
            {
                mProductDetailDictionary = productDetailDAO.ProductCodeDictionary(product);
                CodeList.AddRange(mProductDetailDictionary.Keys);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (CodeList != null)
            {
                ProductCodeHarvestHoursComboBox.DataSource = CodeList;
            }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }



        private void ValidateHarvestHoursButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                MessageBox.Show("Check values");
                return;
            }
            ValidateAddHarvestHours();
        }

        private bool ValidateInput()
        {
            return
            HarvestHoursDateTimePicker.Value == null ||
            SupplierHarvestHoursComboBox.SelectedIndex == -1 ||
            FarmHarvestHoursComboBox.SelectedIndex == -1 ||
            ProductHarvestHoursComboBox.SelectedIndex == -1 ||
            ProductCodeHarvestHoursComboBox.SelectedIndex == -1 ||
            SMHoursDateTimePicker.Value == null ||
            EMHoursDateTimePicker.Value == null ||
            SNHoursDateTimePicker.Value == null ||
            ENHoursDateTimePicker.Value == null ;          
        }

        

        public int getEmployeeType()
        {
            if (ControllerRadioButton.Checked)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        private void ClearHarvestHoursButton_Click(object sender, EventArgs e)
        {
            wipeFields();
        }
        private void wipeFields()
        {
            SupplierNameList();
            FarmNameList();
            ProductNameList();

            HarvesterRadioButton.Checked = true;
            HarvestHoursDateTimePicker.Value = DateTime.Now;
            SupplierHarvestHoursComboBox.SelectedIndex = -1;
            FarmHarvestHoursComboBox.SelectedIndex = -1;
            ProductHarvestHoursComboBox.SelectedIndex = -1;
            ProductCodeHarvestHoursComboBox.SelectedIndex = -1;

            SMHoursDateTimePicker.Value = DateTime.Today;
            EMHoursDateTimePicker.Value = DateTime.Today;
            SNHoursDateTimePicker.Value = DateTime.Today;
            ENHoursDateTimePicker.Value = DateTime.Today;
        }


    }
}