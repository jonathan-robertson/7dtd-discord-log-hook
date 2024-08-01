namespace DiscordLogHook.Data
{
    internal class Embed
    {
        public string title;
        public string type;
        public string description;
        public string url;
        public string timestamp;
        public int? color;
        public EmbedFooter footer;
        public EmbedImage image;
        public EmbedThumbnail thumbnail;
        public EmbedVideo video;
        public EmbedProvider provider;
        public EmbedAuthor author;
        public EmbedField[] fields;
    }

    internal class EmbedFooter
    {
        public string text;
        public string icon_url;
        public string proxy_icon_url;
    }

    internal class EmbedImage
    {
        public string url;
        public string proxy_url;
        public int height;
        public int width;
    }

    internal class EmbedThumbnail
    {
        public string url;
        public string proxy_url;
        public int height;
        public int width;
    }

    internal class EmbedVideo
    {
        public string url;
        public string proxy_url;
        public int height;
        public int width;
    }

    internal class EmbedProvider
    {
        public string name;
        public string url;
    }

    internal class EmbedAuthor
    {
        public string name;
        public string url;
        public string icon_url;
        public string proxy_icon_url;
    }

    internal class EmbedField
    {
        public string name;
        public string value;
        public bool inline;
    }
}
