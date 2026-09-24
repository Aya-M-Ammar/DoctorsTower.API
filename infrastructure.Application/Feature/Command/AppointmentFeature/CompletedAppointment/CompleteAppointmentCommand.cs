using MediatR;

namespace DoctorsTower.Application.Feature.Command.Appointment.CompleteAppointment
{
    public class CompleteAppointmentCommand
        : IRequest<bool>
    {
        public int Id { get; set; }

        public CompleteAppointmentCommand(int id)
        {
            Id = id;
        }
    }
}