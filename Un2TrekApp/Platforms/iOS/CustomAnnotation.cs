using MapKit;
using UIKit;

namespace Un2TrekApp;

public class CustomAnnotation : MKPointAnnotation
{
    public Guid Identifier { get; set; }
    public UIImage? Image { get; set; }
}
