#QESI-Dashboard

The QESI dashboard allows for monitoring QESI provider ingestion and configuring clients

**LocalHost:** http://localhost:4200/

**Development:** https://qes-clientdashboard.dev.questanalytics.com

**UAT:** https://uat-clientdashboard.questanalytics.com

See this [README.md](https://dev.azure.com/questanalytics/Payer%20Products/_git/QES-Shared?path=/QESI-Dashboard/README.md) file for more detailed instructions on how to run the dashboard project locally.
    
#Mass Transit Job Manager

Manages Mass Transit Jobs (long running jobs >5 minutes that are implemented using an IJobConsumer<T>)

#Test Projects

Run the command `docker-compose up` to set up all external dependencies for the project (rabbitmq, seq, sql, etc)
QES.Demo.* project demo publishing/receiving messages
Run QES.Demo.Publisher to publish messages
Run QES.Demo.Worker to consumer messages
Run QES.Demo.Worker/scale.bat to demo a scaled consumer

SEQ URL: http://localhost:5342/#/events?autorefresh
RabbitMQ: http://localhost:15672/#/channels (guest/guest)
SQL: localhost,1401;Initial Catalog=Demo;User Id=sa;Password=8jkGh47hnDw89Haq8LN2