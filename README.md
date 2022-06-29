# Azure-Microservices-Demo

This project is a demo of Azure microservices using MassTransit.  

Presented at the KC.NET user group 6/28/22.  
Slides available [here](https://docs.google.com/presentation/d/1j2pPF2XB-y4yVnTYnjJ9_hgerIwiRz9KW-VtQNFNRkU/edit?usp=sharing) 

# Test Projects

Run the command `docker-compose up` to set up all external dependencies for the project (rabbitmq, seq, sql, etc)  
Demo.* project demo publishing/receiving messages  
Run Demo.Publisher to publish messages  
Run Demo.Worker to consumer messages  

RabbitMQ: http://localhost:15672/#/channels (guest/guest)  
SQL: localhost,1401;Initial Catalog=Demo;User Id=sa;Password=8jkGh47hnDw89Haq8LN2

# Other links

[Mass Transit](https://masstransit-project.com/)  
[Microsoft Microservices Whitepaper](https://docs.microsoft.com/en-us/azure/architecture/microservices)  
[Microsoft Microservices eBook](https://dotnet.microsoft.com/en-us/download/e-book/microservices-architecture/pdf)  
[Microservices Blog Series](https://medium.com/aspnetrun/microservices-architecture-on-net-3b4865eea03f)  
[Microservices Udemy Course](https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/)  