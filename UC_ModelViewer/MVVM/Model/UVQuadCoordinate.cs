using System.Collections.Generic;

namespace UC_ModelViewer.MVVM.Model
{
    public class UVQuadCoordinates
    {
        public List<double> UV { get; set; }

        public UVQuadCoordinates(List<double> uv)
        {
            UV = uv;
        }
    }
}
