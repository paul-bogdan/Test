namespace Cart.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<CartItem> Items { get; set; } = new();
    public decimal Total { get; set; }
    public decimal SubTotal { get; set; }
    public List<CartDiscount> Discounts { get; set; } = new();
    
    public void CalculateTotal()
    {
        Total = SubTotal;
        foreach (var discount in Discounts)
        {
            Total -= discount.Value;
        }
    }
    
    public void AddItem(CartItem item)
    {
        Items.Add(item);
        SubTotal += item.Price;
        CalculateTotal();
    }
    
    public void RemoveItem(Guid itemId)
    {
        var item = Items.Find(i => i.Id == itemId);
        if(item == null) return;
        Items.Remove(item);
        SubTotal -= item.Price;
        CalculateTotal();
    }
    
    public void AddDiscount(CartDiscount discount)
    {
        if(Discounts.Exists(x=> x.DiscountCode==discount.DiscountCode)) return;
        if(Total==0)  return;
        
        discount.Value = Total * discount.Percentage / 100;
        Discounts.Add(discount);
        CalculateTotal();
    }
    
    public void RemoveDiscount(string discountId)
    {
        var discount = Discounts.Find(d => d.DiscountId == discountId);
        if(discount == null) return;
        Discounts.Remove(discount);
        CalculateTotal();
    }
    
    
}