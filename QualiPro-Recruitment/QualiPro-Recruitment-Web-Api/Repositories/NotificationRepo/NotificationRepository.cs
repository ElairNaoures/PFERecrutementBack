using Microsoft.EntityFrameworkCore;
using QualiPro_Recruitment_Data.Data;
using QualiPro_Recruitment_Data.Models;

namespace QualiPro_Recruitment_Web_Api.Repositories.NotificationRepo
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly QualiProContext _qualiProContext;

        public NotificationRepository(QualiProContext qualiProContext)
        {
            _qualiProContext = qualiProContext;

        }
        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            _qualiProContext.Notifications.Add(notification);
            await _qualiProContext.SaveChangesAsync();
            return notification;
        }
        public async Task<Notification?> GetNotificationByIdAsync(int id)
        {
            return await _qualiProContext.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _qualiProContext.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            _qualiProContext.Notifications.Update(notification);
            await _qualiProContext.SaveChangesAsync();
        }

    }
}
