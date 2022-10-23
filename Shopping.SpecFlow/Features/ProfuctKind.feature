Feature: Product Kind

Product kind allows to group products to use it in statistic

Scenario: Get product kind collection
	Given The DB has the product kind
	And The DB has a product for the product kind
	And The DB has another product kind
	When I make a GET request to 'api/products/kinds'
	Then The response status should be success
	And The response should contains both product kind

Scenario: Add new product kind
	Given I want to add a new product kind
	When I make a POST request to 'api/products/kinds'
	Then The response status should be success
	And The DB should contain the product kind

Scenario: Add duplicated product kind
	Given The DB has the product kind
	And The DB has another product kind
	Given I want to add a new product kind with the same name as another product kind
	When I make a POST request to 'api/products/kinds'
	Then The response status should be bad request
	And The response should contains the error 'The product kind with this name is already exists.'

Scenario: Update product kind
	Given The DB has the product kind
	And I want to change the name for the product kind
	When I make a PUT request to 'api/products/kinds/{ProductKindId}'
	Then The response status should be success
	And The DB should contain the product kind with the new name

Scenario: Update product kind with duplicated name
	Given The DB has the product kind
	And The DB has another product kind
	And I want to change the name for product to use the same name as another product kind
	When I make a PUT request to 'api/products/kinds/{ProductKindId}'
	Then The response status should be bad request
	And The response should contains the error 'The product kind name is already used'

Scenario: Delete the product kind
	Given The DB has the product kind
	And I want to delete the product kind
	When I make a DELETE request to 'api/products/kinds/{ProductKindId}'
	Then The response status should be success
	And The DB should not contain the product kind

Scenario: Delete the product kind with product
	Given The DB has the product kind
	And The DB has a product for the product kind
	And I want to delete the product kind
	When I make a DELETE request to 'api/products/kinds/{ProductKindId}'
	Then The response status should be bad request
	And The response should contains the error 'The product kind has products.'

Scenario: Merge product Kinds
	Given The DB has the product kind
	And The DB has another product kind
	And The DB has a product for the product kind
	And The DB has another product for another product kind
	And I want to merge the product kind with another product kind
	When I make a POST request to 'api/products/kinds/merged'
	Then The response status should be success
	And The DB should contain the product kind
	But The DB should not contain another product kind
	And The DB should contain both products for the product kind
