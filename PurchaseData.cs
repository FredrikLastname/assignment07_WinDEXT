
/// <summary>
/// Klass för att lagra data rörande inköpt produkt
/// </summary>
public class PurchaseData
{
    private double price;
    private double amount;
    private UnitTypes unit;
    private DateTime startDate;
    private DateTime endDate;

    /// <summary>
    /// Default constructor
    /// </summary>
    public PurchaseData()
    {

    }

    /// <summary>
    /// Constructor som tar emot fyra parametrar - price (double), amount (double), unit (UnitTypes) och startDate (DateTime)
    /// </summary>
    /// <param name="price"></param>
    /// <param name="amount"></param>
    /// <param name="unit"></param>
    /// <param name="startDate"></param>
    public PurchaseData(double price, double amount, UnitTypes unit, DateTime startDate)
    {
        this.price = price;
        this.amount = amount;
        this.unit = unit;
        this.startDate = startDate;
    }

    #region PROPS

    /// <summary>
    /// Tilldelar och returnerar värdet för EndDate (DateTime)
    /// </summary>
    public DateTime EndDate
    {
        get { return endDate; }
        set { endDate = value; }
    }

    /// <summary>
    /// Tilldelar och returnerar värdet för StartDate (DateTime)
    /// </summary>
    public DateTime StartDate
    {
        get { return startDate; }
        set { startDate = value; }
    }

    /// <summary>
    /// Tilldelar och returnerar värdet för Unit (UnitTypes)
    /// </summary>
    public UnitTypes Unit
    {
        get { return unit; }
        set { unit = value; }
    }

    /// <summary>
    /// Tilldelar och returnerar värdet för Amount (double)
    /// </summary>
    public double Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    /// <summary>
    /// Tilldelar och returnerar värdet för Price (double)
    /// </summary>
    public double Price
    {
        get { return price; }
        set { price = value; }
    }
    #endregion
}
