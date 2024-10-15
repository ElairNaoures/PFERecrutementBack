using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.AuthRepo
{
    public interface IAuthRepository
    {
        Task<AuthModel> SignIn(SignInModel signInModel);

        Task<AuthModel> SignInCondidat(SignInModel signInModel);
        //Task<AuthModel> SignUp(SignUpModel signUpModel);
        Task<AuthModel> SignUp(UserAccountRoleModel userAccountRoleModel);

        Task<AuthModel> SignUpCondidat(CondidatAccountRoleModel condidatAccountRoleModel);
        Task<UserAccountRoleModel> GetUserByToken(TokenModel tokenModel);
        Task<CondidatAccountRoleModel> GetCondidatByToken(TokenModel tokenModel);
        // Task<UserAccountRoleModel> GetUserByIdAsync(int userId);

        //Task<CondidatAccountRoleModel> GetCondidatByToken(TokenModel tokenModel);

        // Task<TabAccountCondidat> GetCondidatAccountById(int id);
        //Task<string?> GetEmailByCondidatId(int condidatId);
        Task<string> GetRoleNameById(int roleId);


    }
}
