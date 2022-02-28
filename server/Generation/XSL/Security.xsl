<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">

'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Security\ISecurityEnforcer_Crud.cs]using System;
using System.Collections.Generic;
using System.Text;
using <xsl:value-of select="items/@projectName"/>.Domain;
using sdk = <xsl:value-of select="items/@projectName"/>.SDK.Models;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Security
{
    // WARNING: THIS FILE IS GENERATED
    public partial interface ISecurityEnforcer
    {
        <xsl:for-each select="items/item">
            <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
            <xsl:variable name="removePattern2"><xsl:value-of select="@removePattern"/></xsl:variable>
        void ValidateCanCreate(Account currentAccount, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>);
        void ValidateCanSearch<xsl:value-of select="@name"/>(Account currentAccount, Guid? <xsl:value-of select="../@securityRoute"/> = null);
        void ValidateCanList<xsl:value-of select="@name"/>(Account currentAccount, Guid? <xsl:value-of select="../@securityRoute"/> = null);
        void ValidateCanRetrieve(Account currentAccount, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>);
        void ValidateCanUpdate(Account currentAccount, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>);
        void ValidateCanDelete(Account currentAccount, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>);
        </xsl:for-each>
    }
}

'''[ENDFILE]


'''[STARTFILE:<xsl:value-of select="items/@projectName"/>.Primary\Security\SecurityEnforcerBase_Crud.cs]using System;
using System.Collections.Generic;
using System.Text;
using <xsl:value-of select="items/@projectName"/>.Common;
using <xsl:value-of select="items/@projectName"/>.Domain;
using <xsl:value-of select="items/@projectName"/>.Common.Exceptions;
using sdk = <xsl:value-of select="items/@projectName"/>.SDK.Models;

namespace <xsl:value-of select="items/@projectName"/>.Primary.Security
{
    // WARNING: THIS FILE IS GENERATED
    public partial class SecurityEnforcerBase : ISecurityEnforcer
    {
        <xsl:for-each select="items/item">
            <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="@name"/></xsl:call-template></xsl:variable>
            <xsl:variable name="removePattern2"><xsl:value-of select="@removePattern"/></xsl:variable>
        #region <xsl:value-of select="@name"/> Methods

        public virtual void ValidateCanCreate(Account currentAccount, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            <xsl:if test="count(create[@trimmed='true'])> 0">
            <xsl:for-each select="create[@trimmed='true']">
            <xsl:call-template name="crudBody"></xsl:call-template>
            </xsl:for-each>
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Create());
            }
            </xsl:if>
        }
        public virtual void ValidateCanRetrieve(Account currentAccount, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            <xsl:if test="count(retrieve[@trimmed='true'])> 0">
            <xsl:for-each select="retrieve[@trimmed='true']">
            <xsl:call-template name="crudBody"></xsl:call-template>
            </xsl:for-each>
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Retrieve());
            }
            </xsl:if>
        }
        public virtual void ValidateCanSearch<xsl:value-of select="@name"/>(Account currentAccount, Guid? <xsl:value-of select="../@securityRoute"/> = null)
        {
            <xsl:if test="count(search[@trimmed='true'])> 0">
            <xsl:for-each select="search[@trimmed='true']">
            <xsl:call-template name="searchBody"></xsl:call-template>
            </xsl:for-each>
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Search());
            }
            </xsl:if>
        }
        public virtual void ValidateCanList<xsl:value-of select="@name"/>(Account currentAccount, Guid? <xsl:value-of select="../@securityRoute"/> = null)
        {
            <xsl:if test="count(list[@trimmed='true'])> 0">
            <xsl:for-each select="list[@trimmed='true']">
            <xsl:call-template name="listBody"></xsl:call-template>
            </xsl:for-each>
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_List());
            }
            </xsl:if>
        }
        public virtual void ValidateCanUpdate(Account currentAccount, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            <xsl:if test="count(update[@trimmed='true'])> 0">
            <xsl:for-each select="update[@trimmed='true']">
            <xsl:call-template name="crudBody"></xsl:call-template>
            </xsl:for-each>
            
            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Update());
            }
            </xsl:if>
        }
        public virtual void ValidateCanDelete(Account currentAccount, <xsl:value-of select="@name"/><xsl:text> </xsl:text><xsl:value-of select="$name_lowered"/>)
        {
            <xsl:if test="count(delete[@trimmed='true'])> 0">
            <xsl:for-each select="delete[@trimmed='true']">
            <xsl:call-template name="crudBody"></xsl:call-template>
            </xsl:for-each>

            if (!allowed)
            {
                throw new UISecurityException(LocalizableString.General_AccessDenied_Delete());
            }
            </xsl:if>
        }

        #endregion
        </xsl:for-each>
    }
}

'''[ENDFILE]

</xsl:template>
<xsl:template name="crudBody">
    <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template></xsl:variable>bool allowed = false;
            <xsl:if test="@super='true'">
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            </xsl:if>
            <xsl:if test="@admin='true'">
            if(!allowed)
            {
                allowed = currentAccount.IsAdmin();
            }
            </xsl:if>
            <xsl:if test="@self='true'">
            if(!allowed)
            {
                // Try Self
                allowed = (currentAccount.account_id == <xsl:value-of select="$name_lowered"/>.<xsl:choose><xsl:when test="string-length(../@selfField)>0"><xsl:value-of select="../@selfField" /></xsl:when><xsl:otherwise>account_id</xsl:otherwise></xsl:choose>);
            }
            </xsl:if>
            <xsl:if test="@owner='true'">
            if(!allowed &amp;&amp; <xsl:value-of select="$name_lowered"/> != null)
            {
                // Get <xsl:value-of select="../@securityEntity"/>
                Guid? <xsl:value-of select="../@securityRoute"/> = <xsl:choose><xsl:when test="string-length(../@routeField)>0 and string-length(../@routeEntity)=0"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="../@routeField"/></xsl:when><xsl:when test="string-length(../@routeField)>0 and string-length(../@routeEntity)=0"><xsl:value-of select="$name_lowered"/>.<xsl:value-of select="../@routeField"/></xsl:when><xsl:otherwise>null</xsl:otherwise></xsl:choose>;
                <xsl:if test="string-length(../@routeEntity)>0">sdk.<xsl:value-of select="../@routeEntity" /> route = this.CachedExecute("<xsl:value-of select="../@routeEntity"/>", <xsl:value-of select="$name_lowered"/>.<xsl:value-of select="../@routeField"/>, this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@routeEntity"/></xsl:call-template>.GetById);
                if (route != null)
                {
                    <xsl:choose><xsl:when test="string-length(../@routeEntityParent)>0">sdk.<xsl:value-of select="../@routeEntityParent" /> parentRoute = this.CachedExecute("<xsl:value-of select="../@routeEntityParent"/>", route.<xsl:value-of select="../@routeFieldParent"/>, this.API.Index.<xsl:call-template name="Pluralize"><xsl:with-param name="inputString" select="../@routeEntityParent"/></xsl:call-template>.GetById);
                    <xsl:value-of select="../@securityRoute"/> = parentRoute.<xsl:value-of select="../@securityRoute"/>;</xsl:when>
                    <xsl:otherwise><xsl:value-of select="../@securityRoute"/> = route.<xsl:value-of select="../@securityRoute"/>;</xsl:otherwise></xsl:choose>
                }
                </xsl:if>
                if (<xsl:value-of select="../@securityRoute"/> != null)
                {
                    <xsl:if test="@owner='true'">if (!allowed &amp;&amp; this.Is<xsl:value-of select="../@securityEntity"/>Role(currentAccount, <xsl:value-of select="../@securityRoute"/>.Value, sdk.<xsl:value-of select="../@securityEntity"/>Role.owner))
                    {
                        allowed = true;
                    }
                    </xsl:if>
                }
            }</xsl:if>
</xsl:template>
<xsl:template name="searchBody">
    <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template></xsl:variable>bool allowed = false;
            <xsl:if test="@super='true'">
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            </xsl:if>
            <xsl:if test="@admin='true'">
            if(!allowed)
            {
                allowed = currentAccount.IsAdmin();
            }
            </xsl:if>
            <xsl:if test="@owner='true'">
            if(!allowed &amp;&amp; <xsl:value-of select="../@securityRoute"/> != null)
            {
                <xsl:if test="@owner='true'">if (!allowed &amp;&amp; this.Is<xsl:value-of select="../@securityEntity"/>Role(currentAccount, <xsl:value-of select="../@securityRoute"/>.Value, sdk.<xsl:value-of select="../@securityEntity"/>Role.owner))
                {
                    allowed = true;
                }
                </xsl:if>
            }</xsl:if>
</xsl:template>
<xsl:template name="listBody">
    <xsl:variable name="name_lowered"><xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@name"/></xsl:call-template></xsl:variable>bool allowed = false;
            <xsl:if test="@super='true'">
            if(!allowed)
            {
                allowed = currentAccount.IsSuperAdmin();
            }
            </xsl:if>
            <xsl:if test="@admin='true'">
            if(!allowed)
            {
                allowed = currentAccount.IsAdmin();
            }
            </xsl:if>
            <xsl:if test="@owner='true'">
            if(!allowed &amp;&amp; <xsl:value-of select="../@securityRoute"/> != null)
            {
                <xsl:if test="@owner='true'">if (!allowed &amp;&amp; this.I<xsl:value-of select="../@securityEntity"/>Role(currentAccount, <xsl:value-of select="../@securityRoute"/>.Value, sdk.<xsl:value-of select="../@securityEntity"/><xsl:value-of select="../@securityEntity"/>Role.owner))
                {
                    allowed = true;
                }
                </xsl:if>
            }</xsl:if>
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
<xsl:template name="Pluralize">
          <xsl:param name="inputString"/>
          <xsl:choose><xsl:when test="substring($inputString, string-length($inputString)) = 'y'"><xsl:value-of select="concat(substring($inputString, 1, string-length($inputString)-1),'ies')"/></xsl:when><xsl:otherwise><xsl:value-of select="$inputString"/>s</xsl:otherwise></xsl:choose>
  </xsl:template>
</xsl:stylesheet>