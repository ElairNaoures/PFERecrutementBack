using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.NotificationRepo
{
    public interface INotificationRepository
    {
        Task<Notification> AddNotificationAsync(Notification notification);
        Task<Notification?> GetNotificationByIdAsync(int id);                
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId); 
        Task UpdateNotificationAsync(Notification notification);            
    }
}
