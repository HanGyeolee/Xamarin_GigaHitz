using System;

namespace GigaHitz.Interfaces
{
    public interface IAdBannerController
    {
        void Load(string UId);
        void GetAdView<T>(ref T adView);
    }
}
