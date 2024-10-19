


using System.ComponentModel.DataAnnotations;

namespace Txt.Ui.Models;

public sealed class RegisterRequest
{
    [Required]
    [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Invalid email")]
    public required string Email { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Incorrect password format.")]
    public required string Password { get; set; }
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
    public required string PasswordConfirmation { get; set; }
}