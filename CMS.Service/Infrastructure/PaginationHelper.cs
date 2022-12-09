using CMS.Storage.Model;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service.Infrastructure
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(IQueryable<T> data, PaginationFilterModel filter)
        {
            var validFilter = new PaginationFilterModel(filter.PageNumber, filter.PageSize);

            var pagedData = data
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();

            var total = data.Count();
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            respose.Total = total;
            return respose;
        }
    }

    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }

    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }
        public string Message { get; set; }
    }    
}
