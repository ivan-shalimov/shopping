Feature: Product

Products are good that is used to define receipt's items

Background: 
Given The DB has the product kind
And The DB has another product kind

Scenario: Get products
	Given The DB has a product for the product kind
	And The product is used
	And The DB has another product for another product kind
	When I make a GET request to 'api/products/'
	Then The response status should be success
	And The response should contains both products

Scenario: Get filtered list of products
	Given The DB has a product for the product kind
	And The DB has another product for another product kind
	When I make a GET request to 'api/products/' with query parameters
	| productKindId      |
	| {TheProductKindId} |
	Then The response status should be success
	And The response should contains products only for the product kind

Scenario: Add new product
	Given I want to add a product for the product kind
	When I make a POST request to 'api/products/'
	Then The response status should be success
	And The DB should contain the product

Scenario: Update the product
	Given The DB has another product kind 
	And The DB has a product for the product kind
	And I want to rename the product and change product kind to another product kind
	When I make a PUT request to 'api/products/{ProductId}'
	Then The response status should be success
	And The DB should contain the product for the another product kind with the new name

Scenario: Delete the product
	Given The DB has a product for the product kind
	And I want to delete the product
	When I make a DELETE request to 'api/products/{ProductId}'
	Then The response status should be success
	And The DB should not contain the product

Scenario: Delete the product that is used
	Given The DB has a product for the product kind
	And The product is used
	And I want to delete the product
	When I make a DELETE request to 'api/products/{ProductId}'
	Then The response status should be bad request
	And The response should contains the error 'Product is used'

Scenario: Merge product
	Given The DB has the product
	And The DB has another product
	And The product is used
	And The another product is used
	And I want to merge the product with another product
	When I make a POST request to 'api/products/merged'
	Then The response status should be success
	And The DB should contain the product
	But The DB should not contain another product
	And The DB should contain receipt's items of the product assigned to the product
	And The DB should contain receipt's items of another product assigned to the product