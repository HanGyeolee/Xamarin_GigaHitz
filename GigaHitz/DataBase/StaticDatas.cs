using System.Threading;
using System.Threading.Tasks;
using GigaHitz.Interfaces;
using Xamarin.Forms;

namespace GigaHitz.DataBase
{
    public static class StaticDatas
    {
        public static ISoundEffect soundEffect { get; private set; } = DependencyService.Get<ISoundEffect>();
        readonly static int sleep = 2;

        public static async Task<bool> Load()
        {
            soundEffect.Initialize(24);

            #region Level2
            soundEffect.AddSystemSound("Do2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Di2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Re2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Ri2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Mi2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Fa2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Fi2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("So2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Si2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("La2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Li2");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Ti2");
            Thread.Sleep(sleep);
            #endregion

            #region Level3
            soundEffect.AddSystemSound("Do3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Di3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Re3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Ri3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Mi3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Fa3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Fi3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("So3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Si3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("La3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Li3");
            Thread.Sleep(sleep);
            soundEffect.AddSystemSound("Ti3");
            Thread.Sleep(sleep);

            #endregion

            return await Task.FromResult<bool>(true);
        }
    }
}
