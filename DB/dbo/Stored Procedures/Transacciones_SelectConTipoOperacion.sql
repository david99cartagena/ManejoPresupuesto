-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Transacciones_SelectConTipoOperacion 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Transacciones.Id,
       Transacciones.UsuarioId,
       Transacciones.Monto,
       Transacciones.Nota,
       TiposOperaciones.Descripcion
	FROM Transacciones
	INNER JOIN TiposOperaciones
    ON Transacciones.TipoOperacionId = TiposOperaciones.Id;
END
