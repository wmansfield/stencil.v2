

-- <Index> --------------------------------------------------------------------


CREATE NONCLUSTERED INDEX [ix_Account_sync_agent] ON [dbo].[Account]
(
	[sync_success_utc] ASC,
	[sync_agent] ASC
) WITH (ONLINE = ON)
GO



CREATE NONCLUSTERED INDEX [ix_asset_id_avatar] ON [dbo].[Account]
(
	[asset_id_avatar] ASC
)
GO

CREATE NONCLUSTERED INDEX [ix_timezone] ON [dbo].[Account]
(
	[timezone] ASC
)
GO


CREATE NONCLUSTERED INDEX [ix_Shop_sync_agent] ON [dbo].[Shop]
(
	[sync_success_utc] ASC,
	[sync_agent] ASC
) WITH (ONLINE = ON)
GO



CREATE NONCLUSTERED INDEX [ix_tenant_id] ON [dbo].[Shop]
(
	[tenant_id] ASC
)
GO


CREATE NONCLUSTERED INDEX [ix_ShopIsolated_sync_agent] ON [dbo].[ShopIsolated]
(
	[sync_success_utc] ASC,
	[sync_agent] ASC
) WITH (ONLINE = ON)
GO



CREATE NONCLUSTERED INDEX [ix_shop_id] ON [dbo].[ShopIsolated]
(
	[shop_id] ASC
)
GO


CREATE NONCLUSTERED INDEX [ix_ShopAccount_sync_agent] ON [dbo].[ShopAccount]
(
	[sync_success_utc] ASC,
	[sync_agent] ASC
) WITH (ONLINE = ON)
GO



CREATE NONCLUSTERED INDEX [ix_shop_id] ON [dbo].[ShopAccount]
(
	[shop_id] ASC
)
GO

CREATE NONCLUSTERED INDEX [ix_account_id] ON [dbo].[ShopAccount]
(
	[account_id] ASC
)
GO


CREATE NONCLUSTERED INDEX [ix_ShopSetting_sync_agent] ON [dbo].[ShopSetting]
(
	[sync_success_utc] ASC,
	[sync_agent] ASC
) WITH (ONLINE = ON)
GO



CREATE NONCLUSTERED INDEX [ix_shop_id] ON [dbo].[ShopSetting]
(
	[shop_id] ASC
)
GO


CREATE NONCLUSTERED INDEX [ix_Company_sync_agent] ON [dbo].[Company]
(
	[sync_success_utc] ASC,
	[sync_agent] ASC
) WITH (ONLINE = ON)
GO



CREATE NONCLUSTERED INDEX [ix_shop_id] ON [dbo].[Company]
(
	[shop_id] ASC
)
GO


CREATE NONCLUSTERED INDEX [ix_Widget_sync_agent] ON [dbo].[Widget]
(
	[sync_success_utc] ASC,
	[sync_agent] ASC
) WITH (ONLINE = ON)
GO



CREATE NONCLUSTERED INDEX [ix_shop_id] ON [dbo].[Widget]
(
	[shop_id] ASC
)
GO


