using System.Collections.Concurrent;
using System.Windows.Media;

namespace EarTrumpet.DataModel
{
    public class ImageCache
    {
        private static ConcurrentDictionary<string, ImageSource> _images = new ConcurrentDictionary<string, ImageSource>();

        public static bool FindCache(string path, out ImageSource source)
        {
            if (path == null)
            {
                source = null;
                return false;
            }
            return _images.TryGetValue(path, out source);
        }

        public static bool CreateCache(string path, ImageSource source)
        {
            if (path == null)
            {
                return false;
            }
            return _images.TryAdd(path, source);
        }

        public static bool DeleteCache(string path, out ImageSource source)
        {
            if (path == null)
            {
                source = null;
                return false;
            }
            return _images.TryRemove(path, out source);
        }
    }
}
