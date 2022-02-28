
CREATE NONCLUSTERED INDEX [ix_language_result] ON [dbo].[Presentation]
(
	[result_id] ASC,
	[language_id] ASC
) 
INCLUDE ([presentation_id], [presentation_status])
GO