# Popular Search Service - Web API

##  Project Overview

A RESTful Web API built with ASP.NET Core that tracks user searches and provides insights into the most popular items searched by clients. This project implements a three-tier architecture for better separation of concerns and maintainability.

**Assignment by:** Earthlink Communications  
**Developer:** Aymen Alhadidi  

---

##  Architecture

The project follows a **Three-Tier Architecture**:

- **API Layer** - Handles HTTP requests and responses (Controllers)
- **Business Logic Layer** - Contains business rules and validation
- **Data Access Layer** - Manages database operations
- **DTOs Layer** - Data Transfer Objects for communication between layers

---

## ✨ Features Implemented

###  Required Features
- [x] **Clients CRUD Operations** - Create, Read, Update, Delete clients
- [x] **Save Search Events** - Record user search activities
- [x] **Get Popular Searches** - Retrieve top 20 most searched items in the last 30 days per client
- [x] **Swagger Documentation** - Interactive API documentation

###  Bonus Features
- [x] **Validation & Error Handling** - Input validation and comprehensive error responses
- [x] **Find Operations** - Additional search functionality for clients and items

---

## 🛠️ Technologies Used

- **ASP.NET Core 8.0** - Web API Framework
- **SQL Server** - Database (instead of PostgreSQL as approved)
- **Hangfire** - Background job processing
- **Swagger/OpenAPI** - API Documentation
- **ADO.NET** - Data Access

---

## 📦 Prerequisites

Before running the project, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or higher)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) - Optional

---

## 🚀 Installation & Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/Aymn-Alhadidi/Search_WebAPI.git
cd Search_WebAPI
```

### Step 2: Create Database

Open SQL Server Management Studio (SSMS) and run the SQL script provided below to create the required tables.

### Step 3: Configure Connection String

Navigate to the **Data Access Layer** and update the connection string in `clsDataAccessSettings.cs`:

```csharp
public static string ConnectionString = 
    "Server=YOUR_SERVER_NAME;Database=SearchServiceDB;Integrated Security=true;";
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name (e.g., `localhost` or `.` or `DESKTOP-XXXXX`).

### Step 4: Restore NuGet Packages

```bash
dotnet restore
```

### Step 5: Build the Project

```bash
dotnet build
```

### Step 6: Run the Application

```bash
dotnet run --project Search_WebAPI
```

Or press **F5** in Visual Studio.

The API will start at: `https://localhost:7XXX` (port may vary)

### Step 7: Access Swagger UI

Open your browser and navigate to:
```
https://localhost:7XXX/swagger
```

---

## 📊 Database Schema

### SQL Script to Create Tables

```sql

-- 1. Clients Table
CREATE TABLE Clients (
    id NVARCHAR(255) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE()
);

-- 2. Items Table
CREATE TABLE Items (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE()
);

-- 3. Searches Table (Current searches - last 30 days)
CREATE TABLE Searches (
    id INT PRIMARY KEY IDENTITY(1,1),
    client_id NVARCHAR(255) NOT NULL,
    keyword NVARCHAR(255) NOT NULL,
    item_id INT NULL,
    item_name NVARCHAR(255) NULL,
    searched_at DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT fk_searches_client
        FOREIGN KEY (client_id) 
        REFERENCES Clients(id)
        ON DELETE CASCADE,
    CONSTRAINT fk_searches_item
        FOREIGN KEY (item_id) 
        REFERENCES Items(id)
        ON DELETE SET NULL
);


---

## 🔌 API Endpoints

### 👥 Clients API

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/clients` | Get all clients |
| GET | `/api/clients/{id}` | Get client by ID |
| POST | `/api/clients` | Create new client |
| PUT | `/api/clients/{id}` | Update client |
| DELETE | `/api/clients/{id}` | Delete client |

### 📦 Items API

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/items` | Get all items |
| GET | `/api/items/{id}` | Get item by ID |
| POST | `/api/items` | Create new item |
| PUT | `/api/items/{id}` | Update item |
| DELETE | `/api/items/{id}` | Delete item |

### 🔍 Searches API

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/searches` | Save a search event |
| GET | `/api/searches/popular/{clientId}` | Get top 20 popular items for a client (last 30 days) |

---

## 📝 Example Requests

### 1. Create a Client

**Request:**
```http
POST /api/clients
Content-Type: application/json

{
  "id": "client-123-abc",
  "name": "Tech Store"
}
```

**Response:**
```json
{
  "id": "client-123-abc",
  "name": "Tech Store",
  "created_at": "2025-11-11T10:30:00Z"
}
```

### 2. Create an Item

**Request:**
```http
POST /api/items
Content-Type: application/json

{
  "name": "iPhone 15 Pro"
}
```

**Response:**
```json
{
  "id": 1,
  "name": "iPhone 15 Pro",
  "created_at": "2025-11-11T10:35:00Z"
}
```

### 3. Save a Search Event

**Request:**
```http
POST /api/searches
Content-Type: application/json

{
  "client_id": "client-123-abc",
  "keyword": "iphone",
  "item_id": 1,
  "item_name": "iPhone 15 Pro",
  "searched_at": "2025-11-11T10:40:00Z"
}
```

**Response:**
```json
{
  "id": 1,
  "client_id": "client-123-abc",
  "keyword": "iphone",
  "item_id": 1,
  "item_name": "iPhone 15 Pro",
  "searched_at": "2025-11-11T10:40:00Z"
}
```

### 4. Get Popular Searches

**Request:**
```http
GET /api/searches/popular/client-123-abc
```

**Response:**
```json
[
  {
    "item_id": 1,
    "item_name": "iPhone 15 Pro",
    "search_count": 150
  },
  {
    "item_id": 2,
    "item_name": "Samsung Galaxy S24",
    "search_count": 120
  }
]
```

---

## ⚙️ Background Job (Hangfire)

The project includes a **Hangfire background job** that automatically runs **daily at 2:00 AM** to:
- Move searches older than 30 days from `Searches` table to `OldSearches` table
- Keep the main searches table optimized for recent queries

### Access Hangfire Dashboard

Navigate to:
```
https://localhost:7XXX/hangfire
```

**Note:** In production, secure this dashboard with authentication.

---

## 🧪 Testing the API

### Using Swagger UI
1. Run the application
2. Open `https://localhost:7XXX/swagger`
3. Try out each endpoint directly from the browser

### Using Postman
1. Import the API endpoints
2. Set `Content-Type: application/json` header
3. Send requests with appropriate JSON payloads

### Using cURL

```bash
# Create a client
curl -X POST https://localhost:7XXX/api/clients \
  -H "Content-Type: application/json" \
  -d '{"id":"test-client","name":"Test Store"}'

# Get popular searches
curl -X GET https://localhost:7XXX/api/searches/popular/test-client
```

---

## 📁 Project Structure

```
Search_WebAPI/
├── Search_WebAPI/              # API Layer (Controllers)
│   ├── Controllers/
│   │   ├── ClientsController.cs
│   │   ├── ItemsController.cs
│   │   └── SearchesController.cs
│   ├── Program.cs
│   └── appsettings.json
├── BusinessLayer/              # Business Logic Layer
│   ├── clsClient.cs
│   ├── clsItem.cs
│   └── clsSearch.cs
├── DataAccessLayer/            # Data Access Layer
│   ├── clsClientData.cs
│   ├── clsItemData.cs
│   ├── clsSearchData.cs
│   └── clsDataAccessSettings.cs
└── DTOs/                       # Data Transfer Objects
    ├── ClientDTO.cs
    ├── ItemDTO.cs
    └── SearchDTO.cs
```

---

## ⚠️ Known Issues / Notes

- **Database:** SQL Server is used instead of PostgreSQL (as discussed and approved)
- **Background Jobs:** Hangfire is configured to run daily at 2 AM server time
- **Authentication:** Not implemented (can be added for production)
- **Validation:** Basic validation is implemented; can be enhanced with FluentValidation

---

## 🔒 Security Considerations

For production deployment, consider:
- Adding authentication/authorization (JWT tokens)
- Securing Hangfire dashboard
- Using parameterized queries (already implemented)
- Adding rate limiting
- Implementing logging and monitoring

---

## 🤝 Contact

For any questions or clarifications:

**Developer:** Aymen Alhadidi  
**Assignment Supervisor:** Mr. Muhaymin Tariq al-Ani  
**Email:** muhtariq@earthlink.iq  
**Organization:** Earthlink Communications

---

## 📄 License

This project is developed as part of a technical assessment for Earthlink Communications.

---

**Thank you for reviewing my submission!** 🚀
