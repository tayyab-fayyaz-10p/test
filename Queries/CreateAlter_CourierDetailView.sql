USE [SMSAStgDB]
GO

IF OBJECT_ID ('[dbo].[CourierDetailView]', 'V') IS NULL
BEGIN
EXEC (
	'CREATE VIEW [dbo].[CourierDetailView] 
	AS
	SELECT JAD.Id AS ID
		   ,(CASE WHEN DP.Id IS NOT NULL THEN CONCAT(''FL-'', CONVERT(varchar(50), DP.Id)) ELSE NULL END) AS CourierID
		   ,J.SubstationId AS StationID
		   ,S.Name AS StationName
		   ,I.Awb AS AWB
		   ,JAD.ScanTime AS ScanTime
		   ,JAD.ScanAddress AS Location
		   ,JAD.ScanLatitude AS Latitude
		   ,JAD.ScanLongitude AS Longitude
		   ,JA.SigneeName AS SigneeName
		   ,(CASE WHEN JA.AssignmentStatus = 7 THEN JE.ExceptionType
			 WHEN (JA.AssignmentStatus = 5 OR JA.AssignmentStatus = 8) AND JA.AssignmentType = 1 THEN ''PU''
			 WHEN (JA.AssignmentStatus = 5 OR JA.AssignmentStatus = 8) AND JA.AssignmentType = 2 THEN ''DL''
			 WHEN JA.AssignmentStatus = 4 THEN ''Abandoned''
			 WHEN JA.AssignmentStatus = 6 THEN ''InProgress''
			 ELSE CONVERT(varchar(50), JA.AssignmentStatus) END) AS ScanCode
		   ,JA.Comments AS Comments
	FROM JobAssignmentDetails JAD
	INNER JOIN JobAssignments JA ON JA.Id = JAD.JobAssignmentId
	INNER JOIN Jobs J ON J.Id = JA.JobId
	INNER JOIN Substations S ON S.Id = J.SubstationId
	LEFT JOIN DeliveryPartners DP ON DP.UserId = J.DriverId
	INNER JOIN JobItems JI ON JI.Id = JAD.JobItemId AND JI.JobId = J.Id
	INNER JOIN Items I ON I.Id = JI.ItemId
	LEFT JOIN JobExceptions JE ON JE.JobAssignmentId = JA.Id'
	)
END
ELSE
BEGIN
EXEC (
	'ALTER VIEW [dbo].[CourierDetailView] 
	AS
	SELECT JAD.Id AS ID
		   ,(CASE WHEN DP.Id IS NOT NULL THEN CONCAT(''FL-'', CONVERT(varchar(50), DP.Id)) ELSE NULL END) AS CourierID
		   ,J.SubstationId AS StationID
		   ,S.Name AS StationName
		   ,I.Awb AS AWB
		   ,JAD.ScanTime AS ScanTime
		   ,JAD.ScanAddress AS Location
		   ,JAD.ScanLatitude AS Latitude
		   ,JAD.ScanLongitude AS Longitude
		   ,JA.SigneeName AS SigneeName
		   ,(CASE WHEN JA.AssignmentStatus = 7 THEN JE.ExceptionType
			 WHEN (JA.AssignmentStatus = 5 OR JA.AssignmentStatus = 8) AND JA.AssignmentType = 1 THEN ''PU''
			 WHEN (JA.AssignmentStatus = 5 OR JA.AssignmentStatus = 8) AND JA.AssignmentType = 2 THEN ''DL''
			 WHEN JA.AssignmentStatus = 4 THEN ''Abandoned''
			 WHEN JA.AssignmentStatus = 6 THEN ''InProgress''
			 ELSE CONVERT(varchar(50), JA.AssignmentStatus) END) AS ScanCode
		   ,JA.Comments AS Comments
	FROM JobAssignmentDetails JAD
	INNER JOIN JobAssignments JA ON JA.Id = JAD.JobAssignmentId
	INNER JOIN Jobs J ON J.Id = JA.JobId
	INNER JOIN Substations S ON S.Id = J.SubstationId
	LEFT JOIN DeliveryPartners DP ON DP.UserId = J.DriverId
	INNER JOIN JobItems JI ON JI.Id = JAD.JobItemId AND JI.JobId = J.Id
	INNER JOIN Items I ON I.Id = JI.ItemId
	LEFT JOIN JobExceptions JE ON JE.JobAssignmentId = JA.Id'
	)
END
GO