﻿using Abp.Notifications;

namespace DTKH2024.SbinSolution.Notifications;

public class SendNotificationToAllUsersArgs
{
    public string NotificationName { get; set; }
    public string Message { get; set; }
    public NotificationSeverity Severity { get; set; }
}