Feature: Car Costs

Allow add and manage last car costs, like petrol cost, car details purchases, etc

Scenario: View car costs
	Given There are car costs in the DB
	And I want to get car costs for current month 
	When I make a GET request to 'api/car-costs' with query parameters
	| month   |
	| {CurrentMonth} |
	Then The response status should be success
	And The response should contains car costs for the current month

Scenario: Add a new car cost
	Given I want to add a new car cost with
	| Description | Price | Amount | Date     |
	| Petrol A95  | 55.6  | 5      | {UtcNow} |
	When I make a POST request to 'api/car-costs'
	Then The response status should be success
	And The DB should contain a new car cost

Scenario: Add a new car cost with invalid data
	Given I want to add a new car cost with 
	| Description   | Price   | Amount   | Date     |
	| <Description> | <Price> | <Amount> | {UtcNow} |
	When I make a POST request to 'api/car-costs'
	Then The response status should be bad request
Examples: 
	| Description   | Price | Amount | Error                                   |
	| {EmptyString} | 55.6  | 5      | Description is required.                |
	| Petrol A95    | 0     | 5      | The Price can not be zero or negative.  |
	| Petrol A95    | 55.6  | 0      | The Amount can not be zero or negative. |

Scenario: Update the car cost
	Given There is the car cost in the DB
	And I want to update the car cost with
	| Description | Price | Amount | Date     |
	| Petrol A95  | 55.6  | 5      | {UtcNow} |
	When I make a PUT request to 'api/car-costs/{Id}'
	Then The response status should be success
	And The DB should contain the updated car cost
	
Scenario: Update the car cost by random id
	Given There is the car cost in the DB
	Given I want to add a new car cost with 
	| Description | Price | Amount | Date     |
	| Petrol A95  | 55.6  | 5      | {UtcNow} |
	When I make a PUT request to 'api/car-costs/{RandomId}'
	Then The response status should be bad request
	
Scenario: Update the car cost with invalid data
	Given There is the car cost in the DB
	Given I want to add a new car cost with 
	| Description   | Price   | Amount   | Date     |
	| <Description> | <Price> | <Amount> | {UtcNow} |
	When I make a PUT request to 'api/car-costs/{Id}'
	Then The response status should be bad request
Examples: 
	| Description   | Price | Amount | Error                                   |
	| {EmptyString} | 55.6  | 5      | Description is required.                |
	| Petrol A95    | 0     | 5      | The Price can not be zero or negative.  |
	| Petrol A95    | 55.6  | 0      | The Amount can not be zero or negative. |

Scenario: Delete the car cost
	Given There is the car cost in the DB
	And I want to delete the car cost
	When I make a DELETE request to 'api/car-costs/{Id}'
	Then The response status should be success
	And The DB should not contains the car cost

Scenario: Delete the car cost by random id
	Given There is the car cost in the DB
	And I want to delete the car cost
	When I make a DELETE request to 'api/car-costs/{RandomId}'
	Then The response status should be bad request