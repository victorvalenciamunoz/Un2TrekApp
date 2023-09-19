using Microsoft.Maui.Controls.Maps;

namespace Un2TrekApp;

public class CustomPin : Pin
{
    public bool IsAddingTreki { get; set; }
    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(CustomPin),
                                propertyChanged: OnImageSourceChanged);

    public ImageSource? ImageSource
    {
        get => (ImageSource?)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public Microsoft.Maui.Maps.IMap? Map { get; set; }

    static async void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomPin)bindable;        
        if (control.Handler?.PlatformView is null)
        {
            // Workaround for when this executes the Handler and PlatformView is null
            control.HandlerChanged += OnHandlerChanged;
            return;
        }
#if IOS
    await control.AddAnnotation();
#else
        await Task.CompletedTask;
#endif
        await Task.CompletedTask;
        void OnHandlerChanged(object? s, EventArgs e)
        {
            OnImageSourceChanged(control, oldValue, newValue);
            control.HandlerChanged -= OnHandlerChanged;
        }
    }
}
