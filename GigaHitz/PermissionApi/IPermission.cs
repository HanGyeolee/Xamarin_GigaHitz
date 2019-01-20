using System.Threading.Tasks;
using System.Collections.Generic;

namespace GigaHitz.PermissionApi
{
    public interface IPermission
    {
        Task<PermissionStatus> CheckPermissionAsync(Permission permission);
        Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions);
    }
}
