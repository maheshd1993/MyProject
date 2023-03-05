using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam._Class
{
    public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        private String PropertyName { get; set; }
        private Object InvalidValue { get; set; }
        private readonly RequiredAttribute _innerAttribute;

        public RequiredIfAttribute(String propertyName, Object invalidValue) 
        {
            PropertyName = propertyName;
            InvalidValue = invalidValue;
            _innerAttribute = new RequiredAttribute();
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var dependentValue = context.ObjectInstance.GetType().GetProperty(PropertyName).GetValue(context.ObjectInstance, null);

            if (dependentValue.ToString() != InvalidValue.ToString())
            {
                if (!_innerAttribute.IsValid(value))
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "requiredifnot",
            };
            rule.ValidationParameters["dependentproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(PropertyName);
            rule.ValidationParameters["invalidvalue"] = InvalidValue is bool ? InvalidValue.ToString().ToLower() : InvalidValue;

            yield return rule;
        }
    }
}