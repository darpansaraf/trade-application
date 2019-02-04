The design of Trade Engine is as follows:

	- TradeEngine:-
		- This is the Host Project.
		- This should be the starup project and should be hosted.
		
	- TradeEngine.Entities:-
		- This Project defines the contract the TradeEngine API exposes to the outside world.
	
	- TradeEngine.Providers.BusinessLogic
		- This is the project which contains the core logic for the working of TradeEngine.
		- This project references the TradeEngine.Entities project and operates on the classes defined in it.
		
	- TradeEngine.Tests
		- This project contains the controller test cases for TradeEngine.		


 - Currently TradeEngine stores the trades in a static list.
 - When the "submit" API is called trade engine validates the incoming request and stores the data in a static list. 
 - Trade Executions are performed when a user fires a GET call on a particular stock. In response, TradeEngine returns a JSON response which contains the below fields 
	- BuyRequestId
	- SellRequestId
	- Quantity
	- Price
	
I did not understand what exactly was supposed to be done for this statement:"List of valid stock identifiers should be loaded from configuration on application startup.". Hence I did not implement anything regarding this.

I am attaching a postman collection which contains the various sample requests relating the example that was mentioned in the given problem statement.
	
In case if you encounter any issues while building the project from the .zip file, you can clone it from https://github.com/darpansaraf/trade-application or please contact me on:
	- darpan.saraf@gmail.com
	- +91 9028600638 / +91 9168793240.
	

	
