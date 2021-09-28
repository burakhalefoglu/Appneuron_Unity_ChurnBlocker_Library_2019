using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AppneuronUnity.Core.Extentions
{
    public class SerializableDictionary<TK, TV> : ISerializationCallbackReceiver
    {
        private Dictionary<TK, TV> _Dictionary;
        [SerializeField] List<TK> _Keys;
        [SerializeField] List<TV> _Values;

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }
}
