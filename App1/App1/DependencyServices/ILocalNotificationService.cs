﻿using System;

namespace App1.DependencyServices
{
    public interface ILocalNotificationService
    {
        void LocalNotification(string title, string body, int id, DateTime notifyTime);
        void Cancel(int id);
    }
}
