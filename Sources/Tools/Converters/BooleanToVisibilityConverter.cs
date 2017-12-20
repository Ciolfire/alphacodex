using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AlphaCodex.Tools
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static BooleanToVisibilityConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new BooleanToVisibilityConverter();
            }
            return _converter;
        }
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "inverted":
                        if (((bool)value))
                        {
                            return Visibility.Collapsed;
                        }
                        else
                        {
                            return Visibility.Visible;
                        }
                    case "hidden":
                        if (((bool)value))
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Hidden;
                        }
                    default:
                        return Visibility.Hidden;
                }
            }
            else
            {
                if (((bool)value))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
