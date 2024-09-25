namespace Web.Models;

public class DiscountsDto
{
    public string Code { get; set; }
    public string Id { get; set; }
    public decimal Percentage { get; set; }
    public bool Used { get; set; }
}