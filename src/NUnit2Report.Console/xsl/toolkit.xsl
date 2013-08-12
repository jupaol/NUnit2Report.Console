<?xml version="1.0" encoding="ISO-8859-1"?>
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
    <xsl:value-of select="format-number($value,'0.000')"/>
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
    <h1>
      <span id=":i18n:UnitTestsResults">Unit Tests Results</span> <xsl:value-of select="$nant.project.name"/>
    </h1>
    <table width="100%">
      <tr>
        <td align="left">
          <span id=":i18n:GeneratedBy">Generated </span> on: <xsl:value-of select="@date"/> - <xsl:value-of select="concat(@time,' ')"/> <a href="#envinfo" id=":i18n:EnvironmentInformation">Environment Information</a>
        </td>
        <td align="right">
          <span id=":i18n:Designed">Designed for use with </span> <a href='http://nunit.sourceforge.net/'>NUnit.</a>
        </td>
      </tr>
    </table>
    <hr size="1"/>
  </xsl:template>

  <xsl:template name="summaryHeader">
    <tr valign="top" class="TableHeader">
      <td width="50px">
        <b id=":i18n:Tests">Tests</b>
      </td>
      <td width="70px">
        <b id=":i18n:Failures">Failures</b>
      </td>
      <td width="70px">
        <b id=":i18n:Errors">Errors</b>
      </td>
      <td width="70px">
        <b id=":i18n:Errors">Ignored</b>
      </td>
      <td colspan="2">
        <b id=":i18n:SuccessRate">Success Rate</b>
      </td>
      <td width="70px" nowrap="nowrap">
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
      <td width="75%" colspan="3">
        <b id=":i18n:Name">Name</b>
      </td>
      <td width="5%">
        <b id=":i18n:Tests">Tests</b>
      </td>
      <td width="5%">
        <b id=":i18n:Errors">Errors</b>
      </td>
      <td width="5%">
        <b id=":i18n:Failures">Failures</b>
      </td>
      <td width="5%">
        <b id=":i18n:Ignored">Ignored</b>
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
    <tr class="TableHeader" valign="top">
      <td width="85%" colspan="2">
        <b id=":i18n:Name">Name</b>
      </td>
      <td width="10%">
        <b id=":i18n:Status">Status</b>
      </td>
      <td width="5%" nowrap="nowrap">
        <b id=":i18n:Time">Time(s)</b>
      </td>
    </tr>
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
    <h2 id=":i18n:Summary">Summary</h2>
    <xsl:variable name="runCount" select="@total"/>
    <xsl:variable name="failureCount" select="@failures"/>
    <xsl:variable name="errorCount" select="@errors"/>
    <xsl:variable name="ignoreCount" select="@not-run"/>
    <xsl:variable name="total" select="$runCount + $errorCount + $failureCount"/>

    <xsl:variable name="timeCount" select="translate(test-suite/@time,',','.')"/>

    <xsl:variable name="successRate" select="$runCount div $total"/>
    <table border="0" cellpadding="2" cellspacing="0" width="95%" style="border: #dcdcdc 1px solid;">
      <xsl:call-template name="summaryHeader"/>
      <tr valign="top">
        <xsl:attribute name="class">
          <xsl:choose>
            <xsl:when test="$failureCount &gt; 0">Failure</xsl:when>
            <xsl:when test="$errorCount &gt; 0">Error</xsl:when>
            <xsl:when test="$ignoreCount &gt; 0">Ignored</xsl:when>
            <xsl:otherwise>Pass</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <td>
          <xsl:value-of select="$runCount"/>
        </td>
        <td>
          <xsl:value-of select="$failureCount"/>
        </td>
        <td>
          <xsl:value-of select="$errorCount"/>
        </td>
        <td>
          <xsl:value-of select="$ignoreCount"/>
        </td>
        <td nowrap="nowrap" width="70px">
          <xsl:call-template name="display-percent">
            <xsl:with-param name="value" select="$successRate"/>
          </xsl:call-template>
        </td>
        <td>
          <xsl:if test="round($runCount * 200 div $total )!=0">
            <span class="covered">
              <xsl:attribute name="style">
                width:<xsl:value-of select="round($runCount * 200 div $total )"/>px
              </xsl:attribute>
              <xsl:attribute name="title">
                <xsl:text>Success</xsl:text>
              </xsl:attribute>
              <xsl:call-template name="for.loop">
                <xsl:with-param name="i">1</xsl:with-param>
                <xsl:with-param name="count">
                  <xsl:value-of select="round($runCount * 10 div $total )"/>
                </xsl:with-param>
              </xsl:call-template>
              <xsl:text>�</xsl:text>
            </span>
          </xsl:if>
          <xsl:if test="round($errorCount * 200 div $total )!=0">
            <span class="ignored">
              <xsl:attribute name="style">
                width:<xsl:value-of select="round($errorCount * 200 div $total )"/>px
              </xsl:attribute>
              <xsl:attribute name="title">
                <xsl:text>Errors</xsl:text>
              </xsl:attribute>
              <xsl:text>�</xsl:text>
            </span>
          </xsl:if>
          <xsl:if test="round($failureCount * 200 div $total )!=0">
            <span class="uncovered">
              <xsl:attribute name="style">
                width:<xsl:value-of select="round($failureCount * 200 div $total )"/>px
              </xsl:attribute>
              <xsl:attribute name="title">
                <xsl:text>Failures</xsl:text>
              </xsl:attribute>
              <xsl:call-template name="for.loop">
                <xsl:with-param name="i">1</xsl:with-param>
                <xsl:with-param name="count">
                  <xsl:value-of select="round($runCount * 10 div $total )"/>
                </xsl:with-param>
              </xsl:call-template>
              <xsl:text>�</xsl:text>
            </span>
          </xsl:if>
        </td>
        <td>
          <xsl:call-template name="display-time">
            <xsl:with-param name="value" select="$timeCount"/>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <span id=":i18n:Note">Note</span>: <i id=":i18n:failures">failures </i> <span id=":i18n:anticipated">are anticipated and checked for with assertions while </span> <i id=":i18n:errors">errors </i> <span id=":i18n:unanticipated">are unanticipated.</span>
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
    <tr valign="top">
      <xsl:attribute name="class">
        <xsl:value-of select="$result"/>
      </xsl:attribute>

      <td width="20%" >
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
              <!-- Set the good triangle image 6/4 font-family:Webdings-->
              <xsl:choose>
                <xsl:when test="$result != &quot;Pass&quot;">-</xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$open.description='yes'">-</xsl:when>
                    <xsl:otherwise>+</xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
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
            <xsl:attribute name="class">method</xsl:attribute>
            <xsl:value-of select="nunit2report:TestCaseName(./@name)"/>
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td width="65%" style="padding-left:3px" height="9px">
        <xsl:choose>
          <xsl:when test="$result = 'Pass'">
            <span class="covered" style="width:200px">
              <xsl:call-template name="for.loop">
                <xsl:with-param name="i">1</xsl:with-param>
                <xsl:with-param name="count">
                  10
                </xsl:with-param>
              </xsl:call-template>
            </span>
          </xsl:when>
          <xsl:when test="$result = 'Ignored'">
            <span class="ignored" style="width:200px"></span>
          </xsl:when>
          <xsl:when test="$result = 'Failure' or $result = 'Error'">
            <span class="uncovered" style="width:200px"></span>
          </xsl:when>
        </xsl:choose>
        <!-- The test method description-->
        <xsl:choose>
          <xsl:when test="$summary.xml != ''">
            <div class="description" style="display:block">
              <!-- Attribute id -->
              <xsl:attribute name="id">
                <xsl:value-of select="concat('M:',$newid)"/>
              </xsl:attribute>
              <!-- Open method description if failure -->
              <xsl:choose>
                <xsl:when test="$result != &quot;Pass&quot;">
                  <xsl:attribute name="style">display:block</xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$open.description = 'yes'">
                      <xsl:attribute name="style">display:block</xsl:attribute>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:attribute name="style">display:none</xsl:attribute>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
              <!-- The description of the test method -->
              <xsl:value-of select="normalize-space($summaries//member[@name=$Mname]/summary/text())"/>
            </div>
          </xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:attribute name="id">
          :i18n:<xsl:value-of select="$result"/>
        </xsl:attribute>
        <xsl:value-of select="$result"/>
      </td>
      <td>
        <xsl:call-template name="display-time">
          <xsl:with-param name="value" select="@time"/>
        </xsl:call-template>
      </td>
    </tr>

    <xsl:if test="$result != &quot;Pass&quot;">
      <tr style="display: block;">
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
    <a name="envinfo"></a>
    <h2 id=":i18n:EnvironmentInformation">Environment Information</h2>
    <table border="0" cellpadding="5" cellspacing="2" width="95%">
      <tr class="TableHeader">
        <td id=":i18n:Property">Property</td>
        <td id=":i18n:Value">Value</td>
      </tr>
      <tr>
        <td id=":i18n:NETCLRVersion">Machine name</td>
        <td>
          <xsl:value-of select="$sys.machine.name"/>
        </td>
      </tr>
      <tr>
        <td id=":i18n:NETCLRVersion">User</td>
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
    <a href="#top" id=":i18n:Backtotop">Back to top</a>
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