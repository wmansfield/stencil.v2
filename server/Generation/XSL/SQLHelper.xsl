<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">

'''[STARTFILE:<xsl:value-of select="../@folderName"/>\Scripts\SQL\create_helper.sql]

-- &lt;Tables&gt; --------------------------------------------------------------------
<xsl:for-each select="items/item">
CREATE TABLE [<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@schema"/></xsl:call-template>].[<xsl:value-of select="@name" />] (
	 <xsl:for-each select="field[not(@derivedProperty='true')]"><xsl:if test="position() > 1">,</xsl:if>[<xsl:value-of select="text()" />] <xsl:value-of select="@dbType" /><xsl:if test="@dbType!='rowversion'"><xsl:if test="not(@isNullable='true')"> NOT</xsl:if> NULL</xsl:if><xsl:text>
    </xsl:text></xsl:for-each><xsl:if test="@useIndex='true' or @useStore='true'">,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL</xsl:if><xsl:if test="@trackUpdates='true'">,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL</xsl:if>
  ,CONSTRAINT [PK_<xsl:value-of select="@name" />] PRIMARY KEY CLUSTERED 
  (
	  [<xsl:value-of select="field[1]"/>] ASC
  )
)

GO

</xsl:for-each>
-- &lt;/Tables&gt; --------------------------------------------------------------------


-- &lt;Procedures&gt; --------------------------------------------------------------------

CREATE PROCEDURE [dbo].[spIndex_InvalidateAll]
AS
<xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">
   UPDATE [dbo].[<xsl:value-of select="@name" />] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'</xsl:for-each>

GO

CREATE PROCEDURE [dbo].[spIndexHydrate_InvalidateAll]
AS
<xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">
   UPDATE [dbo].[<xsl:value-of select="@name" />] SET [sync_hydrate_utc] = NULL</xsl:for-each>
GO


CREATE PROCEDURE [dbo].[spIndex_InvalidateAggregates]
AS
<xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">
	<xsl:if test="@manualAggregate='true' or count(field[@computedBy='Sum' or @computedBy='Count']) > 0 or count(indexfield[@computedBy='Sum' or @computedBy='Count']) > 0">
	UPDATE [dbo].[<xsl:value-of select="@name" />] SET [sync_success_utc] = NULL</xsl:if></xsl:for-each>

GO


CREATE PROCEDURE [dbo].[spIndexHydrate_InvalidateAggregates]
AS
<xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">
	<xsl:if test="@manualAggregate='true' or count(field[@computedBy='Sum' or @computedBy='Count']) > 0 or count(indexfield[@computedBy='Sum' or @computedBy='Count']) > 0">
	UPDATE [dbo].[<xsl:value-of select="@name" />] SET [sync_hydrate_utc] = NULL</xsl:if></xsl:for-each>

GO

CREATE PROCEDURE [dbo].[spIndex_Status]
AS

SELECT * FROM 
(
 SELECT 'Pending Items' AS [Entity], 0 as [Count]
<xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">
<xsl:sort select="@indexPriority" data-type="number" order="ascending"/>
      UNION ALL (select '<xsl:value-of select="@indexPriority" /> - <xsl:value-of select="@name" />' as [Entity], count(1) as [Count] from [dbo].[<xsl:value-of select="@name" />] where [sync_success_utc] IS NULL)</xsl:for-each>
) a
WHERE [Count] > 0
GO

CREATE PROCEDURE [dbo].[spIndexHydrate_Status]
AS

SELECT * FROM 
(
   SELECT 'Pending Items' AS [Entity], 0 as [Count]
<xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">
<xsl:sort select="@indexPriority" data-type="number" order="ascending"/>
      UNION ALL (select '<xsl:value-of select="@indexPriority" /> - <xsl:value-of select="@name" />' as [Entity], count(1)  as [Count] from [dbo].[<xsl:value-of select="@name" />] where [sync_hydrate_utc] IS NULL)</xsl:for-each>
) a
WHERE [Count] > 0
GO



-- &lt;Procedures&gt; --------------------------------------------------------------------


-- &lt;Foreign Keys&gt; --------------------------------------------------------------------
<xsl:for-each select="items/item">
<xsl:sort select="position()" data-type="number" order="descending"/>
<xsl:for-each select="field[@foreignKey and not(@fakeForeignKey='true')]">
ALTER TABLE [dbo].[<xsl:value-of select="../@name" />] WITH CHECK ADD  CONSTRAINT [FK_<xsl:value-of select="../@name" />_<xsl:value-of select="@foreignKey" />_<xsl:value-of select="text()" />] FOREIGN KEY([<xsl:value-of select="text()" />])
REFERENCES [dbo].[<xsl:value-of select="@foreignKey" />] ([<xsl:value-of select="@foreignKeyField" />])
GO
</xsl:for-each>
</xsl:for-each>
-- &lt;/Foreign Keys&gt; --------------------------------------------------------------------


-- &lt;Unique Keys&gt; --------------------------------------------------------------------
<xsl:for-each select="items/item">
<xsl:sort select="position()" data-type="number" order="descending"/>
<xsl:for-each select="field[string-length(@ukGroup)>0]">
<xsl:variable name="groupName" select="@ukGroup"/>

IF OBJECT_ID('dbo.UK_<xsl:value-of select="$groupName" />', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[<xsl:value-of select="../@name" />] 
		DROP CONSTRAINT UK_<xsl:value-of select="$groupName" />
END
ALTER TABLE [dbo].[<xsl:value-of select="../@name" />] 
   ADD CONSTRAINT UK_<xsl:value-of select="$groupName" /> UNIQUE (<xsl:for-each select="../field[@ukGroup=$groupName]"><xsl:if test="position()>1">,</xsl:if>[<xsl:value-of select="text()" />]</xsl:for-each>); 
GO
</xsl:for-each>
</xsl:for-each>
-- &lt;/Unique Keys&gt; --------------------------------------------------------------------

-- &lt;Unique Index&gt; --------------------------------------------------------------------
<xsl:for-each select="items/item">
<xsl:for-each select="field[string-length(@nullableUnique)>0]">

CREATE UNIQUE NONCLUSTERED INDEX [<xsl:value-of select="@uniqueIndex" />_nullable]
	ON [dbo].[<xsl:value-of select="../@name" />] ([<xsl:value-of select="text()" />])
WHERE [<xsl:value-of select="text()" />] IS NOT NULL

</xsl:for-each>
</xsl:for-each>
-- &lt;/Unique Index&gt; --------------------------------------------------------------------


'''[ENDFILE]

'''[STARTFILE:<xsl:value-of select="../@folderName"/>\Scripts\SQL\delete_helper.sql]

-- &lt;Foreign Keys&gt; --------------------------------------------------------------------
<xsl:for-each select="items/item">
<xsl:sort select="position()" data-type="number" order="descending"/>
<xsl:for-each select="field[@foreignKey and not(@fakeForeignKey='true')]">
ALTER TABLE [dbo].[<xsl:value-of select="../@name" />] DROP  [FK_<xsl:value-of select="../@name" />_<xsl:value-of select="@foreignKey" />_<xsl:value-of select="text()" />]
GO
</xsl:for-each>

</xsl:for-each>
-- &lt;/Foreign Keys&gt; --------------------------------------------------------------------



-- &lt;Unique Keys&gt; --------------------------------------------------------------------
<xsl:for-each select="items/item">
<xsl:sort select="position()" data-type="number" order="descending"/>
<xsl:for-each select="field[string-length(@ukGroup)>0]">
<xsl:variable name="groupName" select="@ukGroup"/>
IF OBJECT_ID('dbo.UK_<xsl:value-of select="$groupName" />', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[<xsl:value-of select="../@name" />] 
		DROP CONSTRAINT UK_<xsl:value-of select="$groupName" />
END
GO

</xsl:for-each>
</xsl:for-each>
-- &lt;/Unique Keys&gt; --------------------------------------------------------------------


-- &lt;Tables&gt; --------------------------------------------------------------------
<xsl:for-each select="items/item">
<xsl:sort select="position()" data-type="number" order="descending"/>
DROP TABLE [<xsl:call-template name="ToLower"><xsl:with-param name="inputString" select="../@schema"/></xsl:call-template>].[<xsl:value-of select="@name" />]
GO
</xsl:for-each>
-- &lt;/Tables&gt; --------------------------------------------------------------------

-- &lt;Procedures&gt; --------------------------------------------------------------------

DROP PROCEDURE [dbo].[spIndex_InvalidateAll]
GO
DROP PROCEDURE [dbo].[spIndex_InvalidateAggregates]
GO

DROP PROCEDURE [dbo].[spIndex_Status]
GO

DROP PROCEDURE [dbo].[spIndexHydrate_InvalidateAll]
GO

DROP PROCEDURE [dbo].[spIndexHydrate_InvalidateAggregates]
GO


DROP PROCEDURE [dbo].[spIndexHydrate_Status]
GO


-- &lt;Procedures&gt; --------------------------------------------------------------------
'''[ENDFILE]




'''[STARTFILE:<xsl:value-of select="../@folderName"/>\Scripts\SQL\sync_index.sql]

-- &lt;Index&gt; --------------------------------------------------------------------
<xsl:for-each select="items/item[@useIndex='true' or @useStore='true']">

CREATE NONCLUSTERED INDEX [ix_<xsl:value-of select="@name"/>_sync_agent] ON [dbo].[<xsl:value-of select="@name"/>]
(
	[sync_success_utc] ASC,
	[sync_agent] ASC
) WITH (ONLINE = ON)
GO


<xsl:for-each select="field[string-length(@foreignKey)>0]">
CREATE NONCLUSTERED INDEX [ix_<xsl:value-of select="text()"/>] ON [dbo].[<xsl:value-of select="../@name"/>]
(
	[<xsl:value-of select="text()"/>] ASC
)
GO
</xsl:for-each>

</xsl:for-each>

'''[ENDFILE]



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
</xsl:stylesheet>