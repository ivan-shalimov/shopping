Feature: Statistic

A short summary of the feature
Background: 
Given The DB has the product kind
And The DB has another product kind
And The DB has a product for the product kind
And The DB has another product for another product kind

Scenario: Get expenses by kind for current month
	Given The DB has the set receipt with items created in this month
	And The DB has the set receipt with items created in previous month
	When I make a GET request to 'api/statistic/expenses-by-kind/current/month'
	Then The response status should be success
	And The response should contains expenses of current month grouped by kind

Scenario: Get expenses by month for year
	Given The DB has the set receipt with items created in this month
	And The DB has the set receipt with items created in previous month
	When I make a GET request to 'api/statistic/expenses-by-month/current/year'
	Then The response status should be success
	And The response should contains expenses of this year by month
	
Scenario: Get expenses by shop for current month
	Given The DB has the set receipt with items created in this month
	And The DB has the set receipt with items created in previous month
	When I make a GET request to 'api/statistic/expenses-by-shop/current/month'
	Then The response status should be success
	And The response should contains expenses of current month grouped by shop
	
Scenario: Get product cost change
	Given The DB has the set receipt with items created in this month
	And The DB has the set receipt with items created in previous month
	When I make a GET request to 'api/statistic/product-cost-change' with query parameters
	| page | pageSize | orderBy |
	| 1    | 10       | percent |
	Then The response status should be success
	And The response should contains product cost change