using DoctorsTower.Application.DTOs;
using MediatR;

namespace DoctorsTower.Application.Feature.Query.AppointmentQuery.GetAvailableSlots
{
    public class GetAvailableSlotsQuery
        : IRequest<IEnumerable<AvailableSlotDTO>?>
    {
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }

        public GetAvailableSlotsQuery(
            int doctorId,
            DateOnly date)
        {
            DoctorId = doctorId;
            Date = date;
        }
    }
}