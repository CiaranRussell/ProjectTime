// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjectTime.Areas.Identity.Pages.Account
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class AccessDeniedModel : PageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        private readonly ILogger<LockoutModel> _logger;

        public AccessDeniedModel(ILogger<LockoutModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            _logger.LogWarning((EventId)100, "Unauthorised user login attempt on {0}: ", DateTime.Now);
        }
    }
}
