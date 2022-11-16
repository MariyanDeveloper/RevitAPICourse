using Autodesk.Revit.DB;

namespace CoreRevitLibrary.Extensions
{
    public static class WallExtensions
    {
        public static bool IsCurtain(this Wall wall) => wall.WallType.Kind == WallKind.Curtain;
    }
}
