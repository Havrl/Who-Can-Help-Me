Feature: Search by tag
    In order to find people who can help me
    As a user
    I want to be able to search for assertions by tag name

Scenario: Search by tag name
    Given I search by tag name
    And the tag exists
    And there are 3 matching assertions
    When I ask for matching assertions
    Then the list of matching assertions should be returned
    And the view count on the tag should be updated

Scenario: Search by tag name where there are no matching assertions
    Given I search by tag name
    And the tag exists
    And there are 0 matching assertions
    When I ask for matching assertions
    Then an empty assertion list should be returned

Scenario: Search by tag name where the tag name does not exist
    Given I search by tag name
    And the tag does not exist
    When I ask for matching assertions
    Then an empty assertion list should be returned
