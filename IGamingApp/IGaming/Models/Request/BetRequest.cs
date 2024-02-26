using System.ComponentModel.DataAnnotations;

namespace IGaming.Models.Request;

public class BetRequest
{
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public string Details { get; set; }
}