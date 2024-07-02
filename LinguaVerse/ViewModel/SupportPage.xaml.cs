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


}
