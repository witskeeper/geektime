using GeekTime.Domain;
using GeekTime.Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Ordering.Domain.OrderAggregate
{
    public class Order : Entity<long>, IAggregateRoot
    {
        public string UserId { get; private set; }

        public string UserName { get; private set; }

        public Address Address { get; private set; }

        public int ItemCount { get; private set; }

        protected Order()
        { }

        public Order(string userId, string userName, int itemCount, Address address)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.Address = address;
            this.ItemCount = itemCount;

            this.AddDomainEvent(new OrderCreatedDomainEvent(this));
        }


        public void ChangeAddress(Address address)
        {
            this.Address = address;
            //this.AddDomainEvent(new OrderAddressChangedDomainEvent(this));
        }



    }
}
