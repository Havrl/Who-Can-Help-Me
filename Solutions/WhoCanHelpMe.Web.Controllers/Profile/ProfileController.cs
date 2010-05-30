namespace WhoCanHelpMe.Web.Controllers.Profile
{
    #region Using Directives

    using System;
    using System.Web.Mvc;

    using ActionFilters;

    using Domain;
    using Domain.Contracts.Tasks;

    using Extensions;

    using Home;

    using Mappers.Contracts;

    using MvcContrib;
    using MvcContrib.Attributes;
    using MvcContrib.Filters;

    using Shared.ActionResults;

    using ViewModels;

    #endregion

    public class ProfileController : BaseController
    {
        private readonly IAddAssertionDetailsMapper addAssertionDetailsMapper;

        private readonly ICategoryTasks categoryTasks;

        private readonly ICreateProfileDetailsMapper createProfileDetailsMapper;

        private readonly ICreateProfilePageViewModelMapper createProfilePageViewModelMapper;

        private readonly IIdentityTasks identityTasks;

        private readonly IProfilePageViewModelMapper profilePageViewModelMapper;

        private readonly IProfileTasks userTasks;

        public ProfileController(
            IIdentityTasks identityTasks,
            IProfileTasks userTasks,
            ICategoryTasks categoryTasks,
            IProfilePageViewModelMapper profilePageViewModelMapper,
            IAddAssertionDetailsMapper addAssertionDetailsMapper,
            ICreateProfilePageViewModelMapper createProfilePageViewModelMapper,
            ICreateProfileDetailsMapper createProfileDetailsMapper)
        {
            this.identityTasks = identityTasks;
            this.userTasks = userTasks;
            this.categoryTasks = categoryTasks;
            this.profilePageViewModelMapper = profilePageViewModelMapper;
            this.addAssertionDetailsMapper = addAssertionDetailsMapper;
            this.createProfilePageViewModelMapper = createProfilePageViewModelMapper;
            this.createProfileDetailsMapper = createProfileDetailsMapper;
        }

        [Authorize]
        [HttpGet]
        [ModelStateToTempData]
        [RequireNoExistingProfile("Profile", "Update")]
        public ActionResult Create()
        {
            var createProfileDetails = this.TempData.Get<CreateProfileDetails>();

            var viewModel = this.createProfilePageViewModelMapper.MapFrom(createProfileDetails);

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateToTempData]
        [RequireNoExistingProfile("Profile", "Update")]
        public ActionResult Create(CreateProfileFormModel createProfile)
        {
            if (ModelState.IsValid)
            {
                var identity = this.identityTasks.GetCurrentIdentity();

                var createProfileDetails = this.createProfileDetailsMapper.MapFrom(createProfile, identity);

                this.userTasks.CreateProfile(createProfileDetails);

                return this.RedirectToAction(x => x.Update());
            }

            return this.RedirectToAction(x => x.Create());
        }

        [Authorize]
        [HttpGet]
        [RequireExistingProfile("Profile", "Create")]
        public ActionResult Delete()
        {
            var identity = this.identityTasks.GetCurrentIdentity();

            this.userTasks.DeleteProfile(identity.UserName);

            return this.RedirectToAction<HomeController>(x => x.Index());
        }

        [Authorize]
        [HttpGet]
        [RequireExistingProfile("Profile", "Create")]
        public ActionResult DeleteAssertion(int assertionId)
        {
            var identity = this.identityTasks.GetCurrentIdentity();

            var user = this.userTasks.GetProfileByUserName(identity.UserName);

            this.userTasks.RemoveAssertion(
                user,
                assertionId);

            return this.RedirectToAction(x => x.Update());
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.RedirectToAction(x => x.Update());
        }

        [Authorize]
        [HttpGet]
        [ModelStateToTempData]
        [RequireExistingProfile("Profile", "Create")]
        public ActionResult Update()
        {
            var identity = this.identityTasks.GetCurrentIdentity();

            var user = this.userTasks.GetProfileByUserName(identity.UserName);

            var categories = this.categoryTasks.GetAll();

            var viewModel = this.profilePageViewModelMapper.MapFrom(
                user,
                categories);

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelStateToTempData]
        [RequireExistingProfile("Profile", "Create")]
        public ActionResult Update(AddAssertionFormModel formModel)
        {
            if (ModelState.IsValid)
            {
                var identity = this.identityTasks.GetCurrentIdentity();

                var addAssertionDetails = this.addAssertionDetailsMapper.MapFrom(formModel, identity);

                this.userTasks.AddAssertion(addAssertionDetails);
            }

            return this.RedirectToAction(x => x.Update());
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            var user = this.userTasks.GetProfileById(id);

            if (user != null)
            {
                var profileViewModel = this.profilePageViewModelMapper.MapFrom(user);

                return this.View(profileViewModel);
            }

            return new NotFoundResult();
        }
    }
}