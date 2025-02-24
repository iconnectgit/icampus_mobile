using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.FinancialStatus;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.FinancialStatus;

public class FinancialStatusForm : ViewModelBase
    {
        #region Declarations
        private bool _isEnableCaching = true;
        public ICommand FinancialCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand HistoryStmtListTappedCommand { get; set; }
        public ICommand FinancilaExpandCollapseClickCommand { get; set; }
        public ICommand HistoryExpandCollapseClickCommand { get; set; }
        #endregion

        #region Properties

        BindableFinancialStatementView _financialStatementData = new BindableFinancialStatementView();
        public BindableFinancialStatementView FinancialStatementData
        {
            get => _financialStatementData;
            set
            {
                _financialStatementData = value;
                OnPropertyChanged(nameof(FinancialStatementData));
            }
        }

        IList<BindableFinancialStatementHistoryView> _statementHistoryList = new List<BindableFinancialStatementHistoryView>();
        public IList<BindableFinancialStatementHistoryView> StatementHistoryList
        {
            get => _statementHistoryList;
            set
            {
                _statementHistoryList = value;
                OnPropertyChanged(nameof(StatementHistoryList));
            }
        }

        BindableFinancialStatementHistoryView _selectedHistoryStmt = new BindableFinancialStatementHistoryView();
        public BindableFinancialStatementHistoryView SelectedHistoryStmt
        {
            get => _selectedHistoryStmt;
            set
            {
                _selectedHistoryStmt = value;
                OnPropertyChanged(nameof(SelectedHistoryStmt));
            }
        }
        bool _isNoStmtHistory;
        public bool IsNoStmtHistory
        {
            get => _isNoStmtHistory;
            set
            {
                _isNoStmtHistory = value;
                OnPropertyChanged(nameof(IsNoStmtHistory));
            }
        }
        bool _isNoFinancialStmt;
        public bool IsNoFinancialStmt
        {
            get => _isNoFinancialStmt;
            set
            {
                _isNoFinancialStmt = value;
                OnPropertyChanged(nameof(IsNoFinancialStmt));
            }
        }
        private decimal _financialButtonOpacity;
        public decimal FinancialButtonOpacity
        {
            get => _financialButtonOpacity;
            set
            {
                _financialButtonOpacity = value;
                OnPropertyChanged(nameof(FinancialButtonOpacity));
            }
        }     
        private decimal _historyButtonOpacity;
        public decimal HistoryButtonOpacity
        {
            get => _historyButtonOpacity;
            set
            {
                _historyButtonOpacity = value;
                OnPropertyChanged(nameof(HistoryButtonOpacity));
            }
        }  
        private bool _noRecordFoundVisibility = false;
        public bool NoRecordFoundVisibility
        {
            get => _noRecordFoundVisibility;
            set
            {
                _noRecordFoundVisibility = value;
                OnPropertyChanged(nameof(NoRecordFoundVisibility));
            }
        }
        private bool _isHistoryVisible;
        public bool IsHistoryVisible
        {
            get => _isHistoryVisible;
            set
            {
                _isHistoryVisible = value;
                OnPropertyChanged(nameof(IsHistoryVisible));
            }
        }
        private bool _isFinancialVisible;
        public bool IsFinancialVisible
        {
            get => _isFinancialVisible;
            set
            {
                _isFinancialVisible = value;
                OnPropertyChanged(nameof(IsFinancialVisible));
            }
        }
        #endregion

        public FinancialStatusForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        #region Methods
        private async void InitializePage()
        {
            HelperMethods.GetSelectedStudent();
            IsFinancialVisible = true;
            IsHistoryVisible = false;
            FinancialButtonOpacity = 1.0m;
            HistoryButtonOpacity = 0.5m;
            FinancialCommand = new Command(FinancialCommandClicked);
            HistoryCommand = new Command(HistoryCommandClicked);
            HistoryStmtListTappedCommand = new Command(HistoryStatementClicked);
            FinancilaExpandCollapseClickCommand = new Command<BindableFinancialStatementView>(FinancialExpandCollapseClicked);
            HistoryExpandCollapseClickCommand = new Command<BindableFinancialStatementHistoryView>(HistoryExpandCollapseClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            await GetFinancialStatementData();
        }
        
        private void FinancialCommandClicked(object obj)
        {
            IsHistoryVisible = false;
            IsFinancialVisible = true;
            IsNoFinancialStmt = !FinancialStatementData.StatementList.Any();
            FinancialButtonOpacity = 1.0m;
            HistoryButtonOpacity = 0.5m;
        }
        private void HistoryCommandClicked(object obj)
        {
            IsHistoryVisible = true;
            IsFinancialVisible = false;
            NoRecordFoundVisibility = false;
            FinancialButtonOpacity = 0.5m;
            HistoryButtonOpacity = 1.0m;
            IsNoStmtHistory = !StatementHistoryList.Any();
        }
        
        public void FinancialExpandCollapseClicked(BindableFinancialStatementView bindableFinancialStatementView)
        {
            if (bindableFinancialStatementView != null)
            {
                foreach (var item in FinancialStatementData.StatementList)
                {
                    if (item != null)
                    {
                        if(item.DocumentNumber == 0)
                        {
                            item.DetailsVisibility = true;
                        }
                        else if (item.DocumentNumber == bindableFinancialStatementView.DocumentNumber && item.UniqueID == bindableFinancialStatementView.UniqueID)
                        {
                            item.DetailsVisibility = !item.DetailsVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                        }
                        else
                        {
                            item.DetailsVisibility = false;
                            item.ArrowImageSource = "dropdown_gray.png";
                        }
                    }
                }
                MessagingCenter.Send("", "FinancialDetailsExpandCollapse");
            }

        }

        public void HistoryExpandCollapseClicked(BindableFinancialStatementHistoryView bindableFinancialStatementHistoryView)
        {
            if (bindableFinancialStatementHistoryView != null)
            {
                foreach (var item in StatementHistoryList)
                {
                    if (item != null)
                    {
                        if (item != null && item.UniqueID == bindableFinancialStatementHistoryView.UniqueID && item.UniqueID == bindableFinancialStatementHistoryView.UniqueID)
                        {
                            item.DetailsVisibility = !item.DetailsVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                        }
                        else
                        {
                            item.DetailsVisibility = false;
                            item.ArrowImageSource = "dropdown_gray.png";
                        }
                    }
                }
                MessagingCenter.Send("", "FinancialDetailsExpandCollapse");
            }
        }

        private async Task<FinancialDataView> GetFinancialStatementData()
        {
            try
            {
                FinancialDataView financialDataView = await ApiHelper.GetObject<FinancialDataView>(TextResource.FinancialStatementApiUrl, _isEnableCaching,attachStudentIdIfParent:false);
                FinancialStatementData = _mapper.Map<BindableFinancialStatementView>(financialDataView.FinancialStatement);
                FinancialStatementData.StatementList.Where(x => x.DocumentNumber == 0).FirstOrDefault().DetailsVisibility = true;
                StatementHistoryList = _mapper.Map<List<BindableFinancialStatementHistoryView>> (financialDataView.StatementHistoryList);
                IsNoFinancialStmt = FinancialStatementData.StatementList == null || FinancialStatementData.StatementList.Count() == 0;
                IsNoStmtHistory = StatementHistoryList == null || StatementHistoryList.Count() == 0;
                return financialDataView;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                return new FinancialDataView();
            }
        }

        private async void HistoryStatementClicked(object obj)
        {
            if (obj != null)
            {
                StatementHistoryDetailsForm statementHistoryDetailsForm = new(_mapper, _nativeServices, Navigation)
                {
                    SelectedHistoryStmt = (BindableFinancialStatementHistoryView)obj,
                    BackVisible = true,
                    PageTitle = TextResource.FinancialStatementText
                };
                StatementHistoryDetails statementHistoryDetails = new ()
                {
                    BindingContext = statementHistoryDetailsForm
                };
                await Navigation.PushAsync(statementHistoryDetails);
                SelectedHistoryStmt = null;
            }
        }
        #endregion
    }