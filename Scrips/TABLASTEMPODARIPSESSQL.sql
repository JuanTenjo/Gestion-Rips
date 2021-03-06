USE [DARIPSESSQL]
GO
/****** Object:  Table [dbo].[Datos temporal transacciones RIPS]    Script Date: 17/06/2021 2:33:35 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Datos temporal transacciones RIPS](
	[CodDigita] [nvarchar](4) NOT NULL,
	[NumRemi] [nvarchar](6) NOT NULL,
	[CodIPS] [nvarchar](12) NOT NULL,
	[RazonSocial] [nvarchar](60) NOT NULL,
	[TipIdenti] [nvarchar](2) NOT NULL,
	[NumIdenti] [nvarchar](20) NOT NULL,
	[NumFactur] [nvarchar](20) NOT NULL,
	[FecFactur] [date] NOT NULL,
	[FecInicio] [date] NOT NULL,
	[FecFinal] [date] NOT NULL,
	[CodAdmin] [nvarchar](6) NOT NULL,
	[NomAdmin] [nvarchar](30) NULL,
	[NumContra] [nvarchar](15) NULL,
	[PlanBene] [nvarchar](30) NOT NULL,
	[NumPoli] [nvarchar](10) NULL,
	[Copago] [float] NOT NULL,
	[ValorComi] [float] NOT NULL,
	[ValorDes] [float] NOT NULL,
	[ValorNeto] [float] NOT NULL,
	[VaLorDeta] [float] NOT NULL,
	[CausExter] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_Datos temporal transacciones RIPS] PRIMARY KEY CLUSTERED 
(
	[CodDigita] ASC,
	[NumRemi] ASC,
	[NumFactur] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Datos temporal usuarios RIPS]    Script Date: 17/06/2021 2:33:35 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Datos temporal usuarios RIPS](
	[CodDigita] [nvarchar](4) NOT NULL,
	[NumRemi] [nvarchar](6) NOT NULL,
	[TipoDocum] [nvarchar](2) NOT NULL,
	[NumDocum] [nvarchar](20) NOT NULL,
	[CodAdmin] [nvarchar](6) NOT NULL,
	[TipUsuario] [nvarchar](1) NOT NULL,
	[Apellido1] [nvarchar](30) NOT NULL,
	[Apellido2] [nvarchar](30) NULL,
	[Nombre1] [nvarchar](20) NOT NULL,
	[Nombre2] [nvarchar](20) NULL,
	[Edad] [tinyint] NOT NULL,
	[EdadMedi] [nvarchar](1) NOT NULL,
	[Sexo] [nvarchar](1) NOT NULL,
	[CodDpto] [nvarchar](2) NOT NULL,
	[CodMuni] [nvarchar](3) NOT NULL,
	[ZonaResi] [nvarchar](1) NOT NULL,
	[Exportado] [bit] NOT NULL,
 CONSTRAINT [PK_Datos temporal usuarios RIPS] PRIMARY KEY CLUSTERED 
(
	[CodDigita] ASC,
	[NumRemi] ASC,
	[TipoDocum] ASC,
	[NumDocum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_CodDigita]  DEFAULT (N'001') FOR [CodDigita]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_FecFactur]  DEFAULT (getdate()) FOR [FecFactur]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_FecInicio]  DEFAULT (getdate()) FOR [FecInicio]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_FecFinal]  DEFAULT (getdate()) FOR [FecFinal]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_CodAdmin]  DEFAULT (N'000000') FOR [CodAdmin]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_PlanBene]  DEFAULT (N'N''REGIMEN SUBSIDIADO"') FOR [PlanBene]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_Copago]  DEFAULT ((0)) FOR [Copago]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_ValorComi]  DEFAULT ((0)) FOR [ValorComi]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_ValorDes]  DEFAULT ((0)) FOR [ValorDes]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_ValorNeto]  DEFAULT ((0)) FOR [ValorNeto]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_VaLorDeta]  DEFAULT ((0)) FOR [VaLorDeta]
GO
ALTER TABLE [dbo].[Datos temporal transacciones RIPS] ADD  CONSTRAINT [DF_Datos temporal transacciones RIPS_CausExter]  DEFAULT (N'13') FOR [CausExter]
GO
ALTER TABLE [dbo].[Datos temporal usuarios RIPS] ADD  CONSTRAINT [DF_Datos temporal_CodDigita]  DEFAULT (N'001') FOR [CodDigita]
GO
ALTER TABLE [dbo].[Datos temporal usuarios RIPS] ADD  CONSTRAINT [DF_Datos temporal usuarios RIPS_Exportado]  DEFAULT ((0)) FOR [Exportado]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del usuraio que se encuentra seleccionado los RIPS en el momento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'CodDigita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de la remisión que la relaciona con la información por cada envío' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'NumRemi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del prestador de servicios de salud (IPS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'CodIPS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Razon social de la IPS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'RazonSocial'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de identificación del número de identificación de la IPS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'TipIdenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número del documento de identificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'NumIdenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de la factura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'NumFactur'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de expedición de la factura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'FecFactur'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de inicio del periodo de la facturación enviada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'FecInicio'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha final del periodo de la facturación enviada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'FecFinal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código de la entidad administradora (Blanco cuando es particular)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'CodAdmin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre de la entidad administradora' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'NomAdmin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número del contrato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'NumContra'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción textual del plan de beneficios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'PlanBene'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de la póliza del seguró obligatorio (SOAT)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'NumPoli'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor total del pago compartido (Copago)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'Copago'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor de la comisión' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'ValorComi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor total del descuento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'ValorDes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor neto de la factura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'ValorNeto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permite registrar el valor total de detalle de la factura en el proceso de auditoría' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'VaLorDeta'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Define la causa externa de la atencion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal transacciones RIPS', @level2type=N'COLUMN',@level2name=N'CausExter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del usuraio que se encuentra seleccionado los RIPS en el momento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'CodDigita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de la remisión que la relaciona con la información por cada envío' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'NumRemi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de documento de identificación del usuario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'TipoDocum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de identificación del usuario en el sistema (Puede ser el de historia)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'NumDocum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código de la entidad administradora (Blanco cuando es particular)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'CodAdmin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de usuario. 1 = Contributivo, 2 = Subsidiado...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'TipUsuario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primer apellido del usuario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'Apellido1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Segundo apellido del usuario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'Apellido2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primer nombre del usuario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'Nombre1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Segundor nombre del usuario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'Nombre2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edad del usuario al momento de la prestación del servicio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'Edad'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unidad de medida de la edad. 1 = Años, 2 = Meses, 3 = Días' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'EdadMedi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sexo del usuario M = Masculino, F = Femenino' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'Sexo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del departamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'CodDpto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del municipio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'CodMuni'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Zona de residencia habitual. U = Urbana, R = Rural' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'ZonaResi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sí = El registro fue exportado satisfactoriamente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal usuarios RIPS', @level2type=N'COLUMN',@level2name=N'Exportado'
GO
