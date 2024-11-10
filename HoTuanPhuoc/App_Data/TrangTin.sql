
use SachOnline
GO


CREATE TABLE TRANGTIN
(
	MaTT INT IDENTITY(1,1) PRIMARY KEY,
	TenTrang NVARCHAR(100) NOT NULL,
	NoiDung NTEXT,
	NgayTao SMALLDATETIME,
	MetaTitle NVARCHAR(100)
	
)
GO


/****** Object:  Table [dbo].[SACH]    Script Date: 10/19/2020 13:14:22 ******/
SET IDENTITY_INSERT [dbo].[TRANGTIN] ON
INSERT [dbo].[TRANGTIN] ([MaTT], [TenTrang], [NoiDung], [NgayTao], [MetaTitle]) VALUES (1, N'GIỚI THIỆU', N'Đây là nội dung trang giới thiệu', '04/12/2021', N'gioi-thieu')
INSERT [dbo].[TRANGTIN] ([MaTT], [TenTrang], [NoiDung], [NgayTao], [MetaTitle]) VALUES (2, N'LIÊN HỆ', N'Đây là nội dung trang liên hệ', '04/10/2022', N'lien-he')
SET IDENTITY_INSERT [dbo].[TRANGTIN] OFF

