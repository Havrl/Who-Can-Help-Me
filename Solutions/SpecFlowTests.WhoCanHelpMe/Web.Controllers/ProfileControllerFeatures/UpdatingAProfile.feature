Feature: Updating a profile
    In order to keep my profile current
    As a user
    I want to be able to modify it

Scenario: Requesting the update profile page
    Given I have a profile
    When I request the profile update page
    Then the default view should be returned
    And my profile data should be shown in the view
    And all categories should be available for selection in the view