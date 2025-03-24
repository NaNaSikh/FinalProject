-- ბონუსის ჩარიცხვა 
--GO
--CREATE PROCEDURE AddBonuses
--    @EmployeeId INT,
--    @BonusAmount DECIMAL(18,2)
--AS
--BEGIN
--    SET NOCOUNT ON;   
--    DECLARE @Salary DECIMAL(18,2), @MaxBonusAmount DECIMAL(18,2), @MaxBonusPercentage INT,
--            @MinBonusPercentage INT, @MaxRecommendationLevel INT, @RecommendationBonusRate DECIMAL(18,2),
--            @CreateDate DATETIME, @BonusPercentage DECIMAL(18,2);

--    SET @CreateDate = GETUTCDATE();
    
--    SELECT @Salary = Salary
--    FROM Employees
--    WHERE Id = @EmployeeId;

--    IF @Salary IS NULL
--    BEGIN
--        THROW 50000, 'თანამშრომლის ხელფასი ვერ მოიძებნა.', 1;
--        RETURN;
--    END;
    
--    SET @BonusPercentage = (@BonusAmount / @Salary) * 100;
    
--    SELECT @MaxBonusAmount = MaxBonusAmount,
--           @MaxBonusPercentage = MaxBonusPercentage,
--           @MinBonusPercentage = MinBonusPercentage,
--           @MaxRecommendationLevel = MaxRecommendationLevel,
--           @RecommendationBonusRate = RecommendationBonusRate
--    FROM BonusConfigurations;
    
--    IF @BonusAmount > @MaxBonusAmount
--    BEGIN
--        THROW 50001, 'ბონუსის თანხა სცდება შესაბამის ლიმიტს.', 1;
--        RETURN;
--    END;
    
--    IF @BonusPercentage < @MinBonusPercentage
--    BEGIN
--        THROW 50002, 'ბონუსის თანხა ნაკლებია შესაბამის ლიმიტზე.', 1;
--        RETURN;
--    END;
    
--    DECLARE @CurrentEmployeeId INT = @EmployeeId, @CurrentBonusAmount DECIMAL(18,2) = @BonusAmount;
--    DECLARE @Level INT = 1;

--    WHILE @Level <= @MaxRecommendationLevel
--    BEGIN
--        DECLARE @RecommenderId INT;

--        SELECT @RecommenderId = RecommenderEmployeeId
--        FROM Employees
--        WHERE Id = @CurrentEmployeeId;

--        IF @RecommenderId IS NULL
--            BREAK;
        
--        SET @CurrentBonusAmount = (@CurrentBonusAmount * @RecommendationBonusRate) / 100;
       
--        INSERT INTO Bonuses (EmployeeId, Amount, IsRecommenderBonus, Reason, CreateDate, CreateByUserId, RecommendationLevel)
--        VALUES (@RecommenderId, @CurrentBonusAmount, 1, 'რეკომენდატორის ბონუსი', @CreateDate, 1, @Level);
     
--        SET @CurrentEmployeeId = @RecommenderId;
--        SET @Level = @Level + 1;
--    END;
--END;
--GO

-- მითითებულ თარიღებში გაცემული ბონუსების რაოდენობა  GetTotalBonuses

SELECT 
    COUNT(*) AS TotalBonuses, 
    SUM(Amount) AS TotalBonusAmount
FROM Bonuses
WHERE CreateDate BETWEEN @StartDate AND @EndDate;


-- 10 თანამშრომელი, ვინც ყველაზე მეტი ბონუსი მიიღო მითითებულ თარიღში GetTenEmployeeByBonuses
GO
CREATE PROCEDURE GetTenEmployeeByBonuses
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT TOP 10 e.Id, e.FirstName, e.LastName, SUM(b.Amount) AS TotalBonus
    FROM Employees e
    JOIN Bonuses b ON e.Id = b.EmployeeId
    WHERE b.CreateDate BETWEEN @StartDate AND @EndDate
    GROUP BY e.Id, e.FirstName, e.LastName
    ORDER BY TotalBonus DESC
END;

-- 10 თანამშრომელი, ვისი რეკომენდაციითაც ყველაზე მეტი ბონუსი გაიცა მითითებულ თარიღში GetTenRecommenderByBonuses
GO
CREATE PROCEDURE GetTenRecommenderByBonuses
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON; 
    WITH BonusSums AS (
        SELECT 
            b.EmployeeId, 
            SUM(b.Amount) AS TotalBonus
        FROM Bonuses b
        WHERE 
            b.CreateDate BETWEEN @StartDate AND @EndDate
            AND b.IsRecommenderBonus = 0
        GROUP BY b.EmployeeId
    )
    SELECT TOP 10
        e.RecommenderEmployeeId AS RecommenderId, 
        CONCAT(r.FirstName, ' ', r.LastName) AS RecommenderName, 
        COUNT(bs.EmployeeId) AS TotalRecommendedBonuses,
        SUM(bs.TotalBonus) AS TotalBonusAmount
    FROM BonusSums bs
    JOIN Employees e ON e.Id = bs.EmployeeId
    JOIN Employees r ON r.Id = e.RecommenderEmployeeId
    WHERE e.RecommenderEmployeeId IS NOT NULL
    GROUP BY e.RecommenderEmployeeId, r.FirstName, r.LastName
    ORDER BY TotalBonusAmount DESC;
END;
GO


-- ჯამურად გაცემული ხელფასების რაოდენობა მითითებულ თარიღში  დეპარტამენტების მიხედვით GetTotalSalariesByDepartment
GO
CREATE PROCEDURE GetTotalSalariesByDepartment
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT d.Id, d.Name, SUM(e.Salary) AS TotalSalary
    FROM Employees e
    JOIN Departments d ON e.DepartmentId = d.Id
    WHERE e.HireDate <= @EndDate
    GROUP BY d.Id, d.Name
END;


-- ჯამურად გაცემული ბონუსების რაოდენობა მითითებულ თარიღში დეპარტამენტების მიხედვით 
GO
CREATE PROCEDURE GetTotalBonusesByDepartment
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT d.Id, d.Name, SUM(b.Amount) AS TotalBonuses
    FROM Bonuses b
    JOIN Employees e ON b.EmployeeId = e.Id
    JOIN Departments d ON e.DepartmentId = d.Id
    WHERE b.CreateDate BETWEEN @StartDate AND @EndDate
    GROUP BY d.Id, d.Name
END;
