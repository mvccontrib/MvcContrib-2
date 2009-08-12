<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Shipment>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="MvcContrib.Samples.Models"%>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
	You created a new Shipment:<br />
	<span style="font-weight: bolder;">Ship To</span>
		<div>
			<%= Model.ShipTo.Name %><br />
			<%= Model.ShipTo.StreetAddress %><br />
			<%= Model.ShipTo.City %>, <%= Model.ShipTo.StateProvince %> <%= Model.ShipTo.ZipPostalCode %><br />
			<%= Model.ShipTo.Country %><br />
		</div>
		<span style="font-weight: bolder;">Dimensions</span>
		<div>
			<%= Model.Dimensions.Length %>L, <%= Model.Dimensions.Width %>W, <%= Model.Dimensions.Height %>H <%= Model.Dimensions.Units.ToString() %><br />
		</div>
		</div>
</asp:Content>