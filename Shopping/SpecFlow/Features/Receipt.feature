Feature: Receipt

A receipt is main thing to declare shopping results

Background: 
Given The DB has the product kind
And The DB has another product kind
And The DB has a product for the product kind
And The DB has another product for another product kind

Scenario: Get all receipts
	Given The DB has the receipt created in this month
	And I want to get receipts for current month 
	When I make a GET request to 'api/receipts' with query parameters
	| month   |
	| {CurrentMonth} |
	Then The response status should be success
	And The response should contains the receipt

Scenario: Add new receipt
	Given I want to add a new receipt with
	| Description | Date     |
	| Test        | {UtcNow} |
	When I make a POST request to 'api/receipts'
	Then The response status should be success
	And The DB should contain the receipt

Scenario: Add new receipt without description
	Given I want to add a new receipt with
	| Description  | Date     |
	| {EmptyString} | {UtcNow} |
	When I make a POST request to 'api/receipts'
	Then The response status should be bad request
	And The response should contains the error 'The description should not be empty.'	
	
Scenario: Update receipt
	Given The DB has the receipt
	Given I want to update the receipt with
	| Description        | Date     |
	| Update description | {UtcNow} |
	When I make a PUT request to 'api/receipts/{TheReceiptId}'
	Then The response status should be success
	And The DB should contain the receipt with the new description and date
	
Scenario: Update receipt with empty description
	Given The DB has the receipt
	Given I want to update the receipt with
	| Description   | Date     |
	| {EmptyString} | {UtcNow} |
	When I make a PUT request to 'api/receipts/{TheReceiptId}'
	Then The response status should be bad request
	And The response should contains the error 'The description should not be empty.'	


Scenario: Get items of the receipt
	Given The DB has the receipt
	And The DB has the item with the product of the receipt
	And I want to get items of the receipt
	When I make a GET request to 'api/receipts/{TheReceiptId}/items'
	Then The response status should be success
	And The response should contains the item with the product of the receipt
	

Scenario: Add item to the receipt
	Given The DB has the receipt
	And I want to add new item to the receipt with
	| ProductId   | Price | Amount |
	| {ProductId} | 1.2   | 2      |
	When I make a POST request to 'api/receipts/{TheReceiptId}/items'
	Then The response status should be success
	And The DB should contain the item of the receipt

Scenario Outline: Add item to the receipt with invalid data
	Given The DB has the receipt
	And I want to add new item to the receipt with
	| ProductId   | Price   | Amount   |
	| <ProductId> | <Price> | <Amount> |
	When I make a POST request to 'api/receipts/{TheReceiptId}/items'
	Then The response status should be bad request
	And The response should contains the error '<Error>'
Examples: 
	| ProductId   | Price | Amount | Error                                   |
	| {EmptyId}   | 1.2   | 2      | The ProductId is required.              |
	| {ProductId} | 0     | 2      | The Price can not be zero or negative.  |
	| {ProductId} | 1.2   | 0      | The Amount can not be zero or negative. |
	
Scenario: Update price or amount of the receipt's item
	Given The DB has the receipt
	And The DB has the item with the product of the receipt
	And I want to update the item of the receipt
	| Price | Amount |
	| 2.2   | 3      |
	When I make a PUT request to 'api/receipts/{TheReceiptId}/items/{TheReceiptItemId}'
	Then The response status should be success
	And The DB should contain the item of the receipt
		
Scenario Outline: Update item of the receipt with invalid data
	Given The DB has the receipt
	And The DB has the item with the product of the receipt
	And I want to update the item of the receipt
	| Price   | Amount   |
	| <Price> | <Amount> |
	When I make a PUT request to 'api/receipts/{TheReceiptId}/items/{TheReceiptItemId}'
	Then The response status should be bad request
	And The response should contains the error '<Error>'
Examples: 
	| Price | Amount | Error                                   |
	| 0     | 2      | The Price can not be zero or negative.  |
	| 2.2   | 0      | The Amount can not be zero or negative. |

Scenario: Delete item of the receipt
	Given The DB has the receipt
	And The DB has the item with the product of the receipt
	And I want to delete the item of the receipt
	When I make a DELETE request to 'api/receipts/{TheReceiptId}/items/{TheReceiptItemId}'
	Then The response status should be success
	And The DB should not contain the item of the receipt