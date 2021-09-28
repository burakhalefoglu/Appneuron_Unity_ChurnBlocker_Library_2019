namespace AppneuronUnity.Core.AuthModule.AuthComponent.DataModel
{
    using System;

    [Serializable]
    internal class TokenDataModel
    {

        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
