﻿#region Copyright 2009 - 2010 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Nine.Content.Pipeline
{    
    [ContentTypeSerializer]
    class RangeSerializer<T> : ContentTypeSerializer<Range<T>>
    {
        protected override Range<T> Deserialize(IntermediateReader input, ContentSerializerAttribute format, Range<T> existingInstance)
        {   
            try
            {
                // Shawn choose to internalize this method, so hack around using reflection.
                ContentTypeSerializer serializer = input.Serializer.GetTypeSerializer(typeof(T));
                T value = (T)Invoke(serializer, "Deserialize", input, format, existingInstance.Min);
                return new Range<T>(value, value);
            }
            catch (Exception)
            {
                Range<T> range = new Range<T>();
                string elementName = format.ElementName;
                {
                    format.ElementName = "Min";
                    range.Min = input.ReadObject<T>(format);
                    format.ElementName = "Max";
                    range.Max = input.ReadObject<T>(format);
                }
                format.ElementName = elementName;
                return range;
            }
        }

        protected override void Serialize(IntermediateWriter output, Range<T> value, ContentSerializerAttribute format)
        {
            if (value.Min.Equals(value.Max))
            {
                ContentTypeSerializer serializer = output.Serializer.GetTypeSerializer(typeof(T));
                Invoke(serializer, "Deserialize", output, value.Min, format);
                return;
            }

            string elementName = format.ElementName;
            {
                format.ElementName = "Min";
                output.WriteObject(value.Min, format);
                format.ElementName = "Max";
                output.WriteObject(value.Max, format);
            }
            format.ElementName = elementName;
        }

        static object Invoke(object target, string method, params object[] args)
        {
            try
            {
                MethodInfo methodInfo = target.GetType().GetMethod(method,
                                                   BindingFlags.Instance | BindingFlags.InvokeMethod |
                                                   BindingFlags.NonPublic | BindingFlags.Public, null,
                                                   args.Select(o => o.GetType()).ToArray(), null);

                return methodInfo.Invoke(target, args);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}
