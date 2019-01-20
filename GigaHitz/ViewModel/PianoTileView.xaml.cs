using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GigaHitz.ViewModel
{
    public partial class PianoTileView : ContentView
    {
        public PianoTileView()
        {
            InitializeComponent();
        }

        void PressedTile(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.BackgroundColor = new Color(0.875, 0.875, 0.875);
        }

        void ReleasedWhiteTile(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.BackgroundColor = new Color(1, 1, 1);
        }

        void ReleasedBlackTile(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.BackgroundColor = new Color(0, 0, 0);
        }

        public Button Octave_Do
        {
            get { return Do; }
        }

        public Button Octave_Di
        {
            get { return Di; }
        }

        public Button Octave_Re
        {
            get { return Re; }
        }

        public Button Octave_Ri
        {
            get { return Ri; }
        }

        public Button Octave_Mi
        {
            get { return Mi; }
        }

        public Button Octave_Fa
        {
            get { return Fa; }
        }

        public Button Octave_Fi
        {
            get { return Fi; }
        }

        public Button Octave_So
        {
            get { return So; }
        }

        public Button Octave_Si
        {
            get { return Si; }
        }

        public Button Octave_La
        {
            get { return La; }
        }

        public Button Octave_Li
        {
            get { return Li; }
        }

        public Button Octave_Ti
        {
            get { return Ti; }
        }
    }
}
