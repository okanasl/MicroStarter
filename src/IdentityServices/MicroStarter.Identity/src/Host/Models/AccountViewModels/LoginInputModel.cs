// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace Host.Models
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name="Email / Username")]
        
        public string Email { get; set; }
        [Required(ErrorMessage = "passwordRequired")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}