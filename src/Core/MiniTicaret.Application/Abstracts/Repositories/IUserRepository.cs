using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Repositories;

public interface IUserRepository:IRepository<AppUser>
{
    Task<AppUser?> GetByIdAsync(string id);
    Task<AppUser?> GetByIdWithRelationsAsync(string id);
    Task<List<AppUser>> GetAllUsersWithRolesAsync();
    Task<List<string>> GetUserRolesAsync(AppUser user);
}
