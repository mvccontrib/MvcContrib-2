<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>IncludeCombiner</h2>
    <p>
        This page is intentionally unstyled and lacking in JS.  This is so the first request doesn't have the side-effect of populating the include-combiner while I benchmark.  It also means the first-request JIT compilation doesn't get included in my benchmarking.
    </p>
    <p><%= Html.RouteLink("index", new { controller = "include", action = "index" })%></p>
    
    <h3>Benchmarking</h3>
    <p>Before each test:</p>
    <ol>
			<li>Ensure Compilation Debug="false" in web.config</li>
			<li>Recycle IIS app-pool</li>
			<li>Clear browser cache</li>
		</ol>
		<ol>
			<li>Request each of the benchmark pages in order; note down timing result from Firebug/Net; this gives us time-to-render whole page with empty server-side cache.</li>
			<li>Run Hammerhead against each benchmark page (now that the server-side cache is primed)</li>
    </ol>
    
    <h3>Tests:</h3>
    <ol>
			<li>
				<h4><%= Html.RouteLink("jsjquery", new { action = "jsjquery" })%></h4>
				<table>
					<thead>
						<tr>
							<th>Test</th>
							<th>Glob together to 1 request?</th>
							<th>Minify?</th>
							<th>Gzip?</th>
							<th>Deflate?</th>
							<th>Write cache headers?</th>
							<th>Includes' size (Kb)</th>
							<th>1st request (prime server-side cache) (ms)</th>
							<th>Hammerhead; Empty client-cache (ms)</th>
							<th>Hammerhead; Primed client-cache (ms)</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>Raw</td>
							<td>no</td><td>no</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request raw</td>
							<td>yes</td><td>no</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified</td>
							<td>yes</td><td>yes</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified gzip'd</td>
							<td>yes</td><td>yes</td><td>yes</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified gzip'd cache'd</td>
							<td>yes</td><td>yes</td><td>yes</td><td>no</td><td>yes</td>
							<td></td><td></td><td></td><td></td>
						</tr>
					</tbody>
				</table>
			</li>
			<li>
				<h4><%= Html.RouteLink("jsmany", new { action = "jsmany" })%></h4>
				<table>
					<thead>
						<tr>
							<th>Test</th>
							<th>Glob together to 1 request?</th>
							<th>Minify?</th>
							<th>Gzip?</th>
							<th>Deflate?</th>
							<th>Write cache headers?</th>
							<th>Includes' size</th>
							<th>1st request (prime server-side cache)</th>
							<th>Hammerhead; Cleared client-cache</th>
							<th>Hammerhead; Allow client-cache</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>Raw</td>
							<td>no</td><td>no</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request raw</td>
							<td>yes</td><td>no</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified</td>
							<td>yes</td><td>yes</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified gzip'd</td>
							<td>yes</td><td>yes</td><td>yes</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified gzip'd cache'd</td>
							<td>yes</td><td>yes</td><td>yes</td><td>no</td><td>yes</td>
							<td></td><td></td><td></td><td></td>
						</tr>
					</tbody>
				</table>
			</li>
			<li>
				<h4><%= Html.RouteLink("cssone", new { action = "cssone" })%></h4>
				<table>
					<thead>
						<tr>
							<th>Test</th>
							<th>Glob together to 1 request?</th>
							<th>Minify?</th>
							<th>Gzip?</th>
							<th>Deflate?</th>
							<th>Write cache headers?</th>
							<th>Includes' size</th>
							<th>1st request (prime server-side cache)</th>
							<th>Hammerhead; Cleared client-cache</th>
							<th>Hammerhead; Allow client-cache</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>Raw</td>
							<td>no</td><td>no</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request raw</td>
							<td>yes</td><td>no</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified</td>
							<td>yes</td><td>yes</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified gzip'd</td>
							<td>yes</td><td>yes</td><td>yes</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified gzip'd cache'd</td>
							<td>yes</td><td>yes</td><td>yes</td><td>no</td><td>yes</td>
							<td></td><td></td><td></td><td></td>
						</tr>
					</tbody>
				</table>
			</li>
			<li>
				<h4><%= Html.RouteLink("cssmany", new { action = "cssmany" })%></h4>
				<table>
					<thead>
						<tr>
							<th>Test</th>
							<th>Glob together to 1 request?</th>
							<th>Minify?</th>
							<th>Gzip?</th>
							<th>Deflate?</th>
							<th>Write cache headers?</th>
							<th>Includes' size</th>
							<th>1st request (prime server-side cache)</th>
							<th>Hammerhead; Cleared client-cache</th>
							<th>Hammerhead; Allow client-cache</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>Raw</td>
							<td>no</td><td>no</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request raw</td>
							<td>yes</td><td>no</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified</td>
							<td>yes</td><td>yes</td><td>no</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified gzip'd</td>
							<td>yes</td><td>yes</td><td>yes</td><td>no</td><td>no</td>
							<td></td><td></td><td></td><td></td>
						</tr>
						<tr>
							<td>1 request minified gzip'd cache'd</td>
							<td>yes</td><td>yes</td><td>yes</td><td>no</td><td>yes</td>
							<td></td><td></td><td></td><td></td>
						</tr>
					</tbody>
				</table>
			</li>
		</ol>
</asp:Content>
