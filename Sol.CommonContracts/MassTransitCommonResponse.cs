namespace Sol.CommonContracts;

public class MassTransitCommonResponse
{

    public bool IsSuccess { get; set; }
    public object? Data { get; set; }
    public string? Error { get; set; }
  
}
