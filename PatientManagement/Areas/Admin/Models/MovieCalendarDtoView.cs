using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientManagement.Areas.Admin.Models
{
    public class MovieCalendarDtoView
    {
        public Guid Id { get; set; }

        public string FilmName { get; set; }

        public DateTime StartWeek { get; set; }

        public List<MovieCalendarDayOfWeek> DayOfWeeks { get; set; }
    }

    public class MovieCalendarDayOfWeek
    {
        public string Day { get; set; }

        public List<MovieCalendarMovieType> Type { get; set; }
    }

    public class MovieCalendarMovieType
    {
        public string Type { get; set; }

        //Film id, DayOf
        public IdentityMovieCalendar IdObj { get; set; }

        public List<string> Time { get; set; }
    }

    public class IdentityMovieCalendar
    {
        public Guid FilmId { get; set; }

        public DateTime StartWeek { get; set; }

        public Guid CinemaRoomId { get; set; }

        public Guid DayOfWeekId { get; set; }

        public Guid MovieDisplayTypeId { get; set; }
    }
}