// Copyright © 2017-2025 QL-Win Contributors
//
// This file is part of QuickLook program.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using PureSharpCompress.Archives.SevenZip;
using PureSharpCompress.Common;
using PureSharpCompress.Readers;
using QuickLook.Common.Plugin;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Resources;

namespace QuickLook.Plugin.WoW64HookHelper;

public class Plugin : IViewer
{
    public int Priority => int.MinValue;

    public void Init()
    {
        Package.Prepare();
    }

    public bool CanHandle(string path) => false;

    public void Prepare(string path, ContextObject context)
    {
    }

    public void View(string path, ContextObject context)
    {
    }

    public void Cleanup()
    {
    }
}

file static class Package
{
    public static void Prepare()
    {
        try
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QuickLook.WoW64HookHelper.exe");

            if (!File.Exists(fileName))
            {
                using Stream stream = ResourceHelper.GetStream("pack://application:,,,/QuickLook.Plugin.WoW64HookHelper;component/Resources/Package.7z");

                ReaderOptions readerOptions = new()
                {
                    LookForHeader = true,
                    Password = "KwCHSDZpd5uQnjMazdMS",
                };

                ExtractionOptions extractionOptions = new()
                {
                    ExtractFullPath = true,
                    Overwrite = true,
                    PreserveAttributes = false,
                    PreserveFileTime = true,
                };

                using SevenZipArchive archive = SevenZipArchive.Open(stream, readerOptions);
                using IReader reader = archive.ExtractAllEntries();

                while (reader.MoveToNextEntry())
                {
                    reader.WriteEntryToDirectory(AppDomain.CurrentDomain.BaseDirectory, extractionOptions);
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    private static class ResourceHelper
    {
        static ResourceHelper()
        {
            if (!UriParser.IsKnownScheme("pack"))
                _ = PackUriHelper.UriSchemePack;
        }

        public static Stream GetStream(string uriString)
        {
            Uri uri = new(uriString);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            return info?.Stream!;
        }
    }
}
