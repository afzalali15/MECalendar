// ####################
// CalendarView.xaml.cs
// 3/27/2018
// Afzal
// ####################
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;

namespace CalendarView
{
    public partial class CalendarView : ContentView, INotifyPropertyChanged
    {
        #region Properties
        //static Color DayColor = Color.Black;
        //static Color WeekendDayColor = DayColor.MultiplyAlpha(0.5);
        //static Color SelectedDayColor = Color.Red;
        //static Color SelectedDayBGColor = Color.FromRgb(51, 153, 255);

        public Color DayColor
        {
            get { return (Color)GetValue(DayColorProperty); }
            set { SetValue(DayColorProperty, value); }
        }

        public static BindableProperty DayColorProperty = BindableProperty.Create(
            nameof(DayColor),
            typeof(Color),
            typeof(CalendarView),
            Color.Black,
            propertyChanged: DayColorChanged
        );

        static void DayColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CalendarView)bindable;
            control.WeekendDayColor = ((Color)newValue).MultiplyAlpha(0.5);
        }

        public Color WeekendDayColor
        {
            get { return (Color)GetValue(WeekendDayColorProperty); }
            set { SetValue(WeekendDayColorProperty, value); }
        }

        public static BindableProperty WeekendDayColorProperty = BindableProperty.Create(
            nameof(WeekendDayColor),
            typeof(Color),
            typeof(CalendarView),
            Color.Black.MultiplyAlpha(0.5)
        );

        public Color SelectedDayColor
        {
            get { return (Color)GetValue(SelectedDayColorProperty); }
            set { SetValue(SelectedDayColorProperty, value); }
        }

        public static BindableProperty SelectedDayColorProperty = BindableProperty.Create(
            nameof(SelectedDayColor),
            typeof(Color),
            typeof(CalendarView),
            Color.Red
        );

        public Color SelectedDayBGColor
        {
            get { return (Color)GetValue(SelectedDayBGColorProperty); }
            set { SetValue(SelectedDayBGColorProperty, value); }
        }

        public static BindableProperty SelectedDayBGColorProperty = BindableProperty.Create(
            nameof(SelectedDayBGColor),
            typeof(Color),
            typeof(CalendarView),
            Color.FromRgb(51, 153, 255)
        );

        public CalendarType CalendarType
        {
            get { return (CalendarType)GetValue(CalendarTypeProperty); }
            set { SetValue(CalendarTypeProperty, value); }
        }

        public static BindableProperty CalendarTypeProperty = BindableProperty.Create(
            nameof(CalendarType),
            typeof(CalendarType),
            typeof(CalendarView),
            CalendarType.Gregorian,
            propertyChanged: CalendarTypeChanged
        );

        static void CalendarTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CalendarView)bindable;
            control.CalendarType = (CalendarType)newValue;
            control.InitializeCalendarView();
        }

        public string WeekendDays
        {
            get { return (string)GetValue(WeekendDaysProperty); }
            set { SetValue(WeekendDaysProperty, value); }
        }

        public static BindableProperty WeekendDaysProperty = BindableProperty.Create(
            nameof(WeekendDays),
            typeof(string),
            typeof(CalendarView),
            DayOfWeek.Sunday.ToString(),
            propertyChanged: WeekendDaysChanged
        );

        static void WeekendDaysChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CalendarView)bindable;
            control.WeekendDays = (string)newValue;
        }

        public static BindableProperty EventsProperty = BindableProperty.Create(
            nameof(Events),
            typeof(List<CalendarEvent>),
            typeof(CalendarView),
            new List<CalendarEvent>()
        );
        static void EventsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CalendarView)bindable;
            control.Events = newValue as List<CalendarEvent>;
        }

        public List<CalendarEvent> Events
        {
            get { return (List<CalendarEvent>)GetValue(EventsProperty); }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SetValue(EventsProperty, value);
                    switch (CalendarType)
                    {
                        case CalendarType.Gregorian:
                            PopulateGregorianCalendar(0);
                            break;
                        case CalendarType.Hijri:
                            PopulateHijriCalendar(0);
                            break;
                        case CalendarType.ArabicGregorian:
                            PopulateArabicGregCalendar(0);
                            break;
                        default:
                            PopulateGregorianCalendar(0);
                            break;
                    }
                });
            }
        }

        public static BindableProperty StartDayOfWeekProperty = BindableProperty.Create(
            nameof(StartDayOfWeek),
            typeof(DayOfWeek),
            typeof(CalendarView),
            DayOfWeek.Monday,
            propertyChanged: StartDayChanged
        );

        private static void StartDayChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CalendarView)bindable;
            control.StartDayOfWeek = (DayOfWeek)newValue;
        }

        public DayOfWeek StartDayOfWeek
        {
            get { return (DayOfWeek)GetValue(StartDayOfWeekProperty); }
            set
            {
                SetValue(StartDayOfWeekProperty, value);
            }
        }

        public static BindableProperty InitialDateProperty = BindableProperty.Create(
            nameof(InitialDate),
            typeof(DateTime),
            typeof(CalendarView),
            DateTime.Now,
            propertyChanged: InitialDateChanged
        );

        private static void InitialDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CalendarView)bindable;
            control.InitialDate = (DateTime)newValue;
        }

        public DateTime InitialDate
        {
            get { return (DateTime)GetValue(InitialDateProperty); }
            set
            {
                SetValue(InitialDateProperty, value);
                CurrentDateInfo = value;
                switch (CalendarType)
                {
                    case CalendarType.Gregorian:
                        PopulateGregorianCalendar(0);
                        break;
                    case CalendarType.Hijri:
                        PopulateHijriCalendar(0);
                        break;
                    case CalendarType.ArabicGregorian:
                        PopulateArabicGregCalendar(0);
                        break;
                    default:
                        PopulateGregorianCalendar(0);
                        break;
                }
            }
        }

        public event EventHandler<DateSelectionArgs> OnDateSelected;
        DateTime _currentDateInfo;
        public DateTime CurrentDateInfo
        {
            get { return _currentDateInfo; }
            set
            {
                _currentDateInfo = value;
            }
        }

        string _todayLabel;
        public string TodayLabel
        {
            get { return _todayLabel; }
            set
            {
                _todayLabel = value;
                OnPropertyChanged(nameof(TodayLabel));
            }
        }

        int monthYearInfoCol = 0;
        public int MonthYearInfoCol
        {
            get { return monthYearInfoCol; }
            set
            {
                monthYearInfoCol = value;
                OnPropertyChanged(nameof(MonthYearInfoCol));
            }
        }

        int monthActionCol = 1;
        public int MonthActionCol
        {
            get { return monthActionCol; }
            set
            {
                monthActionCol = value;
                OnPropertyChanged(nameof(MonthActionCol));
            }
        }

        LayoutOptions monthYearAlighment;
        public LayoutOptions MonthYearAlighment
        {
            get { return monthYearAlighment; }
            set
            {
                monthYearAlighment = value;
                OnPropertyChanged(nameof(MonthYearAlighment));
            }
        }

        LayoutOptions monthActionAlighment;
        public LayoutOptions MonthActionAlighment
        {
            get { return monthActionAlighment; }
            set
            {
                monthActionAlighment = value;
                OnPropertyChanged(nameof(MonthActionAlighment));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        HashSet<int> cellIndexesToClean;
        HashSet<int> columnIndexOfWeekend;
        int indexOfLastSelectedDate;
        Color colorOfLastSelectedDate;
        #endregion

        protected override void OnParentSet()
        {
            base.OnParentSet();
            InitializeCalendarView();
        }

        void InitializeCalendarView()
        {
            #region WeekDays
            var weekendDays = WeekendDays.Split(',');
            foreach (var item in weekendDays)
            {
                var dayString = item.Trim();
                try
                {
                    var day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayString);
                    columnIndexOfWeekend.Add(GetFirstDateIndex(day));
                }
                catch (ArgumentException argEx)
                {
                    throw new Exception("Please provide correct DayOfWeek for WeekendDays property.");
                }
            }
            #endregion

            for (int i = 0; i < 7; i++)
            {

                switch (CalendarType)
                {
                    case CalendarType.Gregorian:
                        var dayLabel = grd_days.Children.ElementAt(i) as DayCell;
                        dayLabel.Day = Enum.GetName(typeof(DayOfWeek), ((int)StartDayOfWeek + i) % 7).Substring(0, 1);
                        if (columnIndexOfWeekend.Contains(i))
                            dayLabel.Color = WeekendDayColor;
                        else
                            dayLabel.Color = DayColor;
                        break;
                    case CalendarType.Hijri:
                        var dayLabelAr = grd_days.Children.ElementAt(6 - i) as DayCell;
                        dayLabelAr.Day = Enum.GetName(typeof(ArabicDayOfWeek), ((int)StartDayOfWeek + i) % 7);
                        if (columnIndexOfWeekend.Contains(i))
                            dayLabelAr.Color = WeekendDayColor;
                        else
                            dayLabelAr.Color = DayColor;
                        break;
                    case CalendarType.ArabicGregorian:
                        var dayLabelArGreg = grd_days.Children.ElementAt(6 - i) as DayCell;
                        dayLabelArGreg.Day = Enum.GetName(typeof(ArabicDayOfWeek), ((int)StartDayOfWeek + i) % 7);
                        if (columnIndexOfWeekend.Contains(i))
                            dayLabelArGreg.Color = WeekendDayColor;
                        else
                            dayLabelArGreg.Color = DayColor;
                        break;
                    default:
                        var dayLabelEn = grd_days.Children.ElementAt(i) as DayCell;
                        dayLabelEn.Day = Enum.GetName(typeof(DayOfWeek), ((int)StartDayOfWeek + i) % 7).Substring(0, 1);
                        if (columnIndexOfWeekend.Contains(i))
                            dayLabelEn.Color = WeekendDayColor;
                        else
                            dayLabelEn.Color = DayColor;
                        break;
                }
            }

            switch (CalendarType)
            {
                case CalendarType.Gregorian:
                    PopulateGregorianCalendar(0);
                    MonthYearInfoCol = 0;
                    MonthActionCol = 1;
                    MonthYearAlighment = LayoutOptions.Start;
                    MonthActionAlighment = LayoutOptions.End;
                    TodayLabel = "Today";
                    break;
                case CalendarType.Hijri:
                    PopulateHijriCalendar(0);
                    MonthYearInfoCol = 1;
                    MonthActionCol = 0;
                    MonthYearAlighment = LayoutOptions.End;
                    MonthActionAlighment = LayoutOptions.Start;
                    TodayLabel = "اليوم";
                    break;
                case CalendarType.ArabicGregorian:
                    PopulateArabicGregCalendar(0);
                    MonthYearInfoCol = 1;
                    MonthActionCol = 0;
                    MonthYearAlighment = LayoutOptions.End;
                    MonthActionAlighment = LayoutOptions.Start;
                    TodayLabel = "اليوم";
                    break;
                default:
                    PopulateGregorianCalendar(0);
                    break;
            }

            //var arg = new DateSelectionArgs();
            //arg.SelectedDate = DateTime.Today;
            //arg.Events = Events.Where(events => events.EventDate.Equals(DateTime.Today)).ToList();
            //OnDateSelected?.Invoke(null, arg);

            //reset last selected info
            indexOfLastSelectedDate = 0;
            colorOfLastSelectedDate = Color.Default;
        }

        public CalendarView()
        {
            InitializeComponent();
            this.BindingContext = this;
            cellIndexesToClean = new HashSet<int>();
            columnIndexOfWeekend = new HashSet<int>();
            CurrentDateInfo = InitialDate;

            var arabicDate = DateTime.Now.ToString("dd/MMMM/yyyy", new CultureInfo("ar-SA").DateTimeFormat);
            Debug.WriteLine(arabicDate.ToString());
        }

        void PopulateGregorianCalendar(int monthsToAdd)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CurrentDateInfo = CurrentDateInfo.AddMonths(monthsToAdd);
            var firstDayOfMonth = new DateTime(CurrentDateInfo.Year, CurrentDateInfo.Month, 1);
            var index = GetFirstDateIndex(firstDayOfMonth.DayOfWeek);

            lbl_month.Text = CurrentDateInfo.ToString("MMMM yyyy");
            var daysInMonth = DateTime.DaysInMonth(CurrentDateInfo.Year, CurrentDateInfo.Month);

            for (int i = 0; i < index; i++)
                ClearCellAt(i);

            for (int i = index + daysInMonth; i < 42; i++)
                ClearCellAt(i);

            foreach (var i in cellIndexesToClean)
                ClearCellAt(i);
            cellIndexesToClean.Clear();

            for (int i = 1; i <= daysInMonth; i++)
            {
                var calendarCell = grd_calendar.Children.ElementAt(index) as CalendarCell;

                calendarCell.GestureRecognizers.Clear();
                calendarCell.GestureRecognizers.Add(new TapGestureRecognizer((View arg1, object arg2) =>
                {
                    if (indexOfLastSelectedDate != 0)
                    {
                        var prevSelectedCell = grd_calendar.Children.ElementAt(indexOfLastSelectedDate) as CalendarCell;
                        prevSelectedCell.Color = colorOfLastSelectedDate;
                    }
                    var arg = new DateSelectionArgs();
                    arg.SelectedDate = (arg1 as CalendarCell).Date;
                    arg.Events = (arg1 as CalendarCell).Events;
                    colorOfLastSelectedDate = (arg1 as CalendarCell).Color;
                    indexOfLastSelectedDate = grd_calendar.Children.IndexOf(arg1 as CalendarCell);
                    (arg1 as CalendarCell).Color = SelectedDayColor;
                    OnDateSelected?.Invoke(arg1, arg);
                }));

                calendarCell.Day = i.ToString();

                #region Event Assignment
                calendarCell.Date = firstDayOfMonth;
                var eventForThisDay = Events.Where(evnt => evnt.EventDate == firstDayOfMonth);
                for (int j = 0; j < eventForThisDay.Count(); j++)
                {
                    calendarCell.Events.Add(eventForThisDay.ElementAt(j));
                    cellIndexesToClean.Add(index);
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
                #endregion


                if (columnIndexOfWeekend.Contains(index % 7))
                    calendarCell.Color = WeekendDayColor;
                else
                    calendarCell.Color = DayColor;

                //Highligh current day
                if (new DateTime(calendarCell.Date.Year, calendarCell.Date.Month, calendarCell.Date.Day).Equals(DateTime.Today))
                {
                    calendarCell.BgColor = SelectedDayBGColor;
                    calendarCell.Color = Color.White;
                    cellIndexesToClean.Add(index);
                }

                index++;
            }

            Debug.WriteLine("Time taken to render entire calendar view : " + stopwatch.ElapsedMilliseconds);
        }

        void PopulateHijriCalendar(int monthsToAdd)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CurrentDateInfo = CurrentDateInfo.AddMonths(monthsToAdd);
            var firstHijriDate = Convert.ToInt32(CurrentDateInfo.ToString("dd", new CultureInfo("ar-SA").DateTimeFormat));

            var firstDayOfMonth = CurrentDateInfo.AddDays(-(firstHijriDate - 1));
            var index = GetFirstDateIndex(firstDayOfMonth.DayOfWeek);

            lbl_month.Text = GetArabicNumbers(CurrentDateInfo.ToString("MMMM yyyy", new CultureInfo("ar-SA").DateTimeFormat));
            var daysInMonth = HijriDaysInMonth(firstDayOfMonth);

            //Clear cells which are coming Right side of ArabicDate => 1
            for (int i = 0; i < 7; i++)
                ClearCellAt(i);

            //Clear cells which are coming left side of ArabicDate => lastDayOfMonth
            for (int i = 28; i < 42; i++)
                ClearCellAt(i);

            foreach (var i in cellIndexesToClean)
                ClearCellAt(i);
            cellIndexesToClean.Clear();

            var gregDaysInMonth = DateTime.DaysInMonth(CurrentDateInfo.Year, CurrentDateInfo.Month);
            var tempDateInfo = CurrentDateInfo;
            for (int i = 1; i <= daysInMonth; i++)
            {
                var rowFactor = Math.Floor(index / 7.0);
                var reverseIndex = Convert.ToInt32((7 * rowFactor) + (6 - index % 7));
                var calendarCell = grd_calendar.Children.ElementAt(reverseIndex) as CalendarCell;

                calendarCell.GestureRecognizers.Clear();
                calendarCell.GestureRecognizers.Add(new TapGestureRecognizer((View arg1, object arg2) =>
                {
                    if (indexOfLastSelectedDate != 0)
                    {
                        var prevSelectedCell = grd_calendar.Children.ElementAt(indexOfLastSelectedDate) as CalendarCell;
                        prevSelectedCell.Color = colorOfLastSelectedDate;
                    }
                    var arg = new DateSelectionArgs();
                    arg.SelectedDate = (arg1 as CalendarCell).Date;
                    arg.Events = (arg1 as CalendarCell).Events;
                    colorOfLastSelectedDate = (arg1 as CalendarCell).Color;
                    indexOfLastSelectedDate = grd_calendar.Children.IndexOf(arg1 as CalendarCell);
                    (arg1 as CalendarCell).Color = SelectedDayColor;
                    OnDateSelected?.Invoke(arg1, arg);
                }));

                calendarCell.Day = GetArabicNumbers(i.ToString());

                #region Event Assignmenlbl_montht
                calendarCell.Date = firstDayOfMonth;
                var eventForThisDay = Events.Where(evnt => evnt.EventDate.Date == firstDayOfMonth.Date);
                for (int j = 0; j < eventForThisDay.Count(); j++)
                {
                    calendarCell.Events.Add(eventForThisDay.ElementAt(j));
                    cellIndexesToClean.Add(reverseIndex);
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
                #endregion

                if (columnIndexOfWeekend.Contains(index % 7))
                    calendarCell.Color = WeekendDayColor;
                else
                    calendarCell.Color = DayColor;

                //Highligh current day
                if (new DateTime(calendarCell.Date.Year, calendarCell.Date.Month, calendarCell.Date.Day).Equals(DateTime.Today))
                {
                    calendarCell.BgColor = SelectedDayBGColor;
                    calendarCell.Color = Color.White;
                    cellIndexesToClean.Add(reverseIndex);
                }

                index++;
            }

            Debug.WriteLine("Time taken to render entire calendar view : " + stopwatch.ElapsedMilliseconds);
        }

        void PopulateArabicGregCalendar(int monthsToAdd)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CurrentDateInfo = CurrentDateInfo.AddMonths(monthsToAdd);
            var firstDayOfMonth = new DateTime(CurrentDateInfo.Year, CurrentDateInfo.Month, 1);
            var index = GetFirstDateIndex(firstDayOfMonth.DayOfWeek);

            var monthText = CurrentDateInfo.ToString("MMMM yyyy", new CultureInfo("ar-AE").DateTimeFormat);
            lbl_month.Text = GetArabicNumbers(monthText);
            var daysInMonth = DateTime.DaysInMonth(CurrentDateInfo.Year, CurrentDateInfo.Month);

            //Clear cells which are coming Right side of ArabicDate => 1
            for (int i = 0; i < 7; i++)
                ClearCellAt(i);

            //Clear cells which are coming left side of ArabicDate => lastDayOfMonth
            for (int i = 28; i < 42; i++)
                ClearCellAt(i);

            foreach (var i in cellIndexesToClean)
                ClearCellAt(i);
            cellIndexesToClean.Clear();

            var gregDaysInMonth = DateTime.DaysInMonth(CurrentDateInfo.Year, CurrentDateInfo.Month);
            var tempDateInfo = CurrentDateInfo;
            for (int i = 1; i <= daysInMonth; i++)
            {
                var rowFactor = Math.Floor(index / 7.0);
                var reverseIndex = Convert.ToInt32((7 * rowFactor) + (6 - index % 7));
                var calendarCell = grd_calendar.Children.ElementAt(reverseIndex) as CalendarCell;

                calendarCell.GestureRecognizers.Clear();
                calendarCell.GestureRecognizers.Add(new TapGestureRecognizer((View arg1, object arg2) =>
                {
                    if (indexOfLastSelectedDate != 0)
                    {
                        var prevSelectedCell = grd_calendar.Children.ElementAt(indexOfLastSelectedDate) as CalendarCell;
                        prevSelectedCell.Color = colorOfLastSelectedDate;
                    }
                    var arg = new DateSelectionArgs();
                    arg.SelectedDate = (arg1 as CalendarCell).Date;
                    arg.Events = (arg1 as CalendarCell).Events;
                    colorOfLastSelectedDate = (arg1 as CalendarCell).Color;
                    indexOfLastSelectedDate = grd_calendar.Children.IndexOf(arg1 as CalendarCell);
                    (arg1 as CalendarCell).Color = SelectedDayColor;
                    OnDateSelected?.Invoke(arg1, arg);
                }));

                calendarCell.Day = GetArabicNumbers(i.ToString());

                #region Event Assignment
                calendarCell.Date = firstDayOfMonth;
                var eventForThisDay = Events.Where(evnt => evnt.EventDate.Date == firstDayOfMonth.Date);
                for (int j = 0; j < eventForThisDay.Count(); j++)
                {
                    calendarCell.Events.Add(eventForThisDay.ElementAt(j));
                    cellIndexesToClean.Add(reverseIndex);
                }
                firstDayOfMonth = firstDayOfMonth.AddDays(1);
                #endregion

                if (columnIndexOfWeekend.Contains(index % 7))
                    calendarCell.Color = WeekendDayColor;
                else
                    calendarCell.Color = DayColor;

                //Highligh current day
                if (new DateTime(calendarCell.Date.Year, calendarCell.Date.Month, calendarCell.Date.Day).Equals(DateTime.Today))
                {
                    calendarCell.BgColor = SelectedDayBGColor;
                    calendarCell.Color = Color.White;
                    cellIndexesToClean.Add(reverseIndex);
                }

                index++;
            }

            Debug.WriteLine("Time taken to render entire calendar view : " + stopwatch.ElapsedMilliseconds);
        }

        int GetFirstDateIndex(DayOfWeek dayOfWeek)
        {
            var diffInStartDay = (int)dayOfWeek - (int)StartDayOfWeek;
            if (diffInStartDay < 0)
                return 7 - Math.Abs(diffInStartDay);
            else
                return diffInStartDay;
        }

        void ClearCellAt(int index)
        {
            var calendarCell = grd_calendar.Children.ElementAt(index) as CalendarCell;
            calendarCell.GestureRecognizers.Clear();
            calendarCell.Clear();
        }

        int HijriDaysInMonth(DateTime firstDate)
        {
            var monthsLaterDate = firstDate.AddDays(29);
            var lastHijriDate = Convert.ToInt32(monthsLaterDate.ToString("dd", new CultureInfo("ar-SA").DateTimeFormat));
            if (lastHijriDate == 30)
                return lastHijriDate;
            else
                return 29;
        }

        string GetArabicNumbers(string englishNumber)
        {
            return englishNumber.Replace('0', '\u0660')
                    .Replace('1', '\u0661')
                    .Replace('2', '\u0662')
                    .Replace('3', '\u0663')
                    .Replace('4', '\u0664')
                    .Replace('5', '\u0665')
                    .Replace('6', '\u0666')
                    .Replace('7', '\u0667')
                    .Replace('8', '\u0668')
                    .Replace('9', '\u0669');
        }

        void Handle_NextClicked(object sender, System.EventArgs e)
        {
            switch (CalendarType)
            {
                case CalendarType.Gregorian:
                    PopulateGregorianCalendar(1);
                    break;
                case CalendarType.Hijri:
                    PopulateHijriCalendar(-1);
                    break;
                case CalendarType.ArabicGregorian:
                    PopulateArabicGregCalendar(-1);
                    break;
                default:
                    PopulateGregorianCalendar(1);
                    break;
            }
        }

        void Handle_TodayClicked(object sender, System.EventArgs e)
        {
            var monthDiff = -(CurrentDateInfo - DateTime.Now).Days / 30;
            if (monthDiff == 0)
                return;
            switch (CalendarType)
            {
                case CalendarType.Gregorian:
                    PopulateGregorianCalendar(monthDiff);
                    break;
                case CalendarType.Hijri:
                    PopulateHijriCalendar(monthDiff);
                    break;
                case CalendarType.ArabicGregorian:
                    PopulateArabicGregCalendar(monthDiff);
                    break;
                default:
                    PopulateGregorianCalendar(monthDiff);
                    break;
            }
        }

        void Handle_PreviousClicked(object sender, System.EventArgs e)
        {
            switch (CalendarType)
            {
                case CalendarType.Gregorian:
                    PopulateGregorianCalendar(-1);
                    break;
                case CalendarType.Hijri:
                    PopulateHijriCalendar(1);
                    break;
                case CalendarType.ArabicGregorian:
                    PopulateArabicGregCalendar(1);
                    break;
                default:
                    PopulateGregorianCalendar(-1);
                    break;
            }
        }
    }
}
