Feature: Get categories
    In order to add meaning to my tags
    As a user
    I want to get a list of possible categories from the repository

Scenario: Get all categories
    Given I have category tasks available
    And there are 3 categories in the repository
    When I ask for all categories
    Then the list of all categories should be returned

Scenario: No categories are available
    Given I have category tasks available
    And there are 0 categories in the repository
    When I ask for all categories
    Then an empty category list should be returned

Scenario: Get a category by id
    Given I have category tasks available
    And there is a category with Id 3 in the repository
    When I ask for the category with Id 3
    Then the category should be returned

Scenario: Get a category by id when there is no matching category
    Given I have category tasks available
    And there is no category with Id 3 in the repository
    When I ask for the category with Id 3
    Then nothing should be returned