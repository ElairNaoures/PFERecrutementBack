using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.DTOs;

namespace QualiPro_Recruitment_Web_Api.Repositories.UserRepo
{
    public interface IUserRepository
    {
        //Task<List<TabUser>> GetAllUsers();
        Task<List<UserDTO>> GetAllUsers();
        //Task<TabUser> GetUserById(int userId);
        Task<UserDTO> GetUserById(int userId);
        Task<TabUser> AddUser(UserDTO userInput);
        Task<TabUser> UpdateUser(int userId, TabUser userInput);
        Task<TabUser> DeleteUser(int userId);

    }
}
