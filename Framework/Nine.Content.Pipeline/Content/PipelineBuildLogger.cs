﻿namespace Nine.Content.Pipeline
{
    using System.Diagnostics;
    using Microsoft.Xna.Framework.Content.Pipeline;

    class PipelineBuildLogger : ContentBuildLogger
    {
        public static PipelineBuildLogger Instance { get; private set; }

        static PipelineBuildLogger()
        {
            Instance = new PipelineBuildLogger();
        }

        public override void LogImportantMessage(string message, params object[] messageArgs)
        {
            Trace.TraceInformation(message, messageArgs);
        }

        public override void LogMessage(string message, params object[] messageArgs)
        {
            Trace.WriteLine(string.Format(message, messageArgs));
        }

        public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
        {
            Trace.TraceWarning(message, messageArgs);
            Trace.WriteLine(helpLink);
            Trace.WriteLine(string.Format("FragmentIdentifier: {0}", contentIdentity.FragmentIdentifier));
            Trace.WriteLine(string.Format("SourceFilename: {0}", contentIdentity.SourceFilename));
            Trace.WriteLine(string.Format("SourceTool: {0}", contentIdentity.SourceTool));
        }
    }
}
