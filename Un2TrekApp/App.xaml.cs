namespace Un2TrekApp;

public partial class App : Application
{
	public static UserInfo UserInfo;

	public static string StorageUserInfoKey = "StorageUserInfoKey";
	public static string UrlBase = @"http://sycapps.net/api/v1/";	
	public static string RoleAdministrator = "Un2TrekAdministrator";
	public static string ActivityEndPoint = "Un2TrekActividad";
	public static string TrekiEndPoint = "Un2TrekTreki";
	public App()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("==");

		InitializeComponent();

		MainPage = new AppShell();
	}
}
