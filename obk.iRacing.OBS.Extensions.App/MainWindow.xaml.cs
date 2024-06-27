using System.Windows;
using System.Windows.Media.Imaging;
using Aydsko.iRacingData;
using obk.iRacing.Service.Interfaces;
using WpfApp3.Helpers;

namespace WpfApp3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int CustomerId { get; set; }
    public bool RememberMe { get; set; }

    private readonly IIRacingService _iRacingService;
    
    public MainWindow(IIRacingService iRacingService)
    {
        InitializeComponent();
        
        _iRacingService = iRacingService;
        
        this.Icon = new BitmapImage(new Uri(PathHelper.IconPath, UriKind.Absolute));
        
        loginImage.Source= new BitmapImage(new Uri(PathHelper.LoginImagePath, UriKind.Absolute));
        
        if (System.IO.File.Exists(PathHelper.CredentialsPath))
        {
            string[] lines = System.IO.File.ReadAllLines(PathHelper.CredentialsPath);
            usernameTextBox.Text = lines[0];
            passwordBox.Password = lines[1];
            customerIdTextBox.Text = lines[2];
            saveCredentialsCheckBox.IsChecked = true;
        }
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        string username = usernameTextBox.Text;
        string password = passwordBox.Password;
        int customerId = Convert.ToInt32(customerIdTextBox.Text);

        try
        {
            var loginStatus = _iRacingService.Login(username, password, customerId).Result;
            if (!loginStatus)
                throw new Exception("Nombre de usuario o contraseña incorrectos.");
            
            if (saveCredentialsCheckBox.IsChecked == true)
            {
                string[] lines = { username, password, customerId.ToString() };
                System.IO.File.WriteAllLines(PathHelper.CredentialsPath, lines);
            }
            
            // open extensions window and close login window
            Extensions extensions = new Extensions(_iRacingService, customerId, username, password);
            extensions.Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Autenticación Error");
        }
    }
}