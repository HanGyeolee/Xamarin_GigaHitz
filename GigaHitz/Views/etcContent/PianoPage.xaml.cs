﻿using System;
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
            C2.Octave_Do.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(0);
            C2.Octave_Do.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(0);
            C2.Octave_Di.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(1);
            C2.Octave_Di.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(1);
            C2.Octave_Re.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(2);
            C2.Octave_Re.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(2);
            C2.Octave_Ri.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(3);
            C2.Octave_Ri.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(3);
            C2.Octave_Mi.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(4);
            C2.Octave_Mi.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(4);
            C2.Octave_Fa.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(5);
            C2.Octave_Fa.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(5);
            C2.Octave_Fi.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(6);
            C2.Octave_Fi.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(6);
            C2.Octave_So.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(7);
            C2.Octave_So.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(7);
            C2.Octave_Si.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(8);
            C2.Octave_Si.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(8);
            C2.Octave_La.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(9);
            C2.Octave_La.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(9);
            C2.Octave_Li.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(10);
            C2.Octave_Li.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(10);
            C2.Octave_Ti.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(11);
            C2.Octave_Ti.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(11);
            #endregion

            #region Level3       
            C3.Octave_Do.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(12);
            C3.Octave_Do.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(12);
            C3.Octave_Di.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(13);
            C3.Octave_Di.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(13);
            C3.Octave_Re.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(14);
            C3.Octave_Re.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(14);
            C3.Octave_Ri.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(15);
            C3.Octave_Ri.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(15);
            C3.Octave_Mi.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(16);
            C3.Octave_Mi.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(16);
            C3.Octave_Fa.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(17);
            C3.Octave_Fa.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(17);
            C3.Octave_Fi.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(18);
            C3.Octave_Fi.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(18);
            C3.Octave_So.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(19);
            C3.Octave_So.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(19);
            C3.Octave_Si.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(20);
            C3.Octave_Si.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(20);
            C3.Octave_La.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(21);
            C3.Octave_La.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(21);
            C3.Octave_Li.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(22);
            C3.Octave_Li.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(22);
            C3.Octave_Ti.Pressed += (sender, e) =>  StaticDatas.pianoSound.Play(23);
            C3.Octave_Ti.Released += (sender, e) =>  StaticDatas.pianoSound.Stop(23);
            //*/
            #endregion
        }

        void UnLoad()
        {
            slider.ValueChanged -= handler;
            /*
            #region Level2
            C2.Octave_Do.Pressed -= e[0];
            C2.Octave_Do.Released -= e[1];
            C2.Octave_Di.Pressed -= e[2];
            C2.Octave_Di.Released -= e[3];
            C2.Octave_Re.Pressed -= e[4];
            C2.Octave_Re.Released -= e[5];
            C2.Octave_Ri.Pressed -= e[6];
            C2.Octave_Ri.Released -= e[7];
            C2.Octave_Mi.Pressed -= e[8];
            C2.Octave_Mi.Released -= e[9];
            C2.Octave_Fa.Pressed -= e[10];
            C2.Octave_Fa.Released -= e[11];
            C2.Octave_Fi.Pressed -= e[12];
            C2.Octave_Fi.Released -= e[13];
            C2.Octave_So.Pressed -= e[14];
            C2.Octave_So.Released -= e[15];
            C2.Octave_Si.Pressed -= e[16];
            C2.Octave_Si.Released -= e[17];
            C2.Octave_La.Pressed -= e[18];
            C2.Octave_La.Released -= e[19];
            C2.Octave_Li.Pressed -= e[20];
            C2.Octave_Li.Released -= e[21];
            C2.Octave_Ti.Pressed -= e[22];
            C2.Octave_Ti.Released -= e[23];
            #endregion

            #region Level3   
            C3.Octave_Do.Pressed -= e[24];
            C3.Octave_Do.Released -= e[25];
            C3.Octave_Di.Pressed -= e[26];
            C3.Octave_Di.Released -= e[27];
            C3.Octave_Re.Pressed -= e[28];
            C3.Octave_Re.Released -= e[29];
            C3.Octave_Ri.Pressed -= e[30];
            C3.Octave_Ri.Released -= e[31];
            C3.Octave_Mi.Pressed -= e[32];
            C3.Octave_Mi.Released -= e[33];
            C3.Octave_Fa.Pressed -= e[34];
            C3.Octave_Fa.Released -= e[35];
            C3.Octave_Fi.Pressed -= e[36];
            C3.Octave_Fi.Released -= e[37];
            C3.Octave_So.Pressed -= e[38];
            C3.Octave_So.Released -= e[39];
            C3.Octave_Si.Pressed -= e[40];
            C3.Octave_Si.Released -= e[41];
            C3.Octave_La.Pressed -= e[42];
            C3.Octave_La.Released -= e[43];
            C3.Octave_Li.Pressed -= e[44];
            C3.Octave_Li.Released -= e[45];
            C3.Octave_Ti.Pressed -= e[46];
            C3.Octave_Ti.Released -= e[47];
            #endregion
            //*/
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            UnLoad();
            await Navigation.PopAsync(false);
        }
    }
}
