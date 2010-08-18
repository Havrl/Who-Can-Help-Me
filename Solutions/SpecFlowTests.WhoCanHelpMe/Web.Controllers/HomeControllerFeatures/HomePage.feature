Feature: Home page
    In order to start using the site
    As a user
    I want to view the home page

Scenario: Request home page
    When I request the home page
    Then the default view should be returned
    And the view should show the current project buzz
