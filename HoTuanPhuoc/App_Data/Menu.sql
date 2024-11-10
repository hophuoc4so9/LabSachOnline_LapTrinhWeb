use SachOnline
GO


CREATE TABLE MENU
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	MenuName NVARCHAR(50),
	MenuLink NVARCHAR(100),
	ParentId INT,
	OrderNumber INT
)
GO

/****** Object:  Table [dbo].[MENU]    Script Date: 10/19/2020 13:14:22 ******/
SET IDENTITY_INSERT [dbo].MENU ON
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (1, N'TRANG CHỦ', N'', NULL, 1)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (2, N'GIỚI THIỆU', N'gioi-thieu', NULL, 2)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (3, N'DANH MỤC', N'danh-muc', NULL, 3)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (6, N'Sách Ngoại ngữ', N'sach-theo-chu-de-1', 3, 1)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (7, N'Sách CNTT', N'sach-theo-chu-de-2', 3, 2)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (8, N'Tin học văn phòng', N'chi-tiet-sach-21', 7, 1)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (9, N'Tin học ứng dụng', N'chi-tiet-sach-15', 7, 2)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (4, N'LIÊN HỆ', N'lien-he', NULL, 4)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (5, N'TDMU Link tự do', N'https://tdmu.edu.vn', NULL, 5)
SET IDENTITY_INSERT [dbo].MENU OFF