﻿using System;
using System.Linq;
using System.Security.Claims;

namespace EntityG.Client.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        public static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Name);

        public static string GetLastName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Surname);

        public static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);

        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
           => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        public static String[] GetRoles(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
    }
}