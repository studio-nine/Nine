﻿// -----------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a text template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------------

#if !TEXT_TEMPLATE
namespace Nine.Physics
{
    /// <summary>
    /// Content reader for <c>RigidBody</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class RigidBodyReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.RigidBody>
    {
        protected override Nine.Physics.RigidBody Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.RigidBody existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new RigidBody();
            existingInstance.Collider = input.ReadObject<Nine.Physics.Colliders.Collider>();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
}
namespace Nine.Physics.Colliders
{
    /// <summary>
    /// Content reader for <c>BoxCollider</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class BoxColliderReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.Colliders.BoxCollider>
    {
        protected override Nine.Physics.Colliders.BoxCollider Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.Colliders.BoxCollider existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new BoxCollider();
            existingInstance.Size = input.ReadVector3();
            existingInstance.Enabled = input.ReadBoolean();
            existingInstance.Mass = input.ReadSingle();
            existingInstance.Friction = input.ReadObject<Nine.Range<System.Single>>();
            existingInstance.Restitution = input.ReadSingle();
            existingInstance.Transform = input.ReadMatrix();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
    /// <summary>
    /// Content reader for <c>CapsuleCollider</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class CapsuleColliderReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.Colliders.CapsuleCollider>
    {
        protected override Nine.Physics.Colliders.CapsuleCollider Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.Colliders.CapsuleCollider existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new CapsuleCollider();
            existingInstance.Height = input.ReadSingle();
            existingInstance.Radius = input.ReadSingle();
            existingInstance.Enabled = input.ReadBoolean();
            existingInstance.Mass = input.ReadSingle();
            existingInstance.Friction = input.ReadObject<Nine.Range<System.Single>>();
            existingInstance.Restitution = input.ReadSingle();
            existingInstance.Transform = input.ReadMatrix();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
    /// <summary>
    /// Content reader for <c>CompoundCollider</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class CompoundColliderReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.Colliders.CompoundCollider>
    {
        protected override Nine.Physics.Colliders.CompoundCollider Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.Colliders.CompoundCollider existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new CompoundCollider();
            {
                var count = input.ReadInt32();
                for (var i = 0; i < count; ++i)
                    existingInstance.Colliders.Add(input.ReadObject<Nine.Physics.Colliders.Collider>());
            }
            existingInstance.Enabled = input.ReadBoolean();
            existingInstance.Mass = input.ReadSingle();
            existingInstance.Friction = input.ReadObject<Nine.Range<System.Single>>();
            existingInstance.Restitution = input.ReadSingle();
            existingInstance.Transform = input.ReadMatrix();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
    /// <summary>
    /// Content reader for <c>ConeCollider</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class ConeColliderReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.Colliders.ConeCollider>
    {
        protected override Nine.Physics.Colliders.ConeCollider Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.Colliders.ConeCollider existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new ConeCollider();
            existingInstance.Height = input.ReadSingle();
            existingInstance.Radius = input.ReadSingle();
            existingInstance.Enabled = input.ReadBoolean();
            existingInstance.Mass = input.ReadSingle();
            existingInstance.Friction = input.ReadObject<Nine.Range<System.Single>>();
            existingInstance.Restitution = input.ReadSingle();
            existingInstance.Transform = input.ReadMatrix();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
    /// <summary>
    /// Content reader for <c>CylinderCollider</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class CylinderColliderReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.Colliders.CylinderCollider>
    {
        protected override Nine.Physics.Colliders.CylinderCollider Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.Colliders.CylinderCollider existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new CylinderCollider();
            existingInstance.Height = input.ReadSingle();
            existingInstance.Radius = input.ReadSingle();
            existingInstance.Enabled = input.ReadBoolean();
            existingInstance.Mass = input.ReadSingle();
            existingInstance.Friction = input.ReadObject<Nine.Range<System.Single>>();
            existingInstance.Restitution = input.ReadSingle();
            existingInstance.Transform = input.ReadMatrix();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
    /// <summary>
    /// Content reader for <c>ModelCollider</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class ModelColliderReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.Colliders.ModelCollider>
    {
        protected override Nine.Physics.Colliders.ModelCollider Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.Colliders.ModelCollider existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new ModelCollider();
            existingInstance.Source = input.ReadObject<Microsoft.Xna.Framework.Graphics.Model>();
            existingInstance.CollisionMesh = input.ReadObject<System.String>();
            existingInstance.Enabled = input.ReadBoolean();
            existingInstance.Mass = input.ReadSingle();
            existingInstance.Friction = input.ReadObject<Nine.Range<System.Single>>();
            existingInstance.Restitution = input.ReadSingle();
            existingInstance.Transform = input.ReadMatrix();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
    /// <summary>
    /// Content reader for <c>SphereCollider</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class SphereColliderReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.Colliders.SphereCollider>
    {
        protected override Nine.Physics.Colliders.SphereCollider Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.Colliders.SphereCollider existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new SphereCollider();
            existingInstance.Radius = input.ReadSingle();
            existingInstance.Enabled = input.ReadBoolean();
            existingInstance.Mass = input.ReadSingle();
            existingInstance.Friction = input.ReadObject<Nine.Range<System.Single>>();
            existingInstance.Restitution = input.ReadSingle();
            existingInstance.Transform = input.ReadMatrix();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
    /// <summary>
    /// Content reader for <c>TerrainCollider</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Content.Reader.tt", "1.1.0.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class TerrainColliderReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Nine.Physics.Colliders.TerrainCollider>
    {
        protected override Nine.Physics.Colliders.TerrainCollider Read(Microsoft.Xna.Framework.Content.ContentReader input, Nine.Physics.Colliders.TerrainCollider existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new TerrainCollider();
            existingInstance.Heightmap = input.ReadObject<Nine.Graphics.Heightmap>();
            existingInstance.Enabled = input.ReadBoolean();
            existingInstance.Mass = input.ReadSingle();
            existingInstance.Friction = input.ReadObject<Nine.Range<System.Single>>();
            existingInstance.Restitution = input.ReadSingle();
            existingInstance.Transform = input.ReadMatrix();
            existingInstance.Name = input.ReadObject<System.String>();
            existingInstance.Tag = input.ReadObject<System.Object>();
            existingInstance.AttachedProperties = input.ReadObject<System.Collections.Generic.Dictionary<System.Xaml.AttachableMemberIdentifier, System.Object>>();
            return existingInstance;
        }
    }
}
#endif