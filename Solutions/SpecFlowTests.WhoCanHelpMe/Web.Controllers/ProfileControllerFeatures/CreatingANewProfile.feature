Feature: Creating a profile
    In order to publicise my skills
    As a user
    I want to create my profile

@mytag
Scenario: Requesting the create page
    Given I am creating a new profile
    When I request the Create page
    Then the default view should be returned

Scenario: Creating a profile
    Given I am creating a new profile
    And I have set my first name to FirstName
    And I have set my surname to Surname
    When I submit my data to the Create page
    Then my profile should be created
    And I should be directed to the update profile page

Scenario: Creating a profile with invalid first name
    Given I am creating a new profile
    And I have left my first name blank
    And I have set my surname to Surname
    When I submit my data to the Create page
    Then I should be directed to the create profile page
    And the invalid first name error message should be shown

Scenario: Creating a profile with invalid last name
    Given I am creating a new profile
    And I have left my first name FirstName
    And I have set my surname to blank
    When I submit my data to the Create page
    Then I should be directed to the create profile page
    And the invalid last name error message should be shown

Scenario: Creating a profile with invalid first and last names
    Given I am creating a new profile
    And I have left my first name blank
    And I have set my surname to blank
    When I submit my data to the Create page
    Then I should be directed to the create profile page
    And the invalid first name error message should be shown
    And the invalid last name error message should be shown
