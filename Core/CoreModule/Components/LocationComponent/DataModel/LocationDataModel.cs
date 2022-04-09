using AppneuronUnity.Core.Models.Abstract;
using System;

namespace AppneuronUnity.Core.CoreModule.Components.LocationComponent.DataModel
{
    internal class LocationDataModel: IEntity
    {
        public long ClientId { get; set; }
        public long ProjectId { get; set; }
        public long CustomerId { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Query { get; set; }
        public string Region { get; set; }
        public string Org { get; set; }

        private readonly DateTime dateTime = DateTime.Now;

    }
}
