using Riode.Domain.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riode.Application.Core.Extensions
{
    static public partial class Extension
    {

        public static string GetCategoriesRaw(this List<Category> categories)
        {
            if (categories == null && !categories.Any())
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class='widget-body filter-items search-ul'>");
            foreach (var item in categories)
            {
                GetChildrenRaw(item);
            }
            sb.Append("</ul>");

            return sb.ToString();

            void GetChildrenRaw(Category category)
            {
                sb.Append("<li>");
                sb.Append($"  <a href = '#' >{category.Name} </a>");
                if (category.Children != null && category.Children.Any())
                {
                    sb.Append("<ul>");
                    foreach (var item in category.Children)
                    {
                        GetChildrenRaw(item);
                    }
                    sb.Append("</ul>");
                }
                sb.Append("</li>");
            }
        }

        //one way to do it
        //public static string GetCategoriesRawAdmin(this IEnumerable<Category> categories, IUrlHelper url)
        //{
        //    if (categories == null && !categories.Any())
        //    {
        //        return "";
        //    }
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in categories)
        //    {
        //        sb.Append($"<tr class='treegrid-{item.Id}'>");

        //        GetChildrenRaw(item);
        //    }
        //    sb.Append("</tr>");

        //    return sb.ToString();

        //    void GetChildrenRaw(Category category)
        //    {
        //        sb.Append("<td>");
        //        sb.Append($"{category.Name}");
        //        sb.Append("</td>");
        //        sb.Append("<td class='d-flex justify-content-end'>");
        //        sb.Append($"<a href={url.Action("details","categories",values:new { id= category.Id })} class='btn btn-success mr-1'><i class='fas fa-info'></i></a>" +
        //                                            $"<a asp-controller = 'Categories' asp-action = 'edit' asp-route-id = '{category.Id}' class='btn btn-warning mr-1'><i class='fas fa-user-edit'></i></a>" +
        //                                           $"<a asp-controller='Categories' asp-action='delete' asp-route-id='{category.Id}' class='btn btn-danger mr-1'><i class='fas fa-trash'></i></a>");
        //        sb.Append("</td>");
        //        if (category.Children != null && category.Children.Any())
        //        {

        //            foreach (var item in category.Children)
        //            {
        //                sb.Append($"<tr class='treegrid-{item.Id} treegrid-parent-{item.ParentId}'>");
        //                GetChildrenRaw(item);
        //            }
        //            sb.Append("</tr>");
        //        }


        //    }
        //}



        //second way to do it
        static public IEnumerable<Category> GetChildren(this Category category)
        {
            if (category.ParentId != null)
            {
                yield return category;
            }
            if (category.Children != null)
            {
                foreach (var item in category.Children.SelectMany(c => c.GetChildren()))
                {
                    yield return item;
                }
            }
        }
    }
}
