<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="NUnit.xsl"/>
  <xsl:preserve-space elements='a root'/>
  
  <!--
	====================================================
		Create the page structure
    ====================================================
-->
  <xsl:template match="test-results">
    <HTML>
      <HEAD>
        <style>
          body {
          font:normal 68% verdana,arial,helvetica;
          color:#000000;
          }

          div.outer{
          width:99%;
          border: #bcd7cd 4px solid;
          margin-top:20px;
          }

          div.inner{
          margin:0 auto;
          width:99%;
          margin-bottom:10px;
          margin-top:10px;
          }

          div.outerHeader{
          padding-left:6px;
          width:99%;
          margin-bottom:10px;
          margin-top:10px;
          }

          div.innerHeader{
          width:99%;
          margin-bottom:10px;
          }

          span.covered {
          background: #00df00;
          border:#9c9c9c 1px solid;
          }
          span.uncovered {
          background: #df0000;
          border-top:#9c9c9c 1px solid;
          border-bottom:#9c9c9c 1px solid;
          border-right:#9c9c9c 1px solid;
          }
          span.ignored {
          background: #ffff00;
          border-top:#9c9c9c 1px solid;
          border-bottom:#9c9c9c 1px solid;
          border-right:#9c9c9c 1px solid;
          }

          table{
          border: #dcdcdc 2px solid;
          border-collapse: collapse;
          width:100%;
          cellpadding:2;
          cellspasing:0;
          }

          table.noborderHeader{
          BORDER-BOTTOM: #dcdcdc 0px solid;
          BORDER-RIGHT: #dcdcdc 0px solid;
          BORDER-TOP: #dcdcdc 0px solid;
          BORDER-LEFT: #dcdcdc 0px solid;
          padding-left:0px;
          padding-right:0px;
          }

          table.noborder{
          width:100%;
          BORDER-BOTTOM: #dcdcdc 0px solid;
          BORDER-RIGHT: #dcdcdc 0px solid;
          BORDER-TOP: #dcdcdc 0px solid;
          BORDER-LEFT: #dcdcdc 0px solid;
          padding-left:0px;
          padding-right:0px;
          }

          div.thin{
          /*width:95%;*/
          BORDER-BOTTOM: #dcdcdc 2px solid;
          BORDER-RIGHT: #dcdcdc 1px solid;
          BORDER-TOP: #dcdcdc 1px solid;
          BORDER-LEFT: #dcdcdc 1px solid;
          padding-left:0px;
          padding-right:0px;
          }

          tr.nobottom{
          BORDER-BOTTOM: #dcdcdc 0px solid;
          BORDER-RIGHT: #dcdcdc 0px solid;
          BORDER-TOP: #dcdcdc 1px solid;
          BORDER-LEFT: #dcdcdc px solid;
          padding-left:0px;
          padding-right:0px;
          }

          td.nobottom {
          FONT-SIZE: 68%;
          BORDER-BOTTOM: #dcdcdc 0px solid;
          BORDER-RIGHT: #dcdcdc 1px solid;
          BORDER-TOP: #dcdcdc 1px solid;
          BORDER-LEFT: #dcdcdc 1px solid;
          padding-left:3px;
          padding-right:3px;
          }

          td.noborder {
          FONT-SIZE: 68%;
          BORDER-BOTTOM: #dcdcdc 0px solid;
          BORDER-RIGHT: #dcdcdc 0px solid;
          BORDER-TOP: #dcdcdc 0px solid;
          BORDER-LEFT: #dcdcdc 0px solid;
          padding-left:0px;
          padding-right:0px;
          }

          td {
          FONT-SIZE: 68%;
          BORDER-BOTTOM: #dcdcdc 1px solid;
          BORDER-RIGHT: #dcdcdc 1px solid;
          BORDER-TOP: #dcdcdc 1px solid;
          BORDER-LEFT: #dcdcdc 1px solid;
          padding-left:3px;
          padding-right:3px;
          }
          p {
          line-height:0em;
          margin-top:5em;
          margin-bottom:0em;
          }
          h1 {
          MARGIN: 0px 0px 5px;
          FONT: bold 200% verdana,arial,helvetica;
          }
          h2 {
          MARGIN: 10px 0px 5px;
          FONT: bold 145% verdana,arial,helvetica;
          }
          h3 {
          MARGIN: 10px 0px 5px;
          FONT: bold 115% verdana,arial,helvetica;
          }
          h4 {
          MARGIN: 10px 0px 5px;
          FONT: bold 100% verdana,arial,helvetica;
          }
          h5 {
          MARGIN: 10px 0px 5px;
          FONT: bold 100% verdana,arial,helvetica
          }
          h6 {
          MARGIN: 10px 0px 5px;
          FONT: bold 100% verdana,arial,helvetica
          }
          .exception{
          display:none;
          }
          .Error {
          color:red;
          font-weight:bold;
          }
          .Failure {
          font-weight:bold;
          color:red;
          }
          .Ignored {
          font-weight:bold;
          }
          .FailureDetail {
          font-size: -1;
          padding-left: 2.0em;
          background:#cdcdcd;
          }
          .Pass{
          font-weight:bold;
          color:green;
          text-decoration: none;
          BORDER-BOTTOM: #dcdcdc 0px solid;
          BORDER-RIGHT: #dcdcdc 1px solid;
          BORDER-TOP: #dcdcdc 1px solid;
          BORDER-LEFT: #dcdcdc 1px solid;
          }
          .TableHeader {
          background: #efefef;
          color: #000;
          font-weight: bold;
          horizontal-align: center;
          }

          a.regular{
          color:blue;
          }
          a:visited {
          color:inherit;
          }
          a {
          color:inherit;
          }
          a.link {
          color:blue;
          }
          a.link:active {
          color:blue;
          }
          a.link:visited {
          color:blue;
          }
          a.summarie {
          color:#000;
          text-decoration: none;
          }
          a.summarie:active {
          color:#000;
          text-decoration: none;
          }
          a.summarie:visited {
          color:#000;
          text-decoration: none;
          }
          .description {
          margin-top:1px;
          padding:3px;
          background-color:#dcdcdc;
          color:#000;
          font-weight:normal;
          }
          a.Pass {
          font-weight:bold;
          color:green;
          }
          a.Pass:link {
          font-weight:bold;
          color:green;
          }
          a.Pass:active {
          font-weight:bold;
          color:green;
          }
          a.Pass:visited {
          font-weight:bold;
          color:green;
          }
          a.Failure {
          font-weight:bold;
          color:red;
          }
          a.Failure:visited {
          font-weight:bold;
          color:red;
          }
          a.Failure:active {
          font-weight:bold;
          color:red;
          }
          a.error {
          font-weight:bold;
          color:red;
          }
          a.error:visited {
          font-weight:bold;
          color:red;
          }
          a.error:active {
          font-weight:bold;
          color:red;
          /*text-decoration: none;
          padding-left:5px;*/
          }
          a.ignored {
          font-weight:bold;
          color:#000;
          }
          a.ignored:visited {
          color:#000;
          font-weight:bold;
          }
          a.ignored:active {
          font-weight:bold;
          color:#000;
          }
          .excellent{
          color:green;
          font-weight:bold;
          }
          .good{
          color:orange;
          font-weight:bold;
          }
          .average{
          color:orange;
          font-weight:bold;
          }
          .bad{
          color:red;
          font-weight:bold;
          }

        </style>
        <script language="JavaScript">
          <![CDATA[   
	  function Toggle(id) {
	  var element = document.getElementById(id);
		 if ( element.style.display == "none" )
			element.style.display = "block";
		 else 
			element.style.display = "none";
	  }

	  function ToggleImage(id) {
	  var element = document.getElementById(id);

		 if ( element.innerText   == "-" )
			element.innerText   = "+";
		 else 
			element.innerText = "-";
	  }
	]]>
        </script>
      </HEAD>
      <body>
        <a name="#top"></a>
        <div class="outerHeader">
          <xsl:call-template name="header"/>
        </div>
        <!-- Summary part -->
        <div class="outer">
          <xsl:call-template name="summary"/>
        </div>
        <!-- Package List part -->
        <div class="outer">
          <xsl:call-template name="packagelist"/>
        </div>
        <!-- For each testsuite create the part -->
        <div class="outer">
          <xsl:call-template name="testsuites"/>
        </div>
        <!-- Environment info part -->
        <div class="outer">
          <xsl:call-template name="envinfo"/>
        </div>
      </body>
    </HTML>
  </xsl:template>
</xsl:stylesheet>
