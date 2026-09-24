using Microsoft.AspNetCore.Mvc;

namespace DoctorsTower.API.Filters
{
    public class CachedAttribute : ServiceFilterAttribute
    { public int DurationInSeconds { get; } 
        public CachedAttribute(int durationInSeconds) : base(typeof(CachedFilter))
        { DurationInSeconds = durationInSeconds; } 
    }
}