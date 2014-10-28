CREATE TABLE [dbo].[ApiLog] (
    [LogId]          VARCHAR (50)  NOT NULL,
    [RequestDate]    DATETIME      DEFAULT (getdate()) NOT NULL,
    [RequestMethod]  VARCHAR (10)  NOT NULL,
    [RequestUrl]     VARCHAR (255) NULL,
    [HttpStatusCode] INT           NOT NULL,
    [ThreadId]       INT           NULL,
    [RemoteAddress]  VARCHAR (25)  NULL,
    [Username]       VARCHAR (50)  NULL,
    [Message] NVARCHAR(MAX) NULL, 
    [ResponseTime] BIGINT NULL, 
    PRIMARY KEY CLUSTERED ([LogId] ASC)
);

