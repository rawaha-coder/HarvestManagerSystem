﻿using HarvestManagerSystem.view;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using HarvestManagerSystem.database;
using HarvestManagerSystem.model;

namespace HarvestManagerSystem
{
    public partial class HarvestMS : Form
    {
        EmployeeDAO employeeDAO = EmployeeDAO.getInstance();
        SupplierDAO supplierDAO = SupplierDAO.getInstance();
        SupplyDAO supplyDAO = SupplyDAO.getInstance();
        FarmDAO farmDAO = FarmDAO.getInstance();
        SeasonDAO seasonDAO = SeasonDAO.getInstance();
        ProductDAO productDAO = ProductDAO.getInstance();
        ProductDetailDAO productDetailDAO = ProductDetailDAO.getInstance();
        CreditDAO creditDAO = CreditDAO.getInstance();
        TransportDAO transportDAO = TransportDAO.getInstance();

        private static List<Employee> list = new List<Employee>();

        public HarvestMS()
        {
            InitializeComponent();
        }

        private void HarvestMS_Load(object sender, EventArgs e)
        {
            
        }

        private void tabProduction_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabProduction.SelectedIndex)
            {
                case 0:
                    DisplayQuantityData();
                    break;
                case 1:
                    DisplayHoursData();
                    break;
                case 2:
                    DisplayCreditData();
                    DisplayTransportData();
                    break;
                case 3:
                    DisplayEmployeeData();
                    break;
                case 4:
                    DisplaySupplierData();
                    break;
                case 5:
                    DisplayFarmData();
                    break;
                case 6:
                    DisplayProductData();
                    break;
                default:
                    Console.WriteLine("nothing");
                    break;
            }
        }

        #region SUPPLIER CODE

        List<Supplier> listSupplier = new List<Supplier>();
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            FormAddSupplier formAddSupplier = FormAddSupplier.getInstance(this);
            formAddSupplier.ShowFormAdd();

        }
        public void DisplaySupplierData()
        {
            listSupplier = supplierDAO.getData();
            SupplierDataGridView.DataSource = listSupplier;
        }

        private void SupplierDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int i = SupplierDataGridView.CurrentCell.RowIndex;
            if (i < listSupplier.Count)
            {
                DisplaySupplyData(listSupplier[i]);
            }

        }

        private void DisplaySupplyData(Supplier supplier)
        {
            SupplyDataGridView.DataSource = supplyDAO.getData(supplier);
        }

        private void SupplierContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditSupplierStrip.Name)
            {
                SupplierContextMenuStrip.Visible = false;
                HandleEditSupplierTable();
            }
            else if (e.ClickedItem.Name == DeleteSupplierStrip.Name)
            {
                SupplierContextMenuStrip.Visible = false;
                HandleDeleteSupplierTable();
            }
        }


        void HandleEditSupplierTable()
        {
            FormAddSupplier formAddSupplier = FormAddSupplier.getInstance(this);
            Supplier supplier = (Supplier)SupplierDataGridView.CurrentRow.DataBoundItem;
            if(supplier == null)
            {
                MessageBox.Show("Select fournisseur");
                return;
            }
            formAddSupplier.InflateUI(supplier);
            formAddSupplier.ShowFormAdd();
        }

        void HandleDeleteSupplierTable()
        {
            Supplier supplier = (Supplier)SupplierDataGridView.CurrentRow.DataBoundItem;
            if (supplier == null)
            {
                MessageBox.Show("Select fournisseur");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to delete: " + supplier.SupplierName, "Delete", MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = supplierDAO.DeleteData(supplier);
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }
            DisplaySupplierData();

        }

        private void SupplyContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditSupplyStrip.Name)
            {
                SupplyContextMenuStrip.Visible = false;
                HandleEditSupplyTable();
            }
            else if (e.ClickedItem.Name == DeleteSupplyStrip.Name)
            {
                SupplyContextMenuStrip.Visible = false;
                HandleDeleteSupplyTable();
            }
        }

        void HandleEditSupplyTable()
        {
            FormAddSupplier formAddSupplier = FormAddSupplier.getInstance(this);
            Supplier supplier = (Supplier)SupplierDataGridView.CurrentRow.DataBoundItem;
            Supply supply = (Supply)SupplyDataGridView.CurrentRow.DataBoundItem;
            if(supply == null)
            {
                MessageBox.Show("Select champ");
                return;
            }

            formAddSupplier.InflateUI(supply, supplier);
            formAddSupplier.ShowFormAdd();
        }

        void HandleDeleteSupplyTable()
        {
            Supply supply = (Supply)SupplyDataGridView.CurrentRow.DataBoundItem;
            if (supply == null)
            {
                MessageBox.Show("Select champ");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to delete champ: " + supply.Farm.FarmName, "Delete", MessageBoxButtons.YesNoCancel,
MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = supplyDAO.DeleteData(supply);
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }
            DisplaySupplierData();

        }

        #endregion

        #region FARM CODE

        List<Farm> listFarm = new List<Farm>();
        private void btnAddFarm_Click(object sender, EventArgs e)
        {
            FormAddFarm formAddFarm = FormAddFarm.getInstance(this);
            formAddFarm.ShowFormAdd();
        }

        public void DisplayFarmData()
        {
            listFarm = farmDAO.getData();
            FarmDataGridView.DataSource = listFarm;
        }

        private void FarmtDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int i = FarmDataGridView.CurrentCell.RowIndex;
            if (i < listFarm.Count)
            {
                DisplaySeasonData(listFarm[i]);
            }

        }

        private void FarmContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditFarmStrip.Name)
            {
                FarmContextMenuStrip.Visible = false;
                HandleEditFarmTable();
            }
            else if (e.ClickedItem.Name == DeleteFarmStrip.Name)
            {
                FarmContextMenuStrip.Visible = false;
                HandleDeleteFarmTable();
            }
        }

        void HandleEditFarmTable()
        {

            FormAddFarm formAddFarm = FormAddFarm.getInstance(this);
            Farm farm = (Farm)FarmDataGridView.CurrentRow.DataBoundItem;
            if (farm == null)
            {
                MessageBox.Show("Select farm");
                return;
            }

            formAddFarm.InflateUI(farm);
            formAddFarm.ShowFormAdd();
        }

        void HandleDeleteFarmTable()
        {
            Farm farm = (Farm)FarmDataGridView.CurrentRow.DataBoundItem;
            if (farm == null)
            {
                MessageBox.Show("Select farm");
                return;
            }
            DialogResult dr = MessageBox.Show("Are you sure you want to delete: " + farm.FarmName, "Delete", MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = farmDAO.DeleteData(farm);
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }
            DisplayFarmData();

        }

        #endregion

        #region Season DATA

        private void DisplaySeasonData(Farm farm)
        {
            SeasonDataGridView.DataSource = seasonDAO.getData(farm);
        }

        private void SeasonContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditSeasonStrip.Name)
            {
                SeasonContextMenuStrip.Visible = false;
                HandleEditSeasonTable();
            }
            else if (e.ClickedItem.Name == DeleteSeasonStrip.Name)
            {
                SeasonContextMenuStrip.Visible = false;
                HandleDeleteSeasonTable();
            }
        }

        void HandleEditSeasonTable()
        {
            FormAddFarm formAddFarm = FormAddFarm.getInstance(this);
            Farm farm = (Farm)FarmDataGridView.CurrentRow.DataBoundItem;
            Season season = (Season)SeasonDataGridView.CurrentRow.DataBoundItem;
            if (season == null)
            {
                MessageBox.Show("Select season");
                return;
            }

            formAddFarm.InflateUI(season, farm);
            formAddFarm.ShowFormAdd();
        }

        void HandleDeleteSeasonTable()
        {
            Season season = (Season)SeasonDataGridView.CurrentRow.DataBoundItem;
            if (season == null)
            {
                MessageBox.Show("Select product");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to delete season from: " + season.SeasonPlantingDate + " to " + season.SeasonHarvestDate, "Delete", MessageBoxButtons.YesNoCancel,
MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = seasonDAO.DeleteData(season); //productDetailDAO.DeleteData(productDetail);
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }
            DisplayFarmData();

        }

        #endregion

        #region PRODUCT CODE

        List<Product> listProduct = new List<Product>();
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            FormAddProduct formAddProduct = FormAddProduct.getInstance(this);
            formAddProduct.ShowFormAdd();
        }

        public void DisplayProductData()
        {
            listProduct = productDAO.getData();
            ProductDataGridView.DataSource = listProduct;
        }

        private void ProductDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int i = ProductDataGridView.CurrentCell.RowIndex;
            if (i < listProduct.Count)
            {
                DisplayProductDetailData(listProduct[i]);
            }

        }

        private void ProductContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditProductStrip.Name)
            {
                ProductContextMenuStrip.Visible = false;
                HandleEditProductTable();
            }
            else if (e.ClickedItem.Name == DeleteProductStrip.Name)
            {
                ProductContextMenuStrip.Visible = false;
                HandleDeleteProductTable();
            }
        }

        void HandleEditProductTable()
        {

            FormAddProduct formAddProduct = FormAddProduct.getInstance(this);
            Product product = (Product)ProductDataGridView.CurrentRow.DataBoundItem;
            if (product == null)
            {
                MessageBox.Show("Select Product");
                return;
            }

            formAddProduct.InflateUI(product);
            formAddProduct.ShowFormAdd();
        }

        void HandleDeleteProductTable()
        {
            Product product = (Product)ProductDataGridView.CurrentRow.DataBoundItem;
            if (product == null)
            {
                MessageBox.Show("Select product");
                return;
            }
            DialogResult dr = MessageBox.Show("Are you sure you want to delete: " + product.ProductName, "Delete", MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = productDAO.DeleteData(product);
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }
            DisplayProductData();

        }

        #endregion

        #region PRODUCT DETAIL CODE

        private void DisplayProductDetailData(Product product)
        {
            ProductDetailDataGridView.DataSource = productDetailDAO.getData(product);
        }

        private void ProductDetailContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditProductDetailStrip.Name)
            {
                ProductDetailContextMenuStrip.Visible = false;
                HandleEditProductDetailTable();
            }
            else if (e.ClickedItem.Name == DeleteProductDetailStrip.Name)
            {
                ProductDetailContextMenuStrip.Visible = false;
                HandleDeleteProductDetailTable();
            }
        }

        void HandleEditProductDetailTable()
        {

            FormAddProduct formEditProductDetail = FormAddProduct.getInstance(this);
            Product product = (Product)ProductDataGridView.CurrentRow.DataBoundItem;
            ProductDetail productDetail = (ProductDetail)ProductDetailDataGridView.CurrentRow.DataBoundItem;
            if (productDetail == null)
            {
                MessageBox.Show("Select Product Detail");
                return;
            }

            formEditProductDetail.InflateUI(productDetail, product);
            formEditProductDetail.ShowFormAdd();
        }

        void HandleDeleteProductDetailTable()
        {
            ProductDetail productDetail = (ProductDetail)ProductDetailDataGridView.CurrentRow.DataBoundItem;
            if (productDetail == null)
            {
                MessageBox.Show("Select product");
                return;
            }
            DialogResult dr = MessageBox.Show("Are you sure you want to delete product with code: " + productDetail.ProductCode, "Delete", MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = productDetailDAO.DeleteData(productDetail);
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }
            DisplayProductData();

        }

        #endregion

        #region //TO DO QUANTITY CODE
        void DisplayQuantityData()
        {

        }
        private void btnSearchQuantityProduction_Click(object sender, EventArgs e)
        {

            loadDataProgressBar.Visible = true;
            loadDataProgressBar.Value = 20;

        }

        private void displayQuantityContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        #endregion

        #region //TO DO HOURS CODE
        void DisplayHoursData()
        {

        }
        #endregion

        #region Credit CODE

        List<Credit> listCredit = new List<Credit>();
        private void btnAddCredit_Click(object sender, EventArgs e)
        {
            FormAddCredit formAddCredit = FormAddCredit.getInstance(this);
            formAddCredit.ShowFormAdd();
        }
        public void DisplayCreditData()
        {
            listCredit = creditDAO.getData();
            CreditDataGridView.DataSource = listCredit;
        }

        private void CreditContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditCreditStrip.Name)
            {
                ProductContextMenuStrip.Visible = false;
                HandleEditCreditTable();
            }
            else if (e.ClickedItem.Name == DeleteCreditStrip.Name)
            {
                ProductContextMenuStrip.Visible = false;
                HandleDeleteCreditTable();
            }
        }

        void HandleEditCreditTable()
        {
            FormAddCredit formAddCredit = FormAddCredit.getInstance(this);
            Credit credit = (Credit)CreditDataGridView.CurrentRow.DataBoundItem;


            if (credit == null)
            {
                MessageBox.Show("Select Credit");
                return;
            }

            formAddCredit.InflateUI(credit);
            formAddCredit.ShowFormAdd();
        }

        void HandleDeleteCreditTable()
        {
            Credit credit = (Credit)CreditDataGridView.CurrentRow.DataBoundItem;
            if (credit == null)
            {
                MessageBox.Show("Select product");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to delete " + credit.EmployeeName + " credit", "Delete", MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = creditDAO.DeleteData(credit); 
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }
            DisplayCreditData();

        }

        #endregion

        #region TRANSPORT CODE

        List<Transport> listTransport = new List<Transport>();

        private void btnAddTransport_Click(object sender, EventArgs e)
        {
            FormAddTransport formAddTransport = FormAddTransport.getInstance(this);
            formAddTransport.ShowFormAdd();
        }
        public void DisplayTransportData()
        {
            listTransport = transportDAO.getData();
            TransportDataGridView.DataSource = listTransport;
        }


        private void TransportContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditTransportStrip.Name)
            {
                ProductContextMenuStrip.Visible = false;
                HandleEditTransportTable();
            }
            else if (e.ClickedItem.Name == DeleteTransportStrip.Name)
            {
                ProductContextMenuStrip.Visible = false;
                HandleDeleteTransportTable();
            }
        }

        void HandleEditTransportTable()
        {

            FormAddTransport formAddTransport = FormAddTransport.getInstance(this);
            Transport transport = (Transport)TransportDataGridView.CurrentRow.DataBoundItem;


            if (transport == null)
            {
                MessageBox.Show("Select Credit");
                return;
            }

            formAddTransport.InflateUI(transport);
            formAddTransport.ShowFormAdd();
        }

        void HandleDeleteTransportTable()
        {
            Transport transport = (Transport)TransportDataGridView.CurrentRow.DataBoundItem;
            if (transport == null)
            {
                MessageBox.Show("Select product");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to delete " + transport.EmployeeName + " transport", "Delete", MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = transportDAO.DeleteData(transport);
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }
            DisplayTransportData();

        }

        #endregion

        #region EMPLOYEE CODE

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            FormAddEmployee formAddEmployee = FormAddEmployee.getInstance(this);
            formAddEmployee.ShowFormAdd();
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            DisplayEmployeeData();
        }
        public void DisplayEmployeeData()
        {
            EmployeeDataGridView.DataSource = employeeDAO.getData();
        }

        private void EmployeeContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == EditEmployeeStrip.Name)
            {
                EmployeeContextMenuStrip.Visible = false;
                HandleEditEmployeeTable();
            }
            else if (e.ClickedItem.Name == DeleteEmployeeStrip.Name)
            {
                EmployeeContextMenuStrip.Visible = false;
                HandleDeleteEmployeeTable();
            }
        }

        void HandleEditEmployeeTable()
        {
            FormAddEmployee formAddEmployee = FormAddEmployee.getInstance(this);
            Employee employee = (Employee)EmployeeDataGridView.CurrentRow.DataBoundItem;
            if (employee == null)
            {
                MessageBox.Show("Select Employee");
                return;
            }
            formAddEmployee.InflateUI(employee);
            formAddEmployee.ShowFormAdd();
        }

        void HandleDeleteEmployeeTable()
        {
            Employee selectedEmployee = (Employee)EmployeeDataGridView.CurrentRow.DataBoundItem;
            if (selectedEmployee == null)
            {
                MessageBox.Show("Select Employee");
                return;
            }
            DialogResult dr = MessageBox.Show("Are you sure you want to delete: " + selectedEmployee.FullName, "Delete", MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information);
            bool deleted = false;
            if (dr == DialogResult.Yes)
            {
                deleted = employeeDAO.DeleteData(selectedEmployee);
            }

            if (deleted)
            {
                MessageBox.Show("Delete");
                DisplayEmployeeData();
            }
            else if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Not Delete");
            }

        }

        private void EmployeeDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == EmployeeDataGridView.Columns["employeeStatusColumn"].Index)
            {
                Employee selectedEmployee = (Employee)EmployeeDataGridView.CurrentRow.DataBoundItem;
                employeeDAO.updateEmployeeStatusById(selectedEmployee);
            }

        }

        private void employeeDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (EmployeeDataGridView.DataSource != null)
            {
                if (e.ColumnIndex == EmployeeDataGridView.Columns["employeeStatusColumn"].Index && e.RowIndex != -1)
                {
                    EmployeeDataGridView.EndEdit();
                }
            }
        }











        #endregion


    }
}
