<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">



'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Foundation\<xsl:value-of select="items/@projectName"/>BootStrap_Graph.cs]using <xsl:value-of select="items/@foundation"/>.Foundation.Common;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Graph;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Graph.Implementation;
using <xsl:value-of select="items/@foundation"/>.Foundation.UI.Web.Core.Unity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Foundation
{
    public partial class <xsl:value-of select="items/@projectName"/>BootStrap
    {
        protected virtual void RegisterGraphElements(IFoundation foundation)
        {
            // Graphs
            <xsl:for-each select="items/item[string-length(@graphNode)>0 or string-length(@graphRelation)>0]">foundation.Container.RegisterType&lt;I<xsl:value-of select="@name" />Graph, <xsl:value-of select="@name" />Graph&gt;(TypeLifetime.Scoped);
            </xsl:for-each>
        }
    }
}

'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\<xsl:value-of select="items/@projectName"/>APIGraph.cs]using <xsl:value-of select="../@foundation"/>.Foundation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Graph;

namespace <xsl:value-of select="items/@projectName"/>.Primary
{
    public class <xsl:value-of select="items/@projectName"/>APIGraph : BaseClass
    {
        public <xsl:value-of select="items/@projectName"/>APIGraph(IFoundation ifoundation)
            : base(ifoundation)
        {
        }
        <xsl:for-each select="items/item[string-length(@graphNode)>0 or string-length(@graphRelation)>0]">public I<xsl:value-of select="@name" />Graph <xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
        {
            get { return this.IFoundation.Resolve&lt;I<xsl:value-of select="@name" />Graph&gt;(); }
        }
        </xsl:for-each>
    }
}


'''[ENDFILE]



'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Mapping\_GraphExtensions_Core.cs]using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;
using <xsl:value-of select="items/@projectName"/>.SDK.Models;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Business.Graph
{
    public static partial class _GraphExtensions_Core
    {
        <xsl:for-each select="items/item[string-length(@graphNode)>0]">
        public static <xsl:value-of select="@name"/>Node As<xsl:value-of select="@name"/>Node(this IRecord record, string key)
        {
            if (record != null)
            {
                INode node = record[key] as INode;
                if (node != null)
                {
                    <xsl:value-of select="@name"/>Node result = new <xsl:value-of select="@name"/>Node();
                    <xsl:for-each select="field[@graphProperty='true']">
                    <xsl:choose>
                    <xsl:when test="@derivedProperty='avatar'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        string <xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].As&lt;string&gt;();
                        if(!string.IsNullOrEmpty(<xsl:value-of select="text()"/>))
                        {
                            result.<xsl:value-of select="@derivedProperty"/> = new AssetInfo(<xsl:value-of select="text()"/>);
                        }
                    }</xsl:when>
                    <xsl:when test="@isEnum='true'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = (<xsl:value-of select="@type"/>)Enum.Parse(typeof(<xsl:value-of select="@type"/>), node["<xsl:value-of select="text()"/>"].As&lt;string&gt;(), true);
                    }</xsl:when>
                    <xsl:when test="@type='Guid'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].AsGuid().GetValueOrDefault();
                    }</xsl:when>
                    <xsl:when test="@type='DateTime'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].As&lt;ZonedDateTime&gt;().ToDateTimeOffset().DateTime;
                    }</xsl:when>
                    <xsl:when test="@type='int'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].As&lt;int&gt;();
                    }</xsl:when>
                    <xsl:otherwise>
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].As&lt;string&gt;();
                    }</xsl:otherwise>
                    </xsl:choose>
                    </xsl:for-each>
                    <xsl:for-each select="indexfield[@graphProperty='true']">
                    <xsl:choose>
                    <xsl:when test="@derivedProperty='avatar'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        string <xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].As&lt;string&gt;();
                        if(!string.IsNullOrEmpty(<xsl:value-of select="text()"/>))
                        {
                            result.<xsl:value-of select="@derivedProperty"/> = new AssetInfo(<xsl:value-of select="text()"/>);
                        }
                    }</xsl:when>
                    <xsl:when test="@isEnum='true'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = (<xsl:value-of select="@type"/>)Enum.Parse(typeof(<xsl:value-of select="@type"/>), node["<xsl:value-of select="text()"/>"].As&lt;string&gt;(), true);
                    }</xsl:when>
                    <xsl:when test="@type='Guid'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].AsGuid().GetValueOrDefault();
                    }</xsl:when>
                    <xsl:when test="@type='DateTime'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].As&lt;ZonedDateTime&gt;().ToDateTimeOffset().DateTime;
                    }</xsl:when>
                    <xsl:when test="@type='int'">
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].As&lt;int&gt;();
                    }</xsl:when>
                    <xsl:otherwise>
                    if (node.Properties.ContainsKey("<xsl:value-of select="text()"/>"))
                    {
                        result.<xsl:value-of select="text()"/> = node["<xsl:value-of select="text()"/>"].As&lt;string&gt;();
                    }</xsl:otherwise>
                    </xsl:choose>
                    </xsl:for-each>
                    return result;
                }
            }
            return null;
        }
        </xsl:for-each>
    }
}

'''[ENDFILE]       

<xsl:for-each select="items/item">
  <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>

<xsl:if test="string-length(@graphNode)>0 or string-length(@graphRelation)>0">

'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Graph\I<xsl:value-of select="@name"/>Graph_Crud.cs]using System;
using System.Collections.Generic;
using System.Text;
using <xsl:value-of select="../@projectName"/>.SDK.Models;
using <xsl:value-of select="../@projectName"/>.Data.Graph;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Graph
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface I<xsl:value-of select="@name"/>Graph
    {
        <xsl:if test="string-length(@graphNode)>0">
        bool UpsertNode(<xsl:value-of select="@name"/> model);
        bool DeleteNode(Guid <xsl:value-of select="field[1]/text()" />);
        </xsl:if>
        <xsl:if test="string-length(@graphRelation)>0">
        bool RelationExists(<xsl:value-of select="field[@graphRelationSource='true']/@type" /><xsl:text> </xsl:text><xsl:value-of select="field[@graphRelationSource='true']/text()" />, <xsl:value-of select="field[@graphRelationTarget='true']/@type" /><xsl:text> </xsl:text><xsl:value-of select="field[@graphRelationTarget='true']/text()" />);
        bool UpsertRelation(<xsl:value-of select="@name"/> model);
        bool DeleteRelation(<xsl:value-of select="@name" /> model);
        <xsl:for-each select="field[@graphRelationSource='true']">
        List&lt;<xsl:value-of select="../field[@graphRelationTarget='true']/@foreignKey" />Node&gt; GetRelationBy<xsl:value-of select="@friendlyName"/>(<xsl:value-of select="@type" /><xsl:text> </xsl:text><xsl:value-of select="text()" />, int skip = 0, int take = 20, string keyword = "", string order_by = "", <xsl:value-of select="../field[@graphRelationTarget='true']/@type" />[]<xsl:text> </xsl:text><xsl:value-of select="../field[@graphRelationTarget='true']/text()" /> = null);
        </xsl:for-each>
        </xsl:if>
    }
}

'''[ENDFILE]


'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Graph\Implementation\<xsl:value-of select="@name"/>Graph_Crud.cs]using <xsl:value-of select="../@foundation"/>.Foundation.Common;
using <xsl:value-of select="../@foundation"/>.Foundation.Common.Aspect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using <xsl:value-of select="../@projectName"/>.SDK.Models;
using <xsl:value-of select="../@projectName"/>.Data.Graph;
using <xsl:value-of select="../@projectName"/>.Primary.Mapping;
using Neo4j.Driver.V1;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Graph.Implementation
{
    // WARNING: THIS FILE IS GENERATED
    public partial class <xsl:value-of select="@name"/>Graph : GraphBase, I<xsl:value-of select="@name"/>Graph
    {
        public <xsl:value-of select="@name"/>Graph(IFoundation foundation)
            : base(foundation, "<xsl:value-of select="@name"/>")
        {
        }
        <xsl:if test="string-length(@graphNode)>0">
        public bool UpsertNode(<xsl:value-of select="@name" /> model)
        {
            return base.ExecuteFunction("UpsertNode", delegate ()
            {
                try
                {
                    using (ISession session = this.CreateSession())
                    {
                        if(session == null || session is EmptySession) //TODO:Properly Handle No Graph Installations
                        {
                            return true;
                        }
                        Dictionary&lt;string, object&gt; statementParameters = new Dictionary&lt;string, object&gt;
                        {
                           { "<xsl:value-of select="field[1]/text()" />", model.<xsl:value-of select="field[1]/text()" />.ToString().ToLower() },
                           <xsl:for-each select="field[@graphProperty='true' and text() != ../field[1]/text()]">
                            <xsl:choose>
                                <xsl:when test="string-length(@derivedProperty)>0">{ "<xsl:value-of select="@derivedProperty"/>", model.<xsl:value-of select="@derivedProperty" />?.ToString() }</xsl:when>
                                <xsl:when test="@isEnum='true'">{ "<xsl:value-of select="text()"/>", <xsl:if test="@isEnum='true'">(long)</xsl:if>model.<xsl:value-of select="text()"/> }</xsl:when>
                                <xsl:otherwise>{ "<xsl:value-of select="text()"/>", model.<xsl:value-of select="text()"/><xsl:if test="@type='Guid'">.ToString().ToLower()</xsl:if> }</xsl:otherwise>
                                </xsl:choose>,
                           </xsl:for-each><xsl:for-each select="indexfield[@graphProperty='true']">
                            <xsl:choose>
                                <xsl:when test="string-length(@derivedProperty)>0">{ "<xsl:value-of select="@derivedProperty"/>", model.<xsl:value-of select="@derivedProperty" />?.ToString() }</xsl:when>
                                <xsl:when test="@isEnum='true'">{ "<xsl:value-of select="text()"/>", <xsl:if test="@isEnum='true'">(long)</xsl:if>model.<xsl:value-of select="text()"/> }</xsl:when>
                                <xsl:otherwise>{ "<xsl:value-of select="text()"/>", model.<xsl:value-of select="text()"/><xsl:if test="@type='Guid'">.ToString().ToLower()</xsl:if> }</xsl:otherwise>
                                </xsl:choose>,
                           </xsl:for-each>{ "updated_utc", DateTime.UtcNow },
                        };

                        string statement = @"
                            MERGE (entity:<xsl:value-of select="@name" /> {<xsl:value-of select="field[1]/text()" />:{<xsl:value-of select="field[1]/text()" />}})
                            SET entity += { updated_utc: {updated_utc}<xsl:for-each select="field[@graphProperty='true']">, <xsl:choose>
                                <xsl:when test="string-length(@derivedProperty)>0"><xsl:value-of select="@derivedProperty"/>: {<xsl:value-of select="@derivedProperty"/>}</xsl:when>
                                <xsl:otherwise><xsl:value-of select="text()"/>: {<xsl:value-of select="text()"/>}</xsl:otherwise>
                                </xsl:choose>
                           </xsl:for-each><xsl:for-each select="indexfield[@graphProperty='true']">, <xsl:choose>
                                <xsl:when test="string-length(@derivedProperty)>0"><xsl:value-of select="@derivedProperty"/>: {<xsl:value-of select="@derivedProperty"/>}</xsl:when>
                                <xsl:otherwise><xsl:value-of select="text()"/>: {<xsl:value-of select="text()"/>}</xsl:otherwise>
                                </xsl:choose>
                           </xsl:for-each> }
                            RETURN entity.<xsl:value-of select="field[1]/text()" /> as _id";

                        IStatementResult result = session.Run(statement, statementParameters);
                        return (result.Select(record => record["_id"].AsGuid()).FirstOrDefault() != null);
                    }
                }
                catch (Exception ex)
                {
                    this.IFoundation.LogError(ex, "UpsertNode");
                    return false;
                }
            });
        }

        public bool DeleteNode(Guid <xsl:value-of select="field[1]/text()" />)
        {
            return base.ExecuteFunction("DeleteNode", delegate ()
            {
                try
                {
                    using (ISession session = this.CreateSession())
                    {
                        if(session == null || session is EmptySession) //TODO:Properly Handle No Graph Installations
                        {
                            return true;
                        }
                        Dictionary&lt;string, object&gt; statementParameters = new Dictionary&lt;string, object&gt;
                        {
                           { "<xsl:value-of select="field[1]/text()" />", <xsl:value-of select="field[1]/text()" />.ToString().ToLower() }
                        };

                        string statement = @"
                            MATCH (entity:<xsl:value-of select="@name" /> {<xsl:value-of select="field[1]/text()" />:{<xsl:value-of select="field[1]/text()" />}})
                            DELETE entity";

                        IStatementResult result = session.Run(statement, statementParameters);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    this.IFoundation.LogError(ex, "DeleteNode");
                    return false;
                }
            });
        }
        </xsl:if>

        <xsl:if test="string-length(@graphRelation)>0">
        public bool RelationExists(<xsl:value-of select="field[@graphRelationSource='true']/@type" /><xsl:text> </xsl:text><xsl:value-of select="field[@graphRelationSource='true']/text()" />, <xsl:value-of select="field[@graphRelationTarget='true']/@type" /><xsl:text> </xsl:text><xsl:value-of select="field[@graphRelationTarget='true']/text()" />)
        {
            return base.ExecuteFunction("RelationExists", delegate ()
            {
                try
                {
                    using (ISession session = this.CreateSession())
                    {
                        if(session == null || session is EmptySession) //TODO:Properly Handle No Graph Installations
                        {
                            return false;
                        }
                        Dictionary&lt;string, object&gt; statementParameters = new Dictionary&lt;string, object&gt;
                        {
                           { "<xsl:value-of select="field[@graphRelationSource='true']/text()" />", <xsl:value-of select="field[@graphRelationSource='true']/text()" />.ToString().ToLower() },
                           { "<xsl:value-of select="field[@graphRelationTarget='true']/text()" />", <xsl:value-of select="field[@graphRelationTarget='true']/text()" />.ToString().ToLower() }
                        };

                        string statement = @"
                            MATCH (left:<xsl:value-of select="field[@graphRelationSource='true']/@foreignKey" /> {<xsl:value-of select="field[@graphRelationSource='true']/@foreignKeyField" />: {<xsl:value-of select="field[@graphRelationSource='true']/text()" />}})-[r:<xsl:value-of select="@graphRelation" />]-(right:<xsl:value-of select="field[@graphRelationTarget='true']/@foreignKey" /> {<xsl:value-of select="field[@graphRelationTarget='true']/@foreignKeyField" />: {<xsl:value-of select="field[@graphRelationTarget='true']/text()" />}})
                            RETURN r";
                        
                        IStatementResult result = session.Run(statement, statementParameters);
                        return (result.Select(record => record["r"].As&lt;string&gt;()).FirstOrDefault() != null);
                    }
                }
                catch (Exception ex)
                {
                    this.IFoundation.LogError(ex, "RelationExists");
                    return false;
                }
            });
        }
        public bool UpsertRelation(<xsl:value-of select="@name" /> model)
        {
            return base.ExecuteFunction("UpsertRelation", delegate ()
            {
                try
                {
                    using (ISession session = this.CreateSession())
                    {
                        if(session == null || session is EmptySession) //TODO:Properly Handle No Graph Installations
                        {
                            return true;
                        }
                        Dictionary&lt;string, object&gt; statementParameters = new Dictionary&lt;string, object&gt;
                        {
                           { "<xsl:value-of select="field[1]/text()" />", model.<xsl:value-of select="field[1]/text()" />.ToString().ToLower() },
                           { "<xsl:value-of select="field[@graphRelationSource='true']/text()" />", model.<xsl:value-of select="field[@graphRelationSource='true']/text()" />.ToString().ToLower() },
                           { "<xsl:value-of select="field[@graphRelationTarget='true']/text()" />", model.<xsl:value-of select="field[@graphRelationTarget='true']/text()" />.ToString().ToLower() },
                           <xsl:for-each select="field[@graphProperty='true']">
                            <xsl:choose>
                                <xsl:when test="string-length(@derivedProperty)>0">{ "<xsl:value-of select="@derivedProperty"/>", model.<xsl:value-of select="@derivedProperty" />?.ToString() }</xsl:when>
                                <xsl:when test="@isEnum='true'">{ "<xsl:value-of select="text()"/>", <xsl:if test="@isEnum='true'">(long)</xsl:if>model.<xsl:value-of select="text()"/> }</xsl:when>
                                <xsl:otherwise>{ "<xsl:value-of select="text()"/>", model.<xsl:value-of select="text()"/><xsl:if test="@type='Guid'">.ToString().ToLower()</xsl:if> }</xsl:otherwise>
                                </xsl:choose>,
                           </xsl:for-each>
                        };

                        string statement = @"
                            MERGE (left:<xsl:value-of select="field[@graphRelationSource='true']/@foreignKey" /> {<xsl:value-of select="field[@graphRelationSource='true']/@foreignKeyField" />: {<xsl:value-of select="field[@graphRelationSource='true']/text()" />}})
                            MERGE (right:<xsl:value-of select="field[@graphRelationTarget='true']/@foreignKey" /> {<xsl:value-of select="field[@graphRelationTarget='true']/@foreignKeyField" />: {<xsl:value-of select="field[@graphRelationTarget='true']/text()" />}})
                            MERGE (left)-[r:<xsl:value-of select="@graphRelation" />]->(right)
                            SET r += { <xsl:value-of select="field[1]/text()" />: {<xsl:value-of select="field[1]/text()" />}<xsl:for-each select="field[@graphProperty='true']">
                            ,<xsl:choose>
                                <xsl:when test="string-length(@derivedProperty)>0"><xsl:value-of select="@derivedProperty"/>: {<xsl:value-of select="@derivedProperty"/>}</xsl:when>
                                <xsl:otherwise><xsl:value-of select="text()"/>: {<xsl:value-of select="text()"/>}</xsl:otherwise>
                                </xsl:choose>
                           </xsl:for-each> }
                            RETURN r";
                        
                        IStatementResult result = session.Run(statement, statementParameters);
                        return (result.Select(record => record["r"].As&lt;string&gt;()).FirstOrDefault() != null);
                    }
                }
                catch (Exception ex)
                {
                    this.IFoundation.LogError(ex, "UpsertRelation");
                    return false;
                }
            });
        }

        public bool DeleteRelation(<xsl:value-of select="@name" /> model)
        {
            return base.ExecuteFunction("DeleteRelation", delegate ()
            {
                try
                {
                    using (ISession session = this.CreateSession())
                    {
                        if(session == null || session is EmptySession) //TODO:Properly Handle No Graph Installations
                        {
                            return true;
                        }
                        Dictionary&lt;string, object&gt; statementParameters = new Dictionary&lt;string, object&gt;
                        {
                           { "<xsl:value-of select="field[1]/text()" />", model.<xsl:value-of select="field[1]/text()" />.ToString().ToLower() },
                           { "<xsl:value-of select="field[@graphRelationSource='true']/text()" />", model.<xsl:value-of select="field[@graphRelationSource='true']/text()" />.ToString().ToLower() },
                           { "<xsl:value-of select="field[@graphRelationTarget='true']/text()" />", model.<xsl:value-of select="field[@graphRelationTarget='true']/text()" />.ToString().ToLower() },
                        };

                        string statement = @"
                            MATCH (left:<xsl:value-of select="field[@graphRelationSource='true']/@foreignKey" /> {<xsl:value-of select="field[@graphRelationSource='true']/@foreignKeyField" />: {<xsl:value-of select="field[@graphRelationSource='true']/text()" />}})-[r:<xsl:value-of select="@graphRelation" /> {<xsl:value-of select="field[1]/text()" />: {<xsl:value-of select="field[1]/text()" />}}]-(right:<xsl:value-of select="field[@graphRelationTarget='true']/@foreignKey" /> {<xsl:value-of select="field[@graphRelationTarget='true']/@foreignKeyField" />: {<xsl:value-of select="field[@graphRelationTarget='true']/text()" />}})
                            DELETE r";

                        IStatementResult result = session.Run(statement, statementParameters);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    this.IFoundation.LogError(ex, "DeleteRelation");
                    return false;
                }
            });
        }
        <xsl:for-each select="field[@graphRelationSource='true']">
        <xsl:variable name="targetEntity"><xsl:value-of select="../field[@graphRelationTarget='true']/@foreignKey"/></xsl:variable>
        public List&lt;<xsl:value-of select="../field[@graphRelationTarget='true']/@foreignKey" />Node&gt; GetRelationBy<xsl:value-of select="@friendlyName"/>(<xsl:value-of select="@type" /><xsl:text> </xsl:text><xsl:value-of select="text()" />, int skip = 0, int take = 20, string keyword = "", string order_by = "", <xsl:value-of select="../field[@graphRelationTarget='true']/@type" />[]<xsl:text> </xsl:text><xsl:value-of select="../field[@graphRelationTarget='true']/text()" /> = null)
        {
            return base.ExecuteFunction("GetRelationBy<xsl:value-of select="@friendlyName"/>", delegate ()
            {
                try
                {
                    keyword = PrimaryUtility.CleanSearch(keyword);
                    order_by = PrimaryUtility.CleanField(order_by);
                    <xsl:if test="count(../../item[@name=$targetEntity]/field[@graphSearchable='true'])>0 or count(../../item[@name=$targetEntity]/indexfield[@graphSearchable='true'])>0">
                    // only allowed fields
                    switch (order_by.ToLower())
                    {
                        <xsl:for-each select="../../item[@name=$targetEntity]/field[@graphSearchable='true']">case "<xsl:value-of select="text()"/>":
                        </xsl:for-each><xsl:for-each select="../../item[@name=$targetEntity]/indexfield[@graphSearchable='true']">case "<xsl:value-of select="text()"/>":
                        </xsl:for-each>
                            // leave it
                            break;
                        default:
                            order_by = "<xsl:value-of select="../../item[@name=$targetEntity]/field[@graphSearchable='true']/text()"/>";
                            break;
                    }
                    </xsl:if>

                    using (ISession session = this.CreateSession())
                    {
                        if(session == null || session is EmptySession) //TODO:Properly Handle No Graph Installations
                        {
                            return new List&lt;<xsl:value-of select="../field[@graphRelationTarget='true']/@foreignKey" />Node&gt;();
                        }
                        Dictionary&lt;string, object&gt; statementParameters = new Dictionary&lt;string, object&gt;
                        {
                           { "<xsl:value-of select="text()" />", <xsl:value-of select="text()" />.ToString().ToLower() },
                           { "keyword", keyword },
                        };

                        string rightFilter = string.Empty;
                        if (<xsl:value-of select="../field[@graphRelationTarget='true']/text()" />?.Length > 0)
                        {
                            for (int i = 0; i &lt; <xsl:value-of select="../field[@graphRelationTarget='true']/text()" />.Length; i++)
                            {
                                string key = string.Format("<xsl:value-of select="../field[@graphRelationTarget='true']/text()" />_{0}", i);
                                rightFilter += string.Format("{{{0}}},", key);
                                statementParameters[key] = <xsl:value-of select="../field[@graphRelationTarget='true']/text()" />[i].ToString();
                            }
                            rightFilter = string.Format("AND right.<xsl:value-of select="../field[@graphRelationTarget='true']/@foreignKeyField" /> in [{0}]", rightFilter.Trim(','));
                        }
                        
                        <xsl:if test="count(../../item[@name=$targetEntity]/field[@graphSearchable='true'])>0 or count(../../item[@name=$targetEntity]/indexfield[@graphSearchable='true'])>0">//1=2 is for code gen ease</xsl:if>
                        string statement = @"
                            MATCH (left:<xsl:value-of select="../field[@graphRelationSource='true']/@foreignKey" /> {<xsl:value-of select="@foreignKeyField" />: {<xsl:value-of select="text()" />}})-[r:<xsl:value-of select="../@graphRelation" />]-(right:<xsl:value-of select="../field[@graphRelationTarget='true']/@foreignKey" />)
                            <xsl:if test="count(../../item[@name=$targetEntity]/field[@graphSearchable='true'])>0 or count(../../item[@name=$targetEntity]/indexfield[@graphSearchable='true'])>0">WHERE ( 1 = 2 <xsl:for-each select="../../item[@name=$targetEntity]/field[@graphSearchable='true']">
                               OR right.<xsl:value-of select="text()"/> =~ '(?i)" + keyword + @".*' </xsl:for-each><xsl:for-each select="../../item[@name=$targetEntity]/indexfield[@graphSearchable='true']">
                               OR right.<xsl:value-of select="text()"/> =~ '(?i)" + keyword + @".*' </xsl:for-each>
                            )</xsl:if>
                            " + rightFilter + @"
                            RETURN right
                            <xsl:if test="count(../../item[@name=$targetEntity]/field[@graphSearchable='true'])>0 or count(../../item[@name=$targetEntity]/indexfield[@graphSearchable='true'])>0">ORDER BY right." + order_by + @"</xsl:if>
                            SKIP " + skip + @"
                            LIMIT " + take;

                        IStatementResult result = session.Run(statement, statementParameters);
                        return result.Select(record => record.As<xsl:value-of select="../field[@graphRelationTarget='true']/@foreignKey" />Node("right")).ToList();
                    }
                }
                catch (Exception ex)
                {
                    this.IFoundation.LogError(ex, "GetRelationBy<xsl:value-of select="@friendlyName"/>");
                    return new List&lt;<xsl:value-of select="../field[@graphRelationTarget='true']/@foreignKey"/>Node&gt;();
                }
            });
        }
        </xsl:for-each>
        </xsl:if>
   }
}


'''[ENDFILE]

</xsl:if>

</xsl:for-each>


</xsl:template>

	<xsl:template match="@space"> </xsl:template>
  <xsl:template name="ToLower">
          <xsl:param name="inputString"/>
          <xsl:variable name="smallCase" select="'abcdefghijklmnopqrstuvwxyz'"/>
          <xsl:variable name="upperCase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
          <xsl:value-of select="translate($inputString,$upperCase,$smallCase)"/>
  </xsl:template>
 <xsl:template name="ToUpper">
          <xsl:param name="inputString"/>
          <xsl:variable name="smallCase" select="'abcdefghijklmnopqrstuvwxyz'"/>
          <xsl:variable name="upperCase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
          <xsl:value-of select="translate($inputString,$smallCase,$upperCase)"/>
  </xsl:template>
  <xsl:template name="NoSpace">
          <xsl:param name="inputString"/>
          <xsl:variable name="spaces" select="' '"/>
          <xsl:variable name="underlines" select="''"/>
          <xsl:value-of select="translate($inputString,$spaces,$underlines)"/>
  </xsl:template>
  <xsl:template name="Pluralize">
          <xsl:param name="inputString"/>
          <xsl:choose><xsl:when test="substring($inputString, string-length($inputString)) = 'y'"><xsl:value-of select="concat(substring($inputString, 1, string-length($inputString)-1),'ies')"/></xsl:when><xsl:otherwise><xsl:value-of select="$inputString"/>s</xsl:otherwise></xsl:choose>
  </xsl:template>
</xsl:stylesheet>