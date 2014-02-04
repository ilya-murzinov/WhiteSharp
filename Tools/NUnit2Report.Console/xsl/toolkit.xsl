<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:nunit2report="urn:my-scripts">
  <xsl:param name="nant.filename" />
  <xsl:param name="nant.version" />
  <xsl:param name="nant.project.name" />
  <xsl:param name="nant.project.buildfile" />
  <xsl:param name="nant.project.basedir" />
  <xsl:param name="nant.project.default" />
  <xsl:param name="sys.os" />
  <xsl:param name="sys.os.platform" />
  <xsl:param name="sys.os.version" />
  <xsl:param name="sys.clr.version" />
  <xsl:param name="sys.machine.name" />
  <xsl:param name="sys.username" />

  <msxsl:script language="C#" implements-prefix="nunit2report">

    public string TestCaseName(string path) {

    string[] a = path.Split('.');

    return(a[a.Length-1]);
    }

  </msxsl:script>

  <!--
    TO DO
	Corriger les alignement sur error
	Couleur http://nanning.sourceforge.net/junit-report.html
-->

  <!--
    format a number in to display its value in percent
    @param value the number to format
-->
  <xsl:template name="display-time">
    <xsl:param name="value"/>
    <xsl:choose>
      <xsl:when test="format-number($value,'0.000')='NaN'">
        <xsl:value-of select="format-number(0.000,'0.000')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="format-number($value,'0.000')"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--
    format a number in to display its value in percent
    @param value the number to format
-->
  <xsl:template name="display-percent">
    <xsl:param name="value"/>
    <xsl:value-of select="format-number($value,'0.00 %')"/>
  </xsl:template>

  <!--
    transform string like a.b.c to ../../../
    @param path the path to transform into a descending directory path
-->
  <xsl:template name="path">
    <xsl:param name="path"/>
    <xsl:if test="contains($path,'.')">
      <xsl:text>../</xsl:text>
      <xsl:call-template name="path">
        <xsl:with-param name="path">
          <xsl:value-of select="substring-after($path,'.')"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
    <xsl:if test="not(contains($path,'.')) and not($path = '')">
      <xsl:text>../</xsl:text>
    </xsl:if>
  </xsl:template>

  <xsl:template name="substring-after-last">
    <xsl:param name="string" />
    <xsl:param name="delimiter" />
    <xsl:choose>
      <xsl:when test="contains($string, $delimiter)">
        <xsl:call-template name="substring-after-last">
          <xsl:with-param name="string"
            select="substring-after($string, $delimiter)" />
          <xsl:with-param name="delimiter" select="$delimiter" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of
     select="$string" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--
	template that will convert a carriage return into a br tag
	@param word the text from which to convert CR to BR tag
-->
  <xsl:template name="br-replace">
    <xsl:param name="word"/>
    <xsl:choose>
      <xsl:when test="contains($word,'&#xA;')">
        <xsl:value-of select="substring-before($word,'&#xA;')"/>
        <br/>
        <xsl:call-template name="br-replace">
          <xsl:with-param name="word" select="substring-after($word,'&#xA;')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$word"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--
		=====================================================================
		classes summary header
		=====================================================================
-->
  <xsl:template name="header">
    <xsl:param name="path"/>
    <div class="innerHeader">
      <h1 style="margin-bottom:10px;">
        <span id=":i18n:UnitTestsResults">Unit Tests Results</span>
        <xsl:value-of select="$nant.project.name"/>
      </h1>
      <table class="noborderHeader">
        <tr>
          <td class="noborder" width="50%" align="left">
            <span id=":i18n:GeneratedBy">Generated  on: </span><xsl:value-of select="@date"/> - <xsl:value-of select="concat(@time,' ')"/> <a href="#envinfo" class="link" id=":i18n:EnvironmentInformation">Environment Information</a>
          </td>
          <td class="noborder" width="50%" align="right">
            <span id=":i18n:Designed">Designed for use with </span>
            <a class="link" href='http://nunit.sourceforge.net/'>NUnit.</a>
          </td>
        </tr>
      </table>
    </div>
  </xsl:template>

  <xsl:template name="summaryHeader">
    <tr valign="top" class="TableHeader">
      <td width="50%">
        <b id=":i18n:Name">Name</b>
      </td>
      <td width="10%">
        <b id=":i18n:Tests">Tests</b>
      </td>
      <td width="10%">
        <b id=":i18n:Failures">Failures</b>
      </td>
      <td width="10%">
        <b id=":i18n:Errors">Errors</b>
      </td>
      <td width="10%">
        <b id=":i18n:SuccessRate">Success Rate</b>
      </td>
      <td width="10%" nowrap="nowrap">
        <b id=":i18n:Time">Time(s)</b>
      </td>
    </tr>
  </xsl:template>

  <!--
		=====================================================================
		package summary header
		=====================================================================
-->
  <xsl:template name="packageSummaryHeader">
    <tr class="TableHeader" valign="top">
      <td width="50%">
        <b id=":i18n:Name">Name</b>
      </td>
      <td width="10%">
        <b id=":i18n:Tests">Tests</b>
      </td>
      <td width="10%">
        <b id=":i18n:Failures">Failures</b>
      </td>
      <td width="10%">
        <b id=":i18n:Errors">Errors</b>
      </td>
      <td width="10%">
        <b id=":i18n:SuccessRate">Success Rate</b>
      </td>
      <td width="10%" nowrap="nowrap">
        <b id=":i18n:Time">Time(s)</b>
      </td>
    </tr>
  </xsl:template>

  <!--
		=====================================================================
		classes summary header
		=====================================================================
-->
  <xsl:template name="classesSummaryHeader">
    <table class="noborder">
      <tr class="TableHeader" valign="top">
        <td class="nobottom" width="80%">
          <b id=":i18n:Name">Name</b>
        </td>
        <td class="nobottom" width="10%">
          <b id=":i18n:Status">Status</b>
        </td>
        <td class="nobottom" width="10%" nowrap="nowrap">
          <b id=":i18n:Time">Time(s)</b>
        </td>
      </tr>
    </table>
  </xsl:template>

  <!--
		=====================================================================
		Write the summary report
		It creates a table with computed values from the document:
		User | Date | Environment | Tests | Failures | Errors | Rate | Time
		Note : this template must call at the testsuites level
		=====================================================================
-->
  <xsl:template name="summary">
    <div class="inner">
      <h2 id=":i18n:Summary">Summary</h2>
      <xsl:variable name="name" select="@name"/>
      <xsl:variable name="total" select="@total"/>
      <xsl:variable name="failures" select="@failures+@errors"/>
      <xsl:variable name="errors" select="@not-run"/>
      <xsl:variable name="timeCount" select="translate(test-suite/@time,',','.')"/>
      <xsl:variable name="successRate" select="($total - $failures) div ($total + $errors)"/>

      <table>
        <xsl:call-template name="summaryHeader"/>
        <tr valign="top">
          <xsl:attribute name="class">
            <xsl:choose>
              <xsl:when test="$successRate &gt; 0.95">excellent</xsl:when>
              <xsl:when test="$successRate &gt; 0.85 and $successRate &lt; 0.95">good</xsl:when>
              <xsl:when test="$successRate &gt; 0.75 and $successRate &lt; 0.85">average</xsl:when>
              <xsl:otherwise>bad</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <td width="50%">
            <a>
              <xsl:attribute name="href">
                <xsl:variable name="link" select="substring-before($name, '.dll')"/>
                <xsl:call-template name="substring-after-last">
                  <xsl:with-param name="string" select="concat($link,'.txt')"/>
                  <xsl:with-param name="delimiter" select="'\'"/>
                </xsl:call-template>
              </xsl:attribute>
              <xsl:attribute name="class">
                <xsl:choose>
                  <xsl:when test="$successRate &gt; 0.95">excellent</xsl:when>
                  <xsl:when test="$successRate &gt; 0.85 and $successRate &lt; 0.95">good</xsl:when>
                  <xsl:when test="$successRate &gt; 0.75 and $successRate &lt; 0.85">average</xsl:when>
                  <xsl:otherwise>bad</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:call-template name="substring-after-last">
                <xsl:with-param name="string" select="concat(substring-before($name,'.dll'),' (Click to see the log)')"/>
                <xsl:with-param name="delimiter" select="'\'"/>
              </xsl:call-template>
            </a>
          </td>
          <td width="10%">
            <xsl:value-of select="$total + $errors"/>
          </td>
          <td width="10%">
            <xsl:value-of select="$failures"/>
          </td>
          <td width="10%">
            <xsl:value-of select="$errors"/>
          </td>
          <td nowrap="nowrap" width="10%" align="right">
            <xsl:call-template name="display-percent">
              <xsl:with-param name="value" select="$successRate"/>
            </xsl:call-template>
          </td>
          <td  width="10%" align="right">
            <xsl:call-template name="display-time">
              <xsl:with-param name="value" select="$timeCount"/>
            </xsl:call-template>

          </td>
        </tr>
      </table>
      <span id=":i18n:Note">Note</span>: <i id=":i18n:failures">failures </i> <span id=":i18n:anticipated">are anticipated and checked for with assertions while </span> <i id=":i18n:errors">errors </i> <span id=":i18n:unanticipated">are unanticipated.</span>
    </div>
  </xsl:template>

  <!--
		=====================================================================
		testcase report
		=====================================================================
-->
  <xsl:template match="test-case">
    <xsl:param name="summary.xml"/>
    <xsl:param name="open.description"/>

    <xsl:variable name="summaries" select="document($summary.xml)" />

    <xsl:variable name="Mname" select="concat('M:',./@name)" />

    <xsl:variable name="result">
      <xsl:choose>
        <xsl:when test="./failure">
          <span id=":i18n:Failure">Failure</span>
        </xsl:when>
        <xsl:when test="./error">
          <span id=":i18n:Error">Error</span>
        </xsl:when>
        <xsl:when test="@executed='False'">
          <span id=":i18n:Ignored">Ignored</span>
        </xsl:when>
        <xsl:otherwise>
          <span id=":i18n:Pass">Pass</span>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="newid" select="generate-id(@name)" />
    <table class="noborder">
      <tr class="noborder" valign="top">
        <xsl:attribute name="class">
          <xsl:value-of select="$result"/>
        </xsl:attribute>

        <td class="nobottom" width="80%">
          <xsl:choose>
            <xsl:when test="$summary.xml != ''">
              <!-- Triangle image -->
              <a title="Show/Hide XML Comment" class="summarie">
                <xsl:attribute name="href">
                  javascript:Toggle('<xsl:value-of select="concat('M:',$newid)"/>');ToggleImage('<xsl:value-of select="concat('I:',$newid)"/>')
                </xsl:attribute>
                <xsl:attribute name="id">
                  <xsl:value-of select="concat('I:',$newid)"/>
                </xsl:attribute>
              </a>
            </xsl:when>
          </xsl:choose>
          <!-- If failure, add click on the test method name and color red -->
          <xsl:choose>
            <xsl:when test="$result = 'Failure' or $result = 'Error'">
              <a title="Show/Hide message error">
                <xsl:attribute name="href">
                  javascript:Toggle('<xsl:value-of select="$newid"/>')
                </xsl:attribute>
                <xsl:attribute name="class">error</xsl:attribute>
                <xsl:value-of select="nunit2report:TestCaseName(./@name)"/>
              </a>
            </xsl:when>
            <xsl:when test="$result = 'Ignored'">
              <a title="Show/Hide message error">
                <xsl:attribute name="href">
                  javascript:Toggle('<xsl:value-of select="$newid"/>')
                </xsl:attribute>
                <xsl:attribute name="class">ignored</xsl:attribute>
                <xsl:value-of select="nunit2report:TestCaseName(./@name)"/>
              </a>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="class">Pass</xsl:attribute>
              <xsl:value-of select="nunit2report:TestCaseName(./@name)"/>
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td class="nobottom" width="10%" align="right">
          <xsl:attribute name="id">
            :i18n:<xsl:value-of select="$result"/>
          </xsl:attribute>
          <xsl:value-of select="$result"/>
        </td>
        <td class="nobottom" width="10%" align="right">
          <xsl:call-template name="display-time">
            <xsl:with-param name="value" select="@time"/>
          </xsl:call-template>
        </td>
      </tr>

      <xsl:if test="$result != &quot;Pass&quot;">
        <tr class="nobottom" style="display: none;">
          <xsl:attribute name="id">
            <xsl:value-of select="$newid"/>
          </xsl:attribute>
          <td colspan="4" class="FailureDetail">
            <xsl:apply-templates select="./failure"/>
            <xsl:apply-templates select="./error"/>
            <xsl:apply-templates select="./reason"/>
          </td>
        </tr>
      </xsl:if>
    </table>
  </xsl:template>

  <!-- Note : the below template error and failure are the same style
            so just call the same style store in the toolkit template -->
  <!-- <xsl:template match="failure">
	<xsl:call-template name="display-failures"/>
</xsl:template>

<xsl:template match="error">
	<xsl:call-template name="display-failures"/>
</xsl:template> -->

  <!-- Style for the error and failure in the tescase template -->
  <!-- <xsl:template name="display-failures">
	<xsl:choose>
		<xsl:when test="not(@message)">N/A</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="@message"/>
		</xsl:otherwise>
	</xsl:choose> -->
  <!-- display the stacktrace -->
  <!-- 	<code>
		<p/>
		<xsl:call-template name="br-replace">
			<xsl:with-param name="word" select="."/>
		</xsl:call-template>
	</code> -->
  <!-- the later is better but might be problematic for non-21" monitors... -->
  <!--pre><xsl:value-of select="."/></pre-->
  <!-- </xsl:template>
 -->

  <!--
		=====================================================================
		Environtment Info Report
		=====================================================================
-->
  <xsl:template name="envinfo">
    <div class="inner">
      <a name="envinfo"></a>
      <h2 id=":i18n:EnvironmentInformation">Environment Information</h2>
      <table>
        <tr class="TableHeader">
          <td id=":i18n:Property">Property</td>
          <td id=":i18n:Value">Value</td>
        </tr>
        <tr>
          <td id=":i18n:MachineName">Machine name</td>
          <td>
            <xsl:value-of select="$sys.machine.name"/>
          </td>
        </tr>
        <tr>
          <td id=":i18n:User">User</td>
          <td>
            <xsl:value-of select="$sys.username"/>
          </td>
        </tr>
        <tr>
          <td id=":i18n:OperatingSystem">Operating System</td>
          <td>
            <xsl:value-of select="$sys.os"/>
          </td>
        </tr>
        <tr>
          <td id=":i18n:NETCLRVersion">.NET CLR Version</td>
          <td>
            <xsl:value-of select="$sys.clr.version"/>
          </td>
        </tr>
      </table>
      <a href="#top" class="link" id=":i18n:Backtotop">Back to top</a>
    </div>
  </xsl:template>

  <!-- I am sure that all nodes are called -->
  <xsl:template match="*">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template name="for.loop">

    <xsl:param name="i"      />
    <xsl:param name="count"  />

    <!--begin_: Line_by_Line_Output -->
    <xsl:if test="$i &lt;= $count">
      &#160;
    </xsl:if>

    <!--begin_: RepeatTheLoopUntilFinished-->
    <xsl:if test="$i &lt;= $count">
      <xsl:call-template name="for.loop">
        <xsl:with-param name="i">
          <xsl:value-of select="$i + 1"/>
        </xsl:with-param>
        <xsl:with-param name="count">
          <xsl:value-of select="$count"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>

  </xsl:template>
</xsl:stylesheet>