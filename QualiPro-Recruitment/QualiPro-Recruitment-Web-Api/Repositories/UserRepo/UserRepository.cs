using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly QualiProContext _qualiProContext;

        public UserRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }

        public async Task<TabUser> AddUser(UserDTO userInput)
        {
            TabUser user = new TabUser()
            {
                FirstName = userInput.FirstName,
                LastName = userInput.LastName,
                Country = userInput.Country,
                PhoneNumber = userInput.PhoneNumber,
                Birthdate = userInput.Birthdate,
                RoleId = userInput.RoleId
            };

            await _qualiProContext.AddAsync(user);
            await _qualiProContext.SaveChangesAsync();
            return user;
        }

        //public async Task<List<TabUser>> GetAllUsers()
        //{

        //    return await _qualiProContext.TabUsers
        //                         .Include(u => u.Role) // Inclure la relation avec TabRole
        //                         .ToListAsync();
        //    //var ListUsers = await _qualiProContext.TabUsers.ToListAsync();
        //    //return ListUsers;
        //}

        //public async Task<TabUser> GetUserById(int userId)
        //{
        //    return await _qualiProContext.TabUsers
        //                    .Include(u => u.Role) // Inclure la relation avec TabRole
        //                    .FirstOrDefaultAsync(u => u.Id == userId);
        //    //var User = await _qualiProContext.TabUsers.FirstOrDefaultAsync(p => p.Id == userId);
        //    //return User;
        //}
        public async Task<List<UserDTO>> GetAllUsers()
        {
            return await _qualiProContext.TabUsers
                                 .Include(u => u.Role)
                                 .Select(u => new UserDTO
                                 {
                                     Id = u.Id,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Country = u.Country,
                                     PhoneNumber = u.PhoneNumber,
                                     Birthdate = u.Birthdate,
                                     RoleName = u.Role.RoleName
                                 })
                                 .ToListAsync();
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            return await _qualiProContext.TabUsers
                                 .Include(u => u.Role)
                                 .Where(u => u.Id == userId)
                                 .Select(u => new UserDTO
                                 {
                                     Id = u.Id,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Country = u.Country,
                                     PhoneNumber = u.PhoneNumber,
                                     Birthdate = u.Birthdate,
                                    // RoleId = u.Role.RoleId,
                                     RoleName = u.Role.RoleName
                                 })
                                 .FirstOrDefaultAsync();
        }

        public async Task<TabUser> UpdateUser(int userId, TabUser userInput)
        {
            var user = await _qualiProContext.TabUsers.FirstOrDefaultAsync(p => p.Id == userId);
            user.FirstName = userInput.FirstName;
            user.LastName = userInput.LastName;
            user.Country = userInput.Country;
            user.PhoneNumber = userInput.PhoneNumber;
            user.Birthdate = userInput.Birthdate;
            user.RoleId = userInput.RoleId;
            

            await _qualiProContext.SaveChangesAsync();
            return user;
        }

        public async Task<TabUser> DeleteUser(int userId)
        {
            var user = await _qualiProContext.TabUsers.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            _qualiProContext.TabUsers.Remove(user);
            await _qualiProContext.SaveChangesAsync();
            return user;
        }

    }
}
