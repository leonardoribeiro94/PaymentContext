using PaymentContext.Shared.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Shared.Entities
{
    public abstract class Entity : Notifiable
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}
