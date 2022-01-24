using System;
using System.Collections.Generic;
using System.Text;

namespace App1.DependencyServices
{
    public interface IImageResizer
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}
