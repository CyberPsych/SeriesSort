USE [MediaModel]
GO
SET IDENTITY_INSERT [dbo].[MediaTypes] ON 

INSERT [dbo].[MediaTypes] ([MediaTypeId], [Extension], [ValidForSeries]) VALUES (1, N'avi', 1)
INSERT [dbo].[MediaTypes] ([MediaTypeId], [Extension], [ValidForSeries]) VALUES (2, N'mpeg', 1)
INSERT [dbo].[MediaTypes] ([MediaTypeId], [Extension], [ValidForSeries]) VALUES (3, N'mp3', 0)
INSERT [dbo].[MediaTypes] ([MediaTypeId], [Extension], [ValidForSeries]) VALUES (4, N'mkv', 1)
INSERT [dbo].[MediaTypes] ([MediaTypeId], [Extension], [ValidForSeries]) VALUES (5, N'mp4', 1)
SET IDENTITY_INSERT [dbo].[MediaTypes] OFF
