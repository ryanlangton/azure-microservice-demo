using System;
using System.Collections.Generic;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using QES.Demo.Saga.Configuration;

namespace QES.Demo.Saga
{
    public class OutreachStateDbContext : SagaDbContext
    {
        public OutreachStateDbContext(DbContextOptions<OutreachStateDbContext> options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OutreachStateConfiguration(); }
        }
    }
}
