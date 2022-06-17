using System;
using MassTransit;

namespace QES.Demo.Contract.Outreach.Consumer
{
    public interface UpdateProviderContactInfo : CorrelatedBy<Guid>
    {
    }
}
