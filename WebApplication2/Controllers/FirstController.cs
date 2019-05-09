﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Pages;
using WebApplication2.Url;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstController : ControllerBase
    {
        private readonly UrlConfig _urlConfig;

        public FirstController(UrlConfig urlConfig)
        {
            _urlConfig = urlConfig;
        }

        [HttpGet("{s1}/{s2?}/{s3?}/{s4?}/{s5?}/{s6?}/{s7?}/{s8?}")]
        public IActionResult Get(string s1, string s2, string s3, string s4,
            string s5, string s6, string s7, string s8)
        {
            if (Invalid(s1,s2,s3,s4,s5,s6,s7,s8))
            {
                return BadRequest(new {message = "something"});
            }

            var sArray = new[] {s1, s2, s3, s4, s5, s6, s7, s8};
            var sEnum = sArray.Where(s => !string.IsNullOrWhiteSpace(s));

            var possibleToken = string.Join("", sEnum);

            return Ok();
        }

        private bool Invalid(params string[] strings)
        {
            bool hasFoundNull = false;
            for (var index = 0; index < strings.Length; index++)
            {
                var s = strings[index];
                var isNullOrWhiteSpace = string.IsNullOrWhiteSpace(s);
                if (!isNullOrWhiteSpace &&
                    s.Length > _urlConfig.MaxUrlChunkSize)
                {
                    return true;
                }
                if (index == 0 && isNullOrWhiteSpace)
                {
                    return true;
                }
                if (!hasFoundNull && 
                    isNullOrWhiteSpace)
                {
                    hasFoundNull = true;
                    continue;
                }

                if (hasFoundNull &&
                    !isNullOrWhiteSpace)
                {
                    return true;
                }

                if (!hasFoundNull)
                {
                    var sanitized = Regex.Replace(s, @"\s+", ""); //remove whitespace
                    const string pattern = @"^[A-Za-z0-9+/]*"; //@"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$";
                    var isBase64 = Regex.IsMatch(sanitized,
                        pattern);
                    if (!isBase64)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}