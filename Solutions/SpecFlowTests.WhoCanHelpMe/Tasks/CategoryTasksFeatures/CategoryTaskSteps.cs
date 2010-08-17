namespace SpecFlowTests.WhoCanHelpMe.Tasks.CategoryTasksFeatures
{
    using System.Collections.Generic;
    using System.Linq;

    using global::WhoCanHelpMe.Domain;
    using global::WhoCanHelpMe.Domain.Contracts.Repositories;
    using global::WhoCanHelpMe.Domain.Contracts.Tasks;
    using global::WhoCanHelpMe.Tasks;

    using NUnit.Framework;

    using Rhino.Mocks;

    using TechTalk.SpecFlow;

    [Binding]
    public class CategoryTaskSteps
    {
        private ICategoryRepository categoryRepository;
        
        private ICategoryTasks subject;
        
        private IList<Category> getAllResult;
        
        private Category getOneResult;

        private List<Category> theCategories;

        private Category theCategory;

        [Given(@"I have category tasks available")]
        public void GivenIHaveCategoryTasksAvailable()
        {
            this.categoryRepository = MockRepository.GenerateMock<ICategoryRepository>();
            this.subject = new CategoryTasks(this.categoryRepository);
        }

        [Given(@"there are (.*) categories in the repository")]
        public void GivenThereAreCategoriesInTheRepository(int categories)
        {
            this.theCategories = new List<Category>();

            for (int index = 0; index < categories; index++)
            {
                this.theCategories.Add(new Category());
            }

            this.categoryRepository.Stub(r => r.FindAll()).Return(this.theCategories.AsQueryable());
        }

        [Given(@"there is a category with Id (.*) in the repository")]
        public void GivenThereIsACategoryWithIdInTheRepository(int id)
        {
            this.theCategory = new Category();
            this.categoryRepository.Stub(r => r.FindOne(id)).Return(this.theCategory);
        }

        [Given(@"there is no category with Id (.*) in the repository")]
        public void GivenThereIsNoCategoryWithIdInTheRepository(int id)
        {
            this.categoryRepository.Stub(r => r.FindOne(id)).Return(null);
        }

        [When(@"I ask for all categories")]
        public void WhenIAskForAllCategories()
        {
            this.getAllResult = this.subject.GetAll();
        }

        [When(@"I ask for the category with Id (.*)")]
        public void WhenIAskForTheCategoryWithId(int id)
        {
            this.getOneResult = this.categoryRepository.FindOne(id);
        }

        [Then(@"the list of all categories should be returned")]
        public void ThenTheListOfAllCategoriesShouldBeReturned()
        {
            Assert.That(this.getAllResult.Count, Is.EqualTo(3));
            
            foreach (var current in this.theCategories)
            {
                Assert.That(this.getAllResult, Contains.Item(current));
            }
        }

        [Then(@"an empty category list should be returned")]
        public void ThenAnEmptyCategoryListShouldBeReturned()
        {
            Assert.That(this.getAllResult, Is.Empty);
        }

        [Then(@"the category should be returned")]
        public void ThenTheCategoryShouldBeReturned()
        {
            Assert.That(this.getOneResult, Is.EqualTo(this.theCategory));
        }

        [Then(@"nothing should be returned")]
        public void ThenNothingShouldBeReturned()
        {
            Assert.That(this.getOneResult, Is.Null);
        }
    }
}
