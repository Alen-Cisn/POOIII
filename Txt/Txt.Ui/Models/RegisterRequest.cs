using System.ComponentModel.DataAnnotations;

namespace Txt.Ui.Models;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Incorrect password format.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirmation for password is required.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
    public string PasswordConfirmation { get; set; } = null!;
}