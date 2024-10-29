using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.FinancialStatus;

public class StatementHistoryDetailsForm : ViewModelBase
{

    #region Properties
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
    #endregion
    public StatementHistoryDetailsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
    }
}