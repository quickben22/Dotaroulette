﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Models.Helpers
{
    public sealed class IsNumberAfterAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string testedPropertyName;
        private readonly bool allowEqualNumbers;

        public IsNumberAfterAttribute(string testedPropertyName, bool allowEqualNumbers = false)
        {
            this.testedPropertyName = testedPropertyName;
            this.allowEqualNumbers = allowEqualNumbers;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("unknown property {0}", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null || !(value is int))
            {
                return ValidationResult.Success;
            }

            if (propertyTestedValue == null || !(propertyTestedValue is int))
            {
                return ValidationResult.Success;
            }

            // Compare values
            if ((int)value >= (int)propertyTestedValue)
            {
     
                if (this.allowEqualNumbers && (int)value == (int)propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
                else if ((int)value > (int)propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageString,
                ValidationType = "isnumberafter"
            };
            rule.ValidationParameters["propertytested"] = this.testedPropertyName;
            rule.ValidationParameters["allowequalnumbers"] = this.allowEqualNumbers;
            yield return rule;
        }
    }
}