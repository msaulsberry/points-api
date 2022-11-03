# points-api

## Steps to run:

1. Download the .NET SDK: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
2. Clone this repository to your machine
3. In the terminal, enter `cd points` and `dotnet run` to start the service (Alternatively, open points.sln file in visual studio and click run)
4. Perform actions on the three endpoints
  - Post transaction to 'Points/transaction'
  - Post spend event to 'Points/spend'
  - Get payer balances at 'Points/payerBalances'
  
![image](https://user-images.githubusercontent.com/37089264/199819692-33a9f720-aa8c-44b0-9979-83e88ecb60a9.png)
 
<img width="986" alt="image" src="https://user-images.githubusercontent.com/37089264/199819802-5e0ac29c-bcf6-4884-b9d1-b43e54ce0d70.png">
 
<img width="987" alt="image" src="https://user-images.githubusercontent.com/37089264/199819877-faed2023-0baa-48ba-bf64-33e39df71937.png">

<img width="973" alt="image" src="https://user-images.githubusercontent.com/37089264/199820092-d08b49a0-6f6e-4385-96a8-7bc95f3b445e.png">


## Overview of the key areas of the code: 

- Endpoints are in points/Controller/PointsController.cs
- Business logic is separated out into the points/Services/PointService.cs
- Request/Response and domain models are defined in points/Models/
- Service is scaffolded into the project as a singleton in points/Program.cs

