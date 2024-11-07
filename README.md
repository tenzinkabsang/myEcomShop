# myEcomShop ASP.NET Core Application

Sample reference application demonstrating a simple microservices architecture approach utilizing the following technologies:

- .NET 9.0
- C# 12
- SQL Server
- RabbitMq
- Redis
- Serilog
- Microsoft Resilience Package (Polly)
- EntityFramework Core
- Dapper


## Running the application using Docker

You can run all four of the applications (web, catalog-api, orders-api, queue-processor) by running these commands from the root folder (where the .sln file is located):

```
docker-compose build
docker-compose up
```

You should be able to make requests to https://localhost:5201/home/index for the Web project, http://localhost:5011/swagger/index.html for the Catalog API, and http://localhost:5012/swagger/index.html for Orders API once these commands complete.



## Home page

![myEcomShop landing page screenshot](https://raw.githubusercontent.com/tenzinkabsang/myecomshop/main/.github/images/landing.png)

## Item details page

![myEcomShop details page](https://raw.githubusercontent.com/tenzinkabsang/myecomshop/main/.github/images/product-details.png)
