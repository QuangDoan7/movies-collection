# Movies Collection Web Application

## Overview

Movies Collection is a full-stack web application built with ASP.NET Razor Pages that allows users to manage their personal movie collections.

The application supports authentication, role-based authorization, and user-specific data isolation. Each user maintains a separate collection, while administrators have extended privileges for user management.

---

## Features

### Authentication & Authorization

- User registration and login using ASP.NET Identity
- Role-based access control (Admin / User)
- Protected routes using authorization policies

### Movie Collection Management

- Create, view, update, and delete movies
- Each movie is associated with a specific user
- Users can only access their own collections

### Genre Handling

- Multi-select genre support
- Filter movies by genre

### Admin Features

- View all registered users
- Delete users
- Cascade delete user-related movie collections
- Restricted access to admin-only pages

---

## Architecture

```
Client (Browser)
    ↓
Razor Pages (UI + PageModel)
    ↓
Entity Framework Core
    ↓
SQLite Database
```

### Data Relationships

```
AspNetUsers (Identity)
        ↓ (1-to-many)
Movies (UserId foreign key)
```

---

## Tech Stack

### Backend

- ASP.NET Core Razor Pages (.NET 9)
- Entity Framework Core
- ASP.NET Identity

### Database

- SQLite

### Frontend

- Razor Pages
- Bootstrap 5

---

### File Upload

- Images stored in `wwwroot/movieimg`
- Unique file naming using GUID
- Fallback image support for missing posters

---

## Setup Instructions

### 1. Clone Repository

```bash
git clone https://github.com/QuangDoan7/movies-collection.git
cd movies-collection
```

### 2. Install Dependencies

```bash
dotnet restore
```

### 3. Apply Migrations

```bash
dotnet ef database update
```

### 4. Run Application

```bash
dotnet run
```

---

## Deployment

The application can be deployed using:

- Azure App Service (Free Tier)
- GitHub Actions (CI/CD)

---

## Future Improvements

- Pagination for large datasets
- Search functionality
- Image storage using cloud services (Azure Blob Storage)
- User profile customization
- API layer for mobile integration

---

## License

This project is licensed under the MIT License.

___

## What I Learned

- Implementing authentication and authorization using ASP.NET Identity
- Designing multi-user systems with data isolation
- Handling file uploads in web applications
- Managing relational data with Entity Framework Core
- Applying role-based access control in real-world scenarios