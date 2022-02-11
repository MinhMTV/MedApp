using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBMTraining.DependencyServices
{
    public interface IImageResizerWin
    {
        Task<byte[]> ResizeImageWindows(byte[] imageData, float width, float height);
    }
}
