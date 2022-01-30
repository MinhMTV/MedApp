using SQLite;
using System;
using System.Collections.Generic;

namespace App1.Models
{
    public class TrainingSession
    {
        [PrimaryKey, AutoIncrement]
        public int SessionId { get; set; }
        public DateTime SessionDate { get; set; }
        [Indexed]
        public int UserID { get; set; }  //UserID From User
        public int NrOfGoodCorrectImages { get; set; }
        public int NrOfGoodWrongImages { get; set; }
        public int NrOfBadCorrectImages { get; set; }
        public int NrOfBadWrongImages { get; set; }

        public int NrOfAllImages => NrOfGoodCorrectImages + NrOfGoodWrongImages + NrOfBadCorrectImages + NrOfBadWrongImages;

        public int NrOfCorrectImages => NrOfGoodCorrectImages + NrOfBadCorrectImages;
        public int NrOfWrongImages => NrOfGoodWrongImages + NrOfBadWrongImages;

        public long SessionTimeTicks { get; set; } //Time for finishing TrainingSession as Ticks
        public string ElapsedTime { get; set; } //Time for finishing TrainingSession (conversion from Ticks to string for better reading) min,sec,ms
        public long AvgTTicks { get; set; }//average time for Pic as Ticks

        public string AvgT { get; set; }//average time for Pic in min,sec,ms



        public long AvgTGPicTicks { get; set; }//average time for good pics as Ticks

        public string AvgTGPic { get; set; } //average time for good pics in min,sec,ms



        public long AvgTBPicTicks { get; set; }//average time for Bad pics as Ticks

        public string AvgTBPic { get; set; } //average time for bad pics in min,sec,ms



        public long AvgTCPicTicks { get; set; }//average time for correct Pics in Ticks
        public string AvgTCPic { get; set; } //average time for correct pics in min,sec,ms



        public long AvgTWPicTicks { get; set; }//average time for wrong pics as Ticks
        public string AvgTWPic { get; set; } //average time for wrong pics in min,sec,ms



        public long AvgTGAndCPicTicks { get; set; }//average time for good pics and correct pics as Ticks

        public string AvgTGAndCPic { get; set; } //average time for good pics and correct pics in min,sec,ms



        public long AvgTBAndCPicTicks { get; set; }//average time for bad pics and correct pics as Ticks

        public string AvgTBAndCPic { get; set; } //average time for bad pics and correct pics in min,sec,ms



        public long AvgTGAndWPicTicks { get; set; }//average time for good pics and wrong pics as Ticks

        public string AvgTGAndWPic { get; set; } //average time for good pics and wrong pics in min,sec,ms



        public long AvgTBAndWPicTicks { get; set; }//average time for bad pics and wrong pics as Ticks

        public string AvgTBAndWPic { get; set; } //average time for bad pics and wrong pics in min,sec,ms



        public bool IsTrainigQuit { get; set; } //true, if training was quitted and user went back to menu, false he completed the training and end the training before Time! 

        public bool IsTrainingCompleted { get; set; } // true, if training was completet, false if he quit the Training before time End! 

        //We got 3 (4 but, one status should always be false) scopes
        //1. IsTrainigQuit true , IsTrainingCompleted true - Scope cant be true, because user cant quit session and complet the training
        //2. IsTrainigQuit true , IsTrainingCompleted false - Scope is, User quitted the Session without seing the results
        //3. IsTrainigQuit false , IsTrainingCompleted false - Scope is, User didnt quit and didnt completed the session, he ended it before time!
        //4. IsTrainigQuit false , IsTrainingCompleted true - Scope is, User completed the Session in Time and didnt quit the Session!

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
