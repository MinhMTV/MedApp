using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CBMTraining.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ICommand SkeletonCommand { get; set; }

        public ICommand BackCommand { get; set; }

        public BaseViewModel()
        {
            SkeletonCommand = new Command(async (x) =>
            {
                IsBusy = true;
                await Task.Delay(4000);
                IsBusy = false;
            });

            BackCommand = new Command((x) =>
            {
                Shell.Current.SendBackButtonPressed();
            });
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

