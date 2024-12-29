# FlightSearch.API - Project Overview

## About the Project
`FlightSearch.API` is an API designed to integrate multiple flight providers and offer users flight search and price comparison services. It integrates data from two providers, **AybJet** and **HopeAir**, merges the results, and returns them sorted by price. The project is built on principles of microservice architecture, asynchronous communication, and data aggregation. It employs **Clean Architecture** and the **CQRS** (Command Query Responsibility Segregation) design pattern.

---

## Features
- **Microservice Integration**: Connects with HopeAir and AybJet APIs via asynchronous HTTP calls.
- **Asynchronous Data Management**: Continues processing even if one provider fails or responds slowly.
- **Price Sorting**: Merges and sorts flight results by price in ascending order before returning them.
- **Error Handling**: Handles provider errors gracefully, ensuring uninterrupted user experience.
- **BaseHttpClient Usage**: Provides modular and reusable API call management.
- **Reservation Mechanism**: Designed for potential future implementation.
- **Swagger UI**: Integrated for API documentation and testing.

---

## Technologies and Approaches Used
- **.NET 8**
- **Clean Architecture**
- **MediatR (CQRS)**
- **Asynchronous Programming**: Efficiently manages varying response times with `Task.WhenAll`.
- **Generic HTTP Client**: BaseHttpClient class for centralized API call management.
- **Unit Testing**: Components designed with testability in mind.

---

## Project Structure
- **API Layer**: Manages user interactions and provides endpoints.
- **Application Layer**: Contains business logic and handles queries and commands.
- **Infrastructure Layer**: Implements services to communicate with providers (HopeAir and AybJet).
- **Core Layer**: Includes reusable components such as BaseHttpClient and ApiResponse classes.

---

## AybJet and HopeAir Projects
As part of the `FlightSearch.API` infrastructure, separate projects for **AybJet** and **HopeAir** have been developed. These projects facilitate flight search operations and include the following features:

- **Provider Integration**: Customized configurations for each provider.
- **BaseHttpClient Usage**: Ensures consistent and efficient API calls.
- **Filtering Mechanism**: A filtering mechanism is implemented to refine flight data based on query parameters. However, **as specified in the case study, providers are required to return static data**, so the filtering logic has been commented out.

## Using the FlightSearch API
Send a GET request to /api/v1/Flights/search with the following payload:
```json
{
   "origin": "IST",
   "destination": "LHR",
   "departureDate": "2024-11-14",
   "returnDate": "2024-11-14",
   "passengerCount": 1
}
```
## Sample Response
Merged flight data from both providers is returned as follows:
```json
[
   {
      "flightNumber": "HH123",
      "departure": "IST",
      "arrival": "LHR",
      "price": 350.50,
      "currency": "USD",
      "departureTime": "2024-11-14T10:00:00",
      "arrivalTime": "2024-11-14T14:20:00",
      "duration": "4h 20m",
      "providerName": "HopeAir"
   },
   {
      "flightNumber": "AY456",
      "departure": "IST",
      "arrival": "LHR",
      "price": 380.75,
      "currency": "USD",
      "departureTime": "2024-11-14T11:00:00",
      "arrivalTime": "2024-11-14T15:25:00",
      "duration": "4h 25m",
      "providerName": "AybJet"
   }
]
```
## Acknowledgments

This project was developed as part of a case study provided by the RoofStacks Travel Team. It offered a valuable opportunity to improve my skills in microservice architecture, asynchronous communication, data aggregation, and error handling.

I sincerely thank the RoofStacks Travel Team for presenting this valuable experience and fostering a professional development environment. This process has significantly contributed to enhancing my ability to solve real-world software engineering challenges.
