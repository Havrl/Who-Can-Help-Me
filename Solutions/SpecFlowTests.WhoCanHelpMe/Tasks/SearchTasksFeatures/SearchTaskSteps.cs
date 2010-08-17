namespace SpecFlowTests.WhoCanHelpMe.Tasks.SearchTasksFeatures
{
    using System.Collections.Generic;
    using System.Linq;

    using global::WhoCanHelpMe.Domain;
    using global::WhoCanHelpMe.Domain.Contracts.Repositories;
    using global::WhoCanHelpMe.Domain.Contracts.Specifications;
    using global::WhoCanHelpMe.Domain.Contracts.Tasks;
    using global::WhoCanHelpMe.Domain.Specifications;
    using global::WhoCanHelpMe.Tasks;

    using NUnit.Framework;

    using Rhino.Mocks;

    using SharpArch.Testing;

    using TechTalk.SpecFlow;

    [Binding]
    public class SearchTaskSteps
    {
        private ISearchTasks searchTasks;

        private IAssertionRepository assertionRepository;

        private ITagRepository tagRepository;

        private Tag targetTag;

        private List<Assertion> matchingAssertions;

        private IList<Assertion> result;

        [Given(@"I search by tag name")]
        public void GivenISearchByTagName()
        {
            this.assertionRepository = MockRepository.GenerateMock<IAssertionRepository>();
            this.tagRepository = MockRepository.GenerateMock<ITagRepository>();
            this.searchTasks = new SearchTasks(this.assertionRepository, this.tagRepository);
        }

        [Given(@"the tag exists")]
        public void GivenTheTagExists()
        {
            this.targetTag = new Tag();
            this.targetTag.SetIdTo(5);
            this.targetTag.Views = 5;
            this.tagRepository.Stub(r => r.FindOne(Arg<TagByNameSpecification>.Is.Anything)).Return(this.targetTag);
        }

        [Given(@"the tag does not exist")]
        public void GivenTheTagDoesNotExist()
        {
            this.targetTag = null;
            this.tagRepository.Stub(r => r.FindOne(Arg<TagByNameSpecification>.Is.Anything)).Return(this.targetTag);
        }

        [Given(@"there are (.*) matching assertions")]
        public void GivenThereAreMatchingAssertions(int assertionCount)
        {
            this.matchingAssertions = new List<Assertion>();

            for (var index = 0; index < assertionCount; index++)
            {
                this.matchingAssertions.Add(new Assertion { Profile = new Profile() });
            }

            this.assertionRepository.Stub(r => r.FindAll(Arg<AssertionByTagIdSpecification>.Is.Anything)).Return(this.matchingAssertions.AsQueryable());
        }

        [When(@"I ask for matching assertions")]
        public void WhenIAskForMatchingAssertions()
        {
            this.result = this.searchTasks.ByTag("tag");
        }

        [Then(@"the list of matching assertions should be returned")]
        public void ThenTheListOfMatchingAssertionsShouldBeReturned()
        {
            Assert.That(this.result.Count, Is.EqualTo(this.matchingAssertions.Count));
            Assert.That(this.result, Is.EquivalentTo(this.matchingAssertions));
        }

        [Then(@"the view count on the tag should be updated")]
        public void ThenTheViewCountOnTheTagShouldBeUpdated()
        {
            Assert.That(this.targetTag.Views, Is.EqualTo(6));
        }

        [Then(@"an empty assertion list should be returned")]
        public void ThenAnEmptyAssertionListShouldBeReturned()
        {
            Assert.That(this.result.Count, Is.EqualTo(0));
        }
    }
}
