/* =========================================
   Lost & Found Database
   13 Tables 
   ========================================= */


/* =========================================
   1. USER
   ========================================= */

CREATE TABLE [User] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [FullName] NVARCHAR(150),
    [Email] NVARCHAR(255) UNIQUE,
    [PhoneNumber] NVARCHAR(20),
    [ProfileImageUrl] NVARCHAR(1000),
    [IsVerified] BIT,
    [IsBlocked] BIT,
    [IsDeleted] BIT,
    [CreatedAt] DATETIME
);
GO


/* =========================================
   2. USER VERIFICATION
   ========================================= */

CREATE TABLE [UserVerification] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [UserId] INT,
    [Method] NVARCHAR(50),
    [Status] NVARCHAR(50),
    [DocumentUrl] NVARCHAR(1000),
    [ReviewedByAdminId] INT,
    [ReviewedAt] DATETIME
);
GO


/* =========================================
   3. USER LOCATION PREFERENCE
   ========================================= */

CREATE TABLE [UserLocationPreference] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [UserId] INT UNIQUE,
    [Latitude] FLOAT,
    [Longitude] FLOAT,
    [RadiusKm] FLOAT,
    [NotifyEnabled] BIT
);
GO


/* =========================================
   4. NOTIFICATION
   ========================================= */

CREATE TABLE [Notification] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [UserId] INT,
    [Type] NVARCHAR(50),
    [Message] NVARCHAR(MAX),
    [ReferenceType] NVARCHAR(50),
    [ReferenceId] INT,
    [IsRead] BIT,
    [CreatedAt] DATETIME
);
GO


/* =========================================
   5. REPORT
   ========================================= */

CREATE TABLE [Report] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [UserId] INT,
    [ReportType] NVARCHAR(50),
    [TargetType] NVARCHAR(50),
    [Title] NVARCHAR(200),
    [Description] NVARCHAR(MAX),
    [EventDate] DATETIME,
    [Latitude] FLOAT,
    [Longitude] FLOAT,
    [Address] NVARCHAR(300),
    [City] NVARCHAR(100),
    [Status] NVARCHAR(50),
    [IsVerified] BIT,
    [IsDeleted] BIT,
    [CreatedAt] DATETIME,
    [UpdatedAt] DATETIME
);
GO


/* =========================================
   6. PERSON DETAIL
   ========================================= */

CREATE TABLE [PersonDetail] (
    [ReportId] INT PRIMARY KEY,
    [PersonName] NVARCHAR(150),
    [Age] INT,
    [Gender] NVARCHAR(20),
    [HairColor] NVARCHAR(50),
    [EyeColor] NVARCHAR(50),
    [Height] FLOAT,
    [ClothingDescription] NVARCHAR(MAX),
    [DistinguishingMarks] NVARCHAR(MAX),
    [MedicalCondition] NVARCHAR(MAX),
    [ContactPhoneForFamily] NVARCHAR(20)
);
GO


/* =========================================
   7. ITEM CATEGORY
   ========================================= */

CREATE TABLE [ItemCategory] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(100),
    [ParentCategoryId] INT
);
GO


/* =========================================
   8. ITEM DETAIL
   ========================================= */

CREATE TABLE [ItemDetail] (
    [ReportId] INT PRIMARY KEY,
    [CategoryId] INT,
    [Brand] NVARCHAR(100),
    [Model] NVARCHAR(100),
    [Color] NVARCHAR(50),
    [SerialNumber] NVARCHAR(100),
    [Size] NVARCHAR(50)
);
GO


/* =========================================
   9. REPORT IMAGE
   ========================================= */

CREATE TABLE [ReportImage] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [ReportId] INT,
    [ImageUrl] NVARCHAR(1000),
    [IsPrimary] BIT,
    [FaceEmbedding] VARBINARY(MAX),
    [ImageHash] NVARCHAR(256),
    [UploadedAt] DATETIME
);
GO


/* =========================================
   10. MATCH
   ========================================= */

CREATE TABLE [Match] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [LostReportId] INT,
    [FoundReportId] INT,
    [TotalScore] FLOAT,
    [LocationScore] FLOAT,
    [DateScore] FLOAT,
    [DescriptionScore] FLOAT,
    [ImageScore] FLOAT,
    [Status] NVARCHAR(50),
    [CreatedAt] DATETIME,
    [ConfirmedAt] DATETIME
);
GO


/* =========================================
   11. REPORT FLAG
   ========================================= */

CREATE TABLE [ReportFlag] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [ReportId] INT,
    [ReportedByUserId] INT,
    [Reason] NVARCHAR(MAX),
    [Status] NVARCHAR(50),
    [ReviewedByAdminId] INT,
    [ReviewedAt] DATETIME
);
GO


/* =========================================
   12. CONVERSATION
   ========================================= */

CREATE TABLE [Conversation] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [MatchId] INT UNIQUE,
    [CreatedAt] DATETIME
);
GO


/* =========================================
   13. MESSAGE
   ========================================= */

CREATE TABLE [Message] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [ConversationId] INT,
    [SenderId] INT,
    [Content] NVARCHAR(MAX),
    [SentAt] DATETIME,
    [IsRead] BIT
);
GO


/* =========================================
   UNIQUE INDEX FOR MATCH
   ========================================= */

CREATE UNIQUE INDEX [Match_index_0]
ON [Match] ([LostReportId], [FoundReportId]);
GO

/* =========================================
   ADDITIONAL INDEXES
   Lost & Found Project
   ========================================= */


/* 1. Search reports by city */

CREATE NONCLUSTERED INDEX IX_Report_City
ON [Report] ([City]);
GO


/* 2. Filter reports by type and target */

CREATE NONCLUSTERED INDEX IX_Report_ReportType_TargetType
ON [Report] ([ReportType], [TargetType]);
GO


/* 3. Search reports by event date */

CREATE NONCLUSTERED INDEX IX_Report_EventDate
ON [Report] ([EventDate]);
GO


/* 4. Get reports created by a specific user */

CREATE NONCLUSTERED INDEX IX_Report_UserId
ON [Report] ([UserId]);
GO


/* 5. Get reports by status */

CREATE NONCLUSTERED INDEX IX_Report_Status
ON [Report] ([Status]);
GO


/* 6. Get images belonging to a report */

CREATE NONCLUSTERED INDEX IX_ReportImage_ReportId
ON [ReportImage] ([ReportId]);
GO


/* 7. Get messages belonging to a conversation */

CREATE NONCLUSTERED INDEX IX_Message_ConversationId
ON [Message] ([ConversationId]);
GO


/* 8. Get messages sent by a specific user */

CREATE NONCLUSTERED INDEX IX_Message_SenderId
ON [Message] ([SenderId]);
GO


/* 9. Find notifications for a user */

CREATE NONCLUSTERED INDEX IX_Notification_UserId
ON [Notification] ([UserId]);
GO


/* 10. Find flags belonging to a report */

CREATE NONCLUSTERED INDEX IX_ReportFlag_ReportId
ON [ReportFlag] ([ReportId]);
GO


/* 11. Find verifications belonging to a user */

CREATE NONCLUSTERED INDEX IX_UserVerification_UserId
ON [UserVerification] ([UserId]);
GO
/* =========================================
   COLUMN DESCRIPTIONS
   ========================================= */

EXEC sp_addextendedproperty
    @name = N'Column_Description',
    @value = N'Lost / Found',
    @level0type = N'Schema',
    @level0name = N'dbo',
    @level1type = N'Table',
    @level1name = N'Report',
    @level2type = N'Column',
    @level2name = N'ReportType';
GO

EXEC sp_addextendedproperty
    @name = N'Column_Description',
    @value = N'Person / Item',
    @level0type = N'Schema',
    @level0name = N'dbo',
    @level1type = N'Table',
    @level1name = N'Report',
    @level2type = N'Column',
    @level2name = N'TargetType';
GO


