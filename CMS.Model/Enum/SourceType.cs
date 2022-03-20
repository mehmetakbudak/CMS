﻿using System.ComponentModel;

namespace CMS.Model.Enum
{
    public enum SourceType
    {
        [Description("Yazar Yazıları")]
        Article = 1,
        [Description("Blog")]
        Blog,
        [Description("Foto Galeri")]
        PhotoGallery,
        [Description("Video Galeri")]
        VideoGallery
    }
}
