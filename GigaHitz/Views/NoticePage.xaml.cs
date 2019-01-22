using System;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GigaHitz.Views
{
    public partial class NoticePage : ContentPage
    {
        public ObservableCollection<ImgSource> ImgUrl { get; set; }

        public NoticePage()
        {
            InitializeComponent();

            ImgUrl = new ObservableCollection<ImgSource>();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            Load();
        }

        void Load()
        {
            //get url at firebase
            string url = "Logo.png";

            for(int i = 0; i < 2; i++)
            {
                ImgUrl.Add(new ImgSource { Source = ImageSource.FromFile(url) });
            }
            CV.ItemsSource = ImgUrl;
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            await Navigation.PopAsync(false);
        }

        async void Btn_NoticeContent(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new NoticeContent.NoticeContentPage(), false);
        }
    }

    public class ImgSource
    {
        public ImageSource Source { get; set; }
    }
}
