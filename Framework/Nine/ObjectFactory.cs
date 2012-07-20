namespace Nine
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Content;

    class ObjectFactory : IObjectFactory
    {
        ContentManager content;
        IDictionary<string, object> prototypes;

        public ObjectFactory(IDictionary<string, object> prototypes, ContentManager content)
        {
            this.content = content;
            this.prototypes = prototypes;
        }

        public T Create<T>(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return default(T);
            if (prototypes.ContainsKey(typeName))
            {
                var cloneable = prototypes[typeName] as ICloneable;
                if (cloneable != null)
                    return (T)cloneable.Clone();
                return (T)Serialization.Clone(prototypes[typeName]);
            }
            return content.Create<T>(typeName);
        }
    }
}