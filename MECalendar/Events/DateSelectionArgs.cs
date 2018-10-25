// ####################
// DateSelectionArgs.cs
// 4/1/2018
// Afzal
// ####################
using System;
using System.Collections.Generic;

namespace CalendarView
{
    public class DateSelectionArgs : EventArgs
    {
        public DateTime SelectedDate { get; set; }
        public List<CalendarEvent> Events { get; set; }
    }
}
