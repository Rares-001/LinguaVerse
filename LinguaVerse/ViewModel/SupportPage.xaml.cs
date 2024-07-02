namespace LinguaVerse;

    public partial class SupportPage : ContentPage
    {
        public SupportPage()
        {
            InitializeComponent();
            BindingContext = new SupportViewModel();
        }
    }

public class SupportViewModel : BindableObject
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    private string _subject;
    public string Subject
    {
        get => _subject;
        set
        {
            _subject = value;
            OnPropertyChanged();
        }
    }

    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    private string _responseStatus;
    public string ResponseStatus
    {
        get => _responseStatus;
        set
        {
            _responseStatus = value;
            OnPropertyChanged();
        }
    }

    public Command SendMessageCommand => new Command(async () => await SendMessage());

    private async Task SendMessage()
    {
        var formData = new
        {
            Name = this.Name,
            Email = this.Email,
            Subject = this.Subject,
            Message = this.Message
        };

        var json = JsonConvert.SerializeObject(formData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://127.0.0.1:5000/submit_form", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ResponseStatus = "Form data sent successfully!";
                }
                else
                {
                    ResponseStatus = "Failed to send form data.";
                }
            }
        }
        catch (Exception ex)
        {
            ResponseStatus = $"Error: {ex.Message}";
        }
    }
}