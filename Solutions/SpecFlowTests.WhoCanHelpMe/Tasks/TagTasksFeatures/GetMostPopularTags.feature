Feature: Get most popular tags
	In order to see what tags people are interested in 
	As a user
	I want to see the most popular tags

@mytag
Scenario: Get most popular tags
	Given I am using the tag tasks
	When I ask for the most popular tags
	Then the list of popular tags should be returned
