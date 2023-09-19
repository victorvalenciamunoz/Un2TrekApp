namespace Un2TrekApp.Converters;

public class FlyoutItemTitleToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            if (value is string)
            {
                if (value.Equals("Noticias"))
                {
                    var p = ImageSource.FromFile("newspaper.png");
                    return ImageSource.FromFile("newspaper.png");
                }
                if (value.Equals("Trekis"))
                {
                    return ImageSource.FromFile("explore.png");
                }
            }
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
