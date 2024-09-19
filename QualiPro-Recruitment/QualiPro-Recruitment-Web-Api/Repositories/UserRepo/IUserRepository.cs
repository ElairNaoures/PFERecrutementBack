using Microsoft.AspNetCore.Mvc;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;
using QualiPro_Recruitment_Web_Api.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.UserRepo
{
    public interface IUserRepository
    {
        //Task<List<TabUser>> GetAllUsers();
        Task<List<UserDTO>> GetAllUsers();
        //Task<TabUser> GetUserById(int userId);
        Task<UserDTO> GetUserById(int userId);
        //Task<TabUser> AddUser(UserDTO userInput);
      
        //Task<TabUser> AddUser(UserAccountRoleModel userInput, string hashedPassword); // Mise à jour de la signature

        Task UpdateUser(UserDTO UserDTO );
        Task<bool> DeleteUser(int userId);
    }
}
