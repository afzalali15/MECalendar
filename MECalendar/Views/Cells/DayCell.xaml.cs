// ####################
// DayCell.xaml.cs
// 3/29/2018
// Afzal
// ####################
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CalendarView
{
    public partial class DayCell : ContentView
    {
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

        double _fontSize;
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                lbl_day.FontSize = _fontSize;
            }
        }

        public DayCell()
        {
            InitializeComponent();
        }
    }
}
