using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models
{
    public class MasterModel
    {

    }
        
    public class LeadStatusModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="*")]
        public string LeadStatusname { get; set; }
        public string ColorHexValue  { get; set; }
    }

    public class ProjectStatusModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string ProjectStatusname { get; set; }
        public string ColorHexValue { get; set; }
    }

    public class TicketStatusModel 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string TicketStatusName  { get; set; }
    }

    public class ErrorTypeModel 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string ErrorTypeName  { get; set; }
    }
    public class UrgencyTypeModel 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string UrgencyName  { get; set; }
    }
    public class CountryModel
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Countryname { get; set; }
    }
    public class StateModel
    {
        public int CountryId { get; set; }
        public int StateId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Statename { get; set; }
    }
    public class CityModel
    {
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Cityname { get; set; }
    }
    public class Leaddropdownmodel1
    {
        public int dropddownItemId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DropDownItemNamw { get; set; }

    }
    public class Leaddropdownmodel2
    {
        public int dropddownItemId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DropDownItemNamw { get; set; }

    }
    public class Leaddropdownmodel3
    {
        public int dropddownItemId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DropDownItemNamw { get; set; }

    }
    public class Leaddropdownmodel4
    {
        public int dropddownItemId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DropDownItemNamw { get; set; }

    }
    public class Leaddropdownmodel5
    {
        public int dropddownItemId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DropDownItemNamw { get; set; }

    }
    public class LeadSourceModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string LeadsourceName { get; set; }
    }
    public class Leaddropdownmodel
    {
        public int dropddownItemId { get; set; }
        [Required(ErrorMessage = "*")]
        public string DropDownItemNamw { get; set; }

    }
    public class QuotationtypeModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string QuotationtypeName { get; set; }
    }

    public class TypeofleadsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string TypeofleadsName { get; set; }
    }

    public class ProductTypeModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string ProductTypeName { get; set; }
    }
    public class ItemTypeModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string ItemTypeName { get; set; }
    }
    public class InterviewStatusModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "*")]
        public string InterviewStatusName { get; set; }
    }

    public class CreateCategoryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    public class MeasurementunitModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Unit { get; set; }
        [Required(ErrorMessage = "*")]
        public string Description { get; set; }
    }

    public class CreateItemModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public int Catid { get; set; }
        [Required(ErrorMessage = "*")]
        [Remote("ValidateItempartNo", "common", ErrorMessage = "PartNo already exist!")]
        public string PartNo { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [Required(ErrorMessage = "*")]
        public int UnitId { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public decimal PurchaseRate { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public decimal SalesRate { get; set; }
        public double MinStock { get; set; }
        public double MaxStock { get; set; }
        public string Remarks { get; set; }

        public List<CreateItemModel> ViewItemmodelList = new List<CreateItemModel>();
        // To Display the Item.............
        public string Category { get; set; }
        public string UnitName { get; set; }
    }

    public class DepartmentModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Departmentname { get; set; }
    }

    public class ManageBranchModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="*")]
        public string BranchName { get; set; }
        [Required(ErrorMessage = "*")]
        public string BranchCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string Address { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string Pincode { get; set; }
        public string ContactPerson { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "*")]
        public string EmailId { get; set; }
        public string website { get; set; }
        public string TinNo { get; set; }
        public string CstNo { get; set; }
        public string PanNo { get; set; }
        public string ServiceText { get; set; }
        public string ExciseRegNo { get; set; }

        public List<ManageBranchModel> branchModelList = new List<ManageBranchModel>();

    }
}