using System;
using Xamarin.Forms;

namespace GigaHitz.ViewModel
{
    public class CustomStepper : Stepper
    {
        public CustomStepper(LayoutOptions horizontal, double min = 0, double max = 1, double val = 0, double increment = 1)
        {
            HorizontalOptions = horizontal;
            Minimum = min;
            Maximum = max;
            Value = val;
            Increment = increment;
        }

        public void SetWH(double width = 100, double height = 50)
        {
            WidthRequest = width;
            HeightRequest = height;
        }
    }
}
