using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HighscoresInWPF;

namespace Tourplanner
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _output = "Hello World!";
        private string _input;

        public string Input
        {
            get
            {
                Debug.Print("read input");
                return _input;
            }
            set
            {
                Debug.Print("write input");
                if (Input != value)
                {
                    Debug.Print("set input-value");
                    _input = value;
                    Debug.Print("fire propertyChanged: Input");
                    OnPropertyChanged(nameof(Input));
                }
            }
        }

        public string Output
        {
            get
            {
                Debug.Print("read output");
                return _output;
            }
            set
            {
                Debug.Print("write output");
                if (_output != value)
                {
                    Debug.Print("set output");
                    _output = value;
                    Debug.Print("fire propertychanged: output");
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ExecuteCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Debug.Print("ctor MainViewModel");
            this.ExecuteCommand = new RelayCommand((_)=> Output = Input);
            
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.Print($"propertyChanged \"{propertyName}\"");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}