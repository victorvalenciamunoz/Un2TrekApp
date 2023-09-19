using MapKit;
using Microsoft.Maui.Maps.Handlers;

namespace Un2TrekApp;

public class CustomPinHandler : MapPinHandler
{
    protected override IMKAnnotation CreatePlatformElement() => new CustomAnnotation();
}
