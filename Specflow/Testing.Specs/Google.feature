Feature: Google
	In order to use PPM site
	As a normal user
	I want to be able to browse the site

@mytag
Scenario: Test Google Search
	Given I go to Google website
	And I search for Avanade wiki
	And I click on first link
	Then I am at the Avanade wiki page
	Then I capture screenshot

@mytag
Scenario: Login to PPM
	Given I login as User05.Test@whinniy.com
	Then I am at the landing page

@mytag
Scenario: Check Project Team Portfolio Management
	Given I am on the landing page
	And I click on Project Portfolio Management for Project Team
	Then I see a list of projects
