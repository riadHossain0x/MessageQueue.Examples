namespace MassTransit.Models;

//[EntityName("produce")]
public class Order
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}