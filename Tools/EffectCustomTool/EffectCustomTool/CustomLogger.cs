﻿#region File Description
//-----------------------------------------------------------------------------
// CustomLogger.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework.Content.Pipeline;
#endregion

namespace Nine.Tools.EffectCustomTool
{
    /// <summary>
    /// Custom logger class for capturing Content Pipeline output messages. This implementation
    /// just prints messages to the console, and throws an exception if there are any warnings.
    /// </summary>
    class CustomLogger : ContentBuildLogger
    {
        private GenerationEventArgs e;

        public CustomLogger(GenerationEventArgs e)
        {
            this.e = e;
        }
        /// <summary>
        /// Logs a low priority message.
        /// </summary>
        public override void LogMessage(string message, params object[] messageArgs)
        {
            System.Diagnostics.Trace.WriteLine(string.Format(message, messageArgs));
        }


        /// <summary>
        /// Logs a high priority message.
        /// </summary>
        public override void LogImportantMessage(string message, params object[] messageArgs)
        {
            System.Diagnostics.Trace.WriteLine(string.Format(message, messageArgs));
        }


        /// <summary>
        /// Logs a warning message.
        /// </summary>
        public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
        {
            if (e != null)
                e.GenerateWarning(string.Format(message, messageArgs));
        }
    }
}