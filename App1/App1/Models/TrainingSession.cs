using SQLite;
using System;

namespace App1.Models
{
    public class TrainingSession
    {
        [PrimaryKey, AutoIncrement]
        public int SessionId { get; set; }
        public DateTime SessionDate { get; set; }
        public int PatientId { get; set; }
        public int NrOfGoodCorrectImages { get; set; }
        public int NrOfGoodWrongImages { get; set; }
        public int NrOfBadCorrectImages { get; set; }
        public int NrOfBadWrongImages { get; set; }
        public string ElapsedTime { get; set; }
        public double AverageReactionTime { get; set; }
        public bool IsTrainingCompleted { get; set; }
        public bool IsDataSent { get; set; }

        public TrainingSession() { }
        public TrainingSession(DateTime dateTime)
        {
            this.SessionDate = dateTime.Date;
            this.SessionId = 0;
            this.NrOfGoodCorrectImages = 0;
            this.NrOfBadCorrectImages = 0;
            this.NrOfBadWrongImages = 0;
            this.ElapsedTime = "00:00";
        }


    }
}
