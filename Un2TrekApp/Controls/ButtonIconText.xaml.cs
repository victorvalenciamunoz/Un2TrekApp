using System.Windows.Input;

namespace Un2TrekApp.Controls;

public partial class ButtonIconText : ContentView
{
    public delegate void ButtonIconTextClickedHandler(ButtonIconClickedEventArgs args);

    public event ButtonIconTextClickedHandler ButtonIconTextClicked;

    public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ButtonIconText)bindable;
        control.mainBorder.BackgroundColor = (Color)newValue;
        control.mainBorder.Stroke = new SolidColorBrush((Color)newValue);
    });

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ButtonIconText)bindable;
        control.buttonLabel.TextColor = (Color)newValue;
        control.buttonText.TextColor = (Color)newValue;
    });

    public static readonly BindableProperty IconFontSizeProperty = BindableProperty.Create(nameof(IconFontSize), typeof(double), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ButtonIconText)bindable;
        control.buttonLabel.FontSize = (double)newValue;
    });

    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ButtonIconText)bindable;
        control.buttonLabel.Text = newValue.ToString();
    });

    public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ButtonIconText)bindable;
        control.buttonLabel.FontSize = (double)newValue;
    });
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ButtonIconText)bindable;
        control.buttonText.Text = newValue.ToString();
    });

    public static readonly BindableProperty CustomIsEnabledProperty = BindableProperty.Create(nameof(Text), typeof(bool), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ButtonIconText)bindable;
        control.IsEnabled = (bool)newValue;
    });

    public static readonly BindableProperty CustomCommandProperty = BindableProperty.Create(nameof(CustomCommand), typeof(ICommand), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {
        var control = (ButtonIconText)bindable;
        control.buttonText.Command = (ICommand)newValue;

        control.buttonLabel.GestureRecognizers.Clear();
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Command = (ICommand)newValue;
        control.buttonLabel.GestureRecognizers.Add(tapGesture);
    });

    public static readonly BindableProperty ClickedParametersProperty = BindableProperty.Create(nameof(ClickedParameters), typeof(object), typeof(ButtonIconText), propertyChanged: (bindable, oldValue, newValue) =>
    {


    });

    public ButtonIconText()
    {
        InitializeComponent();
    }

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public double IconFontSize
    {
        get => (double)GetValue(IconFontSizeProperty);
        set => SetValue(IconFontSizeProperty, value);
    }
    public string Icon
    {
        get => GetValue(IconProperty) as string;
        set => SetValue(IconProperty, value);
    }

    public double TextFontSize
    {
        get => (double)GetValue(TextFontSizeProperty);
        set => SetValue(TextFontSizeProperty, value);
    }
    public string Text
    {
        get => GetValue(TextProperty) as string;
        set => SetValue(TextProperty, value);
    }

    public bool CustomIsEnabled
    {
        get => (bool)GetValue(CustomIsEnabledProperty);
        set => SetValue(CustomIsEnabledProperty, value);
    }
    public ICommand CustomCommand
    {
        get => GetValue(CustomCommandProperty) as ICommand;
        set => SetValue(CustomCommandProperty, value);
    }

    public object ClickedParameters
    {
        get => GetValue(ClickedParametersProperty);
        set => SetValue(ClickedParametersProperty, value);
    }
    private void buttonText_Clicked(object sender, EventArgs e)
    {
        if (ButtonIconTextClicked == null) return;
        if (ClickedParameters != null)
        {
            ButtonIconTextClicked(new ButtonIconClickedEventArgs { Parameters = ClickedParameters });
        }
        else
        {
            ButtonIconTextClicked(null);
        }
    }
}