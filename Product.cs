
/// <summary>
/// Klass som definierar produkt-objektet som används för att lagra data om inköp
/// </summary>
public class Product: IComparable<Product>
{
    private PurchaseData purchaseData;
    private string productName;
    
    /// <summary>
    /// default constructor
    /// </summary>
    public Product()
    {
        purchaseData = new PurchaseData();
        productName = String.Empty;
    }

    /// <summary>
    /// constructor som tar emot två parametrar - productName (string) och purchaseData (PurchaseData)
    /// </summary>
    /// <param name="productName"></param>
    /// <param name="purchaseData"></param>
    public Product(string productName, PurchaseData purchaseData)
    {
        this.productName = productName;

        if (purchaseData != null)
            this.purchaseData = purchaseData;
        else
            this.purchaseData = new PurchaseData();
    }

    #region PROPS
    /// <summary>
    /// Tilldelar och returnerar värdet för PurchaseDataDetails (PurchaseData)
    /// </summary>
    public PurchaseData PurchaseDataDetails
    {
        get { return purchaseData; }
        set { purchaseData = value; }
    }

    /// <summary>
    /// Tilldelar och returnerar värdet för ProductName (string)
    /// </summary>
    public string ProductName
    {
        get { return productName; }
        set { productName = value; }
    }

    #endregion


    /// <summary>
    /// För att jämföra två produkt-objekt med avseende på produktnamn. Returnerar int.<br/>
    /// Om int == 0, objekt har samma namn. Om int < 0, aktuellt objekts produktnamn kommer<br/>
    /// före det objekt det jämförs med. Om int > 0, aktuellt objekts produktnamn kommer efter det objekt det jämförs med. 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Product? other)
    {
        if (other == null)
            return 1;
        else
            return this.productName.CompareTo(other.ProductName);
    }

}
