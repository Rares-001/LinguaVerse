using System.Text;
using System;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinguaVerse;

public partial class LogIn : ContentPage
{
	public LogIn()
	{
		InitializeComponent();
	}

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        // Input validation
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageLabel.Text = "Please fill in all fields.";
            return;
        }

        // Hash the password
        string hashedPassword = HashPassword(password);

        // Mock login logic
        bool loginSuccess = await AuthenticateUser(username, hashedPassword);

        if (loginSuccess)
        {
            messageLabel.TextColor = Colors.Green;
            messageLabel.Text = "Login successful!";
            // Navigate to a new page or do something after successful login
        }
        else
        {
            messageLabel.Text = "Login failed. Please try again.";
        }
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private Task<bool> AuthenticateUser(string username, string hashedPassword)
    {
        // Simulate a backend authentication process
        // In real applications, you would send the data to a backend server and handle the response accordingly
        // This is just a mock example
        if (username == "testuser" && hashedPassword == HashPassword("testpassword"))
        {
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}


