# Auction API

This is an auction API built using **ASP.NET Core**, **Entity Framework**, and **SQL Server**, designed to allow users to create, manage, and bid on auction items. The API is fully integrated with **JWT authentication**, **Swagger** for API documentation, and **CORS** for cross-origin resource sharing. Unit tests and dependency injection are also implemented to ensure the maintainability and scalability of the application.

## Key Technologies

- **Entity Framework Core**: ORM used for data access and interaction with the MSSQL database.
- **API**: RESTful API built using **ASP.NET Core**.
- **Swagger**: Provides interactive API documentation for easy testing and exploration of the endpoints.
- **Postman**: Can be used to interact with and test the API.
- **MSSQL**: The relational database used to store auction, bid, category, and user data.
- **JWT (JSON Web Tokens)**: Used for secure user authentication and authorization.
- **Unit Testing**: Ensures the API's functionality and correctness.
- **Dependency Injection**: Used to manage the application's services and repositories.
- **CORS**: Configured to allow cross-origin requests from specified domains.

## Features

- **Auction Management**: Create, view, update, and delete auction items.
- **Bidding**: Allows users to place bids on auction items.
- **User Authentication**: Users can register, log in, and access protected routes using JWT tokens.
- **Categories**: Auctions are categorized for better organization.
- **CORS**: Configured to ensure that requests are only accepted from specific origins.
- **Swagger UI**: Provides a user-friendly interface for exploring the API.

## Setup & Installation


### Clone the Repository
Clone this repository to your local machine:

```bash
git clone https://github.com/yourusername/auction-api.git
cd auction-api
````
### Configuration
Make sure to update the appsettings.json file with your SQL Server connection string and JWT settings:

    
          "ConnectionStrings": {
      "Default": "Server=yourserver;Database=yourdatabase;User Id=yourusername;Password=yourpassword;"
    },
    "JwtSettings": {
      "Issuer": "yourIssuer",
      "Audience": "yourAudience",
      "Secret": "yourSecret"
    }

### Run the Application

Restore the dependencies:

    dotnet restore
    dotnet run



