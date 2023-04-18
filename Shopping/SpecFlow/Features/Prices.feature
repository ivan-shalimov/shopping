Feature: Prices

A short summary of the feature

Background: 
	Given  The Db has the following products 
	| Kind name  | Product Name |
	| Vegetables | Carrot       |
	| Vegetables | Onion        |
	And The Db has receipt for 'Local' shop on 1st of the previous month with the following items
	| Product Name | Price | Amount | Cost |
	| Carrot       | 8     | 3      | 24   |
	| Onion        | 4     | 3      | 24   |
	And The Db has receipt for 'Local' shop on 1st of this month with the following items
	| Product Name | Price | Amount | Cost |
	| Carrot       | 10    | 3      | 30   |
	| Onion        | 5.5   | 10     | 55   |
	And The Db has receipt for 'Gypper market' shop on 1st of this month with the following items
	| Product Name | Price | Amount | Cost |
	| Carrot       | 5     | 3      | 15   |
	| Onion        | 5     | 1      | 5    |

Scenario: Get prices for products by from last receipt of the shop
	Given I have created a new receipt for 'Local' shop
	And I have selected the following products
	| Product Name |
	| Carrot       |
	| Onion        |
	And I want to get prices for the products of the shop to fill them for selected products
	When I make a GET request to 'api/prices/latest' with query parameters
	| receiptId      | productIds  |
	| {TheReceiptId} | {ProductIds} |
	Then The response status should be success
	And The response should contains the following prices
	| Product name | Prices |
	| Carrot       | 10     |
	| Onion        | 5.5    |