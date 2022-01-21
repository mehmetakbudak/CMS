using CMS.Model.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Dto
{
    public class TodoModel : BaseModel
    {
        [Required(ErrorMessage = "Kategori seçiniz.")]
        public int TodoCategoryId { get; set; }

        [Required(ErrorMessage = "Durum seçiniz.")]
        public int TodoStatusId { get; set; }

        public int? AssignUserId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }

    public class TodoGetModel : TodoModel
    {
        public string UserNameSurname { get; set; }
        public string TodoStatusName { get; set; }
        public string TodoCategoryName { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }

    public class TodoFilterModel
    {
        public int? TodoCategoryId { get; set; }
        public string Title { get; set; }
        public int? UserId { get; set; }
        public int? TodoStatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
