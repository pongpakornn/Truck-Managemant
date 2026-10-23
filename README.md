# NEXUS Transport

Transport & Logistics Management System using **Next.js, React, TypeScript, Tailwind CSS, ASP.NET Core Web API, EF Core, SQL Server, and Git**.

## Project layout
- `frontend/` — Next.js dashboard (Load Planning, shipment cards, truck slots, dispatch schedule)
- `backend/NexusTransport.Api/` — REST API, EF Core models and SQL Server data context
- `database/schema.sql` — optional SQL Server schema reference

## Prerequisites
- Node.js 20+
- .NET 8 SDK
- SQL Server / SQL Server Express / LocalDB
- Git

## Run the frontend
```bash
cd frontend
npm install
```
Copy `.env.example` to `.env.local` and set `NEXT_PUBLIC_API_BASE_URL` if the API URL differs. Start Next.js:
```bash
npm run dev
```
Open http://localhost:3000.

## Run the API
Check the SQL Server connection string in `backend/NexusTransport.Api/appsettings.json`. The default is Windows LocalDB. If using SQL Server Express, update it to your server name.
```bash
cd backend/NexusTransport.Api
dotnet restore
dotnet run
```
In Development, Swagger is available at the URL shown by `dotnet run` plus `/swagger`; the health check is `/health`.

## Create the database using EF Core
Install the EF CLI if needed, then run from `backend/NexusTransport.Api`:
```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```
Alternatively, run `database/schema.sql` in SQL Server Management Studio. Use **one approach only** on a new database.

## API endpoints
- `GET /api/shipments` — list shipments; optional `search` and `status` query parameters
- `GET /api/shipments/{id}` — shipment detail
- `POST /api/shipments` — create shipment
- `PATCH /api/shipments/{id}/status` — update status
- `GET /api/trucks` — list trucks
- `POST /api/trucks` — create truck
- `POST /api/trucks/{truckId}/assign/{shipmentId}` — assign a ready shipment after capacity validation
- `GET /api/drivers` — list drivers
- `POST /api/drivers` — create driver

## Current scope and known limitations
The dashboard falls back to demo data when the API is not available. Slot selection and assignment UI are currently prototype interactions; assignment is not yet posted from the frontend to the backend. Dashboard KPIs and schedule currently include sample values. Authentication/authorization, audit logs, dispatch trip persistence, integration tests, and production deployment are not yet implemented.

Never commit real secrets. Verify builds, migrations, capacity and dispatch rules, security, and business requirements before production use.
