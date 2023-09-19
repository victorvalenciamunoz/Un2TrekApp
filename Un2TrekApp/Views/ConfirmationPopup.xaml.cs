namespace Un2TrekApp.Views;

public partial class ConfirmationPopup
{
    public ConfirmationPopup(string message)
    {
        InitializeComponent();
        labelMessage.Text = message;
    }

    private void ButtonIconText_CancelClicked(Controls.ButtonIconClickedEventArgs args)
    {
        Close(false);
    }

    private void ButtonIconText_AcceptClicked(Controls.ButtonIconClickedEventArgs args)
    {
        Close(true);
    }
}