    # Ats-Demo

## 🚀 Project Overview
Ats-Demo is a modular and scalable project implementing **Clean Architecture** with **CQRS, MediatR, and Redis Caching**. It is designed to handle **high-performance backend operations** with a microservices-based approach. The system integrates **Azure Service Bus** for message-driven communication and utilizes **MongoDB** for read-heavy operations.

### 🔹 **Key Features**
- **Clean Architecture**: Separation of concerns between application layers.
- **CQRS with MediatR**: Command-Query Responsibility Segregation for better scalability.
- **Redis Caching**: Improves performance by caching frequently accessed data.
- **MongoDB for Reads**: Implements **eventual consistency** using Azure Service Bus.
- **Unit Testing**: Includes unit tests for core functionalities.

---
## 📂 **Branches Overview**
This repository contains multiple branches, each representing a significant feature or update:

| Branch Name                              | Description |
|------------------------------------------|------------------------------------------------|
| **main**                                 | Stable branch with the latest features. |
| **signals-lazyloading-unitofwork**       | Implements Angular Signals, Lazy Loading, and Unit of Work. |
| **clean-architecture-uis**               | Introduces Clean Architecture with UI components. |
| **azure-servicebus-sync-db**             | Implements **Azure Service Bus** to synchronize databases. |
| **redis-validators-exception-middleware** | Enhances caching with Redis and exception handling middleware. |
| **cqrs-mediatr**                         | Integrates **CQRS & MediatR** for command/query segregation. |

---
## 🛠️ **Requirements**
To run the project, ensure you have the following installed:

### **1️⃣ Core Requirements**
- **.NET SDK** `>= 7.0`
- **Node.js** `>= 18.0` (for frontend UI)
- **Docker** `>= 20.10` (for running Redis & MongoDB containers)
- **Azure Service Bus** (optional but required for full functionality)

### **2️⃣ Required Services** (Using Docker)
To avoid manual setup, use Docker to run Redis & MongoDB:

```bash
# Start Redis and MongoDB in Docker
docker run --name redis -d -p 6379:6379 redis

docker run --name mongodb -d -p 27017:27017 mongo
```

## 📖 **Usage**
- **Access the API via** `http://localhost:5000/swagger`
- **Use MongoDB Compass or any Mongo client to inspect the database**
- **Check Redis cache via CLI**`: redis-cli then KEYS *`
