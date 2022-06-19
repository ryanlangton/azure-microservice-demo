using System;

namespace Demo.Saga.Model
{
    public class OutreachEmailAttempt
    {
        public int Id { get; set; }
        public Guid OutreachStateId { get; set; }
        public OutreachState OutreachState { get; set; }
        public DateTime DateAttempted { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
