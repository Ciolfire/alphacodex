using AlphaCodex.Data;
using AlphaCodex.Models;
using AlphaCodex.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AlphaCodex.Tools
{
    public static class JadeTools
    {
        enum Type
        {
            feat
        }

        #region Fields
        // colors
        public static class Colors
        {
            public static Brush TitleBlue = (Brush)(new BrushConverter().ConvertFrom("#FF003364"));

        }
        #endregion

        #region Methods
        public static Hyperlink CreateLink(Link link)
        {

            LinkViewModel linkVM = new LinkViewModel(link);
            Hyperlink linkH = new Hyperlink();
            linkH.Inlines.Add(linkVM.DisplayName);
            linkH.DataContext = linkVM;
            linkH.Command = linkVM.NavigateCommand;
            return linkH;
        }

        /// <summary>
        /// Create a dataTable based on the columns 
        /// </summary>
        public static DataTable CreateTable(List<Column> columns)
        {
            var table = new DataTable();
            // create the columns
            foreach (Column column in columns)
                table.Columns.Add(column.Header);
            // create the rows
            List<string> rows = new List<string>();
            for (int i = 0; i < columns[0].Rows.Count; i++)
            {
                rows.Clear();
                foreach (Column column in columns)
                {
                    rows.Add(column.Rows.ElementAt(i));
                }
                table.Rows.Add(rows.ToArray());
            }
            return table;
        }


        public static Inline ParseString(string toParse)
        {
            string boldPattern = @"(\*{2}.+\*{2})";
            string italicPattern = @"(\~[^~]+\~)";

            if (Regex.IsMatch(toParse, boldPattern))
            {
                return new Bold(new Run(toParse.Trim('*')) { Foreground = Colors.TitleBlue, FontSize = 16 });
            }
            // else check if it's italic
            else if (Regex.IsMatch(toParse, italicPattern))
            {
                return new Italic(new Run(toParse.Trim('~')) { Foreground = Colors.TitleBlue });
            }
            else
                return new Run(toParse);
        }
        /// <summary>
        /// Create the text style
        /// </summary>
        public static void ParseText(string features, string parent, TextBlock target, bool clear=true)
        {
            DatabaseAccess db = null;
            if(clear)
                target.Inlines.Clear();
            if (features == null)
                return;

            string boldPattern = @"(\*{2}.+\*{2})";
            string italicPattern = @"(\~[^~]+\~)";
            string tablePattern = @"(\[{2}[0-9]+\]{2})";

            string[] toSplit = Regex.Split(features, boldPattern + "|" + italicPattern + "|" + tablePattern);
            foreach (string part in toSplit)
            {
                // Check if it's a headline to "bold" it
                if (Regex.IsMatch(part, boldPattern))
                {
                    target.Inlines.Add(new Bold(new Run(part.Trim('*')) { Foreground = Colors.TitleBlue, FontSize = 16 }));
                }
                // else check if it's italic
                else if (Regex.IsMatch(part, italicPattern))
                {
                    target.Inlines.Add(new Italic(new Run(part.Trim('~')) { Foreground = Colors.TitleBlue }));
                }
                // else check if it's a table
                else if (Regex.IsMatch(part, tablePattern))
                {
                    if (db == null)
                    {
                        db = new DatabaseAccess();
                    }
                    var grid = new DataGrid()
                    {
                        ItemsSource = db.GetTable(parent, int.Parse(Regex.Replace(part, "[^0-9]", ""))).DefaultView,
                        Style = Application.Current.FindResource("tableStyle") as Style
                    };
                    target.Inlines.Add(grid);
                }
                // else normal text
                else
                    target.Inlines.Add(part);
            }
        }



        /// <summary>
        /// Used to take a screenshot of a specific UI element
        /// </summary>
        public static BitmapSource CaptureScreen(Visual target, double dpiX, double dpiY)
        {
            if (target == null)
                return null;
            FrameworkElement elem = (FrameworkElement)target;
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(elem.ActualWidth * dpiX / 96.0),
                                                            (int)(elem.ActualHeight * dpiY / 96.0),
                                                            dpiX,
                                                            dpiY,
                                                            PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);
            return rtb;
        }
        #endregion
    }
}
