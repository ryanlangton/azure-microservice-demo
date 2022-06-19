using System;
using System.Collections.Generic;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Demo.Saga.Configuration;

namespace Demo.Saga
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
