using System;

namespace QES.Demo.Saga.Model
{
    public class OutreachPhoneAttempt
    {
        public int Id { get; set; }
        public Guid OutreachStateId { get; set; }
        public OutreachState OutreachState { get; set; }
        public DateTime DateAttempted { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
