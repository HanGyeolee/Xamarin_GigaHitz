using System.Threading;
using System.Threading.Tasks;
using GigaHitz.Interfaces;
using Xamarin.Forms;

namespace GigaHitz.DataBase
{
    public static class StaticDatas
    {
        public static IPianoSound pianoSound { get; private set; } = DependencyService.Get<IPianoSound>();
        readonly static int sleep = 2;

        public static async Task<bool> Load()
        {
            pianoSound.Initialize(24);

            #region Level2
            pianoSound.AddSystemSound("Do2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Di2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Re2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Ri2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Mi2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Fa2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Fi2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("So2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Si2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("La2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Li2");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Ti2");
            Thread.Sleep(sleep);
            #endregion

            #region Level3
            pianoSound.AddSystemSound("Do3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Di3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Re3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Ri3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Mi3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Fa3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Fi3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("So3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Si3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("La3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Li3");
            Thread.Sleep(sleep);
            pianoSound.AddSystemSound("Ti3");
            Thread.Sleep(sleep);

            #endregion

            return await Task.FromResult<bool>(true);
        }
    }
}
