This project is a test solution demonstrating how to build a distributed system using RabbitMQ for messaging between services, Redis for shopping cart storage, and MongoDB for managing discounts. The system leverages gRPC instead of HTTP for inter-service communication and SignalR for real-time updates on the shopping cart page. The solution is fully containerized with Docker Compose for easy setup and deployment.

Key Features

RabbitMQ for Messaging: Enables asynchronous communication between services, supporting a decoupled, distributed architecture.
Redis for Cart Storage: Manages shopping cart data in Redis for fast, in-memory data retrieval.
MongoDB for Discounts: Stores discount data in MongoDB, offering a flexible and scalable data management solution.
gRPC for Inter-Service Communication: Replaces HTTP with gRPC to provide efficient, low-latency communication between services.
SignalR for Real-Time Cart Updates: The shopping cart page uses SignalR to provide real-time updates for a seamless user experience.
MassTransit Integration: Simplifies the management of RabbitMQ messaging, implementing an event-driven architecture across services.
Frontend with Blazor: The frontend is built using Blazor, enabling interactive and rich web UIs using C#.
Backend with .NET Core: The backend services are developed in .NET Core, ensuring performance and cross-platform compatibility.
Architecture

This project implements a microservices-based architecture with services dedicated to specific tasks:

Cart Service: Manages shopping cart data stored in Redis and offers real-time updates via SignalR.
Discount Service: Manages discount data in MongoDB and communicates with frontend using gRPC. It uses RabbitMQ to communicate with Cart Service.
Message Broker: RabbitMQ acts as the message broker, enabling asynchronous communication between services to support event-driven workflows.
Running the Project

The entire system is containerized with Docker Compose, allowing you to spin up all services with a single command.

Prerequisites
  Docker and Docker Compose installed on your machine.
  Steps to Run:
  Clone the repository.
  Navigate to the project directory.
  Run the following command to start all services:
  bash
  Copy code
  docker-compose up
  Access the frontend UI at http://localhost:5003 to start using the application.

Technologies Used
  RabbitMQ for messaging between services
  Redis for shopping cart storage
  MongoDB for managing discounts
  gRPC for inter-service communication, including client and broker interactions
  SignalR for real-time WebSocket communication
  Blazor for the frontend
  .NET Core for the backend services
  MassTransit for handling RabbitMQ messaging
  Docker Compose for container orchestration
