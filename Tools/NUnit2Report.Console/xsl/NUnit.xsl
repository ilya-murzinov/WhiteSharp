<?xml version="1.0" encoding="utf-8"?>

<!--
   This XSL File is based on the NUnitSummary.xsl
   template created by Tomas Restrepo fot NAnt's NUnitReport.
   
   Modified by Gilles Bayon (gilles.bayon@laposte.net) for use
   with NUnit2Report.
   
   Modified by Murzinov Ilya (murz42@gmail.com) for use with
   NUnit2Report.Console.
-->

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0" xmlns:html="http://www.w3.org/Profiles/XHTML-transitional">

  <xsl:output method="xhtml" indent="yes"/>
  <xsl:include href="toolkit.xsl"/>

  <!-- ================================================================== -->
  <!-- Write a list of all packages with an hyperlink to the anchor of    -->
  <!-- of the package name.                                               -->
  <!-- ================================================================== -->
  <xsl:template name="packagelist">
    <div class="inner">
      <h2 id=":i18n:TestSuiteSummary">TestSuite Summary</h2>
      <table>
        <xsl:call-template name="packageSummaryHeader"/>
        <!-- list all packages recursively -->
        <xsl:for-each select="//test-suite[(child::results/test-case)]">
          <xsl:sort select="@name"/>
          <xsl:variable name="testCount" select="count(child::results/test-case)"/>
          <xsl:variable name="errorCount" select="count(child::results/test-case[@executed='False'])"/>
          <xsl:variable name="failureCount" select="count(child::results/test-case[@result='Failure' or @result='Error'])"/>
          <xsl:variable name="timeCount" select="translate(@time,',','.')"/>

          <!-- write a summary for the package -->
          <tr valign="top">
            <!-- set a nice color depending if there is an error/failure -->
            <xsl:attribute name="class">
              <xsl:choose>
                <xsl:when test="$failureCount &gt; 0">Failure</xsl:when>
                <xsl:when test="$errorCount &gt; 0"> Error</xsl:when>
                <xsl:otherwise>Pass</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <td width="50%">
              <a href="#{generate-id(@name)}">
                <xsl:attribute name="class">
                  <xsl:choose>
                    <xsl:when test="$failureCount &gt; 0">Failure</xsl:when>
                    <xsl:when test="$errorCount &gt; 0">Error</xsl:when>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:value-of select="@name"/>
              </a>
            </td>
            <td width="10%">
              <xsl:value-of select="$testCount"/>
            </td>
            <td width="10%">
              <xsl:value-of select="$failureCount"/>
            </td>
            <td width="10%">
              <xsl:value-of select="$errorCount"/>
            </td>
            <td nowrap="nowrap" width="10%" align="right">
              <xsl:variable name="successRate" select="($testCount - $failureCount - $errorCount) div $testCount"/>
              <b>
                <xsl:call-template name="display-percent">
                  <xsl:with-param name="value" select="$successRate"/>
                </xsl:call-template>
              </b>
            </td>
            <td width="10%" align="right">
              <xsl:call-template name="display-time">
                <xsl:with-param name="value" select="$timeCount"/>
              </xsl:call-template>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </div>
  </xsl:template>

  <xsl:template name="testsuites">
    <div class="inner">
      <xsl:for-each select="//test-suite[(child::results/test-case)]">
        <xsl:sort select="@name"/>
        <!-- create an anchor to this class name -->
        <a name="#{generate-id(@name)}"></a>
        <h3>
          <span id=":i18n:TestSuit">TestSuite </span>
          <xsl:value-of select="@name"/>
        </h3>

        <div class="thin">
          <!-- Header -->
          <xsl:call-template name="classesSummaryHeader"/>

          <!-- match the testcases of this package -->
          <xsl:apply-templates select="results/test-case">
            <xsl:sort select="@name" />
          </xsl:apply-templates>
        </div>
        <a style="margin-bottom:10px;" href="#top" class="link" id=":i18n:Backtotop">Back to top</a>
      </xsl:for-each>
    </div>
  </xsl:template>


  <xsl:template name="dot-replace">
    <xsl:param name="package"/>
    <xsl:choose>
      <xsl:when test="contains($package,'.')">
        <xsl:value-of select="substring-before($package,'.')"/>_<xsl:call-template name="dot-replace">
          <xsl:with-param name="package" select="substring-after($package,'.')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$package"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
