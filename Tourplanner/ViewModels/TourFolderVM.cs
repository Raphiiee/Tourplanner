using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HighscoresInWPF;
using Tourplanner.BusinessLayer;
using Tourplanner.Models;
using System.Runtime.CompilerServices;

namespace Tourplanner.ViewModels
{
    public class TourFolderVM : ViewModelBase
    {
        private ICommand searchCommand;
        private ICommand clearCommand;
        private TourItem currentItem;
        private ITourItemFactory tourItemFactory;
        private string searchName;

        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => clearCommand ??= new RelayCommand(Clear);

        public ObservableCollection<TourItem> Items { get; set; }

        public TourItem CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                if (currentItem != value && value != null)
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                }
            }
        }

        public string SearchName
        {
            get
            {
                return searchName;
            }
            set
            {
                if (searchName != value)
                {
                    searchName = value;
                    RaisePropertyChangedEvent(nameof(SearchName));
                }
            }
        }

        public TourFolderVM()
        {
            tourItemFactory = TourItemFactory.GetInstance();
            InitListBox();
        }

        private void InitListBox()
        {
            Items = new ObservableCollection<TourItem>();
            FillListBox();
        }

        private void FillListBox()
        {
            foreach (TourItem item in tourItemFactory.GetItems())
            {
                Items.Add(item);
            }
        }

        private void Search(object commandParameter)
        {
            IEnumerable foundItems = tourItemFactory.Search(SearchName);
            Items.Clear();
            foreach (TourItem item in foundItems)
            {
                Items.Add(item);
            }
        }

        private void Clear(object commandParameter)
        {
            Items.Clear();
            SearchName = "";
            FillListBox();
        }

        

    }
}