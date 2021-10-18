using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
using WebStore.Interfaces.Services.Identity;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity
{
    public class RolesClient : BaseClient, IRolesClient
    {
        public RolesClient(HttpClient Client) : base(Client, WebAPIAddresses.Identity.Roles)
        {
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) { throw new System.NotImplementedException(); }
    }
}
