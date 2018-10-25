// ####################
// CalendarEvent.cs
// 4/1/2018
// Afzal
// ####################
using System;
namespace CalendarView
{
    public class CalendarEvent
    {
        public int EventID
        {
            get;
            set;
        }
        public string EventName
        {
            get;
            set;
        }
        public string EventDescription
        {
            get;
            set;
        }
        public DateTime EventDate
        {
            get;
            set;
        }
    }
}
