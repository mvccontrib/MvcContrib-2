using System;
using System.Text;
using NUnit.Framework;
using MvcContrib.UI.Tags.Validators;
using NUnit.Framework.SyntaxHelpers;
using System.Text.RegularExpressions;
using System.Globalization;
using Rhino.Mocks;
using System.Web;
using System.Collections.Specialized;

namespace MvcContrib.UnitTests.UI.Html
{
	public class ValidatorTester
	{
		[TestFixture]
		public class Base_Validator
		{
			[Test]
			public void All_Validators_Are_Valid()
			{
				var validator1 = new RequiredValidator("myid1", "refid1", "Error!");
				var validator2 = new RequiredValidator("myid2", "refid2", "Error!");
				var mocks = new MockRepository();
				HttpRequestBase request;
				var formValues = new NameValueCollection {{"refid1", "1234"}, {"refid2", "5678"}};

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(BaseValidator.Validate(request, new IValidator[] { validator1, validator2 }));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.IsValid);
				}
			}

			[Test]
			public void Single_Validator_Not_Valid()
			{
				var validator1 = new RequiredValidator("myid1", "refid1", "Error!");
				var validator2 = new RequiredValidator("myid2", "refid2", "Error!");
				var mocks = new MockRepository();
				HttpRequestBase request;
				var formValues = new NameValueCollection {{"refid2", ""}, {"refid1", "1234"}};

			    using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(BaseValidator.Validate(request, new IValidator[] { validator1, validator2 }));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsFalse(validator2.IsValid);
				}
			}

			[Test]
			public void Multiple_Validators_Not_Valid()
			{
				var validator1 = new RequiredValidator("myid1", "refid1", "Error!");
				var validator2 = new RequiredValidator("myid2", "refid2", "Error!");
				var mocks = new MockRepository();
				HttpRequestBase request;
				var formValues = new NameValueCollection {{"refid1", ""}, {"refid2", ""}};

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(BaseValidator.Validate(request, new IValidator[] { validator1, validator2 }));
					Assert.IsFalse(validator1.IsValid);
					Assert.IsFalse(validator2.IsValid);
				}
			}
		}

		[TestFixture]
		public class Required_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				var validator = new RequiredValidator("myId", "myReference", "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("RequiredFieldValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				var validator = new RequiredValidator("myId", "myReference", "Error!")
				                    {
				                        InitialValue = "test",
				                        ErrorMessage = "error",
				                        ValidationGroup = "group",
				                        ReferenceId = "reference"
				                    };
			    Assert.That(validator.InitialValue, Is.EqualTo("test"));
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void Empty_property_returns_null_for_expando_attribute()
			{
				var validator = new RequiredValidator("myId", "myReference", "Error!");
				Assert.IsNull(validator.ValidationGroup);
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				var validator = new RequiredValidator("myId", "myReference", "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new RequiredValidator("myId", "myReference", "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new RequiredValidator("myId", "myReference", "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				var validator = new RequiredValidator("myId", "myReference", "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				var validator = new RequiredValidator("myId", "myReference", "Error!");
				var outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.evaluationfunction\s=\s""RequiredFieldValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}

			[Test]
			public void Validation_On_Value_Present()
			{
				var validator = new RequiredValidator("myid", "refid", "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"refid", "value"}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Value_Present()
			{
				var validator = new RequiredValidator("myid", "refid", "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"refid", ""}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Form_Value_Present()
			{
				var validator = new RequiredValidator("myid", "refid", "error!");
				var mocks = new MockRepository();
				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicHttpRequestBase();
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
				}
			}

			[Test]
			public void Rendering_Validator_When_IsValid_False_Initially_Displays()
			{
				var validator = new RequiredValidator("myid", "refid", "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"refid", ""}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myid\" style=\"color:red;\">error!</span>"));
				}
			}
		}

		[TestFixture]
		public class Regular_Expression_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				var validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("RegularExpressionValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				var validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!");
				Assert.That(validator.ValidationExpression, Is.EqualTo(".*"));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				var validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationExpression, Is.EqualTo(".*"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationExpression, Is.EqualTo(".*"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				var validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				var validator = new RegularExpressionValidator("myId", "myReference", ".*", "Error!");
				var outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.validationexpression\s=\s""\.\*"";.*myId.evaluationfunction\s=\s""RegularExpressionValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}

			[Test]
			public void Validation_On_Value_Present()
			{
				var validator = new RegularExpressionValidator("myid", "refid", "^\\d*$", "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"refid", "1234"}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Value_Present()
			{
				var validator = new RegularExpressionValidator("myid", "refid", "^\\d*$", "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"refid", "value"}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Form_Value_Present()
			{
				var validator = new RegularExpressionValidator("myid", "refid", "^\\d*$", "error!");

				var mocks = new MockRepository();
				HttpRequestBase request;

				using (mocks.Record())
				{
					request = mocks.DynamicHttpRequestBase();
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
				}
			}
		}

		[TestFixture]
		public class Compare_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("CompareValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Assert.That(validator.CompareReferenceId, Is.EqualTo("compareReference"));
				Assert.That(validator.Type, Is.EqualTo(System.Web.UI.WebControls.ValidationDataType.String));
				Assert.That(validator.OperatorType, Is.EqualTo(System.Web.UI.WebControls.ValidationCompareOperator.Equal));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				var outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.operator\s=\s""Equal"";.*myId.controltocompare\s=\s""compareReference"";.*myId.controlhookup\s=\s""compareReference"".*myId.evaluationfunction\s=\s""CompareValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}

			[Test]
			public void Validate_Rendering_double_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				var outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.operator\s=\s""Equal"";.*myId.controltocompare\s=\s""compareReference"";.*myId.controlhookup\s=\s""compareReference"".*myId.type\s=\s""Double"";.*myId.decimalchar\s=\s""\."";.*myId.evaluationfunction\s=\s""CompareValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_currency_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				var outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.operator\s=\s""Equal"";.*myId.controltocompare\s=\s""compareReference"";.*myId.controlhookup\s=\s""compareReference"".*myId.type\s=\s""Currency"";.*myId.decimalchar\s=\s""\."";.*myId.groupchar\s=\s"","";.*myId.digits\s=\s""2"";.*myId.groupsize\s=\s""3"";.*myId.evaluationfunction\s=\s""CompareValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_currency_empty_group_separator_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
				CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator = "\x00a0";

				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				var outputRegex = new Regex(@".*myId.groupchar\s=\s"" "";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_currency_variable_group_size_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
				CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSizes = new[] { 2, 3 };

				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				var outputRegex = new Regex(@".*myId.groupsize.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsFalse(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_date_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				var outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.operator\s=\s""Equal"";.*myId.controltocompare\s=\s""compareReference"";.*myId.controlhookup\s=\s""compareReference"".*myId.type\s=\s""Date"";.*myId.dateorder\s=\s""mdy"";.*myId.cutoffyear\s=\s""2029"";.*myId.century\s=\s""2000"";.*myId.evaluationfunction\s=\s""CompareValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_date_YMD_format_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
				CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy/M/d";

				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				var outputRegex = new Regex(@".*myId.dateorder\s=\s""ymd"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validate_Rendering_date_DMY_format_client_hookup()
			{
				CultureInfo priorInfo = CultureInfo.CurrentCulture;
				System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
				CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern = "d/M/yyyy";

				var validator = new CompareValidator("myId", "myReference", "compareReference", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "Error!");
				var outputRegex = new Regex(@".*myId.dateorder\s=\s""dmy"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));

				System.Threading.Thread.CurrentThread.CurrentCulture = priorInfo;
			}

			[Test]
			public void Validation_On_Date_Value_Present()
			{
				var validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				var validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				var validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				var validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.Date, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				var mocks = new MockRepository();
				HttpRequestBase request = null;
				var formValues = new NameValueCollection
				                 	{
				                 		{"refid1", "1/5/2008"},
				                 		{"compareRefId1", "1/5/2008"},
				                 		{"refid2", "1/6/2008"},
				                 		{"compareRefId2", "1/5/2008"},
				                 		{"refid3", "1/6/2008"},
				                 		{"compareRefId3", "1/5/2008"},
				                 		{"refid4", "1/5/2008"},
				                 		{"compareRefId4", "1/5/2008"},
				                 		{"refid5", "1/4/2008"},
				                 		{"compareRefId5", "1/5/2008"},
				                 		{"refid6", "1/4/2008"},
				                 		{"compareRefId6", "1/5/2008"},
				                 		{"refid7", "1/5/2008"},
				                 		{"compareRefId7", "1/5/2008"},
				                 		{"refid8", "1/4/2008"},
				                 		{"compareRefId8", "1/5/2008"},
				                 		{"refid9", "1/4/2008"}
				                 	};

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_On_Currency_Value_Present()
			{
				var validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				var validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				var validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				var validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.Currency, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				var mocks = new MockRepository();
				HttpRequestBase request = null;
				var formValues = new NameValueCollection
				                 	{
				                 		{"refid1", "1234.12"},
				                 		{"compareRefId1", "1234.12"},
				                 		{"refid2", "2345.12"},
				                 		{"compareRefId2", "1234.12"},
				                 		{"refid3", "2345.12"},
				                 		{"compareRefId3", "1234.12"},
				                 		{"refid4", "1234.12"},
				                 		{"compareRefId4", "1234.12"},
				                 		{"refid5", "123.12"},
				                 		{"compareRefId5", "1234.12"},
				                 		{"refid6", "123.12"},
				                 		{"compareRefId6", "1234.12"},
				                 		{"refid7", "1234.12"},
				                 		{"compareRefId7", "1234.12"},
				                 		{"refid8", "1234.12"},
				                 		{"compareRefId8", "2345.12"},
				                 		{"refid9", "1234.12"}
				                 	};

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_On_Double_Value_Present()
			{
				var validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				var validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				var validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				var validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection
				                 	{
				                 		{"refid1", "1234.12"},
				                 		{"compareRefId1", "1234.12"},
				                 		{"refid2", "2345.12"},
				                 		{"compareRefId2", "1234.12"},
				                 		{"refid3", "2345.12"},
				                 		{"compareRefId3", "1234.12"},
				                 		{"refid4", "1234.12"},
				                 		{"compareRefId4", "1234.12"},
				                 		{"refid5", "123.12"},
				                 		{"compareRefId5", "1234.12"},
				                 		{"refid6", "123.12"},
				                 		{"compareRefId6", "1234.12"},
				                 		{"refid7", "1234.12"},
				                 		{"compareRefId7", "1234.12"},
				                 		{"refid8", "1234.12"},
				                 		{"compareRefId8", "2345.12"},
				                 		{"refid9", "1234.12"}
				                 	};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_On_Integer_Value_Present()
			{
				var validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				var validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				var validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				var validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.Integer, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection
				                 	{
				                 		{"refid1", "1234"},
				                 		{"compareRefId1", "1234"},
				                 		{"refid2", "2345"},
				                 		{"compareRefId2", "1234"},
				                 		{"refid3", "2345"},
				                 		{"compareRefId3", "1234"},
				                 		{"refid4", "1234"},
				                 		{"compareRefId4", "1234"},
				                 		{"refid5", "123"},
				                 		{"compareRefId5", "1234"},
				                 		{"refid6", "123"},
				                 		{"compareRefId6", "1234"},
				                 		{"refid7", "1234"},
				                 		{"compareRefId7", "1234"},
				                 		{"refid8", "1234"},
				                 		{"compareRefId8", "2345"},
				                 		{"refid9", "1234"}
				                 	};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_On_String_Value_Present()
			{
				var validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThan, "error!");
				var validator3 = new CompareValidator("myid", "refid3", "compareRefId3", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator4 = new CompareValidator("myid", "refid4", "compareRefId4", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.GreaterThanEqual, "error!");
				var validator5 = new CompareValidator("myid", "refid5", "compareRefId5", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.LessThan, "error!");
				var validator6 = new CompareValidator("myid", "refid6", "compareRefId6", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator7 = new CompareValidator("myid", "refid7", "compareRefId7", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.LessThanEqual, "error!");
				var validator8 = new CompareValidator("myid", "refid8", "compareRefId8", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.NotEqual, "error!");
				var validator9 = new CompareValidator("myid", "refid9", "compareRefId9", System.Web.UI.WebControls.ValidationDataType.String, System.Web.UI.WebControls.ValidationCompareOperator.DataTypeCheck, "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection
				                 	{
				                 		{"refid1", "c"},
				                 		{"compareRefId1", "c"},
				                 		{"refid2", "d"},
				                 		{"compareRefId2", "c"},
				                 		{"refid3", "d"},
				                 		{"compareRefId3", "c"},
				                 		{"refid4", "c"},
				                 		{"compareRefId4", "c"},
				                 		{"refid5", "b"},
				                 		{"compareRefId5", "c"},
				                 		{"refid6", "b"},
				                 		{"compareRefId6", "c"},
				                 		{"refid7", "c"},
				                 		{"compareRefId7", "c"},
				                 		{"refid8", "b"},
				                 		{"compareRefId8", "c"},
				                 		{"refid9", "c"}
				                 	};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator1.Validate(request));
					Assert.IsTrue(validator1.IsValid);
					Assert.IsTrue(validator2.Validate(request));
					Assert.IsTrue(validator2.IsValid);
					Assert.IsTrue(validator3.Validate(request));
					Assert.IsTrue(validator3.IsValid);
					Assert.IsTrue(validator4.Validate(request));
					Assert.IsTrue(validator4.IsValid);
					Assert.IsTrue(validator5.Validate(request));
					Assert.IsTrue(validator5.IsValid);
					Assert.IsTrue(validator6.Validate(request));
					Assert.IsTrue(validator6.IsValid);
					Assert.IsTrue(validator7.Validate(request));
					Assert.IsTrue(validator7.IsValid);
				}
			}

			[Test]
			public void Validation_InvalidType()
			{
				var validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection
				                 	{
				                 		{"refid1", "abcd"},
				                 		{"compareRefId1", "1234.12"},
				                 		{"refid1", "1234.12"},
				                 		{"compareRefId1", "abcd"}
				                 	};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator1.Validate(request));
					Assert.IsFalse(validator2.Validate(request));
				}
			}

			[Test]
			public void Validation_On_No_Value_Present()
			{
				var validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"compareRefId1", "1234.12"}, {"refid2", "1234.12"}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator1.Validate(request));
					Assert.IsFalse(validator1.IsValid);
					Assert.IsFalse(validator2.Validate(request));
					Assert.IsFalse(validator2.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Form_Value_Present()
			{
				var validator1 = new CompareValidator("myid", "refid1", "compareRefId1", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				var validator2 = new CompareValidator("myid", "refid2", "compareRefId2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");

				var mocks = new MockRepository();
				HttpRequestBase request;

				using (mocks.Record())
				{
					request = mocks.DynamicHttpRequestBase();
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator1.Validate(request));
					Assert.IsFalse(validator1.IsValid);
					Assert.IsFalse(validator2.Validate(request));
					Assert.IsFalse(validator2.IsValid);
				}
			}
		}

		[TestFixture]
		public class Range_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				var validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("RangeValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				var validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
				Assert.That(validator.MinimumValue, Is.EqualTo("1"));
				Assert.That(validator.MaximumValue, Is.EqualTo("10"));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				var validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.MinimumValue, Is.EqualTo("1"));
				Assert.That(validator.MaximumValue, Is.EqualTo("10"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.MinimumValue, Is.EqualTo("1"));
				Assert.That(validator.MaximumValue, Is.EqualTo("10"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				var validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				var validator = new RangeValidator("myId", "myReference", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
				var outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.minimumvalue\s=\s""1"";.*myId.maximumvalue\s=\s""10"";.*myId.type\s=\s""Double"";.*myId.evaluationfunction\s=\s""RangeValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_validation_type_is_date()
			{
				new RangeValidator("myId", "myReference", "1/1/2008", "3/1/2008", System.Web.UI.WebControls.ValidationDataType.Date, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_validation_type_is_currency()
			{
				new RangeValidator("myId", "myReference", "$1.00", "$3.00", System.Web.UI.WebControls.ValidationDataType.Currency, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_minimum_value_is_not_valid_type()
			{
				new RangeValidator("myId", "myReference", "abcd", "1234", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_maximum_value_is_not_valid_type()
			{
				new RangeValidator("myId", "myReference", "1234", "abcd", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_minimum_value_is_not_specified()
			{
				new RangeValidator("myId", "myReference", "", "1234", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
			}

			[Test, ExpectedException(typeof(ArgumentException))]
			public void When_maximum_value_is_not_specified()
			{
				new RangeValidator("myId", "myReference", "1234", "", System.Web.UI.WebControls.ValidationDataType.Double, "Error!");
			}

			[Test]
			public void Validation_On_String_Type()
			{
				var validator = new RangeValidator("myid", "refid", "a", "c", System.Web.UI.WebControls.ValidationDataType.String, "error!");
				var mocks = new MockRepository();
				HttpRequestBase request;
				var formValues = new NameValueCollection {{"refid", "b"}};

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_Double_Type()
			{
				var validator = new RangeValidator("myid", "refid", "1234.12", "2345.12", System.Web.UI.WebControls.ValidationDataType.Double, "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"refid", "2000.00"}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_Integer_Type()
			{
				var validator = new RangeValidator("myid", "refid", "1", "10", System.Web.UI.WebControls.ValidationDataType.Integer, "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"refid", "5"}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsTrue(validator.Validate(request));
					Assert.IsTrue(validator.IsValid);
				}
			}

			[Test]
			public void Validation_InvalidType()
			{
				var validator = new RangeValidator("myid", "refid1", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "error!");
				var mocks = new MockRepository();
				HttpRequestBase request;
				var formValues = new NameValueCollection {{"refid1", "abcd"}};

				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Value_Present()
			{
				var validator = new RangeValidator("myid", "refid1", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "error!");
				var mocks = new MockRepository();
				var formValues = new NameValueCollection {{"refid1", ""}};

				HttpRequestBase request;
				using (mocks.Record())
				{
					request = mocks.DynamicMock<HttpRequestBase>();
					SetupResult.For(request.Form).Return(formValues);
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}

			[Test]
			public void Validation_On_No_Form_Value_Present()
			{
				var validator = new RangeValidator("myid", "refid1", "1", "10", System.Web.UI.WebControls.ValidationDataType.Double, "error!");

				var mocks = new MockRepository();
				HttpRequestBase request;

				using (mocks.Record())
				{
					request = mocks.DynamicHttpRequestBase();
				}

				using (mocks.Playback())
				{
					Assert.IsFalse(validator.Validate(request));
					Assert.IsFalse(validator.IsValid);
				}
			}
		}

		[TestFixture]
		public class Custom_Validator_With_All_Properties
		{
			[Test]
			public void Tag_Is_Correct()
			{
				var validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!");
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.ValidationFunction, Is.EqualTo("CustomValidatorEvaluateIsValid"));
			}

			[Test]
			public void Properties_Stick_When_Set()
			{
				var validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!");
				Assert.That(validator.ClientValidationFunction, Is.EqualTo("MyFunction"));
				validator.ErrorMessage = "error";
				Assert.That(validator.ErrorMessage, Is.EqualTo("error"));
				validator.ValidationGroup = "group";
				Assert.That(validator.ValidationGroup, Is.EqualTo("group"));
				validator.ReferenceId = "reference";
				Assert.That(validator.ReferenceId, Is.EqualTo("reference"));
			}

			[Test]
			public void When_Creating_element_with_validation_group_it_sticks()
			{
				var validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!", "test");
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ClientValidationFunction, Is.EqualTo("MyFunction"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
			}

			[Test]
			public void When_creating_element_with_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ClientValidationFunction, Is.EqualTo("MyFunction"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void When_creating_element_with_validation_group_and_dictionary_it_sticks()
			{
				var hash = new Hash {{"Key1", "Val1"}, {"Key2", "Val2"}, {"Key3", "Val3"}};
				var validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!", "test", hash);
				Assert.That(validator.Id, Is.EqualTo("myId"));
				Assert.That(validator.ReferenceId, Is.EqualTo("myReference"));
				Assert.That(validator.InnerText, Is.EqualTo("Error!"));
				Assert.That(validator.ClientValidationFunction, Is.EqualTo("MyFunction"));
				Assert.That(validator.ValidationGroup, Is.EqualTo("test"));
				Assert.That(validator.Tag, Is.EqualTo("span"));
				Assert.That(validator.Attributes.Count == 5);
				Assert.That(validator["Key1"], Is.EqualTo("Val1"));
			}

			[Test]
			public void Validate_Rendering()
			{
				var validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!");
				Assert.That(validator.ToString(), Is.EqualTo("<span id=\"myId\" style=\"display:none;color:red;\">Error!</span>"));
			}

			[Test]
			public void Validate_Rendering_client_hookup()
			{
				var validator = new CustomValidator("myId", "myReference", "MyFunction", "Error!");
				var outputRegex = new Regex(@"var\smyId.*;.*myId.controltovalidate\s=\s""myReference"";.*myId.clientvalidationfunction\s=\s""MyFunction"";.*myId.evaluationfunction\s=\s""CustomValidatorEvaluateIsValid"";.*", RegexOptions.Singleline);
				var output = new StringBuilder();
				validator.RenderClientHookup(output);
				Assert.IsTrue(outputRegex.IsMatch(output.ToString()));
			}
		}
	}
}
