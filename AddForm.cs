using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assignment07_WinDEXT
{
    public partial class AddForm : Form
    {
        private Product product;
        private bool addNewProduct = false; // true/false anger om en ny produkt ska läggas till
                                            // eller om en tidigare registrerad produkt ska uppgraderas.
        
        public AddForm(Product product, bool addNewProduct)
        {
            this.product = product;
            this.addNewProduct = addNewProduct;

            InitializeComponent();
            InitializeGUI();
        }


        /// <summary>
        /// Iordningställer GUI inför användning
        /// </summary>
        private void InitializeGUI()
        {
            this.Text = addNewProduct ? "Add a new product" : "Renew consumption period";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            populateComboBox();
            dateTimePickerStartDate.MaxDate = DateTime.Today;
            //Tooltip
            toolTipAddForm.SetToolTip(txtBxProduct, "The name of the product");
            toolTipAddForm.SetToolTip(txtBxAmount, "The amount of the product you have purchased (gram/litre/pcs)");
            toolTipAddForm.SetToolTip(cmbBxUnits, "Choose the type of unit to be used (gram/litre/pcs)");
            toolTipAddForm.SetToolTip(txtBxPrice, "The price you payed for the product");
            toolTipAddForm.SetToolTip(btnOk, "Register the product and close the form");
            toolTipAddForm.SetToolTip(btnCancel, "Cancel registration of product and close the form");

            if (!addNewProduct)
                PopulateForm();
        }

        /// <summary>
        /// Vid registrering av ny konsumtionsperiod fylls nödvändiga fält i med data från tidigare inköp
        /// </summary>
        private void PopulateForm()
        {
            txtBxProduct.Text = product.ProductName;
            txtBxProduct.Enabled = false; // Produktnamn ska ej behöva ändras om en ny konsumtionsperiod av tidigare registrerad produkt ska inledas. 

            txtBxAmount.Text = product.PurchaseDataDetails.Amount.ToString();
            txtBxPrice.Text = product.PurchaseDataDetails.Price.ToString();
            cmbBxUnits.Text = product.PurchaseDataDetails.Unit.ToString();
            cmbBxUnits.Enabled = false; // Enhet ska ej behöva ändras om en ny konsumtionsperiod av tidigare registrerad produkt ska inledas.
            dateTimePickerStartDate.Value = DateTime.Today;
        }

        /// <summary>
        /// Tilldelar värden till comboBox från enum UnitTypes
        /// </summary>
        private void populateComboBox()
        {
            foreach (var unitType in Enum.GetValues(typeof(UnitTypes)))
                cmbBxUnits.Items.Add(unitType.ToString());

            if (cmbBxUnits.Items.Count > 0)
                cmbBxUnits.SelectedIndex = (int)UnitTypes.pcs;
        }

        #region READ USER INPUTS

        /// <summary>
        /// Metod som kallar på metoder för att läsa in angiven information.<br/>Returnerar true/false beroende på om användaren angivit ett produktnamn.
        /// </summary>
        /// <returns></returns>
        private bool ReadUserInputs()
        {
            ReadUserInputUnit();
            ReadUserInputStartDate();
            
            return ReadUserInputProduct() && ReadUserInputAmount() && ReadUserInputPrice();
        }

        /// <summary>
        /// Läser in inköpsdatum för den vara som ska registreras
        /// </summary>
        private void ReadUserInputStartDate()
        {
            DateTime date = dateTimePickerStartDate.Value;
            product.PurchaseDataDetails.StartDate = date;
        }
        
        /// <summary>
        /// Läser in info om den enhet som används för att definiera mängd
        /// </summary>
        private void ReadUserInputUnit()
        {
            string userInput = cmbBxUnits.Text;
            product.PurchaseDataDetails.Unit= (UnitTypes)Enum.Parse(typeof(UnitTypes), userInput);
        }

        /// <summary>
        /// Läser in info om mängd som ska registreras.<br/> Om använder inte anger ett värde registreras 1
        /// </summary>
        private bool ReadUserInputAmount()
        {
            double amount = 0;
            string userInput = txtBxAmount.Text.Trim();

            bool validInput = double.TryParse(userInput, out amount);

            if (validInput)
                product.PurchaseDataDetails.Amount = amount;
            else
                MessageBox.Show("You must provide the purchased amount of the product you wish to register", "Attention!");

            return validInput;
        }

        /// <summary>
        /// Läser in info om pris för den vara som ska registreras.<br/> Om använder inte anger ett värde registreras 0
        /// </summary>
        private bool ReadUserInputPrice()
        {
            double price = 0;
            string userInput = txtBxPrice.Text.Trim().Replace(".", ",");

            bool validInput = double.TryParse(userInput, out price);

            if(validInput)
                product.PurchaseDataDetails.Price = price;
            else
                MessageBox.Show("You must provide the price of the product you wish to register", "Attention!");

            return validInput;
        }
        /// <summary>
        /// Läser in namn på den produkt som ska registreras.<br/>Returnerar "false" om fältet lämnats tomt. Annars "true"
        /// </summary>
        private bool ReadUserInputProduct()
        {
            bool validInput = false;
            string userInput = txtBxProduct.Text.ToLower().Trim();

            if (!String.IsNullOrEmpty(userInput))
            {
                validInput= true;
                product.ProductName = userInput;
            }
            else
                MessageBox.Show("You must provide the name of the product you wish to register", "Attention!");

            return validInput;
        }

        #endregion

        #region BUTTONS

        /// <summary>
        /// Registrerar ny/uppdaterad produkt och stänger fönster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if(ReadUserInputs())
                DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Avbryter pågående operation. Stänger fönster. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult= DialogResult.Cancel;
        }
        #endregion
    }
}
