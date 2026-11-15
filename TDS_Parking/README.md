1. Prerequisites:
	1. .NET 11.0 SDK or later installed on your machine. (https://dotnet.microsoft.com/en-us/download/dotnet/11.0)
	2. To run, visit the project directory in your terminal and execute the following command:
		- `dotnet run --project TDS_Parking`

2. Assumptions:
	1. The parking lot has a limited number of spaces - preseeded them witinh repository
	2. Parking fees can be adjusted in the database - final charge will be calculated based on the vehicle type, duration of stay and current fee rates.
	3. Vehicle reg numbers are 4-12 characters long. No special formatting added - this would have to be discussed with the client.
	4. Rates can be adjusted in the db - both per minute rate as well as per period (adjustable, initialized as 1 pound per 5 minute period). Parking users pay for each started minute as well as for each 5 full minutes.
	5. In current implementation, parking slots are separated from parking entries - parking slots are preseeded in the database and are not dynamically created or removed. 
	    Parking entries are created when a vehicle enters the parking lot and are removed when the vehicle exits. 
		This way we can maintain parking entries history as well as check current parking slots effortlesly
3. Questions:
	1. Should predefined vehicle types be adjustable or three grades are enough?
	2. Is there a standard format for license plates that should be enforced? (e.g. 4-12 characters, no special characters, etc.)
	3. In future, would we like to introduce an alert when a slot is taken for too long?
	4. Are all the slots universal? In real world, some slots would not be able to hold larger vehicles.