using System;

namespace DTKH2024.SbinSolution.Notifications.Dto
{
    public class GetPublishedNotificationsInput
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}