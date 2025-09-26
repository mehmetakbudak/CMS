using System.Linq;
using CMS.Storage.Dtos.Filter;
using System.Collections.Generic;

namespace CMS.Business.Helper
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(IQueryable<T> data, PagerDto filter)
        {
            PagedResponse<List<T>> response = null;
            if (filter != null)
            {
                var value = new PaginationFilterDto(filter.Skip, filter.PageSize);
                var pagedData = data.Skip(value.Skip).Take(value.PageSize).ToList();
                var total = data.Count();
                response = new PagedResponse<List<T>>(pagedData, value.Skip, value.PageSize);
                response.Total = total;
            }
            else
            {
                var list = data.ToList();
                response = new PagedResponse<List<T>>(list);
                response.Total = list.Count();
            }
            return response;
        }
    }

    public class PagedResponse<T> : Response<T>
    {
        public int Skip { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public PagedResponse(T data)
        {
            List = data;
            Message = null;
            Succeeded = true;
            Errors = null;
        }
        public PagedResponse(T data, int skip, int pageSize)
        {
            Skip = skip;
            PageSize = pageSize;
            List = data;
            Message = null;
            Succeeded = true;
            Errors = null;
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
