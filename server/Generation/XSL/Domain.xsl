<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">

<xsl:for-each select="items/enum">
  <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.SDK.Shared\Models\<xsl:value-of select="@name"/>.cs]using System;
using System.Text;

namespace <xsl:value-of select="../@projectName"/>.SDK.Models
{
    <xsl:if test="@flag='true'">[Flags]
    </xsl:if>public enum <xsl:value-of select="@name"/>
    {
        <xsl:for-each select="field"><xsl:if test="position() > 1">,
        </xsl:if><xsl:if test="string-length(@obsolete)> 0">[Obsolete("<xsl:value-of select="@obsolete"/>", false)]
        </xsl:if><xsl:value-of select="text()"/> = <xsl:value-of select="@value"/></xsl:for-each>
    }
}
'''[ENDFILE]
</xsl:for-each>

<xsl:for-each select="items/enum">
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Domain\Domain\<xsl:value-of select="@name"/>.cs]using System;
using System.Text;

namespace <xsl:value-of select="../@projectName"/>.Domain
{
    <xsl:if test="@flag='true'">[Flags]
    </xsl:if>public enum <xsl:value-of select="@name"/>
    {
        <xsl:for-each select="field"><xsl:if test="position() > 1">,
        </xsl:if><xsl:if test="string-length(@obsolete)> 0">[Obsolete("<xsl:value-of select="@obsolete"/>", false)]
        </xsl:if><xsl:value-of select="text()"/> = <xsl:value-of select="@value"/></xsl:for-each>
    }
}
'''[ENDFILE]
</xsl:for-each>
  
<xsl:for-each select="items/item">
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Domain\Domain\<xsl:value-of select="@name"/>.cs]using System;
using System.Collections.Generic;
using System.Text;


namespace <xsl:value-of select="../@projectName"/>.Domain
{
    public partial class <xsl:value-of select="@name"/> : DomainModel
    {	
        public <xsl:value-of select="@name"/>()
        {
				
        }
    
        <xsl:for-each select="field"><xsl:if test="string-length(@obsolete)> 0">[Obsolete("<xsl:value-of select="@obsolete"/>", false)]
        </xsl:if>public <xsl:value-of select="@type"/><xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        </xsl:for-each><xsl:if test="@useIndex='true' or @useStore='true'">public DateTime created_utc { get; set; }
        public DateTime updated_utc { get; set; }
        public DateTime? deleted_utc { get; set; }
        public DateTime? sync_success_utc { get; set; }
        public DateTime? sync_invalid_utc { get; set; }
        public DateTime? sync_attempt_utc { get; set; }
        public string sync_agent { get; set; }
        public string sync_log { get; set; }</xsl:if><xsl:if test="@trackUpdates='true'">public DateTime created_utc { get; set; }
        public DateTime updated_utc { get; set; }</xsl:if>
        
        <xsl:for-each select="field[string-length(@derivedProperty)>0]">
        public DerivedField&lt;<xsl:value-of select="@foreignKey"/>&gt; Related<xsl:value-of select="@friendlyName"/> { get; set; }
        </xsl:for-each>
        <xsl:variable name="currentKey"><xsl:value-of select="@name"/></xsl:variable>
        <xsl:for-each select="../item/field[string-length(@derivedParentProperty)>0 and @foreignKey=$currentKey]">
        public DerivedField&lt;List&lt;<xsl:value-of select="../@name"/>&gt;&gt; Related<xsl:value-of select="../@name"/> { get; set; }
        </xsl:for-each>
	}
}

'''[ENDFILE]



'''[STARTFILE:<xsl:value-of select="../@projectName"/>.SDK.Shared\Models\<xsl:value-of select="@name"/>.cs]using System;
using System.Collections.Generic;
using System.Text;<xsl:if test="@hasMarkdown='true'">
using Stencil.Common.Markdown;</xsl:if>

namespace <xsl:value-of select="../@projectName"/>.SDK.Models
{
    public partial class <xsl:value-of select="@name"/> : <xsl:choose><xsl:when test="@sdkBase='true'"><xsl:value-of select="@name"/>Base</xsl:when><xsl:otherwise>SDKModel</xsl:otherwise></xsl:choose>
    {	
        public <xsl:value-of select="@name"/>()
        {
				
        }

        <xsl:if test="count(field[@storePartitionKey='Global'])>0">
        public static string GLOBAL_PARTITION = "<xsl:value-of select="@name"/>";
        </xsl:if>
    
        <xsl:for-each select="field[not(@sdkHidden='true') and not(@derivedProperty='true') and not(@type='DateTimeOffset')]"><xsl:if test="string-length(@obsolete)> 0">[Obsolete("<xsl:value-of select="@obsolete"/>", false)]
        </xsl:if>public virtual <xsl:value-of select="@type"/><xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        </xsl:for-each>
        
        <xsl:for-each select="field[not(@sdkHidden='true') and not(@derivedProperty='true') and @type='DateTimeOffset']">
        #if WEB
        public virtual DateTime<xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        #else
        public virtual DateTimeOffset<xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        #endif
        </xsl:for-each>

        <xsl:if test="@storeBulk='true'">

        /// &lt;summary&gt;
        /// Index Only
        /// &lt;/summary&gt;
        public string transaction_id { get; set; }

        /// &lt;summary&gt;
        /// Index Only
        /// &lt;/summary&gt;
        public DateTime transaction_stamp_utc { get; set; }
        
        </xsl:if>

        <xsl:if test="count(field[@storePartitionKey='Global'])>0">
         /// &lt;summary&gt;
        /// Index Only
        /// &lt;/summary&gt;
        public string partition_key { get { return GLOBAL_PARTITION; } }
        </xsl:if>

        <xsl:if test="count(field[@storePartitionKey='SplitID'])>0">
         /// &lt;summary&gt;
        /// Index Only
        /// &lt;/summary&gt;
        public string partition_key { get { return this.<xsl:value-of select="field[@storePartitionKey='SplitID'][1]/text()"/>.ToString().Substring(0, 5); } }
        </xsl:if>

        <xsl:if test="count(field[@storePartitionKey='Self'])>0">
         /// &lt;summary&gt;
        /// Index Only
        /// &lt;/summary&gt;
        public string partition_key { get { return this.<xsl:value-of select="field[@storePartitionKey='Self'][1]/text()"/>.ToString(); } }
        </xsl:if>
        
        <xsl:for-each select="indexfield">
        /// &lt;summary&gt;
        /// Index Only
        /// &lt;/summary&gt;
        public <xsl:value-of select="@type"/><xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        </xsl:for-each>
	}
}

'''[ENDFILE]
<xsl:if test="count(field[@slim='true'])>0 or count(indexfield[@slim='true'])>0">
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.SDK.Shared\Models\<xsl:value-of select="@name"/>_slim.cs]using System;
using System.Collections.Generic;
using System.Text;

namespace <xsl:value-of select="../@projectName"/>.SDK.Models
{
    public partial class <xsl:value-of select="@name"/>Slim
    {	
        public <xsl:value-of select="@name"/>Slim()
        {
				
        }

        <xsl:for-each select="field[@slim='true' and not(@sdkHidden='true') and not(@derivedProperty='true') and not(@type='DateTimeOffset')]"><xsl:if test="string-length(@obsolete)> 0">[Obsolete("<xsl:value-of select="@obsolete"/>", false)]
        </xsl:if>public virtual <xsl:value-of select="@type"/><xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        </xsl:for-each>
        
        <xsl:for-each select="field[@slim='true' and not(@sdkHidden='true') and not(@derivedProperty='true') and @type='DateTimeOffset']">
        #if WEB
        public virtual DateTime<xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        #else
        public virtual DateTimeOffset<xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        #endif
        </xsl:for-each>
    
        <xsl:if test="count(indexfield[string-length(@slim)>0])>0">
        
        //&lt;IndexOnly&gt;
        <xsl:for-each select="indexfield[string-length(@slim)>0]">
        public <xsl:choose><xsl:when test="not(@slim='true')"><xsl:value-of select="@slim"/></xsl:when><xsl:otherwise><xsl:value-of select="@type"/></xsl:otherwise></xsl:choose><xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        </xsl:for-each>
        //&lt;/IndexOnly&gt;</xsl:if>
	}
}

'''[ENDFILE]
</xsl:if>

<xsl:if test="string-length(@graphNode)>0">
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.SDK.Shared\Models\Graph\<xsl:value-of select="@name"/>Node.cs]using System;
using System.Collections.Generic;
using System.Text;

namespace <xsl:value-of select="../@projectName"/>.SDK.Models
{
    public partial class <xsl:value-of select="@name"/>Node : SDKModel
    {	
        public <xsl:value-of select="@name"/>Node()
        {
				
        }
    
        <xsl:for-each select="field[@graphProperty='true' and not(string-length(@derivedProperty)>0) and not(@type='DateTimeOffset')]">public virtual <xsl:value-of select="@type"/><xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        </xsl:for-each>
        <xsl:for-each select="field[@graphProperty='true' and @derivedProperty='avatar']">public virtual AssetInfo avatar { get; set; }
        </xsl:for-each>
        <xsl:for-each select="indexfield[@graphProperty='true' and not(@type='DateTimeOffset')]">public virtual <xsl:value-of select="@type"/><xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        </xsl:for-each>
        
        <xsl:for-each select="field[@graphProperty='true' and @type='DateTimeOffset']">
        #if WEB
        public virtual DateTime<xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        #else
        public virtual DateTimeOffset<xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        #endif
        </xsl:for-each>
        <xsl:for-each select="indexfield[@graphProperty='true' and @type='DateTimeOffset']">
        #if WEB
        public virtual DateTime<xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        #else
        public virtual DateTimeOffset<xsl:if test="@type!='string' and @isNullable='true'">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> { get; set; }
        #endif
        </xsl:for-each>
	}
}

'''[ENDFILE]
</xsl:if>
</xsl:for-each>

</xsl:template>

<xsl:template name="ToLower">
          <xsl:param name="inputString"/>
          <xsl:variable name="smallCase" select="'abcdefghijklmnopqrstuvwxyz'"/>
          <xsl:variable name="upperCase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
          <xsl:value-of select="translate($inputString,$upperCase,$smallCase)"/>
  </xsl:template>

</xsl:stylesheet>