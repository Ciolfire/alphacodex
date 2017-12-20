using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AlphaCodex.Tools
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    class NullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static NullToVisibilityConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new NullToVisibilityConverter();
            }
            return _converter;
        }
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                if (parameter.ToString() == "inverted")
                {
                    if (value != null && !string.IsNullOrEmpty(value as string))
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                }
                else return null;
            }
            else
            {
                if (value != null && !string.IsNullOrEmpty(value as string))
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