using System;
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
        private ICommand _deleteTourCommand;
        private ICommand _deleteLogCommand;
        private ICommand _addLogCommand;
        private TourItem _currentItem;
        private LogItem _currentLogItem;
        private ITourItemFactory _tourItemFactory;
        private string _searchName;

        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(Clear);
        public ICommand AddTourCommand => _addTourCommand ??= new RelayCommand(AddTourItem);
        public ICommand DeleteTourCommand => _deleteTourCommand ??= new RelayCommand(DeleteTourItem);
        public ICommand AddLogCommand => _addLogCommand ??= new RelayCommand(AddLogItem);
        public ICommand DeleteLogCommand => _deleteLogCommand ??= new RelayCommand(DeleteLogItem);

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

        public LogItem CurrentLogItem
        {
            get
            {
                return _currentLogItem;
            }
            set
            {
                if (_currentLogItem != value && value != null)
                {
                    _currentLogItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentLogItem));
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

            PreselectListviewItem();
        }

        private void PreselectListviewItem()
        {
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
            
            PreselectListviewItem();
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

            if (Items.Count < 2)
            {
                PreselectListviewItem();
            }
        }

        private void DeleteTourItem(object commandParameter)
        {
            _tourItemFactory.DeleteTourItem(CurrentItem);
            Items.Remove(CurrentItem);
            PreselectListviewItem();
        }

        private void AddLogItem(object commandParameter)
        {
            //CurrentItem.Log.Add(new LogItem());
            //RaisePropertyChangedEvent(nameof(Items));

            int i = Items.IndexOf(CurrentItem);
            Items[i].Log.Add(new LogItem(){Date = DateTime.Now});

            RaisePropertyChangedEvent(nameof(Items));
        }

        private void DeleteLogItem(object commandParameter)
        {
            _tourItemFactory.DeleteLogItem(CurrentLogItem);
            int i = Items.IndexOf(CurrentItem);
            Items[i].Log.Remove(CurrentLogItem);

            RaisePropertyChangedEvent(nameof(Items));
        }

    }
}