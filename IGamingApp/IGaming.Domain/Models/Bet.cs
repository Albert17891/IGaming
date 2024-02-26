namespace IGaming.Domain.Models;

public class Bet
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Details { get; set; }

    public int UserId { get; set; }   
    public User User { get; set; }
}