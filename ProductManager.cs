
/// <summary>
/// klass för att hantera Product-objekt. Objekten organiseras i två listor - listOfProducts,<br/>
/// där alla registrerade produkter lagras, och sortedListOfProducts, där senast inköpt produkt lagras i bokstavsordning.
/// </summary>
class ProductManager
{
    private List<Product> listOfProducts;
    private List<Product> sortedListOfProducts;
    //Anger sökväg och filnamn för den txt-fil som lagrar data från programmet
    private string fileName = Application.StartupPath + "\\WinDextProductData.txt"; 

    /// <summary>
    /// Default constructor
    /// </summary>
    public ProductManager()
    {
        listOfProducts = new List<Product>();
        sortedListOfProducts = new List<Product>();

        ReadDataFromFile();
        // TestValues(); Används vid utveckling. Lämnar den kvar utifall att komplettering blir aktuell
    }


    /// <summary>
    /// lägger till ett nytt product-objekt till listOfProducts. Sparar därefter listOfProducts till fil.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public bool AddProduct(Product product)
    {
        bool productIsOk = true;

        if (product != null)
        {
            listOfProducts.Add(product);
            SaveToFile();
        }
        else
            productIsOk = false;

        sortList();
        return productIsOk;
    }

    /// <summary>
    /// Tar bort ett product-objekt från listOfProducts från position som anges av index. Sparar därefter listOfProducts till fil.
    /// </summary>
    /// <param name="index"></param>
    public void RemoveProduct(int index)
    {
        // * leta rätt på produktnamn från sortedListOfProducts
        string prodName = sortedListOfProducts[index].ProductName;

        // * Ta bort alla produkter med funnet produktnamn från listOfProducts
        listOfProducts.RemoveAll(prod =>
        {
            return prod.ProductName == prodName;
        });

        SaveToFile();
        sortList();
    }


    /// <summary>
    /// Sparar aktuell produktlista till lokal textfil.
    /// </summary>
    public void SaveToFile()
    {
        FileManager fileManager = new FileManager();
        fileManager.SaveToFile(listOfProducts, fileName);
    }

    #region FETCH PRODUCT

    /// <summary>
    /// Returnerar lista med produkter. Används i InfoForm för att granska data om konsumtion av vald produkt
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<Product> FetchProductToView(int index)
    {
        List<Product> tempList = new List<Product>();

        string prodName = sortedListOfProducts[index].ProductName;

        tempList = listOfProducts.FindAll(prod => 
        { 
            return prod.ProductName == prodName;
        });

        return tempList;
    }


    /// <summary>
    /// Tar emot index (int) för efterfrågad produkt. Returnerar ett nytt produkt-objekt med nödvändiga värden
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Product FetchProductToRenew(int index)
    {
        string prodName = sortedListOfProducts[index].ProductName;
        DateTime tempDate = new DateTime();
        Product tempProduct = new Product();

        // från listan med registrerade produkter, hämta den produkt med namn == prodName med senaste inköpsdatum
        //  * Tar fram en temporärlista med de produkter med namn == prodName
        List<Product> tempList = new List<Product>();
        tempList = listOfProducts.FindAll(prod =>
        {
            return prod.ProductName == prodName;
        });

        
        //  * Hitta senast inköpsdatum
        for (int i = 0; i < tempList.Count; i++)
        {
            if (i > 0)
            {
                if (tempList[i].PurchaseDataDetails.StartDate > tempList[i - 1].PurchaseDataDetails.StartDate)
                    tempDate = tempList[i].PurchaseDataDetails.StartDate;
            }
            else
                tempDate = tempList[i].PurchaseDataDetails.StartDate;
        }

        //  * Hitta senast inköpt produkt
        Product? foundProduct = listOfProducts.Find(prod => { return prod.PurchaseDataDetails.StartDate == tempDate;});

        // * Gör en "hard copy" av nödvändiga värden till tempProduct
        if(foundProduct != null)
        {
            tempProduct.ProductName = foundProduct.ProductName;
            tempProduct.PurchaseDataDetails.StartDate = foundProduct.PurchaseDataDetails.StartDate;
            tempProduct.PurchaseDataDetails.Amount = foundProduct.PurchaseDataDetails.Amount;
            tempProduct.PurchaseDataDetails.Unit = foundProduct.PurchaseDataDetails.Unit;
            tempProduct.PurchaseDataDetails.Price = foundProduct.PurchaseDataDetails.Price;
        }

        return tempProduct;
    }

    #endregion

    /// <summary>
    /// Returnerar en array (string[]) med information att användas för att visas i listBox i GUI
    /// </summary>
    /// <returns></returns>
    public string[] CreateProductList()
    {
        sortList();
        List<string> tempStringList = new List<string>();
        
        for (int i = 0; i < sortedListOfProducts.Count; i++)
        {
            string prodName = sortedListOfProducts[i].ProductName[0].ToString().ToUpper() + 
                sortedListOfProducts[i].ProductName.Substring(1);
            string startDate = sortedListOfProducts[i].PurchaseDataDetails.StartDate.ToString("dd MMMM yyyy");
            string price = sortedListOfProducts[i].PurchaseDataDetails.Price.ToString("C2");

            tempStringList.Add($"{prodName,-25}{startDate,-25}{price,10}");
        }

        string[] output = tempStringList.ToArray();

        return output;
    }

    /// <summary>
    /// Går igenom listan med registrerade produkter och sparar en kopia (soft copy) sorterad med avseende på produktnamn.
    /// </summary>
    /// <returns></returns>
    private void sortList()
    {
        // Sorterar listan listOfProducts
        listOfProducts.Sort();
        //Tömmer listan sortedListOfProducts
        sortedListOfProducts.Clear();

        // Gå igenom listan. Om produktnamn förekommer fler än en gång -> Spara den senast inköpta
        for (int i = 0; i < listOfProducts.Count; i++)
        {
            if(i == 0)
            {
                sortedListOfProducts.Add(listOfProducts[i]);
            }

            if(i > 0)
            {
                if(listOfProducts[i-1].ProductName.ToLower() == listOfProducts[i].ProductName.ToLower())
                {
                    if (listOfProducts[i-1].PurchaseDataDetails.StartDate.Date <
                        listOfProducts[i].PurchaseDataDetails.StartDate.Date)
                    {
                        sortedListOfProducts[sortedListOfProducts.Count - 1] = listOfProducts[i];
                    }
                }
                else
                {
                    sortedListOfProducts.Add(listOfProducts[i]);
                }
            }
        }
    }

    /// <summary>
    /// Undersöker om det av användaren angivet index omfattas av antal element i listan listOfProducts
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool CheckIndex(int index)
    {
        return index >= 0 && index < listOfProducts.Count;
    }

    /// <summary>
    /// Anropar metoden ReadFromFile() i FileManager. Asynkron operation som returnerar ett Task-objekt
    /// </summary>
    /// <returns></returns>
    public Task ReadDataFromFile()
    {
        FileManager fileManager = new FileManager();
        return Task.Factory.StartNew(() => {
            fileManager.ReadFromFile(listOfProducts, fileName);
        });
    }

    /// <summary>
    /// Testvärden som använts för att underlätta utveckling av programmet
    /// </summary>
    public void TestValues()
    {
        List<Product> products = new List<Product>
        {
            new Product("byxa", new PurchaseData(350, 1, UnitTypes.pcs, new DateTime(2022, 12, 1))),
            new Product("tröja", new PurchaseData(150, 1, UnitTypes.pcs, DateTime.Today)),
            new Product("toalettpapper", new PurchaseData(50, 6,UnitTypes.pcs,new DateTime(2022,11,10))),
            new Product("toalettpapper", new PurchaseData(55, 6,UnitTypes.pcs,new DateTime(2022,12,28))),
            new Product("kaffe", new PurchaseData(48, 450,UnitTypes.gram,new DateTime(2022,12,10))),
            new Product("kaffe", new PurchaseData(48, 450,UnitTypes.gram,new DateTime(2022,12,18))),
            new Product("kaffe", new PurchaseData(51, 450,UnitTypes.gram,new DateTime(2022,12,2))),
            new Product("kaffe", new PurchaseData(50, 450,UnitTypes.gram,new DateTime(2022,12,24)))
        };

        listOfProducts.Clear();

        foreach (var product in products)
            listOfProducts.Add(product);

        SaveToFile();
        sortList();
    }
}
