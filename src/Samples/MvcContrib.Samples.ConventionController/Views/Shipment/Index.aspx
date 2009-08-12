<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Microsoft.Web.Mvc"%>
<asp:Content ContentPlaceHolderId="childContent" runat="server">
	<% using (Html.BeginForm("Track", "Shipment")) { %>
		<% for( int i=0; i<10; i++ ) { %>
			<% string id = "trackingNumbers[" + i + "]"; %>
			<label for="<%= id %>">Tracking Number</label><%= Html.TextBox(id) %><br />
		<% } %>
		<%= Html.SubmitButton("Track", "Track") %> <a href="/shipment/track?trackingNumbers[0]=1234&trackingNumbers[1]=ABCD">Track by Link</a>
	<% } %>
	
	<br />
	<% using (Html.BeginForm("New", "Shipment")) { %>
		<span style="font-weight: bolder;">Ship To</span>
		<div>
			<label for="shipment.ShipTo.Name">Name</label><%= Html.TextBox("shipment.ShipTo.Name") %><br />
			<label for="shipment.ShipTo.StreetAddress">Street Address</label><%= Html.TextBox("shipment.ShipTo.StreetAddress")%><br />
			<label for="shipment.ShipTo.City">City</label><%= Html.TextBox("shipment.ShipTo.City")%><br />
			<label for="shipment.ShipTo.StateProvince">State/Province</label><%= Html.TextBox("shipment.ShipTo.StateProvince")%><br />
			<label for="shipment.ShipTo.ZipPostalCode">Zip/Postal Code</label><%= Html.TextBox("shipment.ShipTo.ZipPostalCode")%><br />
			<label for="shipment.ShipTo.Country">Country</label><%= Html.TextBox("shipment.ShipTo.Country")%><br />
		</div>
		<span style="font-weight: bolder;">Dimensions</span>
		<div>
			<label for="shipment.Dimensions.Length">Length</label><%= Html.TextBox("shipment.Dimensions.Length")%><br />
			<label for="shipment.Dimensions.Width">Width</label><%= Html.TextBox("shipment.Dimensions.Width")%><br />
			<label for="shipment.Dimensions.Height">Height</label><%= Html.TextBox("shipment.Dimensions.Height")%><br />
			<label for="shipment.Dimensions.Units">Units</label><select name="shipment.Dimensions.Units"><option>English</option><option>Metric</option></select>
		</div>
		<%= Html.SubmitButton("New", "New") %>
		</div>
	<% } %>
	
	<br />
	<a href="/shipment/XmlAction">To the Xml Outputing Action</a>
	<a href="/shipment/toTheRescue">To the Rescue!</a>
	<a href="/shipment/DivideByZero">Attempt to divide by zero!</a>
	<a href="/shipment/TrackSingle">Try and access TrackSingle action w/o an "id" RouteData parameter</a>
</asp:Content>
