UC#2 .NET AI Repository

# Description
This project contains a simple .NET Core API with two endpoints for accessing Stripe API. The two endpoints retrieve balance and transaction info from Stripe's test dataset. The 2 endpoints are set up on the /api/stripe/balance and /api/stripe/balanceTransactions URLs.  
The balance transactions endpoint is paginated, meaning that you can add the parameters "?limit=...&offset=..." to limit the number of transactions and start loading transactions from a certain id.
# How to run
The application can be run from the main entry point by building and running it with Visual Studio. Once the server starts up, it loads a page in the browser where you can navigate to the correct endpoint (/api/stripe/balance or /api/stripe/balanceTransactions) to retrieve some test data.  
Before running the application, make sure you set the Stripe:SecretKey variable using the Secrets Manager, otherwise you will not be able to access the test data. The SecretKey can be retrieved from your Stripe account's Dashboard.
