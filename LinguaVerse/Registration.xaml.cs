using System.Text.RegularExpressions;
using System.Text;
using Microsoft.Maui.Controls;
using System.Security.Cryptography;


namespace LinguaVerse;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
	}

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string email = emailEntry.Text;
        string password = passwordEntry.Text;
        string confirmPassword = confirmPasswordEntry.Text;

        // Input validation
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            messageLabel.Text = "Please fill in all fields.";
            return;
        }

        if (!IsValidEmail(email))
        {
            messageLabel.Text = "Invalid email format.";
            return;
        }

        if (password != confirmPassword)
        {
            messageLabel.Text = "Passwords do not match.";
            return;
        }

        // Hash the password
        string hashedPassword = HashPassword(password);

        // Mock registration logic
        bool registrationSuccess = await RegisterUser(username, email, hashedPassword);

        if (registrationSuccess)
        {
            messageLabel.TextColor = Colors.Green;
            messageLabel.Text = "Registration successful!";
        }
        else
        {
            messageLabel.Text = "Registration failed. Please try again.";
        }
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private Task<bool> RegisterUser(string username, string email, string hashedPassword)
    {
        // Simulate a backend registration process
        // In real applications, you would send the data to a backend server and handle the response accordingly
        return Task.FromResult(true);
    }
}
