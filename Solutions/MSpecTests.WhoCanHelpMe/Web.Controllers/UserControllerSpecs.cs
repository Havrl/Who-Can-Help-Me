﻿using WhoCanHelpMe.Web.Controllers.User.ViewModels;

namespace MSpecTests.WhoCanHelpMe.Web.Controllers
{
    #region Using Directives

    using System.Security.Authentication;
    using System.Web.Mvc;

    using global::WhoCanHelpMe.Domain.Contracts.Tasks;
    using global::WhoCanHelpMe.Web.Controllers.Home;
    using global::WhoCanHelpMe.Web.Controllers.User;
    using global::WhoCanHelpMe.Web.Controllers.User.Mappers.Contracts;

    using Machine.Specifications;
    using Machine.Specifications.AutoMocking.Rhino;
    using Rhino.Mocks;
    using Machine.Specifications.Mvc;

    #endregion

    public abstract class specification_for_user_controller : Specification<UserController>
    {
        protected static ILoginPageViewModelMapper login_page_view_model_mapper;
        protected static IRegisterPageViewModelMapper register_page_view_model_mapper;
        protected static IIdentityTasks identity_tasks;

        Establish context = () =>
            {
                identity_tasks = DependencyOf<IIdentityTasks>();
                login_page_view_model_mapper = DependencyOf<ILoginPageViewModelMapper>();
                register_page_view_model_mapper = DependencyOf<IRegisterPageViewModelMapper>();
            };
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_for_the_default_view_and_the_user_is_logged_in : specification_for_user_controller
    {
        static ActionResult result;

        Establish context = () => identity_tasks.Stub(i => i.IsSignedIn()).Return(true);

        Because of = () => result = subject.Index();

        It should_ask_the_identity_tasks_if_the_user_is_signed_in =
            () => identity_tasks.AssertWasCalled(i => i.IsSignedIn());

        It should_redirect_to_home = () => result.ShouldRedirectToAction<HomeController>(x => x.Index());
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_for_the_default_view_and_the_user_is_not_logged_in : specification_for_user_controller
    {
        static ActionResult result;

        Establish context = () => identity_tasks.Stub(i => i.IsSignedIn()).Return(false);

        Because of = () => result = subject.Index();

        It should_ask_the_identity_tasks_if_the_user_is_signed_in =
            () => identity_tasks.AssertWasCalled(i => i.IsSignedIn());

        It should_redirect_to_the_login_action = () =>
            result.ShouldRedirectToAction<UserController>(x => x.Login(null));
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_for_the_login_view_and_the_user_is_logged_in : specification_for_user_controller
    {
        static ActionResult result;

        Establish context = () => identity_tasks.Stub(i => i.IsSignedIn()).Return(true);

        Because of = () => result = subject.Login(string.Empty);

        It should_ask_the_identity_tasks_if_the_user_is_signed_in =
            () => identity_tasks.AssertWasCalled(i => i.IsSignedIn());

        It should_redirect_to_home = () => result.ShouldRedirectToAction<HomeController>(x => x.Index());
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_for_the_login_view_and_the_user_is_not_logged_in : specification_for_user_controller
    {
        static ActionResult result;
        static object the_view_model;

        Establish context = () => identity_tasks.Stub(i => i.IsSignedIn()).Return(false);

        Because of = () => result = subject.Login(string.Empty);

        It should_ask_the_identity_tasks_if_the_user_is_signed_in =
            () => identity_tasks.AssertWasCalled(i => i.IsSignedIn());

        It should_ask_the_login_page_view_model_mapper_to_map_the_view_model =
            () => login_page_view_model_mapper.AssertWasCalled(
                      m => m.MapFrom(
                               null,
                               string.Empty));

        It should_return_the_default_view = () => result.ShouldBeAView().And().ViewName.ShouldBeEmpty();

        It should_pass_the_view_model_to_the_view = () => 
            result.ShouldBeAView().And().ViewData.Model.ShouldEqual(the_view_model);
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_for_the_register_view_and_the_user_is_logged_in : specification_for_user_controller
    {
        static ActionResult result;

        Establish context = () => identity_tasks.Stub(i => i.IsSignedIn()).Return(true);

        Because of = () => result = subject.Register(string.Empty);

        It should_ask_the_identity_tasks_if_the_user_is_signed_in =
            () => identity_tasks.AssertWasCalled(i => i.IsSignedIn());

        It should_redirect_to_home = () => result.ShouldRedirectToAction<HomeController>(x => x.Index());
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_for_the_register_view_and_the_user_is_not_logged_in : specification_for_user_controller
    {
        static ActionResult result;
        static object the_view_model;

        Establish context = () => identity_tasks.Stub(i => i.IsSignedIn()).Return(false);

        Because of = () => result = subject.Register(string.Empty);

        It should_ask_the_identity_tasks_if_the_user_is_signed_in =
            () => identity_tasks.AssertWasCalled(i => i.IsSignedIn());

        It should_ask_the_login_page_view_model_mapper_to_map_the_view_model =
            () => register_page_view_model_mapper.AssertWasCalled(
                      m => m.MapFrom(
                               null,
                               string.Empty));

        It should_return_the_default_view = () => result.ShouldBeAView().And().ViewName.ShouldBeEmpty();

        It should_pass_the_view_model_to_the_view = () =>
            result.ShouldBeAView().And().ViewData.Model.ShouldEqual(the_view_model);
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_to_logout : specification_for_user_controller
    {
        static ActionResult result;

        Establish context = () => identity_tasks.Stub(i => i.IsSignedIn()).Return(true);

        Because of = () => result = subject.SignOut();

        It should_ask_the_identity_tasks_to_log_the_current_user_out =
            () => identity_tasks.AssertWasCalled(i => i.SignOut());

        It should_redirect_to_home = () => result.ShouldRedirectToAction<HomeController>(x => x.Index());
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_to_authenticate_with_a_return_url_and_authentication_is_successful : specification_for_user_controller
    {
        static ActionResult result;
        static LoginFormModel the_login_form_model;

        Establish context = () =>
        {
            the_login_form_model = new LoginFormModel
                                       {
                                           EmailAddress = "user id", 
                                           Password = "password", 
                                           ReturnUrl = "return url"
                                       };
        };

        Because of = () => result = subject.Authenticate(the_login_form_model);

        It should_ask_the_identity_tasks_to_authenticate_the_user =
            () => identity_tasks.AssertWasCalled(i => i.Authenticate(the_login_form_model.EmailAddress, the_login_form_model.Password));

        It should_redirect_to_the_return_url = 
            () => result.ShouldBeARedirect().And().Url.ShouldEqual(the_login_form_model.ReturnUrl);
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_to_authenticate_without_a_return_url_and_authentication_is_successful : specification_for_user_controller
    {
        static ActionResult result;
        static LoginFormModel the_login_form_model;

        Establish context = () =>
        {
            the_login_form_model = new LoginFormModel
            {
                EmailAddress = "user id",
                Password = "password",
                ReturnUrl = string.Empty
            };
        };

        Because of = () => result = subject.Authenticate(the_login_form_model);

        It should_ask_the_identity_tasks_to_authenticate_the_user =
            () => identity_tasks.AssertWasCalled(i => i.Authenticate(the_login_form_model.EmailAddress, the_login_form_model.Password));

        It should_redirect_to_home = () => result.ShouldRedirectToAction<HomeController>(x => x.Index());
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_to_authenticate_and_authentication_is_unsuccessful : specification_for_user_controller
    {
        static ActionResult result;
        static LoginFormModel the_login_form_model;

        Establish context = () =>
        {
            the_login_form_model = new LoginFormModel
            {
                EmailAddress = "user id",
                Password = "password",
                ReturnUrl = "return url"
            };

            identity_tasks.Stub(i => i.Authenticate(the_login_form_model.EmailAddress, the_login_form_model.Password)).Throw(new AuthenticationException());
        };

        Because of = () => result = subject.Authenticate(the_login_form_model);

        It should_ask_the_identity_tasks_to_authenticate_the_user =
            () => identity_tasks.AssertWasCalled(i => i.Authenticate(the_login_form_model.EmailAddress, the_login_form_model.Password));

        It should_redirect_to_the_login_view = () => 
            result.ShouldRedirectToAction<UserController>(x => x.Login(null));
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_to_register_with_a_return_url_and_authentication_is_successful : specification_for_user_controller
    {
        static ActionResult result;
        static RegistrationFormModel the_register_form_model;

        Establish context = () =>
        {
            the_register_form_model = new RegistrationFormModel()
            {
                EmailAddress = "user id",
                Password = "password",
                ConfirmPassword = "password",
                ReturnUrl = "return url"
            };
        };

        Because of = () => result = subject.Register(the_register_form_model);

        It should_ask_the_identity_tasks_to_register_the_user =
            () => identity_tasks.AssertWasCalled(i => i.Register(the_register_form_model.EmailAddress, the_register_form_model.Password));

        It should_redirect_to_the_return_url =
            () => result.ShouldBeARedirect().And().Url.ShouldEqual(the_register_form_model.ReturnUrl);
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_to_register_without_a_return_url_and_authentication_is_successful : specification_for_user_controller
    {
        static ActionResult result;
        static RegistrationFormModel the_register_form_model;

        Establish context = () =>
        {
            the_register_form_model = new RegistrationFormModel
            {
                EmailAddress = "user id",
                Password = "password",
                ConfirmPassword = "password",
                ReturnUrl = string.Empty
            };
        };

        Because of = () => result = subject.Register(the_register_form_model);

        It should_ask_the_identity_tasks_to_register_the_user =
            () => identity_tasks.AssertWasCalled(i => i.Register(the_register_form_model.EmailAddress, the_register_form_model.Password));

        It should_redirect_to_home = () => result.ShouldRedirectToAction<HomeController>(x => x.Index());
    }

    [Subject(typeof(UserController))]
    public class when_the_user_controller_is_asked_to_register_and_registration_is_unsuccessful : specification_for_user_controller
    {
        static ActionResult result;
        static RegistrationFormModel the_register_form_model;

        Establish context = () =>
        {
            the_register_form_model = new RegistrationFormModel()
            {
                EmailAddress = "user id",
                Password = "password",
                ConfirmPassword = "password",
                ReturnUrl = "return url"
            };

            identity_tasks.Stub(i => i.Register(the_register_form_model.EmailAddress, the_register_form_model.Password)).Throw(new AuthenticationException());
        };

        Because of = () => result = subject.Register(the_register_form_model);

        It should_ask_the_identity_tasks_to_authenticate_the_user =
            () => identity_tasks.AssertWasCalled(i => i.Register(the_register_form_model.EmailAddress, the_register_form_model.Password));

        It should_redirect_to_the_login_view = () =>
            result.ShouldRedirectToAction<UserController>(x => x.Register(the_register_form_model.ReturnUrl));
    }
}
