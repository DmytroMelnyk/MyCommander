using System;

namespace MyCommander
{
    public static class PathHelper
    {
        public static string MakeRelative(string basePath, string absolutePath)
        {
            Uri baseUri = new Uri(basePath);
            Uri absoluteUri = new Uri(absolutePath);
            var result = absoluteUri.MakeRelativeUri(baseUri);
            return result.OriginalString;
        }
    }
}
