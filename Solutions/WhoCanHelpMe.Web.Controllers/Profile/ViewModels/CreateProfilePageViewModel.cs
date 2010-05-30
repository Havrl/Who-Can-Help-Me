namespace WhoCanHelpMe.Web.Controllers.Profile.ViewModels
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    using Shared.ViewModels;

    #endregion

    public class CreateProfilePageViewModel : PageViewModel
    {
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }
    }
}