﻿namespace WhoCanHelpMe.Tasks
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using Domain;
    using Domain.Contracts.Repositories;
    using Domain.Contracts.Tasks;
    using Domain.Specifications;

    #endregion

    public class TagQueryTasks : ITagQueryTasks
    {
        private readonly ITagRepository tagRepository;

        public TagQueryTasks(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public IList<Tag> GetWhereNameStartsWith(string characters)
        {
            if (string.IsNullOrEmpty(characters))
            {
                return new List<Tag>();
            }

            return this.DoSearch(new TagByFirstCharactersOfNameSpecification(characters));
        }

        public IList<Tag> GetMostPopularTags(int count)
        {
            return this.tagRepository
                       .PopularTags(count)
                       .ToList();
        }

        public Tag GetByName(string name)
        {
            return this.DoSearch(new TagByNameSpecification(name))
                       .FirstOrDefault();
        }

        private IList<Tag> DoSearch(QuerySpecification<Tag> specification)
        {
            return this.tagRepository
                       .FindAll(specification)
                       .OrderBy(t => t.Name)
                       .ToList();
        }
    }
}
