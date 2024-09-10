using System;
using Abp.Notifications;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}