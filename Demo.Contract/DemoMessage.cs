using MassTransit;
using System;

namespace Demo.Contract
{
    public interface DemoMessage : CorrelatedBy<Guid>
    {
        string MessageNumber { get; set; }
    }
}
