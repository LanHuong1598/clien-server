using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientManagement.Areas.Admin.Models
{
    public class MovieCalendarGet
    {
        public Guid LFilmId { get; set; }

        public Guid LDayOfWeekId { get; set; }

        public Guid LMovieDisplayTypeId { get; set; }

        public DateTime LStartWeek { get; set; }

        public Guid LCinemaRoomId { get; set; }
    }
}