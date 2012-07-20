﻿namespace Nine.Content
{
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

    #region QuadTree<T>
    [ContentTypeWriter()]
    internal class QuadTreeWriter<T> : ContentTypeWriter<QuadTree<T>>
    {
        protected override void Write(ContentWriter output, QuadTree<T> value)
        {
            output.Write(value.MaxDepth);
            output.WriteRawObject<QuadTreeNode<T>>(value.Root, new QuadTreeNodeWriter<T>());
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(QuadTree<T>).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(QuadTreeReader<T>).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter()]
    internal class QuadTreeNodeWriter<T> : ContentTypeWriter<QuadTreeNode<T>>
    {
        protected override void Write(ContentWriter output, QuadTreeNode<T> value)
        {
            output.Write(value.HasChildren);
            output.Write(value.Depth);
            output.WriteObject<BoundingRectangle>(value.Bounds);
            output.WriteObject<T>(value.Value);

            if (value.HasChildren)
                output.WriteObject(value.Children.ToArray());
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(QuadTreeNode<T>).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(QuadTreeNodeReader<T>).AssemblyQualifiedName;
        }
    }
    #endregion

    #region Octree<T>
    [ContentTypeWriter()]
    internal class OctreeWriter<T> : ContentTypeWriter<Octree<T>>
    {
        protected override void Write(ContentWriter output, Octree<T> value)
        {
            output.Write(value.MaxDepth);
            output.WriteRawObject<OctreeNode<T>>(value.Root, new OctreeNodeWriter<T>());
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Octree<T>).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(OctreeReader<T>).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter()]
    internal class OctreeNodeWriter<T> : ContentTypeWriter<OctreeNode<T>>
    {
        protected override void Write(ContentWriter output, OctreeNode<T> value)
        {
            output.Write(value.HasChildren);
            output.Write(value.Depth);
            output.WriteObject<BoundingBox>(value.Bounds);
            output.WriteObject<T>(value.Value);

            if (value.HasChildren)
                output.WriteObject(value.Children.ToArray());
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(OctreeNode<T>).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(OctreeNodeReader<T>).AssemblyQualifiedName;
        }
    }
    #endregion
}
