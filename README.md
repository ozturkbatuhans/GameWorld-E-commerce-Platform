# GameWorld-E-commerce-Platform

This project is a **headless e-commerce solution** developed for **GameWorld**, a fictional Belgian e-commerce platform.

It was created as a **school project** to demonstrate skills in **API
design, integration, and data management**.

The goal is to automate product and order management processes by
integrating multiple technologies.

------------------------------------------------------------------------

##  Project Purpose and Features

###  Product Synchronization (ProductSyncer)

-   Fetches product data from an external **ERP system** via **SOAP
    service**.\
-   Synchronizes this data with the **WooCommerce platform** through a
    **REST API**.\
-   Automatically updates **stock levels**, **prices**, and **product
    details**.\
-   Creates new products in WooCommerce if they do not exist (with SKU,
    description, stock, and image).

###  Order Handling (OrderHandler)

-   Receives new orders from WooCommerce through the REST API.\
-   Stores order data in a **SQL Server database** using **Entity
    Framework Core**.\
-   Sends **email notifications** (via SMTP) to administrators and
    suppliers when a new order is placed.

###  Reporting (ReportsController)

-   Generates **CSV reports** based on order data.\
-   Monthly and supplier-based summaries can be exported to Excel.\
-   Helps calculate the amounts payable to each supplier.

###  Flexible Frontend Integration

-   Backend is designed to be used by different frontend technologies
    (e.g., **React**, **Vue**, **Angular**).\
-   Includes **example frontends** built with **Node.js (Express.js +
    Handlebars)** for REST and GraphQL.

------------------------------------------------------------------------

##  Technologies Used

-   **Backend:** ASP.NET Core (.NET 6)\
-   **Database:** SQL Server with Entity Framework Core\
-   **APIs & Integrations:**
    -   **SOAP** -- ERP Product Service\
    -   **REST API** -- WooCommerce Product & Order API\
    -   **SMTP** -- Email notifications\
-   **Frontend Examples:** Node.js, Express.js, Handlebars, GraphQL\
-   **Containerization:** Docker, Docker Compose\
-   **Version Control:** Git & GitHub

------------------------------------------------------------------------

##  Setup and Running

### 1. Clone the repository

``` bash
git clone https://github.com/[your-username]/gameworld-ecommerce.git
cd gameworld-ecommerce
```

### 2. Configure the database

-   Update `appsettings.json` with your **SQL Server connection
    string**.\
-   Run database migrations to create schema and seed initial data:

``` bash
dotnet ef database update
```

### 3. Configure WooCommerce API Keys

-   In WooCommerce, go to:\
    **WooCommerce → Settings → Advanced → REST API → Create API Key**\
-   Copy the **Consumer Key** and **Consumer Secret** into `Program.cs`.

### 4. Run the application with Docker

``` bash
docker-compose up --build
```

The API will run at: **https://localhost:5001**

### 5. Sync Products

Run the ProductSyncer console application to import products from ERP
into WooCommerce.

### 6. Test Orders

-   Place an order in WooCommerce.\
-   The order will be saved in the SQL Server database.\
-   Email notifications will be sent to the configured addresses.

### 7. Generate Reports

-   Navigate to `/Reports/DownloadCsv` in the browser.\
-   The report will be downloaded as a CSV file, which can be opened in
    Excel.

------------------------------------------------------------------------

## 📂 Project Structure

    GameWorld-Ecommerce/
    │── Controllers/
    │   ├── OrderController.cs        # Handles incoming WooCommerce orders
    │   ├── ReportsController.cs      # Generates CSV reports
    │   └── HomeController.cs         # Default pages and health checks
    │
    │── Data/
    │   ├── OrderDbContext.cs         # EF Core DbContext for SQL Server
    │   ├── Order.cs                  # Order entity
    │   └── OrderDbContextFactory.cs  # Context factory for migrations
    │
    │── Models/
    │   ├── Product.cs                # Product entity
    │   └── ErrorViewModel.cs         # Error handling model
    │
    │── Services/
    │   └── SoapServiceClient.cs      # Connects to ERP via SOAP
    │
    │── Views/                        # Razor views for MVC pages
    │
    │── Migrations/                   # EF Core migrations
    │
    │── Program.cs                    # Main entry point with sync logic
    │── docker-compose.yml            # Docker setup for the project
    │── appsettings.json              # Configuration (DB, SMTP, API keys)

------------------------------------------------------------------------

##  Key Features Demonstrated

-   Integration between **SOAP service** (ERP) and **WooCommerce REST
    API**.\
-   **Product synchronization** with stock and price updates.\
-   **Order receiving** and **storage in SQL Server**.\
-   **Automatic email notifications** on new orders.\
-   **CSV/Excel report generation** for suppliers.\
-   Example **frontend implementations** (REST & GraphQL).\
-   Containerized setup with **Docker Compose**.

------------------------------------------------------------------------

##  Possible Improvements (Future Work)

-   Webhook testing for real-time order updates.\
-   Deployment to external hosting (e.g., Combell).\
-   Advanced reporting with charts and dashboards.\
-   Authentication and role-based access for admin panel.

------------------------------------------------------------------------

## 👨‍💻 Author

Developed as part of the **Programming Advanced Architectures** course
at **VIVES University of Applied Sciences**.
