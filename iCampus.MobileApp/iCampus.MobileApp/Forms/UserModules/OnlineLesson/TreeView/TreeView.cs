using System.Collections.ObjectModel;
using System.Collections.Specialized;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.OnlineLesson.TreeView;

public class TreeView : ScrollView
    {
        #region Fields
        private readonly StackLayout _StackLayout = new StackLayout { Orientation = StackOrientation.Vertical };

        //TODO: This initialises the list, but there is nothing listening to INotifyCollectionChanged so no nodes will get rendered
        private IList<TreeViewNode> _RootNodes = new ObservableCollection<TreeViewNode>();
        private TreeViewNode _SelectedItem;
        #endregion

        #region Public Properties

        /// <summary>
        /// The item that is selected in the tree
        /// TODO: Make this two way - and maybe eventually a bindable property
        /// </summary>
        public TreeViewNode SelectedItem
        {
            get => _SelectedItem;

            set
            {
                if (_SelectedItem == value)
                {
                    return;
                }

                if (_SelectedItem != null)
                {
                    _SelectedItem.IsSelected = false;
                }

                _SelectedItem = value;

                SelectedItemChanged?.Invoke(this, new EventArgs());
            }
        }


        public IList<TreeViewNode> RootNodes
        {
            get => _RootNodes;
            set
            {
                _RootNodes = value;

                //if (value is INotifyCollectionChanged notifyCollectionChanged)
                //{
                //    notifyCollectionChanged.CollectionChanged += (s, e) =>
                //    {
                //        RenderNodes(_RootNodes, _StackLayout, e, null);
                //    };
                //}

                RenderNodes(_RootNodes, _StackLayout, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), null);
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Occurs when the user selects a TreeViewItem
        /// </summary>
        public event EventHandler SelectedItemChanged;


        #endregion

        #region Constructor
        public TreeView()
        {
            Content = _StackLayout;
        }
        #endregion

        #region Private Methods
        public void ClearTree()
        {
            _StackLayout.Children.Clear();
        }
        private void RemoveSelectionRecursive(IEnumerable<TreeViewNode> nodes)
        {
            try
            {

                foreach (var treeViewItem in nodes)
                {
                    if (treeViewItem != SelectedItem)
                    {
                        treeViewItem.IsSelected = false;
                    }

                    RemoveSelectionRecursive(treeViewItem.Children);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }
        #endregion

        #region Private Static Methods
        private static void AddItems(IEnumerable<TreeViewNode> childTreeViewItems, StackLayout parent, TreeViewNode parentTreeViewItem)
        {
            try
            {
                foreach (var childTreeNode in childTreeViewItems)
                {
                    if (!parent.Children.Contains(childTreeNode))
                    {
                        parent.Children.Add(childTreeNode);
                    }
                    childTreeNode.ParentTreeViewItem = parentTreeViewItem;
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// TODO: A bit stinky but better than bubbling an event up...
        /// </summary>
        internal void ChildSelected(TreeViewNode child)
        {
            try
            {
                SelectedItem = child;
                child.IsSelected = true;
                child.SelectionBoxView.Color = child.SelectedBackgroundColor;
                child.SelectionBoxView.Opacity = child.SelectedBackgroundOpacity;
                RemoveSelectionRecursive(RootNodes);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }
        #endregion
        #region Internal Static Methods
        internal static void RenderNodes(IEnumerable<TreeViewNode> childTreeViewItems, StackLayout parent, NotifyCollectionChangedEventArgs e, TreeViewNode parentTreeViewItem)
        {
            try
            {
                if (e.Action != NotifyCollectionChangedAction.Add)
                {
                    //TODO: Reintate this...
                    //parent.Children.Clear();
                    AddItems(childTreeViewItems, parent, parentTreeViewItem);
                }
                else
                {
                    AddItems(e.NewItems.Cast<TreeViewNode>(), parent, parentTreeViewItem);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }
        #endregion

        public TreeViewNode CreateTreeViewNode(object bindingContext, string labelText, string path, FileTreeTypes fileType, bool isItem)
        {
            var label = new CustomLabel
            {
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.Gray,
                Text = labelText,
                FullPath = path,
                FontSize = 16,
                Margin = new Thickness(5, 5, 0, 5),
                FileType = fileType

            };

            var node = new TreeViewNode
            {
                BindingContext = bindingContext,
                BackgroundColor = Colors.Transparent,
                Content = new StackLayout
                {
                    Children =
                        {
                            label
                        },
                    BackgroundColor = Colors.Transparent,
                    Orientation = StackOrientation.Horizontal,
                    MinimumHeightRequest = 5
                }
            };

            try
            {
                //set DataTemplate for expand button content

                var currentNode = bindingContext as FileTreeItem;
                if (currentNode != null)
                {
                    if (currentNode.Children != null)
                    {
                        foreach (var c in currentNode.Children)
                        {
                            node.Children.Add(CreateTreeViewNode(c, c.FileName, c.FullPath, c.FileType, true));
                        }
                        node.ExpandButtonTemplate = new DataTemplate(() => new ExpandButtonContent { BindingContext = node });
                    }
                }
                return node;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
                return node;
            }
        }



    }
    public class CustomLabel : Label
    {
        public string FullPath { get; set; }
        public FileTreeTypes FileType { get; set; }
    }

    public class ExpandButtonContent : ContentView
    {

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var node = (BindingContext as TreeViewNode);
            bool isLeafNode = (node.Children == null || node.Children.Count == 0);

            //empty nodes have no icon to expand unless showExpandButtonIfEmpty is et to true which will show the expand
            //icon can click and populated node on demand propably using the expand event.
            if ((isLeafNode) && !node.ShowExpandButtonIfEmpty)
            {
                Content = new Image
                {
                    Source = new FileImageSource
                    {
                        File = isLeafNode ? "file.png" : " ",
                    },
                    BackgroundColor = Colors.Transparent,
                    HeightRequest = 15,
                    WidthRequest = 15,
                };
            }
            else
            {
                Content = new Image
                {
                    Source = new FileImageSource
                    {
                        File = node.IsExpanded ? "folderopen.png" : "folder.png"
                    },
                    BackgroundColor = Colors.Transparent,
                    HeightRequest = 15,
                    WidthRequest = 15
                };
            }
        }

    }