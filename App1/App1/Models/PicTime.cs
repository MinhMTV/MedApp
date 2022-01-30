using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App1.Models
{
	public class PicTime
	{
		//Class Which stores all the time per Picture and its pictype for the all specifig(Key) TrainingSession
			[PrimaryKey, AutoIncrement]
			public int PicId { get; set; }

			[Indexed]
			public int SessionID	{ get; set; } //ID from TrainingsSession Model

			[Indexed]
			public int UserID { get; set; } //ID From User

			public long TimeTicks { get; set; } // get time in ticks

			public string Time { get; set; } // get time as string in min,sec,ms

			public PicType Type { get; set; }

			public bool CorrectImage { get; set; }

		
	}
}