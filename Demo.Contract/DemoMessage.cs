using MassTransit;
using System;

namespace QES.Demo.Contract
{
    public interface DemoMessage : CorrelatedBy<Guid>
    {
        string MessageNumber { get; set; }
    }
}
