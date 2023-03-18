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

Scenario: Get expenses by product kind for current month
	Given The DB has the set receipt with items created in this month
	And The DB has the set receipt with items created in previous month
	When I make a GET request to 'api/statistic/expenses-by-kinds' with query parameters
	| start                 | end                 |
	| {StartOfCurrentMonth} | {EndOfCurrentMonth} |
	Then The response status should be success
	And The response should contains expenses of current month grouped by kind

Scenario: Get expenses by product for the kind of the current month
	Given The DB has the set receipt with items created in this month
	And The DB has the set receipt with items created in previous month
	When I make a GET request to 'api/statistic/expenses-by-products' with query parameters
	| kind                 | start                 | end                 |
	| {TheProductKindName} | {StartOfCurrentMonth} | {EndOfCurrentMonth} |
	Then The response status should be success
	And The response should contains expenses for the kind of the current month grouped by product

Scenario: Get expenses details for product of the current month
	Given The DB has the set receipt with items created in this month
	And The DB has the set receipt with items created in previous month
	When I make a GET request to 'api/statistic/product-expenses-details' with query parameters
	| productName      | start                 | end                 |
	| {TheProductName} | {StartOfCurrentMonth} | {EndOfCurrentMonth} |
	Then The response status should be success
	And The response should contains details of expenses for the product of the current month