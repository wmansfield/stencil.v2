

-- <Tables> --------------------------------------------------------------------

CREATE TABLE [dbo].[GlobalSetting] (
	 [global_setting_id] uniqueidentifier NOT NULL
    ,[name] nvarchar(100) NOT NULL
    ,[value] nvarchar(max) NULL
    ,[value_encrypted] nvarchar(max) NULL
    ,[encrypted] bit NOT NULL
    
  ,CONSTRAINT [PK_GlobalSetting] PRIMARY KEY CLUSTERED 
  (
	  [global_setting_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Tenant] (
	 [tenant_id] uniqueidentifier NOT NULL
    ,[tenant_name] nvarchar(50) NOT NULL
    ,[tenant_code] nvarchar(10) NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
  ,CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED 
  (
	  [tenant_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Asset] (
	 [asset_id] uniqueidentifier NOT NULL
    ,[asset_kind] int NOT NULL
    ,[available] bit NOT NULL
    ,[resize_required] bit NOT NULL
    ,[encode_required] bit NOT NULL
    ,[resize_processing] bit NOT NULL
    ,[encode_processing] bit NOT NULL
    ,[thumb_small_dimensions] nvarchar(10) NULL
    ,[thumb_medium_dimensions] nvarchar(10) NULL
    ,[thumb_large_dimensions] nvarchar(10) NULL
    ,[resize_status] nvarchar(50) NULL
    ,[resize_attempts] int NOT NULL
    ,[resize_attempt_utc] datetimeoffset(0) NULL
    ,[encode_identifier] nvarchar(50) NULL
    ,[encode_status] nvarchar(50) NULL
    ,[relative_path] nvarchar(512) NULL
    ,[raw_url] nvarchar(512) NULL
    ,[public_url] nvarchar(512) NULL
    ,[thumb_small_url] nvarchar(512) NULL
    ,[thumb_medium_url] nvarchar(512) NULL
    ,[thumb_large_url] nvarchar(512) NULL
    ,[encode_log] nvarchar(max) NULL
    ,[resize_log] nvarchar(max) NULL
    ,[dependencies] int NOT NULL
    ,[encode_attempts] int NOT NULL
    ,[encode_attempt_utc] datetimeoffset(0) NULL
    ,[resize_mode] nvarchar(20) NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
  ,CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED 
  (
	  [asset_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Account] (
	 [account_id] uniqueidentifier NOT NULL
    ,[asset_id_avatar] uniqueidentifier NULL
    ,[email] nvarchar(250) NOT NULL
    ,[first_name] nvarchar(50) NOT NULL
    ,[last_name] nvarchar(50) NOT NULL
    ,[account_display] nvarchar(150) NULL
    ,[password] nvarchar(250) NOT NULL
    ,[password_salt] nvarchar(50) NOT NULL
    ,[account_status] int NOT NULL
    ,[api_key] nvarchar(50) NOT NULL
    ,[api_secret] nvarchar(50) NOT NULL
    ,[timezone] nvarchar(128) NULL
    ,[email_verify_token] nvarchar(50) NOT NULL
    ,[email_verify_utc] datetimeoffset(0) NULL
    ,[entitlements] nvarchar(250) NULL
    ,[password_changed_utc] datetimeoffset(0) NULL
    ,[password_reset_token] nvarchar(50) NULL
    ,[password_reset_utc] datetimeoffset(0) NULL
    ,[single_login_token] nvarchar(50) NULL
    ,[single_login_token_expire_utc] datetimeoffset(0) NULL
    ,[last_login_utc] datetimeoffset(0) NULL
    ,[last_login_platform] nvarchar(250) NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
  (
	  [account_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Shop] (
	 [shop_id] uniqueidentifier NOT NULL
    ,[tenant_id] uniqueidentifier NOT NULL
    ,[shop_name] nvarchar(150) NOT NULL
    ,[private_domain] nvarchar(150) NOT NULL
    ,[public_domain] nvarchar(150) NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_Shop] PRIMARY KEY CLUSTERED 
  (
	  [shop_id] ASC
  )
)

GO


CREATE TABLE [dbo].[ShopIsolated] (
	 [shop_id] uniqueidentifier NOT NULL
    ,[webhoooks_enabled] bit NOT NULL
    ,[fulfillment_enabled] bit NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_ShopIsolated] PRIMARY KEY CLUSTERED 
  (
	  [shop_id] ASC
  )
)

GO


CREATE TABLE [dbo].[ShopAccount] (
	 [shop_account_id] uniqueidentifier NOT NULL
    ,[shop_id] uniqueidentifier NOT NULL
    ,[account_id] uniqueidentifier NOT NULL
    ,[shop_role] int NOT NULL
    ,[enabled] bit NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_ShopAccount] PRIMARY KEY CLUSTERED 
  (
	  [shop_account_id] ASC
  )
)

GO


CREATE TABLE [dbo].[ShopSetting] (
	 [shop_setting_id] uniqueidentifier NOT NULL
    ,[shop_id] uniqueidentifier NOT NULL
    ,[name] nvarchar(255) NOT NULL
    ,[description] nvarchar(max) NULL
    ,[value] nvarchar(max) NULL
    ,[value_encrypted] nvarchar(max) NULL
    ,[encrypted] bit NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_ShopSetting] PRIMARY KEY CLUSTERED 
  (
	  [shop_setting_id] ASC
  )
)

GO


CREATE TABLE [dbo].[Company] (
	 [company_id] uniqueidentifier NOT NULL
    ,[shop_id] uniqueidentifier NOT NULL
    ,[company_name] nvarchar(150) NULL
    ,[disabled] bit NOT NULL
    ,[created_utc] DATETIMEOFFSET(0) NOT NULL
    ,[updated_utc] DATETIMEOFFSET(0) NOT NULL
    ,[deleted_utc] DATETIMEOFFSET(0) NULL
	,[sync_hydrate_utc] DATETIMEOFFSET(0) NULL
    ,[sync_success_utc] DATETIMEOFFSET(0) NULL
    ,[sync_invalid_utc] DATETIMEOFFSET(0) NULL
    ,[sync_attempt_utc] DATETIMEOFFSET(0) NULL
    ,[sync_agent] NVARCHAR(50) NULL
    ,[sync_log] NVARCHAR(MAX) NULL
  ,CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
  (
	  [company_id] ASC
  )
)

GO


-- </Tables> --------------------------------------------------------------------


-- <Procedures> --------------------------------------------------------------------

CREATE PROCEDURE [dbo].[spIndex_InvalidateAll]
AS

   UPDATE [dbo].[Account] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'
   UPDATE [dbo].[Shop] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'
   UPDATE [dbo].[ShopIsolated] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'
   UPDATE [dbo].[ShopAccount] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'
   UPDATE [dbo].[ShopSetting] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'
   UPDATE [dbo].[Company] SET [sync_success_utc] = NULL, [sync_log] = 'invalidateall'

GO

CREATE PROCEDURE [dbo].[spIndexHydrate_InvalidateAll]
AS

   UPDATE [dbo].[Account] SET [sync_hydrate_utc] = NULL
   UPDATE [dbo].[Shop] SET [sync_hydrate_utc] = NULL
   UPDATE [dbo].[ShopIsolated] SET [sync_hydrate_utc] = NULL
   UPDATE [dbo].[ShopAccount] SET [sync_hydrate_utc] = NULL
   UPDATE [dbo].[ShopSetting] SET [sync_hydrate_utc] = NULL
   UPDATE [dbo].[Company] SET [sync_hydrate_utc] = NULL
GO


CREATE PROCEDURE [dbo].[spIndex_InvalidateAggregates]
AS


GO


CREATE PROCEDURE [dbo].[spIndexHydrate_InvalidateAggregates]
AS


GO

CREATE PROCEDURE [dbo].[spIndex_Status]
AS

SELECT * FROM 
(
 SELECT 'Pending Items' AS [Entity], 0 as [Count]

      UNION ALL (select '10 - Account' as [Entity], count(1) as [Count] from [dbo].[Account] where [sync_success_utc] IS NULL)
      UNION ALL (select '20 - Shop' as [Entity], count(1) as [Count] from [dbo].[Shop] where [sync_success_utc] IS NULL)
      UNION ALL (select '20 - ShopIsolated' as [Entity], count(1) as [Count] from [dbo].[ShopIsolated] where [sync_success_utc] IS NULL)
      UNION ALL (select '30 - ShopAccount' as [Entity], count(1) as [Count] from [dbo].[ShopAccount] where [sync_success_utc] IS NULL)
      UNION ALL (select '30 - ShopSetting' as [Entity], count(1) as [Count] from [dbo].[ShopSetting] where [sync_success_utc] IS NULL)
      UNION ALL (select '30 - Company' as [Entity], count(1) as [Count] from [dbo].[Company] where [sync_success_utc] IS NULL)
) a
WHERE [Count] > 0
GO

CREATE PROCEDURE [dbo].[spIndexHydrate_Status]
AS

SELECT * FROM 
(
   SELECT 'Pending Items' AS [Entity], 0 as [Count]

      UNION ALL (select '10 - Account' as [Entity], count(1)  as [Count] from [dbo].[Account] where [sync_hydrate_utc] IS NULL)
      UNION ALL (select '20 - Shop' as [Entity], count(1)  as [Count] from [dbo].[Shop] where [sync_hydrate_utc] IS NULL)
      UNION ALL (select '20 - ShopIsolated' as [Entity], count(1)  as [Count] from [dbo].[ShopIsolated] where [sync_hydrate_utc] IS NULL)
      UNION ALL (select '30 - ShopAccount' as [Entity], count(1)  as [Count] from [dbo].[ShopAccount] where [sync_hydrate_utc] IS NULL)
      UNION ALL (select '30 - ShopSetting' as [Entity], count(1)  as [Count] from [dbo].[ShopSetting] where [sync_hydrate_utc] IS NULL)
      UNION ALL (select '30 - Company' as [Entity], count(1)  as [Count] from [dbo].[Company] where [sync_hydrate_utc] IS NULL)
) a
WHERE [Count] > 0
GO



-- <Procedures> --------------------------------------------------------------------


-- <Foreign Keys> --------------------------------------------------------------------

ALTER TABLE [dbo].[Company] WITH CHECK ADD  CONSTRAINT [FK_Company_Shop_shop_id] FOREIGN KEY([shop_id])
REFERENCES [dbo].[Shop] ([shop_id])
GO

ALTER TABLE [dbo].[ShopSetting] WITH CHECK ADD  CONSTRAINT [FK_ShopSetting_Shop_shop_id] FOREIGN KEY([shop_id])
REFERENCES [dbo].[Shop] ([shop_id])
GO

ALTER TABLE [dbo].[ShopAccount] WITH CHECK ADD  CONSTRAINT [FK_ShopAccount_Shop_shop_id] FOREIGN KEY([shop_id])
REFERENCES [dbo].[Shop] ([shop_id])
GO

ALTER TABLE [dbo].[ShopAccount] WITH CHECK ADD  CONSTRAINT [FK_ShopAccount_Account_account_id] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([account_id])
GO

ALTER TABLE [dbo].[ShopIsolated] WITH CHECK ADD  CONSTRAINT [FK_ShopIsolated_Shop_shop_id] FOREIGN KEY([shop_id])
REFERENCES [dbo].[Shop] ([shop_id])
GO

ALTER TABLE [dbo].[Shop] WITH CHECK ADD  CONSTRAINT [FK_Shop_Tenant_tenant_id] FOREIGN KEY([tenant_id])
REFERENCES [dbo].[Tenant] ([tenant_id])
GO

ALTER TABLE [dbo].[Account] WITH CHECK ADD  CONSTRAINT [FK_Account_Asset_asset_id_avatar] FOREIGN KEY([asset_id_avatar])
REFERENCES [dbo].[Asset] ([asset_id])
GO

-- </Foreign Keys> --------------------------------------------------------------------


-- <Unique Keys> --------------------------------------------------------------------


IF OBJECT_ID('dbo.UK_shop_setting', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[ShopSetting] 
		DROP CONSTRAINT UK_shop_setting
END
ALTER TABLE [dbo].[ShopSetting] 
   ADD CONSTRAINT UK_shop_setting UNIQUE ([shop_id],[name]); 
GO


IF OBJECT_ID('dbo.UK_shop_setting', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[ShopSetting] 
		DROP CONSTRAINT UK_shop_setting
END
ALTER TABLE [dbo].[ShopSetting] 
   ADD CONSTRAINT UK_shop_setting UNIQUE ([shop_id],[name]); 
GO


IF OBJECT_ID('dbo.UK_account_email', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[Account] 
		DROP CONSTRAINT UK_account_email
END
ALTER TABLE [dbo].[Account] 
   ADD CONSTRAINT UK_account_email UNIQUE ([email]); 
GO


IF OBJECT_ID('dbo.UK_account_key', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[Account] 
		DROP CONSTRAINT UK_account_key
END
ALTER TABLE [dbo].[Account] 
   ADD CONSTRAINT UK_account_key UNIQUE ([api_key]); 
GO


IF OBJECT_ID('dbo.UK_tenant_code', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[Tenant] 
		DROP CONSTRAINT UK_tenant_code
END
ALTER TABLE [dbo].[Tenant] 
   ADD CONSTRAINT UK_tenant_code UNIQUE ([tenant_code]); 
GO


IF OBJECT_ID('dbo.UK_singleglobal', 'UQ') IS NOT NULL BEGIN -- multiple passes because of script limitations, thats fine. :)
	ALTER TABLE [dbo].[GlobalSetting] 
		DROP CONSTRAINT UK_singleglobal
END
ALTER TABLE [dbo].[GlobalSetting] 
   ADD CONSTRAINT UK_singleglobal UNIQUE ([name]); 
GO

-- </Unique Keys> --------------------------------------------------------------------

-- <Unique Index> --------------------------------------------------------------------

-- </Unique Index> --------------------------------------------------------------------


