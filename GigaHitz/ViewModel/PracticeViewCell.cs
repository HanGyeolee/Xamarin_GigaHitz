using Xamarin.Forms;

namespace GigaHitz.ViewModel
{
    public class PracticeViewCell : ViewCell
    {
        Label titleLabel, timeLabel;

        ///*
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(RecordViewCell), "title");
        public static readonly BindableProperty TimeProperty =
            BindableProperty.Create("Time", typeof(string), typeof(RecordViewCell), "00:00");

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        //*/

        public PracticeViewCell()
        {
            //instantiate each of our views
            titleLabel = new Label();
            timeLabel = new Label();
            var blank = new Label();

            var h1 = new StackLayout();
            var v1 = new StackLayout();

            //Set properties for desired design
            h1.Orientation = StackOrientation.Horizontal;
            h1.HorizontalOptions = LayoutOptions.FillAndExpand;
            v1.HorizontalOptions = LayoutOptions.FillAndExpand;
            titleLabel.HorizontalOptions = LayoutOptions.StartAndExpand;
            titleLabel.FontSize = 21;
            timeLabel.HorizontalOptions = LayoutOptions.EndAndExpand;
            blank.WidthRequest = 0;

            //add views to the view hierarchy
            v1.Children.Add(titleLabel);
            v1.Children.Add(timeLabel);
            h1.Children.Add(blank);
            h1.Children.Add(v1);
            h1.Children.Add(blank);

            View = h1;
        }
        ///*
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                titleLabel.Text = Title;
                timeLabel.Text = Time;
            }
        }
        //*/
    }
}
