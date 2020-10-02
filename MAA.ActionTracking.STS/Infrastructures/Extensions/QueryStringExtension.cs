using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MAA.ActionTracking.WebHost.Infrastructures.Extensions
{
    public static class QueryStringExtension
    {

        /// <summary>
        /// keySource corresponds to acr_values
        /// key: tenant, idp, database etct
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="keySource"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// 
        [Obsolete]
        public static string GetValue(this IQueryCollection Query, string keySource, string key)
        {
            if (Query.Keys.Contains(keySource))
            {
                List<string> acrValues = Query[keySource].ToString().Split(' ').ToList();

                string keyValue = acrValues.First(s => s.StartsWith(key));

                return keyValue.Replace($"{key}:", string.Empty).Trim();
            }
            else if (Query.Keys.Contains(key))
                return Query[key].ToString();

            return string.Empty;
        }

        public static CustomAuthorizationContext GetAuthorizationContextFromAcrValues(this IQueryCollection Query)
        {
            if (Query.Keys.Contains("acr_values"))
            {
                List<string> acrValues = Query["acr_values"].ToString().Split(' ').ToList();

                if (acrValues == null)
                    return null;

                var tenantKeyValue = acrValues.FirstOrDefault(s => s.StartsWith("tenant"));


                return new CustomAuthorizationContext
                {
                    Tenant = tenantKeyValue?.Replace($"tenant:", string.Empty).Trim()
                };
            }

            return null;
        }

        public static CustomAuthorizationContext GetAuthorizationContextFromReturnUrl(this IQueryCollection Query)
        {
            if (Query.Keys.Contains("returnUrl"))
            {
                List<string> acrValues = null;

                if (!string.IsNullOrEmpty(Query["acr_values"]))
                    acrValues = Query["acr_values"].ToString().Split(' ').ToList();
                else
                    acrValues = Query.GetQueryStringPrameterValue("returnUrl", "acr_values").Split(' ').ToList();

                if (acrValues == null || acrValues.Count <= 1)
                    return null;

                var tenantKeyValue = acrValues.FirstOrDefault(s => s.StartsWith("tenant"));

                return new CustomAuthorizationContext
                {
                    Tenant = tenantKeyValue.Replace($"tenant:", string.Empty).Trim()
                };
            }

            return null;
        }


        public static Dictionary<string, string> GetQueryParameters(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uri.Query.Length == 0)
                return new Dictionary<string, string>();

            return uri.Query.TrimStart('?')
                            .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                            .GroupBy(parts => parts[0],
                                     parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? WebUtility.HtmlDecode(parts[1]).Replace('+', ' ') : ""))
                            .ToDictionary(grouping => grouping.Key,
                                          grouping => string.Join(",", grouping));
        }

        public static Dictionary<string, string> GetQueryParameters(this IQueryCollection query, string keySource)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            if (query.ToString().Length == 0)
                return new Dictionary<string, string>();

            return WebUtility.HtmlDecode(query[keySource].ToString()).TrimStart('?')
                            .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                            .GroupBy(parts => parts[0],
                                     parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? WebUtility.HtmlDecode(parts[1]).Replace('+', ' ').Replace("%3A", ":").Replace("%20", " ") : ""))
                            .ToDictionary(grouping => grouping.Key,
                                          grouping => string.Join(",", grouping));
        }
        public static string GetQueryStringPrameterValue(this Uri uri, string key)
        {
            var paramsDictionary = uri.GetQueryParameters();

            if (paramsDictionary != null && paramsDictionary.ContainsKey(key))
                return paramsDictionary[key];

            return string.Empty;

        }

        public static string GetQueryStringPrameterValue(this IQueryCollection query, string keySource, string key)
        {
            var paramsDictionary = query.GetQueryParameters(keySource);

            if (paramsDictionary != null && paramsDictionary.ContainsKey(key))
                return paramsDictionary[key];

            return string.Empty;

        }
    }
}
