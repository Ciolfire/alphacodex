using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AlphaCodex.Tools
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class InvertedBooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static InvertedBooleanToVisibilityConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new InvertedBooleanToVisibilityConverter();
            }
            return _converter;
        }
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((bool)value))
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
