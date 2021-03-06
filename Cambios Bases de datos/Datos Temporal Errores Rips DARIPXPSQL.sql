USE [DARIPSXPSQL]
GO
/****** Object:  Table [dbo].[Datos temporal errores RIPS]    Script Date: 08/05/2021 11:37:28 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Datos temporal errores RIPS](
	[CodDigita] [nvarchar](4) NOT NULL,
	[TipARchi] [nvarchar](2) NOT NULL,
	[TipDocu] [nvarchar](2) NOT NULL,
	[NumDocu] [nvarchar](20) NOT NULL,
	[CodEnti] [nvarchar](4) NOT NULL,
	[FacturaN] [nvarchar](20) NOT NULL,
	[Observa1] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Datos temporal errores RIPS] ADD  CONSTRAINT [DF_Datos temporal errores RIPS_CodDigita]  DEFAULT (N'001') FOR [CodDigita]
GO
ALTER TABLE [dbo].[Datos temporal errores RIPS] ADD  CONSTRAINT [DF_Datos temporal errores RIPS_TipDocu]  DEFAULT (N'CC') FOR [TipDocu]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del usuraio que se encuentra seleccionado los RIPS en el momento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal errores RIPS', @level2type=N'COLUMN',@level2name=N'CodDigita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de archivo según los RIPS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal errores RIPS', @level2type=N'COLUMN',@level2name=N'TipARchi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal errores RIPS', @level2type=N'COLUMN',@level2name=N'TipDocu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número del documento de identificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal errores RIPS', @level2type=N'COLUMN',@level2name=N'NumDocu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Factura No.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal errores RIPS', @level2type=N'COLUMN',@level2name=N'FacturaN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Observaciones' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Datos temporal errores RIPS', @level2type=N'COLUMN',@level2name=N'Observa1'
GO
