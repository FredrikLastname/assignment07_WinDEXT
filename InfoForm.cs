using ScottPlot.MarkerShapes;
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
    public partial class InfoForm : Form
    {
        private List<Product> productToView;
        private Datapreparation datapreparation;
        
        public InfoForm(List<Product> productToView)
        {
            InitializeComponent();
            datapreparation = new Datapreparation();
            this.productToView = productToView;
            InitializeGUI();
        }

        /// <summary>
        /// Iordningställer det grafiska gränssnittet för informationsfönstret
        /// </summary>
        private void InitializeGUI()
        {
            this.Text = "Purchase information";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            grpBxInfo.Text= $"Consumption info for {productToView[0].ProductName.ToUpper()}";

            toolTipInfo.SetToolTip(formsPlot, "Diagram representing the cost development of the latest consumption period");
            toolTipInfo.SetToolTip(lstBxProductInfo, "Information about cost and expenses of selected product");
            toolTipInfo.SetToolTip(btnOk, "Close the window");

            UpdateListBox();
            UpdatePlot();
        }

        /// <summary>
        /// Använder tillägget scottplot (NuGet) för att rita diagram för att visualisera kostnadsutveckling för vald produkt
        /// https://scottplot.net/
        /// </summary>
        private void UpdatePlot()
        {
            double[] dataY = datapreparation.PreparePlotData(productToView);
            // double[] averageCost = new double[dataY.Length];
            
            double[] dataX = new double[dataY.Length];

            double costPerDay = datapreparation.CostPerDay(productToView);
            string chartTitle = $"Cost development since last purchase ({datapreparation.LastPurchased(productToView).ToString("d")})";

            for (int i = 0; i < dataY.Length; i++)
            {
                dataX[i] = i + 1;
                // averageCost[i] = costPerDay;
            }
            
            formsPlot.Plot.XAxis.MinimumTickSpacing(1);
            formsPlot.Plot.Title($"{chartTitle}", false, null, 15 );
            
            if(dataY.Length > 1)
                formsPlot.Plot.AddScatterLines(dataX, dataY, Color.Blue); // ritar linje för att markera kostnadsutveckling
            else
                formsPlot.Plot.AddScatter(dataX, dataY, null, 1, 5); // Om det bara finns en "mätpunkt" dvs inköpsdatum är samma som dagens datum,
                                                                      // markeras värdet med en cirkel

            // formsPlot.Plot.AddScatterLines(dataX, averageCost ,Color.Red);

            formsPlot.Refresh();
        }

        /// <summary>
        /// Uppdaterar listBox med data om konsumtion avseende vald produkt.
        /// </summary>
        private void UpdateListBox()
        {
            lstBxProductInfo.Items.Clear();

            foreach (var infoString in datapreparation.PrepareData(productToView))
                lstBxProductInfo.Items.Add(infoString);
        }

        /// <summary>
        /// Stänger fönstret
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }


    /// <summary>
    /// Klass som bereder den data som presenteras genom infoForm
    /// </summary>
    class Datapreparation
    {
        public Datapreparation()
        {

        }

        /// <summary>
        /// Returnerar en array (string[]) med data om konsumtion av vald produkt
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public string[] PrepareData(List<Product> product)
        {
            List<string> tempList = new List<string>();
            const int textSpace = -40;
            
            tempList.Add($"{"Last purchased:",textSpace}{LastPurchased(product).ToString("dd MMMM yyyy")}"); //senast inköpt
            tempList.Add($"{"Amount:",textSpace}{LastPurchasedAmount(product)} {LastPurchasedUnit(product).ToString()}"); //senast inköpt mängd och enhet
            tempList.Add($"{"Price:",textSpace}{LastPurchasedPrice(product):C2}"); // senast inköpt pris
            tempList.Add($"{"Cost/Day:",textSpace}{CostPerDay(product):C2}/day");

            // 
            if(product.Count > 1)
            {
                tempList.Add(Environment.NewLine);
                tempList.Add($"{"number of purchases:", textSpace}{product.Count}");
                tempList.Add($"{"Average price:", textSpace}{AveragePrice(product):C2}");
                tempList.Add($"{"Est. consumption time:", textSpace}{ConsumptionTime(product):0} day(s)");
                tempList.Add($"{"Est. consumed:",textSpace}{EstimateWhenConsumed(product).Date.ToString("dd MMMM yyyy")}");
                tempList.Add($"{"Avg. consumption/Day:",textSpace}{ConsumptionPerDay(product):0.00} {LastPurchasedUnit(product).ToString()}/day");
            }

            return tempList.ToArray();
        }

        /// <summary>
        /// Returnerar en array (double[]) med data att användas för att rita diagram
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public double[] PreparePlotData(List<Product> product)
        {
            List<double> temp = new List<double>();
            DateTime today = DateTime.Today; 
            DateTime lastPurchased = LastPurchased(product); // senast inköp

            //Konsumtionsperiodens längd. Sätts till 1 om senast inköp är "idag"
            int days = (today - lastPurchased).Days;
            days += 1; // För att ta med "idag" i beräkningen. En produkt som köptes igår har nu 2 dagars konsumtionsperiod istället för 1.

            // Finn index för senast inköp.
            int index = IndexOfLastPurchased(lastPurchased, product);

            // Pris vid senast inköp
            double price = product[index].PurchaseDataDetails.Price;

            // Lägg till pris/konsumtionstid i listan temp.
            for (int i = 0; i < days; i++)
            {
                temp.Add(price / (i+1));
            }

            return temp.ToArray();
        }


        /// <summary>
        /// Använder bubblesort för att sortera lista med avseende på startDate i stigande ordning
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private List<Product> SortByDate(List<Product> product)
        {
            // List<Product> tempList = new List<Product>();
            Product? tempProd = null;

            for (int i = 0; i < product.Count; i++)
            {
                for (int j = 0; j < (product.Count-1); j++)
                {
                    if (product[j].PurchaseDataDetails.StartDate.Date > product[j + 1].PurchaseDataDetails.StartDate.Date)
                    {
                        tempProd = product[j];
                        product[j] = product[j+1];
                        product[j+1] = tempProd;
                    }
                }
            }
            return product;
        }
        
        /// <summary>
        /// Returnerar uppskattning om konsumtionsperiodens längd baserat på tidigare konsumtion
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private double ConsumptionTime(List<Product> product)
        {
            double tempDifference = 0;
            List<Product> sortedList = SortByDate(product);

            for (int i = 0; i < sortedList.Count-1; i++)
            {
                DateTime startDate = sortedList[i].PurchaseDataDetails.StartDate.Date;
                DateTime endDate = 
                    sortedList[i].PurchaseDataDetails.StartDate.Date > startDate ? 
                    sortedList[i].PurchaseDataDetails.EndDate.Date : 
                    sortedList[i+1].PurchaseDataDetails.StartDate.Date;

                tempDifference += (endDate - startDate).TotalDays;
            }

            return tempDifference / (sortedList.Count-1);
        }


        /// <summary>
        /// Returnerar kostnad per dag för produkten
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        internal double CostPerDay(List<Product> product)
        {
            DateTime lastPurchased = LastPurchased(product);
            DateTime today = DateTime.Today;

            int days = (today - lastPurchased).Days;
            days += 1; // För att ta med "idag" i beräkningen. En produkt som köptes igår har nu 2 dagars konsumtionsperiod istället för 1.
            
            int index = IndexOfLastPurchased(lastPurchased, product);
            double price = product[index].PurchaseDataDetails.Price;

            return price / days;
        }


        /// <summary>
        /// returnerar ett uppskattat datum då senast inköpt vara tagit slut
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private DateTime EstimateWhenConsumed(List<Product> product)
        {
            DateTime lastPurchased = LastPurchased(product);
            double consumptionTime = Math.Floor(ConsumptionTime(product));

            return lastPurchased.AddDays(consumptionTime);
        }

        /// <summary>
        /// Returnerar en uppskattning av förbrukning per dag
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private double ConsumptionPerDay(List<Product> product)
        {
            List<Product> sortedList = SortByDate(product);

            double amount = 0;
            double days = 0;

            for (int i = 0; i < sortedList.Count - 1; i++)
            {
                DateTime startDate = sortedList[i].PurchaseDataDetails.StartDate.Date;
                DateTime endDate =
                    sortedList[i].PurchaseDataDetails.StartDate.Date > startDate ?
                    sortedList[i].PurchaseDataDetails.EndDate.Date :
                    sortedList[i + 1].PurchaseDataDetails.StartDate.Date;

                amount += sortedList[i].PurchaseDataDetails.Amount;
                days += (endDate - startDate).TotalDays;
            }

            if (days == 0)
                days = 1;

            return amount / days;
        }

        /// <summary>
        /// returnerar genomsnittligt pris som betalats för produkten
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private double AveragePrice(List<Product> product)
        {
            double tempPrice = 0;

            foreach (var regProd in product)
            {
                tempPrice += regProd.PurchaseDataDetails.Price;
            }

            return tempPrice/product.Count;
        }


        /// <summary>
        /// Returnerer datum (DateTime) för senaste inköp. Tidsangivelsen är i returvärdet satt till 00:00:00
        /// </summary>
        /// <param name="productToView"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal DateTime LastPurchased(List<Product> productToView)
        {
            DateTime tempDate = new DateTime();
                        
            for (int i = 0; i < productToView.Count; i++)
            {
                if(i > 0)
                {
                    if(productToView[i].PurchaseDataDetails.StartDate > productToView[i-1].PurchaseDataDetails.StartDate)
                        tempDate = productToView[i].PurchaseDataDetails.StartDate;
                }
                else
                    tempDate = productToView[i].PurchaseDataDetails.StartDate;
            }

            return tempDate.Date;
        }

        /// <summary>
        /// returnerar mängd av vara som senast köptes
        /// </summary>
        /// <param name="lastPurchased"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal double LastPurchasedAmount(List<Product> productToView)
        {
            DateTime lastPurchased = LastPurchased(productToView);
            int index = IndexOfLastPurchased(lastPurchased, productToView);

            return productToView[index].PurchaseDataDetails.Amount;
        }

        /// <summary>
        /// Returnerar pris för produkt som betalades vid senaste inköp
        /// </summary>
        /// <param name="productToView"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal double LastPurchasedPrice(List<Product> productToView)
        {
            DateTime lastPurchased = LastPurchased(productToView);
            int index = IndexOfLastPurchased(lastPurchased, productToView);

            return productToView[index].PurchaseDataDetails.Price;
        }

        /// <summary>
        /// Returnerar den enhet (UnitTypes) som registrerades vid senaste inköp
        /// </summary>
        /// <param name="productToView"></param>
        /// <returns></returns>
        internal UnitTypes LastPurchasedUnit(List<Product> productToView)
        {
            DateTime lastPurchased = LastPurchased(productToView);
            int index = IndexOfLastPurchased(lastPurchased, productToView);
            
            return productToView[index].PurchaseDataDetails.Unit;
        }

        /// <summary>
        /// Returnerar position i listan (int) över produkter för senast inköpta artikel
        /// </summary>
        /// <param name="lastPurchased"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        private int IndexOfLastPurchased(DateTime lastPurchased, List<Product> products)
        {
            int index = products.FindIndex(prod =>
            {
                return prod.PurchaseDataDetails.StartDate.Date == lastPurchased;
            });

            return index;
        }
    }
}
