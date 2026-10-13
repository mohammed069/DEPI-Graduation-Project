
CREATE TABLE [USER]
(
    UserID INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Role VARCHAR(20) NOT NULL,
);
 
CREATE TABLE COURSE
(
    CourseID INT PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Description VARCHAR(500),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE QUESTION
(
    QuestionID INT PRIMARY KEY,
    Question_Text VARCHAR(500) NOT NULL,
    Type VARCHAR(10) NOT NULL,
    Marks DECIMAL(5,2) NOT NULL,

    CONSTRAINT CK_QUESTION_Type
        CHECK (Type IN ('MCQ', 'T/F')),

    CONSTRAINT CK_QUESTION_Marks
        CHECK (Marks > 0)
);
CREATE TABLE USER_COURSE_TEACHES
(
    TeachingID INT PRIMARY KEY,
    UserID INT NOT NULL,
    CourseID INT NOT NULL,

    CONSTRAINT FK_Teaches_User
        FOREIGN KEY (UserID) REFERENCES [USER](UserID),

    CONSTRAINT FK_Teaches_Course
        FOREIGN KEY (CourseID) REFERENCES COURSE(CourseID),

    CONSTRAINT UQ_Teaches_User_Course
        UNIQUE (UserID, CourseID)
);

CREATE TABLE USER_COURSE_ENROLLMENT
(
    EnrollmentID INT PRIMARY KEY,
    UserID INT NOT NULL,
    CourseID INT NOT NULL,

    CONSTRAINT FK_Enrollment_User
        FOREIGN KEY (UserID) REFERENCES [USER](UserID),

    CONSTRAINT FK_Enrollment_Course
        FOREIGN KEY (CourseID) REFERENCES COURSE(CourseID),

    CONSTRAINT UQ_Enrollment_User_Course
        UNIQUE (UserID, CourseID)
);
CREATE TABLE QUIZ
(
    QuizID INT PRIMARY KEY,
    Title VARCHAR(150) NOT NULL,
    Start_Time DATETIME2 NOT NULL,
    Duration INT NOT NULL,
    Total_Marks DECIMAL(8,2) NOT NULL,
    CourseID INT NOT NULL,
    CreatedBy INT NOT NULL,

    CONSTRAINT FK_Quiz_Course
        FOREIGN KEY (CourseID) REFERENCES COURSE(CourseID),

    CONSTRAINT FK_Quiz_Creator
        FOREIGN KEY (CreatedBy) REFERENCES [USER](UserID),

    CONSTRAINT CK_Quiz_Duration
        CHECK (Duration > 0),

    CONSTRAINT CK_Quiz_TotalMarks
        CHECK (Total_Marks > 0)
);

CREATE TABLE QUIZ_QUESTIONS
(
    QuizID INT NOT NULL,
    QuestionID INT NOT NULL,

    CONSTRAINT PK_Quiz_Questions
        PRIMARY KEY (QuizID, QuestionID),

    CONSTRAINT FK_QQ_Quiz
        FOREIGN KEY (QuizID) REFERENCES QUIZ(QuizID),

    CONSTRAINT FK_QQ_Question
        FOREIGN KEY (QuestionID) REFERENCES QUESTION(QuestionID)
);
CREATE TABLE OPTIONS
(
    OptionID INT PRIMARY KEY,
    Option_Text VARCHAR(300) NOT NULL,
    IsCorrect BIT NOT NULL DEFAULT 0,
    QuestionID INT NOT NULL,

    CONSTRAINT FK_Options_Question
        FOREIGN KEY (QuestionID) REFERENCES QUESTION(QuestionID),

    CONSTRAINT UQ_Options_Option_Question
        UNIQUE (OptionID, QuestionID)
);
CREATE TABLE QUIZ_ATTEMPT
(
    AttemptID INT PRIMARY KEY,
    StartedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    SubmittedAt DATETIME2 NULL,
    Score DECIMAL(8,2) NOT NULL DEFAULT 0,
    UserID INT NOT NULL,
    QuizID INT NOT NULL,

    CONSTRAINT FK_Attempt_User
        FOREIGN KEY (UserID) REFERENCES [USER](UserID),

    CONSTRAINT FK_Attempt_Quiz
        FOREIGN KEY (QuizID) REFERENCES QUIZ(QuizID),

    CONSTRAINT CK_Attempt_Score
        CHECK (Score >= 0),

    CONSTRAINT CK_Attempt_Submission
        CHECK ( SubmittedAt IS NUll )
);

CREATE TABLE STUDENT_ANSWER
(
    AnswerID INT PRIMARY KEY,
    IsCorrect BIT NULL,
    Marks_Awarded DECIMAL(8,2) NOT NULL DEFAULT 0,
    AttemptID INT NOT NULL,
    QuestionID INT NOT NULL,
    OptionID INT ,

    CONSTRAINT FK_Answer_Attempt
        FOREIGN KEY (AttemptID) REFERENCES QUIZ_ATTEMPT(AttemptID),

    CONSTRAINT FK_Answer_Question
        FOREIGN KEY (QuestionID)  REFERENCES QUESTION(QuestionID),

    CONSTRAINT FK_Answer_Option_Question
        FOREIGN KEY (OptionID, QuestionID) REFERENCES OPTIONS(OptionID, QuestionID),

    CONSTRAINT CK_Answer_Marks CHECK (Marks_Awarded >= 0),

    CONSTRAINT UQ_Answer_Attempt_Question UNIQUE (AttemptID, QuestionID)
);
ALTER TABLE [USER]
ADD CONSTRAINT CK_USER_Role
       CHECK (Role IN ('Admin', 'Teacher', 'Student'))



CREATE INDEX IX_Teaches_CourseID ON USER_COURSE_TEACHES(CourseID);


CREATE INDEX IX_Enrollment_CourseID ON USER_COURSE_ENROLLMENT(CourseID);


CREATE INDEX IX_Quiz_CourseID ON QUIZ(CourseID);


CREATE INDEX IX_Quiz_CreatedBy ON QUIZ(CreatedBy);


CREATE INDEX IX_QuizQuestions_QuestionID ON QUIZ_QUESTIONS(QuestionID);


CREATE INDEX IX_Options_QuestionID
    ON OPTIONS(QuestionID);


CREATE INDEX IX_Attempt_UserID ON QUIZ_ATTEMPT(UserID);


CREATE INDEX IX_Attempt_QuizID
ON QUIZ_ATTEMPT(QuizID);


CREATE INDEX IX_Answer_QuestionID
ON STUDENT_ANSWER(QuestionID);


CREATE INDEX IX_Answer_Option_Question ON STUDENT_ANSWER(OptionID, QuestionID);
