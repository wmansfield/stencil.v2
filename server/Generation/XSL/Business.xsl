<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">

<xsl:for-each select="items/item">
  <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
  <xsl:variable name="removePattern2"><xsl:value-of select="@removePattern"/></xsl:variable>
  <xsl:variable name="currentEntityName"><xsl:value-of select="@name"/></xsl:variable>
<xsl:if test="../@trackChanges='true' and not(@ignoreChanges='true')">
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\History\ITrackHistory_<xsl:value-of select="@name"/>.cs]using System;
using System.Collections.Generic;
using System.Text;
using <xsl:value-of select="../@projectName"/>.Domain;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.History
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface ITrackHistory
    {
        void TrackInsert(<xsl:value-of select="@name"/> updated);
        void TrackUpdate(<xsl:value-of select="@name"/> previous, <xsl:value-of select="@name"/> updated);
        void TrackDelete(<xsl:value-of select="@name"/> deleted);
    }
}
'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\History\Implementation\TrackHistory_<xsl:value-of select="@name"/>.cs]using System;
using <xsl:value-of select="../@projectName"/>.Domain;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.History.Implementation
{
    public partial class TrackHistory
    {
        public virtual void TrackInsert(<xsl:value-of select="@name"/> updated)
        {
            base.ExecuteMethod("TrackInsert", delegate ()
            {
                this.TrackInsert("<xsl:value-of select="@name"/>", updated.<xsl:value-of select="field[1]/text()" />, updated);
            });
        }
        public virtual void TrackUpdate(<xsl:value-of select="@name"/> previous, <xsl:value-of select="@name"/> updated)
        {
            base.ExecuteMethod("TrackUpdate", delegate ()
            {
                int changes = 0;
                if(previous != null)
                {
                    <xsl:for-each select="field">
                    if(previous.<xsl:value-of select="text()" /> != updated.<xsl:value-of select="text()"/>) { changes++; }
                    </xsl:for-each>
                }
                this.TrackUpdate("<xsl:value-of select="@name"/>", updated.<xsl:value-of select="field[1]/text()" />, previous, updated, changes);
            });
        }
        public virtual void TrackDelete(<xsl:value-of select="@name"/> deleted)
        {
            base.ExecuteMethod("TrackDelete", delegate ()
            {
                this.TrackDelete("<xsl:value-of select="@name"/>", deleted.<xsl:value-of select="field[1]/text()" />, deleted);
            });
        }
    }
}
'''[ENDFILE]
</xsl:if>

'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Direct\I<xsl:value-of select="@name"/>Business_Crud.cs]using System;
using System.Collections.Generic;
using System.Text;
using <xsl:value-of select="../@projectName"/>.Domain;
using <xsl:value-of select="../@projectName"/>.Data.Sql;
using <xsl:value-of select="../@projectName"/>.Primary.Business.Synchronization;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Direct
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface I<xsl:value-of select="@name"/>Business
    {
        <xsl:if test="@useIndex='true' or @useStore='true'">I<xsl:value-of select="@name"/>Synchronizer Synchronizer { get; }
        </xsl:if>
    
        <xsl:value-of select="@name"/> GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]"/>);
        <xsl:if test="count(field[@searchable='true' and string-length(@computedFrom)=0]) > 0">List&lt;<xsl:value-of select="@name"/>&gt; Find(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>int skip, int take, string keyword = "", string order_by = "", bool descending = false<xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text> <xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>);
        <xsl:if test="not(@userIndex='true')">int FindTotal(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>string keyword = ""<xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text> <xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>);
        </xsl:if></xsl:if>
        <xsl:for-each select="field[@foreignKey and not(@noGet='true')]"><xsl:variable name="currentForeign"><xsl:value-of select="text()"/></xsl:variable>
        List&lt;<xsl:value-of select="../@name"/>&gt; GetBy<xsl:value-of select="@friendlyName" />(<xsl:for-each select="../field[@tenant='true' and not(@isolated='true') and text() != $currentForeign]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="@foreignKeyField"/><xsl:if test="$removePattern2='true'">, bool includeRemoved</xsl:if>);
        <xsl:if test="@foreignKeyInvalidatesMe='true'">void InvalidateFor<xsl:value-of select="@friendlyName" />(<xsl:for-each select="../field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="@foreignKeyField"/>, string reason);</xsl:if>
        </xsl:for-each>
        <xsl:for-each select="field[@lookup='true']"><xsl:variable name="currentForeign"><xsl:value-of select="text()"/></xsl:variable>
        List&lt;<xsl:value-of select="../@name"/>&gt; GetBy<xsl:call-template name="NoSpace"><xsl:with-param name="inputString" select="@friendlyName"/></xsl:call-template>(<xsl:for-each select="../field[@tenant='true'  and not(@isolated='true') and text() != $currentForeign]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="@type" /><xsl:text> </xsl:text><xsl:value-of select="text()"/>);
        </xsl:for-each>

        <xsl:if test="count(../item/field[@foreignKey=$currentEntityName and @foreignKeyComputesMe='true'])>0">
        
        void CascadeInvalidate(<xsl:value-of select="../@projectName"/>Context database, Guid <xsl:value-of select="field[1]/text()"/>, HashSet&lt;string&gt; chain = null);
        void CascadeCompute(<xsl:value-of select="../@projectName"/>Context database, Guid <xsl:value-of select="field[1]/text()"/>, HashSet&lt;string&gt; chain = null);
        void CascadeSynchronize(Guid <xsl:value-of select="field[1]/text()"/>, Availability availability, HashSet&lt;string&gt; chain = null);
        
        </xsl:if>
        <xsl:text>
        </xsl:text>
        <xsl:value-of select="@name"/> Insert(<xsl:value-of select="@name"/> insert<xsl:value-of select="@name"/>);
        <xsl:value-of select="@name"/> Insert(<xsl:value-of select="@name"/> insert<xsl:value-of select="@name"/>, Availability availability);
        <xsl:value-of select="@name"/> Update(<xsl:value-of select="@name"/> update<xsl:value-of select="@name"/>);
        <xsl:value-of select="@name"/> Update(<xsl:value-of select="@name"/> update<xsl:value-of select="@name"/>, Availability availability);
        <xsl:for-each select="field[string-length(@priorityGroupBy)>0]"><xsl:variable name="priorityGroupBy"><xsl:value-of select="@priorityGroupBy"/></xsl:variable>
        void Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true' and not(@isolated='true') and text()!=$priorityGroupBy]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="@priorityGroupBy!='self'">Guid <xsl:value-of select="@priorityGroupBy"/>, </xsl:if>Guid <xsl:value-of select="../field[1]"/>, int priority);
        </xsl:for-each>
        void Delete(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>);
        
        <xsl:if test="@useIndex='true' or @useStore='true'">void SynchronizationUpdate(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>, bool success, DateTime sync_date_utc, string sync_log);
        List&lt;IdentityInfo&gt; SynchronizationGetInvalid(<xsl:for-each select="field[@tenant='true']">string tenant_code, </xsl:for-each>int retryPriorityThreshold, string sync_agent);
        void SynchronizationHydrateUpdate(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>, bool success, DateTime sync_date_utc, string sync_log);
        List&lt;IdentityInfo&gt; SynchronizationHydrateGetInvalid(<xsl:for-each select="field[@tenant='true']">string tenant_code, </xsl:for-each>int retryPriorityThreshold, string sync_agent);</xsl:if>
        <xsl:if test="@useIndex='true' or @useStore='true'">
        void Invalidate(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>, string reason);</xsl:if>
    }
}

'''[ENDFILE]


'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Direct\Implementation\<xsl:value-of select="@name"/>Business_Crud.cs]using <xsl:value-of select="../@foundation"/>.Foundation;
using <xsl:value-of select="../@foundation"/>.Foundation<xsl:value-of select="../@foundationCommon"/>.Aspect;
using Z.EntityFramework.Plus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dm = <xsl:value-of select="../@projectName"/>.Domain;
using db = <xsl:value-of select="../@projectName"/>.Data.Sql.Models;
using <xsl:value-of select="../@projectName"/>.Common;
using <xsl:value-of select="../@projectName"/>.Common.Exceptions;
using <xsl:value-of select="../@projectName"/>.Domain;
using <xsl:value-of select="../@projectName"/>.Data.Sql;
using <xsl:value-of select="../@projectName"/>.Primary.Business.Synchronization;
using <xsl:value-of select="../@projectName"/>.SDK;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Direct.Implementation
{
    // WARNING: THIS FILE IS GENERATED
    public partial class <xsl:value-of select="@name"/>Business : BusinessBase, I<xsl:value-of select="@name"/>Business, INestedOperation&lt;db.<xsl:value-of select="@name"/>, dm.<xsl:value-of select="@name"/>&gt;
    {
        public <xsl:value-of select="@name"/>Business(IFoundation foundation)
            : base(foundation, "<xsl:value-of select="@name"/>")
        {
        }
        
        <xsl:if test="@useIndex='true' or @useStore='true'">public I<xsl:value-of select="@name"/>Synchronizer Synchronizer
        {
            get
            {
                return this.IFoundation.Resolve&lt;I<xsl:value-of select="@name"/>Synchronizer&gt;();
            }
        }</xsl:if>
        <xsl:if test="string-length(@indexAgent)>0">
        public override string DefaultAgent
        {
            get
            {
                return Daemons.Agents.<xsl:value-of select="@indexAgent"/>;
            }
        }</xsl:if>

        public <xsl:value-of select="@name"/> Insert(<xsl:value-of select="@name"/> insert<xsl:value-of select="@name"/>)
        {
            return this.Insert(insert<xsl:value-of select="@name"/>, Availability.<xsl:choose><xsl:when test="@indexForSearchable='true'">Searchable</xsl:when><xsl:otherwise>Retrievable</xsl:otherwise></xsl:choose>);
        }
        public <xsl:value-of select="@name"/> Insert(<xsl:value-of select="@name"/> insert<xsl:value-of select="@name"/>, Availability availability)
        {
            return base.ExecuteFunction("Insert", delegate()
            {
                using (var database = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']">insert<xsl:value-of select="../@name"/>.<xsl:value-of select="text()" /></xsl:for-each>))
                {
                    <xsl:for-each select="field[string-length(@priorityGroupBy)>0]">
                    <xsl:if test="string-length(@priorityFilterOn)>0">
                    if(insert<xsl:value-of select="../@name"/>.<xsl:value-of select="@priorityFilterOn"/> == true)
                    {
                        </xsl:if>insert<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/> = (from o in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                                                     where <xsl:if test="@priorityGroupBy!='self'">o.<xsl:value-of select="@priorityGroupBy"/> == insert<xsl:value-of select="../@name"/>.<xsl:value-of select="@priorityGroupBy"/></xsl:if><xsl:if test="not(@priorityIncludeDeleted='true')">
                                                     <xsl:if test="../@useIndex='true' or ../@useStore='true'"><xsl:if test="@priorityGroupBy!='self'">
                                                     &amp;&amp; </xsl:if>o.deleted_utc == null</xsl:if></xsl:if><xsl:if test="string-length(@priorityFilterOn)>0">
                                                     &amp;&amp; o.<xsl:value-of select="@priorityFilterOn"/> == true</xsl:if>
                                                     select o.<xsl:value-of select="../field[1]"/>).Count() + 1;
                    <xsl:if test="string-length(@priorityFilterOn)>0">
                    }
                    </xsl:if>
                    </xsl:for-each>

                    this.PreProcess(insert<xsl:value-of select="@name"/>, Crud.Insert);
                    this.Validate(insert<xsl:value-of select="@name"/>, Crud.Insert);
                    var interception = this.Intercept(insert<xsl:value-of select="@name"/>, Crud.Insert);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    if (insert<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]"/> == Guid.Empty)
                    {
                        insert<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]"/> = Guid.NewGuid();
                    }
                    <xsl:if test="@useIndex='true' or @useStore='true' or @trackUpdates='true'">if(insert<xsl:value-of select="@name"/>.created_utc == default(DateTime))
                    {
                        insert<xsl:value-of select="@name"/>.created_utc = DateTime.UtcNow;
                    }
                    if(insert<xsl:value-of select="@name"/>.updated_utc == default(DateTime))
                    {
                        insert<xsl:value-of select="@name"/>.updated_utc = insert<xsl:value-of select="@name"/>.created_utc;
                    }</xsl:if>

                    db.<xsl:value-of select="@name"/> dbModel = insert<xsl:value-of select="@name"/>.ToDbModel();
                    
                    <xsl:if test="@useIndex='true' or @useStore='true'">dbModel.InvalidateSync(this.DefaultAgent, "insert");</xsl:if>

                    database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Add(dbModel);
                    
                    <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                    // Cascade to <xsl:value-of select="@foreignKey" /> 
                    this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeInvalidate(database, dbModel.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>); 
                    </xsl:for-each>
                    database.SaveChanges();

                    <xsl:if test="@tenant='Route'">
                    // Insert Isolate
                    using (var dbIsolated = base.CreateSQLIsolatedContext(<xsl:for-each select="field[1]">dbModel.<xsl:value-of select="text()"/></xsl:for-each>))
                    {
                        db.<xsl:value-of select="@name"/> match = (from n in dbIsolated.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                where n.<xsl:value-of select="field[1]"/> == dbModel.<xsl:value-of select="field[1]"/>
                                select n).FirstOrDefault();

                        if (match != null)
                        {
                            match = insert<xsl:value-of select="@name"/>.ToDbModel(match);
                            dbIsolated.SaveChanges();
                        }
                    }
                    </xsl:if>

                    <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                    // Cascade to <xsl:value-of select="@foreignKey" />
                    this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeCompute(database, dbModel.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>); 
                    </xsl:for-each>

                    <xsl:if test="../@trackChanges='true' and not(@ignoreChanges='true')">
                    this.API.Integration.TrackHistory.TrackInsert(dbModel.ToDomainModel());</xsl:if>
                    
                    this.AfterInsertPersisted(database, dbModel, insert<xsl:value-of select="@name"/>);
                    
                    <xsl:if test="@useIndex='true' or @useStore='true'">this.Synchronizer.SynchronizeItem(new IdentityInfo(dbModel.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, dbModel.<xsl:value-of select="text()"/></xsl:for-each>), availability);
                    this.AfterInsertIndexed(database, dbModel, insert<xsl:value-of select="@name"/>);
                    </xsl:if>

                    <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                    // Cascade to <xsl:value-of select="@foreignKey" />
                    this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeSynchronize(dbModel.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>, availability);
                    </xsl:for-each>
                    this.DependencyCoordinator.<xsl:value-of select="@name"/>Invalidated(Dependency.none, dbModel.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">, dbModel.<xsl:value-of select="text()"/></xsl:for-each>);
                }
                return this.GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">insert<xsl:value-of select="../@name"/><xsl:text>.</xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>insert<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]"/>);
            });
        }
        public <xsl:value-of select="@name"/> Update(<xsl:value-of select="@name"/> update<xsl:value-of select="@name"/>)
        {
            return this.Update(update<xsl:value-of select="@name"/>, Availability.<xsl:choose><xsl:when test="@indexForSearchable='true'">Searchable</xsl:when><xsl:otherwise>Retrievable</xsl:otherwise></xsl:choose>);
        }
        public <xsl:value-of select="@name"/> Update(<xsl:value-of select="@name"/> update<xsl:value-of select="@name"/>, Availability availability)
        {
            return base.ExecuteFunction("Update", delegate()
            {
                using (var database = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']">update<xsl:value-of select="../@name"/>.<xsl:value-of select="text()" /></xsl:for-each>))
                {
                    this.PreProcess(update<xsl:value-of select="@name"/>, Crud.Update);
                    this.Validate(update<xsl:value-of select="@name"/>, Crud.Update);
                    var interception = this.Intercept(update<xsl:value-of select="@name"/>, Crud.Update);
                    if(interception.Intercepted)
                    {
                        return interception.ReturnEntity;
                    }
                    
                    <xsl:if test="@useIndex='true' or @trackUpdates='true' or @useStore='true'">update<xsl:value-of select="@name"/>.updated_utc = DateTime.UtcNow;</xsl:if>
                    
                    db.<xsl:value-of select="@name"/> found = (from n in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where n.<xsl:value-of select="field[1]"/> == update<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]"/>
                                    select n).FirstOrDefault();

                    if (found != null)
                    {
                        <xsl:value-of select="@name"/> previous = found.ToDomainModel();
                        <xsl:for-each select="field[string-length(@priorityGroupBy)>0]">
                        update<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/> = found.<xsl:value-of select="text()"/>;// prevent priority update
                        </xsl:for-each>
                        found = update<xsl:value-of select="@name"/>.ToDbModel(found);
                        <xsl:if test="@useIndex='true' or @useStore='true'">found.InvalidateSync(this.DefaultAgent, "updated");</xsl:if>

                        <xsl:for-each select="../item/field[@foreignKey=$currentEntityName and @iExtendForeignKey='true']">
                        // Cascade to <xsl:value-of select="../@name" />
                        db<xsl:value-of select="../@name" /> found<xsl:value-of select="../@name" /> = (from n in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                                    where n.<xsl:value-of select="text()"/> == found.<xsl:value-of select="@foreignKeyField"/>
                                    select n).FirstOrDefault();
                        if(found<xsl:value-of select="../@name" /> != null)
                        {
                            found<xsl:value-of select="../@name" />.InvalidateSync(this.DefaultAgent, "updated");
                        }
                        </xsl:for-each>

                        <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                        this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeInvalidate(database, found.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>); // Cascade to <xsl:value-of select="@foreignKey" />
                        </xsl:for-each>
                        database.SaveChanges();

                        <xsl:if test="@tenant='Route'">
                        // Update Isolate
                        using (var dbIsolated = base.CreateSQLIsolatedContext(<xsl:for-each select="field[1]">found.<xsl:value-of select="text()"/></xsl:for-each>))
                        {
                            db.<xsl:value-of select="@name"/> match = (from n in dbIsolated.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where n.<xsl:value-of select="field[1]"/> == found.<xsl:value-of select="field[1]"/>
                                    select n).FirstOrDefault();

                            if (match != null)
                            {
                                match = update<xsl:value-of select="@name"/>.ToDbModel(match);
                                dbIsolated.SaveChanges();
                            }
                        }
                        </xsl:if>

                        <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                        this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeCompute(database, found.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>); // Cascade to <xsl:value-of select="@foreignKey" />
                        </xsl:for-each>
                        <xsl:if test="../@trackChanges='true' and not(@ignoreChanges='true')">
                        this.API.Integration.TrackHistory.TrackUpdate(previous, found.ToDomainModel());</xsl:if>

                        this.AfterUpdatePersisted(database, found, update<xsl:value-of select="@name"/>, previous);
                        
                        <xsl:if test="@useIndex='true' or @useStore='true'">this.Synchronizer.SynchronizeItem(new IdentityInfo(found.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, found.<xsl:value-of select="text()"/></xsl:for-each>), Availability.<xsl:choose><xsl:when test="@indexForSearchable='true'">Searchable</xsl:when><xsl:otherwise>Retrievable</xsl:otherwise></xsl:choose>);
                        this.AfterUpdateIndexed(database, found);
                        <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                        this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeSynchronize(found.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>, availability);// Cascade to <xsl:value-of select="@foreignKey" />
                        </xsl:for-each>
                        </xsl:if>
                        this.DependencyCoordinator.<xsl:value-of select="@name"/>Invalidated(Dependency.none, found.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">, found.<xsl:value-of select="text()"/></xsl:for-each>);
                    
                    }
                    
                    return this.GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">update<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>, </xsl:for-each>update<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]"/>);
                }
            });
        }
        <xsl:for-each select="field[string-length(@priorityGroupBy)>0]"><xsl:variable name="priorityGroupBy"><xsl:value-of select="@priorityGroupBy"/></xsl:variable>
        public void Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true'  and not(@isolated='true') and text()!=$priorityGroupBy]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="@priorityGroupBy!='self'">Guid <xsl:value-of select="@priorityGroupBy"/>, </xsl:if>Guid <xsl:value-of select="../field[1]"/>, int priority)
        {
            base.ExecuteMethod("Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>", delegate ()
            {
                List&lt;IdentityInfo&gt; changedList = new List&lt;IdentityInfo&gt;();
                using (var database = base.CreateSQL<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared'">Shared</xsl:if>Context(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="text()" /></xsl:for-each>))
                {
                    List&lt;db.<xsl:value-of select="../@name"/>&gt; ordered = (from f in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                                                      where <xsl:if test="@priorityGroupBy!='self'">f.<xsl:value-of select="@priorityGroupBy"/> == <xsl:value-of select="@priorityGroupBy"/></xsl:if><xsl:if test="not(@priorityIncludeDeleted='true')">
                                                      &amp;&amp; f.deleted_utc == null</xsl:if><xsl:if test="string-length(@priorityFilterOn)>0">
                                                      &amp;&amp; f.<xsl:value-of select="@priorityFilterOn"/> == true</xsl:if>
                                                      orderby f.<xsl:value-of select="text()"/>
                                                      select f).ToList();


                    db.<xsl:value-of select="../@name"/> match = ordered.Where(x =&gt; x.<xsl:value-of select="../field[1]"/> == <xsl:value-of select="../field[1]"/>).FirstOrDefault();
                    
                    if (match != null &amp;&amp; match.<xsl:value-of select="text()"/> != priority)
                    {
                        ordered.Remove(match);
                        bool added = false;

                        for (int i = 0; i &lt; ordered.Count; i++)
                        {
                            if (priority &lt;= ordered[i].<xsl:value-of select="text()"/>)
                            {
                                if (match.<xsl:value-of select="text()"/> &gt; priority)
                                {
                                    // moving up, so insert before
                                    ordered.Insert(i, match);
                                }
                                else
                                {
                                    // moving down, so insert after [assumes we have a contiguous list]
                                    if (ordered.Count &gt; i + 1)
                                    {
                                        ordered.Insert(i + 1, match);
                                    }
                                    else
                                    {
                                        ordered.Add(match);
                                    }
                                }
                                added = true;
                                break;
                            }
                        }
                        if (!added)
                        {
                            ordered.Add(match);
                        }
                    }
                    for (int i = 0; i &lt; ordered.Count; i++)
                    {
                        int newPriority = i + 1;
                        if (ordered[i].<xsl:value-of select="text()"/> != newPriority)
                        {
                            ordered[i].<xsl:value-of select="text()"/> = i + 1;
                            ordered[i].updated_utc = DateTime.UtcNow;
                            ordered[i].InvalidateSync(DefaultAgent, "Sort");
                            changedList.Add(new IdentityInfo(ordered[i].<xsl:value-of select="../field[1]"/><xsl:for-each select="../field[@tenant='true']">, ordered[i].<xsl:value-of select="text()" /></xsl:for-each>));
                        }
                    }
                    database.SaveChanges();
                }

                // outside of scope
                foreach (IdentityInfo item in changedList)
                {
                    this.Synchronizer.SynchronizeItem(item, Availability.Retrievable);
                }
                if(changedList.Count &gt; 0)
                {
                    // do the last one again only for sync time
                    this.Synchronizer.SynchronizeItem(changedList[0], Availability.Searchable);
                }
                foreach (IdentityInfo item in changedList)
                {
                    this.DependencyCoordinator.<xsl:value-of select="../@name"/>Invalidated(Dependency.none, item.primary_key<xsl:for-each select="../field[@tenant='true' and not(@isolated='true')]">, item.route_id.GetValueOrDefault()</xsl:for-each>);
                }
                this.After<xsl:value-of select="@friendlyName"/>Updated(<xsl:value-of select="@priorityGroupBy"/>, <xsl:value-of select="../field[1]"/>);
            });
        }

        partial void After<xsl:value-of select="@friendlyName"/>Updated(Guid <xsl:value-of select="@priorityGroupBy"/>, Guid <xsl:value-of select="../field[1]"/>);
        
        </xsl:for-each>
        <xsl:choose><xsl:when test="@removePattern='true'">public void Delete(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>)
        {
            base.ExecuteMethod("Delete", delegate ()
            {
                <xsl:for-each select="field[string-length(@priorityGroupBy)>0 and not(@priorityIncludeDeleted='true')]"><xsl:variable name="priorityGroupBy"><xsl:value-of select="@priorityGroupBy"/></xsl:variable>
                this.Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true'  and not(@isolated='true') and text() != $priorityGroupBy]"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="../field[1]"/>, int.MaxValue); // move to end
                </xsl:for-each>
                using (var database = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context())
                {
                    db.<xsl:value-of select="@name"/> found = (from a in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                           where a.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>
                                           select a).FirstOrDefault();

                    this.PreProcess(found.ToDomainModel(), Crud.Delete);

                    var interception = this.Intercept(found.ToDomainModel(), Crud.Delete);
                    if (interception.Intercepted)
                    {
                        return;
                    }
                    if (found != null &amp;&amp; !found.removed)
                    {
                        found.removed = true;
                        found.removed_utc = DateTime.UtcNow;
                        <xsl:if test="@useIndex='true' or @useStore='true'">found.InvalidateSync(this.DefaultAgent, "deleted");</xsl:if>
                        <xsl:for-each select="../item/field[@foreignKey=$currentEntityName and @iExtendForeignKey='true']">
                        
                        // Cascade to <xsl:value-of select="../@name" />
                        db.<xsl:value-of select="../@name" /> found<xsl:value-of select="../@name" /> = (from n in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                                    where n.<xsl:value-of select="@foreignKeyField"/> == found.<xsl:value-of select="text()"/>
                                    select n).FirstOrDefault();
                        found<xsl:value-of select="../@name" />.InvalidateSync(this.DefaultAgent, "updated");
                        </xsl:for-each>
                        database.SaveChanges();

                        <xsl:if test="@tenant='Route'">
                        // Delete From Isolate
                        using (var dbIsolated = base.CreateSQLIsolatedContext(<xsl:for-each select="field[1]">found.<xsl:value-of select="text()"/></xsl:for-each>))
                        {
                            db.<xsl:value-of select="@name"/> match = (from a in dbIsolated.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                           where a.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>
                                           select a).FirstOrDefault();

                            if (match != null)
                            {
                                match.removed = true;
                                match.removed_utc = DateTime.UtcNow;
                                dbIsolated.SaveChanges();
                            }
                        }
                        </xsl:if>

                        <xsl:if test="../@trackChanges='true' and not(@ignoreChanges='true')">
                        this.API.Integration.TrackHistory.TrackDelete(found.ToDomainModel());</xsl:if>
                        
                        this.AfterDeletePersisted(database, found);

                        <xsl:if test="@useIndex='true' or @useStore='true'">this.Synchronizer.SynchronizeItem(new IdentityInfo(found.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, found.<xsl:value-of select="text()" /></xsl:for-each>), Availability.<xsl:choose><xsl:when test="@indexForSearchable='true'">Searchable</xsl:when><xsl:otherwise>Retrievable</xsl:otherwise></xsl:choose>);
                        this.AfterDeleteIndexed(database, found);
                        </xsl:if>
                        this.DependencyCoordinator.<xsl:value-of select="@name"/>Invalidated(Dependency.none, found.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">, found.<xsl:value-of select="text()"/></xsl:for-each>);
                    }
                }
            });
        }</xsl:when>
        <xsl:otherwise>public void Delete(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>)
        {
            base.ExecuteMethod("Delete", delegate()
            {
                
                using (var database = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()" /></xsl:for-each>))
                {
                    db.<xsl:value-of select="@name"/> found = (from a in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where a.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>
                                    select a).FirstOrDefault();

                    if (found != null)
                    {
                        
                        <xsl:for-each select="field[string-length(@priorityGroupBy)>0 and not(@priorityIncludeDeleted='true')]"><xsl:variable name="priorityGroupBy"><xsl:value-of select="@priorityGroupBy"/></xsl:variable>
                        this.Update<xsl:value-of select="../@name"/><xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true' and text() != $priorityGroupBy]"><xsl:value-of select="text()"/>, </xsl:for-each>found.<xsl:value-of select="@priorityGroupBy"/>, <xsl:value-of select="../field[1]"/>, int.MaxValue); // move to end
                        </xsl:for-each>
                        <xsl:if test="count(field[string-length(@priorityGroupBy)>0 and not(@priorityIncludeDeleted='true')])>0">
                        //re-retrieve
                        found = (from a in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where a.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>
                                    select a).FirstOrDefault();
                                    
                        </xsl:if>

                        <xsl:choose><xsl:when test="@useIndex='true' or @useStore='true'">found.deleted_utc = DateTime.UtcNow;
                        found.InvalidateSync(this.DefaultAgent, "deleted");</xsl:when>
                        <xsl:otherwise>database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Remove(found);
                        </xsl:otherwise>
                        </xsl:choose>
                        <xsl:for-each select="../item/field[@foreignKey=$currentEntityName and @iExtendForeignKey='true']">
                        
                        // Cascade to <xsl:value-of select="../@name" />
                        db.<xsl:value-of select="../@name" /> found<xsl:value-of select="../@name" /> = (from n in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                                    where n.<xsl:value-of select="text()"/> == found.<xsl:value-of select="@foreignKeyField"/>
                                    select n).FirstOrDefault();
                        if(found<xsl:value-of select="../@name" /> != null)
                        {
                            found<xsl:value-of select="../@name" />.deleted_utc = found.deleted_utc;
                            found<xsl:value-of select="../@name" />.InvalidateSync(this.DefaultAgent, "deleted");
                        }
                        </xsl:for-each>

                        <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                        this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeInvalidate(database, found.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>); // Cascade to <xsl:value-of select="@foreignKey" />
                        </xsl:for-each>

                        database.SaveChanges();

                        <xsl:if test="@tenant='Route'">
                        // Delete From Isolate
                        using (var dbIsolated = base.CreateSQLIsolatedContext(<xsl:for-each select="field[1]">found.<xsl:value-of select="text()"/></xsl:for-each>))
                        {
                            db.<xsl:value-of select="@name"/> match = (from a in dbIsolated.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where a.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>
                                    select a).FirstOrDefault();

                            if (match != null)
                            {
                                <xsl:choose><xsl:when test="@useIndex='true' or @useStore='true'">match.deleted_utc = DateTime.UtcNow;
                                match.InvalidateSync(this.DefaultAgent, "deleted");</xsl:when>
                                <xsl:otherwise>dbIsolated.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Remove(found);
                                </xsl:otherwise>
                                </xsl:choose>
                                dbIsolated.SaveChanges();
                            }
                        }
                        </xsl:if>
                        <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                        this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeCompute(database, found.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>); // Cascade to <xsl:value-of select="@foreignKey" />
                        </xsl:for-each>

                        <xsl:if test="../@trackChanges='true' and not(@ignoreChanges='true')">
                        this.API.Integration.TrackHistory.TrackDelete(found.ToDomainModel());</xsl:if>
                        
                        this.AfterDeletePersisted(database, found);
                        
                        <xsl:if test="@useIndex='true' or @useStore='true'">this.Synchronizer.SynchronizeItem(new IdentityInfo(found.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, found.<xsl:value-of select="text()" /></xsl:for-each>), Availability.<xsl:choose><xsl:when test="@indexForSearchable='true'">Searchable</xsl:when><xsl:otherwise>Retrievable</xsl:otherwise></xsl:choose>);
                        this.AfterDeleteIndexed(database, found);
                        <xsl:for-each select="field[@foreignKeyComputesMe='true']">
                        this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeSynchronize(found.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.GetValueOrDefault()</xsl:if>, Availability.<xsl:choose><xsl:when test="../@indexForSearchable='true'">Searchable</xsl:when><xsl:otherwise>Retrievable</xsl:otherwise></xsl:choose>);// Cascade to <xsl:value-of select="@foreignKey" />
                        </xsl:for-each>
                        </xsl:if>
                        this.DependencyCoordinator.<xsl:value-of select="@name"/>Invalidated(Dependency.none, found.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">, found.<xsl:value-of select="text()"/></xsl:for-each>);
                    }
                }
            });
        }
        </xsl:otherwise>
        </xsl:choose>

        
        <xsl:if test="count(../item/field[@foreignKey=$currentEntityName and @foreignKeyComputesMe='true'])>0">
        
        public void CascadeInvalidate(<xsl:value-of select="../@projectName"/>Context database, Guid <xsl:value-of select="field[1]/text()"/>, HashSet&lt;string&gt; chain = null)
        {
            base.ExecuteMethod("CascadeCalculate", delegate ()
            {
                if(chain == null) { chain = new HashSet&lt;string&gt;(); }

                // invalidate self
                db.<xsl:value-of select="@name" /> found = (from n in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            where n.<xsl:value-of select="field[1]/text()"/> == <xsl:value-of select="field[1]/text()"/>
                            select n).FirstOrDefault();
                
                if(found != null)
                {
                    found.InvalidateSync(this.DefaultAgent, "updated");
                }

                if(!chain.Add("<xsl:value-of select="@name"/>"))
                {
                    return;// no circular references
                }

                <xsl:if test="count(field[@foreignKeyComputesMe='true'])>0">
                if (found != null)
                {
                    <xsl:for-each select="field[@foreignKeyComputesMe='true']">// Cascade To <xsl:value-of select="@foreignKey"/>
                    this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeInvalidate(db, found.<xsl:value-of select="text()"/>, chain);
                    </xsl:for-each>
                }
                </xsl:if>
             });
        }
        public void CascadeCompute(<xsl:value-of select="../@projectName"/>Context database, Guid <xsl:value-of select="field[1]/text()"/>, HashSet&lt;string&gt; chain = null)
        {
            base.ExecuteMethod("CascadeCompute", delegate ()
            {
                if(chain == null) { chain = new HashSet&lt;string&gt;(); }

                // calculate self
                db.<xsl:value-of select="@name" /> found = (from n in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            where n.<xsl:value-of select="field[1]/text()"/> == <xsl:value-of select="field[1]/text()"/>
                            select n).FirstOrDefault();
                
                if(found != null)
                {
                    this.PerformCascadeCompute(database, found);
                }

                if(!chain.Add("<xsl:value-of select="@name"/>"))
                {
                    return;// no circular references
                }

                <xsl:if test="count(field[@foreignKeyComputesMe='true'])>0">
                if (found != null)
                {
                    <xsl:for-each select="field[@foreignKeyComputesMe='true']">// Cascade To <xsl:value-of select="@foreignKey"/>
                    this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeCompute(db, found.<xsl:value-of select="text()"/>, chain);
                    </xsl:for-each>
                }
                </xsl:if>
            });
        }
        public void CascadeSynchronize(Guid <xsl:value-of select="field[1]/text()"/>, Availability availability, HashSet&lt;string&gt; chain = null)
        {
            base.ExecuteMethod("CascadeSynchronize", delegate ()
            {
                if(chain == null) { chain = new HashSet&lt;string&gt;(); }

                // sync self
                this.Synchronizer.SynchronizeItem(new IdentityInfo(<xsl:value-of select="field[1]/text()"/>), availability);

                if(!chain.Add("<xsl:value-of select="@name"/>"))
                {
                    return;// no circular references
                }

                <xsl:if test="count(field[@foreignKeyComputesMe='true'])>0">
                <xsl:value-of select="@name"/> found = this.GetById(<xsl:value-of select="field[1]/text()"/>);
                if (found != null)
                {
                    <xsl:for-each select="field[@foreignKeyComputesMe='true']">// Cascade To <xsl:value-of select="@foreignKey"/>
                    this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeSynchronize(found.<xsl:value-of select="text()"/>, availability, chain);
                    </xsl:for-each>
                }
                </xsl:if>
                
            });
        }
        partial void PerformCascadeCompute(<xsl:value-of select="../@projectName"/>Context database, db.<xsl:value-of select="@name"/> entity);

        </xsl:if>
        
        
        <xsl:if test="@useIndex='true' or @useStore='true'">public void SynchronizationUpdate(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdate", delegate ()
            {
                using (var db = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()" /></xsl:for-each>))
                {
                    if (success)
                    {
                        db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            .Where(x =&gt; (x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                                    &amp;&amp; ((x.sync_invalid_utc == null) || (x.sync_invalid_utc &lt;= sync_date_utc)))
                            .Update(x =&gt; new db.<xsl:value-of select="@name"/>()
                            {
                                sync_success_utc = sync_date_utc,
                                sync_attempt_utc = null,
                                sync_invalid_utc = null,
                                sync_agent = string.Empty,
                                sync_log = sync_log
                            });
                    }
                    else
                    {
                        db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            .Where(x =&gt; x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/> &amp;&amp; x.sync_success_utc == null)
                            .Update(x =&gt; new db.<xsl:value-of select="@name"/>()
                            {
                                sync_attempt_utc = DateTime.UtcNow,
                                sync_log = sync_log
                            });
                    }
                }
            });
        }
        <xsl:if test="@tenant='Route'">
        public void SynchronizationUpdateIsolated(<xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationUpdateIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(<xsl:value-of select="field[1]/text()"/>))
                {
                    if (success)
                    {
                        db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            .Where(x =&gt; (x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                                    &amp;&amp; ((x.sync_invalid_utc == null) || (x.sync_invalid_utc &lt;= sync_date_utc)))
                            .Update(x =&gt; new db.<xsl:value-of select="@name"/>()
                            {
                                sync_success_utc = sync_date_utc,
                                sync_attempt_utc = null,
                                sync_invalid_utc = null,
                                sync_agent = string.Empty,
                                sync_log = sync_log
                            });
                    }
                    else
                    {
                        db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            .Where(x =&gt; x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/> &amp;&amp; x.sync_success_utc == null)
                            .Update(x =&gt; new db.<xsl:value-of select="@name"/>()
                            {
                                sync_attempt_utc = DateTime.UtcNow,
                                sync_log = sync_log
                            });
                    }
                }
            });
        }
        </xsl:if>
        public List&lt;IdentityInfo&gt; SynchronizationGetInvalid(<xsl:for-each select="field[@tenant='true']">string tenant_code, </xsl:for-each>int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalid", delegate ()
            {
                using (var db = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']">tenant_code</xsl:for-each>))
                {
                    if(string.IsNullOrWhiteSpace(sync_agent))
                    {
                        var data = (from a in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where a.sync_success_utc == null
                                    &amp;&amp; ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, route_id = a.<xsl:value-of select="text()"/></xsl:for-each>});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where a.sync_success_utc == null
                                    &amp;&amp; (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, route_id = a.<xsl:value-of select="text()"/></xsl:for-each>});
                        return data.ToList();
                    }
                }
            });
        }
        <xsl:if test="@tenant='Route'">
        public List&lt;IdentityInfo&gt; SynchronizationGetInvalidIsolated(<xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>, int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationGetInvalidIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(<xsl:value-of select="field[1]/text()"/>))
                {
                    if(string.IsNullOrWhiteSpace(sync_agent))
                    {
                        var data = (from a in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where a.sync_success_utc == null
                                    &amp;&amp; ((a.sync_agent == null) || (a.sync_agent == ""))
                                    select new IdentityInfo() { primary_key = a.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, route_id = a.<xsl:value-of select="text()"/></xsl:for-each>});
                        return data.ToList();
                    }
                    else
                    {
                        var data = (from a in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where a.sync_success_utc == null
                                    &amp;&amp; (a.sync_agent == sync_agent)
                                    select new IdentityInfo() { primary_key = a.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, route_id = a.<xsl:value-of select="text()"/></xsl:for-each>});
                        return data.ToList();
                    }
                }
            });
        }
        </xsl:if>
        public void SynchronizationHydrateUpdate(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdate", delegate ()
            {
                using (var db = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/></xsl:for-each>))
                {
                    if (success)
                    {
                        db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            .Where(x =&gt; x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                            .Update(x =&gt; new db.<xsl:value-of select="@name"/>()
                            {
                                sync_hydrate_utc = sync_date_utc
                            });
                    }
                    else
                    {
                        db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            .Where(x =&gt; x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                            .Update(x =&gt; new db.<xsl:value-of select="@name"/>()
                            {
                                sync_hydrate_utc = null
                            });
                    }
                }
            });
        }
        <xsl:if test="@tenant='Route'">
        public void SynchronizationHydrateUpdateIsolated(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>, bool success, DateTime sync_date_utc, string sync_log)
        {
            base.ExecuteMethod("SynchronizationHydrateUpdateIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(<xsl:value-of select="field[1]"/>))
                {
                    if (success)
                    {
                        db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            .Where(x =&gt; x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                            .Update(x =&gt; new db.<xsl:value-of select="@name"/>()
                            {
                                sync_hydrate_utc = sync_date_utc
                            });
                    }
                    else
                    {
                        db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                            .Where(x =&gt; x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                            .Update(x =&gt; new db.<xsl:value-of select="@name"/>()
                            {
                                sync_hydrate_utc = null
                            });
                    }
                }
            });
        }
        </xsl:if>
        public List&lt;IdentityInfo&gt; SynchronizationHydrateGetInvalid(<xsl:for-each select="field[@tenant='true']">string tenant_code, </xsl:for-each>int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalid", delegate ()
            {
                using (var db = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']">tenant_code</xsl:for-each>))
                {
                    var data = (from a in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, route_id = a.<xsl:value-of select="text()"/></xsl:for-each>});
                    return data.ToList();
                }
            });
        }
        <xsl:if test="@tenant='Route'">
        public List&lt;IdentityInfo&gt; SynchronizationHydrateGetInvalidIsolated(<xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>, int retryPriorityThreshold, string sync_agent)
        {
            return base.ExecuteFunction("SynchronizationHydrateGetInvalidIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(<xsl:value-of select="field[1]/text()"/>))
                {
                    var data = (from a in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                where a.sync_hydrate_utc == null
                                select new IdentityInfo() { primary_key = a.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, route_id = a.<xsl:value-of select="text()"/></xsl:for-each>});
                    return data.ToList();
                }
            });
        }
        </xsl:if>
        </xsl:if>
        public <xsl:value-of select="@name"/> GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>)
        {
            return base.ExecuteFunction("GetById", delegate()
            {
                using (var db = this.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/></xsl:for-each>))
                {
                    db.<xsl:value-of select="@name"/> result = (from n in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                     where (n.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                                     select n).FirstOrDefault();
                    return result.ToDomainModel();
                }
            });
        }
        <xsl:variable name="removePattern"><xsl:value-of select="@removePattern"/></xsl:variable>
        <xsl:for-each select="field[@foreignKey and not(@noGet='true')]"><xsl:variable name="currentForeign"><xsl:value-of select="text()"/></xsl:variable>public List&lt;<xsl:value-of select="../@name"/>&gt; GetBy<xsl:value-of select="@friendlyName" />(<xsl:for-each select="../field[@tenant='true' and text() != $currentForeign]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="@foreignKeyField"/><xsl:if test="$removePattern='true'">, bool includeRemoved</xsl:if>)
        {
            return base.ExecuteFunction("GetBy<xsl:value-of select="@friendlyName" />", delegate()
            {
                using (var db = this.CreateSQL<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared' or ../@tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="text()"/></xsl:for-each>))
                {
                    var result = (from n in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                                     where (n.<xsl:value-of select="text()" /> == <xsl:value-of select="@foreignKeyField"/>)<xsl:if test="$removePattern='true'">
                                     &amp;&amp; (includeRemoved || n.removed == false)</xsl:if><xsl:if test="string-length(../@uiDefaultSort)>0">
                                     orderby n.<xsl:value-of select="../@uiDefaultSort" /></xsl:if>
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        
        <xsl:if test="@foreignKeyInvalidatesMe='true'">
        public void InvalidateFor<xsl:value-of select="@friendlyName" />(<xsl:for-each select="../field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="@foreignKeyField"/>, string reason)
        {
            base.ExecuteMethod("InvalidateFor<xsl:value-of select="@friendlyName" />", delegate ()
            {
                using (var database = base.CreateSQL<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="text()"/></xsl:for-each>))
                {
                    database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                        .Where(x => x.<xsl:value-of select="text()"/> == <xsl:value-of select="@foreignKeyField"/>)
                        .Update(x => new db.<xsl:value-of select="../@name"/>() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                    <xsl:variable name="selfName"><xsl:value-of select="../@name"/></xsl:variable>
                    <xsl:variable name="selfProperty"><xsl:value-of select="text()"/></xsl:variable>
                    <xsl:variable name="paramName"><xsl:value-of select="@foreignKeyField"/></xsl:variable>
                    <xsl:for-each select="../../item/field[@foreignKey=$selfName and @foreignKeyInvalidatesMe='true']">
                     database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                        .Where(x => x.<xsl:value-of select="$selfName"/>.<xsl:value-of select="$selfProperty"/> == <xsl:value-of select="$paramName"/>)
                        .Update(x => new db.<xsl:value-of select="../@name"/>()
                        {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                    </xsl:for-each>
                }
            });
        }
        </xsl:if>
        </xsl:for-each>
        <xsl:for-each select="field[@lookup='true']">public List&lt;<xsl:value-of select="../@name"/>&gt; GetBy<xsl:call-template name="NoSpace"><xsl:with-param name="inputString" select="@friendlyName"/></xsl:call-template>(<xsl:value-of select="@type" /><xsl:text> </xsl:text><xsl:value-of select="text()"/>)
        {
            return base.ExecuteFunction("GetBy<xsl:call-template name="NoSpace"><xsl:with-param name="inputString" select="@friendlyName"/></xsl:call-template>", delegate()
            {
                using (var db = this.CreateSQL<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared'">Shared</xsl:if>Context())
                {
                    var result = (from n in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                                     where (n.<xsl:value-of select="text()" /> == <xsl:value-of select="text()" />)<xsl:if test="string-length(../@uiDefaultSort)>0">
                                     orderby n.<xsl:value-of select="../@uiDefaultSort" /></xsl:if>
                                     select n);
                    return result.ToDomainModel();
                }
            });
        }
        </xsl:for-each>
        <xsl:if test="@useIndex='true' or @useStore='true'">
        public void Invalidate(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>, string reason)
        {
            base.ExecuteMethod("Invalidate", delegate ()
            {
                using (var db = base.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/></xsl:for-each>))
                {
                    db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                        .Where(x => x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                        .Update(x => new db.<xsl:value-of select="@name"/>() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        <xsl:if test="@tenant='Route'">
        public void InvalidateIsolated(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]"/>, string reason)
        {
            base.ExecuteMethod("InvalidateIsolated", delegate ()
            {
                using (var db = base.CreateSQLIsolatedContext(<xsl:value-of select="field[1]/text()"/>))
                {
                    db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                        .Where(x => x.<xsl:value-of select="field[1]"/> == <xsl:value-of select="field[1]"/>)
                        .Update(x => new db.<xsl:value-of select="@name"/>() {
                            sync_success_utc = null,
                            sync_hydrate_utc = null,
                            sync_invalid_utc = DateTime.UtcNow,
                            sync_log = reason
                        });
                }
            });
        }
        </xsl:if>
        </xsl:if>

        <xsl:if test="count(field[@searchable='true' and string-length(@computedFrom)=0]) > 0">public List&lt;<xsl:value-of select="@name"/>&gt; Find(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>int skip, int take, string keyword = "", string order_by = "", bool descending = false<xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text> <xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction("Find", delegate()
            {
                using (var db = this.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/></xsl:for-each>))
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }

                    var data = (from p in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                where (keyword == "" <xsl:for-each select="field[@searchable='true' and string-length(@computedFrom)=0]">
                                    || p.<xsl:value-of select="text()"/>.Contains(keyword)
                                </xsl:for-each>)<xsl:for-each select="field[@foreignKey and not(@noGet='true')]">
                                &amp;&amp; (<xsl:if test="not(@tenant='true')"><xsl:value-of select="text()"/> == null || </xsl:if>p.<xsl:value-of select="text()"/> == <xsl:if test="@type!='int' and @dbType='int'">(int)</xsl:if><xsl:value-of select="text()"/><xsl:if test="not(@type='string')"></xsl:if>)
                                </xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">
                                &amp;&amp; (<xsl:if test="not(@tenant='true')"><xsl:value-of select="text()"/> == null || </xsl:if>p.<xsl:value-of select="text()"/> == <xsl:if test="@type!='int' and @dbType='int'">(int)</xsl:if><xsl:value-of select="text()"/><xsl:if test="not(@type='string')"></xsl:if>)
                                </xsl:for-each>
                                select p);

                    List&lt;db.<xsl:value-of select="@name"/>&gt; result = new List&lt;db.<xsl:value-of select="@name"/>&gt;();

                    switch (order_by)
                    {<xsl:for-each select="field[@sortable='true']">
                        case "<xsl:value-of select="text()"/>":
                            if (!descending)
                            {
                                result = data.OrderBy(s => s.<xsl:value-of select="text()"/>).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.<xsl:value-of select="text()"/>).Skip(skip).Take(take).ToList();
                            }
                            break;
                        </xsl:for-each>
                        default:
                            <xsl:if test="@uiDefaultSort">if (!descending)
                            {
                                result = data.OrderBy(s => s.<xsl:value-of select="@uiDefaultSort"></xsl:value-of>).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                result = data.OrderByDescending(s => s.<xsl:value-of select="@uiDefaultSort"></xsl:value-of>).Skip(skip).Take(take).ToList();
                            }
                            </xsl:if><xsl:if test="not(@uiDefaultSort)">result = data.OrderBy(s => s.<xsl:value-of select="field[1]"/>).Skip(skip).Take(take).ToList();</xsl:if>
                            break;
                    }
                    return result.ToDomainModel();
                }
            });
        }
        
        public int FindTotal(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>string keyword = ""<xsl:for-each select="field[@foreignKey and not(@noGet='true') and not(@tenant='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text> <xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction("FindTotal", delegate()
            {
                using (var db = this.CreateSQL<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Context(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/></xsl:for-each>))
                {
                    if(string.IsNullOrEmpty(keyword))
                    { 
                        keyword = ""; 
                    }
                    var data = (from p in db.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                where (keyword == "" <xsl:for-each select="field[@searchable='true' and @type!='Guid' and string-length(@computedFrom)=0]">
                                    || p.<xsl:value-of select="text()"/>.Contains(keyword)
                                </xsl:for-each>)<xsl:for-each select="field[@foreignKey and not(@noGet='true')]">
                                &amp;&amp; (<xsl:if test="not(@tenant='true')"><xsl:value-of select="text()"/> == null || </xsl:if>p.<xsl:value-of select="text()"/> == <xsl:if test="@type!='int' and @dbType='int'">(int)</xsl:if><xsl:value-of select="text()"/><xsl:if test="not(@type='string')"></xsl:if>)
                                </xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">
                                &amp;&amp; (<xsl:if test="not(@tenant='true')"><xsl:value-of select="text()"/> == null || </xsl:if>p.<xsl:value-of select="text()"/> == <xsl:if test="@type!='int' and @dbType='int'">(int)</xsl:if><xsl:value-of select="text()"/><xsl:if test="not(@type='string')"></xsl:if>)
                                </xsl:for-each>
                                select p).Count();

                    
                    return data;
                }
            });
        }</xsl:if>
        


        public virtual void Validate(<xsl:if test="@namespace='true'">dm.</xsl:if><xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>, Crud crud)
        {
            Dictionary&lt;string, LocalizableString&gt; errors = new Dictionary&lt;string, LocalizableString&gt;();
            <xsl:for-each select="field[@validate='length']">
            <xsl:variable name="stringlength"><xsl:call-template name="ExtractCharacterSize"><xsl:with-param name="text" select="@dbType" /></xsl:call-template></xsl:variable>
            <xsl:if test="@isNullable='false'">if(string.IsNullOrWhiteSpace(<xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" />))
            {
                errors["<xsl:value-of select="text()"/>"] = new LocalizableString(LocalizableString.SERVER, "<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>.Required", "<xsl:value-of select="@friendlyName"/> is required");
            }
            </xsl:if><xsl:if test="$stringlength != 'max'"><xsl:if test="@isNullable='false'">else </xsl:if>if(<xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" /> != null &amp;&amp; <xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" />.Length > <xsl:call-template name="ExtractCharacterSize">
                    <xsl:with-param name="text" select="@dbType" />
                </xsl:call-template>)
            {
                errors["<xsl:value-of select="text()"/>"] = new LocalizableString(LocalizableString.SERVER, "<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>.CharacterSize", "<xsl:value-of select="@friendlyName"/> must be <xsl:value-of select="$stringlength"/> characters or less");
            }
            </xsl:if>

            </xsl:for-each>

            <xsl:for-each select="field[@validate='email' and @isNullable='false']">if(string.IsNullOrWhiteSpace(<xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" />))
            {
                errors["<xsl:value-of select="text()"/>"] = new LocalizableString(LocalizableString.SERVER, "<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>.Required", "<xsl:value-of select="@friendlyName"/> is required");
            }
            else
            {
                string parsed_<xsl:value-of select="text()" /> = null;
                if(!SDKUtility.IsValidEmail(<xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" />, out parsed_<xsl:value-of select="text()" />))
                {
                    errors["<xsl:value-of select="text()"/>"] = new LocalizableString(LocalizableString.SERVER, "<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>.EmailInvalid", "<xsl:value-of select="@friendlyName"/> is not a valid e-mail address");
                }
                else
                {
                    <xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" /> = parsed_<xsl:value-of select="text()" />;
                }
            }
            </xsl:for-each>
            <xsl:for-each select="field[@validate='email' and @isNullable='true']">
            if(!string.IsNullOrWhiteSpace(<xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" />))
            {
                string parsed_<xsl:value-of select="text()" /> = null;
                if(!SDKUtility.IsValidEmail(<xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" />, out parsed_<xsl:value-of select="text()" />))
                {
                    errors["<xsl:value-of select="text()"/>"] = new LocalizableString(LocalizableString.SERVER, "<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>.EmailInvalid", "<xsl:value-of select="@friendlyName"/> is not a valid e-mail address");
                }
                else
                {
                    <xsl:value-of select="$name_lowered" />.<xsl:value-of select="text()" /> = parsed_<xsl:value-of select="text()" />;
                }
            }
            </xsl:for-each>

            this.ValidatePostProcess(<xsl:value-of select="$name_lowered" />, crud, errors);

            if(errors.Count > 0)
            {
                throw new UIException(LocalizableString.General_ValidationError(), errors);
            }
        }

        
        

        public NestedInsertInfo&lt;db.<xsl:value-of select="@name"/>, dm.<xsl:value-of select="@name"/>&gt; PrepareNestedInsert(<xsl:value-of select="../@projectName"/>Context database, dm.<xsl:value-of select="@name"/> insert<xsl:value-of select="@name"/>)
        {
            return base.ExecuteFunction("PrepareNestedInsert", delegate()
            {
                <xsl:for-each select="field[string-length(@priorityGroupBy)>0]">
                <xsl:if test="string-length(@priorityFilterOn)>0">
                if(insert<xsl:value-of select="../@name"/>.<xsl:value-of select="@priorityFilterOn"/> == true)
                {
                    </xsl:if>insert<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/> = (from o in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>
                                                     where <xsl:if test="@priorityGroupBy!='self'">o.<xsl:value-of select="@priorityGroupBy"/> == insert<xsl:value-of select="../@name"/>.<xsl:value-of select="@priorityGroupBy"/></xsl:if><xsl:if test="not(@priorityIncludeDeleted='true')">
                                                     <xsl:if test="@priorityGroupBy!='self'"> &amp;&amp;</xsl:if> o.deleted_utc == null</xsl:if><xsl:if test="string-length(@priorityFilterOn)>0">
                                                     &amp;&amp; o.<xsl:value-of select="@priorityFilterOn"/> == true</xsl:if>
                                                     select o.<xsl:value-of select="../field[1]"/>).Count() + 1;
                <xsl:if test="string-length(@priorityFilterOn)>0">
                }
                </xsl:if>
                </xsl:for-each>

                this.PreProcess(insert<xsl:value-of select="@name"/>, Crud.Insert);
                this.Validate(insert<xsl:value-of select="@name"/>, Crud.Insert);
                
                if (insert<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]"/> == Guid.Empty)
                {
                    insert<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]"/> = Guid.NewGuid();
                }
                <xsl:if test="@useIndex='true' or @useStore='true' or @trackUpdates='true'">insert<xsl:value-of select="@name"/>.created_utc = DateTime.UtcNow;
                insert<xsl:value-of select="@name"/>.updated_utc = insert<xsl:value-of select="@name"/>.created_utc;</xsl:if>

                db.<xsl:value-of select="@name"/> dbModel = insert<xsl:value-of select="@name"/>.ToDbModel();
                
                <xsl:if test="@useIndex='true' or @useStore='true'">dbModel.InvalidateSync(this.DefaultAgent, "insert");</xsl:if>

                database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.Add(dbModel);
                
                <xsl:for-each select="field[@foreignKeyComputesMe='true']">

                // Cascade to <xsl:value-of select="@foreignKey" />
                <xsl:if test="@isNullable='true'"><xsl:text>
                </xsl:text>if (dbModel.<xsl:value-of select="text()"/>.HasValue)
                {</xsl:if><xsl:text>
                </xsl:text><xsl:if test="@isNullable='true'"><xsl:text>    </xsl:text></xsl:if>this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeInvalidate(database, dbModel.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.Value</xsl:if>);<xsl:if test="@isNullable='true'">
                }</xsl:if>
                </xsl:for-each>
                return new NestedInsertInfo&lt;db.<xsl:value-of select="@name"/>, dm.<xsl:value-of select="@name"/>&gt;()
                { 
                    DbModel = dbModel,  
                    InsertModel = insert<xsl:value-of select="@name"/>
                };
            });
        }
        public dm.<xsl:value-of select="@name"/> FinalizeNestedInsert(<xsl:value-of select="../@projectName"/>Context database, NestedInsertInfo&lt;db.<xsl:value-of select="@name"/>, dm.<xsl:value-of select="@name"/>&gt; insertInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedInsert", delegate()
            {
                <xsl:if test="../@trackChanges='true' and not(@ignoreChanges='true')">
                this.API.Integration.TrackHistory.TrackInsert(insertInfo.DbModel.ToDomainModel());</xsl:if>
                
                this.AfterInsertPersisted(database, insertInfo.DbModel, insertInfo.InsertModel);
                
                <xsl:if test="@useIndex='true' or @useStore='true'">this.Synchronizer.SynchronizeItem(new IdentityInfo(insertInfo.DbModel.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, insertInfo.DbModel.<xsl:value-of select="text()" /></xsl:for-each>), availability);
                this.AfterInsertIndexed(database, insertInfo.DbModel, insertInfo.InsertModel);
                </xsl:if>

                <xsl:for-each select="field[@foreignKeyComputesMe='true']">

                // Cascade to <xsl:value-of select="@foreignKey" />
                <xsl:if test="@isNullable='true'"><xsl:text>
                </xsl:text>if (insertInfo.DbModel.<xsl:value-of select="text()"/>.HasValue)
                {</xsl:if><xsl:text>
                </xsl:text><xsl:if test="@isNullable='true'"><xsl:text>    </xsl:text></xsl:if>this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeSynchronize(insertInfo.DbModel.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.Value</xsl:if>, availability, insertInfo.Chain);<xsl:if test="@isNullable='true'">
                }</xsl:if>
                </xsl:for-each>
                this.DependencyCoordinator.<xsl:value-of select="@name"/>Invalidated(Dependency.none, insertInfo.DbModel.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">, insertInfo.DbModel.<xsl:value-of select="text()"/></xsl:for-each>);
            
                return this.GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">insertInfo.InsertModel.<xsl:value-of select="text()"/>, </xsl:for-each>insertInfo.InsertModel.<xsl:value-of select="field[1]"/>);
            });
        }
        public NestedUpdateInfo&lt;db.<xsl:value-of select="@name"/>, dm.<xsl:value-of select="@name"/>&gt; PrepareNestedUpdate(<xsl:value-of select="../@projectName"/>Context database, dm.<xsl:value-of select="@name"/> update<xsl:value-of select="@name"/>)
        {
            return base.ExecuteFunction("PrepareNestedUpdate", delegate ()
            {
                this.PreProcess(update<xsl:value-of select="@name"/>, Crud.Update);
                this.Validate(update<xsl:value-of select="@name"/>, Crud.Update);

                <xsl:if test="@useIndex='true' or @trackUpdates='true'">update<xsl:value-of select="@name"/>.updated_utc = DateTime.UtcNow;
                </xsl:if>
                db.<xsl:value-of select="@name"/> found = (from n in database.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
                                    where n.<xsl:value-of select="field[1]"/> == update<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]"/>
                                    select n).FirstOrDefault();

                if (found != null)
                {
                    dm.<xsl:value-of select="@name"/> previous = found.ToDomainModel();

                    found = update<xsl:value-of select="@name"/>.ToDbModel(found);
                    <xsl:if test="@useIndex='true' or @useStore='true'">found.InvalidateSync(this.DefaultAgent, "updated");</xsl:if>

                    return new NestedUpdateInfo&lt;db.<xsl:value-of select="@name"/>, dm.<xsl:value-of select="@name"/>&gt;()
                    {
                        DbModel = found,
                        PreviousModel = previous,
                        UpdateModel = update<xsl:value-of select="@name"/>
                    };
                }
                return null;

            });
        }
        public dm.<xsl:value-of select="@name"/> FinalizeNestedUpdate(<xsl:value-of select="../@projectName"/>Context database, NestedUpdateInfo&lt;db.<xsl:value-of select="@name"/>, dm.<xsl:value-of select="@name"/>&gt; updateInfo, Availability availability)
        {
            return base.ExecuteFunction("FinalizeNestedUpdate", delegate ()
            {
                <xsl:if test="../@trackChanges='true' and not(@ignoreChanges='true')">
                this.API.Integration.TrackHistory.TrackUpdate(updateInfo.PreviousModel, updateInfo.DbModel.ToDomainModel());</xsl:if>

                this.AfterUpdatePersisted(database, updateInfo.DbModel, updateInfo.UpdateModel, updateInfo.PreviousModel);

                <xsl:if test="@useIndex='true' or @useStore='true'">this.Synchronizer.SynchronizeItem(new IdentityInfo(updateInfo.DbModel.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true']">, updateInfo.DbModel.<xsl:value-of select="text()" /></xsl:for-each>), availability);
                this.AfterUpdateIndexed(database, updateInfo.DbModel);
                </xsl:if>

                <xsl:for-each select="field[@foreignKeyComputesMe='true']">

                // Cascade to <xsl:value-of select="@foreignKey" />
                <xsl:if test="@isNullable='true'"><xsl:text>
                </xsl:text>if (updateInfo.DbModel.<xsl:value-of select="text()"/>.HasValue)
                {</xsl:if><xsl:text>
                </xsl:text><xsl:if test="@isNullable='true'"><xsl:text>    </xsl:text></xsl:if>this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.CascadeSynchronize(updateInfo.DbModel.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.Value</xsl:if>, availability, updateInfo.Chain);<xsl:if test="@isNullable='true'">
                }</xsl:if>
                </xsl:for-each>
                this.DependencyCoordinator.<xsl:value-of select="@name"/>Invalidated(Dependency.none, updateInfo.DbModel.<xsl:value-of select="field[1]"/><xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">, updateInfo.DbModel.<xsl:value-of select="text()"/></xsl:for-each>);

                return this.GetById(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">updateInfo.UpdateModel.<xsl:value-of select="text()"/>, </xsl:for-each>updateInfo.DbModel.<xsl:value-of select="field[1]"/>);
            });
        }
        
        public InterceptArgs&lt;<xsl:value-of select="@name"/>&gt; Intercept(<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>, Crud crud)
        {
            InterceptArgs&lt;<xsl:value-of select="@name"/>&gt; args = new InterceptArgs&lt;<xsl:value-of select="@name"/>&gt;()
            {
                Crud = crud,
                ReturnEntity = <xsl:value-of select="$name_lowered"/>
            };
            this.PerformIntercept(args);
            return args;
        }

        partial void ValidatePostProcess(<xsl:if test="@namespace='true'">dm.</xsl:if><xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>, Crud crud, Dictionary&lt;string, LocalizableString&gt; errors);
        partial void PerformIntercept(InterceptArgs&lt;<xsl:value-of select="@name"/>&gt; args);
        partial void PreProcess(<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>, Crud crud);
        partial void AfterInsertPersisted(<xsl:value-of select="../@projectName"/>Context database, db.<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>, <xsl:value-of select="@name"/> insert<xsl:value-of select="@name"/>);
        partial void AfterUpdatePersisted(<xsl:value-of select="../@projectName"/>Context database, db.<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>, <xsl:value-of select="@name"/> update<xsl:value-of select="@name"/>, <xsl:value-of select="@name"/> previous);
        partial void AfterDeletePersisted(<xsl:value-of select="../@projectName"/>Context database, db.<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>);
        <xsl:if test="@useIndex='true' or @useStore='true'">partial void AfterUpdateIndexed(<xsl:value-of select="../@projectName"/>Context database, db.<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>);
        partial void AfterInsertIndexed(<xsl:value-of select="../@projectName"/>Context database, db.<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>, <xsl:value-of select="@name"/> insert<xsl:value-of select="@name"/>);
        partial void AfterDeleteIndexed(<xsl:value-of select="../@projectName"/>Context database, db.<xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>);</xsl:if>
    }
}

'''[ENDFILE]

</xsl:for-each>
  

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\<xsl:value-of select="items/@projectName"/>APIStore.cs]using <xsl:value-of select="items/@foundation"/>.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Store;

namespace <xsl:value-of select="items/@projectName"/>.Primary
{
    public partial class <xsl:value-of select="items/@projectName"/>APIStore : BaseClass
    {
        public <xsl:value-of select="items/@projectName"/>APIStore(IFoundation ifoundation)
            : base(ifoundation)
        {
        }
        <xsl:for-each select="items/item[@useStore='true']">public I<xsl:value-of select="@name" />Store <xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
        {
            get { return this.IFoundation.Resolve&lt;I<xsl:value-of select="@name" />Store&gt;(); }
        }
        </xsl:for-each>
    }
}


'''[ENDFILE]


<xsl:for-each select="items/item[@useStore='true']">
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Store\I<xsl:value-of select="@name"/>Store_Core.cs]using <xsl:value-of select="../@foundation"/>.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using <xsl:value-of select="../@projectName"/>.SDK;
using <xsl:value-of select="../@projectName"/>.SDK.Models;
using sdk = <xsl:value-of select="../@projectName"/>.SDK.Models;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Store
{
    public partial interface I<xsl:value-of select="@name"/>Store
    {
        <xsl:choose><xsl:when test="count(field[@storePartitionKey='true'])>0"><xsl:if test="count(field[@storePartitionKey='true' and not(@tenant='true')])>0">
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        Task&lt;<xsl:value-of select="@name"/>&gt; GetDocumentAsync(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>);
        </xsl:if></xsl:when><xsl:otherwise>
        Task&lt;<xsl:value-of select="@name"/>&gt; GetDocumentAsync(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true') and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:for-each select="field[@storePartitionKey='true']"> <xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>);
        </xsl:otherwise></xsl:choose>
        <xsl:for-each select="field[@storePartitionKey='true']">
        Task&lt;<xsl:value-of select="../@name"/>&gt; GetDocumentAsync(<xsl:if test="not(@tenant='true')"><xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each></xsl:if><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/><xsl:if test="not(../field[1]/@isolated='true')">, <xsl:value-of select="../field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="../field[1]/text()"/></xsl:if>);
        </xsl:for-each>
        Task&lt;bool&gt; CreateDocumentAsync(<xsl:value-of select="@name"/> model);
        Task&lt;bool&gt; DeleteDocumentAsync(<xsl:value-of select="@name"/> model);
        
        <xsl:if test="count(field[string-length(@storePartitionKey)>0])>0">
        <xsl:if test="count(field[@storePartitionKey='Self'])>0 or count(field[@storePartitionKey='SplitID'])>0">[Obsolete("Use caution, this is expensive for multi-partition tables", false)]</xsl:if>
        Task&lt;ListResult&lt;<xsl:value-of select="@name"/>&gt;&gt; <xsl:choose><xsl:when test="count(field[@storePartitionKey='Global' or @storePartitionKey='Self' or @storePartitionKey='SplitID'])>0">Find</xsl:when><xsl:otherwise>FindFor<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:otherwise></xsl:choose>Async(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(field[@storePartitionKey='true'])>0"><xsl:value-of select="field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>int skip, int take, string order_by = "", bool descending = false<xsl:if test="count(field[@searchable='true'])>0 or count(indexfield[@searchable='true'])>0">, string keyword = ""</xsl:if><xsl:for-each select="field[@filter='true']">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:if test="/items/enum[@name=$searchType] and @searchToggle!='null'">sdk.</xsl:if><xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">, <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:if test="/items/enum[@name=$searchType] and @searchToggle!='null'">sdk.</xsl:if><xsl:value-of select="@searchToggle"/></xsl:for-each>);
        </xsl:if>
        <xsl:for-each select="field[@filter='true']">
        Task&lt;ListResult&lt;<xsl:value-of select="../@name"/>&gt;&gt; GetBy<xsl:value-of select="@friendlyName"/>Async(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(../field[@storePartitionKey='true'])>0"><xsl:value-of select="../field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>, </xsl:if><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, int skip, int take, string order_by = "", bool descending = false<xsl:if test="count(../field[@searchable='true'])>0 or count(../indexfield[@searchable='true'])>0"><xsl:if test="count(../field[@searchable='true'])>0 or (count(../indexfield[@searchable='true'])>0)">, string keyword = ""</xsl:if></xsl:if>);
        </xsl:for-each>
        <xsl:if test="count(field[@storePartitionKey='true'])>0">Task DeleteFor<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/>Async(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>);</xsl:if>
        <xsl:if test="../@storeBulk='true'">void BulkDisplaceAsync(<xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, List&lt;<xsl:value-of select="../@name"/>&gt; entities);</xsl:if>

        Task&lt;PermissionInfo&gt; GenerateReadPermissionForPartition<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Async(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid user_id, string partitionKey, string permissionId = "perm.standard.read");
    }
}
'''[ENDFILE]


'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Store\Implementation\<xsl:value-of select="@name"/>Store_Core.cs]using <xsl:value-of select="../@foundation"/>.Foundation;
using <xsl:value-of select="../@projectName"/>.Primary.Business.Store.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using <xsl:value-of select="../@projectName"/>.SDK;
using <xsl:value-of select="../@projectName"/>.SDK.Models;
using sdk = <xsl:value-of select="../@projectName"/>.SDK.Models;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Store.Implementation
{
    public partial class <xsl:value-of select="@name"/>Store : StoreBase&lt;<xsl:value-of select="@name"/>&gt;, I<xsl:value-of select="@name"/>Store
    {
        #region Constructor
        
        public <xsl:value-of select="@name"/>Store(IFoundation foundation)
            : base(foundation, "<xsl:value-of select="@name"/>Store")
        {

        }

        #endregion

        #region Properties

        private static string[] INDEX_GUID_FIELDS = new string[] { <xsl:for-each select="field[@type='Guid' and not(@noStore='true') and not(@sdkHidden='true') and not(@storeNoIndex='true')]"><xsl:if test="position()>1">, </xsl:if>nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>)</xsl:for-each><xsl:for-each select="indexfield[@type='Guid' and not(@noStore='true')]">, nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>)</xsl:for-each>  };
        private static string[] INDEX_DATE_FIELDS = new string[] { <xsl:for-each select="field[@type='DateTime' and not(@noStore='true') and not(@sdkHidden='true') and not(@storeNoIndex='true')]"><xsl:if test="position()>1">, </xsl:if>nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>)</xsl:for-each><xsl:for-each select="indexfield[@type='DateTime' and not(@noStore='true')]">, nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>)</xsl:for-each> };
        private static string[] INDEX_STRING_FIELDS = new string[] { <xsl:for-each select="field[@type='string' and (@sortable='true' or @searchable='true')]"><xsl:if test="position()>1">, </xsl:if>nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>)</xsl:for-each><xsl:if test="count(field[@sortable='true' or @searchable='true'])>0 and count(indexfield[@sortable='true' or @searchable='true'])>0">, </xsl:if><xsl:for-each select="indexfield[@sortable='true' or @searchable='true']"><xsl:if test="position()>1">, </xsl:if>nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>)</xsl:for-each> };
        private static string[] INDEX_NUMBER_FIELDS = new string[] { <xsl:for-each select="field[@type='int' and not(@sdkHidden='true')]"><xsl:if test="position()>1">, </xsl:if>nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>)</xsl:for-each><xsl:if test="count(field[@type='int'])>0 and count(indexfield[@type='int'])>0">, </xsl:if><xsl:for-each select="indexfield[@type='int']"><xsl:if test="position()>1">, </xsl:if>nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>)</xsl:for-each> };
        private static SortInfo[][] INDEX_COMPOSITES = new SortInfo[][] {  <xsl:for-each select="field[string-length(@storeCompositeName)>0]">
            <xsl:variable name="composite_name"><xsl:value-of select="@storeCompositeName"/></xsl:variable>
            <xsl:if test="text() = ../field[@storeCompositeName=$composite_name][1]/text()">
            new SortInfo[] //<xsl:value-of select="$composite_name"/>
            { 
                <xsl:for-each select="../field[@storeCompositeName=$composite_name]"><xsl:if test="position()>1">, 
                </xsl:if>new SortInfo(){ field = nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>), descending = <xsl:value-of select="@storeCompositeDescending" />}</xsl:for-each><xsl:for-each select="../indexfield[@storeCompositeName=$composite_name]"><xsl:if test="count(../field[@storeCompositeName=$composite_name])>0">, 
                </xsl:if>new SortInfo(){ field = nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>), descending = <xsl:value-of select="@storeCompositeDescending" />}</xsl:for-each> 
            },
            </xsl:if>
        </xsl:for-each>
        };

        public override string ContainerName
        {
            get
            {
                return ContainerNames.<xsl:value-of select="@name"/>;
            }
        }

        public override string PartitionKeyField
        {
            get
            {
                return nameof(<xsl:value-of select="@name"/>.<xsl:choose><xsl:when test="count(field[@storePartitionKey='Global' or @storePartitionKey='Self' or @storePartitionKey='SplitID'])>0">partition_key</xsl:when><xsl:otherwise><xsl:value-of select="field[@storePartitionKey='true']/text()"/></xsl:otherwise></xsl:choose>);
            }
        }

        public override string PrimaryKeyField
        {
            get
            {
                return nameof(<xsl:value-of select="@name"/>.<xsl:value-of select="field[1]/text()"/>);
            }
        }


        public override string[] IndexGuidFields
        {
            get
            {
                return INDEX_GUID_FIELDS;
            }
        }

        public override string[] IndexDateFields
        {
            get
            {
                return INDEX_DATE_FIELDS;
            }
        }

        public override string[] IndexStringFields
        {
            get
            {
                return INDEX_STRING_FIELDS;
            }
        }
        public override string[] IndexNumberFields
        {
            get
            {
                return INDEX_NUMBER_FIELDS;
            }
        }
        public override SortInfo[][] IndexComposites
        {
            get
            {
                return INDEX_COMPOSITES;
            }
        }

        #endregion

        #region Overrides

        protected override string ExtractPartitionKey(<xsl:value-of select="@name"/> entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPartitionKey", delegate ()
            {
                return entity.<xsl:choose><xsl:when test="count(field[@storePartitionKey='true'])>0"><xsl:value-of select="field[@storePartitionKey='true']/text()"/>.ToString();</xsl:when><xsl:otherwise>partition_key</xsl:otherwise></xsl:choose>;
            });
        }

        protected override string ExtractPrimaryKey(<xsl:value-of select="@name"/> entity)
        {
            return base.ExecuteFunctionByPassHealth("ExtractPrimaryKey", delegate ()
            {
                return entity.<xsl:value-of select="field[1]/text()"/>.ToString();
            });
        }

        #endregion

        #region Public Methods

        <xsl:choose><xsl:when test="count(field[@storePartitionKey='true'])>0"><xsl:if test="count(field[@storePartitionKey='true' and not(@tenant='true')])>0">
        [Obsolete("Use caution, this is expensive for multi-partition tables", false)]
        public Task&lt;<xsl:value-of select="@name"/>&gt; GetDocumentAsync(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>)
        {
            return base.ExecuteFunction(nameof(GetDocumentAsync), async delegate ()
            {
                <xsl:choose><xsl:when test="count(field[@storePartitionKey='Global' or @storePartitionKey='Self' or @storePartitionKey='SplitID'])>0">
                string partitionKey = new <xsl:value-of select="@name"/>(){ <xsl:value-of select="field[1]/text()"/> = <xsl:value-of select="field[1]/text()"/>}.partition_key;
                <xsl:value-of select="@name"/> found<xsl:value-of select="@name"/> = await base.RetrieveById<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Async(partitionKey, <xsl:value-of select="field[1]/text()"/>.ToString());</xsl:when>
                <xsl:otherwise><xsl:value-of select="@name"/> found<xsl:value-of select="@name"/> = await base.RetrieveById<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>WithoutPartitionKeyAsync(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()" />, </xsl:for-each><xsl:value-of select="field[1]/text()"/>.ToString());</xsl:otherwise>
                </xsl:choose>
                <xsl:if test="count(indexfield[@sensitive='true'])>0 or count(field[@sensitive='true'])>0">

                this.PostProcessSensitive(found<xsl:value-of select="@name"/>);</xsl:if>

                return found<xsl:value-of select="@name"/>;
                
            });
        }
        </xsl:if></xsl:when><xsl:otherwise>
        public Task&lt;<xsl:value-of select="@name"/>&gt; GetDocumentAsync(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[1]/text()"/>)
        {
            return base.ExecuteFunction(nameof(GetDocumentAsync), async delegate ()
            {
                <xsl:choose>
                <xsl:when test="count(field[@storePartitionKey='Global' or @storePartitionKey='Self' or @storePartitionKey='SplitID'])>0">
                string partitionKey = new <xsl:value-of select="@name"/>(){ <xsl:value-of select="field[1]/text()"/> = <xsl:value-of select="field[1]/text()"/>}.partition_key;

                <xsl:value-of select="@name"/> found<xsl:value-of select="@name"/> = await base.RetrieveById<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Async(<xsl:for-each select="field[@tenant='true' and @isolated='true']"><xsl:value-of select="text()" />, </xsl:for-each>partitionKey, <xsl:value-of select="field[1]/text()"/>.ToString());</xsl:when>
                <xsl:otherwise>
                <xsl:value-of select="@name"/> found<xsl:value-of select="@name"/> = await base.RetrieveById<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>WithoutPartitionKeyAsync(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()" />, </xsl:for-each><xsl:value-of select="field[1]/text()"/>);</xsl:otherwise>
                </xsl:choose>
                <xsl:if test="count(indexfield[@sensitive='true'])>0 or count(field[@sensitive='true'])>0">

                this.PostProcessSensitive(found<xsl:value-of select="@name"/>);</xsl:if>

                return found<xsl:value-of select="@name"/>;
            });
        }
        </xsl:otherwise></xsl:choose>
        
        <xsl:for-each select="field[@storePartitionKey='true']">
        public Task&lt;<xsl:value-of select="../@name"/>&gt; GetDocumentAsync(<xsl:if test="not(@tenant='true' and not(@isolated='true'))"><xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each></xsl:if><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/><xsl:if test="not(../field[1]/@isolated='true')">, <xsl:value-of select="../field[1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="../field[1]/text()"/></xsl:if>)
        {
            return base.ExecuteFunction(nameof(GetDocumentAsync), async delegate ()
            {
                <xsl:value-of select="../@name"/> found<xsl:value-of select="@name"/> = await this.RetrieveById<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared' or ../@tenant='Route'">Shared</xsl:if>Async(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="text()" />, </xsl:for-each><xsl:value-of select="text()"/>.ToString(), <xsl:value-of select="../field[1]/text()"/>.ToString());
                <xsl:if test="count(../indexfield[@sensitive='true'])>0 or count(../field[@sensitive='true'])>0">
                this.PostProcessSensitive(found<xsl:value-of select="@name"/>);</xsl:if>

                return found<xsl:value-of select="@name"/>;
            });
        }
        </xsl:for-each>


        public Task&lt;bool&gt; CreateDocumentAsync(<xsl:value-of select="@name"/> model)
        {
            return base.ExecuteFunction(nameof(CreateDocumentAsync), async delegate ()
            {
                <xsl:if test="@storeBulk='true'">model.transaction_id = Guid.NewGuid().ToString();
                model.transaction_stamp_utc = DateTime.UtcNow;
                </xsl:if>ItemResponse&lt;<xsl:value-of select="@name"/>&gt; result = await base.Upsert<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Async(<xsl:for-each select="field[@tenant='true']">model.<xsl:value-of select="text()" />, </xsl:for-each>model);
                return result.StatusCode.IsSuccess();
            });
        }

        public Task&lt;bool&gt; DeleteDocumentAsync(<xsl:value-of select="@name"/> model)
        {
            return base.ExecuteFunctionByPassHealth(nameof(DeleteDocumentAsync), delegate ()
            {
                return base.Remove<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Async(<xsl:for-each select="field[@tenant='true']">model.<xsl:value-of select="text()" />, </xsl:for-each>model);
            });
        }

        <xsl:if test="count(field[string-length(@storePartitionKey)>0])>0">
        <xsl:if test="count(field[@storePartitionKey='Self'])>0 or count(field[@storePartitionKey='SplitID'])>0">[Obsolete("Use caution, this is expensive for multi-partition tables", false)]</xsl:if>
        public Task&lt;ListResult&lt;<xsl:value-of select="@name"/>&gt;&gt; <xsl:choose><xsl:when test="count(field[@storePartitionKey='Global' or @storePartitionKey='Self' or @storePartitionKey='SplitID'])>0">Find</xsl:when><xsl:otherwise>FindFor<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:otherwise></xsl:choose>Async(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(field[@storePartitionKey='true'])>0"><xsl:value-of select="field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>, </xsl:if>int skip, int take, string order_by = "", bool descending = false<xsl:if test="count(field[@searchable='true'])>0 or count(indexfield[@searchable='true'])>0">, string keyword = ""</xsl:if><xsl:for-each select="field[@filter='true']">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:if test="/items/enum[@name=$searchType] and @searchToggle!='null'">sdk.</xsl:if><xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">, <xsl:variable name="searchType"><xsl:value-of select="@type" /></xsl:variable><xsl:if test="/items/enum[@name=$searchType]">sdk.</xsl:if><xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:if test="/items/enum[@name=$searchType] and @searchToggle!='null'">sdk.</xsl:if><xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction(nameof(<xsl:choose><xsl:when test="count(field[@storePartitionKey='Global' or @storePartitionKey='Self' or @storePartitionKey='SplitID'])>0">Find</xsl:when><xsl:otherwise>FindFor<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/></xsl:otherwise></xsl:choose>Async), async delegate ()
            {
                <xsl:choose><xsl:when test="count(field[@storePartitionKey='Global'])>0">
                IQueryable&lt;<xsl:value-of select="@name"/>&gt; query = base.QueryByPartition<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>(<xsl:for-each select="field[@tenant='true' and @isolated='true']"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="@name"/>.GLOBAL_PARTITION);
                </xsl:when><xsl:when test="count(field[@storePartitionKey='Self'])>0">
                IQueryable&lt;<xsl:value-of select="@name"/>&gt; query = base.Query<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>WithoutPartitionKey();
                </xsl:when><xsl:when test="count(field[@storePartitionKey='SplitID'])>0">
                IQueryable&lt;<xsl:value-of select="@name"/>&gt; query = base.Query<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>WithoutPartitionKey();
                </xsl:when><xsl:otherwise>IQueryable&lt;<xsl:value-of select="@name"/>&gt; query = this.QueryByPartition<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>.ToString());</xsl:otherwise>
                </xsl:choose>
                
                <xsl:if test="count(field[@searchable='true'])>0 or count(indexfield[@searchable='true'])>0">
                
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    <xsl:choose><xsl:when test="count(field[@searchable='true'])>0 and count(indexfield[@searchable='true'])>0">
                    query = query.Where(x => 
                        <xsl:for-each select="field[@searchable='true']">    <xsl:if test="position()>1">    || </xsl:if>x.<xsl:value-of select="text()"/>.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    </xsl:for-each><xsl:for-each select="indexfield[@searchable='true']">    || x.<xsl:value-of select="text()"/>.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    </xsl:for-each>
                    );
                    </xsl:when><xsl:when test="count(field[@searchable='true'])>0">query = query.Where(x => 
                        <xsl:for-each select="field[@searchable='true']">    <xsl:if test="position()>1">    || </xsl:if>x.<xsl:value-of select="text()"/>.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    </xsl:for-each>);</xsl:when><xsl:when test="count(indexfield[@searchable='true'])>0">query = query.Where(x => 
                        <xsl:for-each select="indexfield[@searchable='true']">    <xsl:if test="position()>1">    || </xsl:if>x.<xsl:value-of select="text()"/>.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    </xsl:for-each>);</xsl:when>
                    </xsl:choose>
                }
                </xsl:if>

                <xsl:for-each select="field[@filter='true']">
                if(<xsl:choose><xsl:when test="@type='string'">!string.IsNullOrWhiteSpace(<xsl:value-of select="text()"/>)</xsl:when><xsl:otherwise><xsl:value-of select="text()"/>.HasValue</xsl:otherwise></xsl:choose>)
                {
                    query = query.Where(x =&gt; x.<xsl:value-of select="text()"/> == <xsl:value-of select="text()"/><xsl:if test="not(@type='string')">.Value</xsl:if>);
                }
                </xsl:for-each>

                <xsl:for-each select="field[string-length(@searchToggle)>0]">
                if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query = query.Where(x =&gt; x.<xsl:value-of select="text()"/> == <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>
                
                <xsl:for-each select="indexfield[string-length(@searchToggle)>0]">
                if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query = query.Where(x =&gt; x.<xsl:value-of select="text()"/> == <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult&lt;<xsl:value-of select="@name"/>&gt; result = await query.FetchAsSteppedListAsync(skip, take);
                <xsl:if test="count(indexfield[@sensitive='true'])>0 or count(field[@sensitive='true'])>0">
                this.PostProcessSensitive(result.items);
                </xsl:if>
                return result;
            });
        }

        <xsl:for-each select="field[@filter='true']">
        public Task&lt;ListResult&lt;<xsl:value-of select="../@name"/>&gt;&gt; GetBy<xsl:value-of select="@friendlyName"/>Async(<xsl:for-each select="../field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:if test="count(../field[@storePartitionKey='true'])>0"><xsl:value-of select="../field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>, </xsl:if><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, int skip, int take, string order_by = "", bool descending = false<xsl:if test="count(../field[@searchable='true'])>0 or count(../indexfield[@searchable='true'])>0"><xsl:if test="count(../field[@searchable='true'])>0 or (count(../indexfield[@searchable='true'])>0)">, string keyword = ""</xsl:if></xsl:if>)
        {
            return base.ExecuteFunction(nameof(GetBy<xsl:value-of select="@friendlyName"/>Async), async delegate ()
            {
                <xsl:choose><xsl:when test="count(../field[@storePartitionKey='Global'])>0">
                IQueryable&lt;<xsl:value-of select="../@name"/>&gt; query = base.QueryByPartition<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared' or ../@tenant='Route'">Shared</xsl:if>(<xsl:value-of select="../@name"/>.GLOBAL_PARTITION);
                </xsl:when><xsl:when test="count(../field[@storePartitionKey='Self'])>0">
                IQueryable&lt;<xsl:value-of select="../@name"/>&gt; query = base.Query<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared' or ../@tenant='Route'">Shared</xsl:if>WithoutPartitionKey();
                </xsl:when><xsl:when test="count(../field[@storePartitionKey='SplitID'])>0">
                IQueryable&lt;<xsl:value-of select="../@name"/>&gt; query = base.Query<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared' or ../@tenant='Route'">Shared</xsl:if>WithoutPartitionKey();
                </xsl:when><xsl:otherwise>IQueryable&lt;<xsl:value-of select="../@name"/>&gt; query = this.QueryByPartition<xsl:if test="../@tenant='Isolated'">Isolated</xsl:if><xsl:if test="../@tenant='Shared' or ../@tenant='Route'">Shared</xsl:if>(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="../field[@storePartitionKey='true'][1]/text()"/>.ToString());</xsl:otherwise>
                </xsl:choose>

                query = query.Where(x => x.<xsl:value-of select="text()"/> == <xsl:value-of select="text()"/>);
                
                <xsl:if test="count(../field[@searchable='true'])>0 or count(../indexfield[@searchable='true'])>0">
                
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    <xsl:for-each select="../field[@searchable='true']">query = query.Where(x => x.<xsl:value-of select="text()"/>.Contains(keyword));
                    </xsl:for-each>
                    <xsl:for-each select="../indexfield[@searchable='true']">query = query.Where(x => x.<xsl:value-of select="text()"/>.Contains(keyword));
                    </xsl:for-each>
                }
                </xsl:if>

                query = this.ApplySafeSort(query, order_by, descending);

                ListResult&lt;<xsl:value-of select="../@name"/>&gt; result = await query.FetchAsSteppedListAsync(skip, take);
                <xsl:if test="count(../indexfield[@sensitive='true'])>0 or count(../field[@sensitive='true'])>0">
                this.PostProcessSensitive(result.items);
                </xsl:if>
                return result;
            });
        }
        </xsl:for-each>

        <xsl:if test="../@storeBulk='true'">public Task BulkDisplaceAsync(<xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, List&lt;<xsl:value-of select="../@name"/>&gt; entities)
        {
            return.ExecuteFunction("BulkDisplace", delegate ()
            {
                DateTime stamp_utc = DateTime.UtcNow;
                string transaction_id = Guid.NewGuid().ToString();
                foreach (<xsl:value-of select="../@name"/> item in entities)
                {
                    item.transaction_id = transaction_id;
                    item.transaction_stamp_utc = stamp_utc;
                }

                base.BulkUpsertAsync(entities).Wait();

                List&lt;<xsl:value-of select="../@name"/>&gt; deleteItems = base.QueryByPartition(<xsl:value-of select="text()"/>.ToString())
                    .Where(x => x.transaction_id != transaction_id &amp;&amp; x.transaction_stamp_utc &lt; stamp_utc)
                    .ToList();

                base.BulkRemoveAsync(deleteItems).Wait();
            });
        }
        </xsl:if>
        <xsl:if test="count(field[@storePartitionKey='true'])>0">
        public Task DeleteFor<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/>Async(<xsl:for-each select="field[@tenant='true' and not(@storePartitionKey='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[@storePartitionKey='true'][1]/@type"/><xsl:text> </xsl:text><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>)
        {
            return base.ExecuteFunction(nameof(DeleteFor<xsl:value-of select="field[@storePartitionKey='true'][1]/@friendlyName"/>Async), async delegate ()
            {
                List&lt;<xsl:value-of select="@name"/>&gt; deleteItems = await base.RetrieveByPartition<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Async(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="field[@storePartitionKey='true'][1]/text()"/>.ToString());

                return base.BulkRemove<xsl:if test="@tenant='Isolated'">Isolated</xsl:if><xsl:if test="@tenant='Shared' or @tenant='Route'">Shared</xsl:if>Async(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/>, </xsl:for-each>deleteItems);
            });
        }
        </xsl:if>
        </xsl:if>

        #endregion

        #region Protected Methods 

        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable&lt;<xsl:value-of select="@name"/>&gt; ApplySafeSort(IQueryable&lt;<xsl:value-of select="@name"/>&gt; query, string order_by, bool descending)
        {
            return this.ApplySafeSort(query, new List&lt;SortInfo&gt;() { new SortInfo() { field = order_by, descending = descending } });
        }
        
        /// <summary>
        /// Only allow sorting by known indexed fields
        /// </summary>
        protected IOrderedQueryable&lt;<xsl:value-of select="@name"/>&gt; ApplySafeSort(IQueryable&lt;<xsl:value-of select="@name"/>&gt; query, List&lt;SortInfo&gt; sorts)
        {
            return base.ExecuteFunctionByPassHealth(nameof(ApplySafeSort), delegate ()
            {
                IOrderedQueryable&lt;<xsl:value-of select="@name"/>&gt; result = null;

                foreach (SortInfo item in sorts)
                {
                    if(!string.IsNullOrWhiteSpace(item.field))
                    {
                        switch (item.field)
                        {
                            <xsl:for-each select="field[not(@noStore='true') and not(@sdkHidden='true') and (@sortable='true' or @searchable='true' or @isEnum='true' or @type='int')]">case nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x =&gt; x.<xsl:value-of select="text()"/>);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x =&gt; x.<xsl:value-of select="text()"/>);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x =&gt; x.<xsl:value-of select="text()"/>);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x =&gt; x.<xsl:value-of select="text()"/>);
                                    }
                                }
                                break;
                            </xsl:for-each><xsl:for-each select="indexfield[not(@noStore='true') and not(@sdkHidden='true') and (@sortable='true' or @searchable='true' or @isEnum='true' or @type='int')]">case nameof(<xsl:value-of select="../@name"/>.<xsl:value-of select="text()"/>):
                                if(item.descending)
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderByDescending(x =&gt; x.<xsl:value-of select="text()"/>);
                                    }
                                    else
                                    {
                                        result = result.ThenByDescending(x =&gt; x.<xsl:value-of select="text()"/>);
                                    }
                                }
                                else
                                {
                                    if(result == null)
                                    {
                                        result = query.OrderBy(x =&gt; x.<xsl:value-of select="text()"/>);
                                    }
                                    else
                                    {
                                        result = result.ThenBy(x =&gt; x.<xsl:value-of select="text()"/>);
                                    }
                                }
                                break;
                            </xsl:for-each>
                            default:
                                // nothing
                                break;
                        }
                    }
                }
                if(result == null)
                {
                    result = query.OrderBy(x =&gt; x.<xsl:value-of select="field[1]/text()"/>);
                }
                return result;
                
            });
        }

        <xsl:if test="count(indexfield[@sensitive='true'])>0  or count(field[@sensitive='true'])>0">
        protected void PostProcessSensitive&lt;TModel&gt;(List&lt;TModel&gt; items)
        {
            if(items != null)
            {
                foreach(var item in items)
                {
                    this.PostProcessSensitive(item);
                }
            }
        }
        protected void PostProcessSensitive&lt;TModel&gt;(TModel item)
        {
            if(item is <xsl:value-of select="@name"/>)
            {
               <xsl:for-each select="indexfield[@sensitive='true']">
               (item as <xsl:value-of select="../@name"/>).<xsl:value-of select="text()"/> = default(<xsl:value-of select="@type"/>);</xsl:for-each>
               <xsl:for-each select="field[@sensitive='true' and not(@sdkHidden='true')]">
               (item as <xsl:value-of select="../@name"/>).<xsl:value-of select="text()"/> = default(<xsl:value-of select="@type"/>);</xsl:for-each>
            }
        }
        </xsl:if>

        #endregion
    }
}


'''[ENDFILE]

</xsl:for-each>

<xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">
  <xsl:variable name="computeDirect"><xsl:value-of select="../@computeDirect"/></xsl:variable>
'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Synchronization\I<xsl:value-of select="@name"/>Synchronizer_Core.cs]using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sdk = <xsl:value-of select="../@projectName"/>.SDK.Models;
using dm = <xsl:value-of select="../@projectName"/>.Domain;<xsl:if test="@hydrateCachedData='true'">
using <xsl:value-of select="../@projectName"/>.Primary.Business.Synchronization.Caching;</xsl:if>

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Synchronization
{
    public partial interface I<xsl:value-of select="@name"/>Synchronizer : ISynchronizer
    {
        void SynchronizeItem(IdentityInfo primaryKey, Availability availability);
        sdk.<xsl:value-of select="@name"/> AdHocHydrate(dm.<xsl:value-of select="@name"/> domainModel);
        <xsl:if test="@hydrateCachedData='true'">
        void PerformSynchronizationAdhoc(<xsl:choose><xsl:when test="count(field[@tenant='true'])>0">IdentityInfo</xsl:when><xsl:otherwise>Guid</xsl:otherwise></xsl:choose> <xsl:value-of select="field[1]"/>, <xsl:value-of select="@name"/>CachedData cachedData);
        </xsl:if>
    }
}

'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Synchronization\Implementation\<xsl:value-of select="@name"/>Synchronizer_Core.cs]using <xsl:value-of select="../@foundation"/>.Foundation;
using <xsl:value-of select="../@projectName"/>.Common;
using <xsl:value-of select="../@projectName"/>.Domain;
using <xsl:value-of select="../@projectName"/>.Primary.Health;
using sdk = <xsl:value-of select="../@projectName"/>.SDK.Models;
using dm = <xsl:value-of select="../@projectName"/>.Domain;<xsl:if test="@hydrateCachedData='true'">
using <xsl:value-of select="../@projectName"/>.Primary.Business.Synchronization.Caching;</xsl:if>
using System;
using System.Collections.Generic;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Synchronization.Implementation
{
    public partial class <xsl:value-of select="@name"/>Synchronizer : SynchronizerBase&lt;IdentityInfo&gt;, I<xsl:value-of select="@name"/>Synchronizer
    {
        public <xsl:value-of select="@name"/>Synchronizer(IFoundation foundation)
            : base(foundation, "<xsl:value-of select="@name"/>Synchronizer")
        {
        }

        <xsl:if test="@useStore='true' and not(@useIndex='true')">public override int MillisecondsForRefresh
        {
            get
            {
                return 0; // Only ES needs this
            }
        }
        </xsl:if>

        public override int Priority
        {
            get
            {
                return <xsl:value-of select="@indexPriority"/>;
            }
        }

        <xsl:if test="@hydrateCachedData='true'">
        public void PerformSynchronizationAdhoc(Guid primaryKey, <xsl:value-of select="@name"/>CachedData cachedData)
        {
            base.ExecuteMethod("PerformSynchronizationAdhoc", delegate ()
            {
                <xsl:value-of select="@name"/> domainModel = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(primaryKey);
                if (domainModel != null)
                {
                    Action&lt;Guid, bool, DateTime, string&gt; synchronizationUpdateMethod = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.SynchronizationUpdate;
                    if(this.API.Integration.Settings.IsHydrate())
                    {
                        synchronizationUpdateMethod = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.SynchronizationHydrateUpdate;
                    }
                    DateTime syncDate = DateTime.UtcNow;
                    if (domainModel.sync_invalid_utc.HasValue)
                    {
                        syncDate = domainModel.sync_invalid_utc.Value;
                    }
                    try
                    {
                        sdk.<xsl:value-of select="@name"/> sdkModel = domainModel.ToSDKModel();
                        this.SanitizeModel(sdkModel);
                        
                        this.HydrateSDKModelComputed(domainModel, sdkModel);
                        this.HydrateSDKModel(domainModel, sdkModel, cachedData);

                        if (domainModel.deleted_utc.HasValue)
                        {
                            <xsl:if test="@useIndex='true' and not(@byPassIndex='true')">this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.DeleteDocumentAsync(sdkModel).SyncResult();
                            </xsl:if><xsl:if test="string-length(@graphNode)>0">bool deleted = this.API.Graph.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.DeleteNode(sdkModel.<xsl:value-of select="field[1]"/>);
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from graph");
                            }
                            </xsl:if>
                            <xsl:if test="string-length(@graphRelation)>0">bool deleted = this.API.Graph.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.DeleteRelation(sdkModel);
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from graph");
                            }
                            </xsl:if>
                            <xsl:if test="@useStore='true'">bool deleted = this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.DeleteDocumentAsync(sdkModel).SyncResult();
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from store");
                            }
                            </xsl:if>
                            synchronizationUpdateMethod(domainModel.<xsl:value-of select="field[1]"/>, true, syncDate, null);
                        }
                        else
                        {
                            string sync_log = string.Empty;
                            <xsl:if test="not(@byPassIndex='true')">IndexResult result = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.UpdateDocument(sdkModel);
                            if (!result.success)
                            {
                                throw new Exception(result.ToString());
                            }
                            sync_log = result.ToString();
                            </xsl:if>
                            <xsl:if test="string-length(@graphNode)>0 ">bool persisted = this.API.Graph.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.UpsertNode(sdkModel);
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to graph");
                            }
                            sync_log += " Persisted to Graph";
                            </xsl:if>
                            <xsl:if test="string-length(@graphRelation)>0">bool persisted = this.API.Graph.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.UpsertRelation(sdkModel);
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to graph");
                            }
                            sync_log += " Persisted to Graph";
                            </xsl:if>
                            <xsl:if test="@useStore='true'">bool persisted = this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.CreateDocumentAsync(sdkModel).SyncResult();
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to store");
                            }
                            sync_log += " Persisted to Store";
                            </xsl:if>
                            synchronizationUpdateMethod(domainModel.<xsl:value-of select="field[1]"/>, true, syncDate, sync_log);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.IFoundation.LogError(ex, "PerformSynchronizationForItem");
                        HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_ERROR_SYNC, this.EntityName), 0, 1);
                        synchronizationUpdateMethod(primaryKey, false, syncDate, FoundationUtility.FormatException(ex));
                    }
                }
            });
        }
        </xsl:if>
        
        public override void PerformSynchronizationForItem(IdentityInfo identity)
        {
            base.ExecuteMethod("PerformSynchronizationForItem", delegate ()
            {
                <xsl:value-of select="@name"/> domainModel = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:if test="count(field[@tenant='true' and not(@isolated='true')])>0">identity.route_id.Value, </xsl:if>identity.primary_key);
                if (domainModel != null)
                {
                    Action&lt;<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/>, </xsl:for-each>Guid, bool, DateTime, string&gt; synchronizationUpdateMethod = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.SynchronizationUpdate;
                    if(this.API.Integration.Settings.IsHydrate())
                    {
                        synchronizationUpdateMethod = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.SynchronizationHydrateUpdate;
                    }
                    DateTime syncDate = DateTime.UtcNow;
                    if (domainModel.sync_invalid_utc.HasValue)
                    {
                        syncDate = domainModel.sync_invalid_utc.Value;
                    }
                    try
                    {
                        sdk.<xsl:value-of select="@name"/> sdkModel = domainModel.ToSDKModel();
                        this.SanitizeModel(sdkModel);
                        
                        this.HydrateSDKModelComputed(domainModel, sdkModel);
                        this.HydrateSDKModel(domainModel, sdkModel<xsl:if test="@hydrateCachedData='true'">, null</xsl:if>);

                        if (domainModel.deleted_utc.HasValue)
                        {
                            <xsl:if test="@useIndex='true' and not(@byPassIndex='true')">this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.DeleteDocumentAsync(sdkModel).SyncResult();
                            </xsl:if><xsl:if test="string-length(@graphNode)>0">bool deleted = this.API.Graph.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.DeleteNode(sdkModel.<xsl:value-of select="field[1]"/>);
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from graph");
                            }
                            </xsl:if>
                            <xsl:if test="string-length(@graphRelation)>0">bool deleted = this.API.Graph.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.DeleteRelation(sdkModel);
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from graph");
                            }
                            </xsl:if>
                            <xsl:if test="@useStore='true'">bool deleted = this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.DeleteDocumentAsync(sdkModel).SyncResult();
                            if (!deleted)
                            {
                                throw new Exception("Error deleting from store");
                            }
                            </xsl:if>
                            synchronizationUpdateMethod(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">domainModel.<xsl:value-of select="text()"/>, </xsl:for-each>domainModel.<xsl:value-of select="field[1]"/>, true, syncDate, null);
                        }
                        else
                        {
                            string sync_log = string.Empty;
                            <xsl:if test="@useIndex='true' and not(@byPassIndex='true')">IndexResult result = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.UpdateDocument(sdkModel);
                            if (!result.success)
                            {
                                throw new Exception(result.ToString());
                            }
                            sync_log = result.ToString();
                            </xsl:if>
                            <xsl:if test="string-length(@graphNode)>0 ">bool persisted = this.API.Graph.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.UpsertNode(sdkModel);
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to graph");
                            }
                            sync_log += " Persisted to Graph";
                            </xsl:if>
                            <xsl:if test="string-length(@graphRelation)>0">bool persisted = this.API.Graph.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.UpsertRelation(sdkModel);
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to graph");
                            }
                            sync_log += " Persisted to Graph";
                            </xsl:if>
                            <xsl:if test="@useStore='true'">bool persisted = this.API.Store.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.CreateDocumentAsync(sdkModel).SyncResult();
                            if (!persisted)
                            {
                                throw new Exception("Error persisting to store");
                            }
                            sync_log += " Persisted to Store";
                            </xsl:if>
                            synchronizationUpdateMethod(<xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">domainModel.<xsl:value-of select="text()"/>, </xsl:for-each>domainModel.<xsl:value-of select="field[1]"/>, true, syncDate, sync_log);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.IFoundation.LogError(ex, "PerformSynchronizationForItem");
                        HealthReporter.Current.UpdateMetric(HealthTrackType.Each, string.Format(HealthReporter.INDEXER_ERROR_SYNC, this.EntityName), 0, 1);
                        synchronizationUpdateMethod(<xsl:if test="count(field[@tenant='true' and not(@isolated='true')])>0">identity.route_id.Value, </xsl:if>identity.primary_key, false, syncDate, FoundationUtility.FormatException(ex));
                    }
                }
            });
        }
        
        public override int PerformSynchronization(string requestedAgentName, string tenant_code)
        {
            return base.ExecuteFunction("PerformSynchronization", delegate ()
            {
                string agentName = requestedAgentName;
                if(string.IsNullOrEmpty(agentName))
                {
                    agentName = this.AgentName;
                }
                List&lt;IdentityInfo&gt; invalidItems = new List&lt;IdentityInfo&gt;();

                if(this.API.Integration.Settings.IsHydrate())
                {
                    invalidItems = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.SynchronizationHydrateGetInvalid(<xsl:for-each select="field[@tenant='true']">tenant_code, </xsl:for-each>CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                else
                {
                    invalidItems = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.SynchronizationGetInvalid(<xsl:for-each select="field[@tenant='true']">tenant_code, </xsl:for-each>CommonAssumptions.INDEX_RETRY_THRESHOLD_SECONDS, agentName);
                }
                foreach (IdentityInfo item in invalidItems)
                {
                    this.PerformSynchronizationForItem(item);
                }
                return invalidItems.Count;
            });
        }

        public sdk.<xsl:value-of select="@name"/> AdHocHydrate(dm.<xsl:value-of select="@name"/> domainModel)
        {
            return base.ExecuteFunction(nameof(AdHocHydrate), delegate ()
            {
                if(domainModel == null)
                {
                    return null;
                }
                else
                {
                    sdk.<xsl:value-of select="@name"/> sdkModel = domainModel.ToSDKModel();
                    this.SanitizeModel(sdkModel);

                    this.HydrateSDKModelComputed(domainModel, sdkModel);
                    this.HydrateSDKModel(domainModel, sdkModel);

                    return sdkModel;
                }
            });
        }

        protected virtual void SanitizeModel(sdk.<xsl:value-of select="@name"/> sdkModel)
        {
            <xsl:for-each select="field[@type='string' and not(@html='true') and not(@sdkHidden='true')]">
            sdkModel.<xsl:value-of select="text()" /> = PrimaryUtility.SanitizeHtml(this.API, "<xsl:value-of select="../@name"/>", sdkModel.<xsl:value-of select="../field[1]"/>, "<xsl:value-of select="text()" />", sdkModel.<xsl:value-of select="text()" />);</xsl:for-each>
            <xsl:for-each select="indexfield[@type='string' and not(@html='true')]">
            sdkModel.<xsl:value-of select="text()" /> = PrimaryUtility.SanitizeHtml(this.API, "<xsl:value-of select="../@name"/>", sdkModel.<xsl:value-of select="../field[1]"/>, "<xsl:value-of select="text()" />", sdkModel.<xsl:value-of select="text()" />);</xsl:for-each>

        }
        
        /// &lt;summary&gt;
        /// Computed and Calculated Aggs, Typically Generated
        /// &lt;/summary&gt;
        protected void HydrateSDKModelComputed(<xsl:value-of select="@name"/> domainModel, sdk.<xsl:value-of select="@name"/> sdkModel)
        {
            <xsl:for-each select="field[string-length(@computedFrom) > 0]">
            <xsl:variable name="computedFromCurrent"><xsl:value-of select="@computedFrom"/></xsl:variable>
            <xsl:variable name="computedFieldCurrent"><xsl:value-of select="@computedField" /></xsl:variable>
            <xsl:variable name="computedReferenceField"><xsl:value-of select="@computedReferenceField" /></xsl:variable>
            <xsl:variable name="computedByCurrent"><xsl:value-of select="@computedBy" /></xsl:variable>
            <xsl:choose><xsl:when test="$computedByCurrent='Count'">
            sdkModel.<xsl:value-of select="text()" /> = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@computedFrom"/></xsl:call-template>.Get<xsl:value-of select="@computedBy" /><xsl:value-of select="../../item[@name=$computedFromCurrent]/field[text()=$computedFieldCurrent]/@friendlyName" />(sdkModel.<xsl:value-of select="../field[1]" />);
            </xsl:when><xsl:when test="$computedByCurrent='Extra'">
            sdk.<xsl:value-of select="@computedFrom"/> reference<xsl:value-of select="@computedFrom"/> = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@computedFrom"/></xsl:call-template>.GetById(sdkModel.<xsl:value-of select="$computedReferenceField" />);
            if(reference<xsl:value-of select="@computedFrom"/> != null)
            {
                sdkModel.<xsl:value-of select="text()" /> = reference<xsl:value-of select="@computedFrom"/>.<xsl:value-of select="text()"/>;
            }
            else
            {
                <xsl:value-of select="@computedFrom"/> referenceDomain<xsl:value-of select="@computedFrom"/> = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@computedFrom"/></xsl:call-template>.GetById(sdkModel.<xsl:value-of select="$computedReferenceField"/>);
                if(referenceDomain<xsl:value-of select="@computedFrom"/> != null)
                {
                    sdkModel.<xsl:value-of select="text()" /> = referenceDomain<xsl:value-of select="@computedFrom"/>.<xsl:value-of select="text()"/>;
                }
            }
            </xsl:when><xsl:when test="$computedByCurrent='NotNull'">
            sdkModel.<xsl:value-of select="text()" /> = sdkModel.<xsl:value-of select="@computedFrom"/> != null;
            </xsl:when><xsl:when test="$computedByCurrent='Null'">
            sdkModel.<xsl:value-of select="text()" /> = sdkModel.<xsl:value-of select="@computedFrom"/> == null;
            </xsl:when></xsl:choose>
            </xsl:for-each>
            <xsl:for-each select="indexfield[string-length(@computedFrom) > 0]">
            <xsl:variable name="computedFromCurrent"><xsl:value-of select="@computedFrom"/></xsl:variable>
            <xsl:variable name="computedFieldCurrent"><xsl:value-of select="@computedField" /></xsl:variable>
            <xsl:variable name="computedReferenceField"><xsl:value-of select="@computedReferenceField" /></xsl:variable>
            <xsl:variable name="computedByCurrent"><xsl:value-of select="@computedBy" /></xsl:variable>
            <xsl:choose><xsl:when test="$computedByCurrent='Count'">
            sdkModel.<xsl:value-of select="text()" /> = this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@computedFrom"/></xsl:call-template>.Get<xsl:value-of select="@computedBy" /><xsl:value-of select="../../item[@name=$computedFromCurrent]/field[text()=$computedFieldCurrent]/@friendlyName" />(sdkModel.<xsl:value-of select="../field[1]" />);
            </xsl:when><xsl:when test="$computedByCurrent='Extra'">
            sdk.<xsl:value-of select="@computedFrom"/> reference<xsl:value-of select="@computedFrom"/> = <xsl:choose>
            <xsl:when test="$computeDirect='true'">null; // Index/Store Disabled, force direct</xsl:when>
            <xsl:otherwise>this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@computedFrom"/></xsl:call-template>.GetById(sdkModel.<xsl:value-of select="$computedReferenceField" />);</xsl:otherwise>
            </xsl:choose>
            
            if(reference<xsl:value-of select="@computedFrom"/> != null)
            {
                sdkModel.<xsl:value-of select="text()" /> = reference<xsl:value-of select="@computedFrom"/>.<xsl:value-of select="text()"/>;
            }
            else
            {
                <xsl:value-of select="@computedFrom"/> referenceDomain<xsl:value-of select="@computedFrom"/> = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@computedFrom"/></xsl:call-template>.GetById(sdkModel.<xsl:value-of select="$computedReferenceField"/>);
                if(referenceDomain<xsl:value-of select="@computedFrom"/> != null)
                {
                    sdkModel.<xsl:value-of select="text()" /> = referenceDomain<xsl:value-of select="@computedFrom"/>.<xsl:value-of select="text()"/>;
                }
            }
            </xsl:when><xsl:when test="$computedByCurrent='NotNull'">
            sdkModel.<xsl:value-of select="text()" /> = sdkModel.<xsl:value-of select="@computedFrom"/> != null;
            </xsl:when><xsl:when test="$computedByCurrent='Null'">
            sdkModel.<xsl:value-of select="text()" /> = sdkModel.<xsl:value-of select="@computedFrom"/> == null;
            </xsl:when></xsl:choose>
            </xsl:for-each>
        }
        partial void HydrateSDKModel(<xsl:value-of select="@name"/> domainModel, sdk.<xsl:value-of select="@name"/> sdkModel<xsl:if test="@hydrateCachedData='true'">, <xsl:value-of select="@name"/>CachedData cachedData</xsl:if>);
    }
}

'''[ENDFILE]

</xsl:for-each>

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Business\Index\Factory\<xsl:value-of select="items/@projectName"/>ElasticClientFactory_Core.cs]using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using <xsl:value-of select="items/@projectName"/>.SDK.Models;
using sdk = <xsl:value-of select="items/@projectName"/>.SDK.Models;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Business.Index
{
    public partial class <xsl:value-of select="items/@projectName"/>ElasticClientFactory
    {
        partial void MapIndexModels(CreateIndexDescriptor indexer)
        {
            MappingsDescriptor descriptor = new MappingsDescriptor();
            <xsl:for-each select="items/item[@useIndex='true']"><xsl:variable name="currentType"><xsl:value-of select="@currentType"/></xsl:variable>descriptor.AddMapping&lt;sdk.<xsl:value-of select="@name"/>&gt;(DocumentNames.<xsl:value-of select="@name"/>, p => p
                .AutoMap()<xsl:if test="string-length(@indexParent) > 0">
                .Parent(DocumentNames.<xsl:value-of select="@indexParent"/>)</xsl:if>
                .Properties(props => props
                    <xsl:for-each select="indexfield[@storeOnly='true']">.Object&lt;<xsl:if test="@namespace='true'">sdk.</xsl:if><xsl:value-of select="@type"/>&gt;(a => a
                        .Name(n => n.<xsl:value-of select="text()"/>)
                        .Enabled(false)
                    )</xsl:for-each>
                    <xsl:for-each select="indexfield[@indexIgnore='true']">.Object&lt;<xsl:if test="@namespace='true'">sdk.</xsl:if><xsl:value-of select="@type"/>&gt;(a => a
                        .Name(n => n.<xsl:value-of select="text()"/>)
                        //.Store(false)
                    )</xsl:for-each>
                    <xsl:for-each select="indexfield[@isPercolator='true']">.Percolator(s => s
                        .Name(n => n.<xsl:value-of select="text()"/>)
                    )</xsl:for-each>
                    <xsl:for-each select="field[@indexNested='true']">.Nested&lt;<xsl:value-of select="@type" />&gt;(n => n.Name(nn => nn.<xsl:value-of select="text()" />).AutoMap())
                    </xsl:for-each>
                    <xsl:for-each select="field[@type='Guid' and not(@indexAs='string')]">.Keyword(s => s
                        .Name(n => n.<xsl:value-of select="text()"/>)
                    )</xsl:for-each>
                    <xsl:for-each select="indexfield[@type='Guid' or @type='Guid[]']">.Keyword(s => s
                        .Name(n => n.<xsl:value-of select="text()"/>)
                    )</xsl:for-each>
                    <xsl:for-each select="indexfield[@notAnalyzed='true']">.Keyword(s => s
                        .Name(n => n.<xsl:value-of select="text()"/>)
                    )</xsl:for-each>
                    <xsl:for-each select="indexfield[@indexStore='true']">.Text(s => s
                        .Name(n => n.<xsl:value-of select="text()"/>)
                        .Fielddata(true)
                    )</xsl:for-each>
                    <xsl:for-each select="indexfield[@sortable='true' or @indexExact='true']">.Text(m => m
                        .Name(t => t.<xsl:value-of select="text()"/>)
                        .Fields(f => f
                                .Text(s => s
                                    .Name(n => n.<xsl:value-of select="text()"/>))<xsl:if test="@sortable='true'">
                                .Keyword(s => s
                                    .Name(n => n.<xsl:value-of select="text()"/>.Suffix("sort")))</xsl:if><xsl:if test="@indexExact='true'">
                                .Keyword(s => s
                                    .Name(n => n.<xsl:value-of select="text()"/>.Suffix("exact")))</xsl:if>
                                
                        )
                    )</xsl:for-each>
                    <xsl:for-each select="field[@sortable='true' or @indexExact='true']">.Text(m => m
                        .Name(t => t.<xsl:value-of select="text()"/>)
                        .Fields(f => f
                                .Text(s => s
                                    .Name(n => n.<xsl:value-of select="text()"/>))<xsl:if test="@sortable='true'">
                                .Keyword(s => s
                                    .Name(n => n.<xsl:value-of select="text()"/>.Suffix("sort")))</xsl:if><xsl:if test="@indexExact='true'">
                                .Keyword(s => s
                                    .Name(n => n.<xsl:value-of select="text()"/>.Suffix("exact")))</xsl:if>
                                
                        )
                    )</xsl:for-each>
                    <xsl:for-each select="indexfield[@indexNested='true']"><xsl:variable name="nestedObject"><xsl:value-of select="@nestedItem"/></xsl:variable>.Nested&lt;<xsl:value-of select="$nestedItem"/>&gt;(m => m
                        .Name("<xsl:value-of select="text()"/>")
                        .AutoMap()
                        .Properties(nprops => nprops
                            <xsl:for-each select="../../item[@name=$nestedItem]/field[@storeOnly='true']">.Object&lt;<xsl:if test="../../item[@name=$nestedItem]/@namespace='true'">sdk.</xsl:if><xsl:value-of select="@type"/>&gt;(a => a
                                 .Name(n => n.<xsl:value-of select="text()"/>)
                                 .Enabled(false)
                            )</xsl:for-each>
                            <xsl:for-each select="../../item[@name=$nestedItem]/field[@indexIgnore='true']">.Object&lt;<xsl:if test="../../item[@name=$nestedItem]/@namespace='true'">sdk.</xsl:if><xsl:value-of select="@type"/>&gt;(a => a
                                 .Name(n => n.<xsl:value-of select="text()"/>)
                                 //.Store(false)
                            )</xsl:for-each>
                            <xsl:for-each select="../../item[@name=$nestedItem]/field[@type='Guid' or @notAnalyzed='true']">.String(s => s
                                .Keyword(n => n.<xsl:value-of select="text()"/>)
                                .Index(FieldIndexOption.NotAnalyzed)
                            )</xsl:for-each>
                            <xsl:for-each select="../../nested[@name=$nestedItem]/field[@type='Guid' or @notAnalyzed='true']">.String(s => s
                                .Keyword(n => n.<xsl:value-of select="text()"/>)
                                .Index(FieldIndexOption.NotAnalyzed)
                            )</xsl:for-each>
                        )

                    )</xsl:for-each>
                )
            );
            </xsl:for-each>
            indexer.Mappings(m => descriptor);
        }
    }
}
'''[ENDFILE]


  
'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Mapping\_DomainModelExtensions_Core.cs]using AutoMapper;
using <xsl:value-of select="items/@foundation"/>.Foundation;
using System;
using System.Collections.Generic;
using db = <xsl:value-of select="items/@projectName"/>.Data.Sql.Models;
using dm = <xsl:value-of select="items/@projectName"/>.Domain;

namespace <xsl:value-of select="items/@projectName"/>.Primary
{
    public static partial class _DomainModelExtensions
    {
        private static Lazy&lt;IMapper&gt; _Mapper = new Lazy&lt;IMapper&gt;(() => 
        {
            // just a touch of anti-pattern
            IMapper mapper = CoreFoundation.Current.ResolveWithFallback&lt;IMapper&gt;();
            return mapper;
        });
        public static IMapper Mapper
        {
            get
            {
                return _Mapper.Value;
            }
        }
        <xsl:for-each select="items/item">
        public static db.<xsl:value-of select="@name" /> ToDbModel(this dm.<xsl:value-of select="@name" /> entity, db.<xsl:value-of select="@name" /> destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new db.<xsl:value-of select="@name" />(); }
                return Mapper.Map&lt;dm.<xsl:value-of select="@name" />, db.<xsl:value-of select="@name" />&gt;(entity, destination);
            }
            return null;
        }
        public static dm.<xsl:value-of select="@name" /> ToDomainModel(this db.<xsl:value-of select="@name" /> entity, dm.<xsl:value-of select="@name" /> destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new dm.<xsl:value-of select="@name" />(); }
                return Mapper.Map&lt;db.<xsl:value-of select="@name" />, dm.<xsl:value-of select="@name" />&gt;(entity, destination);
            }
            return null;
        }
        public static List&lt;dm.<xsl:value-of select="@name" />&gt; ToDomainModel(this IEnumerable&lt;db.<xsl:value-of select="@name" />&gt; entities)
        {
            List&lt;dm.<xsl:value-of select="@name" />&gt; result = new List&lt;dm.<xsl:value-of select="@name" />&gt;();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToDomainModel());
                }
            }
            return result;
        }
        
        
        </xsl:for-each>
    }
}

'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Mapping\_SDKModelExtensions_Core.cs]using AutoMapper;
using <xsl:value-of select="items/@foundation"/>.Foundation;
using System;
using System.Collections.Generic;
using <xsl:value-of select="items/@projectName"/>.Data.Sql;
using <xsl:value-of select="items/@projectName"/>.Domain;

namespace <xsl:value-of select="items/@projectName"/>.Primary
{
    public static partial class _SDKModelExtensions
    {
        private static Lazy&lt;IMapper&gt; _Mapper = new Lazy&lt;IMapper&gt;(() => 
        {
            // just a touch of anti-pattern
            IMapper mapper = CoreFoundation.Current.ResolveWithFallback&lt;IMapper&gt;();
            return mapper;
        });
        public static IMapper Mapper
        {
            get
            {
                return _Mapper.Value;
            }
        }

        <xsl:for-each select="items/item">
        <xsl:if test="count(field[@slim='true'])>0 or count(indexfield[@slim='true'])>0">
        public static SDK.Models.<xsl:value-of select="@name" />Slim ToSlim(this SDK.Models.<xsl:value-of select="@name" /> entity)
        {
            return PrimaryUtility.JsonCloneAs&lt;SDK.Models.<xsl:value-of select="@name" />Slim&gt;(entity);
        }
        </xsl:if>
        public static <xsl:value-of select="@name" /> ToDomainModel(this SDK.Models.<xsl:value-of select="@name" /> entity, <xsl:value-of select="@name" /> destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new Domain.<xsl:value-of select="@name" />(); }
                <xsl:value-of select="@name" /> result = Mapper.Map&lt;SDK.Models.<xsl:value-of select="@name" />, <xsl:value-of select="@name" />&gt;(entity, destination);<xsl:if test="count(field[@facadeMapping='true'])>0">
                result = entity.MapFacade(result);</xsl:if>
                return result;
            }
            return null;
        }
        public static SDK.Models.<xsl:value-of select="@name" /> ToSDKModel(this <xsl:value-of select="@name" /> entity, SDK.Models.<xsl:value-of select="@name" /> destination = null)
        {
            if (entity != null)
            {
                if (destination == null) { destination = new SDK.Models.<xsl:value-of select="@name" />(); }
                SDK.Models.<xsl:value-of select="@name" /> result = Mapper.Map&lt;<xsl:value-of select="@name" />, SDK.Models.<xsl:value-of select="@name" />&gt;(entity, destination);<xsl:if test="count(field[@facadeMapping='true'])>0">
                result = entity.MapFacade(result);</xsl:if><xsl:for-each select="field[string-length(@derivedProperty)>0]">
                result.<xsl:value-of select="@derivedProperty" /> = entity.Related<xsl:value-of select="@friendlyName" />.GetValueOrDefault().ToRelatedModel();
                </xsl:for-each>
                <xsl:variable name="currentKey"><xsl:value-of select="@name"/></xsl:variable><xsl:for-each select="../item/field[string-length(@derivedParentProperty)>0 and @foreignKey=$currentKey]">
                result.<xsl:value-of select="@derivedParentProperty"/> = entity.Related<xsl:value-of select="../@name"/>.GetValueOrDefault().ToRelatedModel();
                </xsl:for-each>
                return result;
            }
            return null;
        }
        public static List&lt;SDK.Models.<xsl:value-of select="@name" />&gt; ToSDKModel(this IEnumerable&lt;<xsl:value-of select="@name" />&gt; entities)
        {
            List&lt;SDK.Models.<xsl:value-of select="@name" />&gt; result = new List&lt;SDK.Models.<xsl:value-of select="@name" />&gt;();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    result.Add(item.ToSDKModel());
                }
            }
            return result;
        }
        
        
        </xsl:for-each>
    }
}

'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Mapping\PrimaryMappingProfile_Core.cs]using System;
using db = <xsl:value-of select="items/@projectName"/>.Data.Sql.Models;
using dm = <xsl:value-of select="items/@projectName"/>.Domain;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Mapping
{
    public partial class PrimaryMappingProfile : AutoMapper.Profile
    {
        public PrimaryMappingProfile()
            : base("PrimaryMappingProfile")
        {
            this.Configure();
        }

        protected void Configure()
        {
            this.DbAndDomainMappings();
            this.DomainAndSDKMappings();
            
            this.DbAndDomainMappings_Manual();
            this.DomainAndSDKMappings_Manual();
        }
        
        partial void DbAndDomainMappings_Manual();
        partial void DomainAndSDKMappings_Manual();
        
        protected void DbAndDomainMappings()
        {
            this.CreateMap&lt;DateTimeOffset?, DateTime?&gt;()
                .ConvertUsing(x => x.HasValue ? x.Value.UtcDateTime : (DateTime?)null);

            this.CreateMap&lt;DateTimeOffset, DateTime?&gt;()
                .ConvertUsing(x => x.UtcDateTime);

            this.CreateMap&lt;DateTimeOffset, DateTime&gt;()
                .ConvertUsing(x => x.UtcDateTime);

            this.CreateMap&lt;DateTime?, DateTimeOffset?&gt;()
                .ConvertUsing(x => x.HasValue ? new DateTimeOffset(x.Value) : (DateTimeOffset?)null);
                
            <xsl:for-each select="items/item">this.CreateMap&lt;db.<xsl:value-of select="@name" />, dm.<xsl:value-of select="@name" />&gt;();
            this.CreateMap&lt;dm.<xsl:value-of select="@name" />, db.<xsl:value-of select="@name" />&gt;();
            </xsl:for-each>
        }
        protected void DomainAndSDKMappings()
        {
            <xsl:for-each select="items/enum">this.CreateMap&lt;Domain.<xsl:value-of select="@name" />, SDK.Models.<xsl:value-of select="@name" />&gt;().ConvertUsing(x => (SDK.Models.<xsl:value-of select="@name" />)(int)x);
            this.CreateMap&lt;SDK.Models.<xsl:value-of select="@name" />, Domain.<xsl:value-of select="@name" />&gt;().ConvertUsing(x => (Domain.<xsl:value-of select="@name" />)(int)x);
            </xsl:for-each>
            <xsl:for-each select="items/item">
            this.CreateMap&lt;Domain.<xsl:value-of select="@name" />, SDK.Models.<xsl:value-of select="@name" />&gt;();
            this.CreateMap&lt;SDK.Models.<xsl:value-of select="@name" />, Domain.<xsl:value-of select="@name" />&gt;();
            </xsl:for-each>
        }
    }
}

'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Foundation\<xsl:value-of select="items/@projectName"/>BootStrap_Business.cs]using <xsl:value-of select="items/@foundation"/>.Foundation;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Direct;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Direct.Implementation;<xsl:if test="count(items/item[@useStore='true'])>0">
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Store;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Store.Implementation;</xsl:if><xsl:if test="count(items/item[@useIndex='true'])>0">
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Index;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Index.Implementation;
</xsl:if>
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Synchronization;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Synchronization.Implementation;
using Unity;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Foundation
{
    public partial class <xsl:value-of select="items/@projectName"/>BootStrap
    {
        protected virtual void RegisterDataElements(IFoundation foundation)
        {
            <xsl:for-each select="items/item">foundation.Container.RegisterType&lt;I<xsl:value-of select="@name" />Business, <xsl:value-of select="@name" />Business&gt;(TypeLifetime.Scoped);
            </xsl:for-each>
            
            //Indexes
            <xsl:for-each select="items/item[@useIndex='true' and not(@byPassIndex='true')]">foundation.Container.RegisterType&lt;I<xsl:value-of select="@name" />Index, <xsl:value-of select="@name" />Index&gt;(TypeLifetime.Scoped);
            </xsl:for-each>

            //Stores
            <xsl:for-each select="items/item[@useStore='true']">foundation.Container.RegisterType&lt;I<xsl:value-of select="@name" />Store, <xsl:value-of select="@name" />Store&gt;(TypeLifetime.Scoped);
            </xsl:for-each>
            
            //Synchronizers
            <xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">foundation.Container.RegisterType&lt;I<xsl:value-of select="@name" />Synchronizer, <xsl:value-of select="@name" />Synchronizer&gt;(TypeLifetime.Scoped);
            </xsl:for-each>
        }
    }
}

'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\<xsl:value-of select="items/@projectName"/>APIIndex.cs]using <xsl:value-of select="items/@foundation"/>.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Index;

namespace <xsl:value-of select="items/@projectName"/>.Primary
{
    public partial class <xsl:value-of select="items/@projectName"/>APIIndex : BaseClass
    {
        public <xsl:value-of select="items/@projectName"/>APIIndex(IFoundation ifoundation)
            : base(ifoundation)
        {
        }

        <xsl:for-each select="items/item[@useIndex='true' and not(@byPassIndex='true')]">public I<xsl:value-of select="@name" />Index <xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
        {
            get { return this.IFoundation.Resolve&lt;I<xsl:value-of select="@name" />Index&gt;(); }
        }
        </xsl:for-each>
    }
}


'''[ENDFILE]



'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Business\Integration\IDependencyCoordinator.cs]using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using <xsl:value-of select="items/@projectName"/>.Domain;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Business.Integration
{
    public interface IDependencyCoordinator
    {
        <xsl:for-each select="items/item">void <xsl:value-of select="@name"/>Invalidated(Dependency affectedDependencies, Guid <xsl:value-of select="field[1]" /><xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">, <xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/></xsl:for-each>);
        </xsl:for-each>
    }
}


'''[ENDFILE]


'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Business\Integration\DependencyCoordinator_Core.cs]using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using <xsl:value-of select="items/@projectName"/>.Domain;
using <xsl:value-of select="items/@foundation"/>.Foundation<xsl:value-of select="../@foundationCommon"/>.Aspect;
using <xsl:value-of select="items/@foundation"/>.Foundation;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Business.Integration
{
    public partial class DependencyCoordinator_Core : ChokeableClass, IDependencyCoordinator
    {
        public DependencyCoordinator_Core(IFoundation iFoundation)
            : base(iFoundation)
        {
            this.API = new <xsl:value-of select="items/@projectName"/>API(iFoundation);
        }
        public virtual <xsl:value-of select="items/@projectName"/>API API { get; set; }
        
        <xsl:for-each select="items/item">public virtual void <xsl:value-of select="@name"/>Invalidated(Dependency affectedDependencies, Guid <xsl:value-of select="field[1]" /><xsl:for-each select="field[@tenant='true' and not(@isolated='true')]">, <xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/></xsl:for-each>)
        {
            base.ExecuteMethod("<xsl:value-of select="@name"/>Invalidated", delegate ()
            {
                DependencyWorker&lt;<xsl:value-of select="@name"/>&gt;.EnqueueRequest(this.IFoundation, affectedDependencies, <xsl:value-of select="field[1]" />, <xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="text()"/>, </xsl:for-each>this.Process<xsl:value-of select="@name"/>Invalidation);
            });
        }
        protected virtual void Process<xsl:value-of select="@name"/>Invalidation(Dependency dependencies, <xsl:for-each select="field[@tenant='true' and not(@isolated='true')]"><xsl:value-of select="@type"/><xsl:text> </xsl:text><xsl:value-of select="text()"/>, </xsl:for-each>Guid <xsl:value-of select="field[1]" />)
        {
            base.ExecuteMethod("Process<xsl:value-of select="@name"/>Invalidation", delegate ()
            {
                <xsl:variable name="currentName"><xsl:value-of select="@name"/></xsl:variable>
                <xsl:variable name="currentField"><xsl:value-of select="field[1]" /></xsl:variable>
                <xsl:for-each select="../item/field[@foreignKey=$currentName and @foreignKeyInvalidatesMe='true']">
                this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template>.InvalidateFor<xsl:value-of select="@friendlyName"/>(<xsl:for-each select="../field[@tenant='true']"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="$currentField"/>, "<xsl:value-of select="$currentName"/> changed");
                </xsl:for-each>
                <xsl:if test="count(field[@iInvalidateforeignKey='true'])>0">
                <xsl:value-of select="@name"/> item = this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>.GetById(<xsl:for-each select="field[@tenant='true']"><xsl:value-of select="text()"/>, </xsl:for-each><xsl:value-of select="$currentField"/>);
                </xsl:if>
                <xsl:for-each select="field[@iInvalidateforeignKey='true']">
                <xsl:variable name="currentForeignKey"><xsl:value-of select="@foreignKey" /></xsl:variable>
                if (item != null<xsl:if test="@isNullable='true'"> &amp;&amp; item.<xsl:value-of select="text()"/>.HasValue</xsl:if>)
                {
                    this.API.Direct.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@foreignKey"/></xsl:call-template>.Invalidate(<xsl:for-each select="../../item[@name=$currentForeignKey]/field[@tenant='true']">item.<xsl:value-of select="text()"/>, </xsl:for-each>item.<xsl:value-of select="text()"/><xsl:if test="@isNullable='true'">.Value</xsl:if>, "<xsl:value-of select="$currentName"/> changed");
                }
                </xsl:for-each>
                <xsl:if test="count(../item/field[@foreignKey=$currentName and @foreignKeyInvalidatesMe='true'])>0 or count(field[@iInvalidateforeignKey='true'])>0">
                this.API.Integration.Synchronization.AgitateSyncDaemon(<xsl:for-each select="field[@tenant='true']"><xsl:if test="position()>1">, </xsl:if><xsl:value-of select="text()"/></xsl:for-each><xsl:if test="count(field[@tenant='true'])=0">null</xsl:if>);</xsl:if>
            });
        }
        </xsl:for-each>
    }
}


'''[ENDFILE]


'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Data.Sql\Extensions\DatabaseExtensions.cs]using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using db = <xsl:value-of select="items/@projectName"/>.Data.Sql.Models;

namespace <xsl:value-of select="items/@projectName"/>.Data.Sql
{
    public static class DatabaseExtensions
    {
        <xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">
        public static void InvalidateSync(this db.<xsl:value-of select="@name"/> model, string agent, string reason)
        {
            if (model != null)
            {
                model.sync_attempt_utc = null;
                model.sync_success_utc = null;
                model.sync_hydrate_utc = null;
                model.sync_log = reason;
                model.sync_invalid_utc = DateTime.UtcNow;
                model.sync_agent = agent;
            }
        }
        </xsl:for-each>
    }
}
'''[ENDFILE]

<xsl:for-each select="items/item[@useIndex='true' and not(@byPassIndex='true')]">

'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Index\I<xsl:value-of select="@name"/>Index_Core.cs]using <xsl:value-of select="../@foundation"/>.Foundation;
using <xsl:value-of select="../@projectName"/>.SDK.Models;
using <xsl:value-of select="../@projectName"/>.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Index
{
    public partial interface I<xsl:value-of select="@name" />Index : IIndexer&lt;<xsl:value-of select="@name" />&gt;
    {
        <xsl:value-of select="@name" /> GetById(<xsl:if test="string-length(@indexGrandParent) > 0">Guid grandParentId, </xsl:if><xsl:if test="string-length(@indexParent) > 0">Guid parentId, </xsl:if>Guid id);
        TCustomModel GetById&lt;TCustomModel&gt;(<xsl:if test="string-length(@indexGrandParent) > 0">Guid grandParentId, </xsl:if><xsl:if test="string-length(@indexParent) > 0">Guid parentId, </xsl:if>Guid id)
            where TCustomModel : class;
        <xsl:if test="string-length(@userSpecificData)>0">
        <xsl:value-of select="@name"/> GetById(<xsl:if test="string-length(@indexGrandParent) > 0">Guid grandParentId, </xsl:if><xsl:if test="string-length(@indexParent) > 0">Guid parentId, </xsl:if>Guid id, Guid? for_<xsl:value-of select="@userSpecificData"/>);
        </xsl:if>
        <xsl:for-each select="field[@foreignKey and not(@noGet='true')]">ListResult&lt;<xsl:value-of select="../@name"/>&gt; GetBy<xsl:value-of select="@friendlyName" />(Guid <xsl:value-of select="@foreignKeyField"/>, int skip, int take, string order_by = "", bool descending = false<xsl:if test="count(../field[@searchable='true']) > 0 or count(../indexfield[@searchable='true']) > 0">, string keyword = ""</xsl:if><xsl:if test="string-length(../@pagingWindow)>0">, DateTime? before_<xsl:value-of select="../@pagingWindow" /> = null</xsl:if><xsl:if test="string-length(../@userSpecificData)>0">, Guid? for_<xsl:value-of select="../@userSpecificData"/> = null</xsl:if><xsl:for-each select="../field[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text>with_<xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text>with_<xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>);
        </xsl:for-each>
        <xsl:for-each select="indexfield[@lookup='true']">ListResult&lt;<xsl:value-of select="../@name"/>&gt; GetBy<xsl:value-of select="@friendlyName" />(Guid <xsl:value-of select="text()"/>, int skip, int take, string order_by = "", bool descending = false<xsl:if test="string-length(../@userSpecificData)>0">, Guid? for_<xsl:value-of select="../@userSpecificData"/> = null</xsl:if><xsl:for-each select="../field[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text>with_<xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text>with_<xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>);
        </xsl:for-each>
       <xsl:if test="count(field[@searchable='true']) > 0 or count(indexfield[@searchable='true']) > 0">
        ListResult&lt;<xsl:value-of select="@name"/>&gt; Find(<xsl:if test="string-length(@userSpecificData)>0">Guid? for_<xsl:value-of select="@userSpecificData"/>, </xsl:if>int skip, int take, string keyword = "", string order_by = "", bool descending = false<xsl:for-each select="field[@foreignKey and not(@noGet='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text>with_<xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text>with_<xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>);
       </xsl:if>
        <xsl:if test="string-length(@indexGrandParent) = 0 and string-length(@indexParent) = 0">
       IndexResult UpdateDocumentPartial(Guid id, object partialModel);
       </xsl:if>
        <xsl:variable name="nameCache"><xsl:value-of select="@name"/></xsl:variable>
        <xsl:for-each select="../item/field[@computedFrom = $nameCache]">
         <xsl:variable name="computedFrom"><xsl:value-of select="@computedFrom"/></xsl:variable>
         <xsl:variable name="computedType"><xsl:value-of select="@type"/></xsl:variable>
         <xsl:variable name="computedField"><xsl:value-of select="@computedField" /></xsl:variable>
         <xsl:variable name="computedBy"><xsl:value-of select="@computedBy" /></xsl:variable>
         <xsl:variable name="primaryKeyCache"><xsl:value-of select="../field[1]" /></xsl:variable>

         <xsl:for-each select="../../item[@name=$computedFrom]">
        <xsl:choose><xsl:when test="$computedBy='Count'">
        <xsl:value-of select="$computedType"/> Get<xsl:value-of select="$computedBy"/><xsl:value-of select="field[text()=$computedField]/@friendlyName" />(Guid <xsl:value-of select="$primaryKeyCache"/>);
        </xsl:when></xsl:choose>
         </xsl:for-each>
        </xsl:for-each>
        <xsl:for-each select="../item/indexfield[@computedFrom = $nameCache]">
         <xsl:variable name="computedFrom"><xsl:value-of select="@computedFrom"/></xsl:variable>
         <xsl:variable name="computedType"><xsl:value-of select="@type"/></xsl:variable>
         <xsl:variable name="computedField"><xsl:value-of select="@computedField" /></xsl:variable>
         <xsl:variable name="computedBy"><xsl:value-of select="@computedBy" /></xsl:variable>
         <xsl:variable name="primaryKeyCache"><xsl:value-of select="../field[1]" /></xsl:variable>

         <xsl:for-each select="../../item[@name=$computedFrom]">
        <xsl:choose><xsl:when test="$computedBy='Count'">
        <xsl:value-of select="$computedType"/> Get<xsl:value-of select="$computedBy"/><xsl:value-of select="field[text()=$computedField]/@friendlyName" />(Guid <xsl:value-of select="$primaryKeyCache"/>);
        </xsl:when></xsl:choose>
         </xsl:for-each>
        </xsl:for-each>
    }
}
'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="../@projectName"/>.Primary\Business\Index\Implementation\<xsl:value-of select="@name"/>Index_Core.cs]using <xsl:value-of select="../@foundation"/>.Foundation;
using <xsl:value-of select="../@projectName"/>.SDK;
using sdk = <xsl:value-of select="../@projectName"/>.SDK.Models;
using <xsl:value-of select="../@projectName"/>.SDK.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace <xsl:value-of select="../@projectName"/>.Primary.Business.Index.Implementation
{
    public partial class <xsl:value-of select="@name"/>Index : DualIndexer<xsl:if test="string-length(@indexGrandParent) > 0">Grand</xsl:if><xsl:if test="string-length(@indexParent) > 0">Child</xsl:if>Base&lt;sdk.<xsl:value-of select="@name" />&gt;, I<xsl:value-of select="@name" />Index
    {
        public <xsl:value-of select="@name"/>Index(IFoundation foundation)
            : base(foundation, "<xsl:value-of select="@name"/>Index", DocumentNames.<xsl:value-of select="@name"/>)
        {

        }
        protected override string GetModelId(sdk.<xsl:value-of select="@name"/> model)
        {
            return model.<xsl:value-of select="field[1]" />.ToString();
        }
        <xsl:if test="string-length(@indexGrandParent) > 0">
        protected override string GetGrandParentId(sdk.<xsl:value-of select="@name"/> model)
        {
            <xsl:for-each select="field[@indexGrandParent='true']">return model.<xsl:value-of select="text()" />.ToString();</xsl:for-each>
        }
        </xsl:if>
        <xsl:if test="string-length(@indexParent) > 0">
        protected override string GetParentId(sdk.<xsl:value-of select="@name"/> model)
        {
            <xsl:for-each select="field[@indexParent='true']">return model.<xsl:value-of select="text()" />.ToString();</xsl:for-each>
        }
        </xsl:if>
        
        <xsl:if test="string-length(@userSpecificData)>0">
        public virtual sdk.<xsl:value-of select="@name"/> GetById(<xsl:if test="string-length(@indexGrandParent) > 0">Guid grandParentId, </xsl:if><xsl:if test="string-length(@indexParent) > 0">Guid parentId, </xsl:if>Guid id, Guid? for_<xsl:value-of select="@userSpecificData"/>)
        {
            return base.ExecuteFunction("GetById", delegate ()
            {
                sdk.<xsl:value-of select="@name"/> result = base.GetById(<xsl:if test="string-length(@indexGrandParent) > 0">grandParentId, </xsl:if><xsl:if test="string-length(@indexParent) > 0">parentId, </xsl:if>id);
                if(result != null)
                {
                    this.PostProcessForUser(new List&lt;sdk.<xsl:value-of select="@name"/>&gt;() { result }, for_<xsl:value-of select="@userSpecificData"/>);
                    <xsl:if test="count(indexfield[@sensitive='true'])>0 or count(field[@sensitive='true'])>0">
                    this.PostProcessSensitive(new List&lt;sdk.<xsl:value-of select="@name"/>&gt;() { result });
                    </xsl:if>
                }
                return result;
            });
        }
        </xsl:if>
        
        <xsl:for-each select="field[@foreignKey and not(@noGet='true')]">public ListResult&lt;sdk.<xsl:value-of select="../@name"/>&gt; GetBy<xsl:value-of select="@friendlyName" />(Guid <xsl:value-of select="@foreignKeyField"/>, int skip, int take, string order_by = "", bool descending = false<xsl:if test="count(../field[@searchable='true']) > 0 or count(../indexfield[@searchable='true']) > 0">, string keyword = ""</xsl:if><xsl:if test="string-length(../@pagingWindow)>0">, DateTime? before_<xsl:value-of select="../@pagingWindow" /> = null</xsl:if><xsl:if test="string-length(../@userSpecificData)>0">, Guid? for_<xsl:value-of select="../@userSpecificData"/> = null</xsl:if><xsl:for-each select="../field[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text> <xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction("GetBy<xsl:value-of select="@friendlyName" />", delegate ()
            {
                QueryContainer query = Query&lt;sdk.<xsl:value-of select="../@name"/>&gt;.Term(w => w.<xsl:value-of select="text()"/>, <xsl:value-of select="@foreignKeyField"/>);
                
                <xsl:if test="count(../field[@searchable='true']) > 0 or count(../indexfield[@searchable='true']) > 0">
                if(!string.IsNullOrEmpty(keyword))
                {
                    query &amp;= Query&lt;sdk.<xsl:value-of select="../@name"/>&gt;
                        .MultiMatch(m => m
                            .Query(keyword)
                            .Type(TextQueryType.Phrase<xsl:if test="not(@searchNonPrefix='true')">Prefix</xsl:if>)
                            .Fields(mf => mf<xsl:for-each select="../field[@searchable='true']">
                                    .Field(f => f.<xsl:value-of select="text()"/>)</xsl:for-each><xsl:for-each select="../indexfield[@searchable='true']">
                                    .Field(f => f.<xsl:value-of select="text()"/>)</xsl:for-each>
                    ));
                }
                </xsl:if>

                <xsl:if test="string-length(../@pagingWindow)>0">if(before_<xsl:value-of select="../@pagingWindow" />.HasValue)
                {
                    query &amp;= Query&lt;sdk.<xsl:value-of select="../@name"/>&gt;.DateRange(r => r.Field(f => f.<xsl:value-of select="../@pagingWindow" />).LessThanOrEquals(before_<xsl:value-of select="../@pagingWindow" />.Value));
                }
                </xsl:if>
                <xsl:variable name="searchParentName"><xsl:value-of select="../@name"/></xsl:variable>
                <xsl:for-each select="../field[string-length(@searchToggle)>0]">if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query &amp;= Query&lt;<xsl:value-of select="$searchParentName"/>&gt;.Term(f => f.<xsl:value-of select="text()"/>, <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>
                <xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query &amp;= Query&lt;<xsl:value-of select="$searchParentName"/>&gt;.Term(f => f.<xsl:value-of select="text()"/>, <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>

                int takePlus = take;
                if(take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }
                
                List&lt;SortFieldDescriptor&lt;sdk.<xsl:value-of select="../@name"/>&gt;&gt; sortFields = new List&lt;SortFieldDescriptor&lt;sdk.<xsl:value-of select="../@name"/>&gt;&gt;();
                if(!string.IsNullOrEmpty(order_by))
                {
                    SortFieldDescriptor&lt;sdk.<xsl:value-of select="../@name"/>&gt; item = new SortFieldDescriptor&lt;sdk.<xsl:value-of select="../@name"/>&gt;()
                        .Field(order_by)
                        .Order(descending ? SortOrder.Descending : SortOrder.Ascending);
                        
                    sortFields.Add(item);
                }
                SortFieldDescriptor&lt;sdk.<xsl:value-of select="../@name"/>&gt; defaultSort = new SortFieldDescriptor&lt;sdk.<xsl:value-of select="../@name"/>&gt;()
                    .Field(r => r.<xsl:choose><xsl:when test="string-length(../@uiDefaultSort)>0"><xsl:value-of select="../@uiDefaultSort" /></xsl:when><xsl:otherwise><xsl:value-of select="../field[1]" /></xsl:otherwise></xsl:choose>)
                    .<xsl:choose><xsl:when test="../@uiDefaultSortDescending='true'">Descending()</xsl:when><xsl:otherwise>Ascending()</xsl:otherwise></xsl:choose>;
                
                sortFields.Add(defaultSort);
                
                IElasticClient client = this.ClientFactory.CreateReadClient();
                ISearchResponse&lt;sdk.<xsl:value-of select="../@name"/>&gt; searchResponse = client.Search&lt;sdk.<xsl:value-of select="../@name"/>&gt;(s => s
                    .Query(q => query)
                    .Skip(skip)
                    .Take(takePlus)
                    .Sort(sr => sr.Multi(sortFields))
                    .Type(this.DocumentType));

                ListResult&lt;sdk.<xsl:value-of select="../@name"/>&gt; result = searchResponse.Documents.ToSteppedListResult(skip, take, searchResponse.GetTotalHit());
                <xsl:if test="string-length(../@userSpecificData)>0">
                this.PostProcessForUser(result.items, for_<xsl:value-of select="../@userSpecificData"/>);
                </xsl:if>
                <xsl:if test="count(../indexfield[@sensitive='true'])>0 or count(../field[@sensitive='true'])>0">
                this.PostProcessSensitive(result.items);
                </xsl:if>
                return result;
            });
        }
        </xsl:for-each>

        <xsl:for-each select="indexfield[@lookup='true']">public ListResult&lt;sdk.<xsl:value-of select="../@name"/>&gt; GetBy<xsl:value-of select="@friendlyName" />(Guid <xsl:value-of select="text()"/>, int skip, int take, string order_by = "", bool descending = false<xsl:if test="string-length(../@userSpecificData)>0">, Guid? for_<xsl:value-of select="../@userSpecificData"/> = null</xsl:if><xsl:for-each select="../field[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text> <xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">, <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text><xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction("GetBy<xsl:value-of select="@friendlyName" />", delegate ()
            {
                QueryContainer query = Query&lt;sdk.<xsl:value-of select="../@name"/>&gt;.Term(w => w.<xsl:value-of select="text()"/>, <xsl:value-of select="text()"/>);

                <xsl:variable name="searchParentName"><xsl:value-of select="../@name"/></xsl:variable>
                <xsl:for-each select="../field[string-length(@searchToggle)>0]">if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query &amp;= Query&lt;<xsl:value-of select="$searchParentName"/>&gt;.Term(f => f.<xsl:value-of select="text()"/>, <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>
                <xsl:for-each select="../indexfield[string-length(@searchToggle)>0]">if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query &amp;= Query&lt;<xsl:value-of select="$searchParentName"/>&gt;.Term(f => f.<xsl:value-of select="text()"/>, <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>

                int takePlus = take;
                if(take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }
                
                List&lt;SortFieldDescriptor&lt;<xsl:value-of select="../@name"/>&gt;&gt; sortFields = new List&lt;SortFieldDescriptor&lt;<xsl:value-of select="../@name"/>&gt;&gt;();
                if(!string.IsNullOrEmpty(order_by))
                {
                    SortFieldDescriptor&lt;<xsl:value-of select="../@name"/>&gt; item = new SortFieldDescriptor&lt;<xsl:value-of select="../@name"/>&gt;()
                        .Field(order_by)
                        .Order(descending ? SortOrder.Descending : SortOrder.Ascending);
                        
                    sortFields.Add(item);
                }
                SortFieldDescriptor&lt;<xsl:value-of select="../@name"/>&gt; defaultSort = new SortFieldDescriptor&lt;<xsl:value-of select="../@name"/>&gt;()
                    .Field(r => r.<xsl:choose><xsl:when test="string-length(../@uiDefaultSort)>0"><xsl:value-of select="../@uiDefaultSort" /></xsl:when><xsl:otherwise><xsl:value-of select="../field[1]" /></xsl:otherwise></xsl:choose>)
                    .<xsl:choose><xsl:when test="../@uiDefaultSortDescending='true'">Descending()</xsl:when><xsl:otherwise>Ascending()</xsl:otherwise></xsl:choose>;
                
                sortFields.Add(defaultSort);
                
                IElasticClient client = this.ClientFactory.CreateReadClient();
                ISearchResponse&lt;<xsl:value-of select="../@name"/>&gt; searchResponse = client.Search&lt;<xsl:value-of select="../@name"/>&gt;(s => s
                    .Query(q => query)
                    .Skip(skip)
                    .Take(takePlus)
                    .Sort(sr => sr.Multi(sortFields))
                    .Type(this.DocumentType));

                ListResult&lt;<xsl:value-of select="../@name"/>&gt; result = searchResponse.Documents.ToSteppedListResult(skip, take, searchResponse.GetTotalHit());
                <xsl:if test="string-length(../@userSpecificData)>0">
                this.PostProcessForUser(result.items, for_<xsl:value-of select="../@userSpecificData"/>);
                </xsl:if>
                <xsl:if test="count(../indexfield[@sensitive='true'])>0 or count(../field[@sensitive='true'])>0">
                this.PostProcessSensitive(result.items);
                </xsl:if>
                return result;
            });
        }
        </xsl:for-each>
        
        
        
        <xsl:variable name="name"><xsl:value-of select="@name"/></xsl:variable>
        
        <xsl:for-each select="../item/field[@computedFrom = $name]">
        <xsl:variable name="computedFrom"><xsl:value-of select="@computedFrom"/></xsl:variable>
        <xsl:variable name="computedField"><xsl:value-of select="@computedField" /></xsl:variable>
        <xsl:variable name="computedBy"><xsl:value-of select="@computedBy" /></xsl:variable>
        <xsl:variable name="computedType"><xsl:value-of select="@type"/></xsl:variable>
        <xsl:variable name="currentField"><xsl:value-of select="text()" /></xsl:variable>
        <xsl:variable name="primaryKey"><xsl:value-of select="../field[1]" /></xsl:variable>
        
        <xsl:for-each select="../../item[@name=$computedFrom]">
        <xsl:variable name="friendlyName"><xsl:value-of select="field[text()=$computedField]/@friendlyName" /></xsl:variable>
        <xsl:choose><xsl:when test="$computedBy='Count'">
        public <xsl:value-of select="$computedType"/> Get<xsl:value-of select="$computedBy"/><xsl:value-of select="$friendlyName"/>(Guid <xsl:value-of select="$primaryKey"/>)
        {
            return base.ExecuteFunction("Get<xsl:value-of select="$computedBy"/><xsl:value-of select="$friendlyName"/>", delegate ()
            {
                QueryContainer query = Query&lt;<xsl:value-of select="@name" />&gt;.Term(w => w.<xsl:value-of select="$primaryKey"/>, <xsl:value-of select="$primaryKey"/>);
                <xsl:if test="$computedBy='Count'">
                query &amp;= Query&lt;<xsl:value-of select="@name" />&gt;.Exists(f => f.Field(x => x.<xsl:value-of select="$computedField"/>));
               
                IElasticClient client = this.ClientFactory.CreateReadClient();
                ISearchResponse&lt;sdk.<xsl:value-of select="@name"/>&gt; response = client.Search&lt;sdk.<xsl:value-of select="@name"/>&gt;(s => s
                    .Query(q => query)
                    .Skip(0)
                    .Take(0)
                    .Type(this.DocumentType));

                 </xsl:if>
                return (int)response.GetTotalHit();
            });
        }
        </xsl:when></xsl:choose>

        </xsl:for-each>
        
        </xsl:for-each>
        
        <xsl:for-each select="../item/indexfield[@computedFrom = $name]">
        <xsl:variable name="computedFrom"><xsl:value-of select="@computedFrom"/></xsl:variable>
        <xsl:variable name="computedField"><xsl:value-of select="@computedField" /></xsl:variable>
        <xsl:variable name="computedBy"><xsl:value-of select="@computedBy" /></xsl:variable>
        <xsl:variable name="computedType"><xsl:value-of select="@type"/></xsl:variable>
        <xsl:variable name="currentField"><xsl:value-of select="text()" /></xsl:variable>
        <xsl:variable name="primaryKey"><xsl:value-of select="../field[1]" /></xsl:variable>
        
        <xsl:for-each select="../../item[@name=$computedFrom]">
        <xsl:variable name="friendlyName"><xsl:value-of select="field[text()=$computedField]/@friendlyName" /></xsl:variable>
        <xsl:choose><xsl:when test="$computedBy='Count'">
        public <xsl:value-of select="$computedType"/> Get<xsl:value-of select="$computedBy"/><xsl:value-of select="$friendlyName"/>(Guid <xsl:value-of select="$primaryKey"/>)
        {
            return base.ExecuteFunction("Get<xsl:value-of select="$computedBy"/><xsl:value-of select="$friendlyName"/>", delegate ()
            {
                QueryContainer query = Query&lt;<xsl:value-of select="@name" />&gt;.Term(w => w.<xsl:value-of select="$primaryKey"/>, <xsl:value-of select="$primaryKey"/>);
                <xsl:if test="$computedBy='Count'">
                query &amp;= Query&lt;<xsl:value-of select="@name" />&gt;.Exists(f => f.Field(x => x.<xsl:value-of select="$computedField"/>));
               
                IElasticClient client = this.ClientFactory.CreateReadClient();
                ISearchResponse&lt;sdk.<xsl:value-of select="@name"/>&gt; response = client.Search&lt;sdk.<xsl:value-of select="@name"/>&gt;(s => s
                    .Query(q => query)
                    .Skip(0)
                    .Take(0)
                    .Type(this.DocumentType));

                 </xsl:if>
                return (int)response.GetTotalHit();
            });
        }
        </xsl:when></xsl:choose>

        </xsl:for-each>
        
        </xsl:for-each>
        
        
        
        <xsl:if test="count(field[@searchable='true']) > 0 or count(indexfield[@searchable='true']) > 0">
        public ListResult&lt;sdk.<xsl:value-of select="@name"/>&gt; Find(<xsl:if test="string-length(@userSpecificData)>0">Guid? for_<xsl:value-of select="@userSpecificData"/>, </xsl:if>int skip, int take, string keyword = "", string order_by = "", bool descending = false<xsl:for-each select="field[@foreignKey and not(@noGet='true')]">, Guid? <xsl:value-of select="text()"/> = null</xsl:for-each><xsl:for-each select="field[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text> <xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each><xsl:for-each select="indexfield[string-length(@searchToggle)>0]">,  <xsl:value-of select="@type"/><xsl:if test="not(@type='string')">?</xsl:if><xsl:text> </xsl:text> <xsl:value-of select="text()"/> = <xsl:value-of select="@searchToggle"/></xsl:for-each>)
        {
            return base.ExecuteFunction("Find", delegate ()
            {
                int takePlus = take;
                if(take != int.MaxValue)
                {
                    takePlus++; // for stepping
                }

                <xsl:choose>
                <xsl:when test="@searchNonPrefix='true'">
                QueryContainer prefixQuery = Query&lt;sdk.<xsl:value-of select="@name"/>&gt;
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields(mf => mf<xsl:for-each select="field[@searchable='true']">
                                .Field(f => f.<xsl:value-of select="text()"/>)</xsl:for-each><xsl:for-each select="indexfield[@searchable='true']">
                                .Field(f => f.<xsl:value-of select="text()"/>)</xsl:for-each>
                ));
                QueryContainer nonPrefixQuery = Query&lt;sdk.<xsl:value-of select="@name"/>&gt;
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Type(TextQueryType.Phrase)
                        .Fields(mf => mf<xsl:for-each select="field[@searchable='true']">
                                .Field(f => f.<xsl:value-of select="text()"/>)</xsl:for-each><xsl:for-each select="indexfield[@searchable='true']">
                                .Field(f => f.<xsl:value-of select="text()"/>)</xsl:for-each>
                ));
                QueryContainer query = prefixQuery | nonPrefixQuery;
                </xsl:when>
                <xsl:otherwise>
                QueryContainer query = Query&lt;sdk.<xsl:value-of select="@name"/>&gt;
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields(mf => mf<xsl:for-each select="field[@searchable='true']">
                                .Field(f => f.<xsl:value-of select="text()"/>)</xsl:for-each><xsl:for-each select="indexfield[@searchable='true']">
                                .Field(f => f.<xsl:value-of select="text()"/>)</xsl:for-each>
                ));
                </xsl:otherwise></xsl:choose>
                
                                
                <xsl:variable name="searchParentName"><xsl:value-of select="@name"/></xsl:variable>
                <xsl:for-each select="field[@foreignKey and not(@noGet='true')]">if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query &amp;= Query&lt;sdk.<xsl:value-of select="$searchParentName"/>&gt;.Term(f => f.<xsl:value-of select="text()"/>, <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>
                <xsl:for-each select="field[string-length(@searchToggle)>0]">if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query &amp;= Query&lt;sdk.<xsl:value-of select="$searchParentName"/>&gt;.Term(f => f.<xsl:value-of select="text()"/>, <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>
                <xsl:for-each select="indexfield[string-length(@searchToggle)>0]">if(<xsl:value-of select="text()"/>.HasValue)
                {
                    query &amp;= Query&lt;sdk.<xsl:value-of select="$searchParentName"/>&gt;.Term(f => f.<xsl:value-of select="text()"/>, <xsl:value-of select="text()"/>.Value);
                }
                </xsl:for-each>
                
                SortOrder sortOrder = SortOrder.Ascending;
                if (descending)
                {
                    sortOrder = SortOrder.Descending;
                }
                if (string.IsNullOrEmpty(order_by))
                {
                    order_by = "<xsl:choose><xsl:when test="string-length(../@uiDefaultSort)>0"><xsl:value-of select="../@uiDefaultSort" /></xsl:when><xsl:otherwise><xsl:value-of select="../@uiDisplayField" /></xsl:otherwise></xsl:choose>";
                }

                IElasticClient client = this.ClientFactory.CreateReadClient();
                ISearchResponse&lt;sdk.<xsl:value-of select="@name"/>&gt; searchResponse = client.Search&lt;sdk.<xsl:value-of select="@name"/>&gt;(s => s
                    .Query(q => query)
                    .Skip(skip)
                    .Take(takePlus)
                    .Sort(r => r.Field(order_by, sortOrder))
                    .Type(this.DocumentType));
                
                ListResult&lt;sdk.<xsl:value-of select="@name"/>&gt; result = searchResponse.Documents.ToSteppedListResult(skip, take, searchResponse.GetTotalHit());
                <xsl:if test="string-length(@userSpecificData)>0">
                this.PostProcessForUser(result.items, for_<xsl:value-of select="@userSpecificData"/>);
                </xsl:if>
                <xsl:if test="count(indexfield[@sensitive='true'])>0 or count(field[@sensitive='true'])>0">
                this.PostProcessSensitive(result.items);
                </xsl:if>
                return result;
            });
        }
        </xsl:if>
        
        <xsl:if test="string-length(@userSpecificData)>0">
        partial void PostProcessForUser(List&lt;<xsl:value-of select="@name"/>&gt; items, Guid? <xsl:value-of select="@userSpecificData"/>);
        </xsl:if>

        <xsl:if test="count(indexfield[@sensitive='true'])>0  or count(field[@sensitive='true'])>0">
        protected void PostProcessSensitive&lt;TModel&gt;(List&lt;TModel&gt; items)
        {
            if(items != null)
            {
                foreach(var item in items)
                {
                    this.PostProcessSensitive(item);
                }
            }
        }
        protected void PostProcessSensitive&lt;TModel&gt;(TModel item)
        {
            if(item is <xsl:value-of select="@name"/>)
            {
               <xsl:for-each select="indexfield[@sensitive='true']">
               (item as <xsl:value-of select="../@name"/>).<xsl:value-of select="text()"/> = default(<xsl:value-of select="@type"/>);</xsl:for-each>
               <xsl:for-each select="field[@sensitive='true']">
               (item as <xsl:value-of select="../@name"/>).<xsl:value-of select="text()"/> = default(<xsl:value-of select="@type"/>);</xsl:for-each>
            }
        }
        </xsl:if>

    }
}
'''[ENDFILE]

</xsl:for-each>

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\<xsl:value-of select="items/@projectName"/>APIDirect.cs]using <xsl:value-of select="items/@foundation"/>.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using <xsl:value-of select="items/@projectName"/>.Primary.Business.Direct;

namespace <xsl:value-of select="items/@projectName"/>.Primary
{
    public class <xsl:value-of select="items/@projectName"/>APIDirect : BaseClass
    {
        public <xsl:value-of select="items/@projectName"/>APIDirect(IFoundation ifoundation)
            : base(ifoundation)
        {
        }
        <xsl:for-each select="items/item">public I<xsl:value-of select="@name" />Business <xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="@name"/></xsl:call-template>
        {
            get { return this.IFoundation.Resolve&lt;I<xsl:value-of select="@name" />Business&gt;(); }
        }
        </xsl:for-each>
    }
}


'''[ENDFILE]


<!-- <xsl:choose><xsl:when test=""></xsl:when><xsl:otherwise></xsl:otherwise></xsl:choose> -->
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
          <xsl:choose>
            <xsl:when test="substring($inputString, string-length($inputString)) = 'x'"><xsl:value-of select="$inputString"/>es</xsl:when>
            <xsl:when test="substring($inputString, string-length($inputString)-1) = 'ch'"><xsl:value-of select="$inputString"/>es</xsl:when>
            <xsl:when test="substring($inputString, string-length($inputString)) = 'y'"><xsl:value-of select="concat(substring($inputString, 1, string-length($inputString)-1),'ies')"/></xsl:when>
            <xsl:otherwise><xsl:value-of select="$inputString"/>s</xsl:otherwise></xsl:choose>
  </xsl:template>
  <xsl:template name="ExtractCharacterSize">
    <xsl:param name="text" />
    <xsl:call-template name="Replace">
        <xsl:with-param name="text"><xsl:call-template name="Replace">
        <xsl:with-param name="text" select="$text" />
        <xsl:with-param name="replace" select="'nvarchar('" />
        <xsl:with-param name="by" select="''" />
    </xsl:call-template></xsl:with-param>
        <xsl:with-param name="replace" select="')'" />
        <xsl:with-param name="by" select="''" />
    </xsl:call-template>
</xsl:template>
  

  <xsl:template name="Replace">
    <xsl:param name="text" />
    <xsl:param name="replace" />
    <xsl:param name="by" />
    <xsl:choose>
        <xsl:when test="contains($text, $replace)">
        <xsl:value-of select="substring-before($text,$replace)" />
        <xsl:value-of select="$by" />
        <xsl:call-template name="Replace">
            <xsl:with-param name="text" select="substring-after($text,$replace)" />
            <xsl:with-param name="replace" select="$replace" />
            <xsl:with-param name="by" select="$by" />
        </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
        <xsl:value-of select="$text" />
        </xsl:otherwise>
    </xsl:choose>
</xsl:template>
</xsl:stylesheet>