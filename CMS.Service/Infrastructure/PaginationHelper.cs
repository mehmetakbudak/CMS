using CMS.Storage.Model;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Service.Infrastructure
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(IQueryable<T> data, FilterModel filter)
        {
            var validFilter = new PaginationFilterModel(filter.Page, filter.PageSize);

            var pagedData = data
                .Skip(filter.Skip)
                .Take(filter.Take).ToList();

            var total = data.Count();
            var response = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            response.Total = total;
            return response;
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
            this.List = data;
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
        public Response(T list)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            List = list;
        }
        public T List { get; set; }
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}
