#Azure-Microservices-Demo

This project is a demo of Azure microservices using MassTransit

#Test Projects

Run the command `docker-compose up` to set up all external dependencies for the project (rabbitmq, seq, sql, etc)
Demo.* project demo publishing/receiving messages
Run Demo.Publisher to publish messages
Run Demo.Worker to consumer messages
Run Demo.Worker/scale.bat to demo a scaled consumer

SEQ URL: http://localhost:5342/#/events?autorefresh
RabbitMQ: http://localhost:15672/#/channels (guest/guest)
SQL: localhost,1401;Initial Catalog=Demo;User Id=sa;Password=8jkGh47hnDw89Haq8LN2