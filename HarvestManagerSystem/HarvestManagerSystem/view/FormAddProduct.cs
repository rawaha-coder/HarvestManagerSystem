﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HarvestManagerSystem.database;
using HarvestManagerSystem.model;
using HarvestManagerSystem;

namespace HarvestManagerSystem.view
{
    public partial class FormAddProduct : Form
    {
        private bool isEditProduct = false;
        private bool isEditDetail = false;
        private Product mProduct = new Product();
        private ProductDetail mProductDetail = new ProductDetail();
        private ProductDAO mProductDAO = ProductDAO.getInstance();
        private ProductDetailDAO mProductDetailDAO = ProductDetailDAO.getInstance();
        private Dictionary<string, Product> mProductDictionary = new Dictionary<string, Product>();
        

        private HarvestMS harvestMS;
        private static FormAddProduct instance;

        private FormAddProduct(HarvestMS harvestMS)
        {
            this.harvestMS = harvestMS;
            InitializeComponent();
        }

        private void FormAddProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            wipeFields();
            instance = null;
        }


        public static FormAddProduct getInstance(HarvestMS harvestMS)
        {
            if (instance == null)
            {
                instance = new FormAddProduct(harvestMS);
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
                instance = new FormAddProduct(harvestMS);
                
            }
            instance.Show();
        }


        private void FormAddProduct_Load(object sender, EventArgs e)
        {
            ProductNameList();
            if (isEditProduct)
            {
                ProductNameComboBox.SelectedIndex = ProductNameComboBox.FindStringExact(mProduct.ProductName);
            }
        }

        private void ProductNameList()
        {
            List<string> NamesList = new List<string>();
            mProductDictionary.Clear();
            try
            {
                mProductDictionary = mProductDAO.ProductDictionary();
                NamesList.AddRange(mProductDictionary.Keys);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if(NamesList != null)
            {
                ProductNameComboBox.DataSource = NamesList;
            }

            
        }

        private void ProductNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void ProductNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product product = mProductDictionary.GetValueOrDefault(ProductNameComboBox.GetItemText(ProductNameComboBox.SelectedItem));
            if (product != null)
            {
                ProductTypeList(product);
            }
        }

        private void ProductTypeList(Product product)
        {
            List<string> TypeList = new List<string>();
            
            try
            {
                List<ProductDetail> Details = mProductDetailDAO.getData(product);
                if (Details.Count > 0)
                {
                    foreach (ProductDetail productDetail in Details)
                    {
                        TypeList.Add(productDetail.ProductType);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (TypeList != null)
            {
                ProductTypeComboBox.DataSource = TypeList;
            }
        }

        internal void InflateUI(Product product)
        {
            isEditProduct = true;
            //ProductNameComboBox.Text = product.ProductName;
            ProductNameComboBox.SelectedIndex = -1;
            ProductNameComboBox.SelectedIndex = ProductNameComboBox.FindStringExact(product.ProductName);
            mProduct.ProductId = product.ProductId;
            mProduct.ProductName = product.ProductName;
            ProductTypeComboBox.Enabled = false;
            ProductCode.Enabled = false;
            ProductPriceEmployee.Enabled = false;
            ProductPriceCompany.Enabled = false;
            handleSaveButton.Text = "Update";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (isEditDetail)
            {
                UpdateProductDetail(mProductDetail);
            }
            else if (isEditProduct)
            {
                UpdateProduct(mProduct);
            }
            else
            {
                if (CheckInput() || !validateData())
                {
                    MessageBox.Show("Vérifier les valeurs");
                    return;
                }
                SaveProductData();
            }
            harvestMS.DisplayProductData();
        }

        private bool CheckInput()
        {
            nameProductErrorLabel.Visible = ProductNameComboBox.SelectedIndex == -1 && ProductNameComboBox.Text == "";
            typeProductErrorLabel.Visible = ProductTypeComboBox.SelectedIndex == -1 && ProductTypeComboBox.Text == "";
            codeProductErrorLabel.Visible = (ProductCode.Text == "") ? true : false;
            prixEmployeeErrorlabel.Visible = (ProductPriceEmployee.Text == "") ? true : false;
            prixCompanyErrorlabel.Visible = (ProductPriceCompany.Text == "") ? true : false;
            return nameProductErrorLabel.Visible || typeProductErrorLabel.Visible || codeProductErrorLabel.Visible || prixEmployeeErrorlabel.Visible || prixCompanyErrorlabel.Visible;
        }

        private void UpdateProductDetail(ProductDetail productDetail)
        {

            bool isAdded = mProductDetailDAO.UpdateData(productDetail);

            if (isAdded)
            {
                wipeFields();
                MessageBox.Show("data updated");
                this.Close();
            }
            else
            {
                MessageBox.Show("Not updated");
                this.Close();
            }
        }

        private void UpdateProduct(Product product)
        {
            product.ProductName = ProductNameComboBox.Text;

            bool isAdded = mProductDAO.UpdateData(product);
            if (isAdded)
            {
                wipeFields();
                MessageBox.Show("data updated");
                this.Close();
            }
            else
            {
                MessageBox.Show("Not updated");
                this.Close();
            }

        }

        private void SaveProductData()
        {
            Product product;
            if (!mProductDictionary.TryGetValue(ProductNameComboBox.Text, out product))
            {
                Console.WriteLine("no select value");
            }

            ProductDetail productDetail = new ProductDetail();
            productDetail.ProductType = ProductTypeComboBox.Text;
            productDetail.ProductCode = ProductCode.Text;
            productDetail.PriceEmployee  = Convert.ToDouble(ProductPriceEmployee.Text);
            productDetail.PriceCompany = Convert.ToDouble(ProductPriceCompany.Text);
            bool added = false;
            if (product != null)
            {
                productDetail.Product.ProductId = product.ProductId;
                productDetail.Product.ProductName = product.ProductName;
                added = mProductDetailDAO.addData(productDetail);
            }
            else
            {
                productDetail.Product.ProductName = ProductNameComboBox.Text;
                added = mProductDetailDAO.addNewProductDetail(productDetail);

            }
            if (added)
            {
                wipeFields();
            }
            else
            {
                MessageBox.Show("Not added to database: ");
            }
           
            ProductNameList();
            wipeFields();
        }

        private bool validateData()
        {
            Regex regex = new Regex(@"^[0-9]+\.?[0-9]*$");
            return regex.Match(ProductPriceEmployee.Text).Success && regex.Match(ProductPriceCompany.Text).Success;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            wipeFields();
        }
        private void wipeFields()
        {
            ProductNameComboBox.SelectedIndex = -1;
            ProductTypeComboBox.SelectedIndex = -1;
            ProductCode.Text = "";
            ProductPriceEmployee.Text = "";
            ProductPriceCompany.Text = "";
        }


    }
}
