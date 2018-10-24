Feature: LandingPage
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Login with special credential
	Given I have logged in as 'User01.Test'
  And I navigate to 'Landing Page'
	And I click button 'MeFlex'
	When I click link 'My account'
  Then I click link 'Personal info'
	Then I take a screen shot
  Then I click link 'Subscriptions'
  Then I take a screen shot
