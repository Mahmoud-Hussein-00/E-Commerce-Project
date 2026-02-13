using Domain.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderSpecifications : BaseSpecifications<Order, Guid>
    {
        public OrderSpecifications(Guid id) : base(i => i.Id == id)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.OrderItems);
        }

        public OrderSpecifications(string userEmail) : base(i => i.UserEmail == userEmail)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.OrderItems);
            AddOrderBy(O => O.OrderDate);
        }

    }
}
