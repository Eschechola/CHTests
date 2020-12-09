using System;

namespace CHTests.Data.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Mount { get; set; }

        protected Product()
        {}

        public Product(long id, string name, string description, int mount)
        {
            Id = id == 0 ? GenerateId() : id;
            Name = name;
            Description = description;
            Mount = mount;
        }

        private long GenerateId() => new Random().Next(1, 9999);
    }
}
