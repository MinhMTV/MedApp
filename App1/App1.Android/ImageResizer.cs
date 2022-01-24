using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App1.DependencyServices;
using Android.Graphics;
using System.IO;
using App1.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ImageResizer))]
namespace App1.Droid
{
	public class ImageResizer : IImageResizer
	{
		public byte[] ResizeImage(byte[] imageData, float width, float height)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
				return ms.ToArray();
			}
		}
	}
}