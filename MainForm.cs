using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace assignment07_WinDEXT
{
    public partial class MainForm : Form
    {
        private ProductManager productManager;
        public MainForm()
        {
            productManager= new ProductManager();
            InitializeComponent();
            InitializeGUI();
        }

        /// <summary>
        /// Iordningst�ller GUI inf�r programstart
        /// </summary>
        private async void InitializeGUI()
        {
            this.Text = "Win DExT";
            this.MaximizeBox= false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            grpBxPurchasedProducts.Text = "Purchased products";
            lblProdListInfo.ForeColor = Color.Green;

            //Tooltips
            toolTip.SetToolTip(btnAdd, "Add a new product");
            toolTip.SetToolTip(btnClear, "Clear selection");
            toolTip.SetToolTip(btnRenew, "Renew the consumption period of selected product");
            toolTip.SetToolTip(btnDelete, "Delete selected product and related data");

            await productManager.ReadDataFromFile(); // Data m�ste l�sas in fr�n fil innan anrop av UpdateListBoxProducts().
                                                     // H�nsyn m�ste ocks� tas till att det tar tid (om �n lite) att l�sa fr�n fil. D�rf�r async/await
            ResetControls();
            UpdateListBoxProducts();
        }

        #region BUTTONS

        /// <summary>
        /// Avaktiverar knapparna Clear, Renew och Delete. Avmarkerar vald prod i listBox
        /// </summary>
        private void ResetControls()
        {
            btnClear.Enabled = false;
            btnRenew.Enabled = false;
            btnDelete.Enabled = false;
            lstBxProducts.ClearSelected();
        }


        /// <summary>
        /// �ppnar nytt f�nster f�r att l�gga till ny produkt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Product tempProduct = new Product();
            AddForm addForm = new AddForm(tempProduct, true);
            DialogResult result = addForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                productManager.AddProduct(tempProduct);
                UpdateListBoxProducts();
            }

            ResetControls();
        }

        /// <summary>
        /// Avmerkerar vald produkt och �terst�ller knappstatus till ursprungstillst�nd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        /// <summary>
        /// Tar bort vald produkt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = lstBxProducts.SelectedIndex;

            if(index >= 0)
            {
                DialogResult result = MessageBox.Show("You are about to remove selected product and all related purchase data. Do you wish to continue?",
                    "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    productManager.RemoveProduct(index);

                UpdateListBoxProducts();
                ResetControls();
            }
        }

        /// <summary>
        /// �ppnar nytt f�nster f�r att f�rnya/�ndra vald produkt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRenew_Click(object sender, EventArgs e)
        {
            int index = lstBxProducts.SelectedIndex;

            if(index >= 0)
            {
                Product? tempProduct = productManager.FetchProductToRenew(index);
                
                AddForm addForm = new AddForm(tempProduct!, false);
                DialogResult result = addForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    productManager.AddProduct(tempProduct!);
                    UpdateListBoxProducts();
                }
            }

            ResetControls();
        }

        #endregion

        #region LIST BOX OPERATIONS
        /// <summary>
        /// Uppdaterar listBox med data fr�n lista ink�pta produkter i productManager.
        /// </summary>
        private void UpdateListBoxProducts()
        {
            lstBxProducts.Items.Clear();

            foreach (var item in productManager.CreateProductList())
                lstBxProducts.Items.Add(item);

            DisplayProdListInfo(lstBxProducts.Items.Count > 0);
        }

        /// <summary>
        /// Visar informationstext i lblProdListInfo. Texten �r beroende av om listBox inneh�ller produkter eller ej.
        /// </summary>
        /// <param name="listBoxPopulated"></param>
        private void DisplayProdListInfo(bool listBoxPopulated)
        {
            string itemsInListBox = "Mark a product in the list to enable editing options." +
                Environment.NewLine + "Double click on a product to view consumption information";
            string listBoxEmpty = "Add a product to begin to monitor your consumption costs";

            lblProdListInfo.Text = listBoxPopulated ? itemsInListBox : listBoxEmpty;
        }

        /// <summary>
        /// Vid dubbelklick - �ppnar f�nster med info om kostnad 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstBxProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lstBxProducts.SelectedIndex;

            if(index >= 0)
            {
                List<Product> tempList = productManager.FetchProductToView(index);

                InfoForm infoForm = new InfoForm(tempList);
                infoForm.ShowDialog();
                ResetControls();
            }

        }

        /// <summary>
        /// Vid "enkelklick" - Markerar en produkt. Aktiverar knappar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstBxProducts_MouseClick(object sender, MouseEventArgs e)
        {
            int index = lstBxProducts.SelectedIndex;
            if(index >= 0)
            {
                btnRenew.Enabled = true;
                btnClear.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
        #endregion

        #region TOOLSTRIP
        /// <summary>
        /// Avslutar programmet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You are about to exit the program.",
                "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
                Application.Exit();
            else
                return;
        }

        /// <summary>
        /// �ppnar AboutBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox= new AboutBox();
            aboutBox.Show();
        }

        /// <summary>
        /// F�r att underl�tta demonstration av programmet. L�ser in exempelprodukter till listan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadDemoProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will load a list with demo products. All previously registered products will be replaced.", "Attention!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                productManager.TestValues();
                UpdateListBoxProducts();
            }
            else 
                return;
        }
        #endregion
    }
}
