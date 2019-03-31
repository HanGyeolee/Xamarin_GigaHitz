using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using GigaHitz.DataBase;

namespace GigaHitz.Views.etcContent
{
    public partial class PianoPage : ContentPage
    {
        EventHandler<ValueChangedEventArgs> handler;

        public PianoPage()
        {
            InitializeComponent();

            double max, height = Xamarin.Forms.Application.Current.MainPage.Height;

            max = 1130 * height / 667;
            max *= 0.5 * height / 667;
            Tile3.HeightRequest = 600 * height / 667;
            Tile2.HeightRequest = 600 * height / 667;

            // 기기 세로 크기에 맞춰 재생
            // 안드로이드는 시스템사운드를 이용
            // 아이폰은 avaudio 그대로 이용aa

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            //TODO Slider 랑 Menu button position 재배열 필수 in iOS
            //안드로이드 에서는 완벽함
            slider.Value = 0.5;
            Device.BeginInvokeOnMainThread(delegate
            {
                Tile.ScrollToAsync(0, max * slider.Value, false);
            });

            handler = (object sender, ValueChangedEventArgs e) =>
            {
                Tile.ScrollToAsync(0, max * (1 - slider.Value), false);
            };

            Load();
        }

        void Load()
        {
            slider.ValueChanged += handler;

            #region Level2
            ///*
            C2.Octave_Do.Pressed += (sender, e) => StaticDatas.PianoSound.Play(0);
            C2.Octave_Do.Released += (sender, e) => StaticDatas.PianoSound.Stop(0);
            C2.Octave_Di.Pressed += (sender, e) => StaticDatas.PianoSound.Play(1);
            C2.Octave_Di.Released += (sender, e) => StaticDatas.PianoSound.Stop(1);
            C2.Octave_Re.Pressed += (sender, e) => StaticDatas.PianoSound.Play(2);
            C2.Octave_Re.Released += (sender, e) => StaticDatas.PianoSound.Stop(2);
            C2.Octave_Ri.Pressed += (sender, e) => StaticDatas.PianoSound.Play(3);
            C2.Octave_Ri.Released += (sender, e) => StaticDatas.PianoSound.Stop(3);
            C2.Octave_Mi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(4);
            C2.Octave_Mi.Released += (sender, e) => StaticDatas.PianoSound.Stop(4);
            C2.Octave_Fa.Pressed += (sender, e) => StaticDatas.PianoSound.Play(5);
            C2.Octave_Fa.Released += (sender, e) => StaticDatas.PianoSound.Stop(5);
            C2.Octave_Fi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(6);
            C2.Octave_Fi.Released += (sender, e) => StaticDatas.PianoSound.Stop(6);
            C2.Octave_So.Pressed += (sender, e) => StaticDatas.PianoSound.Play(7);
            C2.Octave_So.Released += (sender, e) => StaticDatas.PianoSound.Stop(7);
            C2.Octave_Si.Pressed += (sender, e) => StaticDatas.PianoSound.Play(8);
            C2.Octave_Si.Released += (sender, e) => StaticDatas.PianoSound.Stop(8);
            C2.Octave_La.Pressed += (sender, e) => StaticDatas.PianoSound.Play(9);
            C2.Octave_La.Released += (sender, e) => StaticDatas.PianoSound.Stop(9);
            C2.Octave_Li.Pressed += (sender, e) => StaticDatas.PianoSound.Play(10);
            C2.Octave_Li.Released += (sender, e) => StaticDatas.PianoSound.Stop(10);
            C2.Octave_Ti.Pressed += (sender, e) => StaticDatas.PianoSound.Play(11);
            C2.Octave_Ti.Released += (sender, e) => StaticDatas.PianoSound.Stop(11);
            #endregion

            #region Level3       
            C3.Octave_Do.Pressed += (sender, e) => StaticDatas.PianoSound.Play(12);
            C3.Octave_Do.Released += (sender, e) => StaticDatas.PianoSound.Stop(12);
            C3.Octave_Di.Pressed += (sender, e) => StaticDatas.PianoSound.Play(13);
            C3.Octave_Di.Released += (sender, e) => StaticDatas.PianoSound.Stop(13);
            C3.Octave_Re.Pressed += (sender, e) => StaticDatas.PianoSound.Play(14);
            C3.Octave_Re.Released += (sender, e) => StaticDatas.PianoSound.Stop(14);
            C3.Octave_Ri.Pressed += (sender, e) => StaticDatas.PianoSound.Play(15);
            C3.Octave_Ri.Released += (sender, e) => StaticDatas.PianoSound.Stop(15);
            C3.Octave_Mi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(16);
            C3.Octave_Mi.Released += (sender, e) => StaticDatas.PianoSound.Stop(16);
            C3.Octave_Fa.Pressed += (sender, e) => StaticDatas.PianoSound.Play(17);
            C3.Octave_Fa.Released += (sender, e) => StaticDatas.PianoSound.Stop(17);
            C3.Octave_Fi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(18);
            C3.Octave_Fi.Released += (sender, e) => StaticDatas.PianoSound.Stop(18);
            C3.Octave_So.Pressed += (sender, e) => StaticDatas.PianoSound.Play(19);
            C3.Octave_So.Released += (sender, e) => StaticDatas.PianoSound.Stop(19);
            C3.Octave_Si.Pressed += (sender, e) => StaticDatas.PianoSound.Play(20);
            C3.Octave_Si.Released += (sender, e) => StaticDatas.PianoSound.Stop(20);
            C3.Octave_La.Pressed += (sender, e) => StaticDatas.PianoSound.Play(21);
            C3.Octave_La.Released += (sender, e) => StaticDatas.PianoSound.Stop(21);
            C3.Octave_Li.Pressed += (sender, e) => StaticDatas.PianoSound.Play(22);
            C3.Octave_Li.Released += (sender, e) => StaticDatas.PianoSound.Stop(22);
            C3.Octave_Ti.Pressed += (sender, e) => StaticDatas.PianoSound.Play(23);
            C3.Octave_Ti.Released += (sender, e) => StaticDatas.PianoSound.Stop(23);
            //*/
            #endregion
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            slider.ValueChanged -= handler;
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            slider.ValueChanged -= handler;
            Navigation.PopAsync(false);
            return true;
        }
    }
}
