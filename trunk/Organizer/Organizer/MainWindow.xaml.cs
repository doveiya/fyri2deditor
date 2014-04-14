using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;

namespace Organizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int currentImageIndex = 0;
        int currentFolderIndex = 0;
        bool isDirty = false;
        //Point lastMouseDown;
        Image draggedImage;
        myTreeViewItem targetTVI;
        string alphaIndex = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        string lastSelectedPath = string.Empty;

        OrganizerInfo myOI = new OrganizerInfo();

        List<FileInfo>      ImageList   = new List<FileInfo>();
        List<DirectoryInfo> FoldersList = new List<DirectoryInfo>();

        public MainWindow()
        {
            InitializeComponent();

            var userPrefs = new UserPreferences();

            this.Height = userPrefs.WindowHeight;
            this.Width = userPrefs.WindowWidth;
            this.Top = userPrefs.WindowTop;
            this.Left = userPrefs.WindowLeft;
            this.WindowState = userPrefs.WindowState;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Avalon.Windows.Dialogs.FolderBrowserDialog dialog = new Avalon.Windows.Dialogs.FolderBrowserDialog
            {
                BrowseFiles = false,
                //ShowEditBox = ShowEditBox.IsChecked == true,
                //BrowseShares = BrowseShares.IsChecked == true
            };
            if (dialog.ShowDialog() == true)
            {
                isDirty = true;
                LoadImageList(dialog.SelectedPath);
            }
        }

        private void LoadImageList(string folderPath)
        {
            ImageList.Clear();
            CurrentImageBox.Source = null;

            CurrentFolderTB.Text = folderPath;
            if (myOI.ImageFolderName != folderPath)
                myOI.ImageFolderName = folderPath;

            DirectoryInfo folderDI = new DirectoryInfo(folderPath);

            FileInfo[] allFiles = folderDI.GetFiles();

            foreach (FileInfo file in allFiles)
            {
                if (file.Extension.ToLower() == ".png" || file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".gif" || file.Extension.ToLower() == ".jpeg" || file.Extension.ToLower() == ".bmp" || file.Extension.ToLower() == ".tif")
                {
                    ImageList.Add(file);
                }
            }

            if (ImageList.Count > 0)
            {
                this.Title = "Organizer - " + ImageList.Count.ToString();

                BitmapImage bitmap = LoadImage();

                if (bitmap != null)
                    CurrentImageBox.Source = bitmap;
            }
            else
                this.Title = "Organizer"; 
        }

        private BitmapImage LoadImage()
        {
            BitmapImage bitmap = null;
            bool finishedLoading = false;
            //ImageSource imageSource = new BitmapImage(new Uri(ImageList[0].FullName));
            while (!finishedLoading)
            {
                if (currentImageIndex >= 0 && currentImageIndex < ImageList.Count)
                {
                    try
                    {
                        bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(ImageList[currentImageIndex].FullName);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        finishedLoading = true;

                        imageTitle.Text = ImageList[currentImageIndex].Name;
                    }
                    catch
                    {
                    }

                    if (!finishedLoading)
                    {
                        bitmap = null;
                        ImageList.RemoveAt(currentImageIndex);
                    }
                }
                else
                {
                    finishedLoading = true;
                    bitmap = null;
                }
            }

            return bitmap;
        }

        private void Button_AddFolder(object sender, RoutedEventArgs e)
        {
            Avalon.Windows.Dialogs.FolderBrowserDialog dialog = new Avalon.Windows.Dialogs.FolderBrowserDialog
            {
                BrowseFiles = false,
                //ShowEditBox = ShowEditBox.IsChecked == true,
                //BrowseShares = BrowseShares.IsChecked == true
            };

            if (lastSelectedPath != string.Empty)
                dialog.SelectedPath = lastSelectedPath;

            if (dialog.ShowDialog() == true)
            {
                if (!myOI.SortFolders.Contains(dialog.SelectedPath))
                {
                    isDirty = true;

                    DirectoryInfo folderDI = new DirectoryInfo(dialog.SelectedPath);

                    lastSelectedPath = folderDI.Parent.FullName;

                    myOI.SortFolders.Add(folderDI.FullName);

                    TreeViewItem rootItem = FoldersTVI as TreeViewItem;

                    if (rootItem != null)
                    {
                        myTreeViewItem subItem = new myTreeViewItem(folderDI, currentFolderIndex);
                        currentFolderIndex++;
                        rootItem.Items.Add(subItem);
                        rootItem.IsExpanded = true;
                    }
                }
                else
                {
                    MessageBox.Show("Folder Already Exists In List");
                }
            }
        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            TreeViewItem rootItem = FoldersTVI as TreeViewItem;

            if (rootItem != null)
            {
                rootItem.Items.Clear();
                myOI.SortFolders.Clear();
                isDirty = true;
                currentFolderIndex = 0;
            }
        }

        private void CurrentImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;

            if (image != null && e.LeftButton == MouseButtonState.Pressed)
            {
                draggedImage = image;
                DataObject data = new DataObject(typeof(ImageSource), image.Source);

                DragDropEffects finalDropEffect = 
                    DragDrop.DoDragDrop(
                        image,
                        data,
                        DragDropEffects.Move);

                //Checking target is not null and item is 
                //dragging(moving)
                if ((finalDropEffect == DragDropEffects.Move) && (targetTVI != null))
                {
                    string uri = (image.Source as BitmapImage).UriSource.LocalPath;

                    FileInfo file = new FileInfo(uri);
                        
                    // A Move drop was accepted
                    if (!file.DirectoryName.Equals(targetTVI.DirectoryInfo.FullName))
                    {
                        MoveFile(file, targetTVI.DirectoryInfo.FullName);
                    }
                    else
                    {
                        MessageBox.Show("File Already Exists");
                    }
                        //MoveItem(file.FullName, targetTVI.DirectoryInfo.FullName);
                    //}
                }
            }
        }

        private void MoveFile(FileInfo file, string targetDirectory)
        {
            string targetFileName = targetDirectory + "\\" + file.Name;
            FileInfo targetFile = new FileInfo(targetFileName);

            if (file.Exists && !targetFile.Exists)
            {
                CurrentImageBox.Source = null;

                try
                {
                    File.Move(file.FullName, targetFileName);
                }
                catch
                {
                    MessageBox.Show("Error Moving File", "Error!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    targetTVI = null;
                    draggedImage = null;
                }

                ImageList.RemoveAt(currentImageIndex);
                //ImageSource imageSource = new BitmapImage(new Uri(ImageList[currentImageIndex].FullName));

                if (currentImageIndex >= ImageList.Count)
                    currentImageIndex = ImageList.Count - 1;


                BitmapImage bitmap = LoadImage();

                if (bitmap != null)
                    CurrentImageBox.Source = bitmap;

                if (ImageList.Count > 0)
                {
                    this.Title = "Organizer - " + ImageList.Count.ToString();
                }
                else
                    this.Title = "Organizer";
            }
        }

        private void FoldersTreeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                myTreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);

                if (item != null)
                {
                    item.IsSelected = true;
                    string uri = (draggedImage.Source as BitmapImage).UriSource.LocalPath;
                    FileInfo file = new FileInfo(uri);

                    if (CheckDropTarget(file.DirectoryName, item.DirectoryInfo.FullName))
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }

                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }

        private bool CheckDropTarget(string _sourceDir, string _targetDir)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;
            if (!_sourceDir.Equals(_targetDir))
            {
                _isEqual = true;
            }
            return _isEqual;

        }

        private myTreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            myTreeViewItem container = element as myTreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as myTreeViewItem;
            }
            return container;
        }

        private void FoldersTreeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                myTreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && draggedImage != null)
                {
                    targetTVI = TargetItem;
                    e.Effects = DragDropEffects.Move;

                }
            }
            catch (Exception)
            {
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.FileName = "ChangeMe";
            dlg.DefaultExt = ".opf";
            dlg.Filter = "Organizer Project Files (.opf)|*.opf";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                SerializeObject(dlg.FileName, myOI);
                isDirty = false;
            }
        }

        private void SerializeObject(string filename, OrganizerInfo objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            //dlg.FileName = "ChangeMe";
            dlg.DefaultExt = ".opf";
            dlg.Filter = "Organizer Project Files (.opf)|*.opf";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                myOI = DeSerializeObject(dlg.FileName);

                LoadImageList(myOI.ImageFolderName);

                TreeViewItem rootItem = FoldersTVI as TreeViewItem;

                if (rootItem != null)
                {
                    rootItem.Items.Clear();
                    currentFolderIndex = 0;

                    foreach (string fullPath in myOI.SortFolders)
                    {
                        DirectoryInfo dir = new DirectoryInfo(fullPath);
                        myTreeViewItem subItem = new myTreeViewItem(dir, currentFolderIndex);
                        currentFolderIndex++;
                        rootItem.Items.Add(subItem);
                        rootItem.IsExpanded = true;
                    }
                }
            }
        }

        public OrganizerInfo DeSerializeObject(string filename)
        {
            OrganizerInfo objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (OrganizerInfo)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (isDirty)
            {
                MessageBoxResult result = 
                    MessageBox.Show("Do you want to save your project?", "Project has Changed!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    Save_Click(sender, e);
            }

            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isDirty)
            {
                MessageBoxResult result =
                    MessageBox.Show("Do you want to save your project?", "Project has Changed!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    Save_Click(sender, null);
            }

            var userPrefs = new UserPreferences();

            userPrefs.WindowHeight = this.Height;
            userPrefs.WindowWidth = this.Width;
            userPrefs.WindowTop = this.Top;
            userPrefs.WindowLeft = this.Left;
            userPrefs.WindowState = this.WindowState;

            userPrefs.Save();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            currentImageIndex++;
            if (currentImageIndex > ImageList.Count - 1)
                currentImageIndex = 0;

            BitmapImage bitmap = LoadImage();

            if (bitmap != null)
                CurrentImageBox.Source = bitmap;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            currentImageIndex--;
            if (currentImageIndex < 0)
                currentImageIndex = ImageList.Count - 1;

            BitmapImage bitmap = LoadImage();

            if (bitmap != null)
                CurrentImageBox.Source = bitmap;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int index = 0;
            bool changeImage = false;
            string keyCodeString = e.Key.ToString();
            char? key = null;
            if (keyCodeString.Length == 1)
            {
                key = keyCodeString[0];
            }
            else
            {
                if (keyCodeString.StartsWith("NumPad"))
                {
                key = keyCodeString[keyCodeString.Length - 1];
                }
            }

            if(key != null)
            {
                for (int i = 0; i < alphaIndex.Length; i++)
                {

                    if (alphaIndex[i].Equals(key))
                    {
                        index = i;
                        changeImage = true;
                        break;
                    }
                }
            }

            

            //switch (e.Key)
            //{
            //    case Key.NumPad0:
            //        changeImage = true;
            //        break;
            //    case Key.NumPad1:
            //        index = 1;
            //        changeImage = true;
            //        break;
            //    case Key.NumPad2:
            //        index = 2;
            //        changeImage = true;
            //        break;
            //    case Key.NumPad3:
            //        index = 3;
            //        changeImage = true;
            //        break;
            //    case Key.NumPad4:
            //        index = 4;
            //        changeImage = true;
            //        break;
            //    case Key.NumPad5:
            //        index = 5;
            //        changeImage = true;
            //        break;
            //    case Key.NumPad6:
            //        index = 6;
            //        changeImage = true;
            //        break;
            //    case Key.NumPad7:
            //        index = 7;
            //        changeImage = true;
            //        break;
            //    case Key.NumPad8:
            //        index = 8;
            //        changeImage = true;
            //        break;
            //    case Key.NumPad9:
            //        index = 9;
            //        changeImage = true;
            //        break;
            //    //case Key.Right:
            //    //    Next_Click(sender, null);
            //    //    break;
            //    //case Key.Left:
            //    //    Previous_Click(sender, null);
            //    //    break;
            //}

            if (changeImage)
            {
                string uri = (CurrentImageBox.Source as BitmapImage).UriSource.LocalPath;

                FileInfo file = new FileInfo(uri);

                MoveFile(file, myOI.SortFolders[index]);
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if(FoldersTreeView.SelectedItem == null || FoldersTreeView.SelectedItem == FoldersTVI)
                return;

            myTreeViewItem tvi = FoldersTreeView.SelectedItem as myTreeViewItem;

            if (tvi.Index > 0)
            {
                isDirty = true;
                int currentIndex = tvi.Index;

                myTreeViewItem newTVI = new myTreeViewItem(tvi.DirectoryInfo, currentIndex - 1);

                //tvi.Index = currentIndex - 1;
                //tvi.Header = tvi.Index + " - " + tvi.DirectoryInfo.Name;

                FoldersTVI.Items.RemoveAt(tvi.Index);
                FoldersTVI.Items.Insert(currentIndex - 1, newTVI);

                myTreeViewItem movedItem = FoldersTVI.Items[currentIndex] as myTreeViewItem;

                movedItem.Index = currentIndex;
                movedItem.Header = movedItem.Index + " - " + movedItem.DirectoryInfo.Name;

                newTVI.IsSelected = true;

                myOI.SortFolders[currentIndex] = movedItem.DirectoryInfo.FullName;
                myOI.SortFolders[currentIndex - 1] = newTVI.DirectoryInfo.FullName;  
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (FoldersTreeView.SelectedItem == null || FoldersTreeView.SelectedItem == FoldersTVI)
                return;

            myTreeViewItem tvi = FoldersTreeView.SelectedItem as myTreeViewItem;

            if (tvi.Index < FoldersTVI.Items.Count - 1)
            {
                isDirty = true;

                int currentIndex = tvi.Index;

                myTreeViewItem newTVI = new myTreeViewItem(tvi.DirectoryInfo, currentIndex + 1);

                //tvi.Index = currentIndex - 1;
                //tvi.Header = tvi.Index + " - " + tvi.DirectoryInfo.Name;

                FoldersTVI.Items.RemoveAt(tvi.Index);

                if (currentIndex + 1 == FoldersTVI.Items.Count)
                    FoldersTVI.Items.Add(newTVI);
                else
                    FoldersTVI.Items.Insert(currentIndex + 1, newTVI);

                myTreeViewItem movedItem = FoldersTVI.Items[currentIndex] as myTreeViewItem;

                movedItem.Index = currentIndex;
                movedItem.Header = movedItem.Index + " - " + movedItem.DirectoryInfo.Name;

                newTVI.IsSelected = true;

                myOI.SortFolders[currentIndex] = movedItem.DirectoryInfo.FullName;
                myOI.SortFolders[currentIndex + 1] = newTVI.DirectoryInfo.FullName; 
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                Next_Click(sender, null);
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                Previous_Click(sender, null);
                e.Handled = true;
            }

            if (!CurrentImageBox.IsFocused)
                CurrentImageBox.Focus();
        }
    }

    class myTreeViewItem : TreeViewItem
    {
        public DirectoryInfo DirectoryInfo;
        public int Index;

        private string alphaIndex = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public myTreeViewItem(DirectoryInfo di, int index)
        {
            Index = index;
            DirectoryInfo = di;

            if (index >= 0 && index < alphaIndex.Length)
                this.Header = alphaIndex[index] + " - " + di.Name;
            else
                this.Header = di.Name;
        }
    }

    [Serializable()]
    public class OrganizerInfo : ISerializable
    {
        public string ImageFolderName;

        public List<string> SortFolders = new List<string>();

        public OrganizerInfo()
        {
            ImageFolderName = string.Empty;
        }

        public OrganizerInfo(SerializationInfo info, StreamingContext ctxt)
        {
            this.ImageFolderName = info.GetString("ImageFolderName");
            this.SortFolders = (List<string>)info.GetValue("SortFolders", typeof(List<string>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ImageFolderName", this.ImageFolderName);
            info.AddValue("SortFolders", this.SortFolders);
        }
    }
}
