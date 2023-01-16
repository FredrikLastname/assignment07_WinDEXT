
/// <summary>
/// Klass som hanterar skriv- och läsoperationer till txt-fil<br/>
/// för att lagra och läsa data som matats in av användaren.
/// </summary>
class FileManager
{
    // Konstnter som skrivs till fil för att identifiera filen vid läsning.
    private const string fileVersionToken = "WinDext_23";
    private const double fileVersionNr = 1.0;

    /// <summary>
    /// sparar aktuell lista med registrerade produkter till fil.
    /// </summary>
    /// <param name="productList"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool SaveToFile(List<Product> productList, string fileName) {
        
        bool fileSavedSuccesfully = true;
        StreamWriter? writer = null;
        //Try-catch för att fånga upp fel vid skrivandet av fil
        try
        {
            writer = new StreamWriter(fileName);
            writer.WriteLine(fileVersionToken);
            writer.WriteLine(fileVersionNr);
            writer.WriteLine(productList.Count);

            for (int i = 0; i < productList.Count; i++)
            {
                writer.WriteLine(productList[i].ProductName);
                writer.WriteLine(productList[i].PurchaseDataDetails.Price);
                writer.WriteLine(productList[i].PurchaseDataDetails.Amount);
                writer.WriteLine(productList[i].PurchaseDataDetails.Unit.ToString());
                writer.WriteLine(productList[i].PurchaseDataDetails.StartDate.Year);
                writer.WriteLine(productList[i].PurchaseDataDetails.StartDate.Month);
                writer.WriteLine(productList[i].PurchaseDataDetails.StartDate.Day);
                writer.WriteLine(productList[i].PurchaseDataDetails.StartDate.Hour);
                writer.WriteLine(productList[i].PurchaseDataDetails.StartDate.Minute);
                writer.WriteLine(productList[i].PurchaseDataDetails.StartDate.Second);
            }

        }
        catch (Exception)
        {
            fileSavedSuccesfully = false;
            
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }

        return fileSavedSuccesfully;
    }

    /// <summary>
    /// Läser in sparade uppgifter från txt-fil
    /// </summary>
    /// <param name="productList"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool ReadFromFile(List<Product> productList, string fileName)
    {
        bool fileReadSuccessfully = true;
        StreamReader? reader = null;

            try
            {
                if (productList != null)
                    productList.Clear();
                else
                    productList = new List<Product>();

                reader = new StreamReader(fileName);

                string? versionTest = reader.ReadLine();
                double? version = double.Parse(reader.ReadLine()!);

                if (versionTest == fileVersionToken && version == fileVersionNr)
                {
                    int count = int.Parse(reader.ReadLine()!);
                    for (int i = 0; i < count; i++)
                    {
                        Product product = new Product();
                        PurchaseData purchaseData = new PurchaseData();

                        int year = 0, month = 0, day = 0,
                            hour = 0, minute = 0, second = 0;


                        product.ProductName = reader.ReadLine()!;

                        purchaseData.Price = double.Parse(reader.ReadLine()!);
                        purchaseData.Amount = double.Parse(reader.ReadLine()!);
                        purchaseData.Unit = (UnitTypes)Enum.Parse(typeof(UnitTypes), reader.ReadLine()!);

                        year = int.Parse(reader.ReadLine()!);
                        month = int.Parse(reader.ReadLine()!);
                        day = int.Parse(reader.ReadLine()!);
                        hour = int.Parse(reader.ReadLine()!);
                        minute = int.Parse(reader.ReadLine()!);
                        second = int.Parse(reader.ReadLine()!);

                        purchaseData.StartDate = new DateTime(year, month, day, hour, minute, second);

                        product.PurchaseDataDetails = purchaseData;

                        productList.Add(product);
                    }

                }
                else
                    fileReadSuccessfully = false;


            }
            catch (Exception)
            {
                fileReadSuccessfully = false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        

        return fileReadSuccessfully;
    }

}