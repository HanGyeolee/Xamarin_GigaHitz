using System.Threading.Tasks;
namespace GigaHitz.Interfaces
{
    public interface IShare
    {
        //Task<bool> Share();
        Task<bool> Share(string filePath, string fileName);
    }
}
