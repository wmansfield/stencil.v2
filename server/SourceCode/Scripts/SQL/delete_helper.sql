

-- <Foreign Keys> --------------------------------------------------------------------

ALTER TABLE [dbo].[Company] DROP  [FK_Company_Shop_shop_id]
GO

ALTER TABLE [dbo].[ShopSetting] DROP  [FK_ShopSetting_Shop_shop_id]
GO

ALTER TABLE [dbo].[ShopAccount] DROP  [FK_ShopAccount_Shop_shop_id]
GO

ALTER TABLE [dbo].[ShopAccount] DROP  [FK_ShopAccount_Account_account_id]
GO

ALTER TABLE [dbo].[ShopIsolated] DROP  [FK_ShopIsolated_Shop_shop_id]
GO

ALTER TABLE [dbo].[Shop] DROP  [FK_Shop_Tenant_tenant_id]
GO

ALTER TABLE [dbo].[Account] DROP  [FK_Account_Asset_asset_id_avatar]
GO

-- </Foreign Keys> --------------------------------------------------------------------



-- <Unique Keys> --------------------------------------------------------------------

IF OBJECT_ID('dbo.UK_shop_setting', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[ShopSetting] 
		DROP CONSTRAINT UK_shop_setting
END
GO


IF OBJECT_ID('dbo.UK_shop_setting', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[ShopSetting] 
		DROP CONSTRAINT UK_shop_setting
END
GO


IF OBJECT_ID('dbo.UK_account_email', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[Account] 
		DROP CONSTRAINT UK_account_email
END
GO


IF OBJECT_ID('dbo.UK_account_key', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[Account] 
		DROP CONSTRAINT UK_account_key
END
GO


IF OBJECT_ID('dbo.UK_tenant_code', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[Tenant] 
		DROP CONSTRAINT UK_tenant_code
END
GO


IF OBJECT_ID('dbo.UK_singleglobal', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[GlobalSetting] 
		DROP CONSTRAINT UK_singleglobal
END
GO


-- </Unique Keys> --------------------------------------------------------------------


-- <Tables> --------------------------------------------------------------------

DROP TABLE [dbo].[Company]
GO

DROP TABLE [dbo].[ShopSetting]
GO

DROP TABLE [dbo].[ShopAccount]
GO

DROP TABLE [dbo].[ShopIsolated]
GO

DROP TABLE [dbo].[Shop]
GO

DROP TABLE [dbo].[Account]
GO

DROP TABLE [dbo].[Asset]
GO

DROP TABLE [dbo].[Tenant]
GO

DROP TABLE [dbo].[GlobalSetting]
GO

-- </Tables> --------------------------------------------------------------------

-- <Procedures> --------------------------------------------------------------------

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


-- <Procedures> --------------------------------------------------------------------
