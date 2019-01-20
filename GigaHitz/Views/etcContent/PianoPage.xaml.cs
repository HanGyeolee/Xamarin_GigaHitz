using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;
using GigaHitz.DataBase;

namespace GigaHitz.Views.etcContent
{
    public partial class PianoPage : ContentPage
    {
        public PianoPage()
        {
            InitializeComponent();

            double max, height = Application.Current.MainPage.Height;

            max = 1130 * height / 667;
            max *= 0.5 * height / 667;
            Tile3.HeightRequest = 600 * height / 667;
            Tile2.HeightRequest = 600 * height / 667;

            // 기기 세로 크기에 맞춰 재생
            // 안드로이드는 시스템사운드를 이용
            // 아이폰은 avaudio 그대로 이용aa

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);

            slider.Value = 0.5;
            Device.BeginInvokeOnMainThread(delegate
            {
                Tile.ScrollToAsync(0, max * slider.Value, false);
            });

            slider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
            {
                Tile.ScrollToAsync(0, max * (1 - slider.Value), false);
            };

            Load();
        }

        void Load()
        {
            #region Level2
            C2.Octave_Do.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(0);
            C2.Octave_Do.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(0);
            C2.Octave_Di.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(1);
            C2.Octave_Di.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(1);
            C2.Octave_Re.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(2);
            C2.Octave_Re.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(2);
            C2.Octave_Ri.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(3);
            C2.Octave_Ri.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(3);
            C2.Octave_Mi.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(4);
            C2.Octave_Mi.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(4);
            C2.Octave_Fa.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(5);
            C2.Octave_Fa.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(5);
            C2.Octave_Fi.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(6);
            C2.Octave_Fi.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(6);
            C2.Octave_So.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(7);
            C2.Octave_So.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(7);
            C2.Octave_Si.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(8);
            C2.Octave_Si.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(8);
            C2.Octave_La.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(9);
            C2.Octave_La.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(9);
            C2.Octave_Li.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(10);
            C2.Octave_Li.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(10);
            C2.Octave_Ti.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(11);
            C2.Octave_Ti.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(11);
            #endregion

            #region Level3       
            C3.Octave_Do.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(12);
            C3.Octave_Do.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(12);
            C3.Octave_Di.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(13);
            C3.Octave_Di.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(13);
            C3.Octave_Re.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(14);
            C3.Octave_Re.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(14);
            C3.Octave_Ri.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(15);
            C3.Octave_Ri.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(15);
            C3.Octave_Mi.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(16);
            C3.Octave_Mi.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(16);
            C3.Octave_Fa.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(17);
            C3.Octave_Fa.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(17);
            C3.Octave_Fi.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(18);
            C3.Octave_Fi.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(18);
            C3.Octave_So.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(19);
            C3.Octave_So.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(19);
            C3.Octave_Si.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(20);
            C3.Octave_Si.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(20);
            C3.Octave_La.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(21);
            C3.Octave_La.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(21);
            C3.Octave_Li.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(22);
            C3.Octave_Li.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(22);
            C3.Octave_Ti.Pressed += (sender, e) =>  StaticDatas.soundEffect.Play(23);
            C3.Octave_Ti.Released += (sender, e) =>  StaticDatas.soundEffect.Stop(23);
            #endregion
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            await Navigation.PopAsync(false);
        }
    }
}
