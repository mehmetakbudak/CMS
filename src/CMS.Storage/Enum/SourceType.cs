using System.ComponentModel;

namespace CMS.Storage.Enum
{
    public enum SourceType
    {
        [Description("Blog")]
        Blog = 1,
        [Description("Foto Galeri")]
        PhotoGallery,
        [Description("Video Galeri")]
        VideoGallery,
        [Description("Kariyer Fırsatları")]
        Job
    }
}
