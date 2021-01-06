using System;
using Domain.Models.Entities;

namespace Domain.Models.Products
{
    public class Description : Entity
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Description(string name, string value)
        {
            Id = Guid.NewGuid();
            Name = name;
            Value = value;
        }
    }
}