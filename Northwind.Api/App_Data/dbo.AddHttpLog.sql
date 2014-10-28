CREATE PROCEDURE [dbo].[AddHttpLog]
	 @LogId varchar(25),
	 @RequestDate datetime,
	 @RequestMethod varchar(10),
     @RequestUrl varchar(255),
     @HttpStatusCode int,
	 @ThreadId int,
	 @RemoteAddress varchar(25),
     @Username varchar(50),
	 @Message varchar(max),
	 @ResponseTime bigint
AS
BEGIN
	INSERT INTO [dbo].[ApiLog]
           ([LogId]
           ,[RequestDate]
           ,[RequestMethod]
           ,[RequestUrl]
           ,[HttpStatusCode]
           ,[ThreadId]
           ,[RemoteAddress]
           ,[Username]
		   ,[Message]
		   ,[ResponseTime])
     VALUES
           (@LogId
           ,@RequestDate
           ,@RequestMethod
           ,@RequestUrl
           ,@HttpStatusCode
           ,@ThreadId
           ,@RemoteAddress
           ,@Username
		   ,@Message
		   ,@ResponseTime)
END
