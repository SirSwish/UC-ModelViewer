using System.Collections.Generic;

namespace UC_ModelViewer.MVVM.Model
{
    //Class provides a mapping for various Prim properties
    public class PrimMaps
    {
        private static Dictionary<int, string> collisionTypeMap = new Dictionary<int, string>
    {
        { 0, "Normal" },
        { 1, "None" },
        { 2, "Tree" },
        { 3, "Reduced" }
    };
        private static Dictionary<int, string> shadowTypeMap = new Dictionary<int, string>
    {
        { 0, "None" },
        { 1, "Box Edge" },
        { 2, "Cylinder" },
        { 3, "Four Legs" },
        { 4, "Full Box" },
    };
        public static string GetCollisionTypeDescription(int collisionType)
        {
            if (collisionTypeMap.TryGetValue(collisionType, out var description))
            {
                return description;
            }
            else
            {
                return "Unknown";
            }
        }
        public static string GetShadowTypeDescription(int shadowType)
        {
            if (shadowTypeMap.TryGetValue(shadowType, out var description))
            {
                return description;
            }
            else
            {
                return "Unknown";
            }
        }
    }
}
