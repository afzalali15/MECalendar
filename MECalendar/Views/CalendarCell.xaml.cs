// ####################
// CalendarCell.xaml.cs
// 3/27/2018
// Afzal
// ####################
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CalendarTest.Views;

namespace CalendarView
{
    public partial class CalendarCell : ContentView
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

        public CalendarCell()
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
            DateBackground.BackgroundColor = Color.Default;
            Events.Clear();
        }
    }

    public class XList<T> : List<T>
    {
        public event EventHandler OnAdd;
        public event EventHandler OnClear;
        public void Add(T item)
        {
            OnAdd?.Invoke(this, null);
            base.Add(item);
        }

        public void Clear()
        {
            OnClear?.Invoke(this, null);
            base.Clear();
        }
    }
}
