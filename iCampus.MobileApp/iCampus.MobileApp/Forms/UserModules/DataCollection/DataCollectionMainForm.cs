using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.DataCollection;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.DataCollection;

public class DataCollectionMainForm : ViewModelBase
    {
        #region Declarations
        public ICommand SubmitClickCommand { get; set; }
        public ICommand EditClickCommand { get; set; }
        public ICommand RefreshedCommand { get; set; }
        #endregion
        #region Properties
        ObservableCollection<ParentsDataCollectionView> _parentDataCollectionList;
        public ObservableCollection<ParentsDataCollectionView> ParentDataCollectionList
        {
            get => _parentDataCollectionList;
            set
            {
                _parentDataCollectionList = value;
                OnPropertyChanged(nameof(ParentDataCollectionList));
            }        
        }

        ParentsDataCollectionView _selectedData;
        public ParentsDataCollectionView SelectedData
        {
            get => _selectedData;
            set
            {
                _selectedData = value;
                OnPropertyChanged(nameof(SelectedData));
            }        
        }
        int _listViewHeight;
        public int ListViewHeight
        {
            get => _listViewHeight;
            set
            {
                _listViewHeight = value;
                OnPropertyChanged(nameof(ListViewHeight));
            }        
        }

        bool _noDataExist;
        public bool NoDataExist
        {
            get => _noDataExist;
            set
            {
                _noDataExist = value;
                OnPropertyChanged(nameof(NoDataExist));
            }        
        }
        bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }        
        }
        #endregion
        public DataCollectionMainForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            SubmitClickCommand = new Command(SubmitClicked);
            EditClickCommand = new Command(EditClicked);
            RefreshedCommand = new Command(RefreshDataCollectionList);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            GetAllDataCollection();
        }

        #region Methods
        public async void GetAllDataCollection()
        {
            try
            {
                ParentDataCollectionList = new ObservableCollection<ParentsDataCollectionView>();
                var data = await ApiHelper.GetObject<ParentsDataCollectionViewModel>(string.Format(TextResource.GetAllDataCollectionApiUrl,null,null));

                if (data != null && data.ParentsDataCollection.Count > 0)
                    ParentDataCollectionList = new ObservableCollection<ParentsDataCollectionView>(data.ParentsDataCollection);
                ListViewHeight = ParentDataCollectionList.Count * 100;
                NoDataExist = !ParentDataCollectionList.Any();
                foreach (var parentData in ParentDataCollectionList)
                {
                    parentData.StudentName = data.FamilyStudentList.Where(x => x.ItemId == parentData.StudentId.ToString()).FirstOrDefault()?.ItemName;
                }                
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }

        private void EditClicked(object obj)
        {
            try
            {
                if (obj != null)
                {
                    SelectedData = (ParentsDataCollectionView)obj;
                    GetDataCollectionFormData(SelectedData);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }

        private void SubmitClicked(object obj)
        {
            try
            {
                if (obj != null)
                {
                    SelectedData = (ParentsDataCollectionView)obj;
                    GetDataCollectionFormData(SelectedData);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }

        public async void GetDataCollectionFormData(ParentsDataCollectionView SelectedData)
        {
            try
            {
                if (SelectedData != null)
                {
                    var data = await ApiHelper.GetObject<DataCollectionView>(string.Format(TextResource.GetDataCollectionApiUrl, SelectedData.FormId, SelectedData.StudentId));
                    if (data != null && data.DataCollectionFormFields.Any())
                    {
                        ShowDataCollection(data);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }
        public async void ShowDataCollection(DataCollectionView dataCollectionView)
        {
            if (dataCollectionView != null && dataCollectionView.DataCollectionFormFields != null && dataCollectionView.DataCollectionFormFields.Count() > 0)
            {
                DataCollectionForm dataCollectionForm = new (_mapper, _nativeServices, Navigation);
                dataCollectionView.ActiveDataCollectionForm = dataCollectionView.DataCollectionForms.FirstOrDefault();
                dataCollectionForm.DataCollection = dataCollectionView;
                dataCollectionForm.PageTitle = TextResource.DataCollectionPageTitle;
                dataCollectionForm.MenuVisible = true;
                dataCollectionForm.IsFromMenu = true;
                dataCollectionForm.BackVisible = true;
                dataCollectionForm.AssignFormData(dataCollectionView.ActiveDataCollectionForm, dataCollectionView.DataCollectionFormFields);

                DataCollectionPage dataCollectionPage = new ()
                {
                    BindingContext = dataCollectionForm
                };
                await Navigation.PushAsync(dataCollectionPage);
            }
        }
        private async void RefreshDataCollectionList()
        {
            IsRefreshing = true;
            GetAllDataCollection();
            IsRefreshing = false;
        }
        #endregion
    }