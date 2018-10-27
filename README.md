# MECalendar 0.1.1
CalendarView with Gregorian, Hijri, and Arabic calendar support. Specailly designed for Middle East languages. Can be used globally for Gregorian calendar

## Screenshots
### Gregorian Arabic |  Gregorian English |     Hijri
<img src="https://preview.ibb.co/mbVEaq/Arabic-Greg.png" width="200" height="200" /><img src="https://preview.ibb.co/mqQSTA/Greg.png" width="200" height="200" /><img src="https://preview.ibb.co/mPeRMV/Arabic-Hijri.png" width="200" height="200" />

## Installation
Install [MECalendar](https://www.nuget.org/packages/MECalendar/) NuGet package on Portable/Shared project.<br/>
*Note: Xamarin Forms 3.2+ is required to install this package.*

## Usage
CalendarView can be used in any Layout like below:
```xml
<calendar:CalendarView
        CalendarType="ArabicGregorian"
        StartDayOfWeek="Sunday"
        WeekendDays="Friday,Saturday"
        DateSelected="{Binding DateSelectedCommand}"
        MonthChanged="{Binding MonthChangedCommand}"
        DayColor="Black"
        WeekendDayColor="Gray"
        SelectedDayColor="Red" />
```
Don't forget to mention reference 
```xml
xmlns:calendar="clr-namespace:CalendarView;assembly=MECalendar"
```
## Documentation
Refer [Wiki](https://github.com/afzalali15/MECalendar/wiki) for detailed documentation about the changes.
