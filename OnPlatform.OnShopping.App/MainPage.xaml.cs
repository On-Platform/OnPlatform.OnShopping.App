using OnPlatform.OnShopping.App.Annotations;
using OnPlatform.OnShopping.App.Models;
using OnPlatform.OnShopping.App.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OnPlatform.OnShopping.App;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
        this.BindingContext = this;

        const string scope = "openid onShoppingApi.read";
        _oidcIdentityService = new IdentityService("onShoppingAuth", App.CallbackScheme, App.SignoutCallbackScheme, scope, AuthorityUrl, "OnPlatform");
        ExecuteLogin = new Command(Login);
        ExecuteRefresh = new Command(RefreshTokens);
        ExecuteLogout = new Command(Logout);
        ExecuteGetInfo = new Command(GetInfo);
        ExecuteCopyAccessToken = new Command(async () => await Clipboard.SetTextAsync(_credentials?.AccessToken));
        ExecuteCopyIdentityToken = new Command(async () => await Clipboard.SetTextAsync(_credentials?.IdentityToken));
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        ExecuteLogin.Execute(null);
    }



    private readonly HttpClient _httpClient = new HttpClient();
#if DEBUG
    private const string AuthorityUrl = "http://10.0.2.2:5123";
#elif RELEASE
        private const string AuthorityUrl = "https://onplatformidserverstaging.azurewebsites.net";
#endif
    private ClientCredentials? _credentials;
    private readonly IdentityService _oidcIdentityService;

    public ICommand ExecuteLogin { get; }
    public ICommand ExecuteRefresh { get; }
    public ICommand ExecuteLogout { get; }
    public ICommand ExecuteGetInfo { get; }
    public ICommand ExecuteCopyAccessToken { get; }
    public ICommand ExecuteCopyIdentityToken { get; }

    public string TokenExpirationText => "Access Token expires at: " + _credentials?.AccessTokenExpiration;
    public string AccessTokenText => "Access Token: " + _credentials?.AccessToken;
    public string IdTokenText => "Id Token: " + _credentials?.IdentityToken;
    public bool IsLoggedIn => _credentials != null;
    public bool IsNotLoggedIn => _credentials == null;

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void GetInfo()
    {
        var url = Path.Combine(AuthorityUrl, "manage", "index");
        var response = await _httpClient.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (string.IsNullOrEmpty(_credentials?.RefreshToken))
            {
                // no valid refresh token exists => authenticate
                await _oidcIdentityService.Authenticate();
            }
            else
            {
                // we have a valid refresh token => refresh tokens
                await _oidcIdentityService.RefreshToken(_credentials!.RefreshToken);
            }
        }

        Debug.WriteLine(await response.Content.ReadAsStringAsync());
    }

    private async void Login()
    {
        ClientCredentials credentials = await _oidcIdentityService.Authenticate();
        UpdateCredentials(credentials);

        _httpClient.DefaultRequestHeaders.Authorization = credentials.IsError
            ? null
            : new AuthenticationHeaderValue("bearer", credentials.AccessToken);
    }

    private async void RefreshTokens()
    {
        if (_credentials?.RefreshToken == null) return;
        ClientCredentials credentials = await _oidcIdentityService.RefreshToken(_credentials.RefreshToken);
        UpdateCredentials(credentials);
    }

    private async void Logout()
    {
        await _oidcIdentityService.Logout(_credentials?.IdentityToken);
        _credentials = null;
        OnPropertyChanged(nameof(TokenExpirationText));
        OnPropertyChanged(nameof(AccessTokenText));
        OnPropertyChanged(nameof(IdTokenText));
        OnPropertyChanged(nameof(IsLoggedIn));
        OnPropertyChanged(nameof(IsNotLoggedIn));
    }

    private void UpdateCredentials(ClientCredentials credentials)
    {
        _credentials = credentials;
        OnPropertyChanged(nameof(TokenExpirationText));
        OnPropertyChanged(nameof(AccessTokenText));
        OnPropertyChanged(nameof(IdTokenText));
        OnPropertyChanged(nameof(IsLoggedIn));
        OnPropertyChanged(nameof(IsNotLoggedIn));
    }
}

