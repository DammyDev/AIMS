using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAPI.Pagination
{
	public class PagedList<T> : List<T>
	{
		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }

		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;

		public PagedList(List<T> items, int count, int pageNumber, int pageSize, HttpContext httpContext)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);

			AddRange(items);

			var paginationMetadata = new
			{
				totalCount = TotalCount,
				pageSize = PageSize,
				currentPage = CurrentPage,
				totalPages = TotalPages,
				HasPrevious,
				HasNext
			};

			httpContext.Response.Headers.Add("PagingHeaders", JsonConvert.SerializeObject(paginationMetadata));
		}

		public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize, HttpContext httpContext)
		{
			var count = source.Count();
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			
			return new PagedList<T>(items, count, pageNumber, pageSize, httpContext);
		}
	}
}
