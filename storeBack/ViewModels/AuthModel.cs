namespace storeBack.ViewModels
{
    public class AuthValidateTokenModel
    { 
        public string Token { get; set; }
    }

    public class RefreshTokenModel
    {
        public string OldToken { get; set; }
    }
}
