namespace HC.Business.Models.VM
{
    public class LoginViewModel
    {
        public string JwtToken { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
