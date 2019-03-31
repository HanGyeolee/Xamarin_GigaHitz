using System;
using System.ComponentModel;
using Android.Content;
using Android.Widget;
using Android.Views;
using AButton = Android.Widget.Button;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GigaHitz.ViewModel;

[assembly : ExportRenderer(typeof(CustomStepper),typeof(GigaHitz.Droid.StepperRenderer_Android))]
namespace GigaHitz.Droid
{
    public class StepperRenderer_Android : ViewRenderer<Stepper, LinearLayout>
    {
        AButton _downButton;
        AButton _upButton;

        public StepperRenderer_Android(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override LinearLayout CreateNativeControl()
        {
            return new LinearLayout(Context)
            {
                Orientation = Orientation.Horizontal
            };
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                _downButton = new AButton(Context) { Text = "-", Gravity = GravityFlags.Center, Tag = this };
                //Set the MinWidth of Button
                _downButton.SetMinWidth(50);

                _downButton.SetOnClickListener(StepperListener.Instance);

                _upButton = new AButton(Context) { Text = "+", Tag = this };
                _upButton.SetOnClickListener(StepperListener.Instance);
                //Set the MinWidth of Button
                _upButton.SetMinWidth(50);

                if (e.NewElement != null)
                {
                    //Set the Width and Height of the button according to the WidthRequest
                    _downButton.LayoutParameters = new LayoutParams((int)e.NewElement.WidthRequest, LayoutParams.MatchParent);
                    _upButton.LayoutParameters = new LayoutParams((int)e.NewElement.WidthRequest, LayoutParams.MatchParent);
                }

                var layout = CreateNativeControl();

                layout.AddView(_downButton);
                layout.AddView(_upButton);

                SetNativeControl(layout);
            }

            UpdateButtonEnabled();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Minimum":
                    UpdateButtonEnabled();
                    break;
                case "Maximum":
                    UpdateButtonEnabled();
                    break;
                case "Value":
                    UpdateButtonEnabled();
                    break;
                case "IsEnabled":
                    UpdateButtonEnabled();
                    break;
            }
        }

        void UpdateButtonEnabled()
        {
            Stepper view = Element;
            _upButton.Enabled = view.IsEnabled ? view.Value < view.Maximum : view.IsEnabled;
            _downButton.Enabled = view.IsEnabled ? view.Value > view.Minimum : view.IsEnabled;
        }

        class StepperListener : Java.Lang.Object, IOnClickListener
        {
            public static readonly StepperListener Instance = new StepperListener();

            public void OnClick(global::Android.Views.View v)
            {
                var renderer = v.Tag as StepperRenderer_Android;
                if (renderer == null)
                    return;

                Stepper stepper = renderer.Element;
                if (stepper == null)
                    return;

                if (v == renderer._upButton)
                    ((IElementController)stepper).SetValueFromRenderer(Stepper.ValueProperty, stepper.Value + stepper.Increment);
                else if (v == renderer._downButton)
                    ((IElementController)stepper).SetValueFromRenderer(Stepper.ValueProperty, stepper.Value - stepper.Increment);
            }
        }
    }
}
