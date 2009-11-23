<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PropertyViewModel<IEnumerable<TypeViewModel>>>" %>
<%@ Import Namespace="MvcContrib.UI.InputBuilder"%>
<%@ Import Namespace="System.Linq"%>
<%@ Import Namespace="System.Collections.Generic"%>
<%@ Import Namespace="MvcContrib.UI.InputBuilder.Views"%>
<asp:Content ID="Content1" ContentPlaceHolderID="Label" runat="server">
<label for="<%=Model.Name%>"><%=Model.Label%></label></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Input" runat="server">
<!-- Array.aspx -->
<%if (Model.Value.Count()>0){%>		
<table class="inputBuilder-array " id="<%=Model.Name%>Fields">
<thead>
<tr><%foreach (var property in Model.Value.First().Properties){%>
		<th><%=property.Label %></th>
	<%} %><%if (Model.Value.First().HasDeleteButton())
		 {%><th></th><%} %></tr></thead>
	<tbody>	
	<%int index = 0;
	int fieldcount = 0;
	foreach (var model in Model.Value)
	{
		fieldcount = 0;%>
		
		<tr class="<%=Model.Name + model.Index%> subform" >
		<input type="hidden" name="<%=Model.Name%>.Index" id="" value="<%=model.Index %>" />	
		<%foreach (var property in model.Properties){%>
		<td><%	Html.RenderPartial(property);%></td>
		<%
	fieldcount++;} %>
		<%if (model.HasDeleteButton()){%>
		<td><%Html.RenderPartial("ArrayRemoveButton", Model); %></td>
		<%fieldcount++;} %>	
		</tr><%
		index++;
	}%>
	</tbody>
	<%if (Model.HasAddButton())
   {%>
	<tfoot>
	<tr><td colspan="<%=fieldcount %>"><%Html.RenderPartial("ArrayAddButton", Model); %></td></tr>
	</tfoot>
	<%} %>
</table>
<%} %>
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
	    $('#<%= parentDivId %> tbody > tr').each(function() {
	        $(this).attr('class', '<%=typeName %>'+id + ' subform');
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
			var templateDiv = $(".<%=typeName %>0").clone();

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

		var parentRow = $(this).parent().parent();
		var parentDivClass = parentRow.attr("class");

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

</asp:Content>
