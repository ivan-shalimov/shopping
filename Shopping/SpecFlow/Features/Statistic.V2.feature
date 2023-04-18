Feature: Statistic.V2

A short summary of the feature

Background: 
	Given  The Db has the following products 
	| Kind name  | Product Name |
	| Vegetables | Carrot       |
	| Vegetables | Onion        |
	| Fruit      | Cherry       |
	| Fruit      | Lemon        |
	And The Db has receipt for 'Local' shop on 1st of the previous month with the following items
	| Product Name | Price | Amount | Cost |
	| Carrot       | 8     | 3      | 24   |
	And The Db has receipt for 'Local' shop on 1st of this month with the following items
	| Product Name | Price | Amount | Cost |
	| Carrot       | 10    | 3      | 30   |
	| Onion        | 5.5   | 10     | 55   |
	| Cherry       | 10    | 0.5    | 5    |
	| Lemon        | 100   | 0.2    | 20   |
	And The Db has receipt for 'Gypper market' shop on 1st of this month with the following items
	| Product Name | Price | Amount | Cost |
	| Carrot       | 5     | 3      | 15   |
	| Onion        | 5     | 1      | 5    |
	| Cherry       | 20    | 0.5    | 10   |
	| Lemon        | 150   | 0.2    | 30   |

Scenario: Get expenses by kind for current month
	When I make a GET request to 'api/statistic/expenses-by-kind/current/month'
	Then The response status should be success
	And The response should be Dictionary<string,decimal> with the following data
	| Kind name  | Expenses |
	| Vegetables | 105      |
	| Fruit      | 65       |

Scenario: Get expenses by month for year
	When I make a GET request to 'api/statistic/expenses-by-month/current/year'
	Then The response status should be success
	And The response should be Dictionary<int,decimal> with the following data
	| Month                 | Expenses |
	| previous month number | 24       |
	| current month number  | 170      |
	
Scenario: Get expenses by shop for current month
	When I make a GET request to 'api/statistic/expenses-by-shop/current/month'
	Then The response status should be success
	And The response should be Dictionary<string,decimal> with the following data
	| Shop          | Expenses |
	| Local         | 110      |
	| Gypper market | 60       |
	
Scenario: Get product cost change
	When I make a GET request to 'api/statistic/product-cost-change' with query parameters
	| page | pageSize | orderBy |
	| 1    | 10       | percent |
	Then The response status should be success
	And The response should be collection of ProductCostChange with the following data
	| Product Name | Kind Name  | Shop  | PreviousCost | LastCost | ChangePercent |
	| Carrot       | Vegetables | Local | 8            | 10       | 0.25          |

Scenario: Get expenses by product kind for current month
	When I make a GET request to 'api/statistic/expenses-by-kinds' with query parameters
	| start                 | end                 |
	| {StartOfCurrentMonth} | {EndOfCurrentMonth} |
	Then The response status should be success
	And The response should be Dictionary<string,decimal> with the following data
	| Kind name  | Expenses |
	| Vegetables | 105      |
	| Fruit      | 65       |

Scenario: Get expenses by product for the kind of the current month
	When I make a GET request to 'api/statistic/expenses-by-products' with query parameters
	| kind  | start                 | end                 |
	| Fruit | {StartOfCurrentMonth} | {EndOfCurrentMonth} |
	Then The response status should be success
	And The response should be Dictionary<string,decimal> with the following data
	| Product name | Expenses |
	| Cherry       | 15       |
	| Lemon        | 50       |

Scenario: Get expenses details for product of the current month
	When I make a GET request to 'api/statistic/product-expenses-details' with query parameters
	| productName      | start                 | end                 |
	| Lemon | {StartOfCurrentMonth} | {EndOfCurrentMonth} |
	Then The response status should be success
	And The response should collection of ProductExpensesDetail with the following data
	| ShopName      | SpentOn           | Price | Amount |
	| Local         | 1st of this month | 100   | 0.2    |
	| Gypper market | 1st of this month | 150   | 0.2    |
