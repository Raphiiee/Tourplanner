using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Tourplanner.BusinessLayer;
using Tourplanner.Models;
[assembly: log4net.Config.XmlConfigurator(Watch=true)]


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
        private ICommand _alterLogCommand;
        private ICommand _alterTourDetails;
        private ICommand _printSpecificTourReport;
        private ICommand _printSumerizeTourReport;
        private TourItem _selectedTourItem;
        private LogItem _selectedLogItem;
        private ITourItemFactory _tourItemFactory;
        private string _searchName;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("TourFolderVM.cs");

        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(Clear);
        public ICommand AddTourCommand => _addTourCommand ??= new RelayCommand(AddTourItem);
        public ICommand DeleteTourCommand => _deleteTourCommand ??= new RelayCommand(DeleteTourItem);
        public ICommand AddLogCommand => _addLogCommand ??= new RelayCommand(AddLogItem);
        public ICommand DeleteLogCommand => _deleteLogCommand ??= new RelayCommand(DeleteLogItem);
        public ICommand AlterLogCommand => _alterLogCommand ??= new RelayCommand(AlterLogItem);
        public ICommand AlterTourDetailsCommand => _alterTourDetails ??= new RelayCommand(AlterTourDetails);
        public ICommand PrintSpecificTourReportCommand => _printSpecificTourReport ??= new RelayCommand(PrintSpecificTourReport);
        public ICommand PrintSumerizeTourReportCommand => _printSumerizeTourReport ??= new RelayCommand(PrintSumerizeTourReport);

        public ObservableCollection<TourItem> TourItemsList { get; set; }

        public TourFolderVM()
        {
            _tourItemFactory = TourItemFactory.GetInstance();
            InitListBox();
        }

        public TourItem SelectedTourItem
        {
            get
            {
                return _selectedTourItem;
            }
            set
            {
                if (_selectedTourItem != value && value != null)
                {
                    _selectedTourItem = value;
                    RaisePropertyChangedEvent(nameof(SelectedTourItem));
                }
            }
        }

        public LogItem SelectedLogItem
        {
            get
            {
                return _selectedLogItem;
            }
            set
            {
                if (_selectedLogItem != value && value != null)
                {
                    _selectedLogItem = value;
                    RaisePropertyChangedEvent(nameof(SelectedLogItem));
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

        public RouteTypeEnum[] PossibleRouteTypes => new RouteTypeEnum[]
        {
            RouteTypeEnum.Fastest,
            RouteTypeEnum.Shortest,
            RouteTypeEnum.Pedestrian,
            RouteTypeEnum.Bicycle
        };

        private void InitListBox()
        {
            log.Info("Init Programm");
            TourItemsList = new ObservableCollection<TourItem>();
            FillListBox();
        }

        private void FillListBox()
        {
            foreach (TourItem item in _tourItemFactory.GetItems())
            {
                TourItemsList.Add(item);
            }

            PreselectListviewItem();
        }

        private void PreselectListviewItem()
        {
            if (TourItemsList.Count > 0)
            {
                SelectedTourItem = TourItemsList[0];
            }
        }

        private void Search(object commandParameter)
        {
            log.Info($"Search for {SearchName} in tour data");
            IEnumerable foundItems = _tourItemFactory.Search(SearchName);
            TourItemsList.Clear();
            foreach (TourItem item in foundItems)
            {
                TourItemsList.Add(item);
            }
            
            PreselectListviewItem();
        }

        private void Clear(object commandParameter)
        {
            log.Info("Clear Search");
            TourItemsList.Clear();
            SearchName = "";
            FillListBox();
        }

        private void AddTourItem(object commandParameter)
        {
            TourItem newItem = _tourItemFactory.AddTourItem();
            TourItemsList.Add(newItem);

            if (TourItemsList.Count < 2)
            {
                PreselectListviewItem();
            }
        }

        private void DeleteTourItem(object commandParameter)
        {
            _tourItemFactory.DeleteTourItem(SelectedTourItem);
            TourItemsList.Remove(SelectedTourItem);
            PreselectListviewItem();
        }

        private void AddLogItem(object commandParameter)
        {
            SelectedLogItem = new LogItem();
            // Add new logitem in database
            _tourItemFactory.AddLogItem(SelectedLogItem, SelectedTourItem);
            // Index of Selected Tour Item in Listbox
            int i = TourItemsList.IndexOf(SelectedTourItem);
            // Add new Log Item at selected Tour Item index
            TourItemsList[i].Log.Add(SelectedLogItem);
            

            RaisePropertyChangedEvent(nameof(TourItemsList));
        }

        private void DeleteLogItem(object commandParameter)
        {
            _tourItemFactory.DeleteLogItem(SelectedLogItem);
            int i = TourItemsList.IndexOf(SelectedTourItem);
            TourItemsList[i].Log.Remove(SelectedLogItem);

            RaisePropertyChangedEvent(nameof(TourItemsList));
        }

        private void AlterLogItem(object commandParameter)
        {
            _tourItemFactory.AlterLogItem(SelectedTourItem);
        }

        private void AlterTourDetails(object commandParameter)
        {
            
            _tourItemFactory.AlterTourDetails(_selectedTourItem);
            _tourItemFactory.CleanUpImages(TourItemsList);
            RaisePropertyChangedEvent(nameof(SelectedTourItem));
            RaisePropertyChangedEvent(nameof(TourItemsList));
        }

        private void PrintSpecificTourReport(object commandParameter)
        {
            _tourItemFactory.PrintSpecificTourReport(_selectedTourItem);
        }

        private void PrintSumerizeTourReport(object commandParameter)
        {
            _tourItemFactory.PrintSumerizeTourReport(TourItemsList);
        }

    }
}