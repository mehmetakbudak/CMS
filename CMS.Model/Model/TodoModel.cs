using CMS.Model.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Dto
{
    public class TodoModel : BaseModel
    {
        [Required(ErrorMessage = "Kategori seçiniz.")]
        public int TodoCategoryId { get; set; }

        public string TodoCategoryName { get; set; }

        [Required(ErrorMessage = "Durum seçiniz.")]
        public int TodoStatusId { get; set; }

        public string TodoStatusName { get; set; }

        public int? UserId { get; set; }

        public string UserNameSurname { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string UserComment { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsActive { get; set; }
    }
}
