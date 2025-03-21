﻿using Microsoft.AspNetCore.Mvc.Rendering;
using WebMVC.Models;

namespace WebMVC.ViewModels
{
    public class CatalogIndexViewModel
    {
        public IEnumerable<SelectListItem> Brands { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<CatalogItem> CatalogItems { get; set; }
        public PaginationInfo PaginationInfo { get; set; }

        public int? BrandFilterApplied { get; set; }
        public int? TypesFilterApplied { get; set; }

    }
}
