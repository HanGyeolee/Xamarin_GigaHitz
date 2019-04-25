using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GigaHitz.DataBase;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using GigaHitz.ViewModel;

namespace GigaHitz.Views.RecordContent
{
    public partial class OptionPage : ContentPage
    {
        LocalDB dB;

        public OptionPage()
        {
            int kbps = 256;
            float rate = 44100f;

            InitializeComponent();

            dB = new LocalDB();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            //Stepper 크기 조절을 위해 custom Renderer를 구현 및 실행
            //현 상황에서 안드로이드만 구현 가능함
            if(Device.RuntimePlatform.Equals(Device.Android))
            {
                CustomStepper BitsStep = new CustomStepper(LayoutOptions.End, -1, 8, 6, 1);
                CustomStepper RateStep = new CustomStepper(LayoutOptions.End, -1, 3, 1, 1);
                BitsStep.SetWH();
                RateStep.SetWH();
                BitsStep.ValueChanged += BitsValueChanged;
                RateStep.ValueChanged += RateValueChanged;

                if (dB.IsExist()) // 존재 할 떄
                {
                    kbps = int.Parse(dB.ReadIndexOf(0));
                    //dB.Read(out kbps);
                    BitsStep.Value = SetBitsValue(kbps);
                    rate = float.Parse(dB.ReadIndexOf(1));
                    //dB.Read(out rate);
                    RateStep.Value = SetRateValue(rate);
                }

                BitsLayout.Children.Add(BitsStep);
                RateLayout.Children.Add(RateStep);
            }
            else
            {
                Stepper BitsStep = new Stepper(-1,8,6,1);
                Stepper RateStep = new Stepper(-1,3,1,1);
                BitsStep.HorizontalOptions = LayoutOptions.End;
                RateStep.HorizontalOptions = LayoutOptions.End;
                BitsStep.ValueChanged += BitsValueChanged;
                RateStep.ValueChanged += RateValueChanged;

                if (dB.IsExist()) // 존재 할 떄
                {
                    kbps = int.Parse(dB.ReadIndexOf(0));
                    //TODO
                    BitsStep.Value = SetBitsValue(kbps);
                    rate = float.Parse(dB.ReadIndexOf(1));
                    //TODO
                    RateStep.Value = SetRateValue(rate);
                }

                BitsLayout.Children.Add(BitsStep);
                RateLayout.Children.Add(RateStep);
            }

            Device.BeginInvokeOnMainThread(delegate
            {
                BitsValue.Text = kbps.ToString("#####");
                RateValue.Text = rate.ToString("#####");
            });
        }

        int SetBitsValue(int kbps)
        {
            switch (kbps)
            {
                case 96:
                    return 0;
                case 112:
                    return 1;
                case 128:
                    return 2;
                case 160:
                    return 3;
                case 192:
                    return 4;
                case 224:
                    return 5;
                case 256:
                    return 6;
                case 320:
                    return 7;
                default:
                    return 6;
            }
        }

        int SetRateValue(float rate)
        {
            switch((int)rate)
            {
                case 32000:
                    return 0;
                case 44100:
                    return 1;
                case 48000:
                    return 2;
                default:
                    return 1;
            }
        }

        void BitsValueChanged(object sender, ValueChangedEventArgs s)
        {
            var tmp = sender as Stepper;
            int value = 0;
            if (8 == (int)tmp.Value)
                tmp.Value = 0;
            else if (-1 == (int)tmp.Value)
                tmp.Value = 7;
            switch ((int)tmp.Value)
            {
                case 0:
                    value = 96;
                    break;
                case 1:
                    value = 112;
                    break;
                case 2:
                    value = 128;
                    break;
                case 3:
                    value = 160;
                    break;
                case 4:
                    value = 192;
                    break;
                case 5:
                    value = 224;
                    break;
                case 6:
                    value = 256;
                    break;
                case 7:
                    value = 320;
                    break;
            }

            Device.BeginInvokeOnMainThread(delegate
            {
                BitsValue.Text = value.ToString("#####");
            });
        }

        void RateValueChanged(object sender, ValueChangedEventArgs s)
        {
            var tmp = sender as Stepper;
            int value = 0;
            if (3 == (int)tmp.Value)
                tmp.Value = 0;
            else if (-1 == (int)tmp.Value)
                tmp.Value = 2;
            switch ((int)tmp.Value)
            {
                case 0:
                    value = 32000;
                    break;
                case 1:
                    value = 44100;
                    break;
                case 2:
                    value = 48000;
                    break;
            }

            Device.BeginInvokeOnMainThread(delegate
            {
                RateValue.Text = value.ToString("#####");
            });
        }

        Task<bool> SetDB()
        {
            dB.ClearItem();
            dB.AddItem(int.Parse(BitsValue.Text));            //kbps
            dB.AddItem<float>(int.Parse(RateValue.Text));     //rate
            if (dB.IsExist())
            {
                dB.AddItem(dB.ReadIndexOf(2)); // string
                bool b;
                dB.Read(out b, 3);
                dB.AddItem(b); // boolean
            }
            else
            {
                var s = StaticDatas.CheckNotice();
                if (s.Length > 3)
                    dB.AddItem(s);
                dB.AddItem(true);
            }
            dB.Write();
            return Task.FromResult<bool>(true);
        }

        async void Btn_Home(object sender, EventArgs s)
        {
            await SetDB();
            await Navigation.PopToRootAsync(false);
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            await SetDB();
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            SetDB();
            Navigation.PopAsync(false);
            return true;
        }
    }
}
