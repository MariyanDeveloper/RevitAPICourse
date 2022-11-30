using Autodesk.Revit.DB;
using System.Windows;

namespace TestWindowLibrary
{
    /// <summary>
    /// Interaction logic for HotReloadWindowTestFromAssembly.xaml
    /// </summary>
    public partial class HotReloadWindowTestFromAssembly : Window
    {
        private readonly Document _document;

        public HotReloadWindowTestFromAssembly(Document document)
        {
            _document = document;
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            using (Transaction transaction = new Transaction(_document, "Do"))
            {
                transaction.Start();
                var familyInstance = new FilteredElementCollector(_document)
                    .OfClass(typeof(FamilyInstance)).FirstElementId();

                ElementTransformUtils.CopyElement(
                    _document,
                    familyInstance,
                    new XYZ(4, 0, 0));
                //var point = XYZ.Zero;
                //DirectShape directShape = DirectShape.CreateElement(_document, new ElementId(BuiltInCategory.OST_GenericModel));
                //directShape.SetShape(new List<GeometryObject>() { Point.Create(point) });
                //TaskDialog.Show("M", "Point has been generated");
                transaction.Commit();
            }
            //_doAction.Invoke();

        }
    }
}
