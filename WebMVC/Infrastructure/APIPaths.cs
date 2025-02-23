namespace WebMVC.Infrastructure
{
    public static class APIPaths
    {
        public static class Catalog
        {
            public static string GetAllTypes(string baseUri)
            {
                return $"{baseUri}/catalogTypes";
            }
            public static string GetAllBrands(string baseUri)
            {
                return $"{baseUri}/catalogbrands";
            }
            public static string GetAllCatalogItems(string baseUri,
                int page, int take, int? brand, int? type)
            {
                var preUri = string.Empty;
                var filterQs = string.Empty;
                if (brand.HasValue)
                {
                    filterQs = $"catalogBrandId={brand.Value}";
                }
                if (type.HasValue)
                {
                    filterQs = (filterQs == string.Empty) ?
                        $"catalogTypeId={type.Value}" :
                        $"{filterQs}&catalogTypeId={type.Value}";
                }
                if (string.IsNullOrEmpty(filterQs))
                {
                    preUri = $"{baseUri}/items?pageIndex={page}&pageSize={take}";
                }
                else
                {
                    preUri = $"{baseUri}/items/filter?pageIndex={page}&pageSize={take}&{filterQs}";
                }
                return preUri;
            }
        }
        public static class Auth
        {
            public static string Register(string baseUri)
            {
                return $"{baseUri}/register";
            }
            public static string Login(string baseUri)
            {
                return $"{baseUri}/login";
            }
            public static string AssignRole(string baseUri)
            {
                return $"{baseUri}/AssignRole";
            }
        }
    }
}
