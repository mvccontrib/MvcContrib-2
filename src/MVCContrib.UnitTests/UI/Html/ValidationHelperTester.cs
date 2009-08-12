using System.Collections.Generic;
using System.Web.Mvc;
using MvcContrib.Interfaces;
using MvcContrib.Services;
using MvcContrib.UI.Html;
using MvcContrib.UI.Tags.Validators;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.UI.Html 
{
#pragma warning disable 618,612
	[TestFixture]
	public class ValidationHelperTester
	{
		public class BaseValidationHelperTester : BaseViewTester
		{
			protected ValidationHelper _helper;

			[SetUp]
			protected override void Setup()
			{
				base.Setup();
				_helper = new ValidationHelper {ViewContext = _viewContext};
			}

		}

		[TestFixture]
		public class When_GetInstance_Is_Invoked : BaseViewTester
		{
			[SetUp]
			public void SetUp()
			{
				//Ensure there isn't a dependencyresolver hanging around from a previous test...
				DependencyResolver.InitializeWith(null);
				base.Setup();
			}

			[TearDown]
			public void TearDown()
			{
				DependencyResolver.InitializeWith(null);
			}

			[Test]
			public void The_ValidationHelper_in_HttpContext_Items_should_be_returned()
			{
				var helper = new ValidationHelper();
				_viewContext.HttpContext.Items.Add(ValidationHelper.CACHE_KEY, helper);
				Assert.That(ValidationHelper.GetInstance(_viewContext), Is.EqualTo(helper));
			}

			[Test]
			public void A_new_ValidationHelper_should_be_created_and_cached_in_HttpContext_items_and_ViewContext_should_be_set()
			{
				IValidationHelper helper = ValidationHelper.GetInstance(_viewContext);
				Assert.That(helper, Is.Not.Null);
				Assert.That(_viewContext.HttpContext.Items[ValidationHelper.CACHE_KEY], Is.EqualTo(helper));
				Assert.That(helper.ViewContext, Is.EqualTo(_viewContext));
			}

			[Test]
			public void Then_the_formhelper_should_be_created_using_the_dependencyresolver()
			{
				var helper = new ValidationHelper();
				using (mocks.Record())
				{
					var resolver = mocks.DynamicMock<IDependencyResolver>();
					DependencyResolver.InitializeWith(resolver);
					Expect.Call(resolver.GetImplementationOf<IValidationHelper>()).Return(helper);
				}
				using (mocks.Playback())
				{
					IValidationHelper instance = ValidationHelper.GetInstance(_viewContext);
					Assert.That(instance, Is.EqualTo(helper));
				}
			}
		}

		[TestFixture]
		public class When_ValidationHelperExtensions_Is_Used : BaseViewTester
		{
			[SetUp]
			protected override void Setup()
			{
				base.Setup();
				//Ensure there isn't a dependencyresolver hanging around from a previous test...
				DependencyResolver.InitializeWith(null);
			}

			[Test]
			public void Then_a_ValidationHelper_should_be_created()
			{
				var helper = new HtmlHelper(_viewContext, new ViewPage());
				IValidationHelper formHelper = HtmlHelperExtensions.Validation(helper);
				Assert.IsNotNull(formHelper);
			}
		}

		[TestFixture]
		public class When_Validator_Registration_Scripts_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_url_is_rendered()
			{
				string html = _helper.ValidatorRegistrationScripts();
				var javascriptRegex = new System.Text.RegularExpressions.Regex(@"<script.*WebResource\.axd.*function\sWebForm_OnSubmit\(\).*function\sWebForm_OnSubmitGroup\(group\).*</script>", System.Text.RegularExpressions.RegexOptions.Singleline);
				Assert.IsTrue(javascriptRegex.IsMatch(html));
			}

			[Test]
			public void The_correct_url_is_rendered_when_the_application_is_at_the_site_root()
			{
				string html = _helper.ValidatorRegistrationScripts();
				Assert.That(html.StartsWith("<script src=\"/WebResource.axd"));
			}

			//TODO: Investigate changing validation helper so the cached URL can be reset between tests. 
			[Test, Ignore("Passes when run by itself, but fails when all the tests in the fixture are run because validationHelper stores the validation url in a static variable.")]
			public void The_correct_url_is_rendered_when_the_application_is_in_a_virtual_directory()
			{
				mocks.BackToRecord(base._viewContext.HttpContext.Request, BackToRecordOptions.None);
				SetupResult.For(_viewContext.HttpContext.Request.ApplicationPath).Return("/Foo");
				mocks.Replay(_viewContext.HttpContext.Request);


				string html = _helper.ValidatorRegistrationScripts();
				Assert.That(html.StartsWith("<script src=\"/Foo/WebResource.axd"));
			}

			[Test]
			public void When_Validator_Registration_Scripts_is_invoked_twice_same_output_is_rendered()
			{
				string html1 = _helper.ValidatorRegistrationScripts();
				Setup();
				string html2 = _helper.ValidatorRegistrationScripts();
				Assert.AreEqual(html1, html2);
			}

			[Test, ExpectedException(typeof(System.InvalidOperationException))]
			public void Multiple_calls_on_same_request_fails()
			{
				string html1 = _helper.ValidatorRegistrationScripts();
				string html2 = _helper.ValidatorRegistrationScripts();
			}

		}

		[TestFixture]
		public class When_Validator_Initialization_Scripts_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_output_is_rendered()
			{
				_helper.ValidatorRegistrationScripts();
				string html = _helper.ValidatorInitializationScripts();
				var javascriptRegex = new System.Text.RegularExpressions.Regex(@"<script.*var\sPage_Validators\s=\snew\sArray\(\);.*<script.*var\sPage_ValidationActive.*ValidatorOnLoad\(\).*function\sValidatorOnSubmit\(group\)\s{.*</script>", System.Text.RegularExpressions.RegexOptions.Singleline);
				Assert.IsTrue(javascriptRegex.IsMatch(html));
			}

			[Test, ExpectedException(typeof(System.InvalidOperationException))]
			public void When_Validator_Registration_Scripts_is_not_invoked()
			{
				_helper.ValidatorInitializationScripts();
			}

			[Test, ExpectedException(typeof(System.InvalidOperationException))]
			public void Multiple_calls_on_same_request_fails()
			{
				_helper.ValidatorRegistrationScripts();
				_helper.ValidatorInitializationScripts();
				_helper.ValidatorInitializationScripts();
			}
		}

		[TestFixture]
		public class When_Form_Validation_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_output_is_rendered()
			{
				_helper.ValidatorRegistrationScripts();
				IDictionary<string, object> output = _helper.FormValidation();
				Assert.That(output.Count, Is.EqualTo(1));
				Assert.IsTrue(output.Keys.Contains("onsubmit"));
				Assert.That(output["onsubmit"], Is.EqualTo("javascript:return WebForm_OnSubmit();"));
			}

			[Test]
			public void The_correct_output_is_rendered_with_validation_group()
			{
				_helper.ValidatorRegistrationScripts();
				IDictionary<string, object> output = _helper.FormValidation("test");
				Assert.That(output.Count, Is.EqualTo(1));
				Assert.IsTrue(output.Keys.Contains("onsubmit"));
				Assert.That(output["onsubmit"], Is.EqualTo("javascript:return WebForm_OnSubmitGroup('test');"));
			}

			[Test, ExpectedException(typeof(System.InvalidOperationException))]
			public void When_Validator_Registration_Scripts_is_not_invoked()
			{
				_helper.FormValidation();
			}

			[Test, ExpectedException(typeof(System.InvalidOperationException))]
			public void When_Validator_Registration_Scripts_is_not_invoked_with_validation_group()
			{
				_helper.FormValidation("test");
			}

			[Test]
			public void When_Form_Validation_does_not_have_a_validation_group()
			{
				_helper.ValidatorRegistrationScripts();
				IDictionary<string, object> output = _helper.FormValidation(null);
				Assert.That(output.Count, Is.EqualTo(1));
				Assert.IsTrue(output.Keys.Contains("onsubmit"));
				Assert.That(output["onsubmit"], Is.EqualTo("javascript:return WebForm_OnSubmit();"));
			}
		}

		[TestFixture]
		public class When_Required_Validator_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_output_is_rendered()
			{
				var hash = new Hash<string>();
				hash["Key1"] = "Val1";

				_helper.ValidatorRegistrationScripts();
				string output1 = _helper.RequiredValidator("myid1", "refid", "error!");
				string output2 = _helper.RequiredValidator("myid2", "refid", "error!", hash);
				string output3 = _helper.RequiredValidator("myid3", "refid", "error!", "group");
				string output4 = _helper.RequiredValidator("myid4", "refid", "error!", "group", hash);
				string output5 = _helper.RequiredValidator("myid5", "refid", "error!", "group", "val");
				string output6 = _helper.RequiredValidator("myid6", "refid", "error!", "group", "val", hash);

				Assert.That(output1, Is.EqualTo("<span id=\"myid1\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output2, Is.EqualTo("<span Key1=\"Val1\" id=\"myid2\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output3, Is.EqualTo("<span id=\"myid3\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output4, Is.EqualTo("<span Key1=\"Val1\" id=\"myid4\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output5, Is.EqualTo("<span id=\"myid5\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output6, Is.EqualTo("<span Key1=\"Val1\" id=\"myid6\" style=\"display:none;color:red;\">error!</span>"));
			}

			[Test, ExpectedException(typeof(System.InvalidOperationException))]
			public void When_Validator_Registration_Scripts_is_not_invoked()
			{
				_helper.RequiredValidator("myid", "refid", "error!");
			}

			[Test, ExpectedException(typeof(System.ArgumentException))]
			public void When_validator_is_duplicated_for_reference_control()
			{
				_helper.ValidatorRegistrationScripts();
				_helper.RequiredValidator("myid", "refid", "error!");
				_helper.RequiredValidator("myid", "refid", "error!");
			}

			[Test]
			public void When_single_validator_is_initialized_verify_initialization()
			{
				var javascriptRegex = new System.Text.RegularExpressions.Regex(@"<script.*var\sPage_Validators\s=\snew\sArray\(.*myid.*\);.*<script.*var\smyid.*.*var\sPage_ValidationActive.*ValidatorOnLoad\(\).*function\sValidatorOnSubmit\(group\)\s{.*</script>", System.Text.RegularExpressions.RegexOptions.Singleline);
				_helper.ValidatorRegistrationScripts();
				_helper.RequiredValidator("myid", "refid", "error!");
				string output = _helper.ValidatorInitializationScripts();

				Assert.IsTrue(javascriptRegex.IsMatch(output));
			}

			[Test]
			public void When_multiple_validator_is_initialized_verify_initialization()
			{
				var javascriptRegex = new System.Text.RegularExpressions.Regex(@"<script.*var\sPage_Validators\s=\snew\sArray\(.*myid.*myid2.*\);.*<script.*var\smyid.*.*var\sPage_ValidationActive.*ValidatorOnLoad\(\).*function\sValidatorOnSubmit\(group\)\s{.*</script>", System.Text.RegularExpressions.RegexOptions.Singleline);
				_helper.ValidatorRegistrationScripts();
				_helper.RequiredValidator("myid", "refid", "error!");
				_helper.RequiredValidator("myid2", "refid", "error!");
				string output = _helper.ValidatorInitializationScripts();

				Assert.IsTrue(javascriptRegex.IsMatch(output));
			}
		}

		[TestFixture]
		public class When_Regular_Expression_Validator_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_output_is_rendered()
			{
				var hash = new Hash<string>();
				hash["Key1"] = "Val1";

				_helper.ValidatorRegistrationScripts();
				string output1 = _helper.RegularExpressionValidator("myid1", "refid", ".*", "error!");
				string output2 = _helper.RegularExpressionValidator("myid2", "refid", ".*", "error!", hash);
				string output3 = _helper.RegularExpressionValidator("myid3", "refid", ".*", "error!", "group");
				string output4 = _helper.RegularExpressionValidator("myid4", "refid", ".*", "error!", "group", hash);

				Assert.That(output1, Is.EqualTo("<span id=\"myid1\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output2, Is.EqualTo("<span Key1=\"Val1\" id=\"myid2\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output3, Is.EqualTo("<span id=\"myid3\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output4, Is.EqualTo("<span Key1=\"Val1\" id=\"myid4\" style=\"display:none;color:red;\">error!</span>"));
			}
		}

		[TestFixture]
		public class When_Compare_Validator_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_output_is_rendered()
			{
				var hash = new Hash<string>();
				hash["Key1"] = "Val1";

				_helper.ValidatorRegistrationScripts();
				string output1 = _helper.CompareValidator("myid1", "refid", "refid2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!");
				string output2 = _helper.CompareValidator("myid2", "refid", "refid2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!", hash);
				string output3 = _helper.CompareValidator("myid3", "refid", "refid2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!", "group");
				string output4 = _helper.CompareValidator("myid4", "refid", "refid2", System.Web.UI.WebControls.ValidationDataType.Double, System.Web.UI.WebControls.ValidationCompareOperator.Equal, "error!", "group", hash);

				Assert.That(output1, Is.EqualTo("<span id=\"myid1\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output2, Is.EqualTo("<span Key1=\"Val1\" id=\"myid2\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output3, Is.EqualTo("<span id=\"myid3\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output4, Is.EqualTo("<span Key1=\"Val1\" id=\"myid4\" style=\"display:none;color:red;\">error!</span>"));
			}
		}

		[TestFixture]
		public class When_Range_Validator_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_output_is_rendered()
			{
				var hash = new Hash<string>();
				hash["Key1"] = "Val1";

				_helper.ValidatorRegistrationScripts();
				string output1 = _helper.RangeValidator("myid1", "refid", "1", "100", System.Web.UI.WebControls.ValidationDataType.Double, "error!");
				string output2 = _helper.RangeValidator("myid2", "refid", "1", "100", System.Web.UI.WebControls.ValidationDataType.Double, "error!", hash);
				string output3 = _helper.RangeValidator("myid3", "refid", "1", "100", System.Web.UI.WebControls.ValidationDataType.Double, "error!", "test");
				string output4 = _helper.RangeValidator("myid4", "refid", "1", "100", System.Web.UI.WebControls.ValidationDataType.Double, "error!", "test", hash);

				Assert.That(output1, Is.EqualTo("<span id=\"myid1\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output2, Is.EqualTo("<span Key1=\"Val1\" id=\"myid2\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output3, Is.EqualTo("<span id=\"myid3\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output4, Is.EqualTo("<span Key1=\"Val1\" id=\"myid4\" style=\"display:none;color:red;\">error!</span>"));
			}
		}

		[TestFixture]
		public class When_Custom_Validator_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_output_is_rendered()
			{
				var hash = new Hash<string>();
				hash["Key1"] = "Val1";

				_helper.ValidatorRegistrationScripts();
				string output1 = _helper.CustomValidator("myid1", "refid", "MyFunction", "error!");
				string output2 = _helper.CustomValidator("myid2", "refid", "MyFunction", "error!", hash);
				string output3 = _helper.CustomValidator("myid3", "refid", "MyFunction", "error!", "test");
				string output4 = _helper.CustomValidator("myid4", "refid", "MyFunction", "error!", "test", hash);

				Assert.That(output1, Is.EqualTo("<span id=\"myid1\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output2, Is.EqualTo("<span Key1=\"Val1\" id=\"myid2\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output3, Is.EqualTo("<span id=\"myid3\" style=\"display:none;color:red;\">error!</span>"));
				Assert.That(output4, Is.EqualTo("<span Key1=\"Val1\" id=\"myid4\" style=\"display:none;color:red;\">error!</span>"));
			}
		}

		[TestFixture]
		public class When_Element_Validators_is_invoked : BaseValidationHelperTester
		{
			[Test]
			public void The_correct_output_is_rendered()
			{
				var validator1 = new RequiredValidator("myid1", "refid", "error!");
				var validator2 = new RequiredValidator("myid2", "refid", "error!");

				_helper.ValidatorRegistrationScripts();
				string output = _helper.ElementValidation(new BaseValidator[] { validator1, validator2 });

				Assert.That(output, Is.EqualTo("<span id=\"myid1\" style=\"display:none;color:red;\">error!</span>\r\n<span id=\"myid2\" style=\"display:none;color:red;\">error!</span>\r\n"));
			}

			[Test]
			public void The_correct_output_is_rendered_with_a_reference_id_specified()
			{
				var validator1 = new RequiredValidator("myid1", "refid", "error!");
				var validator2 = new RequiredValidator("myid2", "refid", "error!");
				var validator3 = new RequiredValidator("myid3", "refid2", "error!");

				_helper.ValidatorRegistrationScripts();
				string output = _helper.ElementValidation(new BaseValidator[] { validator1, validator2, validator3 }, "refid");

				Assert.That(output, Is.EqualTo("<span id=\"myid1\" style=\"display:none;color:red;\">error!</span>\r\n<span id=\"myid2\" style=\"display:none;color:red;\">error!</span>\r\n"));
			}

			[Test]
			public void No_output_is_rendered_with_no_matching_id_specified()
			{
				var validator1 = new RequiredValidator("myid1", "refid", "error!");
				var validator2 = new RequiredValidator("myid2", "refid", "error!");
				var validator3 = new RequiredValidator("myid3", "refid2", "error!");

				_helper.ValidatorRegistrationScripts();
				string output = _helper.ElementValidation(new BaseValidator[] { validator1, validator2, validator3 }, "refid3");

				Assert.That(output, Is.Empty);
			}

			[Test, ExpectedException(typeof(System.ArgumentException))]
			public void When_validator_is_duplicated_for_reference_control()
			{
				var validator1 = new RequiredValidator("myid", "refid", "error!");
				var validator2 = new RequiredValidator("myid", "refid", "error!");

				_helper.ValidatorRegistrationScripts();
				_helper.ElementValidation(new BaseValidator[] { validator1, validator2 });
			}
		}

	}
#pragma warning restore 618,612
}
