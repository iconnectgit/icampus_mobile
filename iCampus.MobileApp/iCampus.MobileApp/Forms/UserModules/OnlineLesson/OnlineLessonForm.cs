using System.Collections.ObjectModel;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.OnlineLesson.TreeView;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;
using System.Linq;

namespace iCampus.MobileApp.Forms.UserModules.OnlineLesson;

public class OnlineLessonForm : ViewModelBase
    {
        #region Declarations
        StudentLessonsView _studentLessons = new StudentLessonsView();
        #endregion
        #region Properties
        private FileTreeItem _fileTree;
        public FileTreeItem FileTree
        {
            get => _fileTree;
            set
            {
                _fileTree = value;
                OnPropertyChanged(nameof(FileTree));
            }
        }
        public StudentLessonsView StudentLessons
        {
            get => _studentLessons;
            set
            {
                _studentLessons = value;
                OnPropertyChanged(nameof(StudentLessons));
            }
        }

        OnlineLessonsModel _onlineLessons;
        public OnlineLessonsModel OnlineLessons
        {
            get => _onlineLessons;
            set
            {
                _onlineLessons = value;
                OnPropertyChanged(nameof(OnlineLessons));
            }
        }
        ObservableCollection<TreeViewNode> _treeViewNodesElective = new ObservableCollection<TreeViewNode>();
        public ObservableCollection<TreeViewNode> TreeViewNodesElective
        {
            get => _treeViewNodesElective;
            set
            {
                _treeViewNodesElective = value;
                OnPropertyChanged(nameof(TreeViewNodesElective));
            }
        }

        bool _isFileAvailable;
        public bool IsFileAvailable
        {
            get => _isFileAvailable;
            set
            {
                _isFileAvailable = value;
                OnPropertyChanged(nameof(IsFileAvailable));
            }
        }
        #endregion
        public OnlineLessonForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Private Methods
        private async void InitializePage()
        {
            IsFileAvailable = false;
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        }


        private async Task<OnlineLessonsModel> DisplayStudentLesson()
        {
            try
            {
                var studentId = AppSettings.Current.SelectedStudent.ItemId;
                OnlineLessons = await ApiHelper.GetObject<OnlineLessonsModel>(string.Format(TextResource.OnlineLessonApiUrl, studentId), cacheKeyPrefix:"onlinelesson", cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                await ApiHelper.ShowProcessingIndicatorPopup();
                if (OnlineLessons!=null)
                {
                    IsFileAvailable = true;
                    var treeView = new TreeView.TreeView();
                    var resultReq = OnlineLessons.AllStudentLessons.FirstOrDefault(x => x.StudentId.ToString() == studentId).RequiredCourses.Select(x => treeView.CreateTreeViewNode(x, x.FileName, x.FullPath, x.FileType, true));
                    var resultElec = OnlineLessons.AllStudentLessons.FirstOrDefault(x => x.StudentId.ToString() == studentId).ElectiveCourses.Select(x => treeView.CreateTreeViewNode(x, x.FileName, x.FullPath, x.FileType, true));
                    
                    MessagingCenter.Send<OnlineLessonForm, Tuple<IEnumerable<TreeViewNode>, IEnumerable<TreeViewNode>>>(this,"StudentChanged", Tuple.Create(resultReq, resultElec));
                    
                }
                await ApiHelper.HideProcessingIndicatorPopup();
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.OnlineLessonPageTitle);
                
            }
            return OnlineLessons;
        }


        public override async void GetStudentData()
        {
            base.GetStudentData();
            await DisplayStudentLesson();
        }


        #endregion
    }