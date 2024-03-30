# Project Name

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## Description

The ideia is create two code, one monolith and another in moduler monolith and see the pros and cons of use this archictecture;

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Installation

To run this project, you will need to have Docker installed on your machine. Docker allows you to create and manage containers for your applications.

1. Install Docker by following the instructions for your operating system: [Docker Installation Guide](https://docs.docker.com/get-docker/).

2. Once Docker is installed, you can pull the PostgreSQL image from Docker Hub by running the following command in your terminal:

    ```bash
    docker pull postgres
    ```

3. After pulling the PostgreSQL image, you can create a container for your project by running the following command:

    ```bash
    docker run --name my-postgres-container -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres
    ```

    This command creates a container named `my-postgres-container` with the PostgreSQL image, sets the password for the database, and maps the container's port 5432 to the host's port 5432.

4. You can now connect to the PostgreSQL database using a database client or by running commands inside the container. The connection details are as follows:

    - Host: `localhost`
    - Port: `5432`
    - Username: `postgres`
    - Password: `mysecretpassword`

    Make sure to update your application's configuration to use these connection details.

6. Update this credential of database on appsettings.Development.json to app has the correct database address
    
5. In order to generate all tables on your database the command bellow is need inside the infra project:
    ```bash
    dotnet ef database update --startup-project ../EGeekApi

    ```
   This code will get all migration of project and update the database to have the sabe tables.



## Usage

To run the project, you need to access the API project (monolith or modular) and run dotnet run.

Each project has a file egeekapi.htpp that informs all endpoints and the header/body to access the api

## Contributing

Guidelines for contributing to your project.

## License

This project is licensed under the [MIT License](LICENSE).