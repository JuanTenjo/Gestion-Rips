USE [ACDATOXPSQL]
GO
/****** Object:  View [dbo].[FacturasDxEspecial]    Script Date: 16/10/2020 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FacturasDxEspecial]
AS
SELECT        [Datos de las facturas realizadas].NumFactura, [Datos de las facturas realizadas].FechaFac, [Datos empresas y terceros].NomAdmin, [Datos empresas y terceros].CodiMinSalud, 
                         [Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].Copago, [Datos de las facturas realizadas].PrefiFac, GEOGRAXPSQL.dbo.[Datos detalle especial de Dx].CodDetaDx
FROM            [Datos de las facturas realizadas] INNER JOIN
                         [Datos empresas y terceros] ON [Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN
                         [Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum INNER JOIN
                         GEOGRAXPSQL.dbo.[Datos detalle especial de Dx] ON [Datos cuentas de consumos].DxSalida = GEOGRAXPSQL.dbo.[Datos detalle especial de Dx].CodDxEspe
WHERE        ([Datos de las facturas realizadas].AnuladaFac = 'False')
UNION ALL
SELECT        [Datos de las facturas realizadas].NumFactura, [Datos de las facturas realizadas].FechaFac, [Datos empresas y terceros].NomAdmin, [Datos empresas y terceros].CodiMinSalud, 
                         [Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].Copago,  [Datos de las facturas realizadas].PrefiFac, GEOGRAXPSQL.dbo.[Datos detalle especial de Dx].CodDetaDx
FROM            [Datos de las facturas realizadas] INNER JOIN
                         [Datos empresas y terceros] ON [Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN
                         [Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum INNER JOIN
                         GEOGRAXPSQL.dbo.[Datos detalle especial de Dx] ON [Datos cuentas de consumos].DxRelac01 = GEOGRAXPSQL.dbo.[Datos detalle especial de Dx].CodDxEspe
WHERE        ([Datos de las facturas realizadas].AnuladaFac = 'False')
UNION ALL
SELECT        [Datos de las facturas realizadas].NumFactura, [Datos de las facturas realizadas].FechaFac, [Datos empresas y terceros].NomAdmin, [Datos empresas y terceros].CodiMinSalud, 
                         [Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].Copago,  [Datos de las facturas realizadas].PrefiFac, GEOGRAXPSQL.dbo.[Datos detalle especial de Dx].CodDetaDx
FROM            [Datos de las facturas realizadas] INNER JOIN
                         [Datos empresas y terceros] ON [Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN
                         [Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum INNER JOIN
                         GEOGRAXPSQL.dbo.[Datos detalle especial de Dx] ON [Datos cuentas de consumos].DxRelac02 = GEOGRAXPSQL.dbo.[Datos detalle especial de Dx].CodDxEspe
WHERE        ([Datos de las facturas realizadas].AnuladaFac = 'False')
UNION ALL
SELECT        [Datos de las facturas realizadas].NumFactura, [Datos de las facturas realizadas].FechaFac, [Datos empresas y terceros].NomAdmin, [Datos empresas y terceros].CodiMinSalud, 
                         [Datos de las facturas realizadas].ValorFac, [Datos de las facturas realizadas].Copago,  [Datos de las facturas realizadas].PrefiFac,  GEOGRAXPSQL.dbo.[Datos detalle especial de Dx].CodDetaDx
FROM            [Datos de las facturas realizadas] INNER JOIN
                         [Datos empresas y terceros] ON [Datos de las facturas realizadas].Cartercero = [Datos empresas y terceros].CarAdmin INNER JOIN
                         [Datos cuentas de consumos] ON [Datos de las facturas realizadas].NumCuenFac = [Datos cuentas de consumos].CuenNum INNER JOIN
                         GEOGRAXPSQL.dbo.[Datos detalle especial de Dx] ON [Datos cuentas de consumos].DxRelac03 = GEOGRAXPSQL.dbo.[Datos detalle especial de Dx].CodDxEspe
WHERE        ([Datos de las facturas realizadas].AnuladaFac = 'False')

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[12] 4[19] 2[51] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FacturasDxEspecial'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FacturasDxEspecial'
GO
