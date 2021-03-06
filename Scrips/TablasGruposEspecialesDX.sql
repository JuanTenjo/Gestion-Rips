USE [GEOGRAXPSQL]
GO
/****** Object:  Table [dbo].[Datos detalle especial de Dx]    Script Date: 15/10/2020 9:31:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Datos detalle especial de Dx](
	[CodDetaDx] [nvarchar](2) NOT NULL,
	[CodDxEspe] [nvarchar](4) NOT NULL,
	[CodRegis] [nvarchar](3) NOT NULL,
	[FecRegis] [date] NOT NULL,
	[CodModi] [nvarchar](3) NOT NULL,
	[FecModi] [date] NOT NULL,
 CONSTRAINT [PK_Datos detalle especial de Dx] PRIMARY KEY CLUSTERED 
(
	[CodDetaDx] ASC,
	[CodDxEspe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Datos grupos especiales de Dx]    Script Date: 15/10/2020 9:31:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Datos grupos especiales de Dx](
	[CodEsDx] [nvarchar](2) NOT NULL,
	[NomEsDx] [nvarchar](100) NOT NULL,
	[ObserEspe] [nvarchar](max) NULL,
	[CodRegis] [nvarchar](3) NOT NULL,
	[FecRegis] [date] NOT NULL,
	[CodModi] [nvarchar](3) NOT NULL,
	[FecModi] [date] NOT NULL,
 CONSTRAINT [PK_Datos grupos especilas de Dx] PRIMARY KEY CLUSTERED 
(
	[CodEsDx] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J00X', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J010', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J011', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J012', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J013', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J014', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J018', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J019', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J020', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J028', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J029', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J030', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J038', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J039', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J040', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J041', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J042', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J050', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J051', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J060', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J068', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J069', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J100', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J101', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J108', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J110', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J111', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J118', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J120', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J121', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J122', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J128', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J129', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J13X', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J14X', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J150', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J151', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J152', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J153', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J154', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J155', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J156', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J157', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J158', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J159', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J160', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J168', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J170', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J171', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J172', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J173', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J178', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J180', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J181', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J182', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J188', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J189', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J200', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J201', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J202', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J203', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J204', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J205', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J206', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J207', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J208', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J209', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J210', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J218', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J219', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J22X', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J440', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J441', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J46X', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J80X', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J81X', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J960', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'J969', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'P220', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'U071', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos detalle especial de Dx] ([CodDetaDx], [CodDxEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'U072', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
INSERT [dbo].[Datos grupos especiales de Dx] ([CodEsDx], [NomEsDx], [ObserEspe], [CodRegis], [FecRegis], [CodModi], [FecModi]) VALUES (N'01', N'Afectados por COVID-19', N'Resolución 992 de junio de 2020', N'002', CAST(N'2020-10-15' AS Date), N'001', CAST(N'2020-10-15' AS Date))
ALTER TABLE [dbo].[Datos detalle especial de Dx] ADD  CONSTRAINT [DF_Table_1_CodEsDx]  DEFAULT (N'000') FOR [CodDetaDx]
GO
ALTER TABLE [dbo].[Datos detalle especial de Dx] ADD  CONSTRAINT [DF_Datos detalle especial de Dx_CodRegis]  DEFAULT ('001') FOR [CodRegis]
GO
ALTER TABLE [dbo].[Datos detalle especial de Dx] ADD  CONSTRAINT [DF_Datos detalle especial de Dx_FecRegis]  DEFAULT (getdate()) FOR [FecRegis]
GO
ALTER TABLE [dbo].[Datos detalle especial de Dx] ADD  CONSTRAINT [DF_Datos detalle especial de Dx_CodModi]  DEFAULT (N'001') FOR [CodModi]
GO
ALTER TABLE [dbo].[Datos detalle especial de Dx] ADD  CONSTRAINT [DF_Datos detalle especial de Dx_FecModi]  DEFAULT (getdate()) FOR [FecModi]
GO
ALTER TABLE [dbo].[Datos grupos especiales de Dx] ADD  CONSTRAINT [DF_Datos grupos especilas de Dx_CodEsDx]  DEFAULT (N'000') FOR [CodEsDx]
GO
ALTER TABLE [dbo].[Datos grupos especiales de Dx] ADD  CONSTRAINT [DF_Datos grupos especilas de Dx_ObserEspe]  DEFAULT (N'') FOR [ObserEspe]
GO
ALTER TABLE [dbo].[Datos grupos especiales de Dx] ADD  CONSTRAINT [DF_Datos grupos especilas de Dx_CodRegis]  DEFAULT ('001') FOR [CodRegis]
GO
ALTER TABLE [dbo].[Datos grupos especiales de Dx] ADD  CONSTRAINT [DF_Datos grupos especilas de Dx_FecRegis]  DEFAULT (getdate()) FOR [FecRegis]
GO
ALTER TABLE [dbo].[Datos grupos especiales de Dx] ADD  CONSTRAINT [DF_Datos grupos especilas de Dx_CodModi]  DEFAULT (N'001') FOR [CodModi]
GO
ALTER TABLE [dbo].[Datos grupos especiales de Dx] ADD  CONSTRAINT [DF_Datos grupos especilas de Dx_FecModi]  DEFAULT (getdate()) FOR [FecModi]
GO
ALTER TABLE [dbo].[Datos detalle especial de Dx]  WITH CHECK ADD  CONSTRAINT [GruposEspecialesDx_DatosDetalleEspecialDx] FOREIGN KEY([CodDetaDx])
REFERENCES [dbo].[Datos grupos especiales de Dx] ([CodEsDx])
GO
ALTER TABLE [dbo].[Datos detalle especial de Dx] CHECK CONSTRAINT [GruposEspecialesDx_DatosDetalleEspecialDx]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código especial del grupo de diagnósticos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos detalle especial de Dx', @level2type=N'COLUMN',@level2name=N'CodDetaDx'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código usuario que ingresa o modifica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos detalle especial de Dx', @level2type=N'COLUMN',@level2name=N'CodRegis'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de ingreso al sistema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos detalle especial de Dx', @level2type=N'COLUMN',@level2name=N'FecRegis'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código especial del grupo de diagnósticos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos grupos especiales de Dx', @level2type=N'COLUMN',@level2name=N'CodEsDx'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre especial del grupo de diagnósticos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos grupos especiales de Dx', @level2type=N'COLUMN',@level2name=N'NomEsDx'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Observaciones especiales del grupo de Dx' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos grupos especiales de Dx', @level2type=N'COLUMN',@level2name=N'ObserEspe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código usuario que ingresa o modifica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos grupos especiales de Dx', @level2type=N'COLUMN',@level2name=N'CodRegis'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de ingreso al sistema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos grupos especiales de Dx', @level2type=N'COLUMN',@level2name=N'FecRegis'
GO
