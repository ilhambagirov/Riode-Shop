using Riode.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace Riode.Application.BlogModule
{
    public class BlogViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public int CategoryId { get; set; }
        public string fileTemp { get; set; }
    }
}
