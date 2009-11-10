<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<PropertyViewModel<IEnumerable<TypeViewModel>>>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<!-- AddNewChild-->
<%
	var parentDivId = Model.Name + "Fields";
	var depth = 0;
	var typeName = Model.Name;
	var arrayName = Model.Name;
	var arrayIndexName = arrayName + ".Index";
%>
<script type="text/javascript">
/* <![CDATA[ */	
	
$(document).ready(function(){
	
	$('.removeLink<%=typeName %>').click(removeRow);

	var id = 0;
	var depth = <%= depth %>;

	var transform = function (text) {
		if (text) {
			var count = 0;
			var result = text.replace(/(\[|_)[0-9]+(\]|_)/g, function (val, prefix, postfix) {
				var toReturn = val;
				if (count == depth) {
					toReturn = prefix + id + postfix;
				}
				count++;
				return toReturn;
			});//'clone' + id);
			return result;
		}
		return text;
	};

	var rework<%=typeName %> = function() {
		var theId = $(this).attr("id");
		if(theId != "" && theId != null){
			$(this).attr("id", transform(theId));
		}
		$(this).attr("for", transform($(this).attr("for")));
		$(this).attr("name", transform($(this).attr("name")));
		if ($(this).attr("name") == "<%=arrayIndexName %>") {
			$(this).val(id);//"clone" + id);
		}
		
		workaroundIERadioButtonCloneProblemByRemovingAndReaddingTheRadioButton<%=typeName %>($(this));
	};	
	
	function reworkAll<%=typeName %>() {
	    id = 0;
	    $('#<%= parentDivId %> > div').each(function() {
	        $(this).attr('class', '<%=typeName %>'+id);
		    $(this).find("*").not("option").each(rework<%=typeName %>);
		    
		    id++;
	    });
	}
	
	var workaroundIERadioButtonCloneProblemByRemovingAndReaddingTheRadioButton<%=typeName %> = function(theInput){
		if ($.browser.msie) {
			var isRadiobutton = theInput.attr("type") == "radio";
			if(isRadiobutton){
				var radio = theInput[0]; 	
				replaceRadio<%=typeName %>(radio);
			}
		}
	}

	var clearInputs<%=typeName %> = function() {
		var isCheckbox = $(this).attr("type") == "checkbox";
		var isRadiobutton = $(this).attr("type") == "radio";
		var isPicker = $(this).attr("id").indexOf('_result') >= 0;

		$(this).removeAttr('disabled');

		if (($(this).attr("type") != "hidden" || isPicker) && !isCheckbox && !isRadiobutton) {
			$(this).val("");
		}
		
		if (isCheckbox) {
			$(this).removeAttr("checked");
		}

		if (isRadiobutton) {
			$(this).removeAttr("checked");
		}
	};

	$(function() {
		$("#clone<%=typeName %>").click(function() {
			var templateDiv = $("div.<%=typeName %>0").clone();

			templateDiv.find('.errorIndicator').remove();
			
			templateDiv.find('.removeLink<%=typeName %>').click(removeRow);
			
			$(templateDiv).find("*").not("option").not(":button").each(clearInputs<%=typeName %>);

			templateDiv.appendTo("#<%=parentDivId %>");

            reworkAll<%=typeName %>();

			$("#<%=parentDivId %>").find("div").trigger("begin");
			return false;
		});
	});	
	
		function removeRow() {
		var firstDiv = "<%= typeName %>0";
		var parentDivClass = $(this).parent().parent().attr("class");

		var parentRow = $(this).parent().parent();

		if (parentDivClass != firstDiv){
			parentRow.remove();
		}
		else {
			parentRow.find("*").not("option").not(":button").each(clearInputs<%=typeName %>);
		}
		
		reworkAll<%=typeName%>();
		
		return false;
	}

});
	/* ]]> */
</script>
<p class="ml155">
<a id="clone<%=Model.Name%>"  href="#">
Add Another <%= Model.Label %></a></p>