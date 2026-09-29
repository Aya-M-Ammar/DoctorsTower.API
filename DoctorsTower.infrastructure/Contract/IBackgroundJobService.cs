using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.infrastructure.Contract
{
    public interface IBackgroundJobService
    {
        void NotifyDoctor(int appointmentId);
        void EnqueueDoctorNotification(int appointmentId);

        void SendAppointmentReminder(int appointmentId);

        void DeleteExpiredAppointments();
    }
}
