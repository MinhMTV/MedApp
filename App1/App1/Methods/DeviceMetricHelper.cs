using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Essentials;

using Xamarin.Forms;

namespace CBMTraining.Methods
{
	public class DeviceMetricHelper : ContentPage
	{
		public DeviceMetricHelper ()
		{
		}

		// Get Metrics
		public DisplayInfo getMainDisplayInfo()
        {
			return DeviceDisplay.MainDisplayInfo;
		}

		public DisplayOrientation GetOrientation()
        {
			var info = getMainDisplayInfo();
			return info.Orientation;
		}
		// Rotation (0, 90, 180, 270)
		public DisplayRotation GetRotation()
        {
			var info = getMainDisplayInfo();
			return info.Rotation;
		}
		// Width (in pixels)
		public double getWidth()
        {
			var info = getMainDisplayInfo();
			return info.Width;
		}

		// Width (in xamarin.forms units) correct width within application
		public double getWidthXamarin()
		{
			var info = getMainDisplayInfo();
			return  getWidth() / info.Density;
		}
		// Heigth (in pixels)
		public double getHeight()
		{
			var info = getMainDisplayInfo();
			return info.Height;
		}

		// Heigth (in xamarin.forms units) correct width within application
		public double getHeightXamarin()
		{
			var info = getMainDisplayInfo();
			return getHeight() / info.Density;
		}

		// get Screen Density
		public double getDensity()
        {
			var info = getMainDisplayInfo();
			return info.Density;
		}
	}
}