Feature: Get tags by first characters
	In order to help me when adding tags to my profile
	As a user
	I want to see tags whose name starts with the characters I specify

@mytag
Scenario: Get tags matching starting characters
	Given I search for tags by starting characters
	And There are matching tags
	When I ask for matching tags
	Then the list of matching tags should be returned
