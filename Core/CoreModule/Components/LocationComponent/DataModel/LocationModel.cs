using AppneuronUnity.Core.Models.Abstract;
using System;

namespace AppneuronUnity.Core.CoreModule.Components.LocationComponent.DataModel
{
    /// <summary>
    /// Defines the <see cref="LocationModel" />.
    /// </summary>
    internal class LocationModel: IEntity
    {
        /// <summary>
        /// Gets or sets the ClientId.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectID.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the CustomerID.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the Continent.
        /// </summary>
        public string Continent { get; set; }

        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the Query.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the Region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the Org.
        /// </summary>
        public string Org { get; set; }

        private readonly DateTime dateTime = DateTime.Now;

    }
}
