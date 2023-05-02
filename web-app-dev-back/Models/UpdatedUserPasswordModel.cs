namespace web_app_dev_back.Models
{
    public class UpdatedUserPasswordModel
    {
        public string OldPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;

    }
}
