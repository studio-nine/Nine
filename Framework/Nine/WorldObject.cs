#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

#endregion

namespace Nine
{
    /// <summary>
    /// Defines an object that has a position, rotation and scale.
    /// </summary>
    [Serializable]
    [ContentProperty("Components")]
    [RuntimeNameProperty("Name")]
    [DictionaryKeyProperty("Name")]
    public sealed class WorldObject : ITransformable, IUpdateable, IDrawable, ICloneable
    {
        #region World
        /// <summary>
        /// Gets the containing world of this world object.
        /// </summary>
        [XmlIgnore]
        public World World
        {
            get { return world; }
            internal set
            {
                if (world != value)
                {
                    if (value != null)
                    {
                        if (world != null)
                        {
                            throw new InvalidOperationException();
                        }
                        world = value;
                        OnAdded();
                    }
                    else
                    {
                        if (world == null)
                        {
                            throw new InvalidOperationException();
                        }
                        OnRemoved();
                        world = null;
                    }
                    world = value;
                }
            }
        }
        private World world;


        private void OnAdded()
        {
            if (components != null)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    var gameComponent = components[i] as IComponent;
                    if (gameComponent != null)
                        gameComponent.Parent = this;
                }
            }
        }

        private void OnRemoved()
        {
            if (components != null)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    var gameComponent = components[i] as IComponent;
                    if (gameComponent != null)
                        gameComponent.Parent = null;
                }
            }
        }
        #endregion

        #region Components
        /// <summary>
        /// Gets a collection of components for this game object container.
        /// </summary>
        [XmlArrayItem("Component")]
        public NotificationCollection<object> Components
        {
            get
            {
                if (components == null)
                {
                    components = new NotificationCollection<object>();
                    components.Added += new EventHandler<NotifyCollectionChangedEventArgs<object>>(components_Added);
                    components.Removed += new EventHandler<NotifyCollectionChangedEventArgs<object>>(components_Removed);
                }
                return components;
            }
        }
        private NotificationCollection<object> components;

        void components_Added(object sender, NotifyCollectionChangedEventArgs<object> e)
        {
            if (world != null && e.Value is IComponent)
            {
                ((IComponent)e.Value).Parent = this;
            }
        }

        void components_Removed(object sender, NotifyCollectionChangedEventArgs<object> e)
        {
            if (world != null && e.Value is IComponent)
            {
                ((IComponent)e.Value).Parent = null;
            }
        }
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets or sets the position of this world object.
        /// </summary>
        [XmlIgnore]
        public Vector3 Position
        {
            get { return transform.Translation; }
            set { transform.Translation = value; transformNeedsUpdate = true; }
        }

        /// <summary>
        /// Gets or sets the transform of this world object.
        /// </summary>
        public Matrix Transform
        {
            get { return transform; }
            set { transform = value; transformNeedsUpdate = true; }
        }
        Matrix transform = Matrix.Identity;
        bool transformNeedsUpdate = true;

        /// <summary>
        /// Gets or sets the name of this <see cref="WorldObject"/>.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tag of this <see cref="WorldObject"/>.
        /// </summary>
        public object Tag { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldObject"/> class.
        /// </summary>
        public WorldObject() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldObject"/> class.
        /// </summary>
        public WorldObject(IEnumerable<object> components)
        {
            Components.AddRange(components);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldObject"/> class.
        /// </summary>
        public WorldObject(string name, IEnumerable<object> components)
        {
            Name = name;
            Components.AddRange(components);
        }
        #endregion

        #region Find
        /// <summary>
        /// Find the first feature of type T owned by this game object container.
        /// </summary>
        public T Find<T>() where T : class
        {
            if (this is T)
                return this as T;

            if (world != null)
            {
                var result = world.GetService<T>();
                if (result != null)
                    return result;
            }

            if (components != null)
            {
                for (int i = 0; i < components.Count; i++)
                {
                    var t = components[i] as T;
                    if (t != null)
                        return t;
                    var serviceProvider = components[i] as IServiceProvider;
                    if (serviceProvider != null)
                    {
                        t = serviceProvider.GetService<T>();
                        if (t != null)
                            return t;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Update & Draw
        public void Update(TimeSpan elapsedTime)
        {
            if (components != null)
            {
                // Update components
                for (int i = 0; i < components.Count; i++)
                {
                    var updateable = components[i] as IUpdateable;
                    if (updateable != null)
                        updateable.Update(elapsedTime);
                }

                // Update transform
                if (transformNeedsUpdate)
                {
                    transformNeedsUpdate = false;
                    for (int i = 0; i < components.Count; i++)
                    {
                        var transformable = components[i] as ITransformable;
                        if (transformable != null)
                            transformable.Transform = transform;
                    }
                }
            }
        }

        public void Draw(TimeSpan elapsedTime)
        {
            if (components != null)
            {
                foreach (var component in components)
                {
                    var drawable = component as IDrawable;
                    if (drawable != null)
                        drawable.Draw(elapsedTime);
                }
            }
        }
        #endregion

        #region ToString
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return Name != null ? Name.ToString() : base.ToString();
        }
        #endregion

        #region Clone
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public WorldObject Clone()
        {
            var cloned = new WorldObject();
            cloned.Transform = Transform;
            cloned.Name = Name;
            cloned.Tag = Tag;

            if (components != null)
                for (int i = 0; i < components.Count; i++)
                    cloned.Components.Add(CloneComponent(components[i]));
            return cloned;
        }

        private object CloneComponent(object component)
        {
            if (component == null)
                return component;

            var cloneable = component as ICloneable;
            if (cloneable != null)
                return cloneable.Clone();

            return Serialization.Clone(component);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }            
        #endregion
    }
}