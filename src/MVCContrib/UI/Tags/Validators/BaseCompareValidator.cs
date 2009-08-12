using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Globalization;

namespace MvcContrib.UI.Tags.Validators
{
	public abstract class BaseCompareValidator : BaseValidator
	{
		private ValidationDataType _type = ValidationDataType.String;

		public BaseCompareValidator(string id, string referenceId, string text, ValidationDataType type)
			: base(id, referenceId, text)
		{
			Type = type;
		}

		public BaseCompareValidator(string id, string referenceId, string text, ValidationDataType type, IDictionary attributes)
			: base(id, referenceId, text, attributes)
		{
			Type = type;
		}

		public BaseCompareValidator(string id, string referenceId, string text, ValidationDataType type, string validationGroup)
			: base(id, referenceId, text, validationGroup)
		{
			Type = type;
		}

		public BaseCompareValidator(string id, string referenceId, string text, ValidationDataType type, string validationGroup, IDictionary attributes)
			: base(id, referenceId, text, validationGroup, attributes)
		{
			Type = type;
		}

		public ValidationDataType Type
		{
			get
			{
				return _type;
			}

			set
			{
				_type = value;
			}
		}

		public override void RenderClientHookup(StringBuilder output)
		{
			if (_type != ValidationDataType.String)
			{
				NullExpandoSet("type", PropertyConverter.EnumToString(typeof(ValidationDataType), _type));
				NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;

				switch (_type)
				{
					case ValidationDataType.Double:
						NullExpandoSet("decimalchar", numberFormat.NumberDecimalSeparator);
						break;
					case ValidationDataType.Currency:
						NullExpandoSet("decimalchar", numberFormat.CurrencyDecimalSeparator);
						string currencyGroupSeparator = numberFormat.CurrencyGroupSeparator;
						if (currencyGroupSeparator[0] == '\x00a0')
						{
							currencyGroupSeparator = " ";
						}

						NullExpandoSet("groupchar", currencyGroupSeparator);
						NullExpandoSet("digits", numberFormat.CurrencyDecimalDigits.ToString(NumberFormatInfo.InvariantInfo));

						int groupSize = GetCurrencyGroupSize(numberFormat);
						if (groupSize > 0)
						{
							NullExpandoSet("groupsize", groupSize.ToString(NumberFormatInfo.InvariantInfo));
						}

						break;
					case ValidationDataType.Date:
						NullExpandoSet("dateorder", GetDateElementOrder());
						NullExpandoSet("cutoffyear", DateTimeFormatInfo.CurrentInfo.Calendar.TwoDigitYearMax.ToString(NumberFormatInfo.InvariantInfo));
						int year = DateTime.Today.Year;
						NullExpandoSet("century", (year - (year % 100)).ToString(NumberFormatInfo.InvariantInfo));
						break;
				}
			}

			base.RenderClientHookup(output);
		}

		private static int GetCurrencyGroupSize(NumberFormatInfo info)
		{
			int[] currencyGroupSizes = info.CurrencyGroupSizes;
			if ((currencyGroupSizes != null) && (currencyGroupSizes.Length == 1))
			{
				return currencyGroupSizes[0];
			}

			return -1;
		}

		private static string GetDateElementOrder()
		{
			string shortDatePattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
			if (shortDatePattern.IndexOf('y') < shortDatePattern.IndexOf('M'))
			{
				return "ymd";
			}

			if (shortDatePattern.IndexOf('M') < shortDatePattern.IndexOf('d'))
			{
				return "mdy";
			}

			return "dmy";
		}

		protected bool CompareValues(string left, string right, ValidationCompareOperator compareOperator)
		{
			object typeValue1 = null, typeValue2 = null;
			bool parsed1 = false, parsed2 = false;

			switch (Type)
			{
				case ValidationDataType.Currency:
					decimal decimalVal1, decimalVal2;
					parsed1 = decimal.TryParse(left, out decimalVal1);
					typeValue1 = decimalVal1;
					if (compareOperator != ValidationCompareOperator.DataTypeCheck)
					{
						parsed2 = decimal.TryParse(right, out decimalVal2);
						typeValue2 = decimalVal2;
					}
					break;
				case ValidationDataType.Date:
					DateTime dateVal1, dateVal2;
					parsed1 = DateTime.TryParse(left, out dateVal1);
					typeValue1 = dateVal1;
					if (compareOperator != ValidationCompareOperator.DataTypeCheck)
					{
						parsed2 = DateTime.TryParse(right, out dateVal2);
						typeValue2 = dateVal2;
					}
					break;
				case ValidationDataType.Double:
					double doubleVal1, doubleVal2;
					parsed1 = double.TryParse(left, out doubleVal1);
					typeValue1 = doubleVal1;
					if (compareOperator != ValidationCompareOperator.DataTypeCheck)
					{
						parsed2 = double.TryParse(right, out doubleVal2);
						typeValue2 = doubleVal2;
					}
					break;
				case ValidationDataType.Integer:
					int intVal1, intVal2;
					parsed1 = int.TryParse(left, out intVal1);
					typeValue1 = intVal1;
					if (compareOperator != ValidationCompareOperator.DataTypeCheck)
					{
						parsed2 = int.TryParse(right, out intVal2);
						typeValue2 = intVal2;
					}
					break;
				case ValidationDataType.String:
					parsed1 = parsed2 = true;
					typeValue1 = left;
					typeValue2 = right;
					break;
			}

			if (parsed1 && compareOperator == ValidationCompareOperator.DataTypeCheck)
				return true;
			else if (!parsed1 || !parsed2)
				return false;

			int compareValue = ((IComparable)typeValue1).CompareTo(typeValue2);
			switch (compareOperator)
			{
				case ValidationCompareOperator.Equal:
					return (compareValue == 0);
				case ValidationCompareOperator.GreaterThan:
					return (compareValue > 0);
				case ValidationCompareOperator.GreaterThanEqual:
					return (compareValue >= 0);
				case ValidationCompareOperator.LessThan:
					return (compareValue < 0);
				case ValidationCompareOperator.LessThanEqual:
					return (compareValue <= 0);
				case ValidationCompareOperator.NotEqual:
					return (compareValue != 0);
				default:
					return false;
			}
		}
	}
}
