Feature: Product

Products are good that is used to define receipt's items

Background:
	Given The DB has a product kind 'Fruit'
	And The DB has a product kind 'Other'
	And I am a client

Scenario: Get products
	Given The DB has a product 'Lemon' for the product kind 'Fruit'
	When I make a Get request to 'api/products/'
	Then The response should have status code '200'
	And The result contains the product 'Lemon' with the product kind 'Fruit'


Scenario: Add new product
	Given I want to add new product 'Apple' for the product kind 'Fruit'
	When I make a POST request to 'api/products/'
	Then The response should have status code '200'
	And The DB should have the product 'Apple' for the product kind 'Fruit'

Scenario: Update the product
	Given The DB has a product 'Water' for the product kind 'Fruit'
	And I want to rename product 'Water' to 'Mineral Water' and change product kind to 'Other'
	When I make a PUT request to 'api/products/{id}'
	Then The response should have status code '200'
	And The DB should have the product 'Mineral Water' for the product kind 'Other'
	But The DB should not have the product 'Water'

Scenario: Delete the product
	Given The DB has a product 'Cleaning liquid' for the product kind 'Fruit'
	And I want to delete product 'Cleaning liquid'
	When I make a Delete request to 'api/products/{id}'
	Then The response should have status code '200'
	And The DB should not have the product 'Cleaning liquid'