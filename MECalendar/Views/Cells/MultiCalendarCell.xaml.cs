using System;
using System.Collections.Generic;
using System.Linq;
using CalendarTest.Views;
using CalendarView;
using MECalendar.Models;
using Xamarin.Forms;

namespace CalendarView
{
    public partial class MultiCalendarCell : ContentView
    {
        public DateTime Date
        {
            get;
            set;
        }

        string _day;
        public string Day
        {
            get { return _day; }
            set
            {
                _day = value;
                lbl_day.Text = _day;
            }
        }

        string _secondaryDay;
        public string SecondaryDay
        {
            get { return _secondaryDay; }
            set
            {
                _secondaryDay = value;
                lbl_secondary_day.Text = _secondaryDay;
            }
        }

        XList<CalendarEvent> _events;
        public XList<CalendarEvent> Events
        {
            get { return _events; }
            set
            {
                _events = value;
            }
        }

        Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                lbl_day.TextColor = _color;
            }
        }

        Color _bgColor;
        public Color BgColor
        {
            get { return _bgColor; }
            set
            {
                _bgColor = value;
                DateBackground.BackgroundColor = _bgColor;
            }
        }

        public MultiCalendarCell()
        {
            InitializeComponent();
            _events = new XList<CalendarEvent>();
            Events.OnAdd += l_OnAdd;
            Events.OnClear += l_OnClear;
        }

        void l_OnClear(object sender, EventArgs e)
        {
            lyt_event_indicator.Children.Clear();
        }

        void l_OnAdd(object sender, EventArgs e)
        {
            if (!lyt_event_indicator.Children.Any())
                lyt_event_indicator.Children.Add(new EventIndicator());
        }

        public void Clear()
        {
            lbl_day.Text = "";
            lbl_secondary_day.Text = "";
            DateBackground.BackgroundColor = Color.Default;
            Events.Clear();
        }
    }
}
