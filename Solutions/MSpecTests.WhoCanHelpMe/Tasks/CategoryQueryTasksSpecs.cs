namespace MSpecTests.WhoCanHelpMe.Tasks
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using global::WhoCanHelpMe.Domain;
    using global::WhoCanHelpMe.Domain.Contracts.Repositories;
    using global::WhoCanHelpMe.Domain.Contracts.Tasks;
    using global::WhoCanHelpMe.Domain.Specifications;
    using global::WhoCanHelpMe.Tasks;

    using Machine.Specifications;
    using Machine.Specifications.AutoMocking.Rhino;
    using Rhino.Mocks;

    #endregion

    public abstract class specification_for_category_query_tasks : Specification<ICategoryQueryTasks, CategoryQueryTasks>
    {
        protected static ICategoryRepository the_category_repository;

        Establish context = () => the_category_repository = DependencyOf<ICategoryRepository>();
    }

    [Subject(typeof(CategoryQueryTasks))]
    public class when_the_category_query_tasks_are_asked_to_get_all : specification_for_category_query_tasks
    {
        protected static IList<Category> result;
        static IQueryable<Category> the_categories;

        Establish context = () =>
            {
                the_categories = new List<Category>{new Category(), new Category(), new Category()}.AsQueryable();

                the_category_repository.Stub(r => r.FindAll()).Return(the_categories);
            };

        Because of = () => result = subject.GetAll();

        It should_ask_the_category_repository_to_get_all = () => the_category_repository.AssertWasCalled(c => c.FindAll());

        It should_return_the_list_of_categories = () => result.ShouldContainOnly(the_categories);
    }

    [Subject(typeof(CategoryQueryTasks))]
    public class when_the_category_query_tasks_are_asked_to_get_all_but_there_are_no_categories_in_the_repository : specification_for_category_query_tasks
    {
        protected static IList<Category> result;
        static IQueryable<Category> the_categories;

        Establish context = () =>
        {
            the_categories = new List<Category>().AsQueryable();

            the_category_repository.Stub(r => r.FindAll()).Return(the_categories);
        };

        Because of = () => result = subject.GetAll();

        It should_ask_the_category_repository_to_get_all = () => the_category_repository.AssertWasCalled(c => c.FindAll());

        It should_return_an_empty_list = () =>
            {
                result.ShouldNotBeNull();
                result.ShouldBeEmpty();
            };
    }

    [Subject(typeof(CategoryQueryTasks))]
    public class when_the_category_query_tasks_are_asked_for_a_category_by_id : specification_for_category_query_tasks
    {
        static int category_id;
        static Category result;
        static Category the_category;

        Establish context = () =>
            {
                category_id = 1;

                the_category = new Category();

                the_category_repository.StubFindOne().Return(the_category);
            };

        Because of = () => result = subject.Get(category_id);

        It should_ask_the_category_repository_for_the_category_by_id = () => the_category_repository.AssertFindOneWasCalledWithSpecification<CategoryByIdSpecification, Category>(s => s.Id == category_id);

        It should_return_the_matching_category = () => result.ShouldEqual(the_category);
    }

    [Subject(typeof(CategoryQueryTasks))]
    public class when_the_category_query_tasks_are_asked_for_a_category_by_id_and_there_is_no_matching_category : specification_for_category_query_tasks
    {
        static int category_id;
        static Category result;

        Establish context = () =>
        {
            category_id = 1;

            the_category_repository.StubFindOne().Return(null);
        };

        Because of = () => result = subject.Get(category_id);

        It should_ask_the_category_repository_for_the_category_by_id = () => the_category_repository.AssertFindOneWasCalledWithSpecification<CategoryByIdSpecification, Category>(s => s.Id == category_id);

        It should_return_null = () => result.ShouldBeNull();
    }
}