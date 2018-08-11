namespace Lazztech.Pomodoro
{
    public class Settings
    {
        //Number of minutes
        public int WorkTime { get; set; }
        //Number of minutes
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int WorkBlocksTillLongBreak { get; set; }
        public int FlatHourlyRate { get; set; }

        public Settings()
        {
            WorkTime = 25;
            ShortBreakTime = 5;
            LongBreakTime = 15;
            WorkBlocksTillLongBreak = 20;
            FlatHourlyRate = 60;
        }
    }
}