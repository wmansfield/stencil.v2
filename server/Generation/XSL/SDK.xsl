<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">


  
<xsl:for-each select="items/item">
  <xsl:variable name="security_route_field"><xsl:value-of select="../@securityRoute"/></xsl:variable>
  <xsl:variable name="security_track"><xsl:value-of select="../@securityTrack"/></xsl:variable>
  <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="removePattern2"><xsl:value-of select="@removePattern"/></xsl:variable>
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.SDK.Client\Endpoints\<xsl:value-of select="../@sdkEndpointFolder"/><xsl:value-of select="@name"/>Endpoint_Core.cs]using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using <xsl:value-of select="../@projectName"/>.SDK.Models;

namespace <xsl:value-of select="../@projectName"/>.SDK.Client.Endpoints
{
    public partial class <xsl:value-of select="@name"/>Endpoint : EndpointBase
    {
        public <xsl:value-of select="@name"/>Endpoint(<xsl:value-of select="../@projectName"/>SDK api)
            : base(api)
        {

        }
        
        public Task&lt;ItemResult&lt;<xsl:value-of select="@name"/>&gt;&gt; Get<xsl:value-of select="@name"/>Async(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="@type" /><xsl:text> </xsl:text><xsl:value-of select="text()" />, </xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true'])>0"><xsl:value-of select="field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if><xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">Guid <xsl:value-of select="text()" />, </xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">Guid <xsl:value-of select="text()" />, </xsl:for-each></xsl:if>Guid<xsl:text> </xsl:text><xsl:value-of select="field[1]"/>)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "<xsl:value-of select="$name_lowered"/>s/<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each><xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">{<xsl:value-of select="text()" />}/</xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">{<xsl:value-of select="text()" />}/</xsl:for-each></xsl:if><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true'])>0">{<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>}/</xsl:if>{<xsl:value-of select="field[1]"/>}";
            <xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]">request.AddUrlSegment("<xsl:value-of select="text()"/>",<xsl:text> </xsl:text><xsl:value-of select="text()"/>.ToString());
            </xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true' and not(@isolated='true')])>0">request.AddUrlSegment("<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>",<xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>.ToString());
            </xsl:if>request.AddUrlSegment("<xsl:value-of select="field[1]"/>",<xsl:text> </xsl:text><xsl:value-of select="field[1]"/>.ToString());
            <xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">
            request.AddUrlSegment("<xsl:value-of select="text()" />",<xsl:text> </xsl:text><xsl:value-of select="text()" />.ToString());</xsl:for-each></xsl:if>
            <xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">
            request.AddUrlSegment("<xsl:value-of select="text()" />",<xsl:text> </xsl:text><xsl:value-of select="text()" />.ToString());</xsl:for-each></xsl:if>
            return this.Sdk.ExecuteAsync&lt;ItemResult&lt;<xsl:value-of select="@name"/>&gt;&gt;(request);
        }

        <xsl:for-each select="field[@filter='true']">
        public Task&lt;ListResult&lt;<xsl:value-of select="../@name"/>&gt;&gt; GetBy<xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(../field[@storePartitionKey='true'])>0"><xsl:value-of select="../field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>, </xsl:if><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, int skip, int take, string order_by = "", bool descending = false<xsl:if test="count(../field[@searchable='true'])>0 or (count(../indexfield[@searchable='true'])>0)">, string keyword = ""</xsl:if>)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "<xsl:value-of select="$name_lowered"/>s/by_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="text()"/></xsl:call-template><xsl:if test="string-length(@extraSecurityRoute)>0">/{<xsl:value-of select="../@extraSecurityRoute"/>}</xsl:if><xsl:if test="count(../field[@storePartitionKey='true'])>0">/{<xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>}</xsl:if>/{<xsl:value-of select="text()"/>}";<xsl:if test="count(../field[@storePartitionKey='true'])>0">
            request.AddUrlSegment("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>.ToString());
            request.AddUrlSegment("<xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>",<xsl:text> </xsl:text><xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>.ToString());</xsl:if>
            <xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]">
            request.AddUrlSegment("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>.ToString());</xsl:for-each>
            <xsl:if test="string-length(@extraSecurityRoute)>0">
            request.AddUrlSegment("<xsl:value-of select="@extraSecurityRoute"/>", <xsl:value-of select="@extraSecurityRoute"/>.ToString()); 
            </xsl:if>
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            <xsl:if test="count(../field[@searchable='true'])>0 or count(../indexfield[@searchable='true'])>0">request.AddParameter("keyword", keyword);</xsl:if>
            <xsl:if test="count(../field[@storePartitionKey='true'])=0"><xsl:for-each select="../field[@foreignKey and not(@noGet='true')]">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each></xsl:if>
            return this.Sdk.ExecuteAsync&lt;ListResult&lt;<xsl:value-of select="../@name"/>&gt;&gt;(request);
        }
        
        </xsl:for-each>
        
       
        <xsl:if test="count(field[@searchable='true']) > 0 or count(indexfield[@searchable='true']) > 0">public Task&lt;ListResult&lt;<xsl:value-of select="@name"/>&gt;&gt; Find<xsl:if test="count(field[@storePartitionKey='true'])>0">For<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:if>(<xsl:if test="string-length(@extraSecurityRoute)>0">Guid <xsl:value-of select="@extraSecurityRoute"/>, </xsl:if><xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(field[@storePartitionKey='true'])>0"><xsl:value-of select="field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>int skip = 0, int take = 10, string keyword = "", string order_by = "", bool descending = false<xsl:if test="count(field[@storePartitionKey='true'])=0"><xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each></xsl:if><xsl:for-each select="field[@filter='true']">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "<xsl:value-of select="$name_lowered"/>s<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]">/{<xsl:value-of select="text()"/>}</xsl:for-each><xsl:if test="count(field[@storePartitionKey='true'])>0">/for_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:call-template>/<xsl:if test="string-length(@extraSecurityRoute)>0">{<xsl:value-of select="@extraSecurityRoute"/>}/</xsl:if>{<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>}</xsl:if>";<xsl:if test="count(field[@storePartitionKey='true'])>0">
            request.AddUrlSegment("<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>",<xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>.ToString());</xsl:if>
            <xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]">
            request.AddUrlSegment("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>.ToString());</xsl:for-each>
            <xsl:if test="string-length(@extraSecurityRoute)>0">
            request.AddUrlSegment("<xsl:value-of select="@extraSecurityRoute"/>", <xsl:value-of select="@extraSecurityRoute"/>.ToString()); 
            </xsl:if>
            request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            request.AddParameter("keyword", keyword);
            <xsl:if test="count(field[@storePartitionKey='true'])=0"><xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each></xsl:if>
            <xsl:for-each select="field[string-length(@searchToggle)>0]">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each>
            <xsl:for-each select="indexfield[string-length(@searchToggle)>0]">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each>
            <xsl:for-each select="field[@filter='true']">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each>
            <xsl:for-each select="field[string-length(@searchToggle)>0]">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each>
            <xsl:for-each select="indexfield[string-length(@searchToggle)>0]">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each>
            return this.Sdk.ExecuteAsync&lt;ListResult&lt;<xsl:value-of select="@name"/>&gt;&gt;(request);
        }</xsl:if>

        <xsl:if test="count(field[@storePartitionKey='true'])=0">
        <xsl:for-each select="field[@foreignKey and not(@noGet='true')]">
        <xsl:variable name="foreignkeyfield"><xsl:value-of select="@foreignKeyField" /></xsl:variable>
        public Task&lt;ListResult&lt;<xsl:value-of select="../@name"/>&gt;&gt; Get<xsl:value-of select="../@name"/>By<xsl:value-of select="@friendlyName" />Async(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true') and text() != $foreignkeyfield]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid<xsl:text> </xsl:text><xsl:value-of select="@foreignKeyField"/><xsl:if test="$removePattern2='true'">, bool include_removed</xsl:if>, int skip = 0, int take = 10, string order_by = "", bool descending = false<xsl:if test="count(../field[@searchable='true']) > 0 or count(../indexfield[@searchable='true']) > 0">, string keyword = ""</xsl:if><xsl:if test="string-length(../@pagingWindow)>0">, DateTime? before_<xsl:value-of select="../@pagingWindow" /> = null</xsl:if><xsl:for-each select="../field[string-length(@searchToggle)>0 and text()!=$foreignkeyfield]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            var request = new RestRequestEx(Method.GET);
            request.Resource = "<xsl:value-of select="$name_lowered"/>s/<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true') and text() != $foreignkeyfield]">{<xsl:value-of select="text()"/>}/</xsl:for-each>by_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@friendlyName"/></xsl:call-template>/{<xsl:value-of select="@foreignKeyField"/>}";
            request.AddUrlSegment("<xsl:value-of select="@foreignKeyField"/>",<xsl:text> </xsl:text><xsl:value-of select="@foreignKeyField"/>.ToString());
            <xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true') and text() != $foreignkeyfield]">request.AddUrlSegment("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>.ToString());
            </xsl:for-each>request.AddParameter("skip", skip);
            request.AddParameter("take", take);
            request.AddParameter("order_by", order_by);
            request.AddParameter("descending", descending);
            <xsl:if test="count(../field[@searchable='true']) > 0 or count(../indexfield[@searchable='true']) > 0">request.AddParameter("keyword", keyword);
            </xsl:if>
            <xsl:if test="string-length(../@pagingWindow)>0">request.AddParameter("before_<xsl:value-of select="../@pagingWindow" />", before_<xsl:value-of select="../@pagingWindow" />);</xsl:if>
            <xsl:if test="$removePattern2='true'">request.AddParameter("include_removed", include_removed);</xsl:if>
            <xsl:for-each select="../field[string-length(@searchToggle)>0]">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each>
            <xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">request.AddParameter("<xsl:value-of select="text()"/>", <xsl:value-of select="text()"/>);
            </xsl:for-each>
            return this.Sdk.ExecuteAsync&lt;ListResult&lt;<xsl:value-of select="../@name"/>&gt;&gt;(request);
        }
        </xsl:for-each>
        </xsl:if>

        public Task&lt;ItemResult&lt;<xsl:value-of select="@name"/>&gt;&gt; Create<xsl:value-of select="@name"/>Async(<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "<xsl:value-of select="$name_lowered"/>s";
            request.AddJsonBody(<xsl:value-of select="$name_lowered"/>);
            return this.Sdk.ExecuteAsync&lt;ItemResult&lt;<xsl:value-of select="@name"/>&gt;&gt;(request);
        }

        public Task&lt;ItemResult&lt;<xsl:value-of select="@name"/>&gt;&gt; Update<xsl:value-of select="@name"/>Async(Guid<xsl:text> </xsl:text><xsl:value-of select="field[1]"/>, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            var request = new RestRequestEx(Method.PUT);
            request.Resource = "<xsl:value-of select="$name_lowered"/>s/{<xsl:value-of select="field[1]"/>}";
            request.AddUrlSegment("<xsl:value-of select="field[1]"/>",<xsl:text> </xsl:text><xsl:value-of select="field[1]"/>.ToString());
            request.AddJsonBody(<xsl:value-of select="$name_lowered"/>);
            return this.Sdk.ExecuteAsync&lt;ItemResult&lt;<xsl:value-of select="@name"/>&gt;&gt;(request);
        }

        <xsl:for-each select="field[string-length(@priorityGroupBy)>0]">
        public Task&lt;ActionResult&gt; Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>Async(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type" /><xsl:text> </xsl:text><xsl:value-of select="text()" />, </xsl:for-each>Guid <xsl:value-of select="../field[1]"/>, int priority)
        {
            var request = new RestRequestEx(Method.POST);
            request.Resource = "<xsl:value-of select="$name_lowered"/>s/<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each>{<xsl:value-of select="../field[1]"/>}/update_<xsl:value-of select="text()"/>/{priority}";
            <xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]">request.AddUrlSegment("<xsl:value-of select="text()"/>",<xsl:text> </xsl:text><xsl:value-of select="text()"/>.ToString());
            </xsl:for-each>request.AddUrlSegment("<xsl:value-of select="../field[1]"/>", <xsl:value-of select="../field[1]"/>.ToString());
            request.AddUrlSegment("priority", priority.ToString());
            return this.Sdk.ExecuteAsync&lt;ActionResult&gt;(request);
        }
        </xsl:for-each>

        public Task&lt;ActionResult&gt; Delete<xsl:value-of select="@name"/>Async(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid<xsl:text> </xsl:text><xsl:value-of select="field[1]"/>)
        {
            var request = new RestRequestEx(Method.DELETE);
            request.Resource = "<xsl:value-of select="$name_lowered"/>s/<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each>{<xsl:value-of select="field[1]"/>}";
            request.AddUrlSegment("<xsl:value-of select="field[1]"/>",<xsl:text> </xsl:text><xsl:value-of select="field[1]"/>.ToString());
            <xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">request.AddUrlSegment("<xsl:value-of select="text()"/>",<xsl:text> </xsl:text><xsl:value-of select="text()"/>.ToString());</xsl:for-each>
            return this.Sdk.ExecuteAsync&lt;ActionResult&gt;(request);
        }
    }
}
'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Plugins.RestApi\Controllers\<xsl:value-of select="@name"/>Controller_Crud.cs]using <xsl:value-of select="../@foundation"/>.Foundation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActionResult = <xsl:value-of select="../@projectName"/>.SDK.ActionResult;
using sdk = <xsl:value-of select="../@projectName"/>.SDK.Models;
using dm = <xsl:value-of select="../@projectName"/>.Domain;
using <xsl:value-of select="../@projectName"/>.Primary;
using <xsl:value-of select="../@projectName"/>.SDK;
using <xsl:value-of select="../@projectName"/>.SDK.Client;
using <xsl:value-of select="../@projectName"/>.Web.Controllers;<xsl:if test="../@useApiWorker='true'">
using <xsl:value-of select="../@projectName"/>.Web.Processing;</xsl:if>

namespace <xsl:value-of select="../@projectName"/>.Plugins.RestAPI.Controllers
{
    <xsl:if test="../@useApiWorker='true'">[UseApiWorker()]
    </xsl:if>[Authorize<xsl:value-of select="../@authorizeSuffix"/>]
    [Route("api/<xsl:value-of select="$name_lowered"/>s")]
    public partial class <xsl:value-of select="@name" />Controller : Health<xsl:value-of select="../@projectName"/>ApiController
    {
        public <xsl:value-of select="@name" />Controller(IFoundation foundation)
            : base(foundation, "<xsl:value-of select="@name" />")
        {
        }

        <xsl:choose><xsl:when test="(@useIndex='true' or @useStore='true') and not(@byPassIndex='true')">
        <xsl:if test="not(@sdkManualGetById='true')">[HttpGet("<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true' and not(@isolated='true')])>0">{<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>}/</xsl:if><xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">{<xsl:value-of select="text()" />}/</xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">{<xsl:value-of select="text()" />}/</xsl:for-each></xsl:if>{<xsl:value-of select="field[1]"/>}")]
        public Task&lt;IActionResult&gt; GetById(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true' and not(@isolated='true')])>0"><xsl:value-of select="field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if><xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">Guid <xsl:value-of select="text()" />, </xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">Guid <xsl:value-of select="text()" />, </xsl:for-each></xsl:if>Guid <xsl:value-of select="field[1]"/>)
        {
            return base.ExecuteFunction<xsl:if test="not(@useIndex='true') or @storePrimary='true'">Async</xsl:if>&lt;IActionResult&gt;("GetById", <xsl:if test="not(@useIndex='true') or @storePrimary='true'">async </xsl:if>delegate()
            {
                <xsl:if test="@userSpecificData='account_id'">dm.Account currentAccount = this.GetCurrentAccount();
                </xsl:if>sdk.<xsl:value-of select="@name" /> result = <xsl:choose>
                <xsl:when test="not(@useIndex='true') or @storePrimary='true'">await this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetDocumentAsync(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true' and not(@isolated='true')])>0"><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if><xsl:value-of select="field[1]"/><xsl:if test="@userSpecificData='account_id'">, currentAccount.account_id</xsl:if>);</xsl:when>
                <xsl:otherwise>this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']"><xsl:value-of select="text()" />, </xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']"><xsl:value-of select="text()" />, </xsl:for-each></xsl:if><xsl:value-of select="field[1]"/><xsl:if test="@userSpecificData='account_id'">, currentAccount.account_id</xsl:if>);</xsl:otherwise>
                </xsl:choose>
                
                if (result == null)
                {
                    return base.Http404("<xsl:value-of select="@name" />");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result.ToDomainModel());
                <xsl:if test="string-length($security_track)>0 and count(field[text()=$security_route_field])>0">this.Analytics.Track<xsl:value-of select="$security_track"/>Access(this.GetCurrentAccount(), result.<xsl:value-of select="$security_route_field"/>);</xsl:if>

                return base.Http200(new ItemResult&lt;sdk.<xsl:value-of select="@name" />&gt;()
                {
                    success = true, 
                    item = result
                });
            });
        }
        </xsl:if>
        
        <xsl:for-each select="field[@filter='true']">
        [HttpGet("by_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="text()"/></xsl:call-template><xsl:if test="string-length(@extraSecurityRoute)>0">/{<xsl:value-of select="../@extraSecurityRoute"/>}</xsl:if><xsl:if test="count(../field[@storePartitionKey='true'])>0">/{<xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>}</xsl:if>/{<xsl:value-of select="text()"/>}")]
        public Task&lt;IActionResult&gt; GetBy<xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(../field[@storePartitionKey='true'])>0"><xsl:value-of select="../field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>, </xsl:if><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, int skip, int take, string order_by = "", bool descending = false<xsl:if test="count(../field[@searchable='true'])>0 or count(../indexfield[@searchable='true'])>0">, string keyword = ""</xsl:if>)
        {
            return base.ExecuteFunction<xsl:if test="not(@useIndex='true') or @storePrimary='true'">Async</xsl:if>&lt;IActionResult&gt;("GetBy<xsl:value-of select="@friendlyName"/>", <xsl:if test="not(@useIndex='true') or @storePrimary='true'">async </xsl:if>delegate()
            {
                Guid? <xsl:value-of select="$security_route_field"/>_security = <xsl:choose>
                <xsl:when test="count(../field[text()=$security_route_field and position()>1 and not(@extraRouteField='true')])>0"><xsl:value-of select="$security_route_field"/>;</xsl:when>
                <xsl:when test="string-length(../@extraSecurityRoute)>0"><xsl:value-of select="../@extraSecurityRoute"/>;</xsl:when>
                <xsl:otherwise>null;</xsl:otherwise></xsl:choose>
                <xsl:for-each select="../field[@securityRoute='true']"><xsl:variable name="foreignKey"><xsl:value-of select="@foreignKey"/></xsl:variable>
                if(<xsl:value-of select="text()"/> != null)
                {
                    sdk.<xsl:value-of select="@foreignKey" /> routeForSecurity = <xsl:choose>
                    <xsl:when test="count(../../item[@name=$foreignKey]/field[@storePartitionKey='true' and text()=$security_route_field])>0 and ((../../item[@name=$foreignKey]/@useStore='true' and not(../../item[@name=$foreignKey]/@useIndex='true')) or ../../item[@name=$foreignKey]/@storePrimary='true')">this.Security.CachedExecute("<xsl:value-of select="@foreignKey" />", <xsl:value-of select="$security_route_field"/>_security, <xsl:value-of select="text()"/>, this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="$foreignKey"/></xsl:call-template>.GetDocument);</xsl:when>
                    <xsl:when test="count(../../item[@name=$foreignKey]/field[@storePartitionKey='true' and text()=$security_route_field])=0 and ((../../item[@name=$foreignKey]/@useStore='true' and not(../../item[@name=$foreignKey]/@useIndex='true')) or ../../item[@name=$foreignKey]/@storePrimary='true')">this.Security.CachedExecute("<xsl:value-of select="@foreignKey" />", <xsl:value-of select="text()"/>, this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="$foreignKey"/></xsl:call-template>.GetById).ToSDKModel();</xsl:when>
                    <xsl:otherwise>this.Security.CachedExecute("<xsl:value-of select="@foreignKey" />", <xsl:value-of select="text()"/>.GetValueOrDefault(), this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.GetById);</xsl:otherwise>
                    </xsl:choose>
                    if(routeForSecurity != null)
                    {
                        <xsl:value-of select="$security_route_field"/>_security = routeForSecurity.<xsl:value-of select="$security_route_field"/>;
                    }
                }</xsl:for-each>
                this.Security.ValidateCanSearch<xsl:value-of select="../@name" />(this.GetCurrentAccount(), <xsl:value-of select="$security_route_field"/>_security);
                <xsl:if test="string-length($security_track)>0 and count(../field[text()=$security_route_field])>0 and count(../field[@foreignKeyField=$security_route_field])>0">this.Analytics.Track<xsl:value-of select="$security_track"/>Access(this.GetCurrentAccount(), <xsl:value-of select="$security_route_field"/>);
                </xsl:if>
                <xsl:if test="@userSpecificData='account_id'">dm.Account currentAccount = this.GetCurrentAccount();</xsl:if>

                ListResult&lt;sdk.<xsl:value-of select="../@name" />&gt; result = await this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.GetBy<xsl:value-of select="@friendlyName"/>Async(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="../@userSpecificData='account_id'">currentAccount.account_id, </xsl:if><xsl:if test="count(../field[@storePartitionKey='true'])>0"><xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>, </xsl:if><xsl:value-of select="text()"/>, skip, take, order_by, descending<xsl:if test="count(../field[@searchable='true'])>0 or (count(../indexfield[@searchable='true'])>0)">, keyword</xsl:if>);
                

                result.success = true;
                return base.Http200(result);
            });
        }
        </xsl:for-each>
        <xsl:if test="not(@sdkManualFind='true') and count(field[@searchable='true']) > 0 or count(indexfield[@searchable='true']) > 0">
        <xsl:if test="count(field[@storePartitionKey='Self'])>0 or count(field[@storePartitionKey='SplitID'])>0">[Obsolete("Use caution, this is expensive for multi-partition tables", false)]</xsl:if>
        [HttpGet("<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each><xsl:if test="count(field[@storePartitionKey='true'])>0">for_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:call-template><xsl:if test="string-length(@extraSecurityRoute)>0">/{<xsl:value-of select="@extraSecurityRoute"/>}</xsl:if>/{<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>}</xsl:if>")]
        public Task&lt;IActionResult&gt; Find<xsl:if test="count(field[@storePartitionKey='true'])>0">For<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:if>(<xsl:if test="string-length(@extraSecurityRoute)>0">Guid <xsl:value-of select="@extraSecurityRoute"/>, </xsl:if><xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(field[@storePartitionKey='true'])>0"><xsl:value-of select="field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = ""<xsl:for-each select="field[@filter='true']">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = null</xsl:for-each><xsl:if test="count(field[@storePartitionKey='true'])=0"><xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each></xsl:if><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:if test="/items/enum[@name=$searchType] and @searchToggle!='null'">sdk.</xsl:if><xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">, <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:if test="/items/enum[@name=$searchType] and @searchToggle!='null'">sdk.</xsl:if><xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction<xsl:if test="not(@useIndex='true') or @storePrimary='true'">Async</xsl:if>&lt;IActionResult&gt;("Find<xsl:if test="count(field[@storePartitionKey='true'])>0">For<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:if>", <xsl:if test="not(@useIndex='true') or @storePrimary='true'">async </xsl:if>delegate()
            {
                Guid? <xsl:value-of select="$security_route_field"/>_security = <xsl:choose>
                <xsl:when test="count(field[text()=$security_route_field and position()>1 and not(@extraRouteField='true')])>0"><xsl:value-of select="$security_route_field"/>;</xsl:when>
                <xsl:when test="string-length(../@extraSecurityRoute)>0"><xsl:value-of select="../@extraSecurityRoute"/>;</xsl:when>
                <xsl:otherwise>null;</xsl:otherwise></xsl:choose>
                <xsl:for-each select="field[@securityRoute='true']"><xsl:variable name="foreignKey"><xsl:value-of select="@foreignKey"/></xsl:variable>
                if(<xsl:value-of select="text()"/> != null)
                {
                    sdk.<xsl:value-of select="@foreignKey" /> routeForSecurity = <xsl:choose>
                    <xsl:when test="count(../../item[@name=$foreignKey]/field[@storePartitionKey='true' and text()=$security_route_field])>0 and ((../../item[@name=$foreignKey]/@useStore='true' and not(../../item[@name=$foreignKey]/@useIndex='true')) or ../../item[@name=$foreignKey]/@storePrimary='true')">this.Security.CachedExecute("<xsl:value-of select="@foreignKey" />", <xsl:value-of select="$security_route_field"/>_security, <xsl:value-of select="text()"/>, this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="$foreignKey"/></xsl:call-template>.GetDocument);</xsl:when>
                    <xsl:when test="count(../../item[@name=$foreignKey]/field[@storePartitionKey='true' and text()=$security_route_field])=0 and ((../../item[@name=$foreignKey]/@useStore='true' and not(../../item[@name=$foreignKey]/@useIndex='true')) or ../../item[@name=$foreignKey]/@storePrimary='true')">this.Security.CachedExecute("<xsl:value-of select="@foreignKey" />", <xsl:value-of select="text()"/>, this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="$foreignKey"/></xsl:call-template>.GetById).ToSDKModel();</xsl:when>
                    <xsl:otherwise>this.Security.CachedExecute("<xsl:value-of select="@foreignKey" />", <xsl:value-of select="text()"/>.GetValueOrDefault(), this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.GetById);</xsl:otherwise>
                    </xsl:choose>
                    if(routeForSecurity != null)
                    {
                        <xsl:value-of select="$security_route_field"/>_security = routeForSecurity.<xsl:value-of select="$security_route_field"/>;
                    }
                }</xsl:for-each>
                this.Security.ValidateCanSearch<xsl:value-of select="@name" />(this.GetCurrentAccount(), <xsl:value-of select="$security_route_field"/>_security);
                <xsl:if test="string-length($security_track)>0 and count(field[text()=$security_route_field])>0 and count(field[@foreignKeyField=$security_route_field])>0">this.Analytics.Track<xsl:value-of select="$security_track"/>Access(this.GetCurrentAccount(), <xsl:value-of select="$security_route_field"/>);
                </xsl:if>
                <xsl:if test="@userSpecificData='account_id'">dm.Account currentAccount = this.GetCurrentAccount();</xsl:if>
                ListResult&lt;sdk.<xsl:value-of select="@name" />&gt; result = <xsl:choose>
                <xsl:when test="not(@useIndex='true') or @storePrimary='true'">await this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Find<xsl:if test="count(field[@storePartitionKey='true'])>0">For<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:if>Async(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="@userSpecificData='account_id'">currentAccount.account_id, </xsl:if><xsl:if test="count(field[@storePartitionKey='true'])>0"><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>skip, take, order_by, descending, keyword<xsl:for-each select="field[@filter='true']">, <xsl:value-of select="text()"/></xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:value-of select="text()"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">, <xsl:value-of select="text()"/></xsl:for-each>);</xsl:when>
                <xsl:otherwise>this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Find(<xsl:if test="@userSpecificData='account_id'">currentAccount.account_id, </xsl:if>skip, take, keyword, order_by, descending<xsl:if test="count(field[@storePartitionKey='true'])=0"><xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, <xsl:value-of select="text()"/></xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:value-of select="text()"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">, <xsl:value-of select="text()"/></xsl:for-each></xsl:if><xsl:if test="count(field[@storePartitionKey='true'])>0">, <xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>: <xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/></xsl:if>);</xsl:otherwise>
                </xsl:choose>
                
                result.success = true;
                return base.Http200(result);
            });
        }
        </xsl:if>
        
        <xsl:if test="count(field[@storePartitionKey='true'])=0">
        <xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@sdkManualGet='true') and (../@useIndex='true' or ../@useStore='true') and not(../@byPassIndex='true')]">
        <xsl:variable name="foreignKey"><xsl:value-of select="@foreignKey"/></xsl:variable>
        <xsl:variable name="securityRoute"><xsl:value-of select="@securityRoute"/></xsl:variable>
        <xsl:variable name="foreignKeyField"><xsl:value-of select="@foreignKeyField"/></xsl:variable>
        [HttpGet("by_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@friendlyName"/></xsl:call-template>/{<xsl:value-of select="@foreignKeyField"/>}")]
        public <xsl:if test="@storePrimary='true'">Task&lt;</xsl:if>IActionResult<xsl:if test="@storePrimary='true'">&gt;</xsl:if> GetBy<xsl:value-of select="@friendlyName" />(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true') and text() != $foreignKeyField]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="@foreignKeyField"/>, int skip = 0, int take = 10, string order_by = "", bool descending = false<xsl:if test="count(../field[@searchable='true']) > 0 or count(../indexfield[@searchable='true']) > 0">, string keyword = ""</xsl:if><xsl:if test="string-length(../@pagingWindow)>0">, DateTime? before_<xsl:value-of select="../@pagingWindow" /> = null</xsl:if><xsl:for-each select="../field[string-length(@searchToggle)>0]">, <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:if test="/items/enum[@name=$searchType] and @searchToggle!='null'">sdk.</xsl:if><xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">, <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:if test="/items/enum[@name=$searchType] and @searchToggle!='null'">sdk.</xsl:if><xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction<xsl:if test="@storePrimary='true'">Async</xsl:if>&lt;IActionResult&gt;("GetBy<xsl:value-of select="@friendlyName" />", <xsl:if test="@storePrimary='true'">async </xsl:if>delegate ()
            {
                <xsl:if test="string-length($security_track)>0 and not(@foreignKeyField=$security_route_field) and ../../item[@name=$foreignKey]/field/text()=$security_route_field">Guid? <xsl:value-of select="$security_route_field"/> = null;
                <xsl:choose><xsl:when test="../../item[@name=$foreignKey]/@useIndex='true'">sdk.<xsl:value-of select="$foreignKey"/> reference<xsl:value-of select="$foreignKey"/> = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="$foreignKey"/></xsl:call-template>.GetById(<xsl:value-of select="@foreignKeyField"/>);</xsl:when><xsl:otherwise>
                dm.<xsl:value-of select="$foreignKey"/> reference<xsl:value-of select="$foreignKey"/> = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="$foreignKey"/></xsl:call-template>.GetById(<xsl:value-of select="@foreignKeyField"/>);
                </xsl:otherwise></xsl:choose>
                if(reference<xsl:value-of select="$foreignKey"/> != null)
                {
                    <xsl:value-of select="$security_route_field"/> = reference<xsl:value-of select="$foreignKey"/>.<xsl:value-of select="$security_route_field"/>;
                }
                <xsl:if test="string-length($security_track)>0 and count(field[text()=$security_route_field])>0">this.Analytics.Track<xsl:value-of select="$security_track"/>Access(this.GetCurrentAccount(), <xsl:value-of select="$security_route_field"/>);</xsl:if></xsl:if>

                Guid? <xsl:value-of select="$security_route_field"/>_security = <xsl:choose><xsl:when test="@foreignKeyField=$security_route_field or ../../item[@name=$foreignKey]/field/text()=$security_route_field"><xsl:value-of select="$security_route_field"/></xsl:when><xsl:otherwise>null</xsl:otherwise></xsl:choose>;
                <xsl:if test="$securityRoute='true'">
                sdk.<xsl:value-of select="@foreignKey" /> routeForSecurity = this.Security.CachedExecute("<xsl:value-of select="@foreignKey" />", <xsl:value-of select="text()"/>, this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.GetById);
                if(routeForSecurity != null)
                {
                    <xsl:value-of select="$security_route_field"/>_security = routeForSecurity.<xsl:value-of select="$security_route_field"/>;
                }
                </xsl:if>

                this.Security.ValidateCanList<xsl:value-of select="../@name" />(this.GetCurrentAccount(), <xsl:value-of select="$security_route_field"/>_security);
                <xsl:if test="../@userSpecificData='account_id'">dm.Account currentAccount = this.GetCurrentAccount();</xsl:if>
                <xsl:choose><xsl:when test="not(@useIndex='true')">ListResult&lt;sdk.<xsl:value-of select="../@name"/>&gt; result = new ListResult&lt;sdk.<xsl:value-of select="../@name"/>&gt;();
                result.items = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.GetBy<xsl:value-of select="@friendlyName" />(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="@foreignKeyField"/>).ToSDKModel();
                </xsl:when>
                <xsl:otherwise>ListResult&lt;sdk.<xsl:value-of select="../@name"/>&gt; result = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.GetBy<xsl:value-of select="@friendlyName" />(<xsl:value-of select="@foreignKeyField"/>, skip, take, order_by, descending<xsl:if test="count(../field[@searchable='true']) > 0 or count(../indexfield[@searchable='true']) > 0">, keyword</xsl:if><xsl:if test="string-length(../@pagingWindow)>0">, before_<xsl:value-of select="../@pagingWindow" /></xsl:if><xsl:if test="../@userSpecificData='account_id'">, currentAccount.account_id</xsl:if><xsl:for-each select="../field[string-length(@searchToggle)>0]">, <xsl:value-of select="text()"/></xsl:for-each><xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">, <xsl:value-of select="text()"/></xsl:for-each>);
                </xsl:otherwise></xsl:choose>
                result.success = true;
                return base.Http200(result);
            });
        }
        </xsl:for-each>
        </xsl:if>

        <xsl:if test="not(@sdkManualCreate='true')">
        [HttpPost]
        public Task&lt;IActionResult&gt; Create(sdk.<xsl:value-of select="@name" /><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            return base.ExecuteFunction<xsl:if test="not(@useIndex='true') or @storePrimary='true'">Async</xsl:if>&lt;IActionResult&gt;("Create", async delegate()
            {
                this.ValidateNotNull(<xsl:value-of select="$name_lowered"/>, "<xsl:value-of select="@name" />");

                dm.<xsl:value-of select="@name" /> insert = <xsl:value-of select="$name_lowered"/>.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Insert(insert);
                
                <xsl:choose>
                <xsl:when test="(@useStore='true' and not(@useIndex='true')) or @storePrimary='true'">sdk.<xsl:value-of select="@name" /> insertResult = await this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetDocumentAsync(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true' and not(@isolated='true')])>0">insert.<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>insert.<xsl:value-of select="field[1]"/>);</xsl:when>
                <xsl:when test="@useIndex='true' and not(@byPassIndex='true')">sdk.<xsl:value-of select="@name" /> insertResult = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">insert.<xsl:value-of select="text()" />.GetValueOrDefault(), </xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">insert.<xsl:value-of select="text()" />, </xsl:for-each></xsl:if>insert.<xsl:value-of select="field[1]"/>);</xsl:when>
                <xsl:otherwise>sdk.<xsl:value-of select="@name" /> insertResult = insert.ToSDKModel();</xsl:otherwise></xsl:choose>

                return base.Http200(new ItemResult&lt;sdk.<xsl:value-of select="@name" />&gt;()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        </xsl:if>

        <xsl:if test="not(@sdkManualUpdate='true')">
        [HttpPut("{<xsl:value-of select="field[1]"/>}")]
        public Task&lt;IActionResult&gt; Update(Guid <xsl:value-of select="field[1]"/>, sdk.<xsl:value-of select="@name" /><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            return base.ExecuteFunction<xsl:if test="not(@useIndex='true') or @storePrimary='true'">Async</xsl:if>&lt;IActionResult&gt;("Update", async delegate()
            {
                this.ValidateNotNull(<xsl:value-of select="$name_lowered"/>, "<xsl:value-of select="@name" />");
                this.ValidateRouteMatch(<xsl:value-of select="field[1]"/>, <xsl:value-of select="$name_lowered"/>.<xsl:value-of select="field[1]"/>, "<xsl:value-of select="@name" />");

                dm.<xsl:value-of select="@name" /> found = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]"/>);
                this.ValidateNotNull(found, "<xsl:value-of select="@name" />");

                <xsl:value-of select="$name_lowered"/>.<xsl:value-of select="field[1]"/> = <xsl:value-of select="field[1]"/>;
                dm.<xsl:value-of select="@name" /> update = <xsl:value-of select="$name_lowered"/>.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Update(update);
                
                <xsl:choose>
                <xsl:when test="(@useStore='true' and not(@useIndex='true')) or @storePrimary='true'">sdk.<xsl:value-of select="@name" /> existing = await this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetDocumentAsync(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true' and not(@isolated='true')])>0">update.<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>update.<xsl:value-of select="field[1]"/>);</xsl:when>
                <xsl:when test="@useIndex='true' and not(@byPassIndex='true')">sdk.<xsl:value-of select="@name" /> existing = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">update.<xsl:value-of select="text()" />.GetValueOrDefault(), </xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">update.<xsl:value-of select="text()" />, </xsl:for-each></xsl:if>update.<xsl:value-of select="field[1]"/>);</xsl:when>
                <xsl:otherwise>sdk.<xsl:value-of select="@name" /> existing = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="text()"/>, </xsl:for-each>update.<xsl:value-of select="field[1]"/>).ToSDKModel();</xsl:otherwise></xsl:choose>
                
                return base.Http200(new ItemResult&lt;sdk.<xsl:value-of select="@name" />&gt;()
                {
                    success = true,
                    item = existing
                });

            });

        }
        </xsl:if>

        <xsl:for-each select="field[string-length(@priorityGroupBy)>0]">
        [HttpPost("<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each>{<xsl:value-of select="../field[1]"/>}/update_<xsl:value-of select="text()"/>/{priority}")]
        public IActionResult Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="../field[1]"/>, int priority)
        {
            return base.ExecuteFunction&lt;IActionResult&gt;("Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>", delegate ()
            {
                dm.<xsl:value-of select="../@name"/> found = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.GetById(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="../field[1]"/>);
                this.ValidateNotNull(found, "<xsl:value-of select="../@name"/>");
                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), found);

                this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="text()" />, </xsl:for-each>found.<xsl:value-of select="@priorityGroupBy"/>, found.<xsl:value-of select="../field[1]"/>, priority);

                return base.Http200(new ActionResult()
                {
                    success = true
                });
            });
        }
        </xsl:for-each>

        <xsl:if test="not(@sdkManualDelete='true')">[HttpDelete("<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each>{<xsl:value-of select="field[1]"/>}")]
        public IActionResult Delete(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.<xsl:value-of select="@name" /> delete = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]"/>);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Delete(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]"/>);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = <xsl:value-of select="field[1]"/>.ToString()
                });
            });
        }</xsl:if>
        
        </xsl:when>
        <xsl:otherwise>[HttpGet("<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each>{<xsl:value-of select="field[1]"/>}")]
        public IActionResult GetById(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>)
        {
            return base.ExecuteFunction&lt;IActionResult&gt;("GetById", delegate()
            {
                dm.<xsl:value-of select="@name" /> result = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]"/>);
                if (result == null)
                {
                    return Http404("<xsl:value-of select="@name" />");
                }

                this.Security.ValidateCanRetrieve(this.GetCurrentAccount(), result);
                <xsl:if test="string-length($security_track)>0 and count(field[text()=$security_route_field])>0">this.Analytics.Track<xsl:value-of select="$security_track"/>Access(this.GetCurrentAccount(), result.<xsl:value-of select="$security_route_field"/>);</xsl:if>

                return base.Http200(new ItemResult&lt;sdk.<xsl:value-of select="@name" />&gt;()
                {
                    success = true,
                    item = result.ToSDKModel()
                });
            });
        }
        
        <xsl:if test="count(field[@searchable='true']) > 0 or count(indexfield[@searchable='true']) > 0">[HttpGet("<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]">{<xsl:value-of select="text()"/>}</xsl:for-each>")]
        public IActionResult Find(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>int skip = 0, int take = 10, string order_by = "", bool descending = false, string keyword = ""<xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">,  <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">,  <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction&lt;IActionResult&gt;("Find", delegate()
            {

                this.Security.ValidateCanSearch<xsl:value-of select="@name" />(this.GetCurrentAccount());

                int takePlus = take;
                if (take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }

                List&lt;dm.<xsl:value-of select="@name" />&gt; result = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Find(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="text()"/>, </xsl:for-each>skip, takePlus, keyword, order_by, descending<xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, <xsl:value-of select="text()"/></xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:value-of select="text()"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">, <xsl:value-of select="text()"/></xsl:for-each>);
                return base.Http200(result.ToSteppedListResult(skip, take, result.Count));

            });
        }
        
        </xsl:if>
        <xsl:variable name="removePattern"><xsl:value-of select="@removePattern"/></xsl:variable>
        <xsl:for-each select="field[@foreignKey and not(@noGet='true')]"><xsl:variable name="foreignKeyField"><xsl:value-of select="@foreignKeyField"/></xsl:variable>[HttpGet("<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true') and text() != $foreignKeyField]">{<xsl:value-of select="text()"/>}/</xsl:for-each>by_<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@friendlyName"/></xsl:call-template>/{<xsl:value-of select="@foreignKeyField"/>}")]
        public IActionResult GetBy<xsl:value-of select="@friendlyName" />(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true') and text() != $foreignKeyField]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="@foreignKeyField"/><xsl:if test="$removePattern='true'">, bool include_removed = false</xsl:if>)
        {
            return base.ExecuteFunction&lt;IActionResult&gt;("GetBy<xsl:value-of select="@friendlyName" />", delegate ()
            {
                List&lt;dm.<xsl:value-of select="../@name"/>&gt; result = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.GetBy<xsl:value-of select="@friendlyName" />(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true') and text() != $foreignKeyField]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="@foreignKeyField"/><xsl:if test="$removePattern='true'">, include_removed</xsl:if>);

                return base.Http200(new ListResult&lt;sdk.<xsl:value-of select="../@name"/>&gt;()
                {
                    success = true,
                    items = result.ToSDKModel()
                });
            });
        }
        </xsl:for-each>


        <xsl:if test="not(@sdkManualCreate='true')">
        [HttpPost]
        public IActionResult Create(sdk.<xsl:value-of select="@name" /><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            return base.ExecuteFunction&lt;IActionResult&gt;("Create", delegate()
            {
                this.ValidateNotNull(<xsl:value-of select="$name_lowered"/>, "<xsl:value-of select="@name" />");

                dm.<xsl:value-of select="@name" /> insert = <xsl:value-of select="$name_lowered"/>.ToDomainModel();

                this.Security.ValidateCanCreate(this.GetCurrentAccount(), insert);
                
                insert = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Insert(insert);
                
                <xsl:choose>
                <xsl:when test="(@useStore='true' and not(@useIndex='true')) or @storePrimary='true'">sdk.<xsl:value-of select="@name" /> insertResult = this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetDocument(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true' and not(@isolated='true')])>0">insert.<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>insert.<xsl:value-of select="field[1]"/>);</xsl:when>
                <xsl:when test="@useIndex='true' and not(@byPassIndex='true')">sdk.<xsl:value-of select="@name" /> insertResult = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">insert.<xsl:value-of select="text()" />.GetValueOrDefault(), </xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">insert.<xsl:value-of select="text()" />, </xsl:for-each></xsl:if>insert.<xsl:value-of select="field[1]"/>);</xsl:when>
                <xsl:otherwise>sdk.<xsl:value-of select="@name" /> insertResult = insert.ToSDKModel();</xsl:otherwise></xsl:choose>

                return base.Http200(new ItemResult&lt;sdk.<xsl:value-of select="@name" />&gt;()
                {
                    success = true,
                    item = insertResult
                });
            });

        }
        </xsl:if>

        <xsl:if test="not(@sdkManualUpdate='true')">
        [HttpPut("{<xsl:value-of select="field[1]"/>}")]
        public IActionResult Update(Guid <xsl:value-of select="field[1]"/>, sdk.<xsl:value-of select="@name" /><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            return base.ExecuteFunction&lt;IActionResult&gt;("Update", delegate()
            {
                this.ValidateNotNull(<xsl:value-of select="$name_lowered"/>, "<xsl:value-of select="@name" />");
                this.ValidateRouteMatch(<xsl:value-of select="field[1]"/>, <xsl:value-of select="$name_lowered"/>.<xsl:value-of select="field[1]"/>, "<xsl:value-of select="@name" />");

                dm.<xsl:value-of select="@name" /> found = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]"/>);
                this.ValidateNotNull(found, "<xsl:value-of select="@name" />");

                <xsl:value-of select="$name_lowered"/>.<xsl:value-of select="field[1]"/> = <xsl:value-of select="field[1]"/>;
                dm.<xsl:value-of select="@name" /> update = <xsl:value-of select="$name_lowered"/>.ToDomainModel(found);

                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), update);

                update = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Update(update);
                
                <xsl:choose>
                <xsl:when test="(@useStore='true' and not(@useIndex='true')) or @storePrimary='true'">sdk.<xsl:value-of select="@name" /> existing = this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetDocument(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="(not(@useIndex='true') or @storePrimary='true') and count(field[@storePartitionKey='true' and not(@isolated='true')])>0">update.<xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>update.<xsl:value-of select="field[1]"/>);</xsl:when>
                <xsl:when test="@useIndex='true' and not(@byPassIndex='true')">sdk.<xsl:value-of select="@name" /> existing = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:if test="string-length(@indexGrandParent) > 0"><xsl:for-each select="field[@indexGrandParent='true']">update.<xsl:value-of select="text()" />.GetValueOrDefault(), </xsl:for-each></xsl:if><xsl:if test="string-length(@indexParent) > 0"><xsl:for-each select="field[@indexParent='true']">update.<xsl:value-of select="text()" />, </xsl:for-each></xsl:if>update.<xsl:value-of select="field[1]"/>);</xsl:when>
                <xsl:otherwise>sdk.<xsl:value-of select="@name" /> existing = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="text()"/>, </xsl:for-each>update.<xsl:value-of select="field[1]"/>).ToSDKModel();</xsl:otherwise></xsl:choose>
                
                return base.Http200(new ItemResult&lt;sdk.<xsl:value-of select="@name" />&gt;()
                {
                    success = true,
                    item = existing
                });

            });

        }
        </xsl:if>

        <xsl:for-each select="field[string-length(@priorityGroupBy)>0]">
        [HttpPost("<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each>{<xsl:value-of select="../field[1]"/>}/update_<xsl:value-of select="text()"/>/{priority}")]
        public IActionResult Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="../field[1]"/>, int priority)
        {
            return base.ExecuteFunction&lt;IActionResult&gt;("Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>", delegate ()
            {
                dm.<xsl:value-of select="../@name"/> found = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.GetById(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="../field[1]"/>);
                this.ValidateNotNull(found, "<xsl:value-of select="../@name"/>");
                this.Security.ValidateCanUpdate(this.GetCurrentAccount(), found);

                this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="text()" />, </xsl:for-each>found.<xsl:value-of select="@priorityGroupBy"/>, found.<xsl:value-of select="../field[1]"/>, priority);

                return base.Http200(new ActionResult()
                {
                    success = true
                });
            });
        }
        </xsl:for-each>

        <xsl:if test="not(@sdkManualDelete='true')">[HttpDelete("<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">{<xsl:value-of select="text()"/>}/</xsl:for-each>{<xsl:value-of select="field[1]"/>}")]
        public IActionResult Delete(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>)
        {
            return base.ExecuteFunction("Delete", delegate()
            {
                dm.<xsl:value-of select="@name" /> delete = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]"/>);
                
                this.Security.ValidateCanDelete(this.GetCurrentAccount(), delete);
                
                this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Delete(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]"/>);

                return Http200(new ActionResult()
                {
                    success = true,
                    message = <xsl:value-of select="field[1]"/>.ToString()
                });
            });
        }</xsl:if>
        
        </xsl:otherwise></xsl:choose>
        
        
       
        

    }
}

'''[ENDFILE]

</xsl:for-each>
  
'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.SDK.Client\<xsl:value-of select="items/@projectName"/>SDK_Endpoints_Core.cs]using <xsl:value-of select="items/@projectName"/>.SDK.Client.Endpoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace <xsl:value-of select="items/@projectName"/>.SDK.Client
{
    public partial class <xsl:value-of select="items/@projectName"/>SDK
    {
        // members for web ease
        <xsl:for-each select="items/item">public <xsl:value-of select="@name" />Endpoint <xsl:value-of select="@name" />;
        </xsl:for-each>

        protected virtual void ConstructCoreEndpoints()
        {
            <xsl:for-each select="items/item">this.<xsl:value-of select="@name" /> = new <xsl:value-of select="@name" />Endpoint(this);
            </xsl:for-each>
        }   
    }
}

'''[ENDFILE]


</xsl:template>

<!-- <xsl:choose><xsl:when test=""></xsl:when><xsl:otherwise></xsl:otherwise></xsl:choose> -->

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
  <xsl:template name="Pluralize">
          <xsl:param name="inputString"/>
          <xsl:choose>
            <xsl:when test="substring($inputString, string-length($inputString)) = 'x'"><xsl:value-of select="$inputString"/>es</xsl:when>
            <xsl:when test="substring($inputString, string-length($inputString)-1) = 'ch'"><xsl:value-of select="$inputString"/>es</xsl:when>
            <xsl:when test="substring($inputString, string-length($inputString)) = 'y'"><xsl:value-of select="concat(substring($inputString, 1, string-length($inputString)-1),'ies')"/></xsl:when>
            <xsl:otherwise><xsl:value-of select="$inputString"/>s</xsl:otherwise></xsl:choose>
  </xsl:template>
</xsl:stylesheet>