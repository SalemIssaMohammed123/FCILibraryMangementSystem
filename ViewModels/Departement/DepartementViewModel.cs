﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Test.ViewModels
{
    public class DepartementViewModel
    {
        public int DepartementID { get; set; }
        [Required(ErrorMessage = "Please,enter the departement name")]
        [MaxLength(20, ErrorMessage = "Please,the departement name must not contain more than or equal 20 characters")]
        [MinLength(3, ErrorMessage = "Please,the departement name must not contain less than 3 characters")]
        //custom validation
        [Microsoft.AspNetCore.Mvc.Remote("CheckDepartementNameUnique", "Departement", ErrorMessage = "DepartementName already exists, please select another name!!.")]
        public string DepartementName { get; set; }
    }
}
