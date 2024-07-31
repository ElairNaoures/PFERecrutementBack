using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;
using QualiPro_Recruitment_Web_Api.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QualiPro_Recruitment_Web_Api.Repositories.AuthRepo
{
    public class AuthRepository : IAuthRepository
    {
        private readonly QualiProContext _qualiProContext;
        private readonly IConfiguration _configuration;

        public AuthRepository(QualiProContext qualiProContext, IConfiguration configuration)
        {
            _qualiProContext = qualiProContext;
            _configuration = configuration;
        }

        // Méthode pour récupérer un compte utilisateur par email
        public async Task<TabAccount> GetUserAccountByEmail(string email)
        {
            return await _qualiProContext.TabAccounts.FirstOrDefaultAsync(u => u.Email == email);
        }


        // Méthode pour récupérer un compte condidat par email
        public async Task<TabAccountCondidat> GetCondidatAccountByEmail(string email)
        {
            return await _qualiProContext.TabAccountCondidats.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Méthode pour récupérer un compte utilisateur par email et mot de passe
        public async Task<TabAccount> GetUserAccountByCredentials(string email, string password)
        {
            return await _qualiProContext.TabAccounts.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        // Méthode pour récupérer un compte condidat par email et mot de passe
        public async Task<TabAccountCondidat> GetCondidatAccountByCredentials(string email, string password)
        {
            return await _qualiProContext.TabAccountCondidats.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        // Méthode pour générer un token JWT pour un utilisateur
        public async Task<AuthModel> GenerateToken(TabAccount userAccount)
        {
            try
            {
                if (userAccount == null)
                {
                    throw new ArgumentNullException(nameof(userAccount), "User account cannot be null");
                }

                // Récupérer les informations de l'utilisateur
                UserAccountRoleModel userInfo = await GetUserInfo(userAccount.UserId.Value);

                // Créer les claims pour le token JWT
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64),
                    new Claim("UserId", userInfo.UserId.ToString()),
                    new Claim("FirstName", userInfo.FirstName),
                    new Claim("LastName", userInfo.LastName),
                    new Claim("Country", userInfo.Country),
                    new Claim("PhoneNumber", userInfo.PhoneNumber),
                    new Claim("Birthdate", userInfo.Birthdate.ToString()),
                    new Claim("AccountId", userInfo.AccountId.ToString()),
                    new Claim("Email", userInfo.Email),
                    new Claim("Blocked", userInfo.Blocked.ToString()),
                    new Claim("RoleId", userInfo.RoleId.ToString()),
                    new Claim("RoleName", userInfo.RoleName.ToString())
                };

                // Créer la clé et les informations de signature pour le token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: signIn);

                // Créer le modèle AuthModel pour la réponse
                AuthModel authModel = new AuthModel
                {
                    Success = true,
                    Message = "Token generated successfully",
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    UserInfo = userInfo
                };

                return authModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating token", ex);
            }
        }


        // Méthode pour générer un token JWT pour un utilisateur
        public async Task<AuthModel> GenerateTokenCondidat(TabAccountCondidat condidatAccount)
        {
            try
            {
                if (condidatAccount == null)
                {
                    throw new ArgumentNullException(nameof(condidatAccount), "Condidat account cannot be null");
                }

                // Récupérer les informations de Condidat
                CondidatAccountRoleModel condidatInfo = await GetCondidatInfo(condidatAccount.CondidatId.Value);

                // Créer les claims pour le token JWT
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64),
                    new Claim("CondidatId", condidatInfo.CondidatId.ToString()),
                    new Claim("Summary", condidatInfo.Summary),
                    new Claim("FirstName", condidatInfo.FirstName),
                    new Claim("LastName", condidatInfo.LastName),
                    new Claim("Country", condidatInfo.Country),
                    new Claim("PhoneNumber", condidatInfo.PhoneNumber),
                    new Claim("Birthdate", condidatInfo.Birthdate.ToString()),
                    new Claim("AccountId", condidatInfo.AccountId.ToString()),
                    new Claim("Email", condidatInfo.Email),
                    new Claim("Blocked", condidatInfo.Blocked.ToString()),
                    //new Claim("RoleId", condidatInfo.RoleId.ToString()),
                    //new Claim("RoleName", condidatInfo.RoleName.ToString())
                };

                // Créer la clé et les informations de signature pour le token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: signIn);

                // Créer le modèle AuthModel pour la réponse
                AuthModel authModel = new AuthModel
                {
                    Success = true,
                    Message = "Token generated successfully",
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    CondidatInfo = condidatInfo
                };

                return authModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating token", ex);
            }
        }

        // Méthode pour récupérer les informations de l'utilisateur
        public async Task<UserAccountRoleModel> GetUserInfo(int userId)
        {
            try
            {
                UserAccountRoleModel userAccountRole = new UserAccountRoleModel();

                // Récupérer l'utilisateur par son ID
                TabUser user = await _qualiProContext.TabUsers.FirstOrDefaultAsync(p => p.Id == userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Remplir les informations de l'utilisateur
                userAccountRole.UserId = user.Id;
                userAccountRole.FirstName = user.FirstName;
                userAccountRole.LastName = user.LastName;
                userAccountRole.Country = user.Country;
                userAccountRole.PhoneNumber = user.PhoneNumber;
                userAccountRole.Birthdate = user.Birthdate;

                // Récupérer le compte associé à l'utilisateur
                TabAccount account = await _qualiProContext.TabAccounts.FirstOrDefaultAsync(p => p.UserId == userId);
                if (account != null)
                {
                    userAccountRole.AccountId = account.Id;
                    userAccountRole.Email = account.Email;
                    userAccountRole.Blocked = account.Blocked;
                }

                // Récupérer le rôle de l'utilisateur
                if (user.RoleId != null && user.RoleId != 0)
                {
                    TabRole role = await _qualiProContext.TabRoles.FirstOrDefaultAsync(p => p.Id == user.RoleId.Value);
                    if (role != null)
                    {
                        userAccountRole.RoleId = role.Id;
                        userAccountRole.RoleName = role.RoleName;
                    }
                }

                return userAccountRole;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user info", ex);
            }
        }

        // Méthode pour récupérer les informations de l'utilisateur
        public async Task<CondidatAccountRoleModel> GetCondidatInfo(int condidatId)
        {
            try
            {
                CondidatAccountRoleModel condidatAccountRole = new CondidatAccountRoleModel();

                // Récupérer le Condidat par son ID
                TabCondidat condidat = await _qualiProContext.TabCondidats.FirstOrDefaultAsync(p => p.Id == condidatId);
                if (condidat == null)
                {
                    throw new Exception("Condidat not found");
                }

                // Remplir les informations de le Condidat
                condidatAccountRole.CondidatId = condidat.Id;
                condidatAccountRole.Summary = condidat.Summary;

                condidatAccountRole.FirstName = condidat.FirstName;
                condidatAccountRole.LastName = condidat.LastName;
                condidatAccountRole.Country = condidat.Country;
                condidatAccountRole.PhoneNumber = condidat.PhoneNumber;
                condidatAccountRole.Birthdate = condidat.Birthdate;

                // Récupérer le compte associé à l'utilisateur
                TabAccountCondidat account = await _qualiProContext.TabAccountCondidats.FirstOrDefaultAsync(p => p.CondidatId == condidatId);
                if (account != null)
                {
                    condidatAccountRole.AccountId = account.Id;
                    condidatAccountRole.Email = account.Email;
                    condidatAccountRole.Blocked = account.Blocked;
                }

                // Récupérer le rôle de l'utilisateur
                //if (condidat.RoleId != null && condidat.RoleId != 0)
                //{
                //    TabRole role = await _qualiProContext.TabRoles.FirstOrDefaultAsync(p => p.Id == user.RoleId.Value);
                //    if (role != null)
                //    {
                //        userAccountRole.RoleId = role.Id;
                //        userAccountRole.RoleName = role.RoleName;
                //    }
                //}

                return condidatAccountRole;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting Condidat info", ex);
            }
        }

        // Méthode pour l'authentification d'un utilisateur
        public async Task<AuthModel> SignIn(SignInModel signInModel)
        {
            AuthModel signInResponse = new AuthModel();
            try
            {
                if (signInModel == null || string.IsNullOrEmpty(signInModel.Email) || string.IsNullOrEmpty(signInModel.Password))
                {
                    signInResponse.Success = false;
                    signInResponse.Message = "Invalid credentials: email or password are empty";
                    return signInResponse;
                }
                //bool passwordMatches = BCrypt.Net.BCrypt.Verify(signInModel.Password, hashedPasswordFromDatabase);

                // Récupérer le compte utilisateur par email
                TabAccount userAccount = await GetUserAccountByEmail(signInModel.Email);
                if (userAccount == null)
                {
                    signInResponse.Success = false;
                    signInResponse.Message = "No account found with provided email";
                    return signInResponse;
                }

                // Vérifier le mot de passe en utilisant BCrypt
                bool passwordMatches = BCrypt.Net.BCrypt.Verify(signInModel.Password, userAccount.Password);
                if (!passwordMatches)
                {
                    signInResponse.Success = false;
                    signInResponse.Message = "Incorrect password";
                    return signInResponse;
                }

                // Générer le token JWT si l'authentification est réussie
                signInResponse = await GenerateToken(userAccount);
                signInResponse.Message = "User authenticated successfully";
                return signInResponse;
            }
            catch (Exception ex)
            {
                signInResponse.Success = false;
                signInResponse.Message = "An error occurred during sign-in";
                throw new Exception("Error signing in", ex);
            }
        }


        // Méthode pour l'authentification d'un condidat
        public async Task<AuthModel> SignInCondidat(SignInModel signInModel)
        {
            AuthModel signInResponse = new AuthModel();
            try
            {
                if (signInModel == null || string.IsNullOrEmpty(signInModel.Email) || string.IsNullOrEmpty(signInModel.Password))
                {
                    signInResponse.Success = false;
                    signInResponse.Message = "Invalid credentials: email or password are empty";
                    return signInResponse;
                }
                //bool passwordMatches = BCrypt.Net.BCrypt.Verify(signInModel.Password, hashedPasswordFromDatabase);

                // Récupérer le compte utilisateur par email
                TabAccountCondidat condidatAccount = await GetCondidatAccountByEmail(signInModel.Email);
                if (condidatAccount == null)
                {
                    signInResponse.Success = false;
                    signInResponse.Message = "No account found with provided email";
                    return signInResponse;
                }

                // Vérifier le mot de passe en utilisant BCrypt
                bool passwordMatches = BCrypt.Net.BCrypt.Verify(signInModel.Password, condidatAccount.Password);
                if (!passwordMatches)
                {
                    signInResponse.Success = false;
                    signInResponse.Message = "Incorrect password";
                    return signInResponse;
                }

                // Générer le token JWT si l'authentification est réussie
                signInResponse = await GenerateTokenCondidat(condidatAccount);
                signInResponse.Message = "Condidat authenticated successfully";
                return signInResponse;
            }
            catch (Exception ex)
            {
                signInResponse.Success = false;
                signInResponse.Message = "An error occurred during sign-in";
                throw new Exception("Error signing in", ex);
            }
        }


        // Méthode pour l'enregistrement d'un nouvel utilisateur
        public async Task<AuthModel> SignUp(UserAccountRoleModel userAccountRoleModel)
        {
            AuthModel signUpResponse = new AuthModel();
            try
            {
                // Vérifier la validité des données d'entrée
                if (userAccountRoleModel == null || string.IsNullOrEmpty(userAccountRoleModel.Email) || string.IsNullOrEmpty(userAccountRoleModel.Password))
                {
                    signUpResponse.Success = false;
                    signUpResponse.Message = "Invalid credentials: email or password cannot be empty";
                    return signUpResponse;
                }

                // Vérifier si l'utilisateur existe déjà dans la base de données
                bool userExists = await _qualiProContext.TabAccounts.AnyAsync(u => u.Email == userAccountRoleModel.Email);
                if (userExists)
                {
                    signUpResponse.Success = false;
                    signUpResponse.Message = "An account with this email already exists";
                    return signUpResponse;
                }

                // Hacher le mot de passe
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userAccountRoleModel.Password);

                // Créer un nouvel utilisateur avec le mot de passe haché
                var newUserAccount = new TabAccount
                {
                    Email = userAccountRoleModel.Email,
                    Password = hashedPassword,
                    Blocked = userAccountRoleModel.Blocked,
                    User = new TabUser
                    {
                        Birthdate = userAccountRoleModel.Birthdate,
                        FirstName = userAccountRoleModel.FirstName,
                        LastName = userAccountRoleModel.LastName,
                        Country = userAccountRoleModel.Country,
                        PhoneNumber = userAccountRoleModel.PhoneNumber,
                        RoleId = userAccountRoleModel.RoleId,
                    }
                };

                // Ajouter l'utilisateur à la base de données
                _qualiProContext.TabAccounts.Add(newUserAccount);
                await _qualiProContext.SaveChangesAsync();

                // Générer un token pour le nouvel utilisateur
                signUpResponse = await GenerateToken(newUserAccount);
                signUpResponse.Message = "User registered successfully";
                return signUpResponse;
            }
            catch (Exception ex)
            {
                signUpResponse.Success = false;
                signUpResponse.Message = "An error occurred during sign-up";
                throw new Exception("Error signing up", ex);
            }
        }



        // Méthode pour l'enregistrement d'un nouvel condidat
        public async Task<AuthModel> SignUpCondidat(CondidatAccountRoleModel condidatAccountRoleModel)
        {
            AuthModel signUpResponse = new AuthModel();
            try
            {
                // Vérifier la validité des données d'entrée
                if (condidatAccountRoleModel == null || string.IsNullOrEmpty(condidatAccountRoleModel.Email) || string.IsNullOrEmpty(condidatAccountRoleModel.Password))
                {
                    signUpResponse.Success = false;
                    signUpResponse.Message = "Invalid credentials: email or password cannot be empty";
                    return signUpResponse;
                }

                // Vérifier si le condidat existe déjà dans la base de données
                bool condidatExists = await _qualiProContext.TabAccountCondidats.AnyAsync(u => u.Email == condidatAccountRoleModel.Email);
                if (condidatExists)
                {
                    signUpResponse.Success = false;
                    signUpResponse.Message = "An account with this email already exists";
                    return signUpResponse;
                }

                // Hacher le mot de passe
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(condidatAccountRoleModel.Password);

                // Créer un nouvel utilisateur avec le mot de passe haché
                var newCondidatAccount = new TabAccountCondidat
                {
                    Email = condidatAccountRoleModel.Email,
                    Password = hashedPassword,
                    Blocked = condidatAccountRoleModel.Blocked,
                    Condidat = new TabCondidat
                    {
                        Summary = condidatAccountRoleModel.Summary,
                        Birthdate = condidatAccountRoleModel.Birthdate,
                        FirstName = condidatAccountRoleModel.FirstName,
                        LastName = condidatAccountRoleModel.LastName,
                        Country = condidatAccountRoleModel.Country,
                        PhoneNumber = condidatAccountRoleModel.PhoneNumber,
                        //RoleId = condidatAccountRoleModel.RoleId,
                    }
                };

                // Ajouter l'utilisateur à la base de données
                _qualiProContext.TabAccountCondidats.Add(newCondidatAccount);
                await _qualiProContext.SaveChangesAsync();

                // Générer un token pour le nouvel utilisateur
                signUpResponse = await GenerateTokenCondidat(newCondidatAccount);
                signUpResponse.Message = "Condidat registered successfully";
                return signUpResponse;
            }
            catch (Exception ex)
            {
                signUpResponse.Success = false;
                signUpResponse.Message = "An error occurred during sign-up";
                throw new Exception("Error signing up", ex);
            }
        }

        public async Task<UserAccountRoleModel> GetUserByToken(TokenModel tokenModel)
        {
            try
            {
                //var handler = new JwtSecurityTokenHandler();
                //var handler = new TokenHandler();


                if (string.IsNullOrWhiteSpace(tokenModel.AccessToken))
                {
                    throw new ArgumentException("Access token is null or empty");
                }

                //if (!handler.CanReadToken(tokenModel.AccessToken))
                //{
                //    throw new ArgumentException("Invalid JWT token format");
                //}

                //handler.ReadJwtToken(tokenModel.AccessToken);
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenModel.AccessToken
                    );

                var userAccountRoleModel = new UserAccountRoleModel()
                {
                    UserId = int.Parse(GetClaimValue(token, "UserId")),
                    FirstName = GetClaimValue(token, "FirstName"),
                    LastName = GetClaimValue(token, "LastName"),
                    Country = GetClaimValue(token, "Country"),
                    PhoneNumber = GetClaimValue(token, "PhoneNumber"),
                    Birthdate = DateTime.Parse(GetClaimValue(token, "Birthdate")),
                    AccountId = int.Parse(GetClaimValue(token, "AccountId")),
                    Email = GetClaimValue(token, "Email"),
                    Blocked = bool.Parse(GetClaimValue(token, "Blocked")),
                    RoleId = int.Parse(GetClaimValue(token, "RoleId")),
                    RoleName = GetClaimValue(token, "RoleName"),
                };

                return userAccountRoleModel;

            }
            catch (ArgumentException argEx)
            {
                // Handle specific argument exceptions
                Console.WriteLine($"Argument error: {argEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }


        private string GetClaimValue(JwtSecurityToken token, string claimType)
        {
            return token.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value
                   ?? throw new ArgumentException($"Claim '{claimType}' not found in the token");
        }

        //public async Task<CondidatAccountRoleModel> GetCondidatById(int id)
        //{
        //    return await _qualiProContext.CondidatAccountRoleModels
        //                         .FirstOrDefaultAsync(c => c.Id == id);
        //}

        //public async Task<TabAccountCondidat> GetCondidatAccountById(int id)
        //{
        //    return await _qualiProContext.TabAccountCondidats
        //                         .FirstOrDefaultAsync(c => c.CondidatId == id);
        //}
        //public async Task<string?> GetEmailByCondidatId(int condidatId)
        //{
        //    var account = await _qualiProContext.TabAccountCondidats
        //                                        .FirstOrDefaultAsync(a => a.CondidatId == condidatId);
        //    return account?.Email;
        //}
    }
}
