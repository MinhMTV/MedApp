﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace App1.Methods
{
    public class Stringmethods : ContentPage
    {
        public Stringmethods()
        {
        }

        public bool isEmpty (string text)
        {
            if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool compareStrNoWhite(string s1, string s2)
        {
            s1 = s1.Replace(" ", String.Empty);
            s2 = s2.Replace(" ", String.Empty);

            return s1.ToLower().Equals(s2.ToLower());

        }
    }
}