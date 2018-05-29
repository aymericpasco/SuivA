using System;

namespace SuivA.Data.Utility
{
    public class StatisticUtility
    {
        public class DoctorStatistic
        {
            public int NumberVisitsThisWeek { get; set; }
            public int NumberVisitsLastWeek { get; set; }
            public int NumberVisitsThisMonth { get; set; }
            public int NumberVisitsLastMonth { get; set; }
            public int NumberVisitsThisYear { get; set; }
        }

        public class VisitorStatistic
        {
            public int NumberVisitsThisWeek { get; set; }
            public int NumberVisitsThisMonth { get; set; }
            public TimeSpan TotalTimeSpentThisWeek { get; set; }
            public TimeSpan TotalTimeSpentLastWeek { get; set; }
            public TimeSpan TotalTimeSpentThisMonth { get; set; }
            public TimeSpan AverageWaitingTime { get; set; }
        }
    }
}