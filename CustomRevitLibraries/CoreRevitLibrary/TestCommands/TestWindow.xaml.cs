using System.Collections;
using System.Windows;

namespace CoreRevitLibrary.TestCommands
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow(IEnumerable collection)
        {
            InitializeComponent();
            MyList.ItemsSource = collection;
            //MyList.DisplayMemberPath = "Name";

        }
    }
}
