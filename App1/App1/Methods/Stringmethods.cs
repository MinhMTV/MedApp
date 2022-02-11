using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace CBMTraining.Methods
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

        /// <summary>
        /// Return TimeSpan object to String in Min:Sec:MS format, return null if object is null
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public string TimeSpanToStringToH(TimeSpan timespan)
        {
            if (timespan == null)
                return "00:00:00";
            else
                return string.Format("{0:hh\\:mm\\:ss}", timespan);
        }

        /// <summary>
        /// Return TimeSpan object to String in Min:Sec:MS format, return null if object is null
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public string TimeSpanToStringToMin(TimeSpan timespan)
        {
            if (timespan == null)
                return "0:0:000";
            else
                return string.Concat(timespan.Minutes, ":", timespan.Seconds, ":", timespan.Milliseconds);
        }

        /// <summary>
        /// Return TimeSpan object to String in Min:Sec:MS format, return null if object is null
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public string TimeSpanToStringToMinExt(TimeSpan timespan)
        {
            if (timespan == null)
                return "00:00:000";
            else
                return string.Format("{0:mm\\:ss\\:fff}", timespan);
        }

        /// <summary>
        /// Return TimeSpan object to String in HH:MM:SS format, return null if object is null
        /// </summary>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public string TimeSpanToStringToHExt(TimeSpan timespan)
        {
            if (timespan == null)
                return "00h:00m:000s";
            else
                return string.Format("{0:hh\\h\\:mm\\m\\:ss}s", timespan);
        }
    }
}