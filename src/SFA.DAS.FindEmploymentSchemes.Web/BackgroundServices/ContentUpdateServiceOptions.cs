namespace SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices
{
    public class ContentUpdateServiceOptions
    {
        /// <summary>
        /// Whether automatic, timed updates are enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// for debugging : "* * * * *"          once a minute
        /// at,test,test2 : "*/5 7-18 * * *"     every five minutes from 7:00 to 18:55
        /// pp,prod       : "0,30 6-23 * * *"    every half hour from 6:00 to 23:30
        /// </summary>
        public string? CronSchedule { get; set; }

        /// <summary>
        /// Whether manually triggered updates are allowed
        /// </summary>
        //public bool AllowManual { get; set; }
    }
}
