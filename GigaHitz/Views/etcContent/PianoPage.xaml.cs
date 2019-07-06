using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using GigaHitz.DataBase;
using GigaHitz.Renderer;

namespace GigaHitz.Views.etcContent
{
    public partial class PianoPage : ContentPage
    {
        Interfaces.IAdBannerController controller;
        //private CancellationTokenSource cts = new CancellationTokenSource();
        //LoadingView loading;

        //EventHandler<ValueChangedEventArgs> handler;
        AdBanner AdBanner;

        //Xamarin.Forms.Slider slider;

        double max, rat;

        public PianoPage()
        {
            InitializeComponent();
            controller = DependencyService.Get<Interfaces.IAdBannerController>();
            AdBanner = new AdBanner();
            RelativeLayout RL = new RelativeLayout()
            {
                Rotation = -90,
                AnchorX = 0,
                AnchorY = 0
            };

            //배너를 만들고 난 이후에 광고 주소 로드.
            if (Device.RuntimePlatform.Equals(Device.Android))
                controller.Load("ca-app-pub-8979507455037422/9611104515");
            else if (Device.RuntimePlatform.Equals(Device.iOS))
                controller.Load("ca-app-pub-8979507455037422/1646227850");
            AdBanner.Size = AdBanner.Sizes.SmartBannerPortrait;

            //Task position = new Task(CheckingPosition);
            Load();
            //position.Start();

            double height = Xamarin.Forms.Application.Current.MainPage.Height;
            max = height;
            rat = 370 * height / 667.0;

            max = 4 * rat;
            max *= 0.5;
            max += 0.2 * rat;
            Tile4.HeightRequest = rat;
            Tile3.HeightRequest = rat;
            Tile2.HeightRequest = rat;
            Tile1.HeightRequest = rat;

            // 기기 세로 크기에 맞춰 재생
            // 안드로이드는 시스템사운드를 이용
            // 아이폰은 avaudio 그대로 이용

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            //loading = new LoadingView();
            //Main.Children.Add(loading, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            /* in ios
            <RelativeLayout x:Name="AL0"
                             BackgroundColor="Black"
                            Rotation="-90"
                            AnchorX="0"
                            AnchorY="0"
                            RelativeLayout.XConstraint="{ConstraintExpression 
                        Type=RelativeToParent,
                        Property=Height,
                        Factor=0.5}"
                            RelativeLayout.YConstraint="{ConstraintExpression 
                        Type=RelativeToParent,
                        Property=Height,
                        Factor=1}"
                            RelativeLayout.WidthConstraint="{ConstraintExpression 
                        Type=RelativeToParent,
                        Property=Height,
                        Factor=1}"
                            RelativeLayout.HeightConstraint="{ConstraintExpression 
                        Type=RelativeToParent,
                        Property=Width,
                        Factor=1}">
            </RelativeLayout>
            //*/
            if (Device.RuntimePlatform.Equals(Device.Android))
            { // 현재 디바이스가 안드로이드일 경우
                AL.Children.Add(RL,
                Constraint.RelativeToParent((parent) =>
                {
                    return 0;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height * (0.4375 + 0.3125);
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height * 0.625;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }));

                AdBanner.AnchorX = 0.5;
                RL.Children.Add(AdBanner,
                Constraint.RelativeToParent((parent) =>
                {
                    return 0; //parent.Width * 0.25;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return 0;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height * 1;
                }));

                Image image = new Image
                {
                    Source = "menu_button.png",
                    IsEnabled = false
                };
                Button button = new Button
                {
                    BackgroundColor = Color.Transparent,
                    CornerRadius = 0
                };
                button.Clicked += Btn_Back;

                AL1.Children.Add(image, new Rectangle(0.5, 0.5, 1, 1), AbsoluteLayoutFlags.All);
                AL1.Children.Add(button, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            }
            else // 현재 디바이스가 아이폰일 경우
            {
                AL.Children.Add(RL,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height * (0.3125) - parent.Width * (0.5) + RL.Height * (0.5);
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height * 0.5 + RL.Width * (0.46875);
                }),
                Constraint.RelativeToParent((parent) =>
                { // Width
                    return parent.Height * 0.625;
                }),
                Constraint.RelativeToParent((parent) =>
                { // Height
                    return parent.Width;
                }));

                AdBanner.AnchorX = 0.5;
                RL.Children.Add(AdBanner,
                Constraint.RelativeToParent((parent) =>
                {
                    return 0;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return 0;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height * 1;
                }));

                Image image = new Image
                {
                    Source = "menu_button.png",
                    IsEnabled = false
                };
                Button button = new Button
                {
                    BackgroundColor = Color.Transparent,
                    CornerRadius = 0
                };
                button.Clicked += Btn_Back;

                AL1.Children.Add(image, new Rectangle(0, 26, 1, 1), AbsoluteLayoutFlags.SizeProportional);
                // 버튼의 width 값을 height와 비교하여, 비율을 알아낸다.
                var w = Xamarin.Forms.Application.Current.MainPage.Width / height * 0.125;
                Main.Children.Add(button, new Rectangle(0, height * (1 - 2 * w), 0.125, w), AbsoluteLayoutFlags.SizeProportional);
            }

            Tile.ScrollToAsync(0, max * 0.5, false);
            /*
            //안드로이드 에서는 완벽함 -> 고로 안드로이드에서만 이 바를 활성화 한다.
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                slider.Value = 0.5;
                handler = (object sender, ValueChangedEventArgs e) =>
                {
                    //slider를 만졌을 때,
                    Tile.ScrollToAsync(0, max * (1 - slider.Value), false);

                    Console.WriteLine("object sender : " + sender + " " + sender.GetType());
                };

                Tile.Scrolled += (object sender, ScrolledEventArgs e) =>
                {
                    //Tile을 만졌을 때
                };

                slider.ValueChanged += handler;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async delegate
                {
                    await Tile.ScrollToAsync(0, max * 0.5, false);
                });
            }
            //*/

            //StartCoroutine(FadeOut);
        }

        /*
        async void FadeOut()
        {
            await loading.FadeTo(0.5, 1000, Easing.Linear);
            //FadeOut and remove View
            await loading.FadeTo(0, 500, Easing.Linear);

            Device.BeginInvokeOnMainThread(delegate
            {
                Main.Children.Remove(loading);
            });
        }
        //*/
        /*
        async void CheckingPosition()
        {
            while(true)
            {
                if (StaticDatas.PianoSoundsAreLoadingDone())
                    break;

                var y = Tile.ScrollY;
                if (y < max * (4.0 / 28.0)) // level 4
                {
                    StaticDatas.SetPianoLoadingPriority(4, 1); // 0.001
                    StaticDatas.SetPianoLoadingPriority(3, 5);
                    StaticDatas.SetPianoLoadingPriority(2, 25);
                    StaticDatas.SetPianoLoadingPriority(1, 50);
                }
                else if (y < max * (10.0 / 28.0)) // level 3
                {
                    StaticDatas.SetPianoLoadingPriority(4, 5);
                    StaticDatas.SetPianoLoadingPriority(3, 1);
                    StaticDatas.SetPianoLoadingPriority(2, 5);
                    StaticDatas.SetPianoLoadingPriority(1, 25);
                }
                else if (y < max * (15.0 / 28.0)) // level 2
                {
                    StaticDatas.SetPianoLoadingPriority(4, 25);
                    StaticDatas.SetPianoLoadingPriority(3, 5);
                    StaticDatas.SetPianoLoadingPriority(2, 1);
                    StaticDatas.SetPianoLoadingPriority(1, 5);
                }
                else // level 1
                {
                    StaticDatas.SetPianoLoadingPriority(4, 50);
                    StaticDatas.SetPianoLoadingPriority(3, 25);
                    StaticDatas.SetPianoLoadingPriority(2, 5);
                    StaticDatas.SetPianoLoadingPriority(1, 1);
                }

                await Task.Delay(125); // 0.125
            }
        }
        //*/
        async void Btn_Back(object sender, EventArgs s)
        {
            /*
            if (Device.RuntimePlatform.Equals(Device.Android))
                slider.ValueChanged -= handler;
            //*/               
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            /*
            if (Device.RuntimePlatform.Equals(Device.Android))
                slider.ValueChanged -= handler;
            //*/
            Navigation.PopAsync(false);
            return true;
        }

        void Load()
        {
            ///*
            #region Level1
            C1.Octave_Do.Pressed += (sender, e) => StaticDatas.PianoSound.Play(0);
            C1.Octave_Do.Released += (sender, e) => StaticDatas.PianoSound.Stop(0);
            C1.Octave_Di.Pressed += (sender, e) => StaticDatas.PianoSound.Play(1);
            C1.Octave_Di.Released += (sender, e) => StaticDatas.PianoSound.Stop(1);
            C1.Octave_Re.Pressed += (sender, e) => StaticDatas.PianoSound.Play(2);
            C1.Octave_Re.Released += (sender, e) => StaticDatas.PianoSound.Stop(2);
            C1.Octave_Ri.Pressed += (sender, e) => StaticDatas.PianoSound.Play(3);
            C1.Octave_Ri.Released += (sender, e) => StaticDatas.PianoSound.Stop(3);
            C1.Octave_Mi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(4);
            C1.Octave_Mi.Released += (sender, e) => StaticDatas.PianoSound.Stop(4);
            C1.Octave_Fa.Pressed += (sender, e) => StaticDatas.PianoSound.Play(5);
            C1.Octave_Fa.Released += (sender, e) => StaticDatas.PianoSound.Stop(5);
            C1.Octave_Fi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(6);
            C1.Octave_Fi.Released += (sender, e) => StaticDatas.PianoSound.Stop(6);
            C1.Octave_So.Pressed += (sender, e) => StaticDatas.PianoSound.Play(7);
            C1.Octave_So.Released += (sender, e) => StaticDatas.PianoSound.Stop(7);
            C1.Octave_Si.Pressed += (sender, e) => StaticDatas.PianoSound.Play(8);
            C1.Octave_Si.Released += (sender, e) => StaticDatas.PianoSound.Stop(8);
            C1.Octave_La.Pressed += (sender, e) => StaticDatas.PianoSound.Play(9);
            C1.Octave_La.Released += (sender, e) => StaticDatas.PianoSound.Stop(9);
            C1.Octave_Li.Pressed += (sender, e) => StaticDatas.PianoSound.Play(10);
            C1.Octave_Li.Released += (sender, e) => StaticDatas.PianoSound.Stop(10);
            C1.Octave_Ti.Pressed += (sender, e) => StaticDatas.PianoSound.Play(11);
            C1.Octave_Ti.Released += (sender, e) => StaticDatas.PianoSound.Stop(11);
            #endregion
            //*/

            #region Level2
            ///*
            C2.Octave_Do.Pressed += (sender, e) => StaticDatas.PianoSound.Play(12);
            C2.Octave_Do.Released += (sender, e) => StaticDatas.PianoSound.Stop(12);
            C2.Octave_Di.Pressed += (sender, e) => StaticDatas.PianoSound.Play(13);
            C2.Octave_Di.Released += (sender, e) => StaticDatas.PianoSound.Stop(13);
            C2.Octave_Re.Pressed += (sender, e) => StaticDatas.PianoSound.Play(14);
            C2.Octave_Re.Released += (sender, e) => StaticDatas.PianoSound.Stop(14);
            C2.Octave_Ri.Pressed += (sender, e) => StaticDatas.PianoSound.Play(15);
            C2.Octave_Ri.Released += (sender, e) => StaticDatas.PianoSound.Stop(15);
            C2.Octave_Mi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(16);
            C2.Octave_Mi.Released += (sender, e) => StaticDatas.PianoSound.Stop(16);
            C2.Octave_Fa.Pressed += (sender, e) => StaticDatas.PianoSound.Play(17);
            C2.Octave_Fa.Released += (sender, e) => StaticDatas.PianoSound.Stop(17);
            C2.Octave_Fi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(18);
            C2.Octave_Fi.Released += (sender, e) => StaticDatas.PianoSound.Stop(18);
            C2.Octave_So.Pressed += (sender, e) => StaticDatas.PianoSound.Play(19);
            C2.Octave_So.Released += (sender, e) => StaticDatas.PianoSound.Stop(19);
            C2.Octave_Si.Pressed += (sender, e) => StaticDatas.PianoSound.Play(20);
            C2.Octave_Si.Released += (sender, e) => StaticDatas.PianoSound.Stop(20);
            C2.Octave_La.Pressed += (sender, e) => StaticDatas.PianoSound.Play(21);
            C2.Octave_La.Released += (sender, e) => StaticDatas.PianoSound.Stop(21);
            C2.Octave_Li.Pressed += (sender, e) => StaticDatas.PianoSound.Play(22);
            C2.Octave_Li.Released += (sender, e) => StaticDatas.PianoSound.Stop(22);
            C2.Octave_Ti.Pressed += (sender, e) => StaticDatas.PianoSound.Play(23);
            C2.Octave_Ti.Released += (sender, e) => StaticDatas.PianoSound.Stop(23);
            #endregion

            #region Level3   
            C3.Octave_Do.Pressed += (sender, e) => StaticDatas.PianoSound.Play(0 + 24);
            C3.Octave_Do.Released += (sender, e) => StaticDatas.PianoSound.Stop(0 + 24);
            C3.Octave_Di.Pressed += (sender, e) => StaticDatas.PianoSound.Play(1 + 24);
            C3.Octave_Di.Released += (sender, e) => StaticDatas.PianoSound.Stop(1 + 24);
            C3.Octave_Re.Pressed += (sender, e) => StaticDatas.PianoSound.Play(2 + 24);
            C3.Octave_Re.Released += (sender, e) => StaticDatas.PianoSound.Stop(2 + 24);
            C3.Octave_Ri.Pressed += (sender, e) => StaticDatas.PianoSound.Play(3 + 24);
            C3.Octave_Ri.Released += (sender, e) => StaticDatas.PianoSound.Stop(3 + 24);
            C3.Octave_Mi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(4 + 24);
            C3.Octave_Mi.Released += (sender, e) => StaticDatas.PianoSound.Stop(4 + 24);
            C3.Octave_Fa.Pressed += (sender, e) => StaticDatas.PianoSound.Play(5 + 24);
            C3.Octave_Fa.Released += (sender, e) => StaticDatas.PianoSound.Stop(5 + 24);
            C3.Octave_Fi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(6 + 24);
            C3.Octave_Fi.Released += (sender, e) => StaticDatas.PianoSound.Stop(6 + 24);
            C3.Octave_So.Pressed += (sender, e) => StaticDatas.PianoSound.Play(7 + 24);
            C3.Octave_So.Released += (sender, e) => StaticDatas.PianoSound.Stop(7 + 24);
            C3.Octave_Si.Pressed += (sender, e) => StaticDatas.PianoSound.Play(8 + 24);
            C3.Octave_Si.Released += (sender, e) => StaticDatas.PianoSound.Stop(8 + 24);
            C3.Octave_La.Pressed += (sender, e) => StaticDatas.PianoSound.Play(9 + 24);
            C3.Octave_La.Released += (sender, e) => StaticDatas.PianoSound.Stop(9 + 24);
            C3.Octave_Li.Pressed += (sender, e) => StaticDatas.PianoSound.Play(10 + 24);
            C3.Octave_Li.Released += (sender, e) => StaticDatas.PianoSound.Stop(10 + 24);
            C3.Octave_Ti.Pressed += (sender, e) => StaticDatas.PianoSound.Play(11 + 24);
            C3.Octave_Ti.Released += (sender, e) => StaticDatas.PianoSound.Stop(11 + 24);
            //*/
            #endregion

            ///*
            #region Level4
            C4.Octave_Do.Pressed += (sender, e) => StaticDatas.PianoSound.Play(12 + 24);
            C4.Octave_Do.Released += (sender, e) => StaticDatas.PianoSound.Stop(12 + 24);
            C4.Octave_Di.Pressed += (sender, e) => StaticDatas.PianoSound.Play(13 + 24);
            C4.Octave_Di.Released += (sender, e) => StaticDatas.PianoSound.Stop(13 + 24);
            C4.Octave_Re.Pressed += (sender, e) => StaticDatas.PianoSound.Play(14 + 24);
            C4.Octave_Re.Released += (sender, e) => StaticDatas.PianoSound.Stop(14 + 24);
            C4.Octave_Ri.Pressed += (sender, e) => StaticDatas.PianoSound.Play(15 + 24);
            C4.Octave_Ri.Released += (sender, e) => StaticDatas.PianoSound.Stop(15 + 24);
            C4.Octave_Mi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(16 + 24);
            C4.Octave_Mi.Released += (sender, e) => StaticDatas.PianoSound.Stop(16 + 24);
            C4.Octave_Fa.Pressed += (sender, e) => StaticDatas.PianoSound.Play(17 + 24);
            C4.Octave_Fa.Released += (sender, e) => StaticDatas.PianoSound.Stop(17 + 24);
            C4.Octave_Fi.Pressed += (sender, e) => StaticDatas.PianoSound.Play(18 + 24);
            C4.Octave_Fi.Released += (sender, e) => StaticDatas.PianoSound.Stop(18 + 24);
            C4.Octave_So.Pressed += (sender, e) => StaticDatas.PianoSound.Play(19 + 24);
            C4.Octave_So.Released += (sender, e) => StaticDatas.PianoSound.Stop(19 + 24);
            C4.Octave_Si.Pressed += (sender, e) => StaticDatas.PianoSound.Play(20 + 24);
            C4.Octave_Si.Released += (sender, e) => StaticDatas.PianoSound.Stop(20 + 24);
            C4.Octave_La.Pressed += (sender, e) => StaticDatas.PianoSound.Play(21 + 24);
            C4.Octave_La.Released += (sender, e) => StaticDatas.PianoSound.Stop(21 + 24);
            C4.Octave_Li.Pressed += (sender, e) => StaticDatas.PianoSound.Play(22 + 24);
            C4.Octave_Li.Released += (sender, e) => StaticDatas.PianoSound.Stop(22 + 24);
            C4.Octave_Ti.Pressed += (sender, e) => StaticDatas.PianoSound.Play(23 + 24);
            C4.Octave_Ti.Released += (sender, e) => StaticDatas.PianoSound.Stop(23 + 24);
            #endregion
            //*/
        }
        /*
        void StartCoroutine(Action action)
        {
            var task = new Task(action);
            task.Start();
        }
        //*/
    }
}
