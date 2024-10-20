using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Web_Api.Repositories;
using QualiPro_Recruitment_Web_Api.Repositories.AuthRepo;
using QualiPro_Recruitment_Web_Api.Repositories.CompteRepo;
using QualiPro_Recruitment_Web_Api.Repositories.CondidatRepo;
using QualiPro_Recruitment_Web_Api.Repositories.ContraTypeRepo;
using QualiPro_Recruitment_Web_Api.Repositories.EducationRepo;
using QualiPro_Recruitment_Web_Api.Repositories.JobApplicationRepo;
using QualiPro_Recruitment_Web_Api.Repositories.JobRepo;
using QualiPro_Recruitment_Web_Api.Repositories.ModuleRepo;
using QualiPro_Recruitment_Web_Api.Repositories.NotificationRepo;
using QualiPro_Recruitment_Web_Api.Repositories.ProfessionalExperienceRepo;
using QualiPro_Recruitment_Web_Api.Repositories.ProfileJobRepo;
using QualiPro_Recruitment_Web_Api.Repositories.QuestionOptionRepo;
using QualiPro_Recruitment_Web_Api.Repositories.QuestionRepo;
using QualiPro_Recruitment_Web_Api.Repositories.QuizEvaluationRepo;
using QualiPro_Recruitment_Web_Api.Repositories.QuizRepo;
using QualiPro_Recruitment_Web_Api.Repositories.RoleRepo;
using QualiPro_Recruitment_Web_Api.Repositories.SkillRepo;
using QualiPro_Recruitment_Web_Api.Repositories.UserRepo;
using QualiPro_Recruitment_Web_Api.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ICompteRepository, CompteRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IContractTypeRepository, ContractTypeRepository>();

builder.Services.AddScoped<ICondidatRepository, CondidatRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();

builder.Services.AddScoped<IProfessionalExperienceRepository, ProfessionalExperienceRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IProfileJobRepository, ProfileJobRepository>();

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

builder.Services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();

builder.Services.AddScoped<IQuizEvaluationRepository, QuizEvaluationRepository>();

builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();


builder.Services.AddDbContext<QualiProContext>();


builder.Services.AddControllers();
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
//        options.JsonSerializerOptions.WriteIndented = true; // Optional: For better readability
//    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Logging.AddConsole();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure CORS
app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"C:\Users\LENOVO\Desktop\back\PFERecrutementBack\QualiPro-Recruitment\QualiPro-Recruitment-Web-Api\UploadedImages"),
    RequestPath = "/UploadedImages"
});

app.UseStaticFiles(new StaticFileOptions      
{
    FileProvider = new PhysicalFileProvider(@"C:\Users\LENOVO\Desktop\back\PFERecrutementBack\QualiPro-Recruitment\QualiPro-Recruitment-Web-Api\UploadedCVs"),
    RequestPath = "/UploadedCVs"
});


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
