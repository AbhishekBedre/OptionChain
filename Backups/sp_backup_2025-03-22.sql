USE [smarttrader]
GO
/****** Object:  StoredProcedure [dbo].[CheckIntradayBlast]    Script Date: 22-03-2025 08:33:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckIntradayBlast]
	@currentDate VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Temp table to store new triggers
    DECLARE @BlastTable TABLE (
        Symbol NVARCHAR(50),
        LastPrice DECIMAL(18,2),
        PrevLastPrice DECIMAL(18,2),
        PChange DECIMAL(5,2),
        EntryDate DATE,
        Time TIME
    );

    -- Identify records where price change exceeds/decresed 2%
    
	WITH PriceChange AS (
		SELECT
			Symbol,
			LastPrice, 
			LAG(LastPrice) OVER (PARTITION BY Symbol ORDER BY Time) AS PrevLastPrice,
			CASE 
				WHEN LAG(LastPrice) OVER (PARTITION BY Symbol ORDER BY Time) IS NOT NULL 
				THEN ((LastPrice - LAG(LastPrice) OVER (PARTITION BY Symbol ORDER BY Time)) / 
					  LAG(LastPrice) OVER (PARTITION BY Symbol ORDER BY Time)) * 100 
				ELSE NULL
			END AS PChange,
			EntryDate,
			[Time]
		from StockData
		where Entrydate = @currentDate
	)
	INSERT INTO @BlastTable (Symbol, LastPrice, PrevLastPrice, PChange, EntryDate, Time)
	SELECT * FROM PriceChange
	Where PChange >= 1.8 OR PChange <= -1.8
	Order by [Time]

    -- Insert into IntradayBlast with counter logic
    INSERT INTO IntradayBlasts (Symbol, LastPrice, PrevLastPrice, PChange, EntryDate, Time, Counter)
    SELECT 
        bt.Symbol,
        bt.LastPrice,
        bt.PrevLastPrice,
        bt.PChange,
        bt.EntryDate,
        bt.Time,
        COALESCE((SELECT MAX(Counter) FROM IntradayBlasts ib 
                  WHERE ib.Symbol = bt.Symbol AND ib.EntryDate = bt.EntryDate), 0) + 1
    FROM @BlastTable bt
	WHERE NOT EXISTS (
        SELECT 1 FROM IntradayBlasts ib
        WHERE ib.Symbol = bt.Symbol 
        AND ib.EntryDate = bt.EntryDate
        AND DATEPART(HOUR, ib.Time) = DATEPART(HOUR, bt.Time)  -- Compare only Hours
        AND DATEPART(MINUTE, ib.Time) = DATEPART(MINUTE, bt.Time)  -- Compare only Minutes
    );
END;
GO
/****** Object:  StoredProcedure [dbo].[GetOpenLowHighStock]    Script Date: 22-03-2025 08:33:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetOpenLowHighStock] 
	-- Add the parameters for the stored procedure here
	@currentDate VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CurrentTime TIME;

	SELECT TOP 1 @CurrentTime = Time FROM StockData WHERE entrydate=@currentDate and [Time] >= '09:16';
	PRINT @CurrentTime
	
    -- Insert statements for procedure here
	SELECT * FROM 
	(
		SELECT CONVERT(VARCHAR(5), sd.[Time], 108) Time, 'Positive' AS 'Type', sd.Id, sd.Symbol, LastPrice, PChange, Change, sd.DayHigh, sd.DayLow, [Open], 
		ROUND(CAST(ISNULL((SELECT TOP 1 RFactor FROM RFactors WHERE Symbol=sd.Symbol AND Entrydate=@currentDate),0.0) AS FLOAT),2) AS 'TFactor' 
		FROM stockdata sd
		WHERE sd.entrydate=@currentDate 
			AND [Open] = sd.DayLow 
			AND LastPrice > 500 
			AND PChange > 1 
			AND CONVERT(VARCHAR(5), sd.[Time], 108) LIKE CONVERT(VARCHAR(5), @CurrentTime, 108) + '%'

		UNION ALL

		SELECT CONVERT(VARCHAR(5), sd.[Time], 108) Time, 'Negetive' AS 'Type', sd.Id, sd.Symbol, LastPrice, PChange, Change, sd.DayHigh, sd.DayLow, [Open], 
		ROUND(CAST(ISNULL((SELECT TOP 1 RFactor FROM RFactors WHERE Symbol=sd.Symbol AND Entrydate=@currentDate),0.0) AS FLOAT),2) AS 'TFactor' 
		FROM stockdata sd
		WHERE sd.entrydate=@currentDate 
			AND [Open] = sd.DayHigh 
			AND LastPrice > 500 
			AND PChange < -1 
			AND CONVERT(VARCHAR(5), sd.[Time], 108) LIKE CONVERT(VARCHAR(5), @CurrentTime, 108) + '%'
		
	)T
	
	ORDER BY pChange DESC

END


-- EXEC [GetOpenLowHighStock] '2025-02-19'
GO
/****** Object:  StoredProcedure [dbo].[StockWithSectors]    Script Date: 22-03-2025 08:33:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StockWithSectors] 
	@CurrentDate VARCHAR(50) = '2025-02-10'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		S.Id, 
		S.Symbol, 
		S.PChange, 
		S.Change, 
		S.LastPrice, 
		S.[Open], 
		S.DayHigh, 
		S.DayLow, 
		CAST(S.[Time] AS VARCHAR(50)) 'Time', 
		ISNULL(
		(
			SELECT top 1 MappingName
			FROM Sectors
			WHERE S.Symbol = Symbol
			GROUP BY Symbol, MappingName
		),'OTHER') 'SectorName',
		(
			SELECT RFactor 
			FROM RFactors
			WHERE ID IN (
				SELECT MAX(ID) ID
				FROM RFactors
				WHERE S.Symbol = Symbol AND EntryDate=@CurrentDate
				GROUP BY Symbol, EntryDate
			)
		) 'TFactor',
		ISNULL(
		(
			SELECT IsFNOSec FROM StockMetaData WHERE S.Symbol= Symbol
		),0) 'IsFNOSec',
		ISNULL(
		(
			SELECT IsNifty50 FROM StockMetaData WHERE S.Symbol= Symbol
		),0) 'IsNifty50',
		ISNULL(
		(
			SELECT IsNifty100 FROM StockMetaData WHERE S.Symbol= Symbol
		),0) 'IsNifty100',
		ISNULL(
		(
			SELECT IsNifty200 FROM StockMetaData WHERE S.Symbol= Symbol
		),0) 'IsNifty200'

	FROM StockData S 
	WHERE ID IN (
		SELECT MAX(ID) ID 
		FROM StockData SD 
			WHERE SD.EntryDate = @CurrentDate
			GROUP BY SD.Symbol, SD.EntryDate
	)
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateRelativeFactor]    Script Date: 22-03-2025 08:33:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC [dbo].[UpdateRelativeFactor]
CREATE PROCEDURE [dbo].[UpdateRelativeFactor]
AS
BEGIN
    SET NOCOUNT ON;

    -- Temporary table to store the calculated relative factors
    CREATE TABLE #TempRFactor (
        Symbol NVARCHAR(50),
		DayHigh FLOAT,
        DayLow FLOAT,
        EntryDate DATE,
        [Time] TIME,
        LastPrice FLOAT,
        WeightedSum FLOAT,
        RFactor FLOAT
    );

    DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);

    -- Temporary table to store the last 20 days' stock data
    CREATE TABLE #TempStockData (
        Id INT,
        Symbol NVARCHAR(50),
        EntryDate DATE,
        Time TIME,
        LastPrice FLOAT,
        DayHigh FLOAT,
        DayLow FLOAT,
        Rank INT,
        [Weight] FLOAT
    );

    -- Insert last 20 days of stock data (excluding today) with ranking
    INSERT INTO #TempStockData (Id, Symbol, EntryDate, Time, LastPrice, DayHigh, DayLow, Rank)
    SELECT 
        SD.Id, 
        SD.Symbol, 
        SD.EntryDate, 
        SD.[Time], 
        SD.LastPrice,
        SD.DayHigh,
        SD.DayLow,
        ROW_NUMBER() OVER (PARTITION BY SD.Symbol ORDER BY SD.EntryDate DESC) AS Rank
    FROM StockData SD
    WHERE SD.Id IN (
        SELECT MAX(Id) 
        FROM StockData 
        WHERE EntryDate >= DATEADD(DAY, -45, '2025-03-14') 
        GROUP BY Symbol, EntryDate
    )
    ORDER BY SD.Symbol, SD.EntryDate DESC;

    -- Update weights based on Rank
    UPDATE #TempStockData 
    SET [Weight] = ((DayHigh - DayLow) * 
        CASE 
            WHEN Rank = 1 THEN 7
            WHEN Rank BETWEEN 2 AND 6 THEN 5
            WHEN Rank BETWEEN 7 AND 16 THEN 3
            WHEN Rank >= 17 THEN 1
        END
    );

    -- Insert into final temp table with calculated RFactor
    INSERT INTO #TempRFactor (Symbol, DayHigh, DayLow, EntryDate, [Time], LastPrice, WeightedSum, RFactor)
    SELECT 
        TSD.Symbol,
		TSD.DayHigh,
		TSD.DayLow,
        EntryDate,
        TSD.[Time], -- Latest available time
        TSD.LastPrice,
        TSD.[Weight] AS WeightedSum,
        ((TSD.[Weight] / (TSD.LastPrice * 66)) * 100) * 10 AS RFactor
    FROM #TempStockData TSD
    
    -- Merge into RFactors table
    MERGE INTO RFactors AS Target
    USING #TempRFactor AS Source
    ON Target.Symbol = Source.Symbol
       AND Target.EntryDate = Source.EntryDate
       AND Target.Time = Source.Time
    WHEN MATCHED THEN
        UPDATE SET 
            Target.DayHigh = Source.DayHigh,
            Target.DayLow = Source.DayLow,
            Target.Price = Source.LastPrice,
            Target.RFactor = Source.RFactor
    WHEN NOT MATCHED THEN
        INSERT (Symbol, DayHigh, DayLow, Price, EntryDate, [Time], RFactor)
        VALUES (Source.Symbol, Source.DayHigh, Source.DayLow, Source.LastPrice, Source.EntryDate, Source.Time, Source.RFactor);

    -- Cleanup
    DROP TABLE #TempStockData;
    DROP TABLE #TempRFactor;

    SET NOCOUNT OFF;
END
GO
/****** Object:  StoredProcedure [dbo].[WeeklyMarketUpdate]    Script Date: 22-03-2025 08:33:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[WeeklyMarketUpdate]
AS
BEGIN
    WITH EODEntries AS (
        SELECT 
			Id,
            IndexSymbol,
            Last,
            EntryDate,
            Time,
			PercentChange,
            DATEADD(DAY, -DATEPART(WEEKDAY, EntryDate) + 2, CAST(EntryDate AS DATE)) AS WeekStartDate
        FROM BroderMarkets WHERE Id IN (
			SELECT Max(Id) FROM BroderMarkets 
			WHERE [Key]='SECTORAL INDICES' AND EntryDate > DATEADD(DAY, -60, GETDATE()) 
			Group BY IndexSymbol, EntryDate
		)
    )
    SELECT 
        IndexSymbol as [Name],
        WeekStartDate,
        DATEADD(DAY, 4, WeekStartDate) AS WeekEndDate, -- Gets Friday
        AVG(Last) AS WeeklyAverage,
		SUM(PercentChange) AS PChange
    FROM EODEntries
	GROUP BY IndexSymbol, WeekStartDate
	Order by WeekStartDate desc
END

-- EXEC WeeklyMarketUpdate

/*
NIFTY IT
NIFTY PVT BANK
NIFTY BANK
NIFTY FIN SERVICE
NIFTY CONSR DURBL
NIFTY PHARMA
NIFTY HEALTHCARE
NIFTY AUTO
NIFTY MEDIA
NIFTY FMCG
NIFTY MS IT TELCM
NIFTY CONSUMPTION
NIFTY PSU BANK
NIFTY REALTY
NIFTY INFRA
NIFTY OIL AND GAS
NIFTY PSE
NIFTY METAL
NIFTY CPSE

*/



GO
/****** Object:  StoredProcedure [dbo].[WeeklyStockUpdate]    Script Date: 22-03-2025 08:33:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[WeeklyStockUpdate]
	@CurrentDate VARCHAR(50) = '2025-02-10',
	@weekEndDate VARCHAR(50) = '2025-02-14'
AS
BEGIN
	SELECT T.*, S.[Open], S.DayHigh, S.DayLow, S.LastPrice, S.EntryDate, CAST(S.[Time] AS VARCHAR(50)) 'Time', 0 'IsNifty50', 0 'IsNifty100', 0 'IsNifty200',
	(
		SELECT AVG(RFactor) TFactor FROM RFactors T 
		WHERE S.Symbol = T.Symbol AND EntryDate >= @CurrentDate AND EntryDate <= @weekEndDate
	)* 10 TFactor
	FROM (
		SELECT MAX(Id) Id, Symbol,  SUM(Change) 'Change', SUM(PChange) 'PChange'
		FROM StockData
		Where Id IN (
			SELECT MAX(ID) AS MaxID 
			FROM StockData 
				WHERE EntryDate >= @CurrentDate AND EntryDate <= @weekEndDate
				GROUP BY Symbol, EntryDate
		)
		GROUP BY Id, Symbol, Change, PChange, [Open], DayHigh, DayLow, LastPrice, EntryDate, [Time], Change, PChange
		HAVING PChange > 5 OR PChange < -5
	)T
	INNER JOIN StockData S ON T.Id = S.Id
END



-- EXEC WeeklyStockUpdate '2025-03-11', '2025-03-15'
GO
