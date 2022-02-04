﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.AdminPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoDetails : ContentPage
    {
        public PhotoDetails(byte[] image)
        {
            InitializeComponent();
            ImageDetail.Source = ImageSource.FromStream(() =>
            {
                return new MemoryStream(image);
            });
        }
    }
}