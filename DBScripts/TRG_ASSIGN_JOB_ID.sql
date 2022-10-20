--====================================
--  Create database trigger template 
--====================================
USE [Jobs]
GO
CREATE TRIGGER TRG_ASSIGN_JOB_ID   
ON Job after Insert   
AS   
BEGIN     
	DECLARE @JobID bigint;   
	SET @JobID = next value for seq_job_id;   
	update Job set JobCode = 'JOB_'+@JobID where JobId=(select JobId from inserted);  
END;
GO


