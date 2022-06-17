using System;
using System.Collections.Generic;
using MassTransit;

namespace QES.Demo.Saga.Model
{
    public class OutreachState : SagaStateMachineInstance
    {
        public OutreachState()
        {
            OutreachEmailAttempts = new List<OutreachEmailAttempt>();
            OutreachPhoneAttempts = new List<OutreachPhoneAttempt>();
        }
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int ProviderId { get; set; }
        public List<OutreachEmailAttempt> OutreachEmailAttempts { get; set; }
        public List<OutreachPhoneAttempt> OutreachPhoneAttempts { get; set; }
        public bool AttestationCompleted { get; set; }
    }
}
