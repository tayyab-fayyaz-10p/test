using Recipe.Core.Base.Interface;
using SSH.Core.Enum;

namespace SSH.Core.Infrastructure
{
    public interface ISSHRequestInfo : IRequestInfo
    {
        UserRoles UserRole { get; }
    }
}
