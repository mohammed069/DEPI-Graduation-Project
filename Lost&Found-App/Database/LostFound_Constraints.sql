/* =========================================
   FOREIGN KEYS CONSTRAINTS
   ========================================= */


/* User Verification */

ALTER TABLE [UserVerification]
ADD CONSTRAINT [FK_UserVerification_User]
FOREIGN KEY ([UserId]) REFERENCES [User]([Id]);
GO

ALTER TABLE [UserVerification]
ADD CONSTRAINT [FK_UserVerification_ReviewedByAdmin]
FOREIGN KEY ([ReviewedByAdminId]) REFERENCES [User]([Id]);
GO


/* User Location Preference */

ALTER TABLE [UserLocationPreference]
ADD CONSTRAINT [FK_UserLocationPreference_User]
FOREIGN KEY ([UserId]) REFERENCES [User]([Id]);
GO


/* Notification */

ALTER TABLE [Notification]
ADD CONSTRAINT [FK_Notification_User]
FOREIGN KEY ([UserId]) REFERENCES [User]([Id]);
GO


/* Report */

ALTER TABLE [Report]
ADD CONSTRAINT [FK_Report_User]
FOREIGN KEY ([UserId]) REFERENCES [User]([Id]);
GO


/* Person Detail */

ALTER TABLE [PersonDetail]
ADD CONSTRAINT [FK_PersonDetail_Report]
FOREIGN KEY ([ReportId]) REFERENCES [Report]([Id]);
GO


/* Item Category */

ALTER TABLE [ItemCategory]
ADD CONSTRAINT [FK_ItemCategory_ParentCategory]
FOREIGN KEY ([ParentCategoryId]) REFERENCES [ItemCategory]([Id]);
GO


/* Item Detail */

ALTER TABLE [ItemDetail]
ADD CONSTRAINT [FK_ItemDetail_Report]
FOREIGN KEY ([ReportId]) REFERENCES [Report]([Id]);
GO

ALTER TABLE [ItemDetail]
ADD CONSTRAINT [FK_ItemDetail_Category]
FOREIGN KEY ([CategoryId]) REFERENCES [ItemCategory]([Id]);
GO


/* Report Image */

ALTER TABLE [ReportImage]
ADD CONSTRAINT [FK_ReportImage_Report]
FOREIGN KEY ([ReportId]) REFERENCES [Report]([Id]);
GO


/* Match */

ALTER TABLE [Match]
ADD CONSTRAINT [FK_Match_LostReport]
FOREIGN KEY ([LostReportId]) REFERENCES [Report]([Id]);
GO

ALTER TABLE [Match]
ADD CONSTRAINT [FK_Match_FoundReport]
FOREIGN KEY ([FoundReportId]) REFERENCES [Report]([Id]);
GO


/* Report Flag */

ALTER TABLE [ReportFlag]
ADD CONSTRAINT [FK_ReportFlag_Report]
FOREIGN KEY ([ReportId]) REFERENCES [Report]([Id]);
GO

ALTER TABLE [ReportFlag]
ADD CONSTRAINT [FK_ReportFlag_ReportedByUser]
FOREIGN KEY ([ReportedByUserId]) REFERENCES [User]([Id]);
GO

ALTER TABLE [ReportFlag]
ADD CONSTRAINT [FK_ReportFlag_ReviewedByAdmin]
FOREIGN KEY ([ReviewedByAdminId]) REFERENCES [User]([Id]);
GO


/* Conversation */

ALTER TABLE [Conversation]
ADD CONSTRAINT [FK_Conversation_Match]
FOREIGN KEY ([MatchId]) REFERENCES [Match]([Id]);
GO


/* Message */

ALTER TABLE [Message]
ADD CONSTRAINT [FK_Message_Conversation]
FOREIGN KEY ([ConversationId]) REFERENCES [Conversation]([Id]);
GO

ALTER TABLE [Message]
ADD CONSTRAINT [FK_Message_Sender]
FOREIGN KEY ([SenderId]) REFERENCES [User]([Id]);
GO

   ALTER TABLE [ReportFlag]
ALTER COLUMN [Reason] NVARCHAR(MAX) NOT NULL;
GO

/*========================================================================================================================================================*/

/* =========================================
   CHECK CONSTRAINTS - GROUP 1
   ========================================= */


/* 1. PersonDetail: Age must not be negative  */

ALTER TABLE [PersonDetail]
ADD CONSTRAINT CK_PersonDetail_Age
CHECK (
    [Age] IS NULL
    OR [Age] BETWEEN 0 AND 120
);
GO


/* 2. PersonDetail: Height must be positive */

ALTER TABLE [PersonDetail]
ADD CONSTRAINT CK_PersonDetail_Height
CHECK ([Height] > 0);
GO


/* 3. Report: Latitude must be between -90 and 90 */

ALTER TABLE [Report]
ADD CONSTRAINT CK_Report_Latitude
CHECK ([Latitude] BETWEEN -90 AND 90);
GO


/* 4. Report: Longitude must be between -180 and 180 */

ALTER TABLE [Report]
ADD CONSTRAINT CK_Report_Longitude
CHECK ([Longitude] BETWEEN -180 AND 180);
GO


/* 5. UserLocationPreference: Latitude range */

ALTER TABLE [UserLocationPreference]
ADD CONSTRAINT CK_UserLocationPreference_Latitude
CHECK ([Latitude] BETWEEN -90 AND 90);
GO


/* 6. UserLocationPreference: Longitude range */

ALTER TABLE [UserLocationPreference]
ADD CONSTRAINT CK_UserLocationPreference_Longitude
CHECK ([Longitude] BETWEEN -180 AND 180);
GO


/* 7. UserLocationPreference: Radius must be positive */

ALTER TABLE [UserLocationPreference]
ADD CONSTRAINT CK_UserLocationPreference_RadiusKm
CHECK ([RadiusKm] > 0);
GO

/* =========================================
   CHECK CONSTRAINTS - GROUP 2
   ========================================= */


/* 1. Total Score */

ALTER TABLE [Match]
ADD CONSTRAINT CK_Match_TotalScore
CHECK ([TotalScore] BETWEEN 0 AND 100);
GO


/* 2. Location Score */

ALTER TABLE [Match]
ADD CONSTRAINT CK_Match_LocationScore
CHECK ([LocationScore] BETWEEN 0 AND 100);
GO


/* 3. Date Score */

ALTER TABLE [Match]
ADD CONSTRAINT CK_Match_DateScore
CHECK ([DateScore] BETWEEN 0 AND 100);
GO


/* 4. Description Score */

ALTER TABLE [Match]
ADD CONSTRAINT CK_Match_DescriptionScore
CHECK ([DescriptionScore] BETWEEN 0 AND 100);
GO


/* 5. Image Score */

ALTER TABLE [Match]
ADD CONSTRAINT CK_Match_ImageScore
CHECK ([ImageScore] BETWEEN 0 AND 100);
GO


/* 6. Prevent matching a report with itself */

ALTER TABLE [Match]
ADD CONSTRAINT CK_Match_DifferentReports
CHECK ([LostReportId] <> [FoundReportId]);
GO


/* =========================================
   CHECK CONSTRAINTS - GROUP 3
   ========================================= */


ALTER TABLE [Report]
ADD CONSTRAINT CK_Report_ReportType
CHECK ([ReportType] IN ('Lost', 'Found'));
GO

ALTER TABLE [Report]
ADD CONSTRAINT CK_Report_TargetType
CHECK ([TargetType] IN ('Person', 'Item'));
GO

ALTER TABLE [Report]
ADD CONSTRAINT CK_Report_Status
CHECK ([Status] IN ('Pending', 'Active', 'Matched', 'Closed' , 'Rejected'));
GO

/* =========================================
   CHECK CONSTRAINTS - GROUP 4
   ========================================= */

ALTER TABLE [Match]
ADD CONSTRAINT CK_Match_Status
CHECK ([Status] IN ('Pending', 'Accepted', 'Rejected'));
GO

/* =========================================
   CHECK CONSTRAINTS - GROUP 5
   ========================================= */
   /*التحقق من هوية المستخدم و حالة مراجعة حسابه */

   ALTER TABLE [UserVerification]
ADD CONSTRAINT CK_UserVerification_Status
CHECK ([Status] IN ('Pending', 'Approved', 'Rejected'));
GO

/* =========================================
   CHECK CONSTRAINTS - GROUP 6
   ========================================= */
   /*  حالات بلاغات المحتوي المخالف للشروط     */

   ALTER TABLE [ReportFlag]
ADD CONSTRAINT CK_ReportFlag_Status
CHECK ([Status] IN ('Pending', 'Reviewed', 'Resolved', 'Rejected'));
GO


/* =========================================
   CHECK CONSTRAINTS - GROUP 7
   ========================================= */
   /*  notifications type    */
   ALTER TABLE [Notification]
ADD CONSTRAINT CK_Notification_Type
CHECK ([Type] IN (
    'NewMatch',
    'NewMessage',
    'ReportUpdate',
    'VerificationUpdate'
));
GO


ALTER TABLE [PersonDetail]
ADD CONSTRAINT CK_PersonDetail_Gender
CHECK ([Gender] IN ('Male', 'Female', 'Unknown'));
GO

/* =========================================
   CHECK CONSTRAINTS - GROUP 8
   ========================================= */
   /*  prevent a category from being its own parent   */
   ALTER TABLE [ItemCategory]
ADD CONSTRAINT CK_ItemCategory_ParentCategory
CHECK ([ParentCategoryId] <> [Id]);
GO

-- Prevent empty or whitespace-only serial numbers
ALTER TABLE [ItemDetail]
ADD CONSTRAINT CK_ItemDetail_SerialNumber
CHECK (
    SerialNumber IS NULL
    OR LEN(LTRIM(RTRIM(SerialNumber))) > 0
);
GO

-- Prevent empty or whitespace-only category names
ALTER TABLE [ItemCategory]
ADD CONSTRAINT CK_ItemCategory_Name
CHECK (
    [Name] IS NULL
    OR LEN(LTRIM(RTRIM([Name]))) > 0
);
GO


/* =========================================
   CHECK CONSTRAINTS - GROUP 9
   ========================================= */

-- Prevent empty or whitespace-only messages
ALTER TABLE [Message]
ADD CONSTRAINT CK_Message_Content
CHECK (
    [Content] IS NULL
    OR LEN(LTRIM(RTRIM([Content]))) > 0
);
GO

/* =========================================
   CHECK CONSTRAINTS - GROUP 10
   ========================================= */

-- Ensure user name is not empty or whitespace-only
ALTER TABLE [User]
ADD CONSTRAINT CK_User_FullName
CHECK (LEN(LTRIM(RTRIM([FullName]))) > 0);
GO

/* =========================================
   CHECK CONSTRAINTS - GROUP 11
   ========================================= */
   -- Prevent empty or whitespace-only notification messages
ALTER TABLE [Notification]
ADD CONSTRAINT CK_Notification_Message
CHECK (
    [Message] IS NULL
    OR LEN(LTRIM(RTRIM([Message]))) > 0
);
GO


-- Make Report Title and Description required
ALTER TABLE [Report]
ALTER COLUMN [Title] NVARCHAR(200) NOT NULL;
GO

ALTER TABLE [Report]
ALTER COLUMN [Description] NVARCHAR(MAX) NOT NULL;
GO


-- Prevent empty or whitespace-only report descriptions
ALTER TABLE [Report]
ADD CONSTRAINT CK_Report_Description
CHECK (LEN(LTRIM(RTRIM([Description]))) > 0);
GO

/* =========================================
   CHECK CONSTRAINTS - GROUP 12
   ========================================= */
ALTER TABLE [ReportFlag]
ADD CONSTRAINT CK_ReportFlag_Reason
CHECK (LEN(LTRIM(RTRIM([Reason]))) > 0);
GO


ALTER TABLE [User]
ADD CONSTRAINT CK_User_Email
CHECK (LEN(LTRIM(RTRIM([Email]))) > 0);
GO

ALTER TABLE [PersonDetail]
ADD CONSTRAINT CK_PersonDetail2_PersonName
CHECK (LEN(LTRIM(RTRIM([PersonName]))) > 0);
GO