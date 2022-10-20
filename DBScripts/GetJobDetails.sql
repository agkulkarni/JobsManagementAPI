USE [Jobs]
GO

/****** Object:  StoredProcedure [dbo].[GetJobDetails]    Script Date: 21-10-2022 01:48:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetJobDetails] 
	@SearchStr nvarchar(max)='', 
	@PageNo int=0,
	@PageSize int=0,
	@LocationId bigint = 0,
	@DepartmentID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Query nvarchar(max);
	SET @Query = 'SELECT a.JobId, a.JobCode, a.Title, b.Name location, c.Title department, a.ClosingDate, a.PostDate FROM Job a left join Location b on a.LocId=b.LocId left join Department c on a.DeptId=c.DeptId';
	DECLARE @WhereCond nvarchar(max) = '';
	if nullif(@SearchStr, '') <> '' 
		if nullif(@WhereCond, '') <> ''	
			SET @WhereCond = @WhereCond +' AND UPPER(a.Description) like ''%'+UPPER(@SearchStr)+'%''';
		ELSE
			SET @WhereCond = @WhereCond + ' WHERE UPPER(a.Description) like ''%'+UPPER(@SearchStr)+'%''';
	if @LocationId > 0 
		if nullif(@WhereCond, '') <> ''	
			SET @WhereCond =  @WhereCond +' AND a.LocId='+CAST(@LocationId AS nvarchar);
		ELSE
			SET @WhereCond = @WhereCond + ' WHERE a.LocId='+CAST(@LocationId AS nvarchar);
	if @DepartmentID > 0 
		if nullif(@WhereCond, '') <> ''	
			SET @WhereCond = @WhereCond + ' AND a.DeptId='+CAST(@DepartmentID AS nvarchar);
		ELSE
			SET @WhereCond = @WhereCond + ' WHERE a.DeptId='+CAST(@DepartmentID AS nvarchar);
	if @PageNo > 0 and @PageSize > 0
		if @PageNo = 1	
			SET @WhereCond = @WhereCond + ' ORDER BY JobId offset 0 rows fetch next '+CAST(@PageSize AS NVARCHAR)+' rows only';
		ELSE
			SET @WhereCond = @WhereCond + ' ORDER BY JobId offset '+CAST(@PageNo*@PageSize AS NVARCHAR)+' rows fetch next '+CAST(@PageNo*@PageSize AS NVARCHAR)+' rows only';

	if(nullif(@WhereCond, '')) <> ''
		set @Query = CONCAT(@Query, @WhereCond);
	print(@Query);
	exec(@Query);
END
GO


