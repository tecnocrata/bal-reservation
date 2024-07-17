# Hotel Reservation Service

This service is designed to manage hotel reservations. It provides functionalities for making, canceling, and listing reservations, as well as user authentication for secure access. The service is built using C# and follows a clean architecture with clearly defined use cases and handlers.

## Table of Contents

- [Hotel Reservation Service](#hotel-reservation-service)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Use Cases](#use-cases)
    - [Make Reservation](#make-reservation)
      - [Endpoint](#endpoint)
      - [Description](#description)
      - [Handler](#handler)
      - [Validation](#validation)
      - [Example Request](#example-request)
    - [Cancel Reservation](#cancel-reservation)
      - [Endpoint](#endpoint-1)
      - [Description](#description-1)
      - [Handler](#handler-1)
      - [Example Request](#example-request-1)
    - [List Reservations](#list-reservations)
      - [Endpoint](#endpoint-2)
      - [Description](#description-2)
      - [Handler](#handler-2)
      - [Example Request](#example-request-2)
    - [Get Reservation Details](#get-reservation-details)
      - [Endpoint](#endpoint-3)
      - [Description](#description-3)
      - [Handler](#handler-3)
      - [Example Request](#example-request-3)
    - [User Authentication](#user-authentication)
      - [Endpoint](#endpoint-4)
      - [Description](#description-4)
      - [Handler](#handler-4)
      - [Example Request](#example-request-4)
  - [Installation](#installation)
  - [Usage](#usage)
  - [Tests](#tests)

## Overview

The Hotel Reservation Service allows users to manage hotel reservations through a set of RESTful API endpoints. The service handles the following scenarios:

- Creating new reservations
- Canceling existing reservations
- Listing all reservations
- Retrieving details of a specific reservation
- Authenticating users

## Use Cases

### Make Reservation

#### Endpoint

`POST /api/reservation`

#### Description

This endpoint allows users to create a new reservation. It validates the reservation details and ensures that the reservation date is within the allowed range.

#### Handler

`MakeCommandHandler`

#### Validation

- Reservation date must be in the future.
- Reservation date must be within 30 days from the current date.
- Reservation time must be between 7:00 PM and 10:00 PM.

#### Example Request

```json
{
  "id": "unique-reservation-id",
  "date": "2023-08-15T20:00:00Z",
  "guests": 2,
  "name": "John Doe"
}
```

### Cancel Reservation

#### Endpoint

`DELETE /api/reservation/{id}`

#### Description

This endpoint allows users to cancel an existing reservation. It verifies the existence of the reservation before canceling it.

#### Handler

`CancelCommandHandler`

#### Example Request

```http
DELETE /api/reservation/unique-reservation-id
```

### List Reservations

#### Endpoint

`GET /api/reservation`

#### Description

This endpoint retrieves a list of all reservations.

#### Handler

`ListQueryHandler`

#### Example Request

```http
GET /api/reservation
```

### Get Reservation Details

#### Endpoint

`GET /api/reservation/{id}`

#### Description

This endpoint retrieves the details of a specific reservation by its ID.

#### Handler

`DetailQueryHandler`

#### Example Request

```http
GET /api/reservation/unique-reservation-id
```

### User Authentication

#### Endpoint

`POST /authorization/token`

#### Description

This endpoint allows users to authenticate by providing their username and password. It generates a token for authenticated users.

#### Handler

`LoginQueryHandler`

#### Example Request

```json
{
  "username": "testuser",
  "password": "password"
}
```

## Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/hotel-reservation-service.git
   ```
2. Navigate to the project directory:
   ```sh
   cd hotel-reservation-service
   ```
3. Install the required dependencies:
   ```sh
   dotnet restore
   ```

## Usage

1. Build the project:
   ```sh
   dotnet build
   ```
2. Run the project:
   ```sh
   dotnet run
   ```

## Tests

Run the tests using the following command:

```sh
dotnet test
```

The project includes comprehensive tests for all handlers to ensure the correctness of the business logic and validate various scenarios.
