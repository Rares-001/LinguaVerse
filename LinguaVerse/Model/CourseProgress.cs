using System.ComponentModel;
using System.Runtime.CompilerServices;

public class CourseProgress : INotifyPropertyChanged
{
    private float _progress;

    public string CourseName { get; set; }

    public float Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            OnPropertyChanged();
            System.Diagnostics.Debug.WriteLine($"Progress value set: {_progress}");
        }
    }

    public string Level { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
