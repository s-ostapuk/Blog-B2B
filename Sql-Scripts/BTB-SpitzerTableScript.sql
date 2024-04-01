CREATE FUNCTION dbo.FetchMonthlyTotal
(
    @Rate DECIMAL(19, 12),
    @Principal DECIMAL(19, 12),
    @Tenure INT
)
RETURNS DECIMAL(19, 12)
WITH EXECUTE AS CALLER
AS
BEGIN
    DECLARE @MonthlyRate DECIMAL(19, 12);
    SET @MonthlyRate = (@Rate / 12);
    DECLARE @MonthlyTotal DECIMAL(19, 12);
    SET @MonthlyTotal
        = (@Principal * @MonthlyRate * POWER(1 + @MonthlyRate, @Tenure))
          / (POWER(1 + @MonthlyRate, @Tenure) - 1);
    RETURN (@MonthlyTotal);
END;
GO

CREATE FUNCTION dbo.FetchPrincipalAmount
(
    @Rate DECIMAL(19, 12),
    @RemainingPrincipal DECIMAL(19, 12),
    @MonthlyTotal DECIMAL(19, 12)
)
RETURNS DECIMAL(19, 12)
WITH EXECUTE AS CALLER
AS
BEGIN
    DECLARE @PrincipalAmount DECIMAL(19, 12);
    SET @PrincipalAmount = @MonthlyTotal - (@RemainingPrincipal * @Rate / 12);
    RETURN (@PrincipalAmount);
END;
GO

CREATE FUNCTION dbo.FetchRow
(
    @Rate DECIMAL(19, 12),
    @Principal DECIMAL(19, 12),
    @Tenure INT,
    @MonthlyTotal DECIMAL(19, 12)
)
RETURNS TABLE
AS
RETURN
(
    SELECT @MonthlyTotal AS TotalDue,
           @MonthlyTotal - dbo.FetchPrincipalAmount(@Rate, @Principal, @MonthlyTotal) AS CalculatedInterestDue,
           dbo.FetchPrincipalAmount(@Rate, @Principal, @MonthlyTotal) AS PrincipalAmount,
           @Principal - dbo.FetchPrincipalAmount(@Rate, @Principal, @MonthlyTotal) AS RemainingPrincipal
)
GO

CREATE PROCEDURE dbo.FetchLoanTableWithRecycle @Principal DECIMAL(19, 2)
AS
SET NOCOUNT ON;

DECLARE @FirstRate DECIMAL(19, 8) = 0.08;
DECLARE @FirstTenure INT = 36;
DECLARE @SecondRate DECIMAL(19, 8) = 0.045;
DECLARE @SecondTenure INT = 48;

WITH RowCTE (PaymentNumber, TotalDue, CalculatedInterestDue, PrincipalAmount, RemainingPrincipal)
AS
(
    SELECT 1, *
    FROM dbo.FetchRow(
                              @FirstRate,
                              @Principal,
                              @FirstTenure,
							  dbo.FetchMonthlyTotal(@FirstRate, @Principal, @FirstTenure)
                          )
    UNION ALL
    SELECT r.PaymentNumber + 1,
           r.TotalDue,
           r.TotalDue - dbo.FetchPrincipalAmount(@FirstRate, r.RemainingPrincipal, r.TotalDue),
           dbo.FetchPrincipalAmount(@FirstRate, r.RemainingPrincipal, r.TotalDue),
           CONVERT(DECIMAL(19, 12), r.RemainingPrincipal)
           - dbo.FetchPrincipalAmount(@FirstRate, r.RemainingPrincipal, r.TotalDue)
    FROM RowCTE AS r
    WHERE r.PaymentNumber < 12
    UNION ALL
    SELECT r.PaymentNumber + 1,
           dbo.FetchMonthlyTotal(@SecondRate, r.RemainingPrincipal, @SecondTenure),
           dbo.FetchMonthlyTotal(@SecondRate, r.RemainingPrincipal, @SecondTenure)
           - dbo.FetchPrincipalAmount(
                                    @SecondRate,
                                    r.RemainingPrincipal,
                                    dbo.FetchMonthlyTotal(@SecondRate, r.RemainingPrincipal, @SecondTenure)
                                ),
           dbo.FetchPrincipalAmount(
                                  @SecondRate,
                                  r.RemainingPrincipal,
                                  dbo.FetchMonthlyTotal(@SecondRate, r.RemainingPrincipal, @SecondTenure)
                              ),
           CONVERT(DECIMAL(19, 12), r.RemainingPrincipal)
           - dbo.FetchPrincipalAmount(
                                    @SecondRate,
                                    r.RemainingPrincipal,
                                    dbo.FetchMonthlyTotal(@SecondRate, r.RemainingPrincipal, @SecondTenure)
                                )
    FROM RowCTE AS r
    WHERE r.PaymentNumber = 12
    UNION ALL
    SELECT r.PaymentNumber + 1,
           r.TotalDue,
           r.TotalDue - dbo.FetchPrincipalAmount(@SecondRate, r.RemainingPrincipal, r.TotalDue),
           dbo.FetchPrincipalAmount(@SecondRate, r.RemainingPrincipal, r.TotalDue),
           CONVERT(DECIMAL(19, 12), r.RemainingPrincipal)
           - dbo.FetchPrincipalAmount(@SecondRate, r.RemainingPrincipal, r.TotalDue)
    FROM RowCTE AS r
    WHERE r.PaymentNumber > 12
          AND r.PaymentNumber < 60
   )
SELECT PaymentNumber,
       CAST(TotalDue AS DECIMAL(19, 2)) AS TotalDue,
       CAST(CalculatedInterestDue AS DECIMAL(19, 2)) AS CalculatedInterestDue,
       CAST(PrincipalAmount AS DECIMAL(19, 2)) AS PrincipalAmount,
       CAST(RemainingPrincipal AS DECIMAL(19, 2)) AS RemainingPrincipal
	   FROM RowCTE;

GO