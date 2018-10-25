# MECalendar
CalendarView with Gregorian, Hijri, and Arabic calendar support. Specailly designed for Middle East languages. Can be used globally for Gregorian calendar

## Installation
Install [MECalendar](https://www.nuget.org/packages/MECalendar/) NuGet package on Portable/Shared project.

## Usage
CalendarView can be used in any Layout like below:
```
        <calendar:CalendarView
            CalendarType="ArabicGregorian"
            StartDayOfWeek="Sunday"
            WeekendDays="Friday,Saturday"
            DayColor="Black"
            WeekendDayColor="Gray"
            SelectedDayColor="Red" />
```
Don't forget to mention reference 
```
    xmlns:calendar="clr-namespace:CalendarView;assembly=mecalendar"
```
