using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1.DependencyServices
{
    public interface IImageResizerWin
    {
        Task<byte[]> ResizeImageWindows(byte[] imageData, float width, float height);
    }
}
