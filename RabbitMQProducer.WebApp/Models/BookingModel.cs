namespace RabbitMQProducer.WebApp.Models;

public class BookingModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public int TicketNo { get; set; }
}
