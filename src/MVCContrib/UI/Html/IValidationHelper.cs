using System;
using System.Web.Mvc;
using MvcContrib.UI.Tags.Validators;
using System.Collections.Generic;
using System.Collections;
namespace MvcContrib.UI.Html
{
	[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
	public interface IValidationHelper
	{
		ViewContext ViewContext { get; set; }
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string ValidatorRegistrationScripts();
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string ValidatorInitializationScripts();
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		IDictionary<string, object> FormValidation();
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		IDictionary<string, object> FormValidation(string validationGroup);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RequiredValidator(string name, string referenceName, string text);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RequiredValidator(string name, string referenceName, string text, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RequiredValidator(string name, string referenceName, string text, string validationGroup);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RequiredValidator(string name, string referenceName, string text, string validationGroup, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RequiredValidator(string name, string referenceName, string text, string validationGroup, string initialValue);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RequiredValidator(string name, string referenceName, string text, string validationGroup, string initialValue, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, string validationGroup);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RegularExpressionValidator(string name, string referenceName, string validationExpression, string text, string validationGroup, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, string validationGroup);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string CompareValidator(string name, string referenceName, string compareReferenceName, System.Web.UI.WebControls.ValidationDataType type, System.Web.UI.WebControls.ValidationCompareOperator operatorType, string text, string validationGroup, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, string validationGroup);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string RangeValidator(string name, string referenceName, string minimumValue, string maximumValue, System.Web.UI.WebControls.ValidationDataType type, string text, string validationGroup, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string CustomValidator(string name, string referenceName, string clientValidationFunction, string text);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, string validationGroup);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string CustomValidator(string name, string referenceName, string clientValidationFunction, string text, string validationGroup, IDictionary attributes);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string ElementValidation(ICollection<IValidator> validators);
		[Obsolete("ValidationHelper is obsolete. Please consider using ModelState for validation instead.")]
		string ElementValidation(ICollection<IValidator> validators, string referenceName);
	}
}