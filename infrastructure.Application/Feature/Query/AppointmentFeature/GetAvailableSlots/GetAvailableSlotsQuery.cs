using DoctorsTower.Application.DTOs;
using MediatR;

namespace DoctorsTower.Application.Feature.Query.AppointmentQuery.GetAvailableSlots
{
    public class GetAvailableSlotsQuery
        : IRequest<IEnumerable<AvailableSlotDTO>?>
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }

        public GetAvailableSlotsQuery(
            int doctorId,
            DateTime date)
        {
            DoctorId = doctorId;
            Date = date;
        }
    }
}