using System;
using System.Linq;
using MassTransit;
using QES.Demo.Contract.Outreach;
using QES.Demo.Contract.Outreach.Consumer;
using QES.Demo.Saga.Model;

namespace QES.Demo.Saga
{
    public class OutreachStateMachine : MassTransitStateMachine<OutreachState>
    {
        public State OutreachNeeded { get; private set; }
        public State RetrievingProviderInfo { get; private set; }
        public State Error { get; private set; }


        public OutreachStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Initially(
                When(BeginOutreach)
                    .Then(context =>
                    {
                        context.Saga.StartDate = DateTime.UtcNow;
                        context.Saga.ProviderId = context.Message.ProviderId;
                    })
                    .PublishAsync(async context =>
                        await context.Init<GetProviderData>(
                            new {context.Saga.CorrelationId, context.Message.ProviderId}))
                    .TransitionTo(RetrievingProviderInfo));

            During(RetrievingProviderInfo,
                When(GetProviderDataSuccess).Then(context =>
                {
                    context.Saga.EmailAddress = context.Message.EmailAddress;
                    context.Saga.PhoneNumber = context.Message.PhoneNumber;
                }).TransitionTo(OutreachNeeded));

            During(OutreachNeeded,
                When(StartOutreachEmail)
                    .Then(context =>
                    {
                        context.Saga.OutreachEmailAttempts.Add(new OutreachEmailAttempt()
                            {DateAttempted = DateTime.UtcNow});
                    }),
                When(OutreachEmailSuccess)
                    .Then(context =>
                    {
                        var emailAttempt =
                            context.Saga.OutreachEmailAttempts.OrderByDescending(a => a.DateAttempted).First();
                        emailAttempt.IsSuccessful = true;
                        context.Saga.AttestationCompleted = true;
                        context.Saga.CompleteDate = DateTime.UtcNow;
                    })
                    .Finalize(),
                When(StartOutreachPhoneCall)
                    .Then(context =>
                    {
                        context.Saga.OutreachPhoneAttempts.Add(new OutreachPhoneAttempt()
                            {DateAttempted = DateTime.UtcNow});
                    }),
                When(OutreachPhoneCallSuccess)
                    .Then(context =>
                    {
                        var phoneAttempt =
                            context.Saga.OutreachPhoneAttempts.OrderByDescending(a => a.DateAttempted).First();
                        phoneAttempt.IsSuccessful = true;
                        context.Saga.AttestationCompleted = true;
                        context.Saga.CompleteDate = DateTime.UtcNow;
                    })
                    .Finalize());

            DuringAny(
                When(UpdateContactInformation)
                    .Then(context =>
                    {
                        context.Saga.EmailAddress = context.Message.EmailAddress;
                        context.Saga.PhoneNumber = context.Message.PhoneNumber;
                    })
            );
        }

        public Event<BeginOutreach> BeginOutreach { get; }
        public Event<StartOutreachEmail> StartOutreachEmail { get; }
        public Event<OutreachEmailSuccess> OutreachEmailSuccess { get; }
        public Event<StartOutreachPhoneCall> StartOutreachPhoneCall { get; }
        public Event<OutreachPhoneCallSuccess> OutreachPhoneCallSuccess { get; }
        public Event<UpdateContactInformation> UpdateContactInformation { get; }
        public Event<GetProviderDataSuccess> GetProviderDataSuccess { get; }
    }
}
