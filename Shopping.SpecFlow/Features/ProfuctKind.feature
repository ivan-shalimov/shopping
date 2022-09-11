Feature: Product Kind

Product kind allows to group products to use it in statistic

Background: 
	Given I am a client

Scenario: Get product kind collection
	Given The DB has a product kind 'Medicine'
	When I make a Get request to 'api/products/kinds'
	Then The response should have status code '200'
	And The response should contains the product kind 'Medicine'

Scenario: Add new product kind
	Given I want to add new product kind 'Furniture'
	When I make a POST request to 'api/products/kinds'
	Then The response should have status code '200'
	And The DB should have the product kind 'Furniture'

Scenario: Add duplicated product kind
	Given The DB has a product kind 'Medicine'
	Given I want to add new product kind 'Medicine'
	When I make a POST request to 'api/products/kinds'
	Then The response should have status code '400'
	And The response should contains the error 'The product kind Medicine already exists'

Scenario: Update product kind
	Given The DB has a product kind 'Cleaning Supplies'
	And I want to change the name for product kind from 'Cleaning Supplies' to 'Cleaning'
	When I make a PUT request to 'api/products/kinds/{id}'
	Then The response should have status code '200'
	And The DB should have the product kind 'Cleaning'
	And The DB should not have the product kind 'Cleaning Supplies'

Scenario: Update product kind with duplicated name
	Given The DB has a product kind 'Medicine'
	And The DB has a product kind 'Spices'
	And I want to change the name for product kind from 'Spices' to 'Medicine'
	When I make a PUT request to 'api/products/kinds/{id}'
	Then The response should have status code '400'
	And The response should contains the error 'The product kind Medicine already exists'

Scenario: Delete the product kind
	Given The DB has a product kind 'Toys'
	And I want to delete product kind 'Toys'
	When I make a Delete request to 'api/products/kinds/{id}'
	Then The response should have status code '200'
	And The DB should not have the product kind 'Toys'

Scenario: Delete the product kind with product
	Given The DB has a product kind 'Medicine'
	And The DB has a product 'Pill' for the product kind 'Medicine'
	And I want to delete product kind 'Medicine'
	When I make a Delete request to 'api/products/kinds/{id}'
	Then The response should have status code '400'
	And The response should contains the error 'The product kind Medicine has products.'

Scenario: Merge product Kinds
	Given The DB has a product kind 'Medicine1'
	And The DB has a product 'Pill' for the product kind 'Medicine1'
	And The DB has a product kind 'Medicine2'
	And The DB has a product 'Vitamin' for the product kind 'Medicine1'
	And I want to merge 'Medicine1' with 'Medicine2' to have only 'Medicine'
	When I make a POST request to 'api/products/kinds/merged'
	Then The response should have status code '200'
	And The DB should have the product kind 'Medicine'
	And The DB should have the product 'Pill' for the product kind 'Medicine'
	And The DB should have the product 'Vitamin' for the product kind 'Medicine'
	But The DB should not have the product kind 'Medicine1'
	And The DB should not have the product kind 'Medicine2'