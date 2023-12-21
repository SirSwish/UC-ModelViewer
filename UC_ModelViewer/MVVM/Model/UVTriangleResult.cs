using System.Collections.Generic;

namespace UC_ModelViewer.MVVM.Model
{
    public class UVTriangleResult
    {
        public int TextureImgNo { get; set; }
        public List<double> UV0 { get; set; }
        public List<double> UV1 { get; set; }
        public List<double> UV2 { get; set; }
        public List<double> UV3 { get; set; }
    }
}
