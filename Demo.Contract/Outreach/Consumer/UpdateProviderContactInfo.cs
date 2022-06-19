using System;
using MassTransit;

namespace Demo.Contract.Outreach.Consumer
{
    public interface UpdateProviderContactInfo : CorrelatedBy<Guid>
    {
    }
}
