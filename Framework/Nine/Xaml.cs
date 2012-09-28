namespace System.Windows.Markup
{
    using Microsoft.Xna.Framework.Content;
    using System.Collections.Generic;
    using System.Xaml;

#if !WINDOWS
#if !SILVERLIGHT
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ContentPropertyAttribute : Attribute
    {
        public ContentPropertyAttribute() { }
        public ContentPropertyAttribute(string name) { Name = name; }
        public string Name { get; private set; }
    }
#endif

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(string name) { }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RuntimeNamePropertyAttribute : Attribute
    {
        public RuntimeNamePropertyAttribute(string name) { Name = name; }
        public string Name { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DictionaryKeyPropertyAttribute : Attribute
    {
        public DictionaryKeyPropertyAttribute(string name) { Name = name; }
        public string Name { get; private set; }
    }
#endif

    class AttachableMemberIdentifierCollection : Dictionary<AttachableMemberIdentifier, object>
    {
        public AttachableMemberIdentifierCollection() { }
        public AttachableMemberIdentifierCollection(int capacity) : base(capacity) { }
    }

    class AttachableMemberIdentifierCollectionReader : ContentTypeReader<AttachableMemberIdentifierCollection>
    {
        protected override AttachableMemberIdentifierCollection Read(ContentReader input, AttachableMemberIdentifierCollection existingInstance)
        {
            var count = input.ReadInt32();
            var result = new AttachableMemberIdentifierCollection(count);
            for (var i = 0; i < count; ++i)
            {
                result.Add(input.ReadObject<AttachableMemberIdentifier>(), input.ReadObject<object>());
            }
            return result;
        }
    }
}