namespace IGaming.Domain.Models;
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public byte[] PasswordSalt { get; set; }

    public ICollection<Bet> Bets { get; set; }
}
