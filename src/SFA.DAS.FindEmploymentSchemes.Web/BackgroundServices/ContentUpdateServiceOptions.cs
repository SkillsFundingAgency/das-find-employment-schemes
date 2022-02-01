namespace SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices
{
    public class ContentUpdateServiceOptions
    {
        public bool Enabled { get; set; }
        /// <summary>
        /// for debugging : "* * * * *"             once a minute
        /// at            : "3-58/5 7-17 * * *"     every five minutes from 7:03 to 17:58
        /// test          : "*/5 8-18 * * *"        every five minutes from 8:00 to 18:55
        /// test2         : "0,30 6-23 * * *"       every half hour from 6:00 to 23:30
        /// pp/prod       : "0,30 6-23 * * *"       every half hour from 6:00 to 23:30
        /// </summary>
        public string? CronSchedule { get; set; }
    }
}
