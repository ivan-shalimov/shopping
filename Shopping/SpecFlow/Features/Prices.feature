Feature: Prices

A short summary of the feature
Background: 
Given The DB has the product kind
And The DB has another product kind
And The DB has a product for the product kind
And The DB has another product for another product kind

Scenario: Get prices for products by from last receipt of the shop
	Given There are some receipt of two shops
	And I have created a new receipt for the shop
	And I have selected products to add them to receipt 
	And I want to get prices for the products of the shop to fill them for selected products
	When I make a GET request to 'api/prices/latest' with query parameters
	| receiptId      | productIds  | productIds         |
	| {TheReceiptId} | {ProductId} | {AnotherProductId} |
	Then The response status should be success
	And The response should contains last product's prices of the shop