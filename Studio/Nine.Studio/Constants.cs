﻿#region Copyright 2009 - 2012 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2012 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Win32;
using System.ComponentModel;

#endregion

namespace Nine.Studio
{
    static class Constants
    {
        public const int MaxRecentFilesCount = 10;
        public const int MaxHeaderBytes = 128;

        public static readonly string Title = "Engine Nine";
        public static readonly string TraceFilename = "Nine.log";
        public static readonly string ExtensionDirectory = ".";
        public static readonly string ProjectExtension = ".nine";

        public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly string VersionString = string.Format("v{0}.{1}", Version.Major, Version.Minor);

        public static readonly string IntermediateDirectory = Path.Combine(Path.GetTempPath(), Strings.Title, VersionString, "Intermediate");
        public static readonly string OutputDirectory = Path.Combine(Path.GetTempPath(), Strings.Title, VersionString, "Bin");
    }
}