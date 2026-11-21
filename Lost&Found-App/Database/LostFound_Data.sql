-- ==============================
-- Table: USER
-- ==============================

INSERT INTO [User]
    (FullName, Email, PhoneNumber, IsVerified, IsBlocked, IsDeleted, CreatedAt)
VALUES
    ('Naglaa Haitham', 'naglaa.demo@example.com', '01000000001', 1, 0, 0, GETDATE()),
    ('Samia Ashraf', 'samia.demo@example.com', '01000000002', 1, 0, 0, GETDATE()),
    ('Amany Ahmed', 'amany.demo@example.com', '01000000003', 0, 0, 0, GETDATE()),
    ('Omar Tarek', 'omar.demo@example.com', '01000000004', 1, 0, 0, GETDATE()),
    ('Mohammed Elshafey', 'mohammed.demo@example.com', '01000000005', 0, 0, 0, GETDATE());




-- ==============================
-- Table: ItemCategory
-- ==============================

INSERT INTO ItemCategory (Name, ParentCategoryId)
VALUES
    ('Electronics', NULL),
    ('Clothing', NULL),
    ('Personal Belongings', NULL),
    ('Accessories', NULL);


-- ==============================
-- Table: ItemCategory
-- Subcategories
-- ==============================

INSERT INTO ItemCategory (Name, ParentCategoryId)
VALUES
    ('Mobile Phones', 1),
    ('Laptops', 1),
    ('Men Clothing', 2),
    ('Women Clothing', 2),
    ('Bags', 3),
    ('Wallets', 3),
    ('Watches', 4),
    ('Jewelry', 4);


-- ==============================
-- Table: Report
-- ==============================

INSERT INTO Report
(
    UserId,
    ReportType,
    TargetType,
    Title,
    Description,
    EventDate,
    Latitude,
    Longitude,
    Address,
    City,
    Status,
    IsVerified,
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
-- Item Reports
(1, 'Lost', 'Item',
 'Lost Samsung Phone',
 'Black Samsung mobile phone lost near the university.',
 DATEADD(DAY, -5, GETDATE()),
 30.5877, 31.5020,
 'Near the university gate',
 'Zagazig',
 'Active', 1, 0, GETDATE(), GETDATE()),

(2, 'Found', 'Item',
 'Found Samsung Phone',
 'A black Samsung mobile phone was found near the university.',
 DATEADD(DAY, -4, GETDATE()),
 30.5880, 31.5025,
 'Near the university gate',
 'Zagazig',
 'Active', 1, 0, GETDATE(), GETDATE()),

(3, 'Lost', 'Item',
 'Lost Black Backpack',
 'Black backpack containing notebooks and personal belongings.',
 DATEADD(DAY, -3, GETDATE()),
 30.5900, 31.5100,
 'Near the main bus station',
 'Zagazig',
 'Active', 0, 0, GETDATE(), GETDATE()),

(4, 'Found', 'Item',
 'Found Black Backpack',
 'A black backpack was found near the bus station.',
 DATEADD(DAY, -2, GETDATE()),
 30.5905, 31.5105,
 'Near the main bus station',
 'Zagazig',
 'Active', 0, 0, GETDATE(), GETDATE()),

-- Person Reports
(1, 'Lost', 'Person',
 'Missing Person - Ahmed',
 'A young man named Ahmed went missing. Last seen wearing a blue shirt.',
 DATEADD(DAY, -2, GETDATE()),
 30.5870, 31.5000,
 'Near Zagazig University',
 'Zagazig',
 'Active', 1, 0, GETDATE(), GETDATE()),

(3, 'Found', 'Person',
 'Found Person - Ahmed',
 'A young man was found near the university and needs to be reunited with his family.',
 DATEADD(DAY, -1, GETDATE()),
 30.5875, 31.5005,
 'Near Zagazig University',
 'Zagazig',
 'Active', 0, 0, GETDATE(), GETDATE()),

(2, 'Lost', 'Person',
 'Missing Person - Child',
 'A child went missing near a public park. Last seen wearing a red jacket.',
 DATEADD(DAY, -3, GETDATE()),
 30.5800, 31.4900,
 'Near the public park',
 'Zagazig',
 'Active', 1, 0, GETDATE(), GETDATE()),

(5, 'Found', 'Person',
 'Found Child',
 'A child was found near the public park and is waiting for family identification.',
 DATEADD(DAY, -2, GETDATE()),
 30.5805, 31.4905,
 'Near the public park',
 'Zagazig',
 'Active', 0, 0, GETDATE(), GETDATE());



 -- ==============================
-- Table: PersonDetail
-- ==============================

INSERT INTO PersonDetail
(
    ReportId,
    PersonName,
    Age,
    Gender,
    HairColor,
    EyeColor,
    Height,
    ClothingDescription,
    DistinguishingMarks,
    MedicalCondition,
    ContactPhoneForFamily
)
VALUES
(5, 'Ahmed Hassan', 24, 'Male',
 'Black', 'Brown', 175,
 'Blue shirt and black trousers',
 'Small scar above the left eyebrow',
 NULL, '01000000006'),

(6, 'Ahmed Hassan', 24, 'Male',
 'Black', 'Brown', 175,
 'Blue shirt and black trousers',
 'Small scar above the left eyebrow',
 NULL, '01000000006'),

(7, 'Omar Ali', 9, 'Male',
 'Brown', 'Brown', 130,
 'Red jacket and blue jeans',
 'Small birthmark on the right hand',
 NULL, '01000000007'),

(8, 'Omar Ali', 9, 'Male',
 'Brown', 'Brown', 130,
 'Red jacket and blue jeans',
 'Small birthmark on the right hand',
 NULL, '01000000007');




 -- ==============================
-- Table: ItemDetail
-- ==============================

INSERT INTO ItemDetail
(
    ReportId,
    CategoryId,
    Brand,
    Model,
    Color,
    SerialNumber,
    Size
)
VALUES
(1, 5, 'Samsung', 'Galaxy A54', 'Black',
 'SN-DEMO-1001', NULL),

(2, 5, 'Samsung', 'Galaxy A54', 'Black',
 'SN-DEMO-1001', NULL),

(3, 9, NULL, NULL, 'Black',
 NULL, 'Medium'),

(4, 9, NULL, NULL, 'Black',
 NULL, 'Medium');


 -- ==============================
-- Table: ReportImage
-- ==============================

INSERT INTO ReportImage
(
    ReportId,
    ImageUrl,
    IsPrimary,
    FaceEmbedding,
    ImageHash,
    UploadedAt
)
VALUES
(1, 'https://example.com/demo-images/lost-samsung.jpg', 1, NULL, NULL, GETDATE()),
(2, 'https://example.com/demo-images/found-samsung.jpg', 1, NULL, NULL, GETDATE()),
(3, 'https://example.com/demo-images/lost-backpack.jpg', 1, NULL, NULL, GETDATE()),
(4, 'https://example.com/demo-images/found-backpack.jpg', 1, NULL, NULL, GETDATE()),
(5, 'https://example.com/demo-images/missing-ahmed.jpg', 1, NULL, NULL, GETDATE()),
(6, 'https://example.com/demo-images/found-ahmed.jpg', 1, NULL, NULL, GETDATE()),
(7, 'https://example.com/demo-images/missing-child.jpg', 1, NULL, NULL, GETDATE()),
(8, 'https://example.com/demo-images/found-child.jpg', 1, NULL, NULL, GETDATE());




-- ==============================
-- Table: Match
-- ==============================

INSERT INTO [Match]
(
    LostReportId,
    FoundReportId,
    TotalScore,
    LocationScore,
    DateScore,
    DescriptionScore,
    ImageScore,
    Status,
    CreatedAt,
    ConfirmedAt
)
VALUES
(1, 2, 95, 95, 90, 100, 95, 'Accepted', GETDATE(), GETDATE()),

(3, 4, 88, 90, 85, 90, 87, 'Pending', GETDATE(), NULL),

(5, 6, 96, 95, 95, 100, 94, 'Accepted', GETDATE(), GETDATE()),

(7, 8, 75, 80, 70, 75, 75, 'Rejected', GETDATE(), NULL);



-- ==============================
-- Table: Conversation
-- ==============================

INSERT INTO Conversation
(
    MatchId,
    CreatedAt
)
VALUES
(1, GETDATE()),
(3, GETDATE());


-- ==============================
-- Table: Message
-- ==============================

INSERT INTO Message
(
    ConversationId,
    SenderId,
    Content,
    SentAt,
    IsRead
)
VALUES
-- Mobile Phone Conversation
(1, 1, 'Hello, I think you found my Samsung phone. Can you confirm its details?', DATEADD(MINUTE, -30, GETDATE()), 1),
(1, 2, 'Hello! Yes, I found a black Samsung phone near the university gate.', DATEADD(MINUTE, -25, GETDATE()), 1),
(1, 1, 'Thank you! I can provide more details to verify ownership.', DATEADD(MINUTE, -20, GETDATE()), 0),

-- Missing Person Conversation
(2, 1, 'Hello, I saw your found person report. I believe this may be the missing person I reported.', DATEADD(MINUTE, -15, GETDATE()), 1),
(2, 3, 'Hello, please share more details so we can verify the identity safely.', DATEADD(MINUTE, -10, GETDATE()), 1),
(2, 1, 'I will share the identifying details with the family.', DATEADD(MINUTE, -5, GETDATE()), 0);


-- ==============================
-- Table: Notification
-- ==============================

INSERT INTO Notification
(
    UserId,
    Type,
    Message,
    ReferenceType,
    ReferenceId,
    IsRead,
    CreatedAt
)
VALUES
(1, 'NewMatch',
 'A possible match was found for your lost Samsung phone.',
 'Match', 1, 0, GETDATE()),

(2, 'NewMatch',
 'Your found Samsung phone report has a possible match.',
 'Match', 1, 0, GETDATE()),

(3, 'NewMatch',
 'A possible match was found for your missing person report.',
 'Match', 3, 1, GETDATE()),

(1, 'NewMessage',
 'You have received a new message about your lost phone.',
 'Message', 2, 0, GETDATE()),

(2, 'NewMessage',
 'You have received a new message in your conversation.',
 'Message', 1, 1, GETDATE()),

(1, 'ReportUpdate',
 'Your lost Samsung phone report has been updated.',
 'Report', 1, 1, GETDATE()),

(3, 'ReportUpdate',
 'Your missing person report is now active.',
 'Report', 5, 0, GETDATE()),

(4, 'VerificationUpdate',
 'Your account verification has been approved.',
 'UserVerification', 1, 0, GETDATE());


 -- ==============================
-- Table: UserLocationPreference
-- ==============================

INSERT INTO UserLocationPreference
(
    UserId,
    Latitude,
    Longitude,
    RadiusKm,
    NotifyEnabled
)
VALUES
(1, 30.5877, 31.5020, 10, 1),
(2, 30.5880, 31.5025, 15, 1),
(3, 30.5900, 31.5100, 5, 1),
(4, 30.5800, 31.4900, 20, 0),
(5, 30.5850, 31.5050, 10, 1);


-- ==============================
-- Table: UserVerification
-- ==============================

INSERT INTO UserVerification
(
    UserId,
    Method,
    Status,
    DocumentUrl,
    ReviewedByAdminId,
    ReviewedAt
)
VALUES
(1, 'National ID', 'Approved',
 'https://example.com/demo-documents/naglaa-id.jpg',
 2, GETDATE()),

(3, 'National ID', 'Pending',
 'https://example.com/demo-documents/amany-id.jpg',
 NULL, NULL),

(5, 'University ID', 'Rejected',
 'https://example.com/demo-documents/mohammed-id.jpg',
 2, GETDATE());


 -- ==============================
-- Table: ReportFlag
-- ==============================

INSERT INTO ReportFlag
(
    ReportId,
    ReportedByUserId,
    Reason,
    Status,
    ReviewedByAdminId,
    ReviewedAt
)
VALUES
(1, 3,
 'The report may contain inaccurate information.',
 'Pending', NULL, NULL),

(3, 4,
 'The uploaded report image does not match the item description.',
 'Reviewed', 2, GETDATE()),

(7, 5,
 'This report may be a duplicate of another missing person report.',
 'Resolved', 2, GETDATE());










 SELECT
    (SELECT COUNT(*) FROM [User]) AS UsersCount,
    (SELECT COUNT(*) FROM ItemCategory) AS CategoriesCount,
    (SELECT COUNT(*) FROM Report) AS ReportsCount,
    (SELECT COUNT(*) FROM PersonDetail) AS PersonDetailsCount,
    (SELECT COUNT(*) FROM ItemDetail) AS ItemDetailsCount,
    (SELECT COUNT(*) FROM ReportImage) AS ImagesCount,
    (SELECT COUNT(*) FROM [Match]) AS MatchesCount,
    (SELECT COUNT(*) FROM Conversation) AS ConversationsCount,
    (SELECT COUNT(*) FROM Message) AS MessagesCount,
    (SELECT COUNT(*) FROM Notification) AS NotificationsCount,
    (SELECT COUNT(*) FROM UserLocationPreference) AS LocationPreferencesCount,
    (SELECT COUNT(*) FROM UserVerification) AS VerificationsCount,
    (SELECT COUNT(*) FROM ReportFlag) AS FlagsCount;