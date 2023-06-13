# URL Shortener API

This repository contains a minimalistic API for shortening URLs. It allows you to generate short and custom URLs, as well as redirect users to their original destinations. The API is built using C# and utilizes the Microsoft.EntityFrameworkCore library for database operations.

## Features

- Shorten a provided URL and generate a unique short URL.
- Redirect users to the original URL based on the short URL.
- Store and manage URLs in a SQLite database.

## Getting Started

To get started with the URL Shortener API, follow these steps:

1. Clone the repository to your local machine.
2. Install the required dependencies using a package manager like NuGet.
3. Build the project using your preferred IDE or build tool.
4. Run the application on your local development server.

## API Endpoints

The following API endpoints are available:

- `POST /shorterurl`: Shorten a URL and generate a unique short URL. The request body should include a JSON object with the `Url` property containing the URL to be shortened.

- Fallback route: Handles redirection based on the short URL provided. If a short URL is valid and exists in the database, the API redirects the user to the original URL. Otherwise, it returns a 400 Bad Request error.

## Configuration

The URL Shortener API utilizes a SQLite database for storing URLs. The connection string is configured in the code:

```csharp
var connStr = "DataSource=app.db";
```

Make sure the connection string points to a valid database file. If the file doesn't exist, the application will create a new database at the specified location with `dotnet ef database update` command.

## Dependencies

The project relies on the following dependencies:

- Microsoft.EntityFrameworkCore: Provides database operations and entity framework functionality.
- Microsoft.AspNetCore: Offers web application and HTTP request handling capabilities.
- Microsoft.AspNetCore.Mvc: Enables building APIs and handling API requests.

Make sure to install these dependencies before running the application.

## Contributing

Contributions to the URL Shortener API are welcome! If you find any bugs or want to suggest improvements, please open an issue or submit a pull request. 

## License

This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code as per the terms of the license.

## Contact

If you have any questions or need further assistance, feel free to contact the project owner.

- Name: Krzysztof
- GitHub: [Your GitHub Profile](https://github.com/kris007iron)

Thank you for using the URL Shortener API!
