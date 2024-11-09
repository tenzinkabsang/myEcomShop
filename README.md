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


## Running the application `(F5 with Visual Studio)` OR using Docker

You can run all four of the applications (web, catalog-api, orders-api, queue-processor) by running these commands from the root folder (where the .sln file is located):

```
docker-compose build
docker-compose up
```

You should be able to make requests to: 

- [Web Project](https://localhost:5201/home/index) `https://localhost:5201/home/index`
- [Catalog API](http://localhost:5011/swagger/index.html) `http://localhost:5011/swagger/index.html`
- [Orders API](http://localhost:5012/swagger/index.html) `http://localhost:5012/swagger/index.html`
- [RabbitMq](http://localhost:8088/) `http://localhost:8088 username: guest password: guest`
- [Serilog Seq Logging](http://localhost:5342) `http://localhost:5342`



## Home page

![myEcomShop landing page screenshot](https://raw.githubusercontent.com/tenzinkabsang/myecomshop/main/.github/images/landing.png)

## Item details page

![myEcomShop details page](https://raw.githubusercontent.com/tenzinkabsang/myecomshop/main/.github/images/product-details.png)

## RabbitMq page
![myEcomShop Rabbit page](https://raw.githubusercontent.com/tenzinkabsang/myecomshop/main/.github/images/rabbitmq-queue.png)

## Serilog Seq page
![myEcomShop Seqs page](https://raw.githubusercontent.com/tenzinkabsang/myecomshop/main/.github/images/seq.png)
