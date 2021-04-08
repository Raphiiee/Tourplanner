using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Tourplanner.BusinessLayer;
using Tourplanner.Models;

namespace Tourplanner.ViewModels
{
    public class TourFolderVM : ViewModelBase
    {
        private ICommand _searchCommand;
        private ICommand _clearCommand;
        private ICommand _addTourCommand;
        private TourItem _currentItem;
        private ITourItemFactory _tourItemFactory;
        private string _searchName;

        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(Clear);
        public ICommand AddTourCommand => _addTourCommand ??= new RelayCommand(AddTourItem);

        public ObservableCollection<TourItem> Items { get; set; }

        public TourItem CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                if (_currentItem != value && value != null)
                {
                    _currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                }
            }
        }

        public string SearchName
        {
            get
            {
                return _searchName;
            }
            set
            {
                if (_searchName != value)
                {
                    _searchName = value;
                    RaisePropertyChangedEvent(nameof(SearchName));
                }
            }
        }

        public TourFolderVM()
        {
            _tourItemFactory = TourItemFactory.GetInstance();
            InitListBox();
        }

        private void InitListBox()
        {
            Items = new ObservableCollection<TourItem>();
            FillListBox();
        }

        private void FillListBox()
        {
            foreach (TourItem item in _tourItemFactory.GetItems())
            {
                Items.Add(item);
            }

            if (Items.Count > 0)
            {
                CurrentItem = Items[0];
            }
        }

        private void Search(object commandParameter)
        {
            IEnumerable foundItems = _tourItemFactory.Search(SearchName);
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

        private void AddTourItem(object commandParameter)
        {
            IEnumerable newItem = _tourItemFactory.AddTourItem();
            foreach (TourItem item in newItem)
            {
                Items.Add(item);
            }
        }

    }
}