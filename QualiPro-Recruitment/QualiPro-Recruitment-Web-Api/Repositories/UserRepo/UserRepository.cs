using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly QualiProContext _qualiProContext;
        private readonly string _imageFolderPath;


        public UserRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;
            _imageFolderPath = "C:\\Users\\LENOVO\\Desktop\\PfERecrutement\\PFERecrutementBack\\QualiPro-Recruitment\\QualiPro-Recruitment-Web-Api\\UploadedImages";


        }


        //public async Task<TabUser> AddUser(UserAccountRoleModel userInput, string hashedPassword)
        //{
        //    // Create a new TabUser object
        //    var user = new TabUser
        //    {
        //        FirstName = userInput.FirstName,
        //        LastName = userInput.LastName,
        //        Country = userInput.Country,
        //        PhoneNumber = userInput.PhoneNumber,
        //        Birthdate = userInput.Birthdate,
        //        RoleId = userInput.RoleId
        //    };

        //    // Add user to the context
        //    await _qualiProContext.TabUsers.AddAsync(user);
        //    await _qualiProContext.SaveChangesAsync();

        //    // Create a new TabAccount object linked to the user
        //    var account = new TabAccount
        //    {
        //        Email = userInput.Email,
        //        Password = hashedPassword,
        //        Blocked = userInput.Blocked ?? false,
        //        UserId = user.Id  // Link the account to the user
        //    };

        //    // Add account to the context
        //    await _qualiProContext.TabAccounts.AddAsync(account);
        //    await _qualiProContext.SaveChangesAsync();

        //    return user;
        //}
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
                  .Where(p => p.Deleted == false || p.Deleted == null)
                .OrderByDescending(p => p.Id)
                                 .Include(u => u.Role)
                                 .Select(u => new UserDTO
                                 {
                                     Id = u.Id,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Country = u.Country,
                                     PhoneNumber = u.PhoneNumber,
                                     Birthdate = u.Birthdate,
                                     //RoleId = u.RoleId,
                                     RoleName = u.Role.RoleName
                                 })
                                 .ToListAsync();
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            return await _qualiProContext.TabUsers.Where(p => p.Id == userId && (p.Deleted == false || p.Deleted == null))

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

        //public async Task UpdateUser(UserDTO userDTO)
        //{
        //    var user = await _qualiProContext.TabUsers.FindAsync(userDTO.Id);
        //    if (user == null)
        //    {
        //        return;
        //    }
        //    user.FirstName = userDTO.FirstName;
        //    user.LastName = userDTO.LastName;
        //    user.Country = userDTO.Country;
        //    user.PhoneNumber = userDTO.PhoneNumber;
        //    user.Birthdate = userDTO.Birthdate;
        //    user.RoleId = userDTO.RoleId;

        //    if (!string.IsNullOrEmpty(userDTO.ImageFileName))
        //    {
        //        user.ImageFileName = SaveFile(_imageFolderPath, userDTO.ImageFileName);
        //    }
        //    _qualiProContext.TabUsers.Update(user);
        //    await _qualiProContext.SaveChangesAsync();
        //}
        public async Task UpdateUser(UserDTO userDTO)
        {
            var user = await _qualiProContext.TabUsers.FindAsync(userDTO.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Mise à jour des autres attributs
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Country = userDTO.Country;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Birthdate = userDTO.Birthdate;

            // Ne mettre à jour le RoleId que s'il est défini dans le DTO
            if (userDTO.RoleId.HasValue)
            {
                user.RoleId = userDTO.RoleId.Value;
            }

            // Gestion de l'image si nécessaire
            if (!string.IsNullOrEmpty(userDTO.ImageFileName))
            {
                user.ImageFileName = SaveFile(_imageFolderPath, userDTO.ImageFileName);
            }

            _qualiProContext.TabUsers.Update(user);
            await _qualiProContext.SaveChangesAsync();
        }


        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _qualiProContext.TabUsers.FindAsync(userId);
            if (user == null)
            {
                return false;
        
            }
            user.Deleted = true;
            await _qualiProContext.SaveChangesAsync();
            return true;
        }
        private string SaveFile(string folderPath, string fileName)
        {
            // Logic to save the file
            var filePath = Path.Combine(folderPath, fileName);
            // Here you can add code to actually move the file to the desired folder
            return fileName;
        }
        public async Task<string> GetRoleNameById(int roleId)
        {
            var role = await _qualiProContext.TabRoles.FindAsync(roleId);
            if (role == null)
            {
                throw new Exception("Role not found");
            }
            return role.RoleName;
        }
    }
}
