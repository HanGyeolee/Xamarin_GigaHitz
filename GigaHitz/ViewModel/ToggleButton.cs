﻿using System;
using Xamarin.Forms;

namespace GigaHitz.ViewModel
{
    public class ToggleButton : Button
    {
        private EventHandler e;
        public event EventHandler<ToggledEventArgs> Toggled;

        public static BindableProperty IsToggledProperty =
            BindableProperty.Create("IsToggled", typeof(bool), typeof(ToggleButton), false,
                                    propertyChanged: OnIsToggledChanged);

        public ToggleButton()
        {
            e = (sender, args) => IsToggled ^= true;
            Clicked += e;
        }

        public bool IsToggled
        {
            set { SetValue(IsToggledProperty, value); }
            get { return (bool)GetValue(IsToggledProperty); }
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            VisualStateManager.GoToState(this, "ToggledOff");
        }

        static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ToggleButton toggleButton = (ToggleButton)bindable;
            bool isToggled = (bool)newValue;

            // Fire event
            toggleButton.Toggled?.Invoke(toggleButton, new ToggledEventArgs(isToggled));

            // Set the visual state
            VisualStateManager.GoToState(toggleButton, isToggled ? "ToggledOn" : "ToggledOff");
        }

        ~ToggleButton()
        {
            Clicked -= e;
        }
    }
}
