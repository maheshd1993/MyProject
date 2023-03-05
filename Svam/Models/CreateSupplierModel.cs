using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svam.Models
{
    public class CreateSupplierModel
    {
        public int Id { get; set; }
        public string SupplierId { get; set; }
        [Required(ErrorMessage="*")]
        public string SupplierName { get; set; }
       
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        public string Mobno { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        public string ZipCode { get; set; }
        public string PanNo { get; set; }
        public string ServiceTaxNo { get; set; }
        public string TaxTinNo { get; set; }
        [Required(ErrorMessage = "*")]
        public string SupplierAddress { get; set; }
        public string OpeningBalance { get; set; }
        public string Remaks { get; set; }
        public List<ViewSupplierModel> ViewSuppliermodelList = new List<ViewSupplierModel>();
    }
    public class ViewSupplierModel
    {
        public int Id { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Mobno { get; set; }
        public string StateName { get; set; }
        public string Email { get; set; }
        public string PanNo { get; set; }
        public string ServiceTaxNo { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string TaxTinNo { get; set; }
        public string SupplierAddress { get; set; }
        public string OpeningBalance { get; set; }
        public string Remaks { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ViewSupplierModel> SuppliermodelList = new List<ViewSupplierModel>();
    }
}